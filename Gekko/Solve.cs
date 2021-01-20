﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;
using System.IO;
using MathNet.Numerics.LinearAlgebra.Sparse.Linear;
using MathNet.Numerics.LinearAlgebra.Sparse;
using MathNet.Numerics.LinearAlgebra.Sparse.Tests;


namespace Gekko
{
    public static class SolveCommon
    {
        public static void SimulateSimulPrologue(Type assembly)
        {
            if (Program.options.solve_newton_robust)
            {
                //preparing to call simulPrologue and simulFeedbackAll
                Globals.newtonRobustHelper1 = 0;
            }
            assembly.InvokeMember("simulPrologue", BindingFlags.InvokeMethod, null, null, new Object[] { Program.model.modelGekko.b });
        }

        public static void FixStartingValuesNumericalError(double[] b, int numericalProblem, Type assembly)
        {
            //DOES NOT WORK WITH FEEDBACK!!!

            //Starting values for endogenous cause numerical problems
            //we must try to find a region where all residuals given the endogenous
            //are valid.
            //Label1:;
            double[] endoValuesStart = new double[Program.model.modelGekko.m2.sparseInfo[numericalProblem].Count];
            for (int iii = 0; iii < Program.model.modelGekko.m2.sparseInfo[numericalProblem].Count; iii++)
            {
                //Remember starting values for endogenous
                int endo = Program.model.modelGekko.m2.sparseInfo[numericalProblem][iii];
                if (b[endo] == 0)
                {
                    //could just as well have been 1d, but we choose 0.001d.
                    b[endo] = Globals.newtonSmallNumber;
                }
                endoValuesStart[iii] = b[endo];
            }

            for (int s1 = 0; s1 < int.MaxValue; s1++)
            {
                //s1 is used for a sequence, 2,4,8, ...
                for (int s2 = 0; s2 < 4; s2++)
                {
                    //s2 is used to switch sign and reciprocity
                    double ss = Math.Pow(Globals.newtonStartingValueProblemSearchParameter, (double)s1 + 1d);
                    if (s2 == 1)
                    {
                        ss = -ss;
                    }
                    else if (s2 == 2)
                    {
                        ss = 1 / ss;
                    }
                    else if (s2 == 3)
                    {
                        ss = -1 / ss;
                    }
                    for (int iii = 0; iii < Program.model.modelGekko.m2.sparseInfo[numericalProblem].Count; iii++)
                    {
                        int endo = Program.model.modelGekko.m2.sparseInfo[numericalProblem][iii];
                        b[endo] = endoValuesStart[iii] * ss;
                        SimulateResidual(b, Program.model.modelGekko.r, numericalProblem, assembly);
                        if (!G.isNumericalError(Program.model.modelGekko.r[numericalProblem]))
                        {
                            return;
                        }
                        b[endo] = endoValuesStart[iii];
                    }
                }
            }
        }


        public static double RssNonScaled(IElementalAccessVector residuals)
        {
            bool check = false;
            double rssNonScaled = 0d;
            double max = double.NegativeInfinity;
            int maxI = -12345;
            for (int i = 0; i < residuals.Length; i++)
            {
                double number = residuals.GetValue(i);
                if (false && G.isNumericalError(number))
                {
                    G.Writeln("NUM ERROR ---> " + Program.model.modelGekko.varsBTypeInverted[Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW[i]]);
                }
                double number2 = number * number;
                rssNonScaled += number2;
                if (number2 > max)
                {
                    max = number2;
                    maxI = i;
                }
            }

            if (check)
            {
                if (Globals.sw == null) Globals.sw = G.GekkoStreamWriter(Program.WaitForFileStream(@"c:\Thomas\Desktop\gekko\testing\cg.txt", Program.GekkoFileReadOrWrite.Write));

                for (int i = 0; i < residuals.Length; i++)
                {
                    double number = residuals.GetValue(i);
                    double number2 = number * number;
                    if (number2 > 1d)
                    {
                        Globals.sw.WriteLine("rs[" + i + "] = " + number2 + ";");
                    }
                    else
                    {
                        Globals.sw.WriteLine("rs[" + i + "] = " + 1d + ";");
                    }
                }


                if (false)
                {

                    Globals.sw.WriteLine("i = " + maxI + " " + max + " " + rssNonScaled + " " + max / rssNonScaled);
                    Globals.sw.WriteLine("---> " + Program.model.modelGekko.varsBTypeInverted[Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW[maxI]]);
                }

                //G.Writeln2("i = " + maxI + " " + max + " " + rssNonScaled + " " + max / rssNonScaled);
                //G.Writeln("---> " + model.varsBTypeInverted[Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW[maxI]]);

                Globals.sw.Flush();
            }

            if (double.IsNaN(rssNonScaled)) rssNonScaled = double.MaxValue;  //should count as being worse than anything
            return rssNonScaled;
        }

        public static void RSS(IElementalAccessVector residuals, IElementalAccessVector x, Type assembly)
        {
            int numericalProblem = -12345;
            //This puts vector x (the feedback variables) into the corresponding b[] slots
            for (int i = 0; i < Program.model.modelGekko.m2.fromEqNumberToBNumber.Length; i++)
            {
                Program.model.modelGekko.b[Program.model.modelGekko.m2.fromEqNumberToBNumber[i]] = x.GetValue(i);
            }
            numericalProblem = -12345;
            Program.model.modelGekko.r = new double[Program.model.modelGekko.m2.fromEqNumberToBNumber.Length];
            //Keep SimulateSimulPrologue() and SimulateResiduals() together
            SimulateSimulPrologue(assembly);
            SimulateResiduals(Program.model.modelGekko.b, Program.model.modelGekko.r, assembly);

            for (int i = 0; i < Program.model.modelGekko.m2.fromEqNumberToBNumber.Length; i++)
            {
                if (G.isNumericalError(Program.model.modelGekko.r[i]))
                {
                    numericalProblem = i;
                }
                residuals.SetValue(i, Program.model.modelGekko.r[i]);
            }
        }

        public static void SolveRevertedAuto()
        {
            Object[] args2 = new Object[1];
            args2[0] = Program.model.modelGekko.b;
            Program.model.modelGekko.assemblyReverted.InvokeMember("revertedAuto", BindingFlags.InvokeMethod, null, null, args2);
        }

        public static void SolveRevertedY()
        {
            Object[] args2 = new Object[1];
            args2[0] = Program.model.modelGekko.b;
            Program.model.modelGekko.assemblyReverted.InvokeMember("reverted" + Globals.equationCodeY.ToUpper(), BindingFlags.InvokeMethod, null, null, args2);
        }

        public static void SolveRevertedT()
        {
            Object[] args2 = new Object[1];
            args2[0] = Program.model.modelGekko.b;
            Program.model.modelGekko.assemblyReverted.InvokeMember("reverted" + Globals.equationCodeT.ToUpper(), BindingFlags.InvokeMethod, null, null, args2);
        }

        public static void SolveAfter()
        {
            //Parser.OrderAndCompileModel(ECompiledModelType.After, false);
            Object[] args2 = new Object[1];
            args2[0] = Program.model.modelGekko.b;
            Program.model.modelGekko.assemblyAfter.InvokeMember("after", BindingFlags.InvokeMethod, null, null, args2);
            Program.model.modelGekko.assemblyAfter.InvokeMember("after2", BindingFlags.InvokeMethod, null, null, args2);
        }



        public static void SimPrintErrorOptionsUndo(Program.LinkContainer lc1)
        {
            G.Write("    Undo the simulation, reverting to pre-simulation values ");
            G.WriteLink("here", "undosim:" + lc1.counter);
            G.Writeln("");
        }

        public static void SimPrintErrorOptionsPack(Program.LinkContainer lc2)
        {
            G.Write("    Create error report with model + data packed in a zip file ");
            G.WriteLink("here", "packsim:" + lc2.counter);
            G.Writeln("");
        }


        public static void SimCheckFirstPeriodForMissingStuff(bool usingFairTaylor, GekkoTime tStart, ErrorContainer ec, Series[] timeSeriesPointers, int[] lagPointers, int[] endoNoLagPointers, int[] endoLeadPointers, string[] varNamePointers, int[] isDJZvarPointers)
        {
            if (true)  //for a period like 2006-2079, this check hardly consumes any time
            {
                GekkoTime t = tStart.Add(0);
                //Fail-fast check of what data is missing in order to simulate
                if (Program.databanks.GetFirst().storage.Count == 0)
                {
                    G.Writeln2("*** ERROR: There were no variables in the databank. Did you forget to load a databank?");
                    G.Writeln("           Simulation is aborted");
                    throw new GekkoException();
                }

                List<string> missingVariables = new List<string>();
                List<string> exoOrLaggedEndoWithNaN = new List<string>();

                for (int i = 0; i < Program.model.modelGekko.varsBType.Count; i++)
                {
                    Series ts = timeSeriesPointers[i];

                    string variable = varNamePointers[i];

                    if (ts == null)
                    {
                        if (isDJZvarPointers[i] == 1)
                        {
                            //we don't do anything about these now if they are missing:
                            //they are handled below (created and given value = 0)
                        }
                        else
                        {
                            //missing variable, and not a DJZ-type variable
                            //G.Writeln("*** ERROR: time series " + variable + " (and possibly more) does not exist in Work bank");
                            //G.Writeln("    Did you forget to load a databank? Simulation is stopped.");
                            if (!missingVariables.Contains(variable))
                            {
                                missingVariables.Add(variable);
                            }
                            //FIXME: undo, or at least write if DJZ variables have been created.
                            //return -12345;
                        }
                    }
                    else
                    {
                        //Variable exists in Work bank
                        //The variable could be a autogenerated DJZ-variable
                        int endoLag = -endoNoLagPointers[i];  //these are 0 or 1 (endo with no lag)
                        if (!Program.options.solve_data_init) endoLag = 0;  //overrides with this option. Typically for use in residual type simulation.
                        double val = ts.GetDataSimple(t.Add(lagPointers[i] + endoLag));

                        if (double.IsNaN(val))
                        {
                            if (Program.options.solve_data_ignoremissing)
                            {
                                val = 0d;
                            }
                        }

                        if (endoNoLagPointers[i] == 1)
                        {
                            //do nothing here
                        }
                        else if (Program.options.solve_data_init && usingFairTaylor && endoLeadPointers[i] == 1)
                        {
                            //do nothing here for leaded variables
                        }
                        else
                        {
                            //not a nolag-endogenous, so lagged endo or exogenous (including DJZ-vars)
                            //endoNoLagPointers[i] = 0.

                            if (double.IsInfinity(val))
                            {
                                //value is NaN -- real problem.... not just starting value question
                                if (isDJZvarPointers[i] == 1)
                                {
                                    //this should be reported maybe: if equation is a JR: log(y) = log(x);
                                    //then y>0 and x=0 gives JR=infinity -- there is no solution. This is
                                    //ok if equation is exogenized with the dummy, but what then if the
                                    //dummy is switched off again? Sim values may jump...
                                    //Setting the J-variable to 0 is a workaround. Happens implicitly
                                    //when writing the bank, since infinity is treated as missing.
                                }
                            }

                            if (double.IsNaN(val))
                            {
                                //value is NaN -- real problem.... not just starting value question
                                if (isDJZvarPointers[i] == 1)
                                {
                                    //see notes regarding .IsInfinity() above
                                }
                                else
                                {
                                    //real lagged endo or exo
                                    //todo: write if exo, lagged endo, and the lag
                                    //G.Writeln("*** ERROR: In period " + (t + lagPointers[i]) + " the variable " + variable + " has a missing");
                                    //G.Writeln("    value. Please check your data bank -- simulation is not performed.");
                                    //FIXME: undo, or at least write if DJZ variables have been created.
                                    //return -12345;

                                    string s = "";
                                    if (lagPointers[i] <= 0) s = variable + "[" + lagPointers[i] + "]";
                                    else s = variable + "[+" + lagPointers[i] + "]";

                                    if (!exoOrLaggedEndoWithNaN.Contains(s))
                                    {
                                        exoOrLaggedEndoWithNaN.Add(s);
                                    }
                                }
                            }
                        }
                    }
                }

                if (missingVariables.Count > 0)
                {
                    if (ec.simNonExistingVariable == null) ec.simNonExistingVariable = new List<string>();
                    missingVariables.Sort(StringComparer.InvariantCulture);
                    ec.simNonExistingVariable.Add("When model variables do not exist in the databank, simulation can not be");
                    ec.simNonExistingVariable.Add("performed. Please make sure the variable(s) exist before simulating.");
                    ec.simNonExistingVariable.Add("There were " + missingVariables.Count + " missing variable(s):");
                    ec.simNonExistingVariable.Add("");
                    StringBuilder sb = new StringBuilder();
                    int count = 0;
                    foreach (string s in missingVariables)
                    {
                        count++;
                        string blank = "";
                        if (count > 0) blank = Globals.blankUsedAsPadding;
                        string count2 = count.ToString();
                        ec.simNonExistingVariable.Add(blank + " " + G.Blanks(4 - count2.Length) + "#" + count + ": The variable " + s + " does not exist in Work databank");
                    }
                    ec.simNonExistingVariable.Add("");
                }

                if (exoOrLaggedEndoWithNaN.Count > 0)
                {
                    if (ec.simMissingValueExoOrLaggedEndo == null) ec.simMissingValueExoOrLaggedEndo = new List<string>();
                    exoOrLaggedEndoWithNaN.Sort(StringComparer.InvariantCulture);
                    ec.simMissingValueExoOrLaggedEndo.Add("When there are missing values regarding exogenous variables or lagged endogenous variables,");
                    ec.simMissingValueExoOrLaggedEndo.Add("simulation can not be performed. Please make sure the relevant data exist before simulating.");
                    ec.simMissingValueExoOrLaggedEndo.Add("There were " + exoOrLaggedEndoWithNaN.Count + " variables with missing values:");
                    ec.simMissingValueExoOrLaggedEndo.Add("");
                    DateTime t0 = DateTime.Now;
                    StringBuilder sb = new StringBuilder();
                    int count = 0;
                    foreach (string s in exoOrLaggedEndoWithNaN)
                    {
                        count++;
                        string s2 = s.Replace("[0]", "");
                        //s2 = s2.Replace("[", "(");
                        //s2 = s2.Replace("]", ")");
                        string count2 = count.ToString();
                        string blank = "";
                        if (count > 0) blank = Globals.blankUsedAsPadding;
                        ec.simMissingValueExoOrLaggedEndo.Add(blank + " " + G.Blanks(4 - count2.Length) + "#" + count + ": Period " + tStart + ": variable " + s2 + " had a missing value in Work databank");
                    }
                    ec.simMissingValueExoOrLaggedEndo.Add("");
                }

                if (missingVariables.Count > 0 || exoOrLaggedEndoWithNaN.Count > 0)
                {
                    //G.writeAbstractScroll("", null, true, Color.Empty, false, ETabs.Output, true);
                    G.Writeln2("*** ERROR: Gekko had problems simulating the first period in the simulation (" + t + "):");
                    if (missingVariables.Count > 0)
                    {
                        G.Write("    There were "); G.WriteLink(missingVariables.Count.ToString() + " missing variables", "tab:output" + ec.counter + "b"); G.Writeln(".");
                    }
                    if (exoOrLaggedEndoWithNaN.Count > 0)
                    {
                        G.Write("    There were missing values in "); G.WriteLink(exoOrLaggedEndoWithNaN.Count.ToString() + " variables", "tab:output" + ec.counter + "c"); G.Writeln(".");
                    }
                    G.Writeln();
                    if (!Globals.outputTabTextContainer.ContainsKey(ec.counter.ToString()))
                    {
                        Globals.outputTabTextContainer.Add(ec.counter.ToString(), ec);
                    }
                    throw new GekkoException();
                }
            }
        }

