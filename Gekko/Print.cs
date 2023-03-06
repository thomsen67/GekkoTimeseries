using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Gekko
{
    public static class Print
    {

        public static void OPrint(O.Prt o, List<string> labelsHandmade, List<string> labelOriginal)
        {
            //Note: it is ok for a prtElement in o to be a List containg Series

            //string format = "f14.4";
            //TODO: we could check if there is 1 object printed and it is of type=normal. If so, the label could be printed.
            //  if .meta is augmented with a pointer to the array-series, the label for x[a] could be taken via that pointer.

            EPrintTypes type = GetPrintType(o);

            if (IsGmulprt(o, type))
            {

                for (int i = 0; i < o.prtElements.Count; i++)
                {
                    o.prtElements[i].operatorsFinal = new List<string> { "n", "p", "rn", "rp", "m", "q" };
                }
            }

            bool rows = false; if (G.Equal(o.opt_rows, "yes")) rows = true;

            List<O.Prt.Element> containerExplode = new List<O.Prt.Element>();

            //If PRT <m> unfold(#m, {#m}), we will get 1 prtElement (since there are no commas), where 
            //variable[0] and [1] are both lists with two items (if #m has two items).

            //If PRT unfold(#m, {#m}), we will get 1 prtElement (since there are no commas), where 
            //variable[0] is a list with two items (if #m has two items).
            //this must be exploded into <n p>, so from the 1 prtElement, we should take
            //variable[0][0] as n, variable[0][0] as p, variable[0][1] as n, variable[0][1] as p.

            int labelMaxLine = 1;

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================
            //bool[] freqs = new bool[6];  //0=U, 1=A, 2=Q, 3=M, 4=D, 5=W
            GekkoDictionary<string, bool> freqs = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            freqs.Add("U", false);
            freqs.Add("A", false);
            freqs.Add("Q", false);
            freqs.Add("M", false);
            freqs.Add("W", false);
            freqs.Add("D", false);

            int numberOfGekkoNullVariables = 0;
            int numberOfOtherVariables = 0;

            int k = -1;

            foreach (O.Prt.Element element in o.prtElements) //for each comma in the prt statement
            {
                k++;
                List xx0 = element.variable[0] as List;
                List xx1 = element.variable[1] as List;
                int prtElementCounter = 1;
                if (xx0 != null && xx1 != null)
                {
                    if (xx0.list.Count != xx1.list.Count)
                    {
                        new Error("Lists with different number of items");
                    }
                }
                if (xx0 != null) prtElementCounter = xx0.list.Count;
                else if (xx1 != null) prtElementCounter = xx1.list.Count;

                for (int i = 0; i < prtElementCounter; i++)  //this element may be a #-list with 2 timeseries, x1 and x2
                {
                    int iOperator = -1;
                    foreach (string operator2 in element.operatorsFinal)  //this may be two printcodes <n p>
                    {

                        iOperator++;
                        //after this, it should be x1<n>  x1<p>  x2<n>  x2<p>

                        O.Prt.Element explodeElement = new O.Prt.Element();

                        int width = -12345;
                        int dec = -12345;
                        bool isPchType = false;
                        if (IsLevelOperator(operator2))
                        {
                            isPchType = false;
                            width = Program.options.print_fields_nwidth;
                            dec = Program.options.print_fields_ndec;
                        }
                        else
                        {
                            isPchType = true;
                            width = Program.options.print_fields_pwidth;
                            dec = Program.options.print_fields_pdec;
                            if (operator2 == Globals.operator_dl || operator2 == Globals.operator_rdl) dec = dec + 2;
                        }

                        // ---------------------------------------------
                        // --- Width -----------------------------------
                        // ---------------------------------------------
                        if (o.opt_width != -12345) width = (int)o.opt_width;
                        if (isPchType)
                        {
                            //overrides ph.width if given
                            if (o.opt_pwidth != -12345) width = (int)o.opt_pwidth;
                        }
                        else
                        {
                            //overrides ph.width if given
                            if (o.opt_nwidth != -12345) width = (int)o.opt_nwidth;
                        }

                        //element-specific stuff
                        if (element.width != -12345) width = element.width;   //element-specific width overrides!
                        if (isPchType)
                        {
                            //overrides ph.width if given
                            if (element.pwidth != -12345) width = element.pwidth;
                        }
                        else
                        {
                            //overrides ph.width if given
                            if (element.nwidth != -12345) width = element.nwidth;
                        }


                        // ---------------------------------------------
                        // --- Decimals --------------------------------
                        // ---------------------------------------------
                        if (o.opt_dec != -12345) dec = (int)o.opt_dec;
                        if (isPchType)
                        {
                            //overrides ph.dec if given
                            if (o.opt_pdec != -12345) dec = (int)o.opt_pdec;
                        }
                        else
                        {
                            //overrides ph.dec if given
                            if (o.opt_ndec != -12345) dec = (int)o.opt_ndec;
                        }

                        //element-specific stuff
                        if (element.dec != -12345) dec = element.dec;   //element-specific dec overrides!
                        if (isPchType)
                        {
                            //overrides ph.dec if given
                            if (element.pdec != -12345) dec = element.pdec;
                        }
                        else
                        {
                            //overrides ph.dec if given
                            if (element.ndec != -12345) dec = element.ndec;
                        }

                        if (type == EPrintTypes.Plot)
                        {
                            explodeElement.widthFinal = 25;
                            explodeElement.decFinal = 10;
                        }
                        else
                        {
                            explodeElement.widthFinal = width;
                            explodeElement.decFinal = dec;
                        }

                        explodeElement.linetype = element.linetype;
                        explodeElement.dashtype = element.dashtype;
                        explodeElement.linewidth = element.linewidth;
                        explodeElement.linecolor = element.linecolor;
                        explodeElement.pointtype = element.pointtype;
                        explodeElement.pointsize = element.pointsize;
                        explodeElement.fillstyle = element.fillstyle;
                        explodeElement.y2 = element.y2;

                        if (xx0 != null) explodeElement.variable[0] = xx0.list[i];
                        else explodeElement.variable[0] = element.variable[0];
                        if (xx1 != null) explodeElement.variable[1] = xx1.list[i];
                        else explodeElement.variable[1] = element.variable[1];

                        int bankCombi = GetBankCombi(operator2);

                        bool tjek1 = bankCombi == 0 && SkipSubSeries(explodeElement.variable[0]);
                        bool tjek2 = bankCombi == 1 && SkipSubSeries(explodeElement.variable[1]);
                        bool tjek3 = bankCombi == 2 && (SkipSubSeries(explodeElement.variable[0]) || SkipSubSeries(explodeElement.variable[1]));

                        if (tjek1 || tjek2 || tjek3)
                        {
                            numberOfGekkoNullVariables++;
                            continue;
                        }

                        numberOfOtherVariables++;

                        Series temp0 = explodeElement.variable[0] as Series;
                        Series temp1 = explodeElement.variable[1] as Series;

                        //========================================================================================================
                        //                          FREQUENCY LOCATION, indicates where to implement more frequencies
                        //========================================================================================================

                        if (temp0 != null)
                        {
                            if (temp0.freq == EFreq.U) freqs["U"] = true;
                            else if (temp0.freq == EFreq.A) freqs["A"] = true;
                            else if (temp0.freq == EFreq.Q) freqs["Q"] = true;
                            else if (temp0.freq == EFreq.M) freqs["M"] = true;                            
                            else if (temp0.freq == EFreq.W) freqs["W"] = true;
                            else if (temp0.freq == EFreq.D) freqs["D"] = true;

                        }
                        else if (temp1 != null)
                        {
                            if (temp1.freq == EFreq.U) freqs["U"] = true;
                            else if (temp1.freq == EFreq.A) freqs["A"] = true;
                            else if (temp1.freq == EFreq.Q) freqs["Q"] = true;
                            else if (temp1.freq == EFreq.M) freqs["M"] = true;                            
                            else if (temp1.freq == EFreq.W) freqs["W"] = true;
                            else if (temp1.freq == EFreq.D) freqs["D"] = true;
                        }

                        if (explodeElement.variable[0] != null && !G.IsValueType(explodeElement.variable[0]) || explodeElement.variable[1] != null && !G.IsValueType(explodeElement.variable[1]))
                        {
                            new Warning("Non-value in PRT");
                            return;
                        }

                        explodeElement.operatorFinal = operator2;

                        // ----------------------------------------------------
                        // Labels start
                        // ----------------------------------------------------

                        List<string> lbl = new List<string>();  //count = 0!

                        try
                        {
                            lbl = OPrintLabels(element.labelGiven, element.labelRecordedPieces, prtElementCounter, i);
                        }
                        catch { lbl = new List<string>(); }

                        if (lbl.Count != prtElementCounter)
                        {
                            Mismatch();
                            string l = G.ReplaceGlueSymbols(RemoveSplitter(labelOriginal[k]).Split('|')[0]);
                            lbl = new List<string>();
                            for (int ii = 0; ii < prtElementCounter; ii++)
                            {
                                lbl.Add(l);
                            }
                        }

                        // ----------------------------------------------------
                        // Labels end
                        // ----------------------------------------------------

                        int lines = -12345;
                        int widthHere = -12345;

                        if (type == EPrintTypes.Plot || type == EPrintTypes.Sheet || type == EPrintTypes.Clip || rows)
                        {
                            lines = 1;
                            widthHere = int.MaxValue;
                        }
                        else
                        {
                            lines = 5;
                            widthHere = explodeElement.widthFinal;
                        }

                        int max2 = PrintCreateLabelsArrayNew(lbl[i], widthHere, lines, lines * widthHere, out explodeElement.labelOLD);

                        labelMaxLine = Math.Max(max2, labelMaxLine);

                        //FIXME
                        //FIXME

                        string ee = null;
                        IVariable iv = element.variable[0];
                        if (iv != null && iv as Series != null)
                        {
                            Series ts = iv as Series;
                            if (ts.type == ESeriesType.Normal)
                            {
                                string name = ts.GetName();
                                if (name != null)
                                {
                                    string n2 = G.Chop_RemoveFreq(name);
                                    if (G.IsSimpleToken(n2))
                                    {
                                        EEndoOrExo endoExo = Program.VariableTypeEndoExo(n2);
                                        if (endoExo == EEndoOrExo.Endo) ee = "(E)";
                                        else if (endoExo == EEndoOrExo.Exo) ee = "(X)";
                                    }
                                }
                            }
                        }

                        if (G.Equal(element.operatorsFinal[0], "n") && iOperator > 0 && G.Equal(operator2, "p"))
                        {
                            explodeElement.labelOLD = new List<string> { ee + "%" };
                        }
                        else if (G.Equal(element.operatorsFinal[0], "m") && iOperator > 0 && G.Equal(operator2, "q"))
                        {
                            explodeElement.labelOLD = new List<string> { ee + "%" };
                        }
                        else
                        {
                            if (iOperator == 0) ; //c.label = c.label + "  (" + printCode.ToLower() + ")";
                            else if (iOperator > 0) explodeElement.labelOLD = new List<string> { "<" + operator2.ToLower() + ">" };
                        }

                        containerExplode.Add(explodeElement);
                    }
                }
            }

            int n = containerExplode.Count;  //number of printed variables (including percent etc.), think of it as number of "columns"

            GekkoSmpl smpl = new GekkoSmpl(o.t1, o.t2);
            List inputList = null;
            List<IVariable> errorList = new List<IVariable>();

            bool good = true;

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================
            // ----------------------- start -------------------------------------------------
            if (freqs["U"] && (freqs["A"] || freqs["Q"] || freqs["M"] || freqs["W"] || freqs["D"]))
            {
                new Error("You cannot mix undated and other frequencies for PRT/PLOT");
            }

            EFreq sameFreq = EFreq.None;
            if      (freqs["U"] && !freqs["A"] && !freqs["Q"] && !freqs["M"] && !freqs["W"] && !freqs["D"]) sameFreq = EFreq.U;
            else if (!freqs["U"] && freqs["A"] && !freqs["Q"] && !freqs["M"] && !freqs["W"] && !freqs["D"]) sameFreq = EFreq.A;
            else if (!freqs["U"] && !freqs["A"] && freqs["Q"] && !freqs["M"] && !freqs["W"] && !freqs["D"]) sameFreq = EFreq.Q;
            else if (!freqs["U"] && !freqs["A"] && !freqs["Q"] && freqs["M"] && !freqs["W"] && !freqs["D"]) sameFreq = EFreq.M;
            else if (!freqs["U"] && !freqs["A"] && !freqs["Q"] && !freqs["M"] && freqs["W"] && !freqs["D"]) sameFreq = EFreq.W;
            else if (!freqs["U"] && !freqs["A"] && !freqs["Q"] && !freqs["M"] && !freqs["W"] && freqs["D"]) sameFreq = EFreq.D;
            else sameFreq = EFreq.None;  //superflous, just to state the obvious

            if (!freqs["U"] && !freqs["A"] && !freqs["Q"] && !freqs["M"] && !freqs["W"] && !freqs["D"])
            {
                //for instance printing a scalar
                sameFreq = Program.options.freq;
                if (Program.options.freq == EFreq.U) freqs["U"] = true;
                else if (Program.options.freq == EFreq.A) freqs["A"] = true;
                else if (Program.options.freq == EFreq.Q) freqs["Q"] = true;
                else if (Program.options.freq == EFreq.M) freqs["M"] = true;
                else if (Program.options.freq == EFreq.W) freqs["W"] = true;
                else if (Program.options.freq == EFreq.D) freqs["D"] = true;                
            }

            // --------------------------- end ---------------------------------------------

            int y1 = smpl.t1.super;
            int y2 = smpl.t2.super;

            if (numberOfOtherVariables == 0 && numberOfGekkoNullVariables > 0)
            {
                new Warning("Non-existing array-series (skipped)");
                return;
            }

            //TIMEFILTER!!!!!!! with avg

            //timefilter removes items hitted. If avg/sum timefilter, track the omitted and print them instead of non-hitted

            if (type == EPrintTypes.Plot)
            {
                if (n > Program.options.plot_elements_max)
                {
                    if (!G.Equal(o.opt_nomax, "yes"))
                    {
                        new Error("PLOT had " + n + " elements, max is " + Program.options.plot_elements_max + ". You can use PLOT<nomax> or set OPTION plot elements max = ... ;");
                    }
                }
            }
            else if (type == EPrintTypes.Print)
            {
                if (n > Program.options.print_elements_max)
                {
                    if (!G.Equal(o.opt_nomax, "yes"))
                    {
                        new Error("PRINT had " + n + " elements, max is " + Program.options.print_elements_max + ". You can use PRT<nomax> or set OPTION print elements max = ... ;");                        
                    }
                }
            }

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================            
            // there is a lot of frequency stuff going on in the following printing
            //--------------------------------------------------------------------------------------------------------

            EFreq highestFreq = EFreq.A;
            if (freqs["U"]) highestFreq = EFreq.U;
            if (freqs["A"]) highestFreq = EFreq.A;
            if (freqs["Q"]) highestFreq = EFreq.Q;
            if (freqs["M"]) highestFreq = EFreq.M;
            if (freqs["W"]) highestFreq = EFreq.W;  //d and w freqs ought to switch places in freqs[] array... but never mind
            if (freqs["D"]) highestFreq = EFreq.D;  //d and w freqs ought to switch places in freqs[] array... but never mind

            bool showAllFreqsEachYear = true;
            if (type == EPrintTypes.Sheet) showAllFreqsEachYear = false;  //SHEET <2010q2 2010q3> should not show q1 and q3

            bool pretty = false;
            if (type == EPrintTypes.Sheet && G.Equal(Program.options.sheet_freq, "pretty")) pretty = true;
            if (type == EPrintTypes.Print && G.Equal(Program.options.print_freq, "pretty")) pretty = true;
            
            EPrtCollapseTypes collapse = GetCollapseType(o, type);
            if (pretty == false) collapse = EPrtCollapseTypes.None;  //switched off for non-pretty

            bool showRowWithYear = pretty || (sameFreq == EFreq.U || sameFreq == EFreq.A);

            EPrtPlotSheet tabletype = EPrtPlotSheet.Unknown;
            if (type == EPrintTypes.Plot)
            {
                tabletype = EPrtPlotSheet.Plot;
            }
            else
            {
                if (!freqs["A"] && !freqs["Q"] && !freqs["M"] && freqs["W"])
                {
                    //W alone or W+D, but not A or Q or M
                    tabletype = EPrtPlotSheet.PrintMixedWDPretty;
                }
                else if (!freqs["A"] && !freqs["Q"] && freqs["D"] && !freqs["W"])
                {
                    //D alone or D+M, but not A or Q or W
                    tabletype = EPrtPlotSheet.PrintMixedMDPretty;
                }
                else if (sameFreq == EFreq.U || ((freqs["A"] || freqs["Q"] || freqs["M"]) && !(freqs["D"] || freqs["W"])))
                {
                    // A or Q or M, but not W or D
                    // Also handles U, if there are not other freqs
                    tabletype = EPrtPlotSheet.PrintMixedAQMPretty;
                }
                else
                {
                    string s = null;
                    foreach (KeyValuePair<string, bool> kvp in freqs)
                    {
                        if (kvp.Value) s += G.ConvertFreq(kvp.Key).Pretty() + ", ";
                    }
                    s = s.Substring(0, s.Length - ", ".Length);
                    string ss = "print"; if (type == EPrintTypes.Clip) ss = "clip"; else if (type == EPrintTypes.Sheet) ss = "sheet";
                    new Error("You cannot mix series with frequencies " + s + " in one " + ss + ". You may try the " + ss + "<split> to split the frequencies.");
                }
            }            

            int iPlot = 0;            

            Table printTable = null;
            PlotTable plotTable = null; //more lightweight than Table

            if (tabletype == EPrtPlotSheet.Plot)
            {
                plotTable = Program.PlotMixed(smpl, type, containerExplode, n, o, highestFreq);
            }
            else
            {
                if (tabletype == EPrtPlotSheet.PrintMixedAQMPretty)
                {
                    printTable = PrintMixedAQM(smpl, type, rows, containerExplode, labelMaxLine, freqs, n, sameFreq, y1, y2, pretty, collapse, showRowWithYear, iPlot, o);
                }
                else if (tabletype == EPrtPlotSheet.PrintMixedWDPretty)
                {
                    printTable = PrintMixedWD(smpl, type, rows, containerExplode, labelMaxLine, n, pretty, freqs, o);
                }
                else if (tabletype == EPrtPlotSheet.PrintMixedMDPretty)
                {
                    printTable = PrintMixedMD(smpl, type, rows, containerExplode, labelMaxLine, n, pretty, freqs, o);
                }
            }

            //bool filter = ShouldFilterPeriod(new GekkoTime());

            bool seriesAreInRows = true;  //default for SHEET, opposite of default for PRT.
            bool hasNames = true; if (G.Equal(o.opt_names, "no")) hasNames = false;
            bool hasDates = true; if (G.Equal(o.opt_dates, "no")) hasDates = false;

            if (type == EPrintTypes.Plot)
            {                
                Plot.CallGnuplot(plotTable, o, containerExplode, highestFreq, new PlotHelper(), smpl.p);
            }
            else if (type == EPrintTypes.Sheet)
            {
                bool isStamp = false; if (o != null && G.Equal(o.opt_stamp, "yes")) isStamp = true;
                string title = o.opt_title;

                Table tab2 = null;

                if (Globals.excelDna)
                {
                    //transposing is easier here for ExcelDna than for Epplus.
                    if (G.Equal(o.opt_cols, "yes"))
                    {
                        tab2 = printTable;
                        seriesAreInRows = false;
                    }
                    else
                    {
                        tab2 = printTable.Transpose();                        
                    }
                    //
                    //
                    // BUG: the method does not handle <names=no> or <dates=no>... #yuadsf8adsfjs
                    //
                    //
                    Program.PrtToExcelDna(tab2, IsMulprt(o), isStamp, title, hasNames, hasDates, seriesAreInRows);
                }
                else
                {
                    tab2 = printTable.Transpose();
                    ExcelOptions eo = Program.PrepareDataForExcel(tab2);
                    Program.WriteExcel(eo, o, IsMulprt(o), false, o.opt_dateformat, o.opt_datetype);
                }
                return;
            }
            else if (type == EPrintTypes.Clip)
            {
                //do not print anything, but put it on clipboard     
                Table tab2 = null;
                if (G.Equal(o.opt_cols, "yes"))
                {
                    tab2 = printTable;
                }                
                else
                {
                    tab2 = printTable.Transpose();
                }
                Program.PrtClipboard(tab2, false);
            }
            else  //is .Print type
            {
                if (rows)
                {
                    printTable = printTable.Transpose();  //else we have series normally, <cols>
                }
                else
                {
                    seriesAreInRows = false;
                }

                if (Globals.excelDna)
                {
                    Program.PrtToExcelDna(printTable, false, false, null, hasNames, hasDates, seriesAreInRows);
                }
                else
                {
                    int widthRemember = Program.options.print_width;
                    Program.options.print_width = int.MaxValue;
                    try
                    {
                        G.Writeln("");
                        List<string> ss = printTable.Print();
                        foreach (string s in ss) G.Writeln(s);
                    }
                    finally
                    {
                        //resetting, also if there is an error
                        Program.options.print_width = widthRemember;
                    }
                    Globals.lastPrtOrMulprtTable = printTable;  //if CLIP x, y, z, this Globals.lastPrtOrMulprtTable is used later on
                    CrossThreadStuff.CopyButtonEnabled(true);
                }
            }
        }

        /// <summary>
        /// Print daily series (possibly together with monthly)
        /// </summary>
        private static Table PrintMixedMD(GekkoSmpl smpl, EPrintTypes type, bool rows, List<O.Prt.Element> containerExplode, int labelMaxLine, int n, bool pretty, GekkoDictionary<string, bool> freqs, O.Prt o)
        {
            Table table = new Table();
            table.writeOnce = true;            

            for (int j = 1; j < n + 2; j++)
            {
                int[] skipCounter = new int[4];

                O.Prt.Element cc;
                string operator2, format;
                List<string> label;
                EFreq freqColumn;
                double scalarValueWork, scalarValueRef;
                Series tsWork, tsRef;
                PrintPrepareColumn(type, containerExplode, j, out cc, out operator2, out label, out format, out freqColumn, out scalarValueWork, out tsWork, out scalarValueRef, out tsRef);

                int i = 0;

                //              x!d      x!m
                // 2019m1                100
                //     d1        22
                //     d2        23
                //     d3        42
                //    ...
                // 2019m2
                //     d1        43
                //     d2        23
                //     d3        44
                //    ...

                bool isMonthlyFreq = false;
                if (tsWork != null && tsWork.freq == EFreq.M || tsRef != null && tsWork.freq == EFreq.M)
                {
                    isMonthlyFreq = true;
                }

                i++;
                i = PutLabelIntoTable(table, i, j, label, labelMaxLine);  //augments i
                i--; //else a blank line too much at start                    

                if (pretty)  //pretty printing (sheet is non-pretty)
                {
                    int counter = 0;

                    foreach (GekkoTime t in new GekkoTimeIterator(Program.ConvertFreqs(smpl.t1, smpl.t2, EFreq.D)))  //handles if the freq given is not daily
                    {
                        //TODO: allow prt x!d, x!m, but not other freqs.

                        counter++;

                        if (counter == 1 || t.subsub == 1)
                        {
                            if (j == 1)
                            {
                                i++;
                                i++;
                                table.Set(i, j, t.super + "m" + t.sub); if (rows) table.SetAlign(i, j, Align.Right);
                            }
                            else
                            {
                                i++;
                                i++;
                                if (t.subsub == 1 && isMonthlyFreq)
                                {                                    
                                    GekkoTime tMonth = new GekkoTime(EFreq.M, t.super, t.sub);

                                    double d = double.NaN;
                                    if (tsWork == null && tsRef == null)  //not series
                                    {
                                        d = PrintHelperTransformScalar(scalarValueWork, scalarValueRef, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                                    }
                                    else
                                    {
                                        d = PrintHelperTransform(smpl, tsWork, tsRef, tMonth, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                                    }
                                    table.SetNumber(i, j, d, format);
                                }
                            }
                        }

                        i++;

                        if (j == 1)
                        {
                            table.Set(i, j, "d" + t.subsub); if (rows) table.SetAlign(i, j, Align.Right);
                        }
                        else
                        {
                            //j > 1

                            if (!isMonthlyFreq)
                            {                                
                                double d = double.NaN;
                                if (tsWork == null && tsRef == null)  //not series
                                {
                                    d = PrintHelperTransformScalar(scalarValueWork, scalarValueRef, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                                }
                                else
                                {
                                    d = PrintHelperTransform(smpl, tsWork, tsRef, t, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                                }

                                table.SetNumber(i, j, d, format);
                            }
                        }
                    }
                }
                else
                {
                    //sheet or non-pretty printing
                    if (isMonthlyFreq)
                    {
                        new Error("Cannot use M and D freq at the same time");
                        //throw new GekkoException();
                    }
                    i++;
                    foreach (GekkoTime t in new GekkoTimeIterator(Program.ConvertFreqs(smpl.t1, smpl.t2, EFreq.D)))  //handles if the freq given is not daily
                    {
                        //TODO: allow prt x!d, x!m, but not other freqs.

                        i++;

                        if (j == 1)
                        {
                            table.Set(i, j, t.ToString()); if (rows) table.SetAlign(i, j, Align.Right);
                            table.Get(i, j).date_hack = t;
                        }
                        else
                        {                            
                            double d = double.NaN;
                            if (tsWork == null && tsRef == null)  //not series
                            {
                                d = PrintHelperTransformScalar(scalarValueWork, scalarValueRef, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                            }
                            else
                            {
                                d = PrintHelperTransform(smpl, tsWork, tsRef, t, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                            }
                            table.SetNumber(i, j, d, format);
                        }
                    }
                }
            }
            return table;
        }

        /// <summary>
        /// Print weekly series (possibly together with daily)
        /// </summary>
        private static Table PrintMixedWD(GekkoSmpl smpl, EPrintTypes type, bool rows, List<O.Prt.Element> containerExplode, int labelMaxLine, int n, bool pretty, GekkoDictionary<string, bool> freqs, O.Prt o)
        {
            Table table = new Table();
            table.writeOnce = true;

            for (int j = 1; j < n + 2; j++)
            {
                int[] skipCounter = new int[4];

                O.Prt.Element cc;
                string operator2, format;
                List<string> label;
                EFreq freqColumn;
                double scalarValueWork, scalarValueRef;
                Series tsWork, tsRef;
                PrintPrepareColumn(type, containerExplode, j, out cc, out operator2, out label, out format, out freqColumn, out scalarValueWork, out tsWork, out scalarValueRef, out tsRef);

                int i = 0;

                //              x!d      x!m
                // 2019m1                100
                //     d1        22
                //     d2        23
                //     d3        42
                //    ...
                // 2019m2
                //     d1        43
                //     d2        23
                //     d3        44
                //    ...

                bool isWeeklyFreq = false;
                if (tsWork != null && tsWork.freq == EFreq.W || tsRef != null && tsWork.freq == EFreq.W)
                {
                    isWeeklyFreq = true;
                }                

                i++;
                i = PutLabelIntoTable(table, i, j, label, labelMaxLine);  //augments i
                i--; //else a blank line too much at start                    

                if (pretty)  //pretty printing (sheet is non-pretty)
                {
                    int counter = 0;

                    GekkoSmplSimple period = Program.ConvertFreqs(smpl.t1, smpl.t2, EFreq.D);

                    int oldYear = -12345;

                    foreach (GekkoTime t in new GekkoTimeIterator(period))  //handles if the freq given is not daily
                    {
                        //TODO: allow prt x!d, x!w, but not other freqs.
                                                
                        counter++;

                        //DateTime dt2 = ISOWeek.ToDateTime(t.super, t.sub, DayOfWeek.Monday);
                        DateTime dt = new DateTime(t.super, t.sub, t.subsub);
                        GekkoTime tWeek = ISOWeek.ToGekkoTime(dt);

                        if (oldYear != -12345 && tWeek.super > oldYear && !freqs["d"])
                        {
                            //line break for each year, if W freq only
                            //note that this separates for instance labels
                            //2001w52 and 2001w1, and these years may be
                            //a bit phoney around New Year.
                            i++;  
                        }
                        oldYear = tWeek.super;

                        if (counter == 1 || dt.DayOfWeek == DayOfWeek.Monday)
                        {
                            i++;
                            i++;
                            if (j == 1)
                            {                                
                                table.Set(i, j, tWeek.super + "w" + tWeek.sub); if (rows) table.SetAlign(i, j, Align.Right);
                            }
                            else
                            {
                                if (dt.DayOfWeek == DayOfWeek.Monday && isWeeklyFreq)
                                {
                                    double d = double.NaN;
                                    if (tsWork == null && tsRef == null)  //not series
                                    {
                                        d = PrintHelperTransformScalar(scalarValueWork, scalarValueRef, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                                    }
                                    else
                                    {
                                        d = PrintHelperTransform(smpl, tsWork, tsRef, tWeek, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                                    }
                                    table.SetNumber(i, j, d, format);
                                }
                            }
                            if (!freqs["d"]) i--;  //to avoid blank lines if only W freq is present
                        }

                        if (freqs["d"])
                        {
                            i++;

                            if (j == 1)
                            {
                                table.Set(i, j, "m" + t.sub + "d" + t.subsub); if (rows) table.SetAlign(i, j, Align.Right);
                            }
                            else
                            {
                                //j > 1

                                if (!isWeeklyFreq)
                                {
                                    double d = double.NaN;
                                    if (tsWork == null && tsRef == null)  //not series
                                    {
                                        d = PrintHelperTransformScalar(scalarValueWork, scalarValueRef, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                                    }
                                    else
                                    {
                                        d = PrintHelperTransform(smpl, tsWork, tsRef, t, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                                    }

                                    table.SetNumber(i, j, d, format);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //sheet or non-pretty printing
                    if (freqs["w"] && freqs["d"])
                    {
                        new Error("Cannot use W and D freq at the same time");
                    }
                    i++;
                    foreach (GekkoTime t in new GekkoTimeIterator(Program.ConvertFreqs(smpl.t1, smpl.t2, EFreq.W)))
                    {
                        //TODO: allow prt x!d, x!m, but not other freqs.

                        i++;

                        if (j == 1)
                        {
                            table.Set(i, j, t.ToString()); if (rows) table.SetAlign(i, j, Align.Right);
                            table.Get(i, j).date_hack = t;
                        }
                        else
                        {
                            double d = double.NaN;
                            if (tsWork == null && tsRef == null)  //not series
                            {
                                d = PrintHelperTransformScalar(scalarValueWork, scalarValueRef, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                            }
                            else
                            {
                                d = PrintHelperTransform(smpl, tsWork, tsRef, t, operator2, o.guiGraphIsLogTransform, o.opt_i, EPrtCollapseTypes.None, 1, skipCounter);
                            }
                            table.SetNumber(i, j, d, format);
                        }
                    }
                }
            }
            return table;
        }

        private static Table PrintMixedAQM(GekkoSmpl smpl, EPrintTypes type, bool rows, List<O.Prt.Element> containerExplode, int labelMaxLine, GekkoDictionary<string, bool> freqs, int n, EFreq sameFreq, int y1, int y2, bool pretty, EPrtCollapseTypes collapse, bool showRowWithYear, int iPlot, O.Prt o)
        {
            Table table = new Table();
            table.writeOnce = true;

            AllFreqsHelper allFreqsDates = G.ConvertDateFreqsToAllFreqs(smpl.t1, smpl.t2);

            for (int j = 1; j < n + 2; j++)
            {
                int[] skipCounter = new int[4];

                O.Prt.Element cc;
                string operator2, format;
                List<string> label;
                EFreq freqColumn;
                double scalarValueWork, scalarValueRef;
                Series tsWork, tsRef;
                PrintPrepareColumn(type, containerExplode, j, out cc, out operator2, out label, out format, out freqColumn, out scalarValueWork, out tsWork, out scalarValueRef, out tsRef);

                int i = 0;

                // 1. 2003 (label)       
                // 2. 2003q1            Q         
                // 3. 2003m1                 M
                // 4. 2003m2                 M      
                // 5. 2003m3                 M
                // 6. SUM3M                  Msum (only when 3 M above else empty)
                // 7. 2003q2            Q           
                // 8. 2003m4                 M
                // 9. 2003m5                 M
                //10. 2003m6                 M
                //11. SUM3M                  Msum           
                //12. 2003q3            Q
                //13. 2003m7                 M
                //14. 2003m8                 M                 <--------- if timefilter is 2003m2..2003m7, we consolidate in 2003m8:  "2003m2-2003m8    123.45"
                //15. 2003m9                 M                            Msum is only shown if not touched by timefilter
                //16. SUM3M                  Msum           
                //17. 2003q4            Q          
                //18. 2003m10                M
                //19. 2003m11                M
                //20. 2003m12                M
                //21. SUM3M                  Msum
                //22. SUM12M                 Msum                                 
                //23. SUM4Q             Qsum           
                //24. ANNUAL     A


                //remember there is a label column which gets number 1
                for (int year = y1; year <= y2; year++)
                {
                    string uglyYear = null; if (!pretty) uglyYear = year.ToString();

                    if (type != EPrintTypes.Plot) // ------------------------------------------------------------- (1)
                    {

                        if (pretty || year == y1 || (sameFreq == EFreq.U || sameFreq == EFreq.A))
                        {
                            i++;
                        }
                        if (year == y1)
                        {
                            i = PutLabelIntoTable(table, i, j, label, labelMaxLine);  //will add to i
                        }
                        if (showRowWithYear)
                        {

                            i++;

                            //Non-plots have a first column with dates, plots have such a column for each series
                            //if (j == 1)  //then iv == null
                            {
                                // --------------------------                                
                                // --------------------------
                                if ((sameFreq == EFreq.U || sameFreq == EFreq.A) && Globals.globalPeriodTimeFilters2.Count > 0 && ((Globals.globalPeriodTimeFilters2[0].freq == EFreq.U && Program.ShouldFilterPeriod(new Gekko.GekkoTime(EFreq.U, year, 1))) || (Globals.globalPeriodTimeFilters2[0].freq == EFreq.A && Program.ShouldFilterPeriod(new Gekko.GekkoTime(EFreq.A, year, 1)))))
                                {
                                    //kind of hack for annual to omit year if the year is filtered out
                                    i--;
                                }
                                else
                                {
                                    if (j == 1)
                                    {
                                        table.Set(i, j, year.ToString()); if (rows) table.SetAlign(i, j, Align.Right);
                                        DateHack(table, i, j, sameFreq, year, 1);
                                    }
                                }
                            }

                            if (sameFreq == EFreq.U || sameFreq == EFreq.A) i--; // #98075235874325
                        }
                    }

                    if (true)  // ------------------------------------------------------------- (2)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 1;
                            int sumOver = 1;
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                // --------------------------
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m1"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {
                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (3)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 2;
                            int sumOver = 1;
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                // --------------------------
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m2"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {
                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (4)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 3;
                            int sumOver = 1;
                            // --------------------------
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m3"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {

                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }

                    if (collapse != EPrtCollapseTypes.None)  // ------------------------------------------------------------- (5)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 3;
                            int sumOver = 3;
                            // --------------------------
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                bool skip = false;
                                GekkoTime t = new GekkoTime(freqHere, year, subHere);
                                foreach (GekkoTime tFilter in Globals.globalPeriodTimeFilters2)
                                {
                                    if (t.freq == tFilter.freq && t.EqualsGekkoTime(tFilter))
                                    {
                                        skip = true;
                                        break;
                                    }
                                }

                                if (!skip)
                                {
                                    if (j == 1)
                                    {
                                        table.Set(i, j, uglyYear + "m1-m3"); if (rows) table.SetAlign(i, j, Align.Right);
                                        //no date hack
                                    }
                                    else
                                    {                                        
                                        PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                    }
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (6)
                    {
                        if ((type != EPrintTypes.Plot && freqs["Q"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.Q))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.Q;
                            int subHere = 1;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "q1"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {
                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }









                    if (true)  // ------------------------------------------------------------- (7)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 4;
                            int sumOver = 1;
                            // --------------------------
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m4"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {
                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (8)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 5;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m5"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {

                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (9)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 6;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m6"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {

                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }

                    if (collapse != EPrtCollapseTypes.None)  // ------------------------------------------------------------- (10)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 6;
                            int sumOver = 3;
                            // --------------------------
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                bool skip = false;
                                GekkoTime t = new GekkoTime(freqHere, year, subHere);
                                foreach (GekkoTime tFilter in Globals.globalPeriodTimeFilters2)
                                {
                                    if (t.freq == tFilter.freq && t.EqualsGekkoTime(tFilter))
                                    {
                                        skip = true;
                                        break;
                                    }
                                }

                                if (!skip)
                                {
                                    if (j == 1)
                                    {
                                        table.Set(i, j, uglyYear + "m4-m6"); if (rows) table.SetAlign(i, j, Align.Right);
                                        //no date hack
                                    }
                                    else
                                    {
                                        PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                    }
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (11)
                    {
                        if ((type != EPrintTypes.Plot && freqs["Q"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.Q))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.Q;
                            int subHere = 2;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "q2"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {
                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }























                    if (true)  // ------------------------------------------------------------- (12)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 7;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m7"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {

                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (13)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 8;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m8"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {

                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (14)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 9;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m9"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {

                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }

                    if (collapse != EPrtCollapseTypes.None)  // ------------------------------------------------------------- (15)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 9;
                            int sumOver = 3;
                            // --------------------------
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                bool skip = false;
                                GekkoTime t = new GekkoTime(freqHere, year, subHere);
                                foreach (GekkoTime tFilter in Globals.globalPeriodTimeFilters2)
                                {
                                    if (t.freq == tFilter.freq && t.EqualsGekkoTime(tFilter))
                                    {
                                        skip = true;
                                        break;
                                    }
                                }

                                if (!skip)
                                {
                                    if (j == 1)
                                    {
                                        table.Set(i, j, uglyYear + "m7-m9"); if (rows) table.SetAlign(i, j, Align.Right);
                                        //no date hack
                                    }
                                    else
                                    {
                                        PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                    }
                                }
                            }
                        }
                    }

                    if (true)  // ------------------------------------------------------------- (16)
                    {
                        if ((type != EPrintTypes.Plot && freqs["Q"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.Q))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.Q;
                            int subHere = 3;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "q3"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {
                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }












                    if (true)  // ------------------------------------------------------------- (17)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 10;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m10"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {

                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (18)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 11;
                            int sumOver = 1;
                            // --------------------------
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m11"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {

                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (19)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.M))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 12;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "m12"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {

                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }

                    if (collapse != EPrtCollapseTypes.None)  // ------------------------------------------------------------- (20)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 12;
                            int sumOver = 3;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                bool skip = false;
                                GekkoTime t = new GekkoTime(freqHere, year, subHere);
                                foreach (GekkoTime tFilter in Globals.globalPeriodTimeFilters2)
                                {
                                    if (t.freq == tFilter.freq && t.EqualsGekkoTime(tFilter))
                                    {
                                        skip = true;
                                        break;
                                    }
                                }

                                if (!skip)
                                {
                                    if (j == 1)
                                    {
                                        table.Set(i, j, uglyYear + "m10-m12"); if (rows) table.SetAlign(i, j, Align.Right);
                                        //no date hack
                                    }
                                    else
                                    {
                                        PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                    }
                                }
                            }
                        }
                    }


                    if (collapse != EPrtCollapseTypes.None)  // ------------------------------------------------------------- (21)
                    {
                        if ((type != EPrintTypes.Plot && freqs["M"]))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.M;
                            int subHere = 12;
                            int sumOver = 12;
                            // --------------------------
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                bool skip = false;
                                GekkoTime t = new GekkoTime(freqHere, year, subHere);
                                foreach (GekkoTime tFilter in Globals.globalPeriodTimeFilters2)
                                {
                                    if (t.freq == tFilter.freq && t.EqualsGekkoTime(tFilter))
                                    {
                                        skip = true;
                                        break;
                                    }
                                }

                                if (!skip)
                                {
                                    if (j == 1)
                                    {
                                        table.Set(i, j, uglyYear + "m1-m12"); if (rows) table.SetAlign(i, j, Align.Right);
                                        //no date hack
                                    }
                                    else
                                    {
                                        PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                    }
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (22)
                    {
                        if ((type != EPrintTypes.Plot && freqs["Q"]) || (type == EPrintTypes.Plot && freqColumn == EFreq.Q))
                        {// --------------------------
                            EFreq freqHere = EFreq.Q;
                            int subHere = 4;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    table.Set(i, j, uglyYear + "q4"); if (rows) table.SetAlign(i, j, Align.Right);
                                    DateHack(table, i, j, freqHere, year, subHere);
                                }
                                else
                                {
                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                            }
                        }
                    }


                    if (collapse != EPrtCollapseTypes.None)  // ------------------------------------------------------------- (23)
                    {
                        if ((type != EPrintTypes.Plot && freqs["Q"]))
                        {
                            // --------------------------
                            EFreq freqHere = EFreq.Q;
                            int subHere = 4;
                            int sumOver = 4;
                            // --------------------------
                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                bool skip = false;
                                GekkoTime t = new GekkoTime(freqHere, year, subHere);
                                foreach (GekkoTime tFilter in Globals.globalPeriodTimeFilters2)
                                {
                                    if (t.freq == tFilter.freq && t.EqualsGekkoTime(tFilter))
                                    {
                                        skip = true;
                                        break;
                                    }
                                }

                                if (!skip)
                                {
                                    if (j == 1)
                                    {
                                        table.Set(i, j, uglyYear + "q1-q4"); if (rows) table.SetAlign(i, j, Align.Right);
                                        //no date hack
                                    }
                                    else
                                    {
                                        PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                    }
                                }
                            }
                        }
                    }
                    if (true)  // ------------------------------------------------------------- (24)
                    {

                        int isUndatedOrAnnual = -12345;
                        if (type == EPrintTypes.Plot)
                        {
                            if (freqColumn == EFreq.U) isUndatedOrAnnual = 0;
                            else if (freqColumn == EFreq.A) isUndatedOrAnnual = 1;
                        }
                        else
                        {
                            if (freqs["U"]) isUndatedOrAnnual = 0;
                            else if (freqs["A"]) isUndatedOrAnnual = 1;
                        }

                        if (isUndatedOrAnnual != -12345)
                        {
                            // --------------------------

                            EFreq freqHere = EFreq.U;
                            if (isUndatedOrAnnual == 1) freqHere = EFreq.A;

                            //EFreq freqHere = EFreq.A;  //will also become "annual" if it is undated
                            int subHere = 1;
                            int sumOver = 1;
                            // --------------------------

                            if (PrintShouldKeep(allFreqsDates, freqHere, year, subHere))
                            {
                                i++;
                                if (j == 1)
                                {
                                    if (type != EPrintTypes.Plot && (sameFreq == EFreq.U || sameFreq == EFreq.A))
                                    {
                                        // #98075235874325
                                    }
                                    else
                                    {
                                        table.Set(i, j, uglyYear + "a");
                                        if (rows) table.SetAlign(i, j, Align.Right);
                                        DateHack(table, i, j, freqHere, year, subHere);
                                    }
                                }
                                else
                                {
                                    PrintHelper3(smpl, type, sameFreq, table, n, i, j, iPlot, operator2, o.guiGraphIsLogTransform, o.opt_i, scalarValueWork, tsWork, scalarValueRef, tsRef, year, freqHere, subHere, collapse, sumOver, skipCounter, cc);
                                }
                                if (type != EPrintTypes.Plot && (sameFreq == EFreq.U || sameFreq == EFreq.A)) i = i - 1; // #98075235874325
                            }
                        }
                    }
                }  //end of years                   
            }
            return table;
        }

        private static void PrintHelper3(GekkoSmpl smpl, EPrintTypes type, EFreq sameFreq, Table table, int count, int i, int j, int iPlot, string operator2, bool isLogTransform, GekkoTime index, double scalarValueWork, Series tsWork, double scalarValueRef, Series tsRef, int year, EFreq freqColumn, int subHere, EPrtCollapseTypes collapse, int sumOver, int[] skipCounter, O.Prt.Element cc)
        {            
            string format = "f" + cc.widthFinal + "." + cc.decFinal;

            GekkoTime t = new GekkoTime(freqColumn, year, subHere);

            foreach (GekkoTime tFilter in Globals.globalPeriodTimeFilters2)
            {
                if (t.freq == tFilter.freq && t.EqualsGekkoTime(tFilter))
                {
                    if (t.freq == EFreq.U) skipCounter[0]++;
                    else if (t.freq == EFreq.A) skipCounter[1]++;
                    else if (t.freq == EFreq.Q) skipCounter[2]++;
                    else if (t.freq == EFreq.M) skipCounter[3]++;
                    return;
                }
            }

            double? d = null;
            if (tsWork == null && tsRef == null)  //not series
            {
                if (sameFreq == freqColumn) d = PrintHelperTransformScalar(scalarValueWork, scalarValueRef, operator2, isLogTransform, index, collapse, sumOver, skipCounter);
            }
            else
            {
                if ((tsWork != null && tsWork.freq == freqColumn) || (tsRef != null && tsRef.freq == freqColumn))
                {
                    d = PrintHelperTransform(smpl, tsWork, tsRef, t, operator2, isLogTransform, index, collapse, sumOver, skipCounter);
                }                    
            }
            if (d != null)
            {
                //cc.min = Math.Min(cc.min, (double)d);
                //cc.max = Math.Max(cc.max, (double)d);
                //if (type != EPrintTypes.Plot)
                //{
                    double dd = dd = (double)d;
                    if (double.IsNaN((double)d))
                    {
                        if ((tsWork != null && tsWork.isNotFoundArraySubSeries == ESeriesMissing.M) || (tsRef != null && tsRef.isNotFoundArraySubSeries == ESeriesMissing.M))
                        {
                            dd = Globals.missingVariableArtificialNumber;
                        }
                    }
                    else if ((double)d == 0d)
                    {
                        if ((tsWork != null && tsWork.isNotFoundArraySubSeries == ESeriesMissing.Zero) || (tsRef != null && tsRef.isNotFoundArraySubSeries == ESeriesMissing.Zero))
                        {
                            dd = Globals.missingVariableZero;
                        }
                    }
                    table.SetNumber(i, j, dd, format);
                //}
                //else
                //{
                //    double tt = ((ScalarVal)Functions.helper_time(t)).val;
                //    if (freqColumn == EFreq.U || freqColumn == EFreq.A) tt += 0.5;
                //    table.SetNumber(i - 1, (j - 2) + 1, tt, format);                 //j=2 -> 1, j=3 -> 2
                //    table.SetNumber(i - 1, (j - 2) + 1 + count, (double)d, format);  //j=2 -> 1+count, j=3 -> 2+count
                //}
            }

            if (t.freq == EFreq.U) skipCounter[0] = 0;
            else if (t.freq == EFreq.A) skipCounter[1] = 0;
            else if (t.freq == EFreq.Q) skipCounter[2] = 0;
            else if (t.freq == EFreq.M) skipCounter[3] = 0;
        }

        public static void PrintPrepareColumn(EPrintTypes type, List<O.Prt.Element> containerExplode, int j, out O.Prt.Element cc, out string operator2, out List<string> label, out string format, out EFreq freqColumn, out double scalarValueWork, out Series tsWork, out double scalarValueRef, out Series tsRef)
        {
            cc = null;
            IVariable ivWork = null;
            IVariable ivRef = null;
            operator2 = null;
            label = new List<string> { "" };
            format = null;
            if (j - 2 >= 0)
            {
                cc = containerExplode[j - 2];
                ivWork = cc.variable[0];
                ivRef = cc.variable[1];
                operator2 = cc.operatorFinal;
                label = cc.labelOLD;
                format = "f" + cc.widthFinal + "." + cc.decFinal;
            }

            int bankCombi = GetBankCombi(operator2);

            freqColumn = EFreq.None;
            if (j > 1)
            {
                if (bankCombi == 0)
                {
                    if (ivWork.Type() == EVariableType.Series)
                    {
                        freqColumn = ((Series)ivWork).freq;
                    }
                    else
                    {
                        freqColumn = Program.options.freq;
                    }
                }
                else if (bankCombi == 1)
                {
                    if (ivRef.Type() == EVariableType.Series)
                    {
                        freqColumn = ((Series)ivRef).freq;
                    }
                    else
                    {
                        freqColumn = Program.options.freq;
                    }
                }
                else if (bankCombi == 2)
                {
                    if (ivWork.Type() == EVariableType.Series)
                    {
                        freqColumn = ((Series)ivWork).freq;
                    }
                    else
                    {
                        freqColumn = Program.options.freq;
                    }
                }
            }

            scalarValueWork = double.NaN;
            tsWork = null;
            if (ivWork != null)
            {
                tsWork = ivWork as Series;  //remember that the first col has phoney null IVariable
                if (tsWork == null) scalarValueWork = ivWork.GetVal(GekkoTime.tNull);
            }
            scalarValueRef = double.NaN;
            tsRef = null;
            if (ivRef != null)
            {
                tsRef = ivRef as Series;  //remember that the first col has phoney null IVariable
                if (tsRef == null) scalarValueRef = ivRef.GetVal(GekkoTime.tNull);
            }
        }

        public static double PrintHelperTransform(GekkoSmpl smpl, Series tsWork, Series tsRef, GekkoTime t, string operator2, bool logTransform, GekkoTime index, EPrtCollapseTypes collapse, int sumOver, int[] skipCounter)
        {
            //TODO filter and skip, see below
            double var1 = double.NaN;
            double varPch = double.NaN;
            Program.ComputeValueForPrintPlotNew(out var1, out varPch, operator2, t, tsWork, tsRef, logTransform, index, false, collapse, sumOver);
            return var1;
        }

        public static double PrintHelperTransformScalar(double scalarWork, double scalarRef, string operator2, bool logTransform, GekkoTime index, EPrtCollapseTypes collapse, int sumOver, int[] skipCounter)
        {           

            if (logTransform)
            {
                scalarWork = Math.Log(scalarWork);
                scalarRef = Math.Log(scalarRef);
            }

            if (collapse == EPrtCollapseTypes.Total && sumOver != 1)
            {
                scalarWork = sumOver * scalarWork;
                scalarRef = sumOver * scalarRef;
            }

            if (G.Equal(operator2, "n")) return scalarWork;
            else if (G.Equal(operator2, "q")) return (scalarWork / scalarRef - 1d) * 100d;
            else if (G.Equal(operator2, "m")) return scalarWork - scalarRef;
            else if (G.Equal(operator2, "d")) return 0d;
            else if (G.Equal(operator2, "p")) return 0d;
            else if (G.Equal(operator2, "dp")) return 0d;
            else
            {
                new Error("Transformation error"); return double.NaN;
            }
        }


        private static bool PrintShouldKeep(AllFreqsHelper allFreqs, EFreq freqHere, int year, int subHere)
        {
            GekkoTime tHere = new GekkoTime(freqHere, year, subHere);
            GekkoSmplSimple smplSimple = allFreqs.GetPeriods(freqHere);
            if (tHere.LargerThanOrEqual(smplSimple.t1) && tHere.SmallerThanOrEqual(smplSimple.t2)) return true;
            return false;
        }

        public static List<string> OPrintLabels(List<string> labelGiven, List<O.RecordedPieces> labelRecordedPieces, int n, int i)
        {
            if (labelGiven.Count > 1)
            {
                //this is the case for an array-series that has been unfolded
                return labelGiven;
            }

            List<string> lbl = new List<string>();  //this must end up with as many strings as the element has subelements (sublist)

            //for prt {#i}{#j} 

            //n is the number of subelements for the prtElement (for example if the item is a list like {#m}).

            string[] w = RemoveSplitter(labelGiven[0]).Split('|');  //raw label   

            if (labelRecordedPieces.Count == 0)
            {
                lbl.Add(G.ReplaceGlueSymbols(w[0]));
                return lbl;
            }

            // ===========================================================

            //n is the number of elements in the prtElements list
            //For PRT x[#i], it would be the number of elements in #i.

            //For each of these, there mayb be a <q m p> option

            //label is only added if --> counter % nn == nn - 1

            int nn = labelRecordedPieces.Count / n;  //how many inserts like <q m p> per column
            if (labelRecordedPieces.Count % n != 0)
            {
                Mismatch(); //only shown on TT computer
            }

            if (Globals.fixWildcardLabel && labelRecordedPieces.Count > 0 && labelRecordedPieces[0].s == Globals.wildcardText)  //just testing first one
            {
                foreach (O.RecordedPieces r in labelRecordedPieces)
                {
                    lbl.Add(r.iv.ConvertToString());
                }
            }
            else
            {

                //
                // for instance PRT <n p> x[#i], #i = a, b, c.
                // 
                // here n = 3, and we expect 3 recorded pieces for simple x[#i], could be 6 if it was x[#i]/y[#i]
                // the option <n p> does not produce more recorded pieces, <n p n p n p> would not require more
                // calculations, just after-processiong. If there is a <m> or <q> only the first one (for First
                // bank) will be run.
                //
                // hence nn will most likely count how many pieces are inserted for each printed variable (may
                // be a array element), for instance 1 for x[#i], 2 for x[#i]/y[#i], etc.
                //
                // counter counts recordedPieces and runs from 0 ... 2 inclusive.
                //
                // 0  'a'      0 % 1 == 0
                // 1  'b'      1 % 1 == 0
                // 2  'c'      2 % 1 == 0

                // 0  'a'      0 % 2 == 0
                // 1  'b'      1 % 2 == 0
                // 2  'c'      2 % 2 == 0
                // 3  'a'      0 % 2 == 0
                // 4  'b'      1 % 2 == 0
                // 5  'c'      2 % 2 == 0

                //
                //

                // ===========================================================


                string[] result = new string[w[0].Length];
                int ci = 0;
                foreach (char c in w[0])
                {
                    result[ci] = c.ToString();
                    ci++;
                }

                string tmp = w[1];
                string[] w2 = tmp.Substring(1, tmp.Length - 2).Split(',');

                string[] w3 = w2[3].Split(':');
                int i1 = int.Parse(w3[0]);
                int i2 = int.Parse(w3[1]);

                //result is the raw label, char by char
                //the indexes i1 and i2 show the line and pos in the input file
                //now we are going to insert items from RecordLabel() into this result string.

                //foreach recorded call of {} or [], via RecordLabel()

                int counter = -1;
                foreach (O.RecordedPieces piece in labelRecordedPieces)  //foreach RecordLabel()
                {
                    counter++;
                    string[] ss = piece.s.Split('|');
                    int length = 0;
                    length = ss[0].Length;

                    string s2 = ss[1].Substring(1, ss[1].Length - 2);  //remove [ and ]                            
                    string[] sss = s2.Split(',');

                    string[] w4 = sss[3].Split(':');
                    int ii1 = int.Parse(w4[0]);
                    int ii2 = int.Parse(w4[1]);

                    bool skip = false;
                    if (i1 != ii1)
                    {
                        //just skip it
                        //TODO: what about multiline PRT expressions??
                        skip = true;
                    }
                    
                    if (piece.iv.Type() == EVariableType.String)
                    {
                        //good
                    }
                    else
                    {
                        skip = true;
                    }

                    if (!skip)
                    {

                        int offset = ii2 - i2;
                        string xx = piece.iv.ConvertToString();

                        result[offset] = xx;
                        for (int ii = offset + 1; ii < offset + length; ii++)
                        {
                            result[ii] = null;
                        }
                        if (result[offset - 1] == "{" && result[offset + length] == "}")
                        {
                            result[offset - 1] = null;
                            result[offset + length] = null;
                        }
                    }

                    string u = null;
                    foreach (string s5 in result)
                    {
                        if (s5 == null) continue;
                        string s6 = s5;
                        if (s5.Trim() != "") s6 = s5.Trim();  //just safety
                        u = u + s6;
                    }
                    string result2 = G.ReplaceGlueSymbols(u);

                    // ----------------------------------------------
                    // NOTE: The string[] result is outside the loop of recorded pieces
                    //       For each piece in the loop, something is put into the
                    //       result array. If there are 3 pieces for each label,
                    //       piece0, piece1 and piece2 should be put into the label
                    //       and after piece2 is done, it should be added to List<string> lbl.
                    //       That is the reason of counter % nn == nn - 1, in this case nn = 3,
                    //       so we need counter % 3 == 2, that is counter 
                    // ----------------------------------------------

                    if (counter % nn == nn - 1)
                    {
                        lbl.Add(result2);
                    }
                }
            }

            return lbl;
        }

        private static int PrintCreateLabelsArrayNew(string label, int width, int numberOfLabelsRowsMax, int maxLength, out List<string> labelsArray)
        {
            //labelsArray = new string[numberOfLabelsRowsMax];

            labelsArray = new List<string>(new string[numberOfLabelsRowsMax]);

            int numberOfLabelsRows = -12345;

            //string label = graphVarsLabels[j];
            string ss = label;
            ss = Program.TruncateTextWithDots(maxLength, ss);

            if (numberOfLabelsRowsMax == 1)  //for some reason, this case must be treated specially... (not sure why)
            {
                numberOfLabelsRows = 1;
                labelsArray[0] = ss;
            }
            else
            {
                for (int i = 0; i < numberOfLabelsRowsMax; i++)
                {
                    //chopping off text backwards
                    if (ss == null) continue;
                    int start = ss.Length - width;
                    string s = "";
                    if (start >= 0)
                    {
                        s = ss.Substring(start);
                        ss = ss.Substring(0, ss.Length - s.Length);  //cropping s off
                    }
                    else
                    {
                        if (ss.Length > 0) s = ss;
                        ss = "[[]]";
                    }
                    labelsArray[i] = s;
                    if (labelsArray[i] != "" && labelsArray[i] != "[[]]")
                    {
                        if (i + 1 > numberOfLabelsRows) numberOfLabelsRows = i + 1;
                    }
                }
            }

            return numberOfLabelsRows;
        }

        public static string RemoveSplitter(string s)
        {
            string lbl;
            string[] ss = s.Split(new string[] { Globals.freelists }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length > 1)
            {
                lbl = ss[1];
            }
            else
            {
                lbl = s;
            }

            return lbl;
        }


        public static EPrintTypes GetPrintType(O.Prt o)
        {
            EPrintTypes type = EPrintTypes.Print;
            if (G.Equal(o.prtType, "plot")) type = EPrintTypes.Plot;
            else if (G.Equal(o.prtType, "sheet")) type = EPrintTypes.Sheet;
            else if (G.Equal(o.prtType, "clip")) type = EPrintTypes.Clip;
            return type;
        }


        public static bool IsGmulprt(O.Prt o, EPrintTypes type)
        {
            return type == EPrintTypes.Print && (G.Equal(o.prtType, "gmulprt") || (o.operators.Count == 1 && G.Equal(o.operators[0].s1, "v") && G.Equal(o.operators[0].s2, "yes")));
        }

        public static bool IsLevelOperator(string operator2)
        {
            return operator2 == "" || operator2 == "n" || operator2 == "d" || operator2 == Globals.operator_r || operator2 == Globals.operator_rn || operator2 == Globals.operator_rd || operator2 == "m" || operator2 == Globals.operator_l || operator2 == Globals.operator_rl;
        }

        private static int GetBankCombi(string operator2)
        {
            int bankCombi = -12345;
            List<int> bankNumbers = O.Prt.GetBankNumbers(null, new List<string> { operator2 });
            if (bankNumbers.Contains(0) && !bankNumbers.Contains(1)) bankCombi = 0;
            else if (bankNumbers.Contains(1) && !bankNumbers.Contains(0)) bankCombi = 1;
            else bankCombi = 2;
            return bankCombi;
        }

        public static bool SkipSubSeries(IVariable x)
        {
            return (x.Type() == EVariableType.Series && ((Series)x).isNotFoundArraySubSeries == ESeriesMissing.Skip);
        }


        private static void Mismatch()
        {
            if (Globals.runningOnTTComputer)
            {
                new Error("Mismatch (only TT computer)", false);
            }
        }

        private static EPrtCollapseTypes GetCollapseType(O.Prt o, EPrintTypes type)
        {
            EPrtCollapseTypes collapse = EPrtCollapseTypes.None;

            if (type == EPrintTypes.Print)
            {
                if (G.Equal(Program.options.print_collapse, "avg")) collapse = EPrtCollapseTypes.Avg;
                else if (G.Equal(Program.options.print_collapse, "total")) collapse = EPrtCollapseTypes.Total;
                if (G.Equal(o.opt_collapse, "avg")) collapse = EPrtCollapseTypes.Avg;  //overrides global options
                else if (G.Equal(o.opt_collapse, "total")) collapse = EPrtCollapseTypes.Total;  //overrides global options
                else if (G.Equal(o.opt_collapse, "yes"))
                {
                    if (G.Equal(Program.options.print_collapse, "none")) collapse = EPrtCollapseTypes.Total;  //default for PRT<collapse>, if no global option
                }
            }
            else if (type == EPrintTypes.Sheet)
            {
                if (G.Equal(Program.options.sheet_collapse, "avg")) collapse = EPrtCollapseTypes.Avg;
                else if (G.Equal(Program.options.sheet_collapse, "total")) collapse = EPrtCollapseTypes.Total;
                if (G.Equal(o.opt_collapse, "avg")) collapse = EPrtCollapseTypes.Avg;  //overrides global options
                else if (G.Equal(o.opt_collapse, "total")) collapse = EPrtCollapseTypes.Total;  //overrides global options
                else if (G.Equal(o.opt_collapse, "yes"))
                {
                    if (G.Equal(Program.options.sheet_collapse, "none")) collapse = EPrtCollapseTypes.Total;  //default for PRT<collapse>, if no global option
                }
            }

            return collapse;
        }

        public static bool IsMulprt(O.Prt o)
        {
            bool isMulprt = false;
            if (G.Equal(o.prtType, "mulprt")) isMulprt = true;
            if (G.Equal(o.prtType, "gmulprt")) isMulprt = true;
            return isMulprt;
        }

        private static int PutLabelIntoTable(Table table, int i, int j, List<string> label, int labelMaxLine)
        {
            {
                if (j <= 1)
                {
                    i += labelMaxLine - 1;
                }
                else
                {
                    for (int ii = 0; ii < labelMaxLine; ii++)
                    {
                        //G.Writeln2(labelMaxLine + " " + ii);
                        if (labelMaxLine - ii - 1 < 0)
                        {

                        }
                        else if (labelMaxLine - ii - 1 >= label.Count)
                        {

                        }
                        else
                        {
                            if (label[labelMaxLine - ii - 1] != "[[]]")
                            {
                                table.Set(i, j, label[labelMaxLine - ii - 1]);
                                table.SetAlign(i, j, Align.Right);
                            }
                        }
                        if (ii < labelMaxLine - 1) i++;
                    }
                }
            }

            return i;
        }

        private static void DateHack(Table table, int i, int j, EFreq freq, int year, int sub)
        {
            table.Get(i, j).date_hack = new GekkoTime(freq, year, sub);
        }


    }
}
