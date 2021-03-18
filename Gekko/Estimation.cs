using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Gekko
{
    public static class Estimation
    {
        public static void Ols(O.Ols o)
        {
            // ------------------------------------------
            //n = number of obs
            //m = number of params (including trends and constant, so poly and constant are part of this)
            //k = number of restrictions (including Finnish trend restrictions)
            //constant = 0 or 1
            //poly = degree of trend poly
            // ------------------------------------------

            //AREMOS:
            //close*;clear;
            //import<tsd mute>maj11_pcim;
            //set per 50 2079
            //set per 80 2010;
            //equ lna1 = pcp, pcp.1, ul, ul.1;

            // LNA1
            // Ordinary Least Squares
            // ANNUAL data for   31 periods from 1980 to 2010
            // Date: 14 NOV 2013

            // lna1

            //   =    418.171 * pcp - 154.400 * pcp[-1] + 0.00154 * ul
            //       (2.96194)       (1.15821)           (0.03103)

            //      - 0.12668 * ul[-1] - 59.8149
            //       (2.58716)          (3.64875)

            // Sum Sq    1148.51   Std Err    6.6463   LHS Mean  154.294
            // R Sq       0.9876   R Bar Sq   0.9856   F  4, 26  515.654
            // D.W.( 1)   0.4673   D.W.( 2)   0.9352

            //Denne stemmer med Gekko.

            //What about: http://christoph.ruegg.name/blog/linear-regression-mathnet-numerics.html ?
            //Also see: http://christoph.ruegg.name/blog/towards-mathnet-numerics-v3.html

            IVariable lhs = o.expressions[0];
            List<IVariable> rhs = new List<IVariable>();
            for (int i = 1; i < o.expressions.Count; i++) rhs.Add(o.expressions[i]);
            List<Series> rhs_unfolded = Program.UnfoldAsSeries(new GekkoSmpl(o.t1, o.t2), rhs);

            List<int> flatStart = new List<int>();
            List<int> flatEnd = new List<int>();
            int constant = 1; if (G.Equal(o.opt_constant, "no")) constant = 0;
            int poly;  //0 if there is none, else > 1. Cf. also constant.
            OLSGetTrendParameters(O.Restrict(o.opt_xtrend, false, false, false, false), O.Restrict(o.opt_xflat, false, false, false, false), flatStart, flatEnd, out poly);
            int n = GekkoTime.Observations(o.t1, o.t2);
            int m = rhs_unfolded.Count + poly + constant; //explanatory vars including poly and constant = number of params estimated INCLUDING possible poly and constant     
            List<int> trendparams = new List<int>();
            if (poly > 0)
            {
                for (int i = m - poly - constant; i < m - constant; i++)
                {
                    trendparams.Add(i);
                }
            }

            Series lhs_series = lhs as Series;
            if (lhs_series == null)
            {
                new Error("Left-hand side should be a SERIES");
                //throw new GekkoException();
            }

            //bool useScale = false; //usually true

            string name = o.name;
            if (name == null) name = "ols";

            double[,] tsData2 = null;
            int m2 = 0;

            double[,] x = new double[n, m];  //includes poly and constant if it is there, does not include lhs
            double[,] xOriginal = new double[n, m];  //includes poly and constant if it is there, does not include lhs
            double[] y = new double[n];

            OLSPackData(rhs_unfolded, constant, poly, lhs_series, o.t1, o.t2, n, x, xOriginal, y);

            Matrix name_param = new Matrix(m, 1, double.NaN);
            Matrix name_t = new Matrix(m, 1, double.NaN);
            Matrix name_se = new Matrix(m, 1, double.NaN);
            Matrix name_stats = new Matrix(9, 1, double.NaN);
            Matrix name_covar = new Matrix(m, m, double.NaN);
            Matrix name_corr = new Matrix(m, m, double.NaN);
            Series name_predict = new Series(o.t1.freq, G.Chop_AddFreq(name + "_predict", lhs_series.freq));
            Series name_residual = new Series(o.t1.freq, G.Chop_AddFreq(name + "_residual", lhs_series.freq));

            double[] scaling = new double[x.GetLength(1)];
            for (int kk = 0; kk < x.GetLength(1); kk++)
            {
                double sum = 0d;
                for (int tt = 0; tt < x.GetLength(0); tt++)
                {
                    sum += Math.Abs(x[tt, kk]);  //with abs we avoid stupid averages = 0 like -2, -1, 0, 1, 2 etc.
                }
                scaling[kk] = sum / x.GetLength(0);
                if (scaling[kk] == 0d) scaling[kk] = 1d;

                for (int tt = 0; tt < x.GetLength(0); tt++)
                {
                    x[tt, kk] = x[tt, kk] / scaling[kk];
                }
            }

            double[,] restrict_input = new double[0, m + 1];  //array[k, m+1]  c[i,0]*beta[0] + ... + c[i,m-1]*beta[m-1] = c[i,m]            

            int k = 0;

            if (o.impose != null)
            {
                Matrix rr = O.ConvertToMatrix(o.impose); //a row for each of the k restriction, and it has m+1 cols (second-last col is const if present, and last is what the restrict sums up to)
                k = rr.data.GetLength(0);
                int cols = rr.data.GetLength(1);

                if (cols != m + 1)
                {
                    new Error("The impose matrix has " + cols + " cols, expected " + (m + 1));
                    //throw new GekkoException();
                }

                restrict_input = new double[rr.data.GetLength(0), rr.data.GetLength(1)];  //needs to be cloned, otherwise the IMPOSE matrix will be changed with scaling
                for (int i = 0; i < rr.data.GetLength(0); i++)
                {
                    for (int j = 0; j < rr.data.GetLength(1) - 1; j++)
                    {
                        restrict_input[i, j] = rr.data[i, j] / scaling[j];
                    }
                    restrict_input[i, rr.data.GetLength(1) - 1] = rr.data[i, rr.data.GetLength(1) - 1];
                }
            }

            double[,] restrict = new double[0, m];
            if (flatStart.Count + flatEnd.Count > 0)
            {
                //note: this changes k (number of restrictions, where the trend restrictions are added)
                double vStart = x[0, trendparams[0]] * scaling[trendparams[0]];
                double vEnd = x[x.GetLength(0) - 1, trendparams[0]] * scaling[trendparams[0]];
                restrict = OLSFinnishTrends(flatStart, flatEnd, trendparams, scaling, restrict_input, vStart, vEnd);
                k += flatStart.Count + flatEnd.Count;
            }
            else
            {
                restrict = restrict_input;
            }

            int df = n - m + k; //degrees of freedom: number of obs - estimated coeffs (including const) + impose restrictions

            if (df <= 0)
            {
                string s = null;
                if (constant == 1) s = "(including constant) ";
                new Error("There are " + m + " params " + s + "and " + k + " restrictions with only " + n + " observations");
                //throw new GekkoException();
            }

            OLSRekurInfo rekurInfo = new OLSRekurInfo();
            rekurInfo.type = "r";

            Program.DeleteGekkoActions(EGekkoActionTypes.Ols, name);

            //!!calling the engine ------------------------------------------------
            //!!calling the engine ------------------------------------------------
            //!!calling the engine ------------------------------------------------
            OLSResults ols = OLSHelper(o.t1, o.t2, y, x, xOriginal, restrict, scaling, n, m, k, df, false);
            //---------------------------------------------------------------------
            //---------------------------------------------------------------------
            //---------------------------------------------------------------------

            //Decomp
            if (true)
            {
                double[] contriby = new double[n];
                double[,] contribx = new double[m, n];
                double[,] contribx_raw = new double[m, n];
                double[] contribtrend = new double[n];
                double[] contribtrend_raw = new double[n];
                double[,] levelx = new double[m, n];
                double[] levely = new double[n];
                double[] sumx = new double[m];
                double sumy = 0d;
                for (int t = 0; t < n; t++) //time
                {
                    double sum = 0d;
                    for (int i = 0; i < m; i++) //variable
                    {
                        contribx[i, t] = ols.coeff[i] * xOriginal[t, i];
                        contribx_raw[i, t] = ols.coeff[i] * xOriginal[t, i];
                    }
                }

                for (int t = 0; t < n; t++) //time
                {
                    for (int i = 0; i < m; i++) //variable
                    {
                        sumx[i] += contribx[i, t];
                    }
                    sumy += ols.ypredict[t];
                }

                for (int t = 0; t < n; t++) //time
                {
                    for (int i = 0; i < m; i++) //variable
                    {
                        contribx[i, t] -= sumx[i] / (double)n;
                    }
                    foreach (int i in trendparams)
                    {
                        contribtrend[t] += contribx[i, t];
                        contribtrend_raw[t] += contribx_raw[i, t];  //used for ols_trend variable
                    }
                    contriby[t] = ols.ypredict[t] - sumy / (double)n;
                }

                for (int i = 0; i < m; i++) //vars
                {
                    string nameWithFreq = G.Chop_AddFreq(name + "_dec" + (i + 1), G.ConvertFreq(lhs_series.freq));
                    Series z = new Series(lhs_series.freq, nameWithFreq);
                    for (int t = 0; t < n; t++) //time
                    {
                        z.SetData(o.t1.Add(t), contribx[i, t]);
                    }
                    Program.databanks.GetFirst().AddIVariableWithOverwrite(nameWithFreq, z);
                }

                if (true)
                {
                    string nameWithFreq = G.Chop_AddFreq(name + "_dec", G.ConvertFreq(lhs_series.freq));
                    Series z = new Series(lhs_series.freq, nameWithFreq);
                    for (int t = 0; t < n; t++) //time
                    {
                        z.SetData(o.t1.Add(t), contriby[t]);
                    }
                    Program.databanks.GetFirst().AddIVariableWithOverwrite(nameWithFreq, z);
                    if (poly > 0)
                    {
                        string nameWithFreq2 = G.Chop_AddFreq(name + "_dec_trend", G.ConvertFreq(lhs_series.freq));
                        Series z2 = new Series(lhs_series.freq, nameWithFreq2);
                        for (int t = 0; t < n; t++) //time
                        {
                            z2.SetData(o.t1.Add(t), contribtrend[t]);
                        }
                        Program.databanks.GetFirst().AddIVariableWithOverwrite(nameWithFreq2, z2);

                        string nameWithFreq3 = G.Chop_AddFreq(name + "_trend", G.ConvertFreq(lhs_series.freq));
                        Series z3 = new Series(lhs_series.freq, nameWithFreq3);
                        for (int t = 0; t < n; t++) //time
                        {
                            z3.SetData(o.t1.Add(t), contribtrend_raw[t]);
                        }
                        Program.databanks.GetFirst().AddIVariableWithOverwrite(nameWithFreq3, z3);
                    }
                }

            }

            //Recursive estimation
            //Recursive estimation
            //Recursive estimation

            if (OLSRecursiveDfOk(df))
            {
                if (rekurInfo.type != null)
                {
                    foreach (string type in new List<string>() { "l", "e", "r" })
                    {
                        OLSRekurDatas rekur = OLSRecursive(m, x, y, scaling, k, n, restrict_input, df, Program.options.fit_ols_rekur_dfmin, type, trendparams, flatStart, flatEnd);
                        EFreq freq = lhs_series.freq;
                        string type2 = "left";
                        if (type == "e") type2 = "slide";
                        else if (type == "r") type2 = "right";
                        for (int i = 0; i < m; i++)
                        {
                            {
                                string nameWithFreq = G.Chop_AddFreq(name + "_v" + type2 + (i + 1) + "_low", G.ConvertFreq(freq));
                                Series z = new Series(freq, nameWithFreq);
                                if (type == "l")
                                    z.SetDataSequence(o.t1, o.t1.Add(df - Program.options.fit_ols_rekur_dfmin), rekur.datas[i].coeff_low);
                                else
                                    z.SetDataSequence(o.t2.Add(-df + Program.options.fit_ols_rekur_dfmin), o.t2, rekur.datas[i].coeff_low);
                                Program.databanks.GetFirst().AddIVariableWithOverwrite(nameWithFreq, z);
                            }

                            {
                                string nameWithFreq = G.Chop_AddFreq(name + "_v" + type2 + (i + 1), G.ConvertFreq(freq));
                                Series z = new Series(freq, nameWithFreq);
                                if (type == "l")
                                    z.SetDataSequence(o.t1, o.t1.Add(df - Program.options.fit_ols_rekur_dfmin), rekur.datas[i].coeff);
                                else
                                    z.SetDataSequence(o.t2.Add(-df + Program.options.fit_ols_rekur_dfmin), o.t2, rekur.datas[i].coeff);
                                Program.databanks.GetFirst().AddIVariableWithOverwrite(nameWithFreq, z);
                            }

                            {
                                string nameWithFreq = G.Chop_AddFreq(name + "_v" + type2 + (i + 1) + "_high", G.ConvertFreq(freq));
                                Series z = new Series(freq, nameWithFreq);
                                if (type == "l")
                                    z.SetDataSequence(o.t1, o.t1.Add(df - Program.options.fit_ols_rekur_dfmin), rekur.datas[i].coeff_high);
                                else
                                    z.SetDataSequence(o.t2.Add(-df + Program.options.fit_ols_rekur_dfmin), o.t2, rekur.datas[i].coeff_high);
                                Program.databanks.GetFirst().AddIVariableWithOverwrite(nameWithFreq, z);
                            }
                        }

                        {
                            string nameWithFreq = G.Chop_AddFreq(name + "_chow" + "_" + type2, G.ConvertFreq(freq));
                            Series z = new Series(freq, nameWithFreq);
                            if (type == "l")
                                z.SetDataSequence(o.t1, o.t1.Add(df - Program.options.fit_ols_rekur_dfmin), rekur.data.coeff);
                            else
                                z.SetDataSequence(o.t2.Add(-df + Program.options.fit_ols_rekur_dfmin), o.t2, rekur.data.coeff);
                            Program.databanks.GetFirst().AddIVariableWithOverwrite(nameWithFreq, z);
                        }
                    }
                }
            }
            else
            {
                G.Writeln2("Recursive estimation skipped because df is too low");
            }

            for (int j = 0; j < n; j++)
            {
                name_predict.SetData(o.t1.Add(j), ols.ypredict[j]);
                name_residual.SetData(o.t1.Add(j), ols.residual[j]);
            }

            Table tab = new Table();

            tab.Set(1, 1, "Variable");
            tab.Set(1, 2, "Estimate");
            tab.Set(1, 3, "Std error");
            tab.Set(1, 4, "T-stat");
            tab.Set(1, 5, "     ");  //to create some space between main table and links

            if (OLSRecursiveDfOk(df))
            {
                tab.Set(1, 6, "Recursive"); tab.SetAlign(1, 6, Align.Center);
            }

            tab.Merge(1, 6, 1, 8);
            tab.SetAlign(1, 1, 1, 1, Align.Left);
            tab.SetAlign(1, 2, 1, 4, Align.Right);
            for (int i = 0; i < m; i++)
            {
                string s = null;
                if (i + 1 < o.expressionsText.Count)
                {
                    s = Program.TruncateTextWithDots(25, G.ReplaceGlueSymbols(o.expressionsText[i + 1]));
                }
                else
                {
                    if (trendparams.Contains(i))
                    {
                        for (int ii = 0; ii < trendparams.Count; ii++)
                        {
                            if (trendparams[ii] == i)
                            {
                                s = "TREND" + (ii + 1);
                            }
                        }
                    }
                    else
                    {
                        s = "CONSTANT";
                    }
                }
                tab.Set(i + 2, 1, s);
                tab.SetAlign(i + 2, 1, Align.Left);

                if (Math.Abs(ols.coeff[i]) > 1e12 || Math.Abs(ols.coeff[i]) < 1e-12)
                {
                    tab.SetNumber(i + 2, 2, ols.coeff[i], "s16.8");
                    tab.SetNumber(i + 2, 3, ols.se[i], "s16.8");
                    tab.SetNumber(i + 2, 4, ols.t[i], "f12.2");
                }
                else
                {
                    int digits = Program.GetDigits(ols.coeff[i], 6);
                    tab.SetNumber(i + 2, 2, ols.coeff[i], "f16." + digits);
                    tab.SetNumber(i + 2, 3, ols.se[i], "f16." + digits);
                    tab.SetNumber(i + 2, 4, ols.t[i], "f12.2");
                }

                if (OLSRecursiveDfOk(df))
                {
                    // ---------
                    int ii = i;  //because of closure, else i is wrong, since it is a loop variable                    
                    Action a = () =>
                    {
                        Program.RunGekkoCommands("plot <" + o.t1.ToString() + " " + o.t2.ToString() + "> " + name + "_vleft" + (ii + 1) + "_low '' <type=lines linecolor='gray'>, " + name + "_vleft" + (ii + 1) + " <linecolor='red'>, " + name + "_vleft" + (ii + 1) + "_high '' <type=lines linecolor='gray'>;", "", 0, new P());
                    };
                    tab.Set(i + 2, 6, G.GetLinkAction("Left", new GekkoAction(EGekkoActionTypes.Ols, name, a)));
                    // ---------
                }

                if (OLSRecursiveDfOk(df))
                {
                    // ---------
                    int ii = i;  //because of closure, else i is wrong, since it is a loop variable

                    Action a = () =>
                    {
                        Program.RunGekkoCommands("plot <" + o.t1.ToString() + " " + o.t2.ToString() + "> " + name + "_vslide" + (ii + 1) + "_low '' <type=lines linecolor='gray'>, " + name + "_vslide" + (ii + 1) + " <linecolor='red'>, " + name + "_vslide" + (ii + 1) + "_high '' <type=lines linecolor='gray'>;", "", 0, new P());
                    };
                    tab.Set(i + 2, 7, G.GetLinkAction("Slide", new GekkoAction(EGekkoActionTypes.Ols, name, a)));
                    // ---------
                }

                if (OLSRecursiveDfOk(df))
                {
                    // ---------
                    int ii = i;  //because of closure, else i is wrong, since it is a loop variable                    
                    Action a = () =>
                    {
                        Program.RunGekkoCommands("plot <" + o.t1.ToString() + " " + o.t2.ToString() + "> " + name + "_vright" + (ii + 1) + "_low '' <type=lines linecolor='gray'>, " + name + "_vright" + (ii + 1) + " <linecolor='red'>, " + name + "_vright" + (ii + 1) + "_high '' <type=lines linecolor='gray'>;", "", 0, new P());
                    };
                    tab.Set(i + 2, 8, G.GetLinkAction("Right", new GekkoAction(EGekkoActionTypes.Ols, name, a)));
                    // ---------
                }

                name_param.data[i, 0] = ols.coeff[i];
                name_se.data[i, 0] = ols.se[i];
                name_t.data[i, 0] = ols.t[i];
            }

            tab.SetBorder(1, 1, 1, 4, BorderType.Top);
            tab.SetBorder(1, 1, 1, 4, BorderType.Bottom);
            tab.SetBorder(m + 1, 1, m + 1, 4, BorderType.Bottom);

            string line = null;
            if (true)
            {
                Action a = () =>
                {
                    Program.RunGekkoCommands("plot <" + o.t1.ToString() + " " + o.t2.ToString() + " separate> " + name + "_predict+" + name + "_residual 'Obs' <linewidth = 6>, " + name + "_predict 'Fit', " + name + "_residual 'Res' <type=boxes>;", "", 0, new P());
                };
                line += "  " + G.GetLinkAction("Fit", new GekkoAction(EGekkoActionTypes.Ols, name, a));

            }

            if (true)
            {

                Action a = () =>
                {
                    string s = null;

                    if (poly == 0)
                    {
                        for (int i = 0; i < m - constant; i++)
                        {
                            string label = o.expressionsText[i + 1];
                            s += ", " + name + "_dec" + (i + 1) + "'" + label + "'";
                        }
                        Program.RunGekkoCommands("plot <" + o.t1.ToString() + " " + o.t2.ToString() + "> " + name + "_dec '" + o.expressionsText[0] + "' <linewidth = 6>" + s + ";", "", 0, new P());
                    }
                    else
                    {
                        //poly > 0
                        for (int i = 0; i < m - poly - constant; i++)
                        {
                            string label = o.expressionsText[i + 1];
                            s += ", " + name + "_dec" + (i + 1) + "'" + label + "'";
                        }
                        s += ", " + name + "_dec_trend 'trend'";
                        Program.RunGekkoCommands("plot <" + o.t1.ToString() + " " + o.t2.ToString() + "> " + name + "_dec '" + o.expressionsText[0] + "' <linewidth = 6>" + s + ";", "", 0, new P());
                    }
                };
                line += "  " + G.GetLinkAction("Dec", new GekkoAction(EGekkoActionTypes.Ols, name, a));
            }

            tab.Set(m + 2, 1, OLSFormatHelper(ols)); tab.SetAlign(m + 2, 1, Align.Left); tab.Merge(m + 2, 1, m + 2, 3);
            tab.Set(m + 2, 4, line); tab.SetAlign(m + 2, 4, Align.Right);

            if (OLSRecursiveDfOk(df))
            {
                // ---------                
                Action a = () =>
                {
                    Program.RunGekkoCommands("plot <" + o.t1.ToString() + " " + o.t2.ToString() + " yline=1> " + name + "_chow_left 'Chow-test (left)' <type=boxes>;", "", 0, new P());
                };
                tab.Set(m + 2, 6, G.GetLinkAction("Chow", new GekkoAction(EGekkoActionTypes.Ols, name, a)));
                // ---------
            }
            if (OLSRecursiveDfOk(df))
            {
                // ---------                
                Action a = () =>
                {
                    Program.RunGekkoCommands("plot <" + o.t1.ToString() + " " + o.t2.ToString() + " yline=1> " + name + "_chow_right 'Chow-test (right)' <type=boxes>;", "", 0, new P());
                };
                tab.Set(m + 2, 8, G.GetLinkAction("Chow", new GekkoAction(EGekkoActionTypes.Ols, name, a)));
                // ---------
            }

            List<string> temp = tab.Print();
            Globals.lastPrtOrMulprtTable = tab;
            CrossThreadStuff.CopyButtonEnabled(true);

            int widthRemember = Program.options.print_width;
            Program.options.print_width = int.MaxValue;

            string flat = null;
            if (poly > 0) flat = ", poly = " + poly;
            if (flatStart.Count + flatEnd.Count > 0)
            {
                flat += ", polydf = " + (poly - (flatStart.Count + flatEnd.Count));
            }

            G.Writeln2(" " + Globals.ols1 + " " + o.t1 + "-" + o.t2 + ", " + n + " obs, " + m + " params, " + k + " restrictions (df = " + df + ")" + flat);
            G.Writeln(" " + Globals.ols2 + "" + G.ReplaceGlueSymbols(o.expressionsText[0])); //labels contain the LHS and all the RHS!       
            foreach (string s in temp) G.Writeln(s);


            if (Math.Abs(ols.resMean) > 0.000001d * ols.see)
            {
                G.Writeln2("+++ NOTE: The residuals do not seem to sum to zero. Did you omit a constant term?");
                G.Writeln("          Note that R2 and other statistics may be misleading in this case.");
            }

            name_stats.data[1 - 1, 0] = ols.rss;
            name_stats.data[2 - 1, 0] = ols.see;
            name_stats.data[3 - 1, 0] = ols.resMean;
            name_stats.data[4 - 1, 0] = ols.rmse; // rep.rmserror;
            name_stats.data[5 - 1, 0] = ols.r2;
            name_stats.data[6 - 1, 0] = ols.r2cor;
            name_stats.data[8 - 1, 0] = ols.lhsMean;
            name_stats.data[9 - 1, 0] = ols.dw;
            name_covar.data = ols.usedCovar;
            name_corr.data = ols.usedCorr;

            Program.databanks.GetFirst().AddIVariableWithOverwrite(name_predict);
            Program.databanks.GetFirst().AddIVariableWithOverwrite(name_residual);
            Program.databanks.GetFirst().AddIVariableWithOverwrite(Globals.symbolCollection + name + "_stats", name_stats);
            Program.databanks.GetFirst().AddIVariableWithOverwrite(Globals.symbolCollection + name + "_param", name_param);
            Program.databanks.GetFirst().AddIVariableWithOverwrite(Globals.symbolCollection + name + "_t", name_t);
            Program.databanks.GetFirst().AddIVariableWithOverwrite(Globals.symbolCollection + name + "_se", name_se);
            Program.databanks.GetFirst().AddIVariableWithOverwrite(Globals.symbolCollection + name + "_covar", name_covar);
            Program.databanks.GetFirst().AddIVariableWithOverwrite(Globals.symbolCollection + name + "_corr", name_corr);


            Program.options.print_width = widthRemember;

            if (o.opt_dump != null)
            {
                string fileName = "ols.frm";
                if (!G.Equal(o.opt_dump, "yes")) fileName = o.opt_dump;

                bool append = false;
                if (G.Equal(o.opt_dumpoptions, "append")) append = true;

                try
                {
                    string s = null;
                    s += "FRML _i ";
                    s += G.ReplaceGlueSymbols(o.expressionsText[0]) + " = ";
                    bool hasVariable = false;
                    for (int i = 0; i < m - poly - constant; i++)
                    {
                        hasVariable = true;
                        if (i > 0)
                        {
                            if (name_param.data[i, 0] >= 0) s += " +";
                            else s += " ";
                        }
                        string var = G.ReplaceGlueSymbols(o.expressionsText[i + 1]);
                        var = var.Replace(" ", "");
                        if (var.Contains("+") || var.Contains("-")) var = "(" + var + ")";
                        s += Program.NumberFormat(name_param.data[i, 0], "f" + Program.GetDigits(name_param.data[i, 0], 6)) + "*" + var;
                    }
                    if (poly > 0)
                    {
                        if (hasVariable) s += " + ";
                        s += name + "_trend ";
                    }
                    if (constant == 1)
                    {
                        int ii = m - 1;
                        if (name_param.data[ii, 0] >= 0) s += " +";
                        else s += " ";
                        s += Program.NumberFormat(name_param.data[ii, 0], "f" + Program.GetDigits(name_param.data[ii, 0], 6));
                    }
                    s += ";";

                    Program.GekkoFileReadOrWrite option = Program.GekkoFileReadOrWrite.Write;
                    if (append) option = Program.GekkoFileReadOrWrite.WriteAppend;

                    using (FileStream fs = Program.WaitForFileStream(Program.CreateFullPathAndFileName(fileName), option))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        sw.WriteLine(s);
                    }
                    G.Writeln2("Dumped OLS result as an equation inside the file '" + fileName + "'");
                }
                catch
                {
                    new Error("OLS<dump> failed: is the file '" + fileName + "' blocked?");
                    //throw new GekkoException();
                }
            }
        }

        private static bool OLSRecursiveDfOk(int df)
        {
            return Program.options.fit_ols_rekur_dfmin < df;
        }

        private static double[,] OLSFinnishTrends(List<int> flatStart, List<int> flatEnd, List<int> trendparams, double[] scaling, double[,] restrict_original, double vStart, double vEnd)
        {
            int extra = flatStart.Count + flatEnd.Count;

            double[,] restrict_rv = new double[restrict_original.GetLength(0) + extra, restrict_original.GetLength(1)];

            for (int i = 0; i < restrict_original.GetLength(0); i++)
            {
                for (int j = 0; j < restrict_original.GetLength(1); j++)
                {
                    restrict_rv[i, j] = restrict_original[i, j];
                }
            }

            int counter2 = -1;

            foreach (int d in flatStart)
            {
                counter2++;
                int j = restrict_original.GetLength(0) + counter2;
                OLSHelper2(trendparams, d, "s");
                OLSFinnishTrends2(trendparams, scaling, restrict_rv, j, d, vStart);
            }

            foreach (int d in flatEnd)
            {
                counter2++;
                int j = restrict_original.GetLength(0) + counter2;
                OLSHelper2(trendparams, d, "e");
                OLSFinnishTrends2(trendparams, scaling, restrict_rv, j, d, vEnd);
            }

            return restrict_rv;
        }

        private static void OLSFinnishTrends2(List<int> trendparams, double[] scaling, double[,] restrict_rv, int j, int d, double v)
        {
            int counter = -1;
            int counterNonZero = -1;
            int degree = trendparams.Count;
            foreach (int i in trendparams)
            {
                counter++;
                double factor = 1d;
                for (int d2 = counter + 2 - d; d2 < counter + 2; d2++)
                {
                    factor *= (double)d2;
                }
                if (factor != 0d)
                {
                    counterNonZero++;
                    factor *= Math.Pow(v, counterNonZero);
                    restrict_rv[j, i] = factor / scaling[i];
                }
            }
        }

        private static void OLSHelper2(List<int> trendparams, int d, string s)
        {
            if (d > trendparams.Count)
            {
                new Error("" + s + "... parameter must be <= " + (trendparams.Count) + " (poly degree)");
                //throw new GekkoException();
            }
        }

        private static void OLSPackData(List<Series> rhs_unfolded, int constant, int poly, Series lhs_series, GekkoTime t1, GekkoTime t2, int n, double[,] x, double[,] xOriginal, double[] y)
        {
            int i = 0;
            foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
            {
                int j = 0;
                foreach (IVariable xx in rhs_unfolded)
                {
                    x[i, j] = xx.GetVal(t);
                    xOriginal[i, j] = x[i, j];
                    j++;
                }
                for (int p = 1; p <= poly; p++)
                {
                    //x[i, j] = Math.Pow(i - n + 1, p);
                    x[i, j] = Math.Pow((double)(i - n + 1) / (double)(n - 1), p);
                    xOriginal[i, j] = x[i, j];
                    j++;
                }
                if (constant == 1)
                {
                    x[i, j] = 1d;
                    xOriginal[i, j] = x[i, j];  //just for symmetry
                    j++;  //just for symmetry
                }
                y[i] = lhs_series.GetVal(t);
                i++;
            }
        }

        private static void OLSGetTrendParameters(List<string> xtrend, List<string> xtrendflat, List<int> flatStart, List<int> flatEnd, out int polydf)
        {
            if (xtrendflat != null)
            {
                foreach (string s in xtrendflat)
                {
                    if (s.StartsWith("s", StringComparison.Ordinal))
                    {
                        string s2 = s.Substring("s".Length);
                        flatStart.Add(G.ConvertToInt(Functions.HelperValConvertFromString(s2)));
                    }
                    else if (s.StartsWith("e", StringComparison.Ordinal))
                    {
                        string s2 = s.Substring("e".Length);
                        flatEnd.Add(G.ConvertToInt(Functions.HelperValConvertFromString(s2)));
                    }
                    else
                    {
                        new Error("xflat: syntax error");
                        //throw new GekkoException();
                    }
                }
            }

            polydf = 0;
            if (xtrend != null)
            {
                //xtrend = 0 will just be ignored (will not become a constant if no constant present)
                if (xtrend.Count > 1)
                {
                    new Error("xtrend: only 1 element is allowed");
                    //throw new GekkoException();
                }
                polydf = G.ConvertToInt(Functions.HelperValConvertFromString(xtrend[0]));
                if ((polydf < 0))
                {
                    new Error("xtrend: polynomium cannot be negative");
                    //throw new GekkoException();
                }
            }

        }

        private static OLSRekurDatas OLSRecursive(int m, double[,] x, double[] y, double[] scaling, int k, int n, double[,] restrict_input, int df_original, int df_start, string type, List<int> trendparams, List<int> flatStart, List<int> flatEnd)
        {
            if (df_start < 1)
            {
                new Error("minimum degrees of freedom must be > 0");
                //throw new GekkoException();
            }

            OLSRekurDatas rekur = new OLSRekurDatas(m, df_original - df_start + 1);  //if df_start == 1, this corresponds to index 0 in arrays

            double rss0 = double.NaN;
            for (int df7 = df_start; df7 <= df_original; df7++)
            {
                int n7; double[] y7; double[,] x7;
                if (type == "r")
                {
                    OLSRecursiveHelperR(m, x, y, k, df7, 0, out n7, out y7, out x7);
                }
                else if (type == "e")
                {
                    OLSRecursiveHelperR(m, x, y, k, df7, df7 - df_start, out n7, out y7, out x7);
                }
                else  //"l"
                {
                    OLSRecursiveHelperL(m, x, y, k, n, df7, out n7, out y7, out x7);
                }

                //Here, we need to adjust the restrict matrix, if there are Finnish trends

                double[,] restrict7 = restrict_input;

                if (trendparams.Count > 0)
                {
                    double vStart7 = x7[0, trendparams[0]] * scaling[trendparams[0]];
                    double vEnd7 = x7[x7.GetLength(0) - 1, trendparams[0]] * scaling[trendparams[0]];
                    restrict7 = OLSFinnishTrends(flatStart, flatEnd, trendparams, scaling, restrict_input, vStart7, vEnd7);
                }

                bool fail = false;
                OLSResults ols7 = null;
                try
                {
                    ols7 = OLSHelper(GekkoTime.tNull, GekkoTime.tNull, y7, x7, null, restrict7, scaling, n7, m, k, df7, true);
                }
                catch
                {
                    fail = true;
                }

                int index = df7 - df_start;
                if (type == "l") index = rekur.datas[0].coeff.Length - (df7 - df_start) - 1;

                // ---------> df_original - df_start + 1

                if (fail)
                {
                    for (int i = 0; i < m; i++)
                    {
                        rekur.datas[i].coeff_low[index] = double.NaN;
                        rekur.datas[i].coeff[index] = double.NaN;
                        rekur.datas[i].coeff_high[index] = double.NaN;
                    }
                    rekur.data.coeff[index] = double.NaN;
                }
                else
                {
                    for (int i = 0; i < m; i++)
                    {
                        rekur.datas[i].coeff_low[index] = ols7.coeff[i] - 2d * ols7.se[i];
                        rekur.datas[i].coeff[index] = ols7.coeff[i];
                        rekur.datas[i].coeff_high[index] = ols7.coeff[i] + 2d * ols7.se[i];
                    }
                    double t = alglib.studenttdistr.invstudenttdistribution(df7, 0.975);  // limit for df --> inf = 1.960. The AREMOS version subtracts 1, maybe this is an error?
                    double chow = (ols7.rss - rss0) / rss0 * df7 / (t * t);
                    rekur.data.coeff[index] = chow;
                    rss0 = ols7.rss;
                }
            }

            return rekur;

        }

        private static void OLSRecursiveHelperR(int m, double[,] x, double[] y, int k, int df7, int offsetLeft, out int n7, out double[] y7, out double[,] x7)
        {
            //handles both right and elv types
            //df = n - m + k, so n = df + m - k
            n7 = df7 + m - k - offsetLeft;
            //cut obs from the right, and also from the left with offsetLeft
            y7 = new double[n7];
            x7 = new double[n7, m];
            for (int i = 0; i < n7; i++)
            {
                y7[i] = y[i + offsetLeft];
                for (int j = 0; j < m; j++)
                {
                    x7[i, j] = x[i + offsetLeft, j];
                }
            }
        }

        private static void OLSRecursiveHelperL(int m, double[,] x, double[] y, int k, int n, int df7, out int n7, out double[] y7, out double[,] x7)
        {
            //handles left types
            //df = n - m + k, so n = df + m - k
            n7 = df7 + m - k;
            //cut obs from the right, and also from the left with offsetLeft
            y7 = new double[n7];
            x7 = new double[n7, m];
            //for (int i = 0; i < n7; i++)
            for (int i = n - n7; i < n; i++)
            {
                y7[i - (n - n7)] = y[i];
                for (int j = 0; j < m; j++)
                {
                    x7[i - (n - n7), j] = x[i, j];
                }
            }
        }

        private static string OLSFormatHelper(OLSResults ols)
        {
            return "R2: " + Math.Round(ols.r2, 6, MidpointRounding.AwayFromZero) + "    " + "SEE: " + Program.RoundToSignificantDigits(ols.see, 6) + "    " + "DW: " + Math.Round(ols.dw, 4, MidpointRounding.AwayFromZero);
        }

        private static OLSResults OLSHelper(GekkoTime t1, GekkoTime t2, double[] y, double[,] x, double[,] xOriginal, double[,] restrict_input, double[] scaling, int n, int m, int k, int df, bool calledFromRecursive)
        {
            OLSResults ols = new OLSResults();

            double[,] r = null;

            r = new double[restrict_input.GetLength(0), restrict_input.GetLength(1) - 1];
            for (int i = 0; i < restrict_input.GetLength(0); i++)
            {
                for (int j = 0; j < restrict_input.GetLength(1) - 1; j++)
                {
                    r[i, j] = restrict_input[i, j];
                }
            }

            alglib.lsfit.lsfitreport rep = new alglib.lsfit.lsfitreport();
            //http://www.alglib.net/translator/man/manual.csharp.html#sub_lsfitlinearc
            //if it detects k = 0, it just calls same procedure as alglib.lsfit.lsfitlinear()
            ols.beta = null;
            int info2 = -12345;
            try
            {
                alglib.lsfit.lsfitlinearc(y, x, restrict_input, n, m, k, ref info2, ref ols.beta, rep);
            }
            catch (Exception e)
            {
                if (calledFromRecursive == false)
                {
                    int missingsAtStart = OLSHelper_missingsStart(y, xOriginal);
                    int missingsAtEnd = OLSHelper_missingsEnd(y, xOriginal);

                    if (missingsAtStart > 0 || missingsAtEnd > 0)
                    {
                        G.Writeln();
                    }

                    if (missingsAtStart > 0)
                    {
                        new Error(missingsAtStart + " missing values at start of sample", false);
                    }

                    if (missingsAtEnd > 0)
                    {
                        new Error(missingsAtEnd + " missing values at end of sample", false);
                    }

                    if (missingsAtStart > 0 || missingsAtEnd > 0)
                    {
                        t1 = t1.Add(missingsAtStart);
                        t2 = t2.Add(-missingsAtEnd);
                        if (t1.SmallerThanOrEqual(t2))
                        {
                            G.Writeln("           Suggested OLS period: <" + t1.ToString() + " " + t2.ToString() + ">", Color.Red);
                        }
                        throw new GekkoException();
                    }

                    if (e.Message != null && e.Message != "")
                    {
                        new Error(e.Message, false);
                        new Error("OLS does not solve, please check data for missings etc.", false);
                    }
                    if (e.InnerException != null && e.InnerException.Message != null && e.InnerException.Message != "")
                    {
                        new Error(e.InnerException.Message, false);
                        new Error("OLS does not solve, please check data for missings etc.", false);
                    }
                }
                throw;
            }
            if (info2 != 1)
            {
                if (calledFromRecursive == false)
                {
                    new Error("OLS does not solve.", false);
                    if (info2 == -2)
                    {
                        new Error("Internal SVD decomposition subroutine failed (degenerate systems only)", false);
                    }
                    if (info2 == -3)
                    {
                        new Error("Either too many constraints (more than # of parameters), degenerate constraints (some constraints are repeated twice) or inconsistent constraints were specified.", false);
                    }
                }
                throw new GekkoException();
            }

            ols.ypredict = new double[n];
            ols.residual = new double[n];
            double dw1 = 0d;
            ols.rss = 0d;
            ols.resMean = 0d;
            ols.lhsMean = 0d;
            double ySum = 0d;
            for (int i = 0; i < n; i++)
            {
                ySum += y[i];
            }
            double yAvg = ySum / (double)n;

            ols.ssTot = 0d;
            for (int i = 0; i < n; i++)
            {
                ols.ypredict[i] = 0d;
                for (int ik = 0; ik < m; ik++)
                {
                    ols.ypredict[i] += ols.beta[ik] * x[i, ik];
                }
                ols.residual[i] = y[i] - ols.ypredict[i];
                ols.resMean += ols.residual[i];

                ols.lhsMean += y[i];
                ols.rss += ols.residual[i] * ols.residual[i];
                ols.ssTot += (y[i] - yAvg) * (y[i] - yAvg);
                if (i > 0) dw1 += (ols.residual[i] - ols.residual[i - 1]) * (ols.residual[i] - ols.residual[i - 1]);
            }
            ols.resMean = ols.resMean / (double)n;
            ols.lhsMean = ols.lhsMean / (double)n;

            ols.dw = dw1 / ols.rss;
            ols.rmse = Math.Sqrt(ols.rss / (double)n);
            ols.see = Math.Sqrt(ols.rss / (double)df);
            ols.usedCovar = null;
            double[,] ixtx = Program.InvertMatrix(Program.XTransposeX(x), !calledFromRecursive);  //fails with an error, silent
            if (r.GetLength(0) == 0)
            {
                //covar = sigma^2 * inv(X'X)
                ols.usedCovar = Program.MultiplyMatrixScalar(ixtx, ols.see * ols.see, ixtx.GetLength(0), ixtx.GetLength(1));
            }
            else
            {
                //covar = sigma^2 *( inv(X'X)  -   inv(X'X) * R' inv( R  inv(X'X) R' ) R  inv(X'X) )                
                double[,] inside = Program.InvertMatrix(Program.MultiplyMatrices(Program.MultiplyMatrices(r, ixtx), Program.Transpose(r)), !calledFromRecursive);  //inv fails with an error, silent
                double[,] temp1 = Program.MultiplyMatrices(Program.MultiplyMatrices(Program.MultiplyMatrices(Program.MultiplyMatrices(ixtx, Program.Transpose(r)), inside), r), ixtx);
                double[,] temp2 = Program.SubtractMatrixMatrix(ixtx, temp1, ixtx.GetLength(0), ixtx.GetLength(1));
                ols.usedCovar = Program.MultiplyMatrixScalar(temp2, ols.see * ols.see, temp2.GetLength(0), temp2.GetLength(1));
            }

            //usedCovar = rep.covpar; --> this yields the same (without restrictions), also in the case without constant

            //unscale
            for (int i = 0; i < ols.usedCovar.GetLength(0); i++)
            {
                for (int j = 0; j < ols.usedCovar.GetLength(1); j++)
                {
                    ols.usedCovar[i, j] = ols.usedCovar[i, j] / scaling[i] / scaling[j];
                }
            }

            ols.usedCorr = new double[ols.usedCovar.GetLength(0), ols.usedCovar.GetLength(1)];
            for (int i = 0; i < ols.usedCovar.GetLength(0); i++)
            {
                for (int j = 0; j < ols.usedCovar.GetLength(1); j++)
                {
                    ols.usedCorr[i, j] = ols.usedCovar[i, j] / Math.Sqrt(ols.usedCovar[i, i]) / Math.Sqrt(ols.usedCovar[j, j]);
                }
            }

            ols.coeff = new double[m];
            ols.se = new double[m];
            ols.t = new double[m];
            for (int i = 0; i < m; i++)
            {
                ols.coeff[i] = 1d / scaling[i] * ols.beta[i];
                ols.se[i] = Math.Sqrt(ols.usedCovar[i, i]);
                ols.t[i] = Math.Abs(ols.coeff[i] / Math.Sqrt(ols.usedCovar[i, i]));
            }

            ols.r2 = 1d - ols.rss / ols.ssTot;
            //k is number of impose restrictions
            //our m includes the constant term
            //See this page: https://en.wikipedia.org/wiki/Coefficient_of_determination
            //There the correction is (n-1)/(n-p-1), where p is number of regressors not counting the constant term.
            ols.r2cor = 1d - (1 - ols.r2) * (n - 1) / (double)df; //google "r2 adjusted formula". Our m includes the constant, usually regressors do not count the constant -> therefore (m-1). TT added k, must be so.

            return ols;

        }

        private static int OLSHelper_missingsStart(double[] y, double[,] xOriginal)
        {
            int missingsAtStart = 0;
            for (int i = 0; i < y.Length; i++)
            {
                if (G.isNumericalError(y[i]))
                {
                    missingsAtStart++;
                    goto Label1;
                }
                int nn = xOriginal.GetLength(1);
                for (int ii = 0; ii < nn; ii++)
                {
                    if (G.isNumericalError(xOriginal[i, ii]))
                    {
                        missingsAtStart++;
                        goto Label1;
                    }
                }
                break;  //no more missings
            Label1:;
            }

            return missingsAtStart;
        }

        private static int OLSHelper_missingsEnd(double[] y, double[,] xOriginal)
        {
            int missingsAtEnd = 0;
            for (int i = y.Length - 1; i >= 0; i--)
            {
                if (G.isNumericalError(y[i]))
                {
                    missingsAtEnd++;
                    goto Label1;
                }
                int nn = xOriginal.GetLength(1);
                for (int ii = 0; ii < nn; ii++)
                {
                    if (G.isNumericalError(xOriginal[i, ii]))
                    {
                        missingsAtEnd++;
                        goto Label1;
                    }
                }
                break;  //no more missings
            Label1:;
            }

            return missingsAtEnd;
        }

    }
}