        public static bool IsDjz(string variable)
        {
            return Program.model.modelGekko.varsDTypeAutoGenerated.ContainsKey(variable) || Program.model.modelGekko.varsJTypeAutoGenerated.ContainsKey(variable) || Program.model.modelGekko.varsZTypeAutoGenerated.ContainsKey(variable);
        }

        public static Type GetAssemblyFromModelType(ECompiledModelType modelType)
        {
            Type assembly = null;
            if (modelType == ECompiledModelType.Res)
            {
                assembly = Program.model.modelGekko.m2.assemblyRes;
            }
            else if (modelType == ECompiledModelType.Gauss)
            {
                assembly = Program.model.modelGekko.m2.assemblyGauss;
            }
            else if (modelType == ECompiledModelType.GaussFailSafe)
            {
                assembly = Program.model.modelGekko.m2.assemblyGaussFailSafe;
            }
            else if (modelType == ECompiledModelType.Newton)
            {
                assembly = Program.model.modelGekko.m2.assemblyNewton;
            }
            //else if (modelType == ECompiledModelType.After)
            //{
            //    assembly = model.m2.assemblyReverted;
            //}
            else throw new GekkoException();
            return assembly;
        }

        public static void FillAWithNaN(int vars, double[,] a, double[] NAN)
        {
            for (int i = 0; i < vars; i++)
            {
                Buffer.BlockCopy(NAN, 0, a, 8 * i * NAN.Length, 8 * NAN.Length);  //TODO: what if out of bounds regarding x???
            }
        }

        public static void InitEndoLaggedOrExo(int[] extraWritebackPointers, int[] lagPointers, int[] endoPointers, int[] isDJZvarPointers, double[,] a, int tInt, ref GekkoTime t, ref GekkoTime tStart, ref GekkoTime tEnd, int i, ref double val, Series ts, string variable, int yy, YesNoNull isStaticLocalOption)
        {
            //not a nolag-endogenous or lead-endogenous, so lagged endo or exogenous (may in principle be leaded exogenous)
            //these are just copied into b[] raw
            //FOR sim and fastsim, try doing it without simgauss, to see what goes into gauss


            val = a[yy, tInt + lagPointers[i]];

            if (Program.options.solve_data_ignoremissing && double.IsNaN(val)) val = 0d;
            //this way, missing DJZ vars also get value = 0

            if (double.IsInfinity(val))
            {
                //this can be possible in successive simulations, see notes in top of method regarding .IsInfinity()
                if (isDJZvarPointers[i] == 1)
                {
                    //probably should issue warning here
                    //J-factor or D or Z variable
                    val = 0;
                    extraWritebackPointers[i] = 1;
                }
            }

            if (double.IsNaN(val))
            {
                //this can be possible for autogenerated DJZ-variables -- not for other types since these are checked above ("real" exogenous and lagged endogenous)
                if (isDJZvarPointers[i] == 1)
                {
                    //J-factor or D or Z variable
                    val = 0;
                    extraWritebackPointers[i] = 1;
                }
                else
                {
                    //real lagged endo or exo (or leaded endo/exo)
                    //it should not be possible to encounter a missing lagged endo here, since lagged endo vars are checked
                    //before simulating first period -- and program breaks if simulation does not converge.
                    //but it could be an exogenous or lagged exogenous with a missing somewhere in the simulation period.
                    //todo: write if exo, lagged endo, and the lag
                    string lag = "";
                    if (lagPointers[i] < 0) lag = Globals.leftParenthesisIndicator + lagPointers[i] + Globals.rightParenthesisIndicator;
                    else if (lagPointers[i] > 0) lag = Globals.leftParenthesisIndicator + "+" + lagPointers[i] + Globals.rightParenthesisIndicator;
                    string type = "exogenous";
                    if (endoPointers[i] == 1) type = "endogenous";

                    G.Writeln("*** ERROR while simulating " + tStart.ToString() + "-" + tEnd.ToString() + ": in " + t.ToString() + " the " + type + " variable '" + variable + lag + "' has a missing value");
                    GekkoTime tLag = t.Add(-1);
                    //G.Writeln("    Simulated " + tStart.ToString() + "-" + tLag + ", but " + t.ToString() + "-" + tEnd + " failed.");
                    bool simFailure = false;
                    if (endoPointers[i] == 1)
                    {
                        if (lagPointers[i] == -1 && t.LargerThanOrEqual(tStart))
                        {
                            G.Writeln("    The problem probably has to do with non-convergence of period " + tLag.ToString());
                            //WriteAboutFailsafeOption();  --> will issue this note 2 times, so deleted here
                        }
                        simFailure = true;
                    }
                    else
                    {
                        G.Writeln("    Please check the databank for missing data regarding exogenous variable '" + variable + "'");
                    }

                    if (simFailure) throw new GekkoException("GekkoException: simFailure");
                    else throw new GekkoException();
                    //FIXME: undo, or at least write if DJZ variables have been created.
                }
            }
            else
            {
                //TODO: check static sim -- seems ok but could need a good check (also with endo/exo stuff)
                if (CheckYesNoNullLogic(isStaticLocalOption, Program.options.solve_static))
                {
                    //This does not run fast, but is seldom used
                    //is also done for exogenous, maybe more safe only for lagged endo.
                    Series tsUndoBank = Globals.undoBank.GetIVariable(ts.name) as Series;
                    //overrides the value -- takes it from the undoBank -- if exo there should be no change
                    val = tsUndoBank.GetDataSimple(t.Add(lagPointers[i]));
                }
            }
        }

        public static void Sim(O.Sim o)
        {
            if (!G.HasModelGekko())
            {
                G.Writeln2("*** ERROR: No model seems to be defined (see MODEL statement)");
                throw new GekkoException();
            }

            if (G.HasModelGekko() && Program.model.modelGekko.subPeriods != -12345 && Program.model.modelGekko.subPeriods != O.CurrentSubperiods())
            {
                G.Writeln2("*** ERROR: The model was not compiled/loaded with the current frequency");
                G.Writeln("    This applies to the pchy(), dify(), diffy(), dlogy() functions. Please put");
                G.Writeln("    the MODEL statement after your 'OPTION freq ... ' statement.");
                throw new GekkoException();
            }

            if (G.Equal(o.opt_after, "yes"))
            {
                Efter(o.t1, o.t2);
                return;
            }
            else if (G.Equal(o.opt_res, "yes"))
            {
                SolveCommon.Res(o.t1, o.t2);
                return;
            }

            if (!G.IsUnitTesting()) Gekko.Gui.gui.textBox1.SuspendLayout();
            SimOptions so = new SimOptions();
            so.method = Program.options.solve_method;
            if (G.Equal(o.opt_fix, "yes")) so.isFix = true;

            so.isStatic = Program.GetYesNoNullLocalOption(o.opt_static);  //works faster as an enumeration

            string before = null;
            if (Program.model.modelGekko.runBefore != null) before = Program.model.modelGekko.runBefore.Replace("runbefore$", "").Replace("runbefore;", "");
            if (!G.NullOrBlanks(before))
            {
                ScalarDate t1 = new ScalarDate(o.t1);
                ScalarDate t2 = new ScalarDate(o.t2);
                Program.databanks.GetLocal().AddIVariableWithOverwrite(Globals.symbolScalar + "__simt1", new ScalarDate(o.t1));
                Program.databanks.GetLocal().AddIVariableWithOverwrite(Globals.symbolScalar + "__simt2", new ScalarDate(o.t2));
                Program.RunCommandCalledFromGUI(before, o.p);
                Program.databanks.GetLocal().RemoveIVariable(Globals.symbolScalar + "__simt1");
                Program.databanks.GetLocal().RemoveIVariable(Globals.symbolScalar + "__simt2");
            }

            SimFast(o.t1, o.t2, so);

            string after = null;
            if (Program.model.modelGekko.runAfter != null) after = Program.model.modelGekko.runAfter.Replace("runafter$", "").Replace("runafter;", "");
            if (!G.NullOrBlanks(after))
            {
                Program.databanks.GetLocal().AddIVariableWithOverwrite(Globals.symbolScalar + "__simt1", new ScalarDate(o.t1));
                Program.databanks.GetLocal().AddIVariableWithOverwrite(Globals.symbolScalar + "__simt2", new ScalarDate(o.t2));
                Program.RunCommandCalledFromGUI(after, o.p);
                Program.databanks.GetLocal().RemoveIVariable(Globals.symbolScalar + "__simt1");
                Program.databanks.GetLocal().RemoveIVariable(Globals.symbolScalar + "__simt2");
            }

            if (!G.IsUnitTesting()) Gekko.Gui.gui.textBox1.ResumeLayout();
        }

