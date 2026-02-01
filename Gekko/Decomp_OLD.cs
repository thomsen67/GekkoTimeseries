using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gekko
{
    public class Decomp_OLD
    {

        public static DecompOutput DecompPivotToTable_OLD(GekkoSmpl smpl, GekkoTime per1, GekkoTime per2, DecompData decompDataMAINClone, DecompDatas decompDatas, string lhs, DecompOperator op, Decomp.EContribType operatorOneOf3Types, DecompOptions2 decompOptions2, Model model)
        {
            int parentI = 0;
            string format2 = Decomp.GetNumberFormat(decompOptions2);

            if (model.DecompType() == EModelType.GAMSScalar)
            {
                //Put the chosen variable "on the l
                Decomp.ENormalizeType normalize = Decomp.ENormalizeType.Lags;
                if (op.lowLevel == Decomp.ELowLevel.BothQuoAndRef)
                {
                    Decomp.DecompAdjust(per1, per2, decompOptions2, parentI, decompDataMAINClone, decompDatas, Decomp.EContribType.D, normalize, op);
                    Decomp.DecompAdjust(per1, per2, decompOptions2, parentI, decompDataMAINClone, decompDatas, Decomp.EContribType.RD, normalize, op);
                }
                else
                {
                    int deduct = 0;
                    if (op.isDoubleDifQuo || op.isDoubleDifRef) deduct = -1;
                    Decomp.DecompAdjust(per1.Add(deduct), per2, decompOptions2, parentI, decompDataMAINClone, decompDatas, operatorOneOf3Types, normalize, op);
                }
            }
            else if (model.DecompType() == EModelType.GAMSRaw || model.DecompType() == EModelType.Gekko)  //is .Gekko even relevant here??
            {
                if (!op.isRaw)
                {
                    //Old and bad method, make it disappear soon!
                    DecompNormalizeOLD(per1, per2, decompOptions2, parentI, decompDataMAINClone, operatorOneOf3Types);
                }
            }

            Decomp.DecompPivotHandleFilters(decompOptions2);

            FrameLight_OLD frame = DecompPivotCreateDataframe_OLD(smpl, per1, per2, lhs, decompDataMAINClone, decompDatas, op, operatorOneOf3Types, decompOptions2, model);

            int xlag = 0; string temp = null;
            Decomp.ConvertFromTurtleName(decompDataMAINClone.lhs, true, out temp, out xlag);
            string normalizerVariableWithIndex = null;
            if (temp != null)
            {
                normalizerVariableWithIndex = G.HandleBlanksRemove(G.Chop_RemoveBank(temp));
            }

            Decomp.DecomposeReplaceVars(decompOptions2.rows, Globals.col_t, Globals.col_variable, Globals.col_lag, Globals.col_universe, Globals.col_equ);
            Decomp.DecomposeReplaceVars(decompOptions2.cols, Globals.col_t, Globals.col_variable, Globals.col_lag, Globals.col_universe, Globals.col_equ);
            Decomp.DecomposeReplaceVars(decompOptions2.filters, Globals.col_t, Globals.col_variable, Globals.col_lag, Globals.col_universe, Globals.col_equ);

            List<string> tempRowNames = new List<string>();
            List<string> tempColNames = new List<string>();
            GekkoDictionary<string, AggContainer> agg = DecompPivotAggregate_OLD(frame, decompOptions2, normalizerVariableWithIndex, tempRowNames, tempColNames, model);

            List<string> rownames, colnames; string rownamesFirst, colnamesFirst;
            DecompPivotOrderRowsAndColumns_OLD(decompOptions2, parentI, tempRowNames, tempColNames, out rownames, out colnames, out rownamesFirst, out colnamesFirst, model);

            Table table = DecompGetTableFromAggObject_OLD(agg, op, decompOptions2, format2, rownames, colnames, rownamesFirst, colnamesFirst);

            DecompOutput decompOutput2 = null;

            DecompTablePostProcessing_OLD(table, rownames, colnames, decompOptions2, model);
            //table.PrintCellsForDebug();

            if (model.DecompType() == EModelType.GAMSScalar)
            {
                DecompTableHandleSignAndShares_OLD(table, decompOptions2);
            }

            decompOutput2 = DecompTableHandleSortAndIgnoreAndErrors_OLD(table, decompOptions2, model);


            return decompOutput2;
        }

        private static Table DecompGetTableFromAggObject_OLD(GekkoDictionary<string, AggContainer> agg, DecompOperator op, DecompOptions2 decompOptions2, string format2, List<string> rownames, List<string> colnames, string rownamesFirst, string colnamesFirst)
        {
            Table table = new Table();
            table.writeOnce = true;

            for (int i = 0; i < rownames.Count; i++)
            {
                for (int j = 0; j < colnames.Count; j++)
                {
                    string key = rownames[i] + "¤" + colnames[j];

                    AggContainer td = null;
                    agg.TryGetValue(key, out td);
                    double d = 0d;
                    double dAlternative = 0d;
                    double dLevel = 0d;
                    double dLevelLag = 0d;
                    double dLevelLag2 = 0d;
                    double dLevelRef = 0d;
                    double dLevelRefLag = 0d;
                    double dLevelRefLag2 = 0d;
                    int n = 0;
                    List<string> fullVariableNames = null;
                    string backgroundColor = "Transparent";

                    if (td != null)
                    {
                        dLevel = td.level;
                        dLevelLag = td.levelLag;
                        dLevelLag2 = td.levelLag2;
                        dLevelRef = td.levelRef;
                        dLevelRefLag = td.levelRefLag;
                        dLevelRefLag2 = td.levelRefLag2;
                        n = td.n;
                        fullVariableNames = td.fullVariableNames;
                        //backgroundColor = td.backgroundColor;

                        // ----- first start -----------------------------------------------
                        double dFirstLevel = double.NaN;
                        double dFirstLevelLag = double.NaN;
                        double dFirstLevelLag2 = double.NaN;
                        double dFirstLevelRef = double.NaN;
                        double dFirstLevelRefLag = double.NaN;
                        double dFirstLevelRefLag2 = double.NaN;
                        int dFirstN = 0;
                        List<string> dFirstFullVariableNames = null;
                        string keyFirst = null;
                        if (rownamesFirst != null) keyFirst = rownamesFirst + "¤" + colnames[j];
                        else if (colnamesFirst != null) keyFirst = rownames[i] + "¤" + colnamesFirst;
                        AggContainer tdFirst = null;
                        agg.TryGetValue(keyFirst, out tdFirst);
                        if (tdFirst != null)
                        {
                            dFirstLevel = tdFirst.level;
                            dFirstLevelLag = tdFirst.levelLag;
                            dFirstLevelLag2 = tdFirst.levelLag2;
                            dFirstLevelRef = tdFirst.levelRef;
                            dFirstLevelRefLag = tdFirst.levelRefLag;
                            dFirstLevelRefLag2 = tdFirst.levelRefLag2;
                            dFirstN = tdFirst.n;
                            dFirstFullVariableNames = tdFirst.fullVariableNames;
                        }
                        // ----- first end --------------------------------------------------

                        if (op.OperatorLower() == "n" || op.OperatorLower() == "xn")
                        {
                            d = dLevel;
                        }
                        else if (op.OperatorLower() == "rn" || op.OperatorLower() == "r" || op.OperatorLower() == "xrn" || op.OperatorLower() == "xr")
                        {
                            d = dLevelRef;
                        }
                        else if (op.OperatorLower() == "d" || op.OperatorLower() == "sd")
                        {
                            d = td.change;
                        }
                        else if (op.OperatorLower() == "p" || op.OperatorLower() == "sp")
                        {
                            d = td.change / dFirstLevelLag * 100d;
                        }
                        else if (op.OperatorLower() == "dp" || op.OperatorLower() == "sdp")
                        {
                            d = td.change / dFirstLevelLag * 100d - td.changeAlternative / dFirstLevelLag2 * 100d;
                        }
                        else if (op.OperatorLower() == "m" || op.OperatorLower() == "sm")
                        {
                            d = td.change;
                        }
                        else if (op.OperatorLower() == "q" || op.OperatorLower() == "sq")
                        {
                            d = td.change / dFirstLevelRef * 100d;
                        }
                        else if (op.OperatorLower() == "mp" || op.OperatorLower() == "smp")
                        {
                            d = td.change / dFirstLevelLag * 100d - td.changeAlternative / dFirstLevelRefLag * 100d;
                        }
                        else if (op.OperatorLower() == "xd")
                        {
                            d = dLevel - dLevelLag;
                        }
                        else if (op.OperatorLower() == "xp")
                        {
                            d = (dLevel - dLevelLag) / dLevelLag * 100d;
                        }
                        else if (op.OperatorLower() == "xdp")
                        {
                            d = (dLevel - dLevelLag) / dLevelLag * 100d - (dLevelLag - dLevelLag2) / dLevelLag2 * 100d;
                        }
                        else if (op.OperatorLower() == "xm")
                        {
                            d = dLevel - dLevelRef;
                        }
                        else if (op.OperatorLower() == "xq")
                        {
                            d = (dLevel - dLevelRef) / dLevelRef * 100d;
                        }
                        else if (op.OperatorLower() == "xmp")
                        {
                            d = (dLevel - dLevelLag) / dLevelLag * 100d - (dLevelRef - dLevelRefLag) / dLevelRefLag * 100d;
                        }
                        // -----------------
                        else if (op.OperatorLower() == "rd" || op.OperatorLower() == "srd")
                        {
                            d = td.change;
                        }
                        else if (op.OperatorLower() == "rp" || op.OperatorLower() == "srp")
                        {
                            d = td.change / dFirstLevelRefLag * 100d;
                        }
                        else if (op.OperatorLower() == "rdp" || op.OperatorLower() == "srdp")
                        {
                            d = td.change / dFirstLevelRefLag * 100d - td.changeAlternative / dFirstLevelRefLag2 * 100d;
                        }
                        else if (op.OperatorLower() == "xrd")
                        {
                            d = dLevelRef - dLevelRefLag;
                        }
                        else if (op.OperatorLower() == "xrp")
                        {
                            d = (dLevelRef - dLevelRefLag) / dLevelRefLag * 100d;
                        }
                        else if (op.OperatorLower() == "xrdp")
                        {
                            d = (dLevelRef - dLevelRefLag) / dLevelRefLag * 100d - (dLevelRefLag - dLevelRefLag2) / dLevelRefLag2 * 100d;
                        }
                    }

                    if (decompOptions2.count == ECountType.N)
                    {
                        table.SetNumber(i + 2, j + 2, n, "f16.0");
                    }
                    else if (decompOptions2.count == ECountType.Names)
                    {
                        string tmp2 = null;
                        if (fullVariableNames != null)
                        {
                            List<string> tmp = new List<string>();
                            foreach (string s in fullVariableNames) tmp.Add(s.Replace("¤", "").Replace(Globals.decompResidualName, Globals.decompResidualName2)); //x[a]¤[-1] --> x[a][-1]
                            tmp2 = Stringlist.GetListWithCommas(tmp).Replace(", ", ",  ");  //a, b --> a,  b.
                        }
                        else
                        {
                            tmp2 = Decomp.Text1(0);
                        }
                        table.Set(i + 2, j + 2, tmp2);
                    }
                    else
                    {
                        table.SetNumber(i + 2, j + 2, d, format2);
                    }

                    Cell c = table.Get(i + 2, j + 2);
                    c.vars_hack = fullVariableNames;
                    c.value_hack = d;  //stored for sort and ignore later on
                    c.backgroundColor = backgroundColor;
                }
            }
            return table;
        }

        /// <summary>
        /// In order for some name bits like &lt;null> to show up first, some tricks were applied when sorting in a previous method. These tricks are resolved here.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="rownames"></param>
        /// <param name="colnames"></param>
        /// <param name="decompOptions2"></param>
        private static void DecompTablePostProcessing_OLD(Table tab, List<string> rownames, List<string> colnames, DecompOptions2 decompOptions2, Model model)
        {
            Decomp.ERowsCols rowsCols = Decomp.VariablesOnRowsOrCols(decompOptions2);
            if (!Decomp.VarsAndTimeDimensionsAreSeparate(decompOptions2)) rowsCols = Decomp.ERowsCols.None;

            if (decompOptions2.decompOperator.isPercentageType || decompOptions2.isShares)
            {
                tab.Set(1, 1, "%" + "  ");
            }
            else
            {
                tab.Set(1, 1, "");
            }

            for (int i = 0; i < rownames.Count; i++)
            {
                string s = rownames[i];
                if (model.DecompType() == EModelType.GAMSScalar)
                {
                    if (s != null) s = s.Replace(Globals.pivotHelper1, "").Replace(Globals.pivotHelper2, "").Replace(Globals.decompResidualName, Globals.decompResidualName2);
                }
                tab.Set(i + 2, 1, s);
                if (rowsCols == Decomp.ERowsCols.Cols) tab.Get(i + 2, 1).date_hack = GekkoTime.FromStringToGekkoTime(s, false, false);
            }

            for (int j = 0; j < colnames.Count; j++)
            {
                string s = colnames[j];
                if (model.DecompType() == EModelType.GAMSScalar)
                {
                    if (s != null) s = s.Replace(Globals.pivotHelper1, "").Replace(Globals.pivotHelper2, "").Replace(Globals.decompResidualName, Globals.decompResidualName2); ;
                }
                tab.Set(1, j + 2, s);
                if (rowsCols == Decomp.ERowsCols.Rows) tab.Get(1, j + 2).date_hack = GekkoTime.FromStringToGekkoTime(s, false, false);
            }
        }

        /// <summary>
        /// Reorder rows and cols names according to alphabetical and numerical sorting (intelligent though, chopping up into tokens). Will also put the chosen variable first.
        /// </summary>
        /// <param name="decompOptions2"></param>
        /// <param name="parentI"></param>
        /// <param name="rownamesInput"></param>
        /// <param name="colnamesInput"></param>
        /// <param name="rownames"></param>
        /// <param name="colnames"></param>
        /// <param name="rownamesFirst"></param>
        /// <param name="colnamesFirst"></param>
        private static void DecompPivotOrderRowsAndColumns_OLD(DecompOptions2 decompOptions2, int parentI, List<string> rownamesInput, List<string> colnamesInput, out List<string> rownames, out List<string> colnames, out string rownamesFirst, out string colnamesFirst, Model model)
        {
            Decomp.ERowsCols rowsOrCols = Decomp.VariablesOnRowsOrCols(decompOptions2);

            //------------------------------------------------
            //ROWS -------------------------------------------
            //------------------------------------------------

            for (int i = 0; i < rownamesInput.Count; i++)
            {
                if (rownamesInput[i] != null) rownamesInput[i] = rownamesInput[i].Replace(Globals.decompNull, Globals.decompNullName);
            }
            List<string> rownamesTemp = new List<string>();

            foreach (var rowname in rownamesInput.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default)))
            {
                rownamesTemp.Add(rowname);
            }

            rownamesInput = rownamesTemp;
            for (int i = 0; i < rownamesInput.Count; i++)
            {
                if (rownamesInput[i] != null) rownamesInput[i] = rownamesInput[i].Replace(Globals.decompNullName, Globals.decompNull);
            }

            //------------------------------------------------
            //COLS -------------------------------------------
            //------------------------------------------------

            for (int i = 0; i < colnamesInput.Count; i++)
            {
                if (colnamesInput[i] != null) colnamesInput[i] = colnamesInput[i].Replace(Globals.decompNull, Globals.decompNullName);
            }
            List<string> colnamesTemp = new List<string>();

            foreach (var colname in colnamesInput.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default)))
            {
                colnamesTemp.Add(colname);
            }

            colnamesInput = colnamesTemp;
            for (int i = 0; i < colnamesInput.Count; i++)
            {
                if (colnamesInput[i] != null) colnamesInput[i] = colnamesInput[i].Replace(Globals.decompNullName, Globals.decompNull);
            }

            // --------------------------------------------------------------------
            // Handle first row/col, putting the selected variable there
            // --------------------------------------------------------------------

            string varnames = decompOptions2.link[parentI].varnames;
            bool orderNormalize = true;

            rownames = new List<string>();
            colnames = new List<string>();
            rownamesFirst = null;
            for (int i = 0; i < rownamesInput.Count; i++)
            {
                bool b1 = rownamesFirst == null && orderNormalize && Decomp.DecompMatchWord(rownamesInput[i], varnames);
                bool b2 = rownamesFirst == null && orderNormalize && (rownamesInput[i] != null && rownamesInput[i].Contains(Globals.pivotHelper2));
                if ((model.DecompType() != EModelType.GAMSScalar && b1) || (model.DecompType() == EModelType.GAMSScalar && b2))
                {
                    rownamesFirst = rownamesInput[i];
                }
                else
                {
                    rownames.Add(rownamesInput[i]);
                }
            }

            colnamesFirst = null;
            for (int i = 0; i < colnamesInput.Count; i++)
            {
                bool b1 = colnamesFirst == null && orderNormalize && Decomp.DecompMatchWord(colnamesInput[i], varnames);
                bool b2 = colnamesFirst == null && orderNormalize && (colnamesInput[i] != null && colnamesInput[i].Contains(Globals.pivotHelper2));
                if ((model.DecompType() != EModelType.GAMSScalar && b1) || (model.DecompType() == EModelType.GAMSScalar && b2))
                {
                    colnamesFirst = colnamesInput[i];
                }
                else
                {
                    colnames.Add(colnamesInput[i]);
                }
            }

            if (rownamesFirst != null) rownames.Insert(0, rownamesFirst);
            if (colnamesFirst != null) colnames.Insert(0, colnamesFirst);

            if (orderNormalize && rownamesFirst == null && colnamesFirst == null)
            {
                new Error("Pivot table problem regarding first row/col (dependent variable).");
            }
        }

        /// <summary>
        /// Aggregate the dataframe (with data rows and field cols) into a rows/cols pivot table for showing.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="normalizerVariableWithIndex"></param>
        /// <param name="tempRowNames"></param>
        /// <param name="tempColNames"></param>
        /// <returns></returns>
        private static GekkoDictionary<string, AggContainer> DecompPivotAggregate_OLD(FrameLight_OLD frame, DecompOptions2 decompOptions2, string normalizerVariableWithIndex, List<string> tempRowNames, List<string> tempColNames, Model model)
        {
            // ==============================================================================
            //Aggregation
            //Aggregation
            //Aggregation into table suitable for showing
            //Aggregation
            //Aggregation
            //
            // decompOptions2.all receives sets at the end, for instance "#i"
            // decompOptions2.freeValues has a Dict for each frame.colnames.Count (for instance 15)
            //
            //
            // ==============================================================================

            decompOptions2.all.Clear();
            foreach (string s in frame.frameColNames)
            {
                decompOptions2.all.Add(s);
            }

            GekkoDictionary<string, AggContainer> agg = new GekkoDictionary<string, AggContainer>(StringComparer.OrdinalIgnoreCase);
            int valueI = FrameLightRow.FindColumn(frame, "value");

            bool getFreeValues = Decomp.DecompPivotAggregateGetFreeValues(frame, decompOptions2);

            foreach (FrameLightRow framerow in frame.frameRows)
            {
                Decomp.ENormalizerType normalizerType = Decomp.ENormalizerType.None;

                if (G.Equal(normalizerVariableWithIndex, framerow.Get(frame, Globals.col_fullVariableName).text))
                {
                    if (framerow.Get(frame, Globals.col_lag).text == "[0]") normalizerType = Decomp.ENormalizerType.Normalizer;
                    else normalizerType = Decomp.ENormalizerType.NormalizerWithLagOrLead;
                }

                if (getFreeValues)
                {
                    for (int i = 0; i < frame.frameColNames.Count; i++)
                    {
                        if (i == valueI) continue;
                        string s = framerow.storageDimensions[i].text;
                        if (s == null) s = Globals.decompNull;  //hmmm used at all??
                        if (!decompOptions2.freeValues[i].ContainsKey(s)) decompOptions2.freeValues[i].Add(s, null);
                    }
                }

                bool skip = false;
                foreach (FrameFilter filter in decompOptions2.filters)
                {
                    CellLight c = framerow.Get(frame, filter.name);
                    if (c.type != ECellLightType.String && c.type != ECellLightType.None) throw new GekkoException();
                    string ss = c.text;
                    if (c.type == ECellLightType.None || !filter.selected.Contains(ss, StringComparer.OrdinalIgnoreCase))
                    {
                        //not part of the filter, is ignored
                        //if the row has a null value regarding the filter, the row is also ignored (for instance, if #a must be 18 and the row has #a = null, the row is ignored)
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;

                string more = null;
                if (model.DecompType() == EModelType.GAMSScalar)
                {
                    if (normalizerType == Decomp.ENormalizerType.NormalizerWithLagOrLead) more = Globals.pivotHelper1;  //so that it is set apart
                    else if (normalizerType == Decomp.ENormalizerType.Normalizer) more = Globals.pivotHelper2;
                }

                string s1 = null;
                foreach (string s in decompOptions2.rows)
                {
                    s1 = Decomp.DecompAddText(frame, framerow, s1, s);
                    if (s == Globals.col_variable) s1 += more;
                }
                if (s1 != null)
                {
                    s1 = s1.Substring(Globals.pivotTableDelimiter.Length);
                }

                string s2 = null;
                foreach (string s in decompOptions2.cols)
                {
                    s2 = Decomp.DecompAddText(frame, framerow, s2, s);
                    if (s == Globals.col_variable) s2 += more;
                }
                if (s2 != null)
                {
                    s2 = s2.Substring(Globals.pivotTableDelimiter.Length);
                }
                string key = s1 + "¤" + s2;  //row ¤ col

                double d = framerow.Get(frame, Globals.col_value).data;
                double dAlternative = framerow.Get(frame, Globals.col_valueAlternative).data;
                double dLevel = framerow.Get(frame, Globals.col_valueLevel).data;
                double dLevelLag = framerow.Get(frame, Globals.col_valueLevelLag).data;
                double dLevelLag2 = framerow.Get(frame, Globals.col_valueLevelLag2).data;
                double dLevelRef = framerow.Get(frame, Globals.col_valueLevelRef).data;
                double dLevelRefLag = framerow.Get(frame, Globals.col_valueLevelRefLag).data;
                double dLevelRefLag2 = framerow.Get(frame, Globals.col_valueLevelRefLag2).data;
                string fullVariableName = framerow.Get(frame, Globals.col_fullVariableName).text;

                string backgroundColor = "Transparent";
                if (Decomp.IsDecompResidualName(fullVariableName)) backgroundColor = Globals.decompResidualColor;

                if (!decompOptions2.showErrors && Decomp.IsDecompResidualName(fullVariableName))
                {
                    //skip residuals if errors are not shown
                }
                else
                {
                    if (!tempRowNames.Contains(s1, StringComparer.OrdinalIgnoreCase)) tempRowNames.Add(s1);
                    if (!tempColNames.Contains(s2, StringComparer.OrdinalIgnoreCase)) tempColNames.Add(s2);
                    AggContainer td = null;
                    agg.TryGetValue(key, out td);
                    if (td == null)
                    {
                        agg.Add(key, new AggContainer(d, dAlternative, dLevel, dLevelLag, dLevelLag2, dLevelRef, dLevelRefLag, dLevelRefLag2, 1, new List<string>() { fullVariableName }, 0d, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN));
                    }
                    else
                    {
                        td.change += d;
                        td.changeAlternative += dAlternative;
                        td.level += dLevel;
                        td.levelLag += dLevelLag;
                        td.levelLag2 += dLevelLag2;
                        td.levelRef += dLevelRef;
                        td.levelRefLag += dLevelRefLag;
                        td.levelRefLag2 += dLevelRefLag2;
                        td.n += 1;
                        //BEWARE
                        //BEWARE
                        //BEWARE Is this too time-consuming?
                        //BEWARE
                        //BEWARE
                        td.fullVariableNames.Add(fullVariableName);
                        //if (backgroundColor != "Transparent") td.backgroundColor = backgroundColor;
                    }
                }
            }
            return agg;
        }

        private static FrameLight_OLD DecompPivotCreateDataframe_OLD(GekkoSmpl smpl, GekkoTime per1, GekkoTime per2, string lhs, DecompData decompDataMAINClone, DecompDatas decompDatas, DecompOperator op, Decomp.EContribType operatorOneOf3Types, DecompOptions2 decompOptions2, Model model)
        {
            int superN = 1;

            //The DataTable dt will get the following colums:
            //time         time
            //vars         variable name, like x or npop
            //lags:        lag or lead, [0] for none
            //<#universe>  universal set for elements without domain info (corresponds to x[*] in contrast to x[#i])
            //#i           set names, like #age, #sector, etc.
            //x#1, x#2:    dimension of x           
            //value        data value

            FrameLight_OLD frame = new FrameLight_OLD();
            frame.AddColName(Globals.col_t);
            frame.AddColName(Globals.col_value);
            frame.AddColName(Globals.col_valueAlternative);
            frame.AddColName(Globals.col_valueLevel);
            frame.AddColName(Globals.col_valueLevelLag);
            frame.AddColName(Globals.col_valueLevelLag2);
            frame.AddColName(Globals.col_valueLevelRef);
            frame.AddColName(Globals.col_valueLevelRefLag);
            frame.AddColName(Globals.col_valueLevelRefLag2);
            frame.AddColName(Globals.col_variable);
            frame.AddColName(Globals.col_lag);
            frame.AddColName(Globals.col_universe);
            frame.AddColName(Globals.col_equ);
            frame.AddColName(Globals.col_fullVariableName);            

            //adding frame rows, while also getting sets defined as frame columns

            for (int super = 0; super < superN; super++)  //Gekko 4.0: remove stuff with > 1 variable explained. Also equationnumber.
                                                          //It was a misconception to make it possible to analyze y[#a] = f(x[#i]) as LSH. If that needs to be done,
                                                          //analyze sum(#a, f(x[#i]) instead. Much more logical.
                                                          //Normally super = 0. Equations like if y[#a] = x[#a] + 5, superN will correspond to number of elements in #a.
            {
                int j = 0;
                foreach (GekkoTime t2 in new GekkoTimeIterator(per1, per2))
                {
                    j++;
                    int i = 0;
                    double lhsSum = 0d;
                    double rhsSum = 0d;

                    //second time, no loop..........

                    DecompDict dd = null;
                    if (op.isRaw)
                    {
                        //data is not used from here, it is just to get the list of
                        //relevant variables. For multiplier type, both if's are true,
                        //and in that case we just use the first.
                        dd = decompDataMAINClone.cellsQuo;
                        if (op.lowLevel == Decomp.ELowLevel.OnlyRef) dd = decompDataMAINClone.cellsRef;
                    }
                    else
                    {
                        if (op.lowLevel == Decomp.ELowLevel.BothQuoAndRef)
                        {
                            dd = Decomp.GetDecompDatas(decompDataMAINClone, Decomp.EContribType.D);  //could just as well be .RD, we are only using the keys
                        }
                        else
                        {
                            dd = Decomp.GetDecompDatas(decompDataMAINClone, operatorOneOf3Types);
                        }
                    }

                    foreach (string dictName in dd.storage.Keys)
                    {
                        i++;

                        string dbName = null; string varName = null; string freq = null; string[] indexes = null;
                        string[] domains = null;

                        //See #876435924365

                        string lag = null;

                        //there is some repeated work done here, but not really bad
                        //problem is we prefer to do one period at a time, to sum up, adjust etc.

                        string[] ss = dictName.Split('¤');
                        string fullName = ss[0];
                        lag = ss[1];
                        string lag2 = lag;  //lag2 keeps [0], lag has null for this.
                        if (lag == "[0]")
                        {
                            lag = null;
                        }
                        int iLag = int.Parse(lag2.Substring(1, lag2.Length - 2));

                        char firstChar;
                        O.Chop(fullName, out dbName, out varName, out freq, out indexes);

                        if (indexes != null) domains = new string[indexes.Length];

                        if (domains != null)
                        {
                            //Adding domain info. We may have x[18, gov] which is part of x[#a, #sector].
                            //So in this case, #a and #sector would be added as columns
                            IVariable iv = O.GetIVariableFromString(fullName, O.ECreatePossibilities.NoneReturnNullAlways);
                            if (iv != null)
                            {
                                Series ts = iv as Series;
                                if (ts?.mmi?.parent?.meta?.domains != null)
                                {
                                    for (int ii = 0; ii < ts.mmi.parent.meta.domains.Length; ii++)
                                    {
                                        domains[ii] = Decomp.ConvertSetname(ts.mmi.parent.meta.domains[ii]);
                                    }
                                }
                            }

                            foreach (string domain in domains)
                            {
                                if (domain != null)
                                {
                                    string setname = domain.ToLower();
                                    if (setname == null) setname = Globals.col_universe; //corresonds to x[*]
                                    frame.AddColName(setname);  //will .tolower() and ignore dublets
                                }
                            }
                        }

                        //See #876435924365              
                        string bank2 = dbName;
                        if (G.Equal(Decomp.DecompFirst(), dbName)) bank2 = null;
                        string name2 = O.UnChop(null, varName, null, indexes);

                        double dLevel = double.NaN;
                        double dLevelLag = double.NaN;
                        double dLevelLag2 = double.NaN;
                        double dLevelRef = double.NaN;
                        double dLevelRefLag = double.NaN;
                        double dLevelRefLag2 = double.NaN;

                        if (Decomp.IsDecompResidualName(dictName))
                        {
                            Tuple<Series, Series> tup = Decomp.GetRealTimeseries(decompDatas, dictName);
                            if (tup.Item1 != null)
                            {
                                dLevel = tup.Item1.GetDataSimple(t2);
                                dLevelLag = tup.Item1.GetDataSimple(t2.Add(-1));
                                dLevelLag2 = tup.Item1.GetDataSimple(t2.Add(-2));
                            }
                            if (tup.Item2 != null)
                            {
                                dLevelRef = tup.Item2.GetDataSimple(t2);
                                dLevelRefLag = tup.Item2.GetDataSimple(t2.Add(-1));
                                dLevelRefLag2 = tup.Item2.GetDataSimple(t2.Add(-2));
                            }
                        }
                        else
                        {
                            //MAybe turn this off for x-type...
                            //a little bit of waste here, if not both series are needed for non-x decomp. But penalty must be really small.
                            //Tuple<Series, Series> tup = GetRealTimeseries(decompDatas, dictName);

                            string fullNameRef = G.Chop_SetBank(fullName, "Ref");

                            if (op.isRaw)
                            {
                                Series tsFirst = O.GetIVariableFromString(fullName, O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                                if (tsFirst != null)
                                {
                                    dLevel = tsFirst.GetDataSimple(t2.Add(iLag));
                                    dLevelLag = tsFirst.GetDataSimple(t2.Add(-1 + iLag));
                                    dLevelLag2 = tsFirst.GetDataSimple(t2.Add(-2 + iLag));
                                }
                                Series tsRef = O.GetIVariableFromString(fullNameRef, O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                                if (tsRef != null)
                                {
                                    dLevelRef = tsRef.GetDataSimple(t2.Add(iLag));
                                    dLevelRefLag = tsRef.GetDataSimple(t2.Add(-1 + iLag));
                                    dLevelRefLag2 = tsRef.GetDataSimple(t2.Add(-2 + iLag));
                                }
                            }
                            else
                            {
                                if (operatorOneOf3Types == Decomp.EContribType.N || operatorOneOf3Types == Decomp.EContribType.M || operatorOneOf3Types == Decomp.EContribType.D)
                                {
                                    Series tsFirst = null;
                                    tsFirst = O.GetIVariableFromString(fullName, O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                                    if (tsFirst == null)
                                    {
                                        string s2 = fullName.Replace("¤", "");
                                        new Error("Could not find variable " + s2 + "");
                                    }
                                    dLevel = tsFirst.GetDataSimple(t2.Add(iLag));
                                    dLevelLag = tsFirst.GetDataSimple(t2.Add(-1 + iLag));
                                    dLevelLag2 = tsFirst.GetDataSimple(t2.Add(-2 + iLag));
                                }

                                if (operatorOneOf3Types == Decomp.EContribType.RN || operatorOneOf3Types == Decomp.EContribType.M || operatorOneOf3Types == Decomp.EContribType.RD)
                                {
                                    Series tsRef = null;
                                    tsRef = O.GetIVariableFromString(fullNameRef, O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                                    if (tsRef == null)
                                    {
                                        string s2 = fullNameRef.Replace("¤", "");
                                        new Error("Could not find variable " + s2 + "");
                                    }
                                    dLevelRef = tsRef.GetDataSimple(t2.Add(iLag));
                                    dLevelRefLag = tsRef.GetDataSimple(t2.Add(-1 + iLag));
                                    dLevelRefLag2 = tsRef.GetDataSimple(t2.Add(-2 + iLag));
                                }
                            }
                        }

                        double d = double.NaN;
                        double dAlternative = double.NaN;
                        if (op.isDoubleDifQuo)  //dp
                        {
                            d = Decomp.DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "d", smpl, lhs, t2, dictName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                            dAlternative = Decomp.DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "d", smpl, lhs, t2.Add(-1), dictName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        }
                        else if (op.isDoubleDifRef) //rdp
                        {
                            d = Decomp.DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "rd", smpl, lhs, t2, dictName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                            dAlternative = Decomp.DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "rd", smpl, lhs, t2.Add(-1), dictName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        }
                        else if (op.lowLevel == Decomp.ELowLevel.BothQuoAndRef) //mp
                        {
                            d = Decomp.DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "d", smpl, lhs, t2, dictName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                            dAlternative = Decomp.DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "rd", smpl, lhs, t2, dictName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        }
                        else
                        {
                            d = Decomp.DecomposePutIntoTable2HelperOperators(decompDataMAINClone, op.OperatorLower(), smpl, lhs, t2, dictName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                            dAlternative = double.NaN;
                        }

                        FrameLightRow dr = new FrameLightRow(frame);
                        //dr.Set(frame, col_fullVariableName, new CellLight(G.Chop_RemoveBank(fullName)));

                        string dictName2 = dictName.Replace(Decomp.DecompFirst() + ":", "").Replace("¤[0]", "");

                        dr.Set(frame, Globals.col_fullVariableName, new CellLight(dictName2));
                        dr.Set(frame, Globals.col_equ, new CellLight(super.ToString()));
                        dr.Set(frame, Globals.col_t, new CellLight(t2.ToString()));
                        dr.Set(frame, Globals.col_variable, new CellLight(varName));

                        dr.Set(frame, Globals.col_lag, new CellLight(lag2));

                        if (indexes != null)
                        {
                            for (int ii = 0; ii < indexes.Length; ii++)
                            {
                                if (domains != null)
                                {
                                    string domain = domains[ii];
                                    string index = indexes[ii];

                                    if (domain != null)
                                    {
                                        dr.Set(frame, domain, new CellLight(index));
                                    }
                                    else
                                    {
                                        dr.Set(frame, Globals.col_universe, new CellLight(index));
                                    }
                                }
                            }
                        }

                        dr.Set(frame, Globals.col_value, new CellLight(d));
                        dr.Set(frame, Globals.col_valueAlternative, new CellLight(dAlternative));
                        dr.Set(frame, Globals.col_valueLevel, new CellLight(dLevel));
                        dr.Set(frame, Globals.col_valueLevelLag, new CellLight(dLevelLag));
                        dr.Set(frame, Globals.col_valueLevelLag2, new CellLight(dLevelLag2));
                        dr.Set(frame, Globals.col_valueLevelRef, new CellLight(dLevelRef));
                        dr.Set(frame, Globals.col_valueLevelRefLag, new CellLight(dLevelRefLag));
                        dr.Set(frame, Globals.col_valueLevelRefLag2, new CellLight(dLevelRefLag2));

                        frame.frameRows.Add(dr);
                    }
                }
            }            

            return frame;
        }

        /// <summary>
        /// Sorting and pruning. Uses .value_hack of each cell, which stores value no matter what is shown in cell.
        /// </summary>
        /// <param name="table1"></param>
        /// <param name="decompOptions2"></param>
        private static DecompOutput DecompTableHandleSortAndIgnoreAndErrors_OLD(Table table1, DecompOptions2 decompOptions2, Model model)
        {
            string numberFormat = Decomp.GetNumberFormat(decompOptions2);

            Decomp.ERowsCols rowsOrCols = Decomp.VariablesOnRowsOrCols(decompOptions2);
            if (rowsOrCols == Decomp.ERowsCols.None) return new DecompOutput(table1, null, null, null); //fast return 

            string ignore = null;
            List<double> red = new List<double>();

            // --------------------------------
            // SORT AND IGNORE START
            // --------------------------------

            List<SortHelper> sortHelperStart = new List<SortHelper>();

            if (rowsOrCols == Decomp.ERowsCols.Rows)
            {
                for (int i = 3; i <= table1.GetRowMaxNumber(); i++)  //ignore first 2 rows
                {
                    Cell c5 = table1.Get(i, 2);
                    string name2 = Decomp.GetVarsHack(c5);
                    double max = 0d;
                    for (int j = 2; j <= table1.GetColMaxNumber(); j++)
                    {
                        Cell c1 = table1.Get(i, j);
                        Cell c2 = table1.Get(2, j);
                        double d = 0d;
                        if (decompOptions2.decompOperator.isRaw) d = Math.Abs(c1.value_hack);
                        else d = Math.Abs(c1.value_hack / c2.value_hack * 100d);
                        if (!G.IsNumericalError(d)) max = Math.Max(max, d);
                    }
                    sortHelperStart.Add(new SortHelper() { position = i, value = max, name = name2 });
                }
            }
            else if (rowsOrCols == Decomp.ERowsCols.Cols)
            {
                for (int j = 3; j <= table1.GetColMaxNumber(); j++)  //ignore first two cols                 
                {
                    Cell c5 = table1.Get(2, j);
                    string name2 = Decomp.GetVarsHack(c5);
                    double max = 0d;
                    for (int i = 2; i <= table1.GetRowMaxNumber(); i++)
                    {
                        Cell c1 = table1.Get(i, j);
                        Cell c2 = table1.Get(i, 2);
                        double d = 0d;
                        if (decompOptions2.decompOperator.isRaw) d = Math.Abs(c1.value_hack);
                        else d = Math.Abs(c1.value_hack / c2.value_hack * 100d);
                        if (!G.IsNumericalError(d)) max = Math.Max(max, d);
                    }
                    sortHelperStart.Add(new SortHelper() { position = j, value = max, name = name2 });
                }
            }

            // ------------------- the following is common for rows vs cols START ------------------------

            //maybe ignore
            double ignoreSum = 0d;
            List<SortHelper> sortHelperNotIgnored = new List<SortHelper>();
            List<SortHelper> sortHelperIgnored = new List<SortHelper>();
            if (!(double.IsNaN(decompOptions2.ignore) || decompOptions2.ignore == 0d || decompOptions2.decompOperator.isRaw))
            {
                foreach (SortHelper sh in sortHelperStart)
                {
                    if (sh.value < decompOptions2.ignore)
                    {
                        sortHelperIgnored.Add(sh);
                    }
                    else
                    {
                        sortHelperNotIgnored.Add(sh);
                    }
                }
            }
            else
            {
                sortHelperNotIgnored.AddRange(sortHelperStart);
            }
            int ignoreCount = sortHelperStart.Count - sortHelperNotIgnored.Count;
            if (ignoreCount > 0)
            {
                string x = "row" + G.S(ignoreCount);
                if (rowsOrCols == Decomp.ERowsCols.Cols) x = "col" + G.S(ignoreCount);
                ignore = ignoreCount + " " + x + " ignored";
            }

            //Maybe sort
            List<SortHelper> sortHelperFinal = new List<SortHelper>();
            if (decompOptions2.sort)
            {
                sortHelperFinal.AddRange(sortHelperNotIgnored.OrderByDescending(x => x.value));
            }
            else
            {
                sortHelperFinal.AddRange(sortHelperNotIgnored);
            }

            // ------------------- the preceding is common for rows vs cols END ------------------------

            Table table2 = new Table();
            table2.writeOnce = true;
            table2.Set(new Coord(1, 1), table1.Get(1, 1));

            //fill in sorted rows/columns
            if (rowsOrCols == Decomp.ERowsCols.Rows)
            {
                //copy the first two rows 
                int two = 2;
                for (int i = 1; i <= two; i++)
                {
                    for (int j = 1; j <= table1.GetColMaxNumber(); j++)
                    {
                        table2.Set(new Coord(i, j), table1.Get(i, j));
                    }
                }
                int i1 = 2;
                foreach (SortHelper sh in sortHelperFinal)
                {
                    i1++;
                    int i2 = sh.position;
                    for (int j = 1; j <= table1.GetColMaxNumber(); j++)
                    {
                        table2.Set(new Coord(i1, j), table1.Get(i2, j));
                    }
                }
            }
            else if (rowsOrCols == Decomp.ERowsCols.Cols)
            {
                //copy the first two cols
                int two = 2;
                for (int j = 1; j <= two; j++)
                {
                    for (int i = 1; i <= table1.GetRowMaxNumber(); i++)
                    {
                        table2.Set(new Coord(i, j), table1.Get(i, j));
                    }
                }
                int j1 = 2;
                foreach (SortHelper sh in sortHelperFinal)
                {
                    j1++;
                    int j2 = sh.position;
                    for (int i = 1; i <= table1.GetRowMaxNumber(); i++)
                    {
                        table2.Set(new Coord(i, j1), table1.Get(i, j2));
                    }
                }
            }

            // --- insert ignores aggregate

            if (decompOptions2.showErrors && !decompOptions2.decompOperator.isRaw && sortHelperIgnored.Count > 0)
            {
                int rowmax = table2.GetRowMaxNumber();  //because it changes dynamically later on
                int colmax = table2.GetColMaxNumber();  //because it changes dynamically later on

                if (rowsOrCols == Decomp.ERowsCols.Rows)
                {
                    table2.Set(rowmax + 1, 1, Globals.decompIgnoreName2);
                    for (int j = 2; j <= colmax; j++)
                    {
                        double sum_hack = 0d;
                        double sum = 0d;
                        foreach (SortHelper x in sortHelperIgnored)
                        {
                            sum += table1.Get(x.position, j).number;
                            sum_hack += table1.Get(x.position, j).value_hack;
                        }

                        if (decompOptions2.count == ECountType.Names)
                        {
                            table2.Set(rowmax + 1, j, Globals.decompIgnoreName2);
                        }
                        else if (decompOptions2.count == ECountType.N)
                        {
                            table2.SetNumber(rowmax + 1, j, 1d, "f16.0");
                        }
                        else
                        {
                            table2.SetNumber(rowmax + 1, j, sum, numberFormat);
                        }

                        Cell c = table2.Get(rowmax + 1, j);
                        c.backgroundColor = Globals.decompIgnoredColor;
                        c.vars_hack = new List<string>() { Globals.decompIgnoreName };
                        c.value_hack = sum_hack;
                    }
                }
                else if (rowsOrCols == Decomp.ERowsCols.Cols)
                {
                    table2.Set(1, colmax + 1, Globals.decompIgnoreName2);
                    for (int i = 2; i <= rowmax; i++)
                    {
                        double sum_hack = 0d;
                        double sum = 0d;
                        foreach (SortHelper x in sortHelperIgnored)
                        {
                            sum += table1.Get(i, x.position).number;
                            sum_hack += table1.Get(i, x.position).value_hack;
                        }

                        if (decompOptions2.count == ECountType.Names)
                        {
                            table2.Set(i, colmax + 1, Globals.decompIgnoreName2);
                        }
                        else if (decompOptions2.count == ECountType.N)
                        {
                            table2.SetNumber(i, colmax + 1, 1d, "f16.0");
                        }
                        else
                        {
                            table2.SetNumber(i, colmax + 1, sum, numberFormat);
                        }

                        Cell c = table2.Get(i, colmax + 1);
                        c.backgroundColor = Globals.decompIgnoredColor;
                        c.vars_hack = new List<string>() { Globals.decompIgnoreName };
                        c.value_hack = sum_hack;
                    }
                }
            }

            // ----------------------------------------------
            // Show non-existing variables as N, not M
            // ----------------------------------------------

            for (int i = 2; i <= table2.GetRowMaxNumber(); i++)
            {
                for (int j = 2; j <= table2.GetColMaxNumber(); j++)
                {
                    try
                    {
                        Cell c = table2.Get(i, j);
                        if (c.cellType != CellType.Number) continue;  //should not happen, just for safety
                        double d = c.number;
                        if (double.IsNaN(d))
                        {
                            bool hit = false;
                            List<string> xx = c.vars_hack;
                            foreach (string s in xx)
                            {
                                int a = model.modelGamsScalar.dict_FromVarNameToANumber[DName.HACK1(s)];
                                if (a == -12345) continue;

                                bool b1 = decompOptions2.decompOperator.lowLevel == Decomp.ELowLevel.OnlyQuo || decompOptions2.decompOperator.lowLevel == Decomp.ELowLevel.BothQuoAndRef || decompOptions2.decompOperator.lowLevel == Decomp.ELowLevel.Multiplier;
                                bool b2 = decompOptions2.decompOperator.lowLevel == Decomp.ELowLevel.OnlyRef || decompOptions2.decompOperator.lowLevel == Decomp.ELowLevel.BothQuoAndRef || decompOptions2.decompOperator.lowLevel == Decomp.ELowLevel.Multiplier;

                                if (b1) //first-position databank checked
                                {
                                    if (model.modelGamsScalar.nonExisting != null && model.modelGamsScalar.nonExisting.ContainsKey(a))
                                    {
                                        hit = true;
                                        goto Lbl1;
                                    }
                                }

                                if (b2) //ref databank checked
                                {
                                    if (model.modelGamsScalar.nonExisting_ref != null && model.modelGamsScalar.nonExisting_ref.ContainsKey(a))
                                    {
                                        hit = true;
                                        goto Lbl1;
                                    }
                                }
                            }
                        Lbl1:;
                            if (hit)
                            {
                                //c.number = Globals.missingVariableArtificialNumber;
                                c.numberShouldShowAsN = true;
                            }
                        }
                    }
                    catch
                    {
                        //if this fails, never mind, just a M instead of a N.
                    }
                }
            }

            // ------------------------------------------
            // ERRORS
            // ------------------------------------------

            //Set error row/column, as a sum of rows 2 and on. Also sets count/names on that row/col.
            if (decompOptions2.showErrors && !decompOptions2.decompOperator.isRaw)
            {
                int rowmax = table2.GetRowMaxNumber();  //because it changes dynamically later on
                int colmax = table2.GetColMaxNumber();  //because it changes dynamically later on
                if (rowsOrCols == Decomp.ERowsCols.Rows)
                {
                    for (int j = 2; j <= colmax; j++)
                    {
                        double target = table2.Get(2, j).number;
                        double sum = 0d;
                        double sum_hack = 0d;
                        for (int i = 3; i <= rowmax; i++)
                        {
                            sum += table2.Get(i, j).number;
                            sum_hack += table2.Get(i, j).value_hack;
                        }

                        if (decompOptions2.count == ECountType.N)
                        {
                            table2.SetNumber(rowmax + 1, j, 1, "f16.0");
                        }
                        else if (decompOptions2.count == ECountType.Names)
                        {
                            table2.Set(rowmax + 1, j, Globals.decompErrorName2);
                        }
                        else
                        {
                            table2.SetNumber(rowmax + 1, j, target - sum, numberFormat);
                        }

                        table2.Get(rowmax + 1, j).vars_hack = new List<string>() { Globals.decompErrorName };
                        table2.Get(rowmax + 1, j).value_hack = -sum_hack;  //probably not used?
                        table2.Get(rowmax + 1, j).backgroundColor = Globals.decompErrorColor;
                    }
                    table2.Set(rowmax + 1, 1, Globals.decompErrorName2);

                }
                else if (rowsOrCols == Decomp.ERowsCols.Cols)
                {

                    for (int i = 2; i <= rowmax; i++)
                    {
                        double target = table2.Get(i, 2).number;
                        double sum = 0d;
                        double sum_hack = 0d;
                        for (int j = 3; j <= colmax; j++)
                        {
                            sum += table2.Get(i, j).number;
                            sum_hack += table2.Get(i, j).value_hack;
                        }

                        if (decompOptions2.count == ECountType.N)
                        {
                            table2.SetNumber(i, colmax + 1, 1, "f16.0");
                        }
                        else if (decompOptions2.count == ECountType.Names)
                        {
                            table2.Set(i, colmax + 1, Globals.decompErrorName2);
                        }
                        else
                        {
                            table2.SetNumber(i, colmax + 1, target - sum, numberFormat);
                        }

                        table2.Get(i, colmax + 1).vars_hack = new List<string>() { Globals.decompErrorName };
                        table2.Get(i, colmax + 1).value_hack = -sum_hack;  //probably not used?
                        table2.Get(i, colmax + 1).backgroundColor = Globals.decompErrorColor;
                    }
                    table2.Set(1, colmax + 1, Globals.decompErrorName2);
                }
                else
                {
                    //do nothing, no errors shown
                }
            }

            // --------------------------------------------------------------------
            // Calculate yellow/orange/red lamps as the very last step
            // --------------------------------------------------------------------

            if (rowsOrCols == Decomp.ERowsCols.Rows)
            {
                for (int j = 2; j <= table2.GetColMaxNumber(); j++)
                {
                    double target = table2.Get(2, j).number;
                    double sum = 0d;
                    for (int i = 3; i <= table2.GetRowMaxNumber(); i++)  //ignore first 2 rows
                    {
                        double x = table2.Get(i, j).number;
                        if (double.IsNaN(x)) x = 0d; //hmmmmmm?
                        sum += x;
                    }
                    double error = 1 - sum / target;  //value 0 for same number.
                    if (sum == 0d && target == 0d) error = 0d;
                    else if (target == 0d || double.IsNaN(target)) error = 1000000d; //just some large number
                    red.Add(error);  //one for each period
                }
            }
            else if (rowsOrCols == Decomp.ERowsCols.Cols)
            {
                for (int i = 2; i <= table2.GetRowMaxNumber(); i++)
                {
                    double target = table2.Get(i, 2).number;
                    double sum = 0d;
                    for (int j = 3; j <= table2.GetColMaxNumber(); j++)  //ignore first 2 cols
                    {
                        double x = table2.Get(i, j).number;
                        if (double.IsNaN(x)) x = 0d; //hmmmmmm?
                        sum += x;
                    }
                    double error = 1 - sum / target;  //value 0 for same number.
                    if (sum == 0d && target == 0d) error = 0d;
                    else if (target == 0d || double.IsNaN(target)) error = 1000000d; //just some large number
                    red.Add(error);  //one for each period
                }
            }
            else
            {
                //lamps not shown
            }

            DecompOutput decompOutput = new DecompOutput(table2, ignore, red, null);
            return decompOutput;
        }


        /// <summary>
        /// At this point, decomp rows sum to 0, so we change the sign of the first row, so the rest sum
        /// to the first. Also, percentages can be set, so first row is 100%. Also works for columns.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="decompOptions2"></param>
        private static void DecompTableHandleSignAndShares_OLD(Table tab, DecompOptions2 decompOptions2)
        {
            Decomp.ERowsCols rowsOrCols = Decomp.VariablesOnRowsOrCols(decompOptions2);

            //
            // SIGN AND SHARES
            //
            //change sign on row/col 2 (dependent var), and calcuate share values for rows/cols 2 and on.
            //no adding of rows/columns.
            if (!decompOptions2.decompOperator.isRaw)
            {
                string formatSShares = "f16." + decompOptions2.decimalsPch;
                if (decompOptions2.count == ECountType.N || decompOptions2.count == ECountType.Names) return;
                if (rowsOrCols == Decomp.ERowsCols.Rows)
                {
                    for (int j = 2; j <= tab.GetColMaxNumber(); j++)
                    {
                        double value = tab.Get(2, j).number;
                        for (int i = 2; i <= tab.GetRowMaxNumber(); i++)
                        {
                            if (i == 2)
                            {
                                Cell c = tab.Get(i, j);
                                c.number = -value;
                            }
                            if (decompOptions2.isShares)
                            {
                                Cell c = tab.Get(i, j);
                                c.number = tab.Get(i, j).number / (-value) * 100d;
                                c.numberFormat = formatSShares;
                            }
                        }
                    }
                }
                else if (rowsOrCols == Decomp.ERowsCols.Cols)
                {
                    for (int i = 2; i <= tab.GetRowMaxNumber(); i++)
                    {
                        double value = tab.Get(i, 2).number;
                        for (int j = 2; j <= tab.GetColMaxNumber(); j++)
                        {
                            if (j == 2)
                            {
                                Cell c = tab.Get(i, j);
                                c.number = -value;
                            }
                            if (decompOptions2.isShares)
                            {
                                Cell c = tab.Get(i, j);
                                c.number = tab.Get(i, j).number / (-value) * 100d;
                                c.numberFormat = formatSShares;
                            }
                        }
                    }
                }
                else
                {
                    //Do nothing: no special handling of the first row/col, and no 
                    //shares calculation.
                    //Should the values change sign? Sign is probably pretty arbitray, and
                    //the cells sum up to zero (?)
                }
            }
        }

        /// <summary>
        /// The decomp provides a linearization where the contributions sum to 0.
        /// Here, this is "translated" into the normal decomp way of showing it.
        /// This method is old, bad and soon obsolete.
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="parentI"></param>
        /// <param name="decompDatasSupremeClone"></param>
        /// <param name="operatorOneOf3Types"></param>
        private static void DecompNormalizeOLD(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, int parentI, DecompData decompDatasSupremeClone, Decomp.EContribType operatorOneOf3Types)
        {
            DecompOperator op = new DecompOperator(decompOptions2.decompOperator.OperatorLower());
            EDecompBanks edb = Decomp.DecompBanks_OLDREMOVESOON(op);

            bool orderNormalize = true;
            //This normalizes the parent-link-variables so that they reflect their real values
            //Parent-link-variables are for instance x1, x2, x3 here: DECOMP x1, x2, x2 IN ...

            if (orderNormalize)
            {
                int j = 0;

                if (true)
                {
                    string name = decompOptions2.link[parentI].varnames;

                    bool isResidualName = name == Globals.decompResidualName;

                    string name1 = Decomp.DecompFirst() + ":" + name + "¤[0]";  //what about lags in eqs??
                    string name2 = Decomp.DecompFirst() + ":" + name;
                    string name2Ref = Program.databanks.GetRef().name + ":" + name;

                    if (Decomp.GetDecompDatas(decompDatasSupremeClone, operatorOneOf3Types).ContainsKey(name1))
                    {
                        Series lhs2 = Decomp.GetDecompDatas(decompDatasSupremeClone, operatorOneOf3Types)[name1];
                        Series lhsReal = null;
                        Series lhsRealRef = null;
                        if (isResidualName)
                        {
                            //just keep lhsReal/lhsRealRef = null
                        }
                        else
                        {
                            if (edb == EDecompBanks.Work)
                            {
                                lhsReal = O.GetIVariableFromString(name2, O.ECreatePossibilities.NoneReportError) as Series;
                            }
                            else if (edb == EDecompBanks.Ref)
                            {
                                lhsRealRef = O.GetIVariableFromString(name2Ref, O.ECreatePossibilities.NoneReportError) as Series;
                            }
                            else if (edb == EDecompBanks.Multiplier)
                            {
                                lhsReal = O.GetIVariableFromString(name2, O.ECreatePossibilities.NoneReportError) as Series;
                                lhsRealRef = O.GetIVariableFromString(name2Ref, O.ECreatePossibilities.NoneReportError) as Series;
                            }
                        }

                        DecompData d = decompDatasSupremeClone;
                        foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                        {
                            double d1 = lhs2.GetDataSimple(t);
                            double factor = 1d;

                            if (isResidualName)
                            {
                                //keep factor = 1
                            }
                            else
                            {
                                // --------------------------------------------
                                //TODO: other operators
                                //TODO: other operators
                                //TODO: other operators
                                //TODO: other operators, this is <d>
                                //TODO: other operators
                                //TODO: other operators
                                //TODO: other operators

                                //Stuff below does not always work: the variable to be shown may not even be in the first equation (for instance: DECOMP y[#a] in demand[#a] = supply[#a]...
                                //So for now, we allow it to be fetched from the databank
                                //Series temp = decompDatasSupremeClone[j].cellsQuo.storage[name1];
                                //double d2 = temp.GetDataSimple(t) - temp.GetDataSimple(t.Add(-1));        

                                double d2 = double.NaN;

                                if (operatorOneOf3Types == Decomp.EContribType.D)
                                {
                                    d2 = lhsReal.GetDataSimple(t) - lhsReal.GetDataSimple(t.Add(-1));
                                }
                                else if (operatorOneOf3Types == Decomp.EContribType.RD)
                                {
                                    d2 = lhsRealRef.GetDataSimple(t) - lhsRealRef.GetDataSimple(t.Add(-1));
                                }
                                else if (operatorOneOf3Types == Decomp.EContribType.M)
                                {
                                    d2 = lhsReal.GetDataSimple(t) - lhsRealRef.GetDataSimple(t);
                                }

                                // ----------------------------------------------

                                factor = d2 / d1;
                            }

                            if (true)
                            {
                                bool found = false;
                                foreach (KeyValuePair<string, Series> kvp in Decomp.GetDecompDatas(d, operatorOneOf3Types).storage)
                                {
                                    if (G.Equal(kvp.Key, name1))
                                    {
                                        kvp.Value.SetData(t, factor * kvp.Value.GetDataSimple(t));
                                        found = true;
                                    }
                                    else
                                    {
                                        //switch sign!
                                        kvp.Value.SetData(t, -factor * kvp.Value.GetDataSimple(t));
                                    }
                                }
                                if (found == false)
                                {
                                    MessageBox.Show("*** ERROR: Did not find " + name1 + " for normalization");
                                }
                            }
                        }
                    }
                    else
                    {
                        new Error("Could not find variable " + name1 + " in non-linked equation number " + j + ".Beware of alignment: the names and equations must match.");
                    }
                }
            }

            return;
        }


    }
}