        public static void SimFast(GekkoTime tStart, GekkoTime tEnd, SimOptions so)
        {
            if (GekkoTime.Observations(tStart, tEnd) < 1)
            {
                G.Writeln2("*** ERROR: start period must be before end period");
                throw new GekkoException();
            }

            Globals.simCounter = 0;
            //ErrorIfDatabanksSwapped();
            if (!G.HasModelGekko())
            {
                G.Writeln2("*** ERROR: It seems no model is defined -- simulation cannot be performed");
                throw new GekkoException();
            }

            if (!G.Equal(so.method, "res"))
            {
                //don't do any terminal logic regarding SIM<res>
                //question is: what about SIM<after>??
                HandleTerminalHelper();
            }

            bool hasEndoExo = false; if (Program.model.modelGekko.endogenized.Count != 0 || Program.model.modelGekko.exogenized.Count != 0) hasEndoExo = true;

            DateTime startTime = DateTime.Now;
            DateTime dtFt = DateTime.Now;

            bool usingFairTaylor = false;
            bool usingNewtonFairTaylor = false;
            if (Program.model.modelGekko.largestLeadOutsideRevertedPart > 0)
            {
                if (G.Equal(Program.options.solve_forward_method, "fair")) usingFairTaylor = true;
                if (G.Equal(Program.options.solve_forward_method, "nfair")) usingNewtonFairTaylor = true;
                if ((usingFairTaylor || usingNewtonFairTaylor) && G.Equal(Program.options.solve_forward_terminal, "growth"))
                {
                    //#375204390457
                    G.Writeln2("*** ERROR: Terminal 'GROWTH' is not working at the moment, please use 'CONST'");
                    throw new GekkoException();
                }
            }

            List<string> outputText = new List<string>();

            bool debug = false;
            G.Writeln();

            bool hasIssuedSeedWarning = false;
            double simTime = 0d;
            //bool hasBeenAutoSetToNewton = false;
            double[] bCheck = null;  //for safety check

            Dictionary<int, int> checkoff = new Dictionary<int, int>();
            if (Program.options.solve_gauss_conv_ignorevars == true)
            {
                foreach (string var in Globals.checkoff)
                {
                    //slack: use .TryGetValue()
                    if (Program.model.modelGekko.varsBType.ContainsKey(var + Globals.lagIndicator + "0"))
                    {
                        int bNumber = Program.model.modelGekko.varsBType[var + Globals.lagIndicator + "0"].bNumber;
                        if (!checkoff.ContainsKey(bNumber)) checkoff.Add(bNumber, 0);
                    }
                }
            }

            if (!(G.Equal(so.method, "gauss") || G.Equal(so.method, "newton") || G.Equal(so.method, "res") || G.Equal(so.method, "reverted") || G.Equal(so.method, "eigen"))) G.Writeln("+++ WARNING: Seems to be a problem with model type");
            //isRes is true if called by Res(), isReverted if called by Efter()

            ErrorContainer ec = new ErrorContainer();

            ECompiledModelType modelType = GetModelTypeFromOptions(so);  //6 types, including Reverted (for EFTER command)

            //only used with ANTLR
            if (!G.HasModelGekko() || Program.model.modelGekko.equations.Count == 0)
            {
                G.Writeln2("*** ERROR: It seems no model is defined: did you forget a MODEL statement?");
                throw new GekkoException();
            }
            Globals.mayPrintConvergenceCheckVariableMissing = true;  //so that there is only 1 warning regarding this
            if (Program.model.modelGekko.endogenized.Count != Program.model.modelGekko.exogenized.Count)
            {
                G.Writeln2("*** ERROR: different number of endogenized/exogenized variables (endo = " + Program.model.modelGekko.endogenized.Count + ", exo = " + Program.model.modelGekko.exogenized.Count + ")");
                throw new GekkoException();
            }

            Program.model.modelGekko.jacobiMatrix = null;
            Program.model.modelGekko.jacobiMatrixInverted = null;
            Program.model.modelGekko.jacobiMatrixInvertedIndex = null;

            //TODO Cleanup: at some point clean up this, using the RAM cache idea more directly
            //if (Globals.hasBeenEndoExoStatementsSinceLastSim == 1)
            if (so.isFix)
            {
                if (hasEndoExo && !G.Equal(so.method, "newton"))
                {
                    //G.Writeln("+++ NOTE: SIM uses Newton method when ENDO/EXO vars are set.");
                    //hasBeenAutoSetToNewton = true;
                    so.method = "newton";
                    modelType = GetModelTypeFromOptions(so);  //5 types
                }
                Parser.Frm.ParserFrmCompileAST.ParserFrmOrderAndCompileAST(modelType, false, so.isFix);
            }
            else
            {
                //Compiles model if it is not already compiled
                //If sim is called several times for same sim options (e.g. Gauss), model will only be
                //compiled first time.
                //It is necessary to do compilation here, because there are some by-products from
                //this, specifically Program.model.modelGekko.endogenous (the model's endogenous). These are used below
                //in endoPointers etc.

                if (modelType != ECompiledModelType.After)
                {
                    Type assembly = GetAssemblyFromModelType(modelType);
                    if (assembly == null)
                    {
                        Parser.Frm.ParserFrmCompileAST.ParserFrmOrderAndCompileAST(modelType, false, so.isFix);
                        assembly = GetAssemblyFromModelType(modelType);
                        if (assembly == null)
                        {
                            G.Writeln2("*** ERROR: Sorry: something has gone wrong regarding model settings");
                            throw new GekkoException();
                        }
                    }
                }
                else
                {
                    Parser.Frm.ParserFrmCompileAST.ParserFrmOrderAndCompileAST(ECompiledModelType.After, false, so.isFix);
                }
            }
            //TODO Cleanup end

            if (CheckYesNoNullLogic(so.isStatic, Program.options.solve_static))
            {
                Globals.undoBank = new Databank("UndoBank");
                G.CloneDatabank(Globals.undoBank, Program.databanks.GetFirst());
            }

            if (Program.options.solve_gauss_dump)
            {
                Program.model.modelGekko.bMemory = null;
                GC.Collect();
                Program.model.modelGekko.bMemory = new GekkoDictionary<string, List<IterMemory>>(StringComparer.OrdinalIgnoreCase);
            }

            Program.model.modelGekko.simulateResults = new double[10];  //fix

            Databank work = Program.databanks.GetFirst();
            Series[] timeSeriesPointers = new Series[Program.model.modelGekko.varsBType.Count];
            int[] extraWritebackPointers = new int[Program.model.modelGekko.varsBType.Count]; //will probably become obsolete at some point
            int[] revertedPointers = new int[Program.model.modelGekko.varsBType.Count];
            int[] lagPointers = new int[Program.model.modelGekko.varsBType.Count];
            int[] aNumberPointers = new int[Program.model.modelGekko.varsBType.Count];
            int[] bNumberPointers = new int[Program.model.modelGekko.varsAType.Count];  //get from an a-number to equivalent b-number (with no lag)
            for (int i = 0; i < bNumberPointers.Length; i++)
            {
                bNumberPointers[i] = -12345;  //init for safety
            }
            int[] endoNoLagPointers = new int[Program.model.modelGekko.varsBType.Count];
            int[] endoLeadPointers = new int[Program.model.modelGekko.varsBType.Count];
            int[] endoPointers = new int[Program.model.modelGekko.varsBType.Count];
            string[] varNamePointers = new string[Program.model.modelGekko.varsBType.Count];  //has dublets, a var and a lagged var just has the name here
            int[] isDJZvarPointers = new int[Program.model.modelGekko.varsBType.Count];
            List<int> isDampedPointers = new List<int>();
            int[] isDampedPointersArray = new int[Program.model.modelGekko.varsBType.Count];
            int i1 = 0;

            foreach (BTypeData value in Program.model.modelGekko.varsBType.Values)
            {

                if (IsDjz(value.variable))
                {
                    isDJZvarPointers[value.bNumber] = 1;
                }
                varNamePointers[value.bNumber] = value.variable;
                //TODO: should make a check here that all slots are filled in from b[min] to b[max]
                Series ts = work.GetIVariable(value.variable + Globals.freqIndicator + G.GetFreq(Program.options.freq)) as Series;  //may be null
                timeSeriesPointers[value.bNumber] = ts;
                lagPointers[value.bNumber] = value.lag;
                aNumberPointers[value.bNumber] = value.aNumber;

                if (value.lag == 0)
                {
                    bNumberPointers[value.aNumber] = value.bNumber;  //used for Fair-Taylor convergence check
                }

                if (Program.model.modelGekko.m2.endogenous.ContainsKey(value.variable))
                {
                    if (value.lag == 0)
                    {
                        endoNoLagPointers[value.bNumber] = 1;
                    }
                    else if (value.lag > 0)
                    {
                        endoLeadPointers[value.bNumber] = 1;
                    }
                    endoPointers[value.bNumber] = 1;
                }

                if (value.lag == 0 && Program.model.modelGekko.reverted.ContainsKey(value.variable))
                {
                    revertedPointers[value.bNumber] = 1;
                }
                //for now, we ignore what the value may be (all damping is with same factor)
                if (Program.model.modelGekko.dampVariables.ContainsKey(value.variable + Globals.lagIndicator + value.lag))  //with lag indicator
                {
                    isDampedPointers.Add(value.bNumber);
                    isDampedPointersArray[value.bNumber] = 1;
                }
                i1++;
            }

            Program.model.modelGekko.b = new double[Program.model.modelGekko.varsBType.Count];
            Program.model.modelGekko.bVariance = new double[Program.model.modelGekko.varsBType.Count];
            Program.model.modelGekko.bOld = new double[Program.model.modelGekko.varsBType.Count];  //used in simulation, to store previous iteration
            for (int i = 0; i < Program.model.modelGekko.varsBType.Count; i++)
            {
                //just for safety, so an error would be found quickly if something gets messed up in referencing these arrays
                Program.model.modelGekko.b[i] = double.NaN;
                Program.model.modelGekko.bVariance[i] = double.NaN;
                Program.model.modelGekko.bOld[i] = double.NaN;
            }

            int number = GekkoTime.Observations(tStart, tEnd);
            double[] temp = new double[number];

            Program.CreateEndoNoLagBNumbers(endoNoLagPointers);

            for (int i = 0; i < Program.model.modelGekko.b.Length; i++)
            {
                Program.model.modelGekko.bVariance[i] = double.NaN;
            }

            Program.CreateBVariance(timeSeriesPointers, tStart.Add(-1));

            int largestLag = -2;  //to get some lagged data into a[] array, for use with initializing endo variables etc.
            if (Program.model.modelGekko.largestLag > 2) largestLag = -Program.model.modelGekko.largestLag;

            GekkoTime tStart0 = tStart.Add(largestLag);

            GekkoTime tEnd_withRE = tEnd.Add(Program.model.modelGekko.largestLeadOutsideRevertedPart);

            int horizon2 = 0;

            //DateTime dt1 = DateTime.Now;
            //G.Writeln("Code up to a[] load: used " + (dt1 - startTime).TotalMilliseconds / 1000d);

            //This will throw errors, if a variable is missing, or an exo or lagged endo has missing values
            //Runs pretty fast -- does not hamper simulation speed noticably
            //The nice thing is that full lists of missing stuff are done
            SimCheckFirstPeriodForMissingStuff(usingFairTaylor || usingNewtonFairTaylor, tStart, ec, timeSeriesPointers, lagPointers, endoNoLagPointers, endoLeadPointers, varNamePointers, isDJZvarPointers);

            int obsWithLagsIncludingLeadsAtEnd = GekkoTime.Observations(tStart0, tEnd_withRE);
            int obsSimPeriodIncludingLeadsAtEnd = GekkoTime.Observations(tStart, tEnd_withRE);
            int obsWithLags = GekkoTime.Observations(tStart0, tEnd);
            int obsSimPeriod = GekkoTime.Observations(tStart, tEnd);
            int vars = Program.model.modelGekko.varsAType.Count; //FIX: plus minus 1??
            //GC.Collect();
            double[,] a = new double[vars, obsWithLagsIncludingLeadsAtEnd]; //these zeroes will be overwritten with data or NaN.
            double[] NAN = new double[obsWithLagsIncludingLeadsAtEnd];
            for (int i = 0; i < NAN.Length; i++) NAN[i] = double.NaN;
            FillAWithNaN(vars, a, NAN);  //small speed penalty, but better safe (NaN) than sorry (0d)

            SolveDataInOut.FromDatabankToA(tStart0, tEnd_withRE, work, obsWithLagsIncludingLeadsAtEnd, a, NAN);

            //if (debug && Globals.runningOnTTComputer) G.Writeln("a[] load: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d, Color.LightGray);

            double ms1 = 0d;
            double ms2 = 0d;

            StringBuilder output = new StringBuilder();
            output.AppendLine("This is detailed output regarding the simulation, ie. the output");
            output.AppendLine("that is shown if 'OPTION solve print iter = yes'.");
            output.AppendLine();

            //do a RE loop here, checking only variables with leads.
            //maybe for each outer iteration print iteration # etc.
            //maybe use simulated values as init for endogenous variables
            //for all re-iterations except the first.

            //double[,] ftVarsOld = null;

            List<int> leadedVarsList = new List<int>();
            ETerminalCondition terminal = ETerminalCondition.Exogenous;
            if (G.Equal(Program.options.solve_forward_terminal, "exo")) terminal = ETerminalCondition.Exogenous;
            else if (G.Equal(Program.options.solve_forward_terminal, "const")) terminal = ETerminalCondition.ConstantLevel;
            else if (G.Equal(Program.options.solve_forward_terminal, "growth")) terminal = ETerminalCondition.ConstantGrowthRate;
            int ftMax = 1;
            int ftMin = 1;

            if (usingFairTaylor || usingNewtonFairTaylor)
            {
                if (usingFairTaylor)
                {
                    ftMax = Program.options.solve_forward_fair_itermax;
                    ftMin = Program.options.solve_forward_fair_itermin;
                }
                else if (usingNewtonFairTaylor)
                {
                    //ftMax = Program.options.solve_forward_nfair_itermax;
                    ftMax = Program.options.solve_forward_nfair_itermax;
                    ftMin = Program.options.solve_forward_nfair_itermin;
                }
                else throw new GekkoException();

                foreach (int leadVar in Program.model.modelGekko.leadedVariables.Keys)
                {
                    //if (Program.model.modelGekko.m2.endogenous.ContainsKey(leadVar)) ... go from bnumber to varname #980734323
                    leadedVarsList.Add(leadVar);
                }
                G.Writeln("+++ NOTE: There are " + Program.model.modelGekko.leadedVariables.Count + " variable(s) with leads: Fair-Taylor algorithm is used");
            }

            double[,] oldNftJacobi = null;

            for (int ft = 0; ft < ftMax; ft++)
            {
                if (Program.options.solve_print_details && Globals.runningOnTTComputer)
                {
                    G.Writeln("");
                    G.Writeln("NEW FAIR TAYLOR ITERATION", Color.Red);
                    G.Writeln("");
                }

                if (usingFairTaylor || usingNewtonFairTaylor) startTime = DateTime.Now;
                int iterCounter = 0;
                int iterMax = int.MinValue;
                int iterMin = int.MaxValue;
                int iterNotSolved = 0;
                int iterConsecutiveNotSolved = 0;
                int iterConsecutiveNotSolvedHelper = 0;

                double[,] ftVars = null;
                if (usingFairTaylor || usingNewtonFairTaylor)
                {
                    //Put the leaded vars into a special array ftVars
                    ftVars = SolveForwardLooking.GetFtVars(largestLag, obsSimPeriod, a, leadedVarsList, ftVars);
                    HandleTerminals(largestLag, obsSimPeriod, a, leadedVarsList, terminal);
                }

                //a list of actions. The first action is a normal simulation.
                //if we use NFair, the actions are used to create the Jacobi matrix
                NewtonFairTaylorHelper helper = SolveForwardLooking.CreateNFairHelper(ref tStart, ref tEnd, usingNewtonFairTaylor, leadedVarsList, ft, oldNftJacobi);
                double[,] aFinalResultFromShockLoop = null;
                int counterJ = -1;
                foreach (NewtonFairTaylorHelper1 shock in helper.shocks)
                {
                    counterJ++;
                    if (!shock.isFirstBaseline) G.Writeln("    Gradient " + counterJ + " of " + (helper.shocks.Count - 1) + " (var " + (shock.varCounter + 1) + " per " + shock.gt.ToString() + ")");
                    double[,] aTemp = null;
                    double nftDelta = double.NaN;
                    if (shock.isFirstBaseline && helper.shocks.Count == 1)
                    {
                        //first iteration in shocks, when there is only 1 element
                        //This means that it is not Newton Fair-Taylor but normal Fair-Taylor (or it is the first iteration
                        //of Newton Fair-Taylor). The reason we make a pointer like this in such cases is just to save time
                        //to avoid copying the content of a into aTemp
                        aTemp = a;
                    }
                    else if (shock.isFirstBaseline && helper.shocks.Count > 1)
                    {
                        aTemp = new double[a.GetLength(0), a.GetLength(1)];
                        Array.Copy(a, aTemp, a.Length);
                        helper.jacobi = new double[helper.shocks.Count - 1, helper.shocks.Count - 1];
                    }
                    else
                    {
                        //second iteration and onwards, a is known from first iteration
                        //we only end here when doing NFair
                        aTemp = new double[a.GetLength(0), a.GetLength(1)];
                        Array.Copy(a, aTemp, a.Length);
                        //int bNumber = bNumberPointers[leadedVarsList[shock.varNumber]];
                        int bNumber = bNumberPointers[shock.varNumber];
                        nftDelta = Program.model.modelGekko.bVariance[bNumber] / 100d;
                        if (nftDelta == 0d || G.isNumericalError(nftDelta)) nftDelta = 1d;  //could be refined, perhaps looking at the level of the variable
                        aTemp[shock.varNumber, GekkoTime.Observations(tStart, shock.gt) - 1 - largestLag] += nftDelta;
                    }

                    SolveForwardLooking.SetTerminalType(terminal);

                    int tInt = -largestLag - 1;
                    foreach (GekkoTime t in new GekkoTimeIterator(tStart, tEnd.Add(-horizon2)))  //horizon2 is deducted. Is 0 for non-stacked models.
                    {
                        int distanceToEnd = GekkoTime.Observations(t, tEnd) - 1;  //0 if last period, 1 if second-last, and so on
                        Program.model.modelGekko.simulateResults[7] = distanceToEnd;

                        if (Program.options.solve_print_details && Globals.runningOnTTComputer)
                        {
                            G.Writeln("");
                            G.Writeln("NEW TIME PERIOD " + t.super + t.freq.ToString() + t.sub, Color.Green);
                            G.Writeln("");
                        }

                        tInt++;  //first time --> = -largestLag = model.largestLag (= positive number)

                        if (Globals.threadIsInProcessOfAborting) throw new GekkoException();

                        DateTime dt10 = DateTime.Now;

                        try
                        {
                            //BTypeData zz1 = model.varsBType["hw¤0"];
                            //BTypeData zz2 = model.varsBType["hw¤1"];
                            //G.Writeln("lkdsfjasljfd " + zz1.aNumber + zz2.aNumber);
                            //G.Writeln("before1 " + model.b[137] + " lead " + model.b[138] + " " + t.ToString());
                            SolveDataInOut.FromAToB(usingFairTaylor, usingNewtonFairTaylor, shock, ft, ec, work, timeSeriesPointers, extraWritebackPointers, lagPointers, aNumberPointers, endoNoLagPointers, endoLeadPointers, endoPointers, varNamePointers, isDJZvarPointers, aTemp, tInt, t, tStart, tEnd, so);
                            //G.Writeln("after1 " + model.b[137] + " lead " + model.b[138] + " " + t.ToString());
                        }
                        catch (Exception e)
                        {
                            if (!Program.options.solve_print_iter)
                            {
                                G.Write("    You may inspect the individual iterations in more detail");
                                Program.IterLink(output, " ", "here", "");
                                G.Writeln();
                            }
                            double[,] a2 = SolveDataInOut.FromAToDatabankWhileRememberingOldDatabank(tStart0, tStart, tEnd, debug, work, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, aTemp, NAN, bNumberPointers, endoNoLagPointers);
                            LinkContainer lc1;
                            LinkContainer lc2;
                            UndoAndPackStuff(out lc1, out lc2, tStart, tEnd, tStart0, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a2);
                            if (Program.FindException(e, "simFailure"))
                            {
                                SimPrintErrorOptionsUndo(lc1);
                                SimPrintErrorOptionsPack(lc2);
                                WriteAboutFailsafeOption();
                            }
                            else
                            {
                                SimPrintErrorOptionsUndo(lc1);
                                WriteAboutFailsafeOption();
                            }
                            throw;
                        }

                        ms1 += (DateTime.Now - dt10).TotalMilliseconds;

                        string culprit = "";

                        if (Globals.simulationCheckThatAllDataGetsFromBArrayToTimeSeries)
                        {
                            bCheck = new double[Program.model.modelGekko.b.Length];
                            System.Array.Copy(Program.model.modelGekko.b, bCheck, Program.model.modelGekko.b.Length);
                        }

                        DateTime t0 = DateTime.Now;

                        try
                        {
                            if (modelType == ECompiledModelType.After)
                            {
                                SolveAfter();
                                SolveRevertedT();
                                SolveRevertedY();
                                SolveRevertedAuto();
                            }
                            else if (modelType == ECompiledModelType.Res)
                            {
                                SolveRes(Program.model.modelGekko.b);
                            }
                            else if (modelType == ECompiledModelType.Gauss || modelType == ECompiledModelType.GaussFailSafe)
                            {
                                if (so.isFix && hasEndoExo)
                                {
                                    //This should never happen
                                    G.Writeln2("*** ERROR: Trying to solve SIM<fix> with Gauss Seidel");
                                    throw new GekkoException();
                                }
                                SolveGauss777.SolveGauss(usingFairTaylor || usingNewtonFairTaylor, Program.model.modelGekko.b, isDampedPointers, isDampedPointersArray, out culprit, modelType, t, checkoff);
                            }
                            else if (modelType == ECompiledModelType.Newton)
                            {
                                NewtonAlgorithmHelper nah = new NewtonAlgorithmHelper();
                                nah.t = t;
                                nah.tStart = tStart;
                                nah.tEnd = tEnd;

                                ModelGekko tempModel = Program.model.modelGekko;

                                SolveNewton777.SolveNewton(modelType, nah);
                            }
                            else throw new GekkoException();  //should be one of these
                        }
                        catch (Exception e)
                        {
                            if (!Program.options.solve_print_iter)
                            {
                                G.Write("    You may inspect the iterations");
                                Program.IterLink(output, " ", "here", "");
                                G.Writeln();
                            }
                            //write the stuff back to databank
                            double[,] a2 = SolveDataInOut.FromAToDatabankWhileRememberingOldDatabank(tStart0, tStart, tEnd, debug, work, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, aTemp, NAN, bNumberPointers, endoNoLagPointers);
                            LinkContainer lc1;
                            LinkContainer lc2;
                            UndoAndPackStuff(out lc1, out lc2, tStart, tEnd, tStart0, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a2);
                            SimPrintErrorOptionsUndo(lc1);
                            SimPrintErrorOptionsPack(lc2);
                            WriteAboutFailsafeOption();
                            throw;
                        }

                        int its = (int)Program.model.modelGekko.simulateResults[0];
                        iterCounter += its;
                        if (its > iterMax) iterMax = its;
                        if (its < iterMin) iterMin = its;
                        if ((G.Equal(so.method, "gauss") && its >= Program.options.solve_gauss_itermax) || (G.Equal(so.method, "newton") && its >= Program.options.solve_newton_itermax))
                        {
                            iterNotSolved++;
                            iterConsecutiveNotSolvedHelper++;
                            if (iterConsecutiveNotSolvedHelper > iterConsecutiveNotSolved) iterConsecutiveNotSolved = iterConsecutiveNotSolvedHelper;
                        }
                        else
                        {
                            iterConsecutiveNotSolvedHelper = 0;
                        }

                        simTime += (DateTime.Now - t0).TotalMilliseconds / 1000d;

                        bool isGaussConverged = true;
                        if ((int)Program.model.modelGekko.simulateResults[0] >= Program.options.solve_gauss_itermax) isGaussConverged = false;

                        DateTime dt11 = DateTime.Now;

                        //G.Writeln("SIM + before2 " + model.b[137] + " lead " + model.b[138] + " " + t.ToString());
                        SolveDataInOut.FromBToA(ref hasIssuedSeedWarning, bCheck, extraWritebackPointers, revertedPointers, aNumberPointers, endoNoLagPointers, varNamePointers, aTemp, tInt);
                        //G.Writeln("");


                        ms2 += (DateTime.Now - dt11).TotalMilliseconds;

                        IterationPrint(ref culprit, tStart, t, modelType, output, isGaussConverged, so);

                        if (Program.model.modelGekko.simulateResults[1] == 12345)
                        {
                            //abort NaN
                            double[,] a2 = SolveDataInOut.FromAToDatabankWhileRememberingOldDatabank(tStart0, tStart, tEnd, debug, work, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, aTemp, NAN, bNumberPointers, endoNoLagPointers);

                            int eqNumber = (int)Program.model.modelGekko.simulateResults[2];
                            EquationHelper eh = Program.model.modelGekko.equations[eqNumber];
                            G.Writeln();
                            G.Writeln("Numerical problem encountered in equation: " + eh.lhs);
                            G.Writeln("Simulation time period: " + tStart + " " + tEnd);
                            G.Writeln("Period being simulated: " + t);
                            G.Writeln("Gauss damping factor:   " + Program.options.solve_gauss_damp);
                            G.Writeln("Current iteration:      " + (int)Program.model.modelGekko.simulateResults[0]);
                            Program.PrintEquationVariables(t, eh);
                            G.Write("*** ERROR: Simulation failed");
                            if (!Program.options.solve_print_iter) Program.IterLink(output, " (", "more", ")");
                            G.Writeln();

                            //G.Writeln("    You may undo the simulation and revert to pre-simulation databank");
                            LinkContainer lc1;
                            LinkContainer lc2;
                            UndoAndPackStuff(out lc1, out lc2, tStart, tEnd, tStart0, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a2);
                            SimPrintErrorOptionsUndo(lc1);
                            SimPrintErrorOptionsPack(lc2);
                            WriteAboutFailsafeOption();
                            throw new GekkoException();
                        }
                    }  //end of foreach t
                    Globals.simCounter++;

                    if (usingFairTaylor || usingNewtonFairTaylor)
                    {
                        if (shock.isFirstBaseline)
                        {
                            aFinalResultFromShockLoop = aTemp;
                            //a = aTemp; //only the first one in shocks will become the 'a' array
                            bool ok = false;
                            SolveForwardLooking.CheckFairTaylorIteration(usingFairTaylor, usingNewtonFairTaylor, bNumberPointers, largestLag, obsSimPeriod, aTemp, leadedVarsList, ft, ftVars, out ok);
                            string extra = null; if (usingNewtonFairTaylor) extra = "Newton-";
                            string extra2 = null; if (usingNewtonFairTaylor) extra2 = "N";
                            if (ok && ft >= ftMin)
                            {
                                G.Writeln(extra + "Fair-Taylor (leads) algorithm converged in " + (ft + 1) + " " + extra2 + "FT-iterations (" + G.SecondsFormat((DateTime.Now - dtFt).TotalMilliseconds) + ")");
                                a = aFinalResultFromShockLoop;
                                goto JumpOut;
                            }
                            if (ft == ftMax - 1)
                            {
                                G.Writeln("+++ WARNING: " + extra + "Fair-Taylor algorithm did not converge in " + (ft + 1) + " " + extra2 + "FT-iterations (" + G.SecondsFormat((DateTime.Now - dtFt).TotalMilliseconds) + ")");
                                a = aFinalResultFromShockLoop;
                                goto JumpOut;
                            }
                        }
                        else
                        {
                            //Compute stuff for the NFT Jacobi matrix
                            int counterI = 0;  //1-based
                            foreach (NewtonFairTaylorHelper1 shock2 in helper.shocks)
                            {
                                if (shock2.isFirstBaseline) continue;  //the first one is skipped
                                counterI++;
                                int var = shock2.varNumber;
                                double v1 = aFinalResultFromShockLoop[shock2.varNumber, GekkoTime.Observations(tStart, shock2.gt) - 1 - largestLag];
                                double v2 = aTemp[shock2.varNumber, GekkoTime.Observations(tStart, shock2.gt) - 1 - largestLag];
                                double jac = (v2 - v1) / nftDelta;
                                //Remember that the DUMP matrices #ft_1 etc. are transposed relative to this!
                                helper.jacobi[counterJ - 1, counterI - 1] = jac;
                            }
                        }
                    }
                    else
                    {
                        aFinalResultFromShockLoop = aTemp; //maybe not necessary
                    }
                } //end of shock loop that computes Jacobi matric for Newton-Fair-Taylor
                a = aFinalResultFromShockLoop;  //stores a pointer to the a array that is done in the first shock iteration

                double time = (DateTime.Now - startTime).TotalMilliseconds;
                double avgIter = (double)iterCounter / (double)obsSimPeriodIncludingLeadsAtEnd;
                string avgIterString = avgIter.ToString("0.0");
                if (avgIter > 50) avgIterString = avgIter.ToString("0");
                string type = "";
                if (so.method.Length > 2) type = so.method.Substring(0, 1).ToUpper() + so.method.Substring(1);  //First letter in capitals
                string s5 = ""; if (usingFairTaylor || usingNewtonFairTaylor) s5 = "#" + (ft + 1) + ": ";
                string s3 = s5 + type + " simulation " + G.FromDateToString(tStart) + "-" + G.FromDateToString(tEnd) + " took " + G.SecondsFormat(time) + " -- " + iterMin + "/" + iterMax + "/" + avgIterString + " iterations (min/max/avg)";
                G.Write(s3);
                string s4 = "";
                if (iterNotSolved > 0)
                {
                    s4 = "+++ WARNING: " + iterNotSolved + " periods not converged (most consecutive: " + iterConsecutiveNotSolved + ")";
                }
                if (!Program.options.solve_print_iter)
                {
                    if (s4.Length > 0) output.AppendLine(s4);
                }
                if (!Program.options.solve_print_iter && !(usingFairTaylor || usingNewtonFairTaylor))
                {
                    //we avoid this when using FairTaylor, because it takes up too much memory
                    Program.IterLink(output, " (", "more", ")");
                }
                G.Writeln();
                if (debug)
                {
                    G.WritelnGray(" --> " + (time / 1000d * (100d / (tEnd.super - tStart.super + 1))));
                }
                if (s4.Length > 0) G.Writeln(s4);

                if (so.isFix)
                {
                    //SIM<fix>
                    if (hasEndoExo)
                    {
                        G.Writeln("+++ NOTE: " + Program.model.modelGekko.endogenized.Count + " ENDO/EXO vars (goals) were enforced with SIM<fix>");
                    }
                    else
                    {
                        G.Writeln("+++ NOTE: SIM<fix> did not enforce any goals, since there are no ENDO/EXO vars (goals) set");
                    }
                }
                else
                {
                    //normal SIM
                    if (hasEndoExo)
                    {
                        G.Writeln("+++ NOTE: There are " + Program.model.modelGekko.endogenized.Count + " ENDO/EXO vars (goals) set, you may use SIM<fix> to enforce them");
                    }
                    else
                    {
                        //do nothing
                    }
                }


                if (usingFairTaylor)
                {
                    SolveForwardLooking.HandleFairTaylorIteration(bNumberPointers, largestLag, tStart0, obsSimPeriod, a, leadedVarsList, ft, ftVars);
                }
                else if (usingNewtonFairTaylor)
                {
                    if (helper.jacobi != null)
                    {
                        oldNftJacobi = new double[helper.jacobi.GetLength(0), helper.jacobi.GetLength(1)];
                        Array.Copy(helper.jacobi, oldNftJacobi, helper.jacobi.Length);
                    }
                    HandleNewtonFairTaylorIteration(dtFt, bNumberPointers, largestLag, tStart0, obsSimPeriod, a, leadedVarsList, ft, ftVars, helper);
                }

            } //Fair-Taylor (ft) iterations

        JumpOut:;

            DateTime dt3 = DateTime.Now;

            if (debug) G.WritelnGray("from a[] to b[]: " + ms1 / 1000d);
            if (debug) G.WritelnGray("from b[] to a[]: " + ms2 / 1000d);

            if (Globals.alwaysEnablcPackForSimulation)  //this is mostly for debugging, "packsim" activates the link showing up always.
            {
                double[,] a2 = SolveDataInOut.FromAToDatabankWhileRememberingOldDatabank(tStart0, tStart, tEnd, debug, work, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a, NAN, bNumberPointers, endoNoLagPointers);
                LinkContainer lc1;
                LinkContainer lc2;
                UndoAndPackStuff(out lc1, out lc2, tStart, tEnd, tStart0, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a2);
                SimPrintErrorOptionsPack(lc2);
                WriteAboutFailsafeOption();
            }
            else
            {
                //This is faster, so the "pack" link is not shown as default.
                SolveDataInOut.FromAToDatabank(tStart, tEnd, debug, work, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a, bNumberPointers, endoNoLagPointers);
            }

            if (ec.simInitEndoMissingValueHelper != null)
            {
                ec.simInitEndoMissingValue = new List<string>();
                ec.simInitEndoMissingValue.Add("Normally, initial values for endogenous variables are lagged values from the databank. If such data is missing, Gekko tries ");
                ec.simInitEndoMissingValue.Add("some arbitrary number (0.12345) as starting value. Simulation will often converge anyway, but if it does not, please ");
                ec.simInitEndoMissingValue.Add("put in some meaningful lagged values in the databank. You may alternatively look into 'option solve init' to force Gekko to use ");
                ec.simInitEndoMissingValue.Add("current (unlagged) values as starting values for the endogenous variables.");
                ec.simInitEndoMissingValue.Add("Below is a list of endogenous variables that were initialized to 0.12345 before simulation:");
                ec.simInitEndoMissingValue.Add("");
                StringBuilder sb = new StringBuilder();
                int count = 0;
                foreach (string s in ec.simInitEndoMissingValueHelper)
                {
                    count++;
                    string blank = "";
                    if (count > 0) blank = Globals.blankUsedAsPadding;
                    string count2 = count.ToString();
                    ec.simInitEndoMissingValue.Add(blank + " " + G.Blanks(4 - count2.Length) + "#" + count + ": " + s);
                }
                ec.simInitEndoMissingValue.Add("");
                G.Write("+++ NOTE: "); G.WriteLink(ec.simInitEndoMissingValueHelper.Count.ToString(), "tab:output" + ec.counter + "a"); G.Writeln(" endogenous were given an arbitrary starting value (this is ok)");
                G.Writeln();
                ec.simInitEndoMissingValueHelper.Clear();  //not used anymore
                if (!Globals.outputTabTextContainer.ContainsKey(ec.counter.ToString()))
                {
                    Globals.outputTabTextContainer.Add(ec.counter.ToString(), ec);
                }
            }

            Globals.hasBeenEndoExoStatementsSinceLastSim = 0;
            Program.model.modelGekko.lastSimPer1 = tStart;
            Program.model.modelGekko.lastSimPer2 = tEnd;
            Program.model.modelGekko.lastSimStamp = Program.GetDateStamp();

            return;
        }

        public static void Res(GekkoTime tStart, GekkoTime tEnd)
        {
            //remember current values
            bool SimulateUseCurrentPeriodEndogenousRemember = Program.options.solve_data_init;
            int SimulateMaximumIterationsRemember = Program.options.solve_gauss_itermax;
            bool simulateStatic = Program.options.solve_static;
            bool fastGaussRemember = Globals.fastGauss;
            Program.options.solve_data_init = false;
            Program.options.solve_gauss_itermax = 1;
            Program.options.solve_static = true;  //could use so.isStatic instead
            Globals.fastGauss = false;  //never for RES, problem is that prologue and epilogue eqs feed into each other
            SimOptions so = new SimOptions();
            so.method = "res";
            try
            {
                SolveCommon.SimFast(tStart, tEnd, so);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                //revert to old values
                Program.options.solve_data_init = SimulateUseCurrentPeriodEndogenousRemember;
                Program.options.solve_gauss_itermax = SimulateMaximumIterationsRemember;
                Program.options.solve_static = simulateStatic;
                Globals.fastGauss = fastGaussRemember;
            }
        }


        public static void Efter(GekkoTime tStart, GekkoTime tEnd)
        {
            //ErrorIfDatabanksSwapped();
            bool SimulateUseCurrentPeriodEndogenousRemember = Program.options.solve_data_init;
            Program.options.solve_data_init = false;
            SimOptions so = new SimOptions();
            so.method = "reverted";
            try
            {
                SimFast(tStart, tEnd, so);
            }
            finally
            {
                //to make sure these are always reset
                Program.options.solve_data_init = SimulateUseCurrentPeriodEndogenousRemember;
            }
        }

        
        //See #98745239543
        private static bool CheckYesNoNullLogic(YesNoNull localOption, bool globalOption)
        {
            //                  global yes        global no
            //  ----------------------------------------------
            // local yes        true              true
            // local no         false             false
            // null             true              false
            return (globalOption && localOption != YesNoNull.No) || (!globalOption && localOption == YesNoNull.Yes);
        }

        private static ECompiledModelType GetModelTypeFromOptions(SimOptions so)
        {
            ECompiledModelType modelType = ECompiledModelType.Unknown;
            if (G.Equal(so.method, "gauss"))
            {
                if (Program.options.solve_failsafe)
                {
                    modelType = ECompiledModelType.GaussFailSafe;
                }
                else modelType = ECompiledModelType.Gauss;
            }
            else if (G.Equal(so.method, "res"))
            {
                modelType = ECompiledModelType.Res;
            }
            else if (G.Equal(so.method, "newton"))
            {
                modelType = ECompiledModelType.Newton;
            }
            else if (G.Equal(so.method, "reverted"))
            {
                modelType = ECompiledModelType.After;
            }
            else throw new GekkoException();
            return modelType;
        }



    }

    public static class SolveDataInOut
    {
        public static bool FromBToA(ref bool hasIssuedSeedWarning, double[] bCheck, int[] extraWritebackPointers, int[] revertedPointers, int[] aNumberPointers, int[] endoNoLagPointers, string[] varNamePointers, double[,] a, int tInt)
        {

            for (int i = 0; i < Program.model.modelGekko.varsBType.Count; i++)
            {
                //#84750237
                //This would probably be better done by comparing Program.model.modelGekko.b and bCheck, and if different
                //put it into a[]. But there are some issues with Infinity and NaN to be sorted out.
                //For now, probably best to keep it as it is.
                //Changing it would require investigating x.Equals(y), to see if it is appropriate for Infinity and NaN.
                if (endoNoLagPointers[i] == 1 || revertedPointers[i] == 1 || extraWritebackPointers[i] == 1)
                {
                    a[aNumberPointers[i], tInt] = Program.model.modelGekko.b[i];
                    if (!hasIssuedSeedWarning && Program.model.modelGekko.b[i] == Globals.missingValueSeedNumber)
                    {
                        //For safety:
                        G.WritelnGray("DEBUGGING: It seems there may be a problem with initializing missing values: 0.123454321: " + varNamePointers[i] + " t: " + tInt + ".");
                        hasIssuedSeedWarning = true;
                    }
                }
                else
                {
                    if (Globals.simulationCheckThatAllDataGetsFromBArrayToTimeSeries)
                    {
                        if (Program.model.modelGekko.b[i] != bCheck[i])  //probably is false if left side is 0 and right side is NaN. Not good.
                        {
                            //should change according to b[] arrays, but does not get written back.
                            string var = Program.model.modelGekko.varsBTypeInverted[i];
                            G.Writeln();
                            G.Writeln("*** ERROR: While backwriting from SIM command -- please report this error to the Gekko editor");
                            G.Writeln("*** ERROR: Variable: " + var);
                            G.Writeln();
                            throw new GekkoException();
                        }
                    }
                }
            }

            return hasIssuedSeedWarning;
        }

        public static void FromDatabankToA(GekkoTime tStart0, GekkoTime tEnd, Databank work, int obsWithLags, double[,] a, double[] NAN)
        {
            foreach (ATypeData atd in Program.model.modelGekko.varsAType.Values)
            {
                int length = -12345;
                string var = atd.varName;
                int id = atd.aNumber;
                int index1 = -12345;
                int index2 = -12345;
                double[] x_beware_do_not_change = null;
                Series ts = work.GetIVariable(var + Globals.freqIndicator + G.GetFreq(Program.options.freq)) as Series;  //Could have an A-array with Series...
                if (ts == null)
                {
                    if (SolveCommon.IsDjz(var))
                    {
                        length = obsWithLags;
                        x_beware_do_not_change = NAN;
                        index1 = 0;
                    }
                    else
                    {
                        //should not be possible: should have been caught in SimCheckFirstPeriodForMissingStuff()
                        G.Writeln2("*** ERROR: Internal Gekko error #874439849");
                        throw new GekkoException();
                    }
                }
                else
                {
                    x_beware_do_not_change = ts.GetDataSequenceUnsafePointerReadOnlyBEWARE(out index1, out index2, tStart0, tEnd);  //no setting of start/end period of timeseries. Does not optionally change NaN to 0, there is a solve option for that
                    length = index2 - index1 + 1;
                }
                //BEWARE: Do not alter x here
                Buffer.BlockCopy(x_beware_do_not_change, 8 * index1, a, 8 * id * obsWithLags, 8 * length);  //TODO: what if out of bounds regarding x???
                //I guess after this loop is done, the whole of a[,] will be filled with data or NaN.
                //It should not be possible that there is a 0 left originating from "double[,] a = new double[vars, obs];"
            }
        }


        public static double[,] FromAToDatabankWhileRememberingOldDatabank(GekkoTime tStart0, GekkoTime tStart, GekkoTime tEnd, bool debug, Databank work, int obsWithLags, int obsSimPeriod, double[,] a, double[] NAN, int[] bNumberPointers, int[] endoNoLagPointers)
        {
            double[,] a2 = new double[a.GetLength(0), a.GetLength(1)];
            Array.Copy(a, a2, a.Length);
            FromDatabankToA(tStart0, tEnd, work, obsWithLags, a2, NAN);  //NOTE: put into a2 array (will be equal to a array as it were at the beginning)
            FromAToDatabank(tStart, tEnd, debug, work, obsWithLags, obsSimPeriod, a, bNumberPointers, endoNoLagPointers);
            return a2;
        }

        public static void FromAToB(bool usingFairTaylor, bool usingNewtonFairTaylor, NewtonFairTaylorHelper1 shock, int ft, ErrorContainer ec, Databank work, Series[] timeSeriesPointers, int[] extraWritebackPointers, int[] lagPointers, int[] aNumberPointers, int[] endoNoLagPointers, int[] endoLeadPointers, int[] endoPointers, string[] varNamePointers, int[] isDJZvarPointers, double[,] a, int tInt, GekkoTime t, GekkoTime tStart, GekkoTime tEnd, SimOptions so)
        {
            bool ftOrNft = usingFairTaylor || usingNewtonFairTaylor;

            for (int i = 0; i < Program.model.modelGekko.varsBType.Count; i++)
            {

                /*
                 * The logic is more or less like this for the simple case with init=yes and no fair-taylor 
                 * 
                IF series does not exist
                    IF DJZ-type series
                        create series and set value to 0
                    ELSE
                        abort with error
                ELSE
                    IF endo with no lag   
                        use lagged value, or the value 0.12345
                    ELSE
                        IF databank value = NaN
                            IF DJZ-type series
                                set value to 0
                            ELSE
                                use value from databank

                */


                double val = double.NaN;
                Series ts = timeSeriesPointers[i];
                string variable = varNamePointers[i];
                if (ts == null)
                {
                    if (isDJZvarPointers[i] == 1)
                    {
                        val = 0d;  //DJZ set to 0 --> will end up in b[]
                                   //J-factor or D or Z variable
                                   //see also #7235432894539
                        Series tsNew = new Series(Program.options.freq, variable + Globals.freqIndicator + G.GetFreq(Program.options.freq));
                        work.AddIVariable(tsNew.name, tsNew);
                        timeSeriesPointers[i] = tsNew;
                        extraWritebackPointers[i] = 1;  //to make sure it gets written back from b[] to a[,] array

                        //Program.model.modelGekko.b[i] = setValue;
                        //if it is a J type, and it has become endogenous, a 0 is fine as starting value (better than lagged J)
                    }
                    else
                    {
                        //missing variable, and not a DJZ-type variable
                        //This will probably never happen, since it gets checked before
                        G.Writeln2("*** ERROR: time series '" + variable + "' does not exist in the Work bank");
                        //FIXME: undo, or at least write if DJZ variables have been created.
                        throw new GekkoException();
                    }
                }
                else
                {
                    //Variable exists in Work bank

                    //---- regarding NaN or 0 ---------
                    //  if options.solve_data_ignoremissing is true, all NaN are replaced by 0,
                    //  except lagged endogeous -- these are treated as usual (given 0.12345).
                    //---------------------------------

                    val = double.NaN;
                    int yy = aNumberPointers[i];

                    //HERE we should have a big IF, if it is NFT doing a "real" shock (not the baseline).
                    //if so, we jump directly to //oiauewrwuer


                    int alternative = -12345;
                    if (endoNoLagPointers[i] == 1 && ft == 0)
                    {
                        //Will only init if init option is = yes, and never in later FT-iterations
                        //Note that all shocks will always be set to alternative = 3 later on.
                        alternative = 1;
                    }
                    else if (Program.options.solve_data_init && ftOrNft && ft == 0 && endoLeadPointers[i] == 1)
                    {
                        alternative = 2;
                    }
                    else
                    {
                        alternative = 3;  //takes plain value
                    }
                    if (!shock.isFirstBaseline)
                    {
                        //for NFT, a lot of the simulations are used to create jacobi matrix
                        //these cases should NEVER have leaded variables initialized, since
                        //these variables are perturbed just before calling this method.
                        //Regarding the first Fair-Taylor iteration (ft = 0), the baseline 'shock'
                        //of these should be initialized (alternative 2), but the following non-baseline
                        //calls should not. Hence, these will be overwritten from alternative = 2 to
                        //alternative = 3 here.
                        alternative = 3;
                    }

                    switch (alternative)
                    {
                        case 1:
                            {
                                //Will only init if init option is = yes
                                InitEndoNoLag(ec, a, tInt, ref t, so, ref val, ts, yy);
                            }
                            break;
                        case 2:
                            {
                                val = InitEndoLeaded(a, tInt, val, yy);
                            }
                            break;
                        case 3:
                            {
                                InitEndoLaggedOrExo(extraWritebackPointers, lagPointers, endoPointers, isDJZvarPointers, a, tInt, ref t, ref tStart, ref tEnd, i, ref val, ts, variable, yy, so.isStatic);
                            }
                            break;
                    } //end switch
                }

                Program.model.modelGekko.b[i] = val;
            } //for each b[i]

            return;
        }

        public static void FromAToDatabank(GekkoTime tStart, GekkoTime tEnd, bool debug, Databank work, int obsWithLags, int obsSimPeriod, double[,] a, int[] bNumberPointers, int[] endoNoLagPointers)
        {
            //TODO: this loop could be speed-optimized. Having the list of Series pre-done would help,
            //      instead of this looping. Maybe even a list of pointers to x[]-arrays pre-done.
            //      But still, reading in and out of a[] is not that costly.
            //      HMM, is the loop also writing back exogenous vars?
            if (!work.editable)
            {
                //NB: This check is here, to avoid having to do it for each timeseries later on.
                //    The data is written in a special (fast) way that does not get checked automatically regarding
                //    dirty and protect, cf. //#98726527
                G.Writeln2("*** ERROR: You are trying to simulate with a first-position databank ('" + work.name + "') that is non-editable");
                throw new GekkoException();
            }
            DateTime dt4 = DateTime.Now;

            string s = O.ShowDatesAsString(tStart, tEnd);
            string src = s + "SIM " + Path.GetFileName(Program.model.modelGekko.modelInfo.fileName) + " (hash " + Program.model.modelGekko.modelHashTrue + ")";            //string stamp = Program.GetDateStampCache();
            foreach (ATypeData atd in Program.model.modelGekko.varsAType.Values)
            {
                string var = atd.varName;
                int id = atd.aNumber;
                Series ts = work.GetIVariable(var + Globals.freqIndicator + G.GetFreq(Program.options.freq)) as Series;  //Could have an A-array with Series...

                if (ts == null && SolveCommon.IsDjz(var))
                {
                    //can be autocreated, this probably will never happen, since it is already created,
                    //see #7235432894539
                    ts = new Series(Program.options.freq, var + Globals.freqIndicator + G.GetFreq(Program.options.freq));
                    work.AddIVariable(var + Globals.freqIndicator + G.GetFreq(Program.options.freq), ts);
                }

                //??? what if above is null??? << create it if djz?
                int index1 = -12345;
                int index2 = -12345;
                double[] x_beware_if_changed = ts.GetDataSequenceUnsafePointerAlterBEWARE(out index1, out index2, tStart, tEnd);  //do not optionally change NaN to 0 here

                //#98726527

                int length = index2 - index1 + 1;  //only done for sim period, not from tStart0 (i.e. lags)
                Buffer.BlockCopy(a, 8 * id * obsWithLags + 8 * (obsWithLags - obsSimPeriod), x_beware_if_changed, 8 * (index1), 8 * length); //TODO: what if out of bounds regarding x???
                if (bNumberPointers != null)
                {
                    int b = bNumberPointers[id];
                    if (b != -12345)
                    {
                        if (endoNoLagPointers[b] == 1)
                        {
                            ts.meta.source = src;
                            ts.Stamp();
                            ts.SetDirty(true);
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                }
            }
            if (debug) G.WritelnGray("reading back from a[]: " + (DateTime.Now - dt4).TotalMilliseconds / 1000d);
        }
    }

    public static class SolveGauss777
    {
        public static void RunOneGaussIterationWithDamping(List<int> isDampedPointers, Type assembly, Object[] args, double[] bOld)
        {
            double[] b = (double[])args[0];
            //double[] simulateResults = (double[])args[1];
            assembly.InvokeMember("eqs", BindingFlags.InvokeMethod, null, null, args);
            if (Program.model.modelGekko.simulateResults[1] != 12345)
            {
                //Do not damp if the iteration just failed -- that will just pollute the results.
                //It is in that case better to keep the results excactly as they were when returning
                //from the assembly (these results will later on in the Sim() method be written
                //back to timerseries.
                DampVariables(b, bOld, isDampedPointers);
            }
        }



        public static bool CheckRelativeDifferenceSmart(double dampingCorrection, bool factor, double historicalVariance, double absCrit, double relCrit, double valNew, double valOld, out double rel1)
        {
            rel1 = double.NaN;
            double histVar = historicalVariance;
            if (histVar == -12345d)
            {
                G.Writeln2("*** ERROR: hist variance");
                throw new GekkoException();
            }
            if (histVar == double.NaN)
            {
                //fortunetely this is quite seldom, and the line below means relative check gets completely switched off...
                histVar = double.MaxValue;
            }
            double absolute = Math.Abs(valNew - valOld) * dampingCorrection; //for damped variables, this gives the "true" non-damped difference between iterations
            double relative = absolute / histVar;  //denominator is always >= 0, or positive infinity or NaN
            if (G.isNumericalError(relative))  //happens when historicalVariance = 0
            {
                relative = double.MaxValue;  //same as saying relative crit MUST be met.
                //if so, we have 0/0 = NaN or x/0 = inf.
                //the first one could be deemed ok, but it will be ok by absolute crit anyway.
                //then we need to meet the absolute crit instead
                //maybe this is bad for some large variables, could alternatively
                //ignore the variable altogether
            }
            bool good = false;
            if (factor)
            {
                absolute = 2d * absolute;
                relative = 2d * relative;
            }
            if (absolute < absCrit || relative < relCrit)
            {
                good = true;
            }
            else
            {
                good = false;
            };
            rel1 = relative;  //rel1 is returned
            return good;
        }


        /// <summary>
        /// Solve by means of Gauss-Seidel method
        /// </summary>
        /// <param name="b">Array with variables</param>
        /// <param name="simulateResults">Results</param>
        /// <param name="culprit">Last variable to converge</param>
        public static void SolveGauss(bool ftOrNft, double[] b, List<int> isDampedPointers, int[] isDampedPointersArray, out string culprit, ECompiledModelType modelType, GekkoTime t, Dictionary<int, int> checkoff)
        {
            List<IterMemory> iterMemories = null;

            if (Program.options.solve_gauss_dump)
            {
                iterMemories = new List<IterMemory>();
                if (ftOrNft)
                {
                    G.Writeln2("*** ERROR: 'OPTION solve gauss dump' cannot be set while doing Fair-Taylor (would exhaust memory)");
                    throw new GekkoException();
                }
                Program.model.modelGekko.bMemory.Add(t.ToString(), iterMemories);
            }

            DateTime t0 = DateTime.Now;

            Type assembly = SolveCommon.GetAssemblyFromModelType(modelType);

            //This should never happen, since FIX sets Newton algo, and if FIX is not set, Gauss algo
            //is fine even if there are endo/exo
            //if (Program.model.modelGekko.endogenized.Count != 0 || Program.model.modelGekko.exogenized.Count != 0)
            //{
            //    G.Writeln("+++ ERROR: There are ENDO/EXO variables set. You should clear the goals,");
            //    G.Writeln("           or use the Newton algorithm ('OPTION solve method = newton').", Color.Red);
            //    throw new GekkoException();
            //}

            culprit = "";
            Object[] args = new Object[1];
            args[0] = b;

            //HACK, fixme, todo, NEVER for RES-type simulation
            if (Program.model.modelGekko.m2.endogenous.ContainsKey("qJzdk"))
            {
                Globals.convergenceCheckVariables = new string[] { "qJzdk" };  //emma
            }
            else if (Program.model.modelGekko.m2.endogenous.ContainsKey("ys1"))
            {
                Globals.convergenceCheckVariables = new string[] { "ys1" };  //adam
            }
            else if (Program.model.modelGekko.m2.endogenous.ContainsKey("fcb"))
            {
                Globals.convergenceCheckVariables = new string[] { "fcb" };  //adam
            }
            else if (Program.model.modelGekko.m2.endogenous.ContainsKey("e__wn_mz"))  //saffier model
            {
                Globals.convergenceCheckVariables = new string[] { "e__wn_mz" };
            }

            int varId = -12345;
            string var = Globals.convergenceCheckVariables[0];

            //BTypeData ti = (BTypeData)model.varsBType[var + Globals.lagIndicator + "0"];
            BTypeData ti = null; Program.model.modelGekko.varsBType.TryGetValue(var + Globals.lagIndicator + "0", out ti);
            if (ti == null)
            {
                if (Globals.mayPrintConvergenceCheckVariableMissing == true)
                {
                    //G.Writeln("+++ WARNING: " + var + " does not exist for convergence check");
                    //Globals.mayPrintConvergenceCheckVariableMissing = false;
                }
            }
            else
            {
                varId = ti.bNumber;
            }

            if (Globals.fastGauss)
            {
                if (modelType == ECompiledModelType.GaussFailSafe)
                {
                    Program.model.modelGekko.simulateResults[1] = 0;
                    Program.model.modelGekko.m2.assemblyPrologueEpilogueFailSafe.InvokeMember("prologue", BindingFlags.InvokeMethod, null, null, args);
                }
                else
                {
                    Program.model.modelGekko.simulateResults[1] = 0;
                    Program.model.modelGekko.m2.assemblyPrologueEpilogue.InvokeMember("prologue", BindingFlags.InvokeMethod, null, null, args);
                }

                if (Program.model.modelGekko.simulateResults[1] == 12345)
                {
                    Program.model.modelGekko.simulateResults[0] = 0;  //no iteration has been done yet
                    return;  //failsafe fast return
                }
            }

            int iterCounter = 0;

            int convType = 1;
            if (G.Equal(Program.options.solve_gauss_conv, "conv2")) convType = 2;

            int culprit2 = -12345;
            if (iterCounter < Program.options.solve_gauss_itermax && (Globals.solveUseStrictCrits || varId == -12345))
            {
                System.Array.Copy(b, Program.model.modelGekko.bOld, b.Length);  //seems quite fast -- and simplifies damping etc.
                bool probing1Variable = true;  //does not seem to give any significant speedup... hmm
                if (varId == -12345) probing1Variable = false;  //if the var name is not found

                for (int i = 0; i < int.MaxValue; i++)
                {
                    IterMemory iterMemory = null;

                    iterCounter++;
                    Program.model.modelGekko.simulateResults[1] = 0;

                    if (Program.options.solve_gauss_dump)
                    {
                        iterMemory = new IterMemory();
                        try
                        {
                            iterMemory.bBefore = new double[b.Length];
                        }
                        catch (Exception e)
                        {
                            G.Writeln2("*** ERROR: Out of memory -- please run for fewer years with option 'dump'");
                            G.Writeln("           The dump option is quite memory-intensive, since it remembers all", Color.Red);
                            G.Writeln("           intermediate simulation values.", Color.Red);
                            throw new GekkoException();
                        }
                        System.Array.Copy(b, iterMemory.bBefore, b.Length);
                    }

                    try
                    {
                        RunOneGaussIterationWithDamping(isDampedPointers, assembly, args, Program.model.modelGekko.bOld);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (Program.options.solve_gauss_dump)
                        {
                            try
                            {
                                iterMemory.bAfter = new double[b.Length];
                            }
                            catch (Exception e)
                            {
                                G.Writeln2("*** ERROR: Out of memory -- please run for fewer years with option 'dump'");
                                G.Writeln("           The dump option is quite memory-intensive, since it remembers all", Color.Red);
                                G.Writeln("           intermediate simulation values.", Color.Red);
                                throw new GekkoException();
                            }
                            System.Array.Copy(b, iterMemory.bAfter, b.Length);
                            iterMemories.Add(iterMemory);
                        }
                    }

                    if (Program.model.modelGekko.simulateResults[1] == 12345)
                    {
                        Program.model.modelGekko.simulateResults[0] = iterCounter;
                        return;  //failsafe fast return
                    }

                    if (iterCounter >= Program.options.solve_gauss_itermax) break;

                    if (iterCounter < Program.options.solve_gauss_itermin)
                    {
                        System.Array.Copy(b, Program.model.modelGekko.bOld, b.Length);
                        continue;
                    }

                    bool converged = true;

                    if (probing1Variable)
                    {
                        double abs; double rel1; double rel2;
                        converged = GaussConvergenceOk(false, false, isDampedPointersArray, b[varId], Program.model.modelGekko.bOld[varId], varId, true, convType, out abs, out rel1, out rel2);
                        if (converged == false)
                        {
                            culprit2 = varId;
                        }
                    }
                    else
                    {
                        //int ii = 0;
                        foreach (int j in Program.model.modelGekko.endogenousBNumbersOriginalInModelList)
                        {
                            //ii++;
                            if (checkoff.ContainsKey(j))
                            {
                                //do nothing
                            }
                            else
                            {
                                double abs; double rel1; double rel2;
                                bool good = GaussConvergenceOk(false, false, isDampedPointersArray, b[j], Program.model.modelGekko.bOld[j], j, false, convType, out abs, out rel1, out rel2);
                                if (good == false)
                                {
                                    culprit2 = j;
                                    converged = false;
                                    //G.Write(" ii" + ii + " ");
                                    break;
                                }
                            }
                        }
                        //G.Writeln("real " + iterCounter + " result " + converged);
                    }

                    if (converged == true && probing1Variable == true)  //done with probe
                    {
                        converged = false;
                        probing1Variable = false;
                        Program.model.modelGekko.simulateResults[2] = culprit2;  //probably irrelevant
                        Program.model.modelGekko.simulateResults[3] = iterCounter;
                        //G.Writeln("shifting");
                        //continue iterating, and now check all variables (may cost 1 superfluous iteration, but never mind)
                    }


                    if (converged == true)  //done with all simulation: SUCCESS
                    {
                        Program.model.modelGekko.simulateResults[2] = culprit2;
                        Program.model.modelGekko.simulateResults[4] = iterCounter;
                        break;  //no more iterations
                    }

                    //G.Writeln("continuing");
                    //not converged if we get here
                    System.Array.Copy(b, Program.model.modelGekko.bOld, b.Length);
                }
            }

            if (culprit2 == -12345)
            {
                culprit = "";
            }
            else
            {
                if (false)  //This stuff is just not working. Try a newton first, and then switch to gauss --> errors
                {
                    try
                    {
                        culprit = G.FromBNumberToVarname(culprit2);
                    }
                    catch
                    {
                        //??? WHY IS THIS NOT A PROBLEM?
                    };
                }

            }

            //reverted equations (J-factors and so on)

            if (Globals.fastGauss)
            {
                if (false && modelType == ECompiledModelType.GaussFailSafe)
                {
                    //This is switched off. Often it is ok that epilogue variables produce missing values.
                    //The model can be deemed converged quite ok, and still have som epilogue variables
                    //with division 0 etc. The difference is that if a prologue or simultaneous variable
                    //produces a missing, and the simulations are continued, the result is that ALL
                    //simultanous variables end up with missing values.
                    //TODO: Maybe the failsafe option should just count how many missings are produced
                    //      and report it.
                    //simulateResults[1] = 0;
                    //Program.model.modelGekko.m2.assemblyCommonFailSafe.InvokeMember("epilogue", BindingFlags.InvokeMethod, null, null, args);
                }
                else
                {
                    //if failsafe is on, this is run without failsafe -- and may produce some missing values!
                    Program.model.modelGekko.simulateResults[1] = 0;
                    Program.model.modelGekko.m2.assemblyPrologueEpilogue.InvokeMember("epilogue", BindingFlags.InvokeMethod, null, null, args);
                }
                if (Program.model.modelGekko.simulateResults[1] == 12345)
                {
                    Program.model.modelGekko.simulateResults[0] = iterCounter;
                    return;  //failsafe fast return
                }
            }


            if (modelType == ECompiledModelType.GaussFailSafe)
            {
                Program.model.modelGekko.assemblyRevertedFailSafe.InvokeMember("reverted" + Globals.equationCodeT.ToUpper(), BindingFlags.InvokeMethod, null, null, args);
                Program.model.modelGekko.assemblyRevertedFailSafe.InvokeMember("reverted" + Globals.equationCodeY.ToUpper(), BindingFlags.InvokeMethod, null, null, args);
                Program.model.modelGekko.assemblyRevertedFailSafe.InvokeMember("revertedAuto", BindingFlags.InvokeMethod, null, null, args);
            }
            else
            {
                Program.model.modelGekko.assemblyReverted.InvokeMember("reverted" + Globals.equationCodeT.ToUpper(), BindingFlags.InvokeMethod, null, null, args);
                Program.model.modelGekko.assemblyReverted.InvokeMember("reverted" + Globals.equationCodeY.ToUpper(), BindingFlags.InvokeMethod, null, null, args);
                Program.model.modelGekko.assemblyReverted.InvokeMember("revertedAuto", BindingFlags.InvokeMethod, null, null, args);
            }

            Program.model.modelGekko.simulateResults[0] = iterCounter;
        }

        public static bool GaussConvergenceOk(bool isFairTaylor, bool isNewtonFairTaylor, int[] isDampedPointersArray, double bj, double bOldj, int j, bool factor, int convType, out double abs, out double rel1, out double rel2)
        {

            //rel1 will be the rel crit using historical variance, whereas rel2 will always return double.NaN.

            //!!If something is changed here, remember to change in Itershow() also

            double dampingCorrection = 1d;
            if (!(isFairTaylor || isNewtonFairTaylor) && isDampedPointersArray[j] == 1)  //if array is null, it is because it is a Fair-Taylor check, where Gauss damping is abstracted away (Fair-Taylor has its own damping)
            {
                dampingCorrection = 1d / (1d - Program.options.solve_gauss_damp); //must multiply difference with this for damped variables, otherwise they look too good!
            }

            abs = double.NaN; rel1 = double.NaN; rel2 = double.NaN;
            if (convType == 1)
            {
                double historicalVariance = Program.model.modelGekko.bVariance[j];
                double absCrit = Program.options.solve_gauss_conv1_abs;
                double relCrit = Program.options.solve_gauss_conv1_rel;
                if (isFairTaylor)
                {
                    absCrit = Program.options.solve_forward_fair_conv1_abs;
                    relCrit = Program.options.solve_forward_fair_conv1_rel;
                }
                else if (isNewtonFairTaylor)
                {
                    absCrit = Program.options.solve_forward_nfair_conv1_abs;
                    relCrit = Program.options.solve_forward_nfair_conv1_rel;
                }
                double valNew = bj;
                double valOld = bOldj;
                //this looks at historical differences from period to period in order to judge whether a deviation between
                //values is 'small' or 'large'
                double relative = double.NaN;
                bool good = CheckRelativeDifferenceSmart(dampingCorrection, factor, historicalVariance, absCrit, relCrit, valNew, valOld, out relative);
                rel1 = relative;
                return good;
            }
            else if (convType == 2)  //the PCIM way
            {
                double x = Math.Abs(bOldj);
                double absolute = Math.Abs(bj - bOldj) * dampingCorrection; //for damped variables, this gives the "true" non-damped difference between iterations
                double relative = absolute / x;
                if (G.isNumericalError(relative))  //happens when historicalVariance = 0
                {
                    if (Math.Abs(bOldj) == 0d) relative = 0d;  //  0/0
                    else relative = double.PositiveInfinity;     //  x/0
                }
                bool good = false;
                if (factor)
                {
                    absolute = 2d * absolute;
                    relative = 2d * relative;
                }

                double tabs = Program.options.solve_gauss_conv2_tabs;
                double trel = Program.options.solve_gauss_conv2_trel;
                if (isFairTaylor)
                {
                    tabs = Program.options.solve_forward_fair_conv2_tabs;
                    trel = Program.options.solve_forward_fair_conv2_trel;
                }
                else if (isNewtonFairTaylor)
                {
                    tabs = Program.options.solve_forward_nfair_conv2_tabs;
                    trel = Program.options.solve_forward_nfair_conv2_trel;
                }

                if (x < tabs)
                {
                    //always ignore variables with a level < 1.0
                    good = true;
                }
                else
                {
                    if (relative < trel)
                    {
                        //good if relaive change is < 0.0001
                        good = true;
                    }
                    else
                    {
                        good = false;
                    }
                };
                return good;
            }
            else
            {
                G.Writeln2("*** ERROR: #98349834");
                throw new GekkoException();
            }
        }
    }

    public static class SolveNewton777
    {

        public static void SolveNewton(ECompiledModelType modelType, NewtonAlgorithmHelper nah)
        {

            //bit of a hack
            Program.model.modelGekko.m2.fromEqNumberToBNumber = Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW;
            Program.model.modelGekko.m2.fromBNumberToEqNumber = Program.model.modelGekko.m2.fromBNumberToEqNumberFeedbackNEW;

            Object[] args = new Object[1];
            args[0] = Program.model.modelGekko.b;
            Program.model.modelGekko.m2.assemblyPrologueEpilogue.InvokeMember("prologue", BindingFlags.InvokeMethod, null, null, args);

            if (Globals.gradientSolve)
            {
                SolveGradient.SolveGradientAlgorithmUsingAlglib(Program.model.modelGekko.b, Program.model.modelGekko.m2.assemblyNewton, nah);
            }
            else
            {
                Program.SolveNewtonAlgorithm(Program.model.modelGekko.b, Program.model.modelGekko.m2.assemblyNewton, nah);
            }

            args = new Object[1];
            args[0] = Program.model.modelGekko.b;
            Program.model.modelGekko.m2.assemblyPrologueEpilogue.InvokeMember("epilogue", BindingFlags.InvokeMethod, null, null, args);

            Object[] args2 = new Object[1];
            args2[0] = Program.model.modelGekko.b;

            Program.model.modelGekko.assemblyReverted.InvokeMember("reverted" + Globals.equationCodeT.ToUpper(), BindingFlags.InvokeMethod, null, null, args2);
            Program.model.modelGekko.assemblyReverted.InvokeMember("reverted" + Globals.equationCodeY.ToUpper(), BindingFlags.InvokeMethod, null, null, args2);
            Program.model.modelGekko.assemblyReverted.InvokeMember("revertedAuto", BindingFlags.InvokeMethod, null, null, args2);

        }

    }

    public static class SolveGradient
    {

        public static CGSolverOutput SolveGradientAlgorithm(double[] x_input, Func<double[], CGSolverInput, double> func, CGSolverInput input)
        {
            //https://en.wikipedia.org/wiki/Nonlinear_conjugate_gradient_method

            if (input.restartInterval == 0) throw new GekkoException();

            int n = x_input.Length;

            CGSolverOutput output = new CGSolverOutput();

            double[] x = (double[])x_input.Clone();
            double[] dx_old = null;
            double[] s_old = null;

            int iterations = 0;

            double[] gradient = new double[n];
            double[] dx = new double[n];
            double beta = double.NaN;
            double[] s = new double[n];


            int jj = -1;  //iterations since resetting
            for (int j = 0; j < int.MaxValue; j++)
            {
                jj++;

                if (j == 0)
                {
                    SolveGradientAlgorithHelper(gradient, func, x, input);
                    for (int i = 0; i < n; i++)
                    {
                        dx[i] = -gradient[i];
                    }

                    double alpha = Globals.naiveGradient;
                    if (double.IsNaN(Globals.naiveGradient)) alpha = Golden(x, dx, func, input);

                    for (int i = 0; i < n; i++)
                    {
                        x[i] += alpha * dx[i];
                    }
                    dx_old = (double[])dx.Clone();
                    s_old = (double[])dx.Clone();
                }
                else
                {
                    SolveGradientAlgorithHelper(gradient, func, x, input);
                    for (int i = 0; i < n; i++)
                    {
                        dx[i] = -gradient[i];
                    }

                    double fraction1 = 0d;
                    double fraction2 = 0d;
                    for (int i = 0; i < n; i++)
                    {
                        fraction1 += dx[i] * dx[i];
                        fraction2 += s_old[i] * (dx[i] - dx_old[i]);
                    }
                    beta = -fraction1 / fraction2;

                    if (beta < 0d) beta = 0d;  //actually restarting, this is always done, also if .limitBeta == false
                    if (input.limitBeta)
                    {
                        if (beta >= 1d) beta = 0.9999d;  //maybe not always a good, even if it means that momentum is > 1...
                    }

                    int interval = n;
                    if (input.restartInterval != -12345) interval = input.restartInterval;

                    if (jj >= interval)
                    {
                        beta = 0d; //reset, because of rounding problems
                        jj = 0;
                    }

                    if (!double.IsNaN(Globals.naiveMomentum)) beta = Globals.naiveMomentum;

                    for (int i = 0; i < n; i++)
                    {
                        s[i] = dx[i] + beta * s_old[i];
                    }

                    if (false)
                    {
                        StreamWriter sw = new StreamWriter(@"c:\Thomas\Desktop\gekko\testing\golden.csv");
                        double[] xx = new double[x.Length];
                        for (double a = 0d; a <= 2000d; a += 0.01d)
                        {
                            goldenHelper1(xx, s, x, a);
                            double f = func(xx, input);
                            sw.WriteLine(a + "; " + f);
                        }
                        sw.Flush(); sw.Close();
                    }

                    double alpha = Globals.naiveGradient;
                    if (double.IsNaN(Globals.naiveGradient)) alpha = Golden(x, s, func, input);

                    for (int i = 0; i < n; i++)
                    {
                        x[i] += alpha * s[i];
                    }

                    dx_old = (double[])dx.Clone();
                    s_old = (double[])s.Clone();
                }

                double f2 = func(x, input);
                if (f2 < input.krit)
                {
                    output.iterations = j + 1;
                    output.f = f2;
                    output.x = x;
                    output.evals = input.evals; input.evals = 0;
                    break;
                }

            }

            //converged
            //Program.model.modelGekko.simulateResults[0] = iterations;
            //Program.model.modelGekko.simulateResults[1] = Math.Sqrt(RssNonScaled(residuals));


            return output;

        }

        public static double Golden(double[] x777, double[] direction, Func<double[], CGSolverInput, double> func, CGSolverInput input)
        {
            //https://en.wikipedia.org/wiki/Golden-section_search, see Python example

            double[] x = (double[])x777.Clone();
            double[] xOriginal = (double[])x777.Clone();

            double invphi = (Math.Sqrt(5d) - 1d) / 2d; // 1/phi                                                                                                                     
            double invphi2 = (3d - Math.Sqrt(5d)) / 2d;

            double a = 0d;

            goldenHelper1(x, direction, xOriginal, a);
            double fStart = func(x, input);

            double b = 2000d;

            if (true)
            {
                b = .00001d; //b = 1 means following direction directly
                if (Globals.gradientSolve) b = 1e-20d;
                while (true)
                {
                    goldenHelper1(x, direction, xOriginal, b);
                    double fEnd = func(x, input);
                    if (fEnd > fStart)
                    {
                        break;
                    }
                    else
                    {

                    }
                    b = 3 * b;
                }
            }

            double h = b - a;

            int n = (int)(Math.Ceiling(Math.Log(input.deltaGolden / h) / Math.Log(invphi)));

            double c = a + invphi2 * h;
            double d = a + invphi * h;

            goldenHelper1(x, direction, xOriginal, c);
            double yc = func(x, input);
            goldenHelper1(x, direction, xOriginal, d);
            double yd = func(x, input);

            for (int k = 0; k < n; k++)
            {

                if (yc < yd)
                {
                    b = d;
                    d = c;
                    yd = yc;
                    h = invphi * h;
                    c = a + invphi2 * h;
                    goldenHelper1(x, direction, xOriginal, c);
                    yc = func(x, input);
                }
                else
                {
                    a = c;
                    c = d;
                    yc = yd;
                    h = invphi * h;
                    d = a + invphi * h;
                    goldenHelper1(x, direction, xOriginal, d);
                    yd = func(x, input);
                }
            }

            if (yc < yd)
            {
                //a, d
                return (a + d) / 2d;
            }
            else
            {
                //c, b
                return (c + b) / 2d;
            }


        }

        private static void goldenHelper1(double[] x, double[] d, double[] xOriginal, double alpha)
        {
            for (int j = 0; j < x.Length; j++)
            {
                x[j] = xOriginal[j] + alpha * d[j];
            }
        }

        private static void SolveGradientAlgorithHelper(double[] gradient, Func<double[], CGSolverInput, double> func, double[] x0, CGSolverInput input)
        {
            int n = x0.Length;
            double y0 = func(x0, input);
            double sum = 0d;
            for (int i = 0; i < n; i++)
            {
                x0[i] += input.deltaGradient;
                double y1 = func(x0, input);
                gradient[i] = (y1 - y0) / input.deltaGradient;
                //sum += gradient[i] * gradient[i];
                x0[i] -= input.deltaGradient;
            }
            //for (int i = 0; i < n; i++)
            //{
            //    gradient[i] = gradient[i] / Math.Sqrt(sum);
            //}
        }

        public static void SolveGradientAlgorithmUsingAlglib(double[] b, Type assembly, NewtonAlgorithmHelper nah)
        {

            double[] bTemp = new double[Program.model.modelGekko.b.Length];
            Array.Copy(Program.model.modelGekko.b, bTemp, Program.model.modelGekko.b.Length);

            int n = Program.model.modelGekko.m2.fromEqNumberToBNumber.Length;

            IElementalAccessVector residuals = new DenseVector(n);
            IElementalAccessVector x0 = new DenseVector(n);

            double[] xstart = new double[n];

            for (int i = 0; i < n; i++)
            {
                x0.SetValue(i, b[Program.model.modelGekko.m2.fromEqNumberToBNumber[i]]);
                //Globals.gradientX0[i] = b[model.m2.fromEqNumberToBNumber[i]];
                xstart[i] = b[Program.model.modelGekko.m2.fromEqNumberToBNumber[i]];
            }

            RSS(residuals, x0, assembly);  //residuals are by-product (b[] also altered)      

            CGSolverInput input = new CGSolverInput();
            input.deltaGradient = 1e-8;
            input.deltaGolden = 1e-8;
            input.krit = Program.options.solve_newton_conv_abs * Program.options.solve_newton_conv_abs;  //0.0001^2 <=> no residual can be > 0.0001, for in that case RSS would be > krit = 0.0001^2        
            input.restartInterval = -12345;
            //input.limitBeta = true;  --> more its
            CGSolverOutput output = SolveGradientAlgorithm(xstart, Function, input);

            double Function(double[] arg, CGSolverInput input2)
            {
                for (int i = 0; i < n; i++)
                {
                    x0.SetValue(i, arg[i]);
                }
                SolveCommon.RSS(residuals, x0, assembly);  //residuals are by-product (b[] also altered)  
                double func = SolveCommon.RssNonScaled(residuals);
                //G.Writeln2(arg[0] + " " + arg[1] + " RSS ---> " + func);
                return func;
            }

            //converged
            Program.model.modelGekko.simulateResults[0] = output.iterations;
            Program.model.modelGekko.simulateResults[1] = Math.Sqrt(SolveCommon.RssNonScaled(residuals));
            return;

        }

    }

    public static class SolveForwardLooking
    {
        private static void SetTerminalType(ETerminalCondition terminal)
        {
            Program.model.modelGekko.simulateResults[8] = 0;

            if (G.Equal(Program.options.solve_forward_method, "none"))
            {
                Program.model.modelGekko.simulateResults[8] = 0;
            }
            else
            {
                if (terminal == ETerminalCondition.Exogenous) Program.model.modelGekko.simulateResults[8] = 0;
                else if (terminal == ETerminalCondition.ConstantLevel) Program.model.modelGekko.simulateResults[8] = 1;
                else if (terminal == ETerminalCondition.ConstantGrowthRate) Program.model.modelGekko.simulateResults[8] = 2;  //not working
            }
        }


        public static double[,] GetFtVars(int largestLag, int obsSimPeriod, double[,] a, List<int> leadedVarsList, double[,] ftVars)
        {
            ftVars = new double[Program.model.modelGekko.leadedVariables.Count, obsSimPeriod];
            for (int lv = 0; lv < leadedVarsList.Count; lv++)
            {
                int t3 = -1;
                for (int t2 = -largestLag; t2 < -largestLag + obsSimPeriod; t2++)
                {
                    t3++;
                    ftVars[lv, t3] = a[leadedVarsList[lv], t2];
                }
            }
            return ftVars;
        }

        public static void HandleFairTaylorIteration(int[] bNumberPointers, int largestLag, GekkoTime tStart0, int obsSimPeriod, double[,] a, List<int> leadedVarsList, int ft, double[,] ftVars)
        {

            for (int lv = 0; lv < leadedVarsList.Count; lv++)
            {
                Series ts = null;
                Series tsrel = null;
                if (Program.options.solve_forward_dump)
                {
                    ts = new Series(Program.options.freq, "ftabs" + (lv + 1) + "_" + ft);
                    tsrel = new Series(Program.options.freq, "ftrel" + (lv + 1) + "_" + ft);
                }
                int t3 = -1;
                for (int t2 = -largestLag; t2 < -largestLag + obsSimPeriod; t2++)
                {
                    t3++;
                    double vNew = a[leadedVarsList[lv], t2];
                    double vOld = ftVars[lv, t3];
                    double vNewDamp = vNew;
                    //only do damping from second ft-iteration and up
                    //there may be NaN's in the lead-variable (if it has not been simulated in the databank),
                    //or the values may be bad. So what we do here, is equivalent to INIT for Gauss-Seidel: we
                    //do not want to just take the databank values as they are.
                    //HMMMMM this means a solved model always has to simulate when rerun on solved databank, with INIT on.
                    if (ft == 0)
                    {
                        //No damping of first iteration. The old values may be full of NaN's.
                        vNewDamp = vNew;
                    }
                    else
                    {
                        //NOTE NOTE NOTE NOTE: Damping is redefined in Gekko 2.0: dampNew = 1-dampOld
                        vNewDamp = (1d - Program.options.solve_forward_fair_damp) * vNew + Program.options.solve_forward_fair_damp * vOld;
                    }
                    a[leadedVarsList[lv], t2] = vNewDamp;
                    if (Program.options.solve_forward_dump)
                    {
                        ts.SetData(tStart0.Add(t2), vNewDamp);
                        tsrel.SetData(tStart0.Add(t2), vNewDamp / vOld - 1);
                    }
                }
                if (Program.options.solve_forward_dump)
                {
                    //G.writeln();
                    if (Program.databanks.GetFirst().ContainsIVariable(ts.name)) Program.databanks.GetFirst().RemoveIVariable(ts.name);
                    if (Program.databanks.GetFirst().ContainsIVariable(tsrel.name)) Program.databanks.GetFirst().RemoveIVariable(tsrel.name);
                    Program.databanks.GetFirst().AddIVariable(ts.name, ts);
                    Program.databanks.GetFirst().AddIVariable(tsrel.name, tsrel);
                }

            }
            return;
        }
        
        public static void HandleNewtonFairTaylorIteration(DateTime dtFt, int[] bNumberPointers, int largestLag, GekkoTime tStart0, int obsSimPeriod, double[,] a, List<int> leadedVarsList, int ft, double[,] ftVars, NewtonFairTaylorHelper helper)
        {
            //TODO: We could also DUMP these for inspection
            double[,] ftVarsNew = GetFtVars(largestLag, obsSimPeriod, a, leadedVarsList, ftVars);

            if (ft > 0)
            {
                //Do a Newton jump here
                //a[leadedVarsList[lv], t2] = vNew...???

                double[,] inv = new double[helper.jacobi.GetLength(0), helper.jacobi.GetLength(1)];
                Array.Copy(helper.jacobi, inv, helper.jacobi.Length);

                if (Program.options.solve_forward_dump)
                {
                    double[,] dump = new double[helper.jacobi.GetLength(0), helper.jacobi.GetLength(1)];
                    Array.Copy(inv, dump, inv.Length);
                    Matrix m = new Matrix();
                    m.data = dump;
                    //m.data = Transpose(m.data);  //easier for humans to understand this orientation, and also without 1 subtracted on the diagonal

                    Program.databanks.GetFirst().AddIVariableWithOverwrite("#ft_" + ft, m);
                }

                for (int i = 0; i < helper.jacobi.GetLength(0); i++)
                {
                    inv[i, i] += -1;
                }

                int success = -12345;
                alglib.matinvreport report = new alglib.matinvreport();
                alglib.rmatrixinverse(ref inv, out success, out report);
                if (success == 3)
                {
                    G.Writeln2("*** ERROR: Inv(): It seems the matrix is singular");
                    throw new GekkoException();
                }
                else if (success != 1)
                {
                    G.Writeln2("*** ERROR: Inv(): Could not invert matrix");
                    throw new GekkoException();
                }

                //#orig-inv(#mm)*(#base - #orig);

                double[] delta = new double[inv.GetLength(0)];
                for (int i = 0; i < inv.GetLength(0); i++)
                {
                    int rowCounter = -1;
                    //The two nested loops below correspond to a full row of the inv array
                    for (int lv = 0; lv < leadedVarsList.Count; lv++)
                    {
                        int t3 = -1;
                        for (int t2 = -largestLag; t2 < -largestLag + obsSimPeriod; t2++)
                        {
                            t3++;
                            rowCounter++;
                            double vNew = ftVarsNew[lv, t3];
                            double vOld = ftVars[lv, t3];
                            delta[i] += -inv[rowCounter, i] * (vNew - vOld);
                        }
                    }
                }

                double[] newBestGuessRegardingLeadVariables = new double[inv.GetLength(0)];

                int ii = -1;
                //The two nested loops below correspond to a full row of the inv array
                for (int lv = 0; lv < leadedVarsList.Count; lv++)
                {
                    int t3 = -1;

                    Series ts = null;
                    Series tsrel = null;
                    if (Program.options.solve_forward_dump)
                    {
                        ts = new Series(Program.options.freq, "ftabs" + (lv + 1) + "_" + ft);
                        tsrel = new Series(Program.options.freq, "ftrel" + (lv + 1) + "_" + ft);
                    }

                    for (int t2 = -largestLag; t2 < -largestLag + obsSimPeriod; t2++)
                    {
                        t3++;
                        ii++;
                        //double vNew = ftVarsNew[lv, t3];
                        double vOld = ftVars[lv, t3];
                        // NOTE NOTE NOTE NOTE: Damping is redefined in Gekko 2.0: dampNew = 1-dampOld
                        newBestGuessRegardingLeadVariables[ii] = vOld + (1d - Program.options.solve_forward_nfair_damp) * delta[ii];
                        //G.Writeln("--> GUESS var = " + lv + " period " + t3 + " " + newBestGuessRegardingLeadVariables[ii] + " (delta = " + delta[ii] + ")");
                        a[leadedVarsList[lv], t2] = newBestGuessRegardingLeadVariables[ii];

                        if (Program.options.solve_forward_dump)
                        {
                            ts.SetData(tStart0.Add(t2), newBestGuessRegardingLeadVariables[ii]);
                            tsrel.SetData(tStart0.Add(t2), newBestGuessRegardingLeadVariables[ii] / vOld - 1);
                        }
                    }

                    if (Program.options.solve_forward_dump)
                    {
                        //G.writeln();
                        if (Program.databanks.GetFirst().ContainsIVariable(ts.name)) Program.databanks.GetFirst().RemoveIVariable(ts.name);
                        if (Program.databanks.GetFirst().ContainsIVariable(tsrel.name)) Program.databanks.GetFirst().RemoveIVariable(tsrel.name);
                        Program.databanks.GetFirst().AddIVariable(ts.name, ts);
                        Program.databanks.GetFirst().AddIVariable(tsrel.name, tsrel);
                    }
                }
            }
            return;
        }

        public static void CheckFairTaylorIteration(bool usingFairTaylor, bool usingNewtonFairTaylor, int[] bNumberPointers, int largestLag, int obsSimPeriod, double[,] a, List<int> leadedVarsList, int ft, double[,] ftVars, out bool ok)
        {
            ok = true;
            double[,] ftVarsNew = null;
            if (ft > 0)
            {
                ftVarsNew = GetFtVars(largestLag, obsSimPeriod, a, leadedVarsList, ftVars);
                for (int lv = 0; lv < leadedVarsList.Count; lv++)
                {
                    int t3 = -1;
                    for (int t2 = -largestLag; t2 < -largestLag + obsSimPeriod; t2++)
                    {
                        t3++;
                        //double vNew = a[leadedVarsList[lv], t2];
                        double vNew = ftVarsNew[lv, t3];
                        double vOld = ftVars[lv, t3];
                        int fairTaylorConvType = 1;
                        if (usingFairTaylor && G.Equal(Program.options.solve_forward_fair_conv, "conv2")) fairTaylorConvType = 2;
                        if (usingNewtonFairTaylor && G.Equal(Program.options.solve_forward_nfair_conv, "conv2")) fairTaylorConvType = 2;
                        int bNumber = bNumberPointers[leadedVarsList[lv]];
                        double abs; double rel1; double rel2;
                        bool converged = SolveGauss777.GaussConvergenceOk(usingFairTaylor, usingNewtonFairTaylor, null, vNew, vOld, bNumber, false, fairTaylorConvType, out abs, out rel1, out rel2);  //first arg is null --> so that there is no Gauss damping correction (Fair-Taylor uses its own damping)
                        if (!converged) ok = false;
                    }
                }
            }
            else
            {
                ok = false;  //so we keep iterating
            }
            return;
        }


        public static NewtonFairTaylorHelper CreateNFairHelper(ref GekkoTime tStart, ref GekkoTime tEnd, bool usingNewtonFairTaylor, List<int> leadedVarsList, int ft, double[,] oldNftJacobi)
        {
            NewtonFairTaylorHelper helper = new NewtonFairTaylorHelper();
            helper.shocks = new List<NewtonFairTaylorHelper1>();
            NewtonFairTaylorHelper1 h2 = new NewtonFairTaylorHelper1();
            h2.gt = GekkoTime.tNull;
            h2.varNumber = -12345;
            h2.isFirstBaseline = true;
            helper.shocks.Add(h2);
            if (Program.options.solve_forward_nfair_updatefreq > 1 && oldNftJacobi != null)
            {
                helper.jacobi = new double[oldNftJacobi.GetLength(0), oldNftJacobi.GetLength(1)];
                Array.Copy(oldNftJacobi, helper.jacobi, oldNftJacobi.Length);
            }
            else
            {
                if (usingNewtonFairTaylor && ft > 0)
                {
                    //after the first FT-iteration (ft = 0), we do not do shocks.
                    //this has to do with data initialization, because in the first FT-iteration we typically take initial leaded variable values
                    //from lagged variable values (remember the leaded variable may be all missing values).
                    //This interacts badly with shocks, so we do it from FT-iteration 2 and onwards

                    int counter = -1;
                    foreach (int leadVar in leadedVarsList)
                    {
                        counter++;
                        foreach (GekkoTime t in new GekkoTimeIterator(tStart, tEnd))  //horizon2 is deducted. Is 0 for non-stacked models.
                        {
                            NewtonFairTaylorHelper1 h = new NewtonFairTaylorHelper1();
                            h.varNumber = leadVar;
                            h.varCounter = counter;  //starts with 0
                            h.gt = t;
                            helper.shocks.Add(h);
                        }
                    }
                }
            }
            return helper;
        }
    }


}
