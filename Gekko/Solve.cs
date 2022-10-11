/*
    Gekko Timeseries Software (www.t-t.dk/gekko)..
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.
*/

using System;
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
using System.Collections;

namespace Gekko
{    

    public static class SolveCommon
    {
        
        public static void InitEndoNoLag(Program.ErrorContainer ec, double[,] a, int tInt, ref GekkoTime t, SimOptions so, ref double val, Series ts, int yy)
        {
            bool endoInitUsesLag = false;
            //we know that lagPointers are always 0 here!
            if (Program.options.solve_data_init)
            {
                endoInitUsesLag = true;
                //initializing real endogenous variables
                double alag = double.NaN;
                if (tInt - 1 >= 0) alag = a[yy, tInt - 1];

                if (Program.options.solve_data_init_growth)
                {
                    double rel = double.NaN;
                    if (tInt - 2 >= 0) rel = alag / a[yy, tInt - 2];
                    val = alag;
                    if (!G.isNumericalError(rel))
                    {
                        if (rel > 1 + Program.options.solve_data_init_growth_min && rel < 1 + Program.options.solve_data_init_growth_max) val = alag * rel;
                    }
                }
                else
                {
                    val = alag;
                }
            }
            else
            {
                val = a[yy, tInt];  //no lag, just plain value
            }

            if (double.IsNaN(val) && (G.Equal(so.method, "gauss") || G.Equal(so.method, "newton")))  //if it is Res() or Efter() type, we should NEVER go here (where starting values for non-lagged endogenous are set to some arbitrary value
            {
                if (Program.options.solve_data_ignoremissing == false)
                {
                    if (ec.simInitEndoMissingValueHelper == null) ec.simInitEndoMissingValueHelper = new List<string>();
                    //todo: break? like for exo part? now we get both warning and error regarding aaa in 2005 if it is set to M and we sim in 2006.
                    int lag = 0;
                    if (endoInitUsesLag) lag = 1;
                    ec.simInitEndoMissingValueHelper.Add(ts.name + " has a missing value in " + (t.Add(-lag)) + "          " + "sim period: " + t.ToString());
                }

                //====================================
                val = Globals.missingValueSeedNumber; //lagged value of endogenous is missing --> set to this as starting value
                                                      //====================================
            }
        }


        public static void IterationPrint(ref string culprit, GekkoTime tStart, GekkoTime t, ECompiledModelType modelType, StringBuilder output, bool isGaussConverged, SimOptions so)
        {
            string s = "";

            if (t.EqualsGekkoTime(tStart))
            {
                if (Program.options.solve_print_iter) G.Writeln();
            }
            if (culprit != "") culprit = G.ExtractOnlyVariableIgnoreLag(culprit);
            if (G.Equal(so.method, "res"))
            {
                s += "Period " + (t) + " " + " -- single equation static forecast ";
                //G.Write(s);
            }
            else if (G.Equal(so.method, "reverted"))
            {
                s += "Period " + (t) + " " + " -- reverted and after variables ";
                //G.Write(s);
            }
            else
            {
                if (isGaussConverged && (Program.options.solve_print_details || Program.options.solve_gauss_dump) && (modelType == ECompiledModelType.Gauss || modelType == ECompiledModelType.GaussFailSafe))
                {
                    s += "Period " + (t) + " " + Program.model.modelGekko.simulateResults[0] + " iterations   --   last conv.: " + culprit;
                    //G.Write(s);
                }
                else
                {
                    s += "Period " + (t) + " " + Program.model.modelGekko.simulateResults[0] + " iterations";
                    //G.Write(s);
                }
                if (G.Equal(so.method, "gauss"))
                {
                    if (!isGaussConverged)
                    {
                        s += " *** NOT CONVERGED ";
                        if (culprit != "")
                        {
                            s += "(" + culprit + ")";
                        }
                    }
                }
                if (G.Equal(so.method, "newton"))
                {
                    s += ",   crit = " + string.Format("{0:0.00000E+00}", Program.model.modelGekko.simulateResults[1]);
                }
            }
            if (Program.options.solve_print_details && G.Equal(so.method, "newton"))
            {
                s += "\n";
                s += "------------------------------------------------------------------\n";
            }
            //s += "\n";

            if (Program.options.solve_print_iter)
            {
                if (s.Length != 0) G.Writeln(s);
            }
            output.AppendLine(s);
        }


        public static void UndoAndPackStuff(out Program.LinkContainer lc1, out Program.LinkContainer lc2, GekkoTime tStart, GekkoTime tEnd, GekkoTime tStart0, int obsWithLags, int obsSimPeriod, double[,] a2)
        {
            lc1 = new Program.LinkContainer("");
            Globals.linkContainer.Add(lc1.counter, lc1);
            Globals.undoSim = new UndoSim();
            Globals.undoSim.id = lc1.counter;
            Globals.undoSim.a = a2;
            Globals.undoSim.tStart0 = tStart0.Add(0); //probably not necessary to clone, but for safety...
            Globals.undoSim.tStart = tStart.Add(0);
            Globals.undoSim.tEnd = tEnd.Add(0);
            Globals.undoSim.obsWithLags = obsWithLags;
            Globals.undoSim.obsSimPeriod = obsSimPeriod;
            lc2 = new Program.LinkContainer("");
            Globals.linkContainer.Add(lc2.counter, lc2);
            Globals.packSim = new PackSim();
            Globals.packSim.id = lc2.counter;
            Globals.packSim.a = a2;
            Globals.packSim.tStart0 = tStart0.Add(0); //probably not necessary to clone, but for safety...
            Globals.packSim.tStart = tStart.Add(0);
            Globals.packSim.tEnd = tEnd.Add(0);
            Globals.packSim.obsWithLags = obsWithLags;
            Globals.packSim.obsSimPeriod = obsSimPeriod;
        }



        public static void SimulateResiduals(double[] b, double[] r, Type assembly)
        {
            Object[] args = new Object[3];
            args[0] = b;
            args[1] = r;
            args[2] = Globals.scaleNewtonValues;
            assembly.InvokeMember("simulFeedbackAll", BindingFlags.InvokeMethod, null, null, args);

            if (Program.options.solve_newton_robust)
            {
                double newtonStartingValuesFixExtra = 0d;
                for (int i = 0; i < Globals.newtonRobustHelper1; i++)
                {
                    newtonStartingValuesFixExtra += Globals.newtonRobustHelper2[i];
                    //G.Writeln2(i + " " + Globals.newtonStartingValuesHelper2[i]);
                }
                for (int i = 0; i < r.Length; i++)
                {
                    double number = r[i];
                    //double number0 = number;

                    //The error must accumulate, so if the residual is negative, something more is subtracted.
                    if (number < 0)
                    {
                        r[i] += -newtonStartingValuesFixExtra;
                    }
                    else
                    {
                        r[i] += newtonStartingValuesFixExtra;
                    }

                    //if (number * number < number0 * number0) throw new GekkoException(); //assert
                    //f += number * number;                
                }
            }

            if (Program.options.solve_newton_robust)
            {
                //do not record more
                Globals.newtonRobustHelper1 = -12345;
            }

            return;
        }

        public static void SimulateResidual(double[] b, double[] r, int n, Type assembly)
        {
            Object[] args = new Object[4];
            args[0] = b;
            args[1] = r;
            args[2] = n;
            args[3] = Globals.scaleNewtonValues;
            assembly.InvokeMember("simulFeedbackSingle", BindingFlags.InvokeMethod, null, null, args);
            return;
        }



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
                if (Globals.sw == null) Globals.sw = G.GekkoStreamWriter(Program.WaitForFileStream(@"c:\Thomas\Desktop\gekko\testing\cg.txt", null, Program.GekkoFileReadOrWrite.Write));

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


        public static void SimCheckFirstPeriodForMissingStuff(bool usingFairTaylor, GekkoTime tStart, Program.ErrorContainer ec, Series[] timeSeriesPointers, int[] lagPointers, int[] endoNoLagPointers, int[] endoLeadPointers, string[] varNamePointers, int[] isDJZvarPointers)
        {
            if (true)  //for a period like 2006-2079, this check hardly consumes any time
            {
                GekkoTime t = tStart.Add(0);
                //Fail-fast check of what data is missing in order to simulate
                if (Program.databanks.GetFirst().storage.Count == 0)
                {
                    new Error("There were no variables in the databank. Did you forget to load a databank? Simulation is aborted");

                    //throw new GekkoException();
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
                        string count2 = count.ToString();
                        ec.simNonExistingVariable.Add(" " + G.Blanks(4 - count2.Length) + "#" + count + ": The variable " + s + " does not exist in Work databank");
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
                        ec.simMissingValueExoOrLaggedEndo.Add(" " + G.Blanks(4 - count2.Length) + "#" + count + ": Period " + tStart + ": variable " + s2 + " had a missing value in Work databank");
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
            if (G.GetModelType() == EModelType.GAMSScalar)
            {
                ModelGamsScalar modelGamsScalar = Program.model.modelGamsScalar;

                // test the scalar model
                // test the scalar model
                // test the scalar model

                bool testForZeroResiduals = false;
                int rep1 = 5;
                int rep2 = 100;
                double rss = double.NaN;
                for (int j1 = 0; j1 < rep1; j1++)
                {
                    DateTime dt0 = DateTime.Now;
                    for (int j2 = 0; j2 < rep2; j2++)
                    {
                        //This must run fast, else see PredictScalarModel()
                        Func<int, double[], double[][], double[], int[][], int[][], double>[] functions = modelGamsScalar.functions;
                        double[][] a = modelGamsScalar.a;
                        double[] r = modelGamsScalar.r;
                        int[][] bb = modelGamsScalar.bb;
                        double[] cc = modelGamsScalar.cc;
                        int[][] dd = modelGamsScalar.dd;
                        int[] ee = modelGamsScalar.ee;

                        for (int i = 0; i < modelGamsScalar.eqCounts; i++)
                        {
                            functions[ee[i]](i, r, a, cc, bb, dd);  //can return a sum (illegals signal)
                                                                    //double x = r[i];                              

                        }
                    }
                    new Writeln(modelGamsScalar.eqCounts + " evaluations x " + rep2 + " took " + G.Seconds(dt0));

                    if (j1 == 0)
                    {
                        rss = 0d;
                        foreach (double d in modelGamsScalar.r)
                        {
                            rss += d * d;
                        }
                        rss = Math.Sqrt(rss);
                        if (testForZeroResiduals && (G.isNumericalError(rss) || Math.Abs(rss) > 2e-10)) new Error("Bad evaluation");
                        rss = rss;
                    }
                }

                new Writeln("RSS = " + rss);
                return;
            }

            if (G.GetModelType() != EModelType.Gekko)
            {                
                new Error("No Gekko model seems to be defined (cf. {a{MODEL¤model.htm}a} statement)");
            }

            if (G.GetModelType() == EModelType.Gekko && Program.model.modelGekko.subPeriods != -12345 && Program.model.modelGekko.subPeriods != O.CurrentSubperiods())
            {
                using (Error e = new Error())
                {
                    e.MainAdd("The Gekko model was not compiled/loaded with the current frequency.");
                    e.MainAdd("This applies to the pchy(), dify(), diffy(), dlogy() functions. Please put");
                    e.MainAdd("the MODEL statement after your 'OPTION freq ... ' statement.");
                }
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

            if (!G.IsUnitTesting()) Gekko.Gui.gui.textBoxMainTabUpper.SuspendLayout();
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
                Program.RunGekkoCommands(before, "", 0, o.p);
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
                Program.RunGekkoCommands(after, "", 0, o.p);
                Program.databanks.GetLocal().RemoveIVariable(Globals.symbolScalar + "__simt1");
                Program.databanks.GetLocal().RemoveIVariable(Globals.symbolScalar + "__simt2");
            }

            if (!G.IsUnitTesting()) Gekko.Gui.gui.textBoxMainTabUpper.ResumeLayout();
        }

        public static void SimFast(GekkoTime tStart, GekkoTime tEnd, SimOptions so)
        {
            if (GekkoTime.Observations(tStart, tEnd) < 1)
            {
                new Error("start period must be before end period");
                //throw new GekkoException();
            }

            Globals.simCounter = 0;
            //ErrorIfDatabanksSwapped();
            if (G.GetModelType() != EModelType.Gekko)
            {
                new Error("It seems no Gekko model is defined -- simulation cannot be performed");
                //throw new GekkoException();
            }

            if (!G.Equal(so.method, "res"))
            {
                //don't do any terminal logic regarding SIM<res>
                //question is: what about SIM<after>??
                SolveForwardLooking.HandleTerminalHelper();
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
                    new Error("Terminal 'GROWTH' is not working at the moment, please use 'CONST'");
                    //throw new GekkoException();
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

            if (!(G.Equal(so.method, "gauss") || G.Equal(so.method, "newton") || G.Equal(so.method, "res") || G.Equal(so.method, "reverted") || G.Equal(so.method, "eigen"))) new Warning("Seems to be a problem with model type");
            //isRes is true if called by Res(), isReverted if called by Efter()

            Program.ErrorContainer ec = new Program.ErrorContainer();

            ECompiledModelType modelType = GetModelTypeFromOptions(so);  //6 types, including Reverted (for EFTER command)

            //only used with ANTLR
            if ((G.GetModelType() != EModelType.Gekko) || Program.model.modelGekko.equations.Count == 0)
            {
                new Error("It seems no Gekko model is defined: did you forget a MODEL statement?");
                //throw new GekkoException();
            }
            Globals.mayPrintConvergenceCheckVariableMissing = true;  //so that there is only 1 warning regarding this
            if (Program.model.modelGekko.endogenized.Count != Program.model.modelGekko.exogenized.Count)
            {
                new Error("different number of endogenized/exogenized variables (endo = " + Program.model.modelGekko.endogenized.Count + ", exo = " + Program.model.modelGekko.exogenized.Count + ")");
                //throw new GekkoException();
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
                            new Error("Sorry: something has gone wrong regarding model settings");
                            //throw new GekkoException();
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
                Series ts = work.GetIVariable(value.variable + Globals.freqIndicator + G.ConvertFreq(Program.options.freq)) as Series;  //may be null
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
            double[] NAN = G.CreateNaN(obsWithLagsIncludingLeadsAtEnd);
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
                new Note("There are " + Program.model.modelGekko.leadedVariables.Count + " variable(s) with leads: Fair-Taylor algorithm is used");
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
                    SolveForwardLooking.HandleTerminals(largestLag, obsSimPeriod, a, leadedVarsList, terminal);
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
                            Program.LinkContainer lc1;
                            Program.LinkContainer lc2;
                            UndoAndPackStuff(out lc1, out lc2, tStart, tEnd, tStart0, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a2);
                            if (Program.FindException(e, "simFailure"))
                            {
                                SimPrintErrorOptionsUndo(lc1);
                                SimPrintErrorOptionsPack(lc2);
                                SolveGauss777.WriteAboutFailsafeOption();
                            }
                            else
                            {
                                SimPrintErrorOptionsUndo(lc1);
                                SolveGauss777.WriteAboutFailsafeOption();
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
                                SolveGauss777.SolveRes(Program.model.modelGekko.b);
                            }
                            else if (modelType == ECompiledModelType.Gauss || modelType == ECompiledModelType.GaussFailSafe)
                            {
                                if (so.isFix && hasEndoExo)
                                {
                                    //This should never happen
                                    new Error("Trying to solve SIM<fix> with Gauss Seidel");
                                    //throw new GekkoException();
                                }
                                SolveGauss777.SolveGauss(usingFairTaylor || usingNewtonFairTaylor, Program.model.modelGekko.b, isDampedPointers, isDampedPointersArray, out culprit, modelType, t, checkoff);
                            }
                            else if (modelType == ECompiledModelType.Newton)
                            {
                                SolveNewton777.NewtonAlgorithmHelper nah = new SolveNewton777.NewtonAlgorithmHelper();
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
                            Program.LinkContainer lc1;
                            Program.LinkContainer lc2;
                            UndoAndPackStuff(out lc1, out lc2, tStart, tEnd, tStart0, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a2);
                            SimPrintErrorOptionsUndo(lc1);
                            SimPrintErrorOptionsPack(lc2);
                            SolveGauss777.WriteAboutFailsafeOption();
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
                            Program.LinkContainer lc1;
                            Program.LinkContainer lc2;
                            UndoAndPackStuff(out lc1, out lc2, tStart, tEnd, tStart0, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a2);
                            SimPrintErrorOptionsUndo(lc1);
                            SimPrintErrorOptionsPack(lc2);
                            SolveGauss777.WriteAboutFailsafeOption();
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
                                new Warning(extra + "Fair-Taylor algorithm did not converge in " + (ft + 1) + " " + extra2 + "FT-iterations (" + G.SecondsFormat((DateTime.Now - dtFt).TotalMilliseconds) + ")");
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
                        new Note(Program.model.modelGekko.endogenized.Count + " ENDO/EXO vars (goals) were enforced with SIM<fix>");
                    }
                    else
                    {
                        new Note("SIM<fix> did not enforce any goals, since there are no ENDO/EXO vars (goals) set");
                    }
                }
                else
                {
                    //normal SIM
                    if (hasEndoExo)
                    {
                        new Note("There are " + Program.model.modelGekko.endogenized.Count + " ENDO/EXO vars (goals) set, you may use SIM<fix> to enforce them");
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
                    SolveForwardLooking.HandleNewtonFairTaylorIteration(dtFt, bNumberPointers, largestLag, tStart0, obsSimPeriod, a, leadedVarsList, ft, ftVars, helper);
                }

            } //Fair-Taylor (ft) iterations

        JumpOut:;

            DateTime dt3 = DateTime.Now;

            if (debug) G.WritelnGray("from a[] to b[]: " + ms1 / 1000d);
            if (debug) G.WritelnGray("from b[] to a[]: " + ms2 / 1000d);

            if (Globals.alwaysEnablcPackForSimulation)  //this is mostly for debugging, "packsim" activates the link showing up always.
            {
                double[,] a2 = SolveDataInOut.FromAToDatabankWhileRememberingOldDatabank(tStart0, tStart, tEnd, debug, work, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a, NAN, bNumberPointers, endoNoLagPointers);
                Program.LinkContainer lc1;
                Program.LinkContainer lc2;
                UndoAndPackStuff(out lc1, out lc2, tStart, tEnd, tStart0, obsWithLagsIncludingLeadsAtEnd, obsSimPeriodIncludingLeadsAtEnd, a2);
                SimPrintErrorOptionsPack(lc2);
                SolveGauss777.WriteAboutFailsafeOption();
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
                    string count2 = count.ToString();
                    ec.simInitEndoMissingValue.Add(" " + G.Blanks(4 - count2.Length) + "#" + count + ": " + s);
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
                            new Error("While backwriting from SIM command -- please report this error to the Gekko editor. Variable: " + var);
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
                Series ts = work.GetIVariable(var + Globals.freqIndicator + G.ConvertFreq(Program.options.freq)) as Series;  //Could have an A-array with Series...
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
                        new Error("Internal Gekko error #874439849");
                        //throw new GekkoException();
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

        public static void FromAToB(bool usingFairTaylor, bool usingNewtonFairTaylor, NewtonFairTaylorHelper1 shock, int ft, Program.ErrorContainer ec, Databank work, Series[] timeSeriesPointers, int[] extraWritebackPointers, int[] lagPointers, int[] aNumberPointers, int[] endoNoLagPointers, int[] endoLeadPointers, int[] endoPointers, string[] varNamePointers, int[] isDJZvarPointers, double[,] a, int tInt, GekkoTime t, GekkoTime tStart, GekkoTime tEnd, SimOptions so)
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
                        Series tsNew = new Series(Program.options.freq, variable + Globals.freqIndicator + G.ConvertFreq(Program.options.freq));
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
                        new Error("time series '" + variable + "' does not exist in the Work bank");
                        //FIXME: undo, or at least write if DJZ variables have been created.
                        //throw new GekkoException();
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
                                SolveCommon.InitEndoNoLag(ec, a, tInt, ref t, so, ref val, ts, yy);
                            }
                            break;
                        case 2:
                            {
                                val = SolveForwardLooking.InitEndoLeaded(a, tInt, val, yy);
                            }
                            break;
                        case 3:
                            {
                                SolveCommon.InitEndoLaggedOrExo(extraWritebackPointers, lagPointers, endoPointers, isDJZvarPointers, a, tInt, ref t, ref tStart, ref tEnd, i, ref val, ts, variable, yy, so.isStatic);
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
                new Error("You are trying to simulate with a first-position databank ('" + work.name + "') that is non-editable");
                //throw new GekkoException();
            }
            DateTime dt4 = DateTime.Now;

            string s = O.ShowDatesAsString(tStart, tEnd);
            string src = s + "SIM " + Path.GetFileName(Program.model.modelGekko.modelInfo.fileName) + " (hash " + Program.model.modelGekko.modelHashTrue + ")";            //string stamp = Program.GetDateStampCache();
            foreach (ATypeData atd in Program.model.modelGekko.varsAType.Values)
            {
                string var = atd.varName;
                int id = atd.aNumber;
                Series ts = work.GetIVariable(var + Globals.freqIndicator + G.ConvertFreq(Program.options.freq), true) as Series;  //Could have an A-array with Series... . This is conceptually a RHS variable assignment (Trace())

                if (ts == null && SolveCommon.IsDjz(var))
                {
                    //can be autocreated, this probably will never happen, since it is already created,
                    //see #7235432894539
                    ts = new Series(Program.options.freq, var + Globals.freqIndicator + G.ConvertFreq(Program.options.freq));
                    work.AddIVariable(var + Globals.freqIndicator + G.ConvertFreq(Program.options.freq), ts);
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
        public static void DampVariables(double[] b, double[] bOld, List<int> isDampedPointers)
        {
            foreach (int bNumber in isDampedPointers)
            {
                // NOTE NOTE NOTE NOTE: Damping is redefined in Gekko 2.0: dampNew = 1-dampOld
                double alfa = 1d - Program.options.solve_gauss_damp;  //it is 0.5 per default <=> halfways between new and old value.
                //in PCIM, the default value for alfa is 1.0 <=> no damping at all
                //the smaller alfa is, the harder the damping
                //if alfa were set to 0, there would be no progress at all
                double bNew = alfa * b[bNumber] + (1 - alfa) * bOld[bNumber];
                if (G.isNumericalError(bNew))
                {
                    //if this is so, should we keep the old value? or the new value?
                    //Console.WriteLine();
                }
                b[bNumber] = bNew;
            }
        }


        public static void WriteAboutFailsafeOption()
        {
            if (Program.options.solve_failsafe == true) return;
            new Note("Use 'OPTION solve failsafe = yes;' to help tracking the root of the problem");
        }

        /// <summary>
        /// Solve by means of Gauss-Seidel method
        /// </summary>
        /// <param name="b">Array with variables</param>
        /// <param name="simulateResults">Results</param>
        /// <param name="culprit">Last variable to converge</param>
        public static void SolveRes(double[] b)
        {
            Type assembly = SolveCommon.GetAssemblyFromModelType(ECompiledModelType.Res);
            Object[] args = new Object[1];
            args[0] = b;
            assembly.InvokeMember("eqs", BindingFlags.InvokeMethod, null, null, args);
        }


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
                new Error("hist variance");
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
                    new Error("'OPTION solve gauss dump' cannot be set while doing Fair-Taylor (would exhaust memory)");
                    //throw new GekkoException();
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
                            new Error("Out of memory -- please run for fewer years with option 'dump'. The dump option is quite memory-intensive, since it remembers all intermediate simulation values.");
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
                                new Error("Out of memory -- please run for fewer years with option 'dump'. The dump option is quite memory-intensive, since it remembers all intermediate simulation values.");


                                //throw new GekkoException();
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
                new Error("#98349834"); return false;
                //throw new GekkoException();
            }
        }
    }

    public static class SolveNewton777
    {
        
        public static void JacobiNull()
        {
            Program.model.modelGekko.jacobiMatrix = null;
            Program.model.modelGekko.jacobiMatrixDense = null;
            //Program.model.modelGekko.jacobiMatrixInverted = null;  //we actually prefer to reuse this -- costly to new[] it for each fast step (it is often > 10.000 doubles)
            Program.model.modelGekko.jacobiMatrixInvertedIndex = null;
        }

        public static void AbortNewtonAlgorithm(NewtonAlgorithmHelper nah, bool printError)
        {
            if (printError)
            {
                new Error("Problem simulating " + nah.tStart + "-" + nah.tEnd + ": in " + nah.t + " the maximum number of newton simulations (" + Program.options.solve_newton_itermax + ") was exceeded. You may augment this number, see the Newton options: 'OPTION solve newton ...'", false);
            }
            throw new GekkoException();
        }

        //TODO: Not strict regarding use of b[] -- actually puts result into Program.model.modelGekko.b[] via RSS(). These are typically the same, but what if not
        public static void SolveNewtonAlgorithm(double[] b, Type assembly, NewtonAlgorithmHelper nah)
        {
            double[] bTemp = new double[Program.model.modelGekko.b.Length];
            Array.Copy(Program.model.modelGekko.b, bTemp, Program.model.modelGekko.b.Length);

            int iiMax = 2;

            bool printErrors = true;
            bool useIntelligent = false;

            for (int ii = 0; ii < iiMax; ii++)
            {
                if (ii == 0)
                {
                    useIntelligent = true;
                }

                if (ii == 1)
                {
                    if (Program.options.solve_print_details)
                    {
                        G.Writeln("--------------------------------------");
                        G.Writeln("First attempt failed, trying 1 last...");
                        G.Writeln("--------------------------------------");
                    }
                    useIntelligent = false;
                    Array.Copy(bTemp, Program.model.modelGekko.b, Program.model.modelGekko.b.Length);  //resetting
                }

                JacobiNull();  //mostly for safety, to make sure we do not have old stuff from other periods lying around

                double krit = Program.options.solve_newton_conv_abs * Program.options.solve_newton_conv_abs;  //0.0001^2 <=> no residual can be > 0.0001, for in that case RSS would be > krit = 0.0001^2
                //Ehm, RSS is divided by 2 now!
                int n = Program.model.modelGekko.m2.fromEqNumberToBNumber.Length;

                IElementalAccessVector residuals = new DenseVector(n);
                IElementalAccessVector x0 = new DenseVector(n);

                for (int i = 0; i < Program.model.modelGekko.m2.fromEqNumberToBNumber.Length; i++)
                {
                    x0.SetValue(i, b[Program.model.modelGekko.m2.fromEqNumberToBNumber[i]]);
                }
                SolveCommon.RSS(residuals, x0, assembly);  //residuals are by-product (b[] also altered)

                IElementalAccessVector residualsBase = new DenseVector(residuals.Length);
                Blas.Default.Copy(residuals, residualsBase);

                //RSS(residuals, x0, assembly);  //residuals are by-product (b[] also altered)

                if (true)
                {
                    //hack here
                    for (int i = 0; i < residuals.Length; i++)
                    {
                        double number = residuals.GetValue(i);
                        if (double.IsNaN(number))
                        {
                            //we will accept infinity, for instance 1/0
                            new Error("Simulating " + nah.tStart + "-" + nah.tEnd + ": in " + nah.t + " the Newton algorithm had starting value problems. Note: You may try 'OPTION solve failsafe = yes;' to handle this problem.");                            
                        }
                    }
                }

                double rss0 = SolveCommon.RssNonScaled(residuals);

                if (Program.options.solve_print_details) G.Writeln("SQRT(RSS) start = " + Math.Sqrt(rss0) + " #residuals = " + residuals.Length);

                double[] residualsArray = new double[residuals.Length];
                //double[,] jacobiArray = PutJacobiIntoArray();

                int backTrackCounter = 0;
                int backTrackCounter2 = 0;

                int iterations = 0;
                if (rss0 > krit)  //else it is already converged
                {
                    for (int it = 0; it < int.MaxValue; it++)
                    {
                        iterations++;
                        if (iterations >= Program.options.solve_newton_itermax) AbortNewtonAlgorithm(nah, printErrors);

                        if (Program.options.solve_print_details) G.Writeln("SQRT(RSS) before iteration #" + it + " = " + Math.Sqrt(rss0), Color.Blue);

                        if (Globals.runningOnTTComputer)
                        {
                            double sum = 0d;
                            List<double> contribution = new List<double>();
                            int largest = -12345;
                            double largestValue = double.NegativeInfinity;
                            for (int i = 0; i < residuals.Length; i++)
                            {
                                double temp = residuals.GetValue(i) * residuals.GetValue(i);
                                sum += temp;
                                if (temp > largestValue)
                                {
                                    largestValue = temp;
                                    largest = i;
                                }
                                contribution.Add(temp);
                            }
                            if (largest >= 0)
                            {
                                double contributionLargest = contribution[largest] / sum;
                                int bnum = Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW[largest];
                                string var = Program.model.modelGekko.varsBTypeInverted[bnum];
                                string var2 = "";
                                int lag = 0;
                                G.ExtractVariableAndLag(var, out var2, out lag);
                                if (Program.options.solve_print_details) G.Writeln("RSS: largest contribution is equation '" + var2 + "' (share of RSS = " + contributionLargest + ")");
                            }
                        }

                        DateTime t0 = DateTime.Now;
                        Jacobi(x0, assembly);

                        if (Program.options.solve_print_details)
                        {
                            if (Globals.runningOnTTComputer) G.Writeln("New " + residuals.Length + "x" + residuals.Length + " Jacobi matrix constructed: " + (DateTime.Now - t0).TotalMilliseconds / 1000d + " seconds", Color.Orange);
                            else G.Writeln("New Jacobi " + residuals.Length + "x" + residuals.Length + " matrix constructed: " + (DateTime.Now - t0).TotalMilliseconds / 1000d + " seconds");
                        }

                        IElementalAccessVector dx = new DenseVector(n);

                        t0 = DateTime.Now;
                        bool ok = Program.InvertMatrix(residuals, dx);  //jacobyMatrix is also used
                        if (Program.options.solve_print_details)
                        {
                            if (Globals.runningOnTTComputer) G.Writeln("Jacobi " + residuals.Length + "x" + residuals.Length + " matrix inverted: " + (DateTime.Now - t0).TotalMilliseconds / 1000d + " seconds", Color.Orange);
                            else G.Writeln("Jacobi " + residuals.Length + "x" + residuals.Length + " matrix inverted: " + (DateTime.Now - t0).TotalMilliseconds / 1000d + " seconds");
                        }

                        if (ok == false)
                        {
                            if (ii == 0)
                            {
                                goto metaIt;
                            }
                            else
                            {
                                new Error("Problem simulating " + nah.tStart + "-" + nah.tEnd + ": in " + nah.t + " the Newton algorithm stalled");
                            }
                        }

                        IElementalAccessVector x = new DenseVector(n);
                        Blas.Default.Add(x0, dx, x); //x = x0 + dx

                        SolveCommon.RSS(residuals, x, assembly);  //residuals are by-product (b[] also altered)
                        double rss = SolveCommon.RssNonScaled(residuals);

                        if (Program.options.solve_print_details) G.Writeln("iter = " + it + " SQRT(RSS) = " + Math.Sqrt(rss));

                        if (rss < krit)
                        {
                            break;
                        }

                        //TODO TODO
                        //TODO TODO
                        //TODO TODO #98075324
                        //TODO TODO

                        if (rss < rss0)
                        {

                            if (Globals.solveUseFastSteps == false)
                            {
                                Blas.Default.Copy(x, x0);
                                rss0 = rss;
                                goto newIt;
                            }

                            //successful newton step: try cheaper steps now (_a)
                            double rss0_a = rss;
                            IElementalAccessVector x0_a = new DenseVector(x);
                            for (int i = 0; i < int.MaxValue; i++)
                            {
                                iterations++;
                                if (iterations >= Program.options.solve_newton_itermax) AbortNewtonAlgorithm(nah, printErrors);

                                IElementalAccessVector dx_a = new DenseVector(n);

                                bool isOk = Program.InvertMatrix(residuals, dx_a); //jacobyMatrix is also used

                                IElementalAccessVector x_a = new DenseVector(n);
                                Blas.Default.Add(x0_a, dx_a, x_a); //x_a = x0_a + dx_a

                                SolveCommon.RSS(residuals, x_a, assembly);  //residuals are by-product (b[] also altered)
                                double rss_a = SolveCommon.RssNonScaled(residuals);

                                if (Program.options.solve_print_details) G.Writeln("    fast iter (no jacobi update) = " + i + " SQRT(RSS) = " + Math.Sqrt(rss_a));
                                if (rss_a > rss0_a)
                                {
                                    Blas.Default.Copy(x0_a, x0);
                                    rss0 = rss0_a;
                                    break;  //no more of these fast iterations, do a proper one
                                }
                                if (i >= Program.options.solve_newton_updatefreq - 1)
                                {
                                    Blas.Default.Copy(x_a, x0);
                                    rss0 = rss_a;
                                    break;  //no more of these fast iterations, do a proper one
                                }
                                if (rss_a < krit)
                                {
                                    Blas.Default.Copy(x_a, x);
                                    rss0 = rss;
                                    goto converged;
                                }
                                Blas.Default.Copy(x_a, x0_a);
                                rss0_a = rss_a;
                            }

                            goto newIt;
                        }
                        //rss > rss0: rss did not diminish as it should: backtrack

                        if (rss > rss0)
                        {
                            if (useIntelligent && Program.options.solve_newton_backtrack == true && backTrackCounter2 >= 20)
                            {
                                //jump into unknown

                                iterations++;
                                if (iterations >= Program.options.solve_newton_itermax) AbortNewtonAlgorithm(nah, printErrors);

                                Jacobi(x, assembly);
                                IElementalAccessVector dx2 = new DenseVector(n);
                                bool ok2 = Program.InvertMatrix(residuals, dx2);  //jacobyMatrix is also used
                                if (ok == false)
                                {
                                    if (ii == 0)
                                    {
                                        goto metaIt;
                                    }
                                    else
                                    {
                                        new Error("Problem simulating " + nah.tStart + "-" + nah.tEnd + ": in " + nah.t + " the Newton algorithm stalled");
                                    }
                                }
                                IElementalAccessVector x2 = new DenseVector(n);
                                Blas.Default.Add(x, dx2, x2); //x2 = x + dx2
                                IElementalAccessVector residuals2 = new DenseVector(n);
                                SolveCommon.RSS(residuals2, x2, assembly);  //residuals are by-product (b[] also altered)
                                double rss2 = SolveCommon.RssNonScaled(residuals2);

                                if (rss2 < rss0)
                                {
                                    if (Program.options.solve_print_details) G.Writeln("Did a successful jump to avoid backtracking");
                                    Blas.Default.Copy(x2, x0);
                                    rss0 = rss2;
                                    backTrackCounter2 = 0;
                                    goto newIt;
                                }
                                else
                                {
                                    //undo, revert jacobi (x and rss and dx are not touched, damping will follow if backtrack is set)
                                    //if (Program.options.solve_print) G.Writeln("jump fail", Color.Orange);
                                    Jacobi(x, assembly);
                                    //--------------------------------------------------------
                                    // It has been checked that this really undoes everything
                                    // relevant, so that search proceeds as if an intelligent
                                    // jump had not been tried out.
                                    // In the long run, the whole algorithm shold
                                    // be done without globals like jacobiMatrix and residuals.
                                    //--------------------------------------------------------
                                }
                            }
                        }

                        if (Program.options.solve_newton_backtrack == false)
                        {
                            Blas.Default.Copy(x, x0);
                            rss0 = rss;
                            goto newIt;
                        }

                        double rss_best = double.MaxValue;
                        IElementalAccessVector x_best = new DenseVector(n);
                        for (int lam = 0; lam < int.MaxValue; lam++)
                        {
                            double lambda = 1d / Math.Pow(2d, lam + 1d);

                            if (lambda < 0.01d)
                            {
                                backTrackCounter2++;
                            }

                            IElementalAccessVector dx_b = new DenseVector(dx);

                            Blas.Default.Scale(lambda, dx_b);

                            IElementalAccessVector x_b = new DenseVector(n);
                            Blas.Default.Add(x0, dx_b, x_b);

                            SolveCommon.RSS(residuals, x_b, assembly);  //residuals are by-product (b[] also altered)
                            double rss_b = SolveCommon.RssNonScaled(residuals);

                            if (rss_b < rss_best)
                            {
                                rss_best = rss_b;
                                Blas.Default.Copy(x_b, x_best);
                            }

                            if (Program.options.solve_print_details) G.Writeln("  lambda=" + lambda + " rss_b=" + Math.Sqrt(rss_b));

                            if (rss_b < rss0 && rss_b > rss_best)
                            {
                                Blas.Default.Copy(x_best, x0);
                                rss0 = rss_best;
                                break;
                            }
                            if (lambda < 1e-8)
                            {
                                backTrackCounter++;
                                if (backTrackCounter >= 6)
                                {
                                    if (ii == 0)
                                    {
                                        goto metaIt;
                                    }
                                    else
                                    {
                                        new Error("Problem simulating " + nah.tStart + "-" + nah.tEnd + ": in " + nah.t + " Newton backtrack could not make further progress. This may indicate that the problem is somehow misspecified, or some of the variables may have very different scale.");                                        
                                    }
                                }

                                Blas.Default.Copy(x_b, x0);  //this gives a small perturbation

                                SolveCommon.RSS(residuals, x0, assembly);  //residuals are by-product (b[] also altered)
                                rss0 = SolveCommon.RssNonScaled(residuals);

                                break;
                            }
                        }
                    newIt:;
                    }
                }
            converged:;
                //converged
                Program.model.modelGekko.simulateResults[0] = iterations;
                Program.model.modelGekko.simulateResults[1] = Math.Sqrt(SolveCommon.RssNonScaled(residuals));
                return;
            metaIt:;
            }  //ii
        }



        public static void ComputeGradientAndPutIntoFF(double delta, int j, int i, Type assembly, bool lu)
        {
            //int eq = simulFeedbackVarBtypeInverted[i];
            int eq = Program.model.modelGekko.m2.fromBNumberToEqNumberFeedbackNEW[i];
            double val0 = Program.model.modelGekko.r[eq];
            Program.model.modelGekko.b[j] += delta;
            SolveCommon.SimulateResidual(Program.model.modelGekko.b, Program.model.modelGekko.r, eq, assembly);
            double val1 = Program.model.modelGekko.r[eq];
            Program.model.modelGekko.b[j] += -delta;
            double grad = (val1 - val0) / delta;
            Program.model.modelGekko.r[eq] = val0;
            if (grad != 0d)
            {
                //int ii = simulFeedbackVarBtypeInverted[j];
                int ii = Program.model.modelGekko.m2.fromBNumberToEqNumberFeedbackNEW[j];
                int jj = eq;
                if (lu)
                {
                    Program.model.modelGekko.jacobiMatrixDense[jj, ii] = grad;
                }
                else
                {
                    Program.model.modelGekko.jacobiMatrix.SetValue(jj, ii, grad);
                }
            }
        }

        public static void Jacobi(IElementalAccessVector x, Type assembly)
        {
            JacobiNull();
            int n = x.Length;

            double delta = Globals.jacobiDeltaProbe;  //must be small enough for an endogenous var
            //cannot be less than this! --> else errors
            //TODO: have a specific delta for each endogenous, based on history

            bool lu = (Program.options.solve_newton_invert == "lu");
            if (lu)
            {
                Program.model.modelGekko.jacobiMatrixDense = new double[n, n];
            }
            else
            {
                Program.model.modelGekko.jacobiMatrix = new SparseRowMatrix(n, n, 5);  //seems faster
            }

            //Keep SimulateSimulPrologue() and SimulateResiduals() together
            SolveCommon.SimulateSimulPrologue(assembly);
            SolveCommon.SimulateResiduals(Program.model.modelGekko.b, Program.model.modelGekko.r, assembly);

            double[] bOriginal = new double[Program.model.modelGekko.b.Length];
            Array.Copy(Program.model.modelGekko.b, bOriginal, Program.model.modelGekko.b.Length);
            double[] rOriginal = new double[Program.model.modelGekko.r.Length];
            Array.Copy(Program.model.modelGekko.r, rOriginal, Program.model.modelGekko.r.Length);

            int counter = -1;
            foreach (int j in Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW)
            {
                counter++;
                int eq_j = Program.model.modelGekko.m2.fromBNumberToEqNumberFeedbackNEW[j];
                Program.model.modelGekko.b[j] += delta;

                //Keep SimulateSimulPrologue() and SimulateResiduals() together
                SolveCommon.SimulateSimulPrologue(assembly);
                SolveCommon.SimulateResiduals(Program.model.modelGekko.b, Program.model.modelGekko.r, assembly);

                foreach (int i in Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW)
                {
                    int eq_i = Program.model.modelGekko.m2.fromBNumberToEqNumberFeedbackNEW[i];
                    double grad = (Program.model.modelGekko.r[eq_i] - rOriginal[eq_i]) / delta;
                    if (grad != 0d)
                    {
                        if (lu)
                        {
                            Program.model.modelGekko.jacobiMatrixDense[eq_i, eq_j] = grad;
                        }
                        else
                        {
                            Program.model.modelGekko.jacobiMatrix.SetValue(eq_i, eq_j, grad);
                        }
                    }
                }

                Array.Copy(bOriginal, Program.model.modelGekko.b, Program.model.modelGekko.b.Length);
                Array.Copy(rOriginal, Program.model.modelGekko.r, Program.model.modelGekko.r.Length);
            }
        }

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
                SolveGradientDescent.SolveGradientAlgorithmUsingAlglib(Program.model.modelGekko.b, Program.model.modelGekko.m2.assemblyNewton, nah);
            }
            else
            {
                SolveNewton777.SolveNewtonAlgorithm(Program.model.modelGekko.b, Program.model.modelGekko.m2.assemblyNewton, nah);
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


        public class NewtonAlgorithmHelper //inner class
        {
            public GekkoTime t;
            public GekkoTime tStart;
            public GekkoTime tEnd;
        }
    }    

    public static class SolveGradientDescent
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

        public static void SolveGradientAlgorithmUsingAlglib(double[] b, Type assembly, SolveNewton777.NewtonAlgorithmHelper nah)
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

            SolveCommon.RSS(residuals, x0, assembly);  //residuals are by-product (b[] also altered)      

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
        public static double InitEndoLeaded(double[,] a, int tInt, double val, int yy)
        {
            val = a[yy, tInt - 1];  //lagged value y(-1) set as init for y(+1) or y(+2) etc., but ONLY in the first FT-iteration
            return val;
        }

        public static void HandleTerminals(int largestLag, int obsSimPeriod, double[,] a, List<int> leadedVarsList, ETerminalCondition terminal)
        {
            for (int lv = 0; lv < leadedVarsList.Count; lv++)
            {
                if (terminal != ETerminalCondition.Exogenous)
                {
                    for (int t2 = -largestLag + obsSimPeriod; t2 < (-largestLag + obsSimPeriod) + Program.model.modelGekko.largestLeadOutsideRevertedPart; t2++)
                    {
                        //NOTE: do not use damping here: terminal values should not be damped!
                        if (terminal == ETerminalCondition.ConstantLevel)
                        {
                            //seems to be ok
                            a[leadedVarsList[lv], t2] = a[leadedVarsList[lv], t2 - 1];
                        }
                        else if (terminal == ETerminalCondition.ConstantGrowthRate)
                        {
                            //seems to be ok
                            a[leadedVarsList[lv], t2] = a[leadedVarsList[lv], t2 - 1] * a[leadedVarsList[lv], t2 - 1] / a[leadedVarsList[lv], t2 - 2];
                        }
                    }
                }
            }
        }

        public static void HandleTerminalHelper()
        {
            Program.model.modelGekko.terminalHelper = null;  //will stay like this if terminal feed=external or there are no leads
            if (G.Equal(Program.options.solve_forward_terminal_feed, "internal"))
            {
                if (Program.model.modelGekko.largestLeadOutsideRevertedPart > 0)
                {
                    Program.model.modelGekko.terminalHelper = new List<Dictionary<int, int>>();
                    for (int i = 0; i < Program.model.modelGekko.largestLeadOutsideRevertedPart; i++)
                    {
                        Program.model.modelGekko.terminalHelper.Add(new Dictionary<int, int>());
                    }
                    foreach (BTypeData data in Program.model.modelGekko.varsBType.Values)
                    {
                        if (data.lag <= 0) continue;
                        for (int i = 0; i < Program.model.modelGekko.largestLeadOutsideRevertedPart; i++)
                        {
                            if (i >= data.lag) continue;
                            //BTypeData data2 = Program.model.modelGekko.varsBType[data.variable + Globals.lagIndicator + i];
                            BTypeData data2 = null;
                            Program.model.modelGekko.varsBType.TryGetValue(data.variable + Globals.lagIndicator + i, out data2);
                            if (data2 != null) Program.model.modelGekko.terminalHelper[i].Add(data.bNumber, data2.bNumber);
                        }
                    }
                }
            }
        }


        public static void SetTerminalType(ETerminalCondition terminal)
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
                    new Error("Inv(): It seems the matrix is singular");
                    //throw new GekkoException();
                }
                else if (success != 1)
                {
                    new Error("Inv(): Could not invert matrix");
                    //throw new GekkoException();
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

    public static class SolveOrdering
    {
        private static void WriteOrderingInfoToFile(List<List<int>> rowsIndexes)
        {
            string path = Program.GetModelInfoPath();

            // Determine whether the directory exists, else create it (used for model related files)
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (FileStream fs = Program.WaitForFileStream(path + "\\" + Globals.modelFileName.Replace(".frm", "") + ".ordering", null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter res = G.GekkoStreamWriter(fs))
            {
                res.WriteLine("Number of endogenous  = " + G.IntFormat(rowsIndexes.Count, 7));
                res.WriteLine("----------------------------------");
                res.WriteLine("Prologue              = " + G.IntFormat(Program.model.modelGekko.m2.prologue.Count, 7));
                res.WriteLine("Simultaneous feedback = " + G.IntFormat(Program.model.modelGekko.m2.simulFeedback.Count, 7));
                res.WriteLine("Simultanous recursive = " + G.IntFormat(Program.model.modelGekko.m2.simulRecursive.Count, 7));
                res.WriteLine("Epilogue              = " + G.IntFormat(Program.model.modelGekko.m2.epilogue.Count, 7));
                res.WriteLine();
                res.Write("The prologue variables can be considered a kind of pre-model of recursive equations feeding into ");
                res.Write("the simultanous block. The epilogue variables can be considered a kind of recursive after-model, depending ");
                res.WriteLine("upon the simultanous block, but not being simultanous itself.");
                res.Write("Inside the simulatenous block, there is a (typically) small kernel of intertwined variables being ");
                res.Write("heavily simultanous: the feedback set. The simultaneous recursive set is a set of simultaneous variables ");
                res.Write("being truly simultanous, but can be understood as being recursive relative to the feedback set. That is, ");
                res.Write("given the feedback (and prologue) variables, the simultanous recursive set can be computed as a (typically) ");
                res.Write("long chain of recursive equations depending only upon each other. These properties are used to reduce the ");
                res.WriteLine("dimensionality of the problem when using the Newton method for goals/means etc.");
                res.WriteLine("");
                res.WriteLine("--- Prologue variables (" + Program.model.modelGekko.m2.prologue.Count + ") ---");
                Program.PrintEquationLeftHandSideNames(Program.model.modelGekko.m2.prologue, res);
                res.WriteLine();
                res.WriteLine("--- Simultaneous block #1 of 2: feedback variables (" + Program.model.modelGekko.m2.simulFeedback.Count + ") ---");
                Program.PrintEquationLeftHandSideNames(Program.model.modelGekko.m2.simulFeedback, res);
                res.WriteLine();
                res.WriteLine("--- Simultaneous block #2 of 2: recursive variables (" + Program.model.modelGekko.m2.simulRecursive.Count + ") ---");
                Program.PrintEquationLeftHandSideNames(Program.model.modelGekko.m2.simulRecursive, res);
                res.WriteLine();
                res.WriteLine("--- Epilogue variables (" + Program.model.modelGekko.m2.epilogue.Count + ") ---");
                Program.PrintEquationLeftHandSideNames(Program.model.modelGekko.m2.epilogue, res);
                res.WriteLine();
                res.Flush();
            }
        }


        private static void PutIntoIndidenceMatrix(List<List<int>> rowsIndexes, List<List<int>> columnsIndexes, List<List<int>> rowsIndexes2, List<List<int>> columnsIndexes2, int lhs, int rhsi)
        {
            rowsIndexes[lhs].Add(rhsi);
            columnsIndexes[rhsi].Add(lhs);
            rowsIndexes2[lhs].Add(rhsi);
            columnsIndexes2[rhsi].Add(lhs);
        }

        private static void Heuristic(List<List<int>> rowsIndexes, List<List<int>> columnsIndexes, List<int> feedback)
        {
            int max2 = -12345;
            int imax = -12345;
            for (int i = 0; i < columnsIndexes.Count; i++)
            {
                List<int> a = columnsIndexes[i];
                if (a != null)
                {
                    List<int> b = rowsIndexes[i];
                    int sza = a.Count;
                    int szb = b.Count;
                    int prod = sza * szb;  //seems to be good rule
                    //int prod = szb; //for jul05 reduces feedb set from 267 to 240, but is slower overall. Also for saffier.
                    if (prod > max2)
                    {
                        max2 = prod;
                        imax = i;
                    }
                }
            }
            if (imax != -12345)
            {
                feedback.Add(imax);
                DeleteRowAndColumn(rowsIndexes, columnsIndexes, imax);
            }
        }

        private static void FindRecursive(List<List<int>> rowsIndexes, List<List<int>> columnsIndexes, List<int> pro)
        {
            while (true)
            {
                bool hit = false;
                for (int i = 0; i < rowsIndexes.Count; i++)
                {

                    List<int> a = rowsIndexes[i];
                    if (a != null && a.Count == 0)
                    {
                        hit = true;
                        //rowsum is 0 for this row
                        pro.Add(i);
                        DeleteRowAndColumn(rowsIndexes, columnsIndexes, i);
                    }
                }
                if (!hit) break;
            }
        }



        private static int FindEqWithVarOnLeftHandSide(string endo)
        {
            int eqEndo = -12345;
            foreach (EquationHelper eh in Program.model.modelGekko.equations)
            {
                if (G.Equal(eh.lhs, endo))
                {
                    eqEndo = eh.equationNumber;
                }
            }
            if (eqEndo == -12345) new Error("Variable " + endo + " is not found as left-hand side var", false);
            return eqEndo;
        }


        private static ArrayList FindEqsWithVarOnRightHandSide(string var1)
        {
            ArrayList eqs = new ArrayList();
            foreach (EquationHelper eh in Program.model.modelGekko.equations)
            {
                foreach (string rhsVar in eh.precedentsWithLagIndicator.Keys)
                {
                    if (G.Equal(rhsVar, var1 + Globals.lagIndicator + "0"))
                    {
                        eqs.Add(eh.equationNumber);
                    }
                }
            }
            return eqs;
        }


        private static void FindDiagonal(List<List<int>> rowsIndexes, List<List<int>> columnsIndexes, List<int> fb)
        {
            for (int i = 0; i < rowsIndexes.Count; i++)
            {
                List<int> a = rowsIndexes[i];
                if (a != null)
                {
                    if (a.Contains(i))
                    {
                        fb.Add(i);
                        DeleteRowAndColumn(rowsIndexes, columnsIndexes, i);
                    }
                }
            }
        }


        private static void FindColsWithSum1(List<List<int>> rowsIndexes, List<List<int>> columnsIndexes, List<int> rec)
        {
            for (int i = 0; i < columnsIndexes.Count; i++)
            {
                List<int> a = columnsIndexes[i];
                if (a != null)
                {
                    if (a.Count == 1)
                    {
                        int subst = (int)a[0];
                        if (i != subst)
                        {

                            rec.Add(i);


                            if (!(rowsIndexes[subst]).Contains(subst))
                            {
                                (rowsIndexes[subst]).Add(subst);
                            }
                            if (!(columnsIndexes[subst]).Contains(subst))
                            {
                                (columnsIndexes[subst]).Add(subst);
                            }

                            DeleteRowAndColumn(rowsIndexes, columnsIndexes, i);
                        }
                    }
                }
            }
        }

        private static void FindRowsWithSum1(List<List<int>> rowsIndexes, List<List<int>> columnsIndexes, List<int> rec)
        {
            for (int i = 0; i < rowsIndexes.Count; i++)
            {
                List<int> a = rowsIndexes[i];
                if (a != null)
                {
                    if (a.Count == 1)
                    {
                        int subst = (int)a[0];  //the right-hand var (y in c = 0.8 y)
                        if (i != subst)  //should not be equal -- meaning it depends only on itself
                        {
                            //this is an equation like "c = 0.8 y"
                            rec.Add(i);  //add "c" to recursive set
                            List<int> b = columnsIndexes[i];
                            for (int j = 0; j < b.Count; j++)
                            {
                                //for each equation containing "c"
                                int c = (int)b[j];  //the left-hand side of that eq (e.g. the equation i = 0.5 c)
                                if (!(rowsIndexes[c]).Contains(subst))
                                {
                                    //if equation "i" does not contain "y", add it
                                    (rowsIndexes[c]).Add(subst);
                                }
                                if (!(columnsIndexes[subst]).Contains(c))
                                {
                                    //if var "y" does not appear in equaton "i" add it
                                    (columnsIndexes[subst]).Add(c);
                                }


                            }
                            DeleteRowAndColumn(rowsIndexes, columnsIndexes, i);
                        }
                    }
                }
            }
        }


        private static void DeleteRowAndColumn(List<List<int>> rowsIndexes, List<List<int>> columnsIndexes, int i)
        {
            List<int> b = columnsIndexes[i];
            for (int j = 0; j < b.Count; j++)
            {
                int c = (int)b[j];
                List<int> d = rowsIndexes[c];
                if (!d.Contains(i)) G.Writeln("error");
                d.Remove(i);
            }

            List<int> b2 = rowsIndexes[i];
            for (int j = 0; j < b2.Count; j++)
            {
                int c = (int)b2[j];
                List<int> d = columnsIndexes[c];
                if (!d.Contains(i)) G.Writeln("error");
                d.Remove(i);
            }


            rowsIndexes[i] = null;
            columnsIndexes[i] = null;
        }


        public static void EndogenizeExogenizeStuff(bool isFix)
        {
            Program.model.modelGekko.m2.endogenous.Clear();
            foreach (string var in Program.model.modelGekko.endogenousOriginallyInModel.Keys)
            {
                Program.model.modelGekko.m2.endogenous.Add(var, "");
            }

            Program.model.modelGekko.m2.endoSubstitution.Clear();
            Program.model.modelGekko.m2.endoSubstitutionBNumbers.Clear();

            if (isFix)  //we skip this for normal SIM, just as if the ENDO/EXO lists were empty even if they are not
            {
                //TODO: Maybe model.endogenized and model.exogenized should be List<string> to begin with,
                //      to preserve order. Might be better for simulation.
                //      BUT then we would have to re-thing caching, where order is scrambled anyway. Probably
                //      preserving order is not noticable anyway, especially when using direct inverter (LU).
                List<string> endogeni = new List<string>();
                List<string> exogeni = new List<string>();
                IEnumerator e1 = null;
                IEnumerator e2 = null;
                e1 = Program.model.modelGekko.endogenized.GetEnumerator();
                e2 = Program.model.modelGekko.exogenized.GetEnumerator();
                while (e1.MoveNext())
                {
                    e2.MoveNext();
                    //string s1 = (string)(((DictionaryEntry)e1.Current).Key);
                    string s1 = ((KeyValuePair<string, string>)e1.Current).Key;
                    string s2 = ((KeyValuePair<string, string>)e2.Current).Key;
                    endogeni.Add(s1);
                    exogeni.Add(s2);
                }
                //This sorting is so that we get the same order no matter which order (and case) the means/goals were set
                //This is probably more safe regarding cacheing of the results. Not sorting might give hard to track errors.
                endogeni.Sort(StringComparer.OrdinalIgnoreCase);
                exogeni.Sort(StringComparer.OrdinalIgnoreCase);

                //endogenous/exogenous are altered due to endogenized/exogenized
                for (int i = 0; i < endogeni.Count; i++)
                {
                    string s1 = endogeni[i];
                    string s2 = exogeni[i];
                    //BTypeData ss1 = (BTypeData)Program.model.modelGekko.varsBType[s1 + Globals.lagIndicator + "0"];
                    BTypeData ss1 = null; Program.model.modelGekko.varsBType.TryGetValue(s1 + Globals.lagIndicator + "0", out ss1);
                    if (ss1 == null)
                    {
                        //TODO: general error handling regarding endo/exo
                        //now we get runtime error
                        new Error("regarding endogenize: variable " + s1 + " does not exist in model");
                        //throw new GekkoException();
                    }
                    int s1BNumber = ss1.bNumber;
                    //int varNumber = ss1.bNumber;
                    if (Program.model.modelGekko.m2.endogenous.ContainsKey(s1))
                    {
                        new Error("regarding endogenize: variable " + s1 + " is already endogenous");
                        //throw new GekkoException();
                    }
                    else
                    {
                        Program.model.modelGekko.m2.endogenous.Add(s1, "");
                        //Program.model.modelGekko.endogenousBNumbers.Add(s1BNumber, "");  DO NOT ACTIVATE THIS ONE -- endogenousBNumbers are dealt with in the ordering code
                        //BTypeData ss2 = (BTypeData)Program.model.modelGekko.varsBType[s2 + Globals.lagIndicator + "0"];
                        BTypeData ss2 = null; Program.model.modelGekko.varsBType.TryGetValue(s2 + Globals.lagIndicator + "0", out ss2);
                        if (ss2 == null)
                        {
                            //TODO: general error handling regarding endo/exo
                            //now we get runtime error
                            new Error("regarding exogenize: variable " + s2 + " does not exist in model");
                            //throw new GekkoException();
                        }
                        int s2BNumber = ss2.bNumber;

                        if (Program.model.modelGekko.m2.endogenous.ContainsKey(s2))
                        {
                            Program.model.modelGekko.m2.endogenous.Remove(s2);
                            //Program.model.modelGekko.endogenousBNumbers.Remove(s2BNumber);  //DO NOT ACTIVATE THIS ONE -- endogenousBNumbers are dealt with in the ordering code
                            Program.model.modelGekko.m2.endoSubstitution.Add(s2, s1);
                            Program.model.modelGekko.m2.endoSubstitutionBNumbers.Add(s2BNumber, s1BNumber);
                        }
                        else
                        {
                            new Error("regarding exogenize: variable " + s2 + " is not endogenous");
                            //throw new GekkoException();
                        }
                    }
                }
            }

            // ---------------------- endogenous with b-number -- end ------------------------

            Program.model.modelGekko.m2.fromEqNumberToBNumber = G.CreateArray(Program.model.modelGekko.m2.endogenous.Count, -12345);  //used in newton
            Program.model.modelGekko.m2.fromBNumberToEqNumber = G.CreateArray(1000000, -12345);  //slack, fix, used in newton
            Program.model.modelGekko.m2.sparseInfo = new List<int>[Program.model.modelGekko.m2.endogenous.Count]; //used in newton
            Program.model.modelGekko.m2.sparseInfoLeftRightSeparated = new List<int>[Program.model.modelGekko.m2.endogenous.Count]; //used for ordering

            foreach (EquationHelper eh in Program.model.modelGekko.equations)
            {
                int varNumber = eh.bNumberLhs;
                List<int> arl = new List<int>();
                List<int> arl2 = new List<int>();
                Program.model.modelGekko.m2.sparseInfo[eh.equationNumber] = arl;
                Program.model.modelGekko.m2.sparseInfoLeftRightSeparated[eh.equationNumber] = arl2;

                int x = -12345;
                if (Program.model.modelGekko.m2.endoSubstitutionBNumbers.TryGetValue(varNumber, out x))
                {
                    varNumber = x;
                }

                Program.model.modelGekko.m2.fromEqNumberToBNumber[eh.equationNumber] = varNumber;
                Program.model.modelGekko.m2.fromBNumberToEqNumber[varNumber] = eh.equationNumber;

                //-------------------------

                List<int> temp = new List<int>();
                //Hmmm, using Contains() on a List<> could waste time instead of Dictionary, but these lists are probably short anyway
                if (!Program.model.modelGekko.m2.sparseInfo[eh.equationNumber].Contains(varNumber))
                {
                    //to avoid duplicates
                    Program.model.modelGekko.m2.sparseInfo[eh.equationNumber].Add(varNumber);
                }
                foreach (string s in eh.precedentsWithLagIndicator.Keys)
                {
                    BTypeData temp3 = (BTypeData)Program.model.modelGekko.varsBType[s];
                    int rhsVarNumber = temp3.bNumber;
                    if (Program.model.modelGekko.endogenousBNumbersOriginallyInModel.ContainsKey(rhsVarNumber))
                    {
                        if (!Program.model.modelGekko.m2.sparseInfo[eh.equationNumber].Contains(rhsVarNumber))
                        {
                            //to avoid duplicates
                            Program.model.modelGekko.m2.sparseInfo[eh.equationNumber].Add(rhsVarNumber);
                        }
                        //if (!first && !((ArrayList)sparseInfoLeftRightSeparated[eqNumber]).Contains(varNumber1))
                        if (!temp.Contains(rhsVarNumber))
                        {
                            //to avoid duplicates
                            temp.Add(rhsVarNumber);
                        }
                    }
                }
                Program.model.modelGekko.m2.sparseInfoLeftRightSeparated[eh.equationNumber].Add(varNumber);
                Program.model.modelGekko.m2.sparseInfoLeftRightSeparated[eh.equationNumber].AddRange(temp);
            }
        }        

        public static void FeedbackOrderingStuff(ECompiledModelType modelType, bool isCalledFromModelStatement)
        {

            //int xx;
            //xx = fromEqNumberToBNumber.Length; //(a)
            //xx = fromBNumberToEqNumber.Length; //(a1)

            //xx = fromEqNumberToBNumberRecursiveNEW.Count;  //(b)
            //xx = fromBNumberToEqNumberRecursiveNEW.Length; //(b1)

            //xx = fromEqNumberToBNumberFeedbackNEW.Length;  //(c)
            //xx = fromBNumberToEqNumberFeedbackNEW.Length;  //(c1)


            //frml _i i1=0.5*g;
            //frml _i i=i1;
            //frml _i y=c+i+g+e-m;                          //frml _i g=g+y-(c+i+g+e-m);
            //frml _i c=0.2*y+0.1*c+0.1*e;
            //frml _i e=-0.2*m+0.2*c-0.1*y *y;
            //frml _i m=m2;
            //frml _i m2=m3;
            //frml _i m3=m4;
            //frml _i m4=m5;
            //frml _i m5=m6;
            //frml _i m6=m7;
            //frml _i m7=m8;
            //frml _i m8=0.1*c     ;
            //frml _i x=2*y;
            //frml _i z=2*x;


            //   (a)   (b)    (c)
            //-------------------------
            //0   0    13     3
            //1   2    12     4
            //2   3    11
            //3   4    10
            //4   5    9
            //5   6    8
            //6   7    7
            //7   8    6
            //8   9    5
            //9   10
            //10  11
            //11  12
            //12  13
            //13  14
            //14  15


            //   (a1)  (b2)  (c2)       <m> = -12345
            //------------------------------------------
            //0   0    <m>   <m>        prologue
            //1  <m>   <m>   <m>        exo
            //2   1    <m>   <m>        prologue
            //3   2    <m>    1
            //4   3    <m>    0
            //5   4     8    <m>
            //6   5     7    <m>
            //7   6     6    <m>
            //8   7     5    <m>
            //9   8     4    <m>
            //10  9     3    <m>
            //11  10    2    <m>
            //12  11    1    <m>
            //13  12    0    <m>
            //14  13   <m>   <m>        epilogue
            //15  14   <m>   <m>        epilogue


            // sparseInfoLeftRightSeparated
            //
            //    before                after endo/exo of g and e.
            //------------------------------------------------
            //0     0                 0     0,1                   ++  added 1
            //1     2,0               1     2,0
            //2     3,4,2,5,6         2     3,4,2,1,6             ++  1 instead of 5
            //3     4,3,4,5           3     4,3,4                 ++  removed 5
            //4     5,6,4,3           4     1,1,6,4,3             ++  1's instead of 5
            //5     6,7               5     6,7
            //6     7,8               6     7,8
            //7     8,9               7     8,9
            //8     9,10              8     9,10
            //9     10,11             9     10,11
            //10    11,12             10    11,12
            //11    12,13             11    12,13
            //12    13,4              12    13,4
            //13    14,3              13    14,3
            //14    15,14             14    15,14

            //endogenized exo variable "g" (1):
            //add the number at rhs in equations where it is found at rhs
            //exogenized endo variable "e" (5):
            //remove where it is found at   rhs
            //at eq where it is at lhs: put in endogenized instead at lhs AND at rhs

            DateTime t0 = DateTime.Now;

            foreach (string endo in Program.model.modelGekko.m2.endoSubstitution.Keys)
            {
                //slack: iterate KeyValuePair<> instead
                string exo = Program.model.modelGekko.m2.endoSubstitution[endo];
                int exoBtype = Program.model.modelGekko.varsBType[exo + Globals.lagIndicator + "0"].bNumber;
                int endoBtype = Program.model.modelGekko.varsBType[endo + Globals.lagIndicator + "0"].bNumber;
                ArrayList eqsRhsExo = FindEqsWithVarOnRightHandSide(exo);
                ArrayList eqsRhsEndo = FindEqsWithVarOnRightHandSide(endo);
                int eqLhsEndo = FindEqWithVarOnLeftHandSide(endo);
                //for each equation with exo var on rhs
                foreach (int eq in eqsRhsExo)
                {
                    //EquationHelper eh = (EquationHelper)equations[eq];
                    //eh.rhs.Add(exoBtype, "");
                    List<int> al = Program.model.modelGekko.m2.sparseInfoLeftRightSeparated[eq];
                    for (int i = 1; i < al.Count; i++)
                    {
                        int number = al[i];
                        if (number == exoBtype) new Error("#32108743", false);
                    }
                    al.Add(exoBtype);
                }

                foreach (int eq in eqsRhsEndo)
                {
                    List<int> al = Program.model.modelGekko.m2.sparseInfoLeftRightSeparated[eq];
                    int toRemoveI = -12345;
                    for (int i = 1; i < al.Count; i++)
                    {
                        int number = al[i];
                        if (number == endoBtype)
                        {
                            toRemoveI = i;
                        }
                    }
                    al.RemoveAt(toRemoveI);
                }
                List<int> al1 = Program.model.modelGekko.m2.sparseInfoLeftRightSeparated[eqLhsEndo];
                al1[0] = exoBtype;
                al1.Add(exoBtype);
                //G.Writeln(exo + " " + endo + " " + eqsRhsEndo.ToString() + " " + eqsRhsExo.ToString() + eqLhsEndo);
            }

            Program.model.modelGekko.m2.sparseInfoSmart = new List<List<int>>();
            Program.model.modelGekko.m2.sparseInfoSmartCondensed = new List<List<int>>();  //creates an identical copy here
            for (int i = 0; i < Program.model.modelGekko.varsBType.Count; i++)
            {
                Program.model.modelGekko.m2.sparseInfoSmart.Add(null);
                Program.model.modelGekko.m2.sparseInfoSmartCondensed.Add(null);
            }
            for (int i = 0; i < Program.model.modelGekko.m2.sparseInfoLeftRightSeparated.Length; i++)
            {
                bool first = true;
                List<int> vars = Program.model.modelGekko.m2.sparseInfoLeftRightSeparated[i];
                foreach (int var in vars)
                {
                    if (first == true)
                    {
                        //first index is left side
                        Program.model.modelGekko.m2.sparseInfoSmart[vars[0]] = new List<int>();
                        Program.model.modelGekko.m2.sparseInfoSmartCondensed[vars[0]] = new List<int>();
                        first = false;
                        continue;
                    }
                    Program.model.modelGekko.m2.sparseInfoSmart[vars[0]].Add(var);
                    Program.model.modelGekko.m2.sparseInfoSmartCondensed[vars[0]].Add(var);
                }
            }


            //=================================
            //=================================
            //======== Ordering start =========
            //=================================
            //=================================


            List<List<int>> rowsIndexes = new List<List<int>>();
            List<List<int>> columnsIndexes = new List<List<int>>();
            for (int i = 0; i < Program.model.modelGekko.m2.endogenous.Count; i++)
            {
                rowsIndexes.Add(new List<int>());
                columnsIndexes.Add(new List<int>());
            }

            List<List<int>> rowsIndexes2 = new List<List<int>>();
            List<List<int>> columnsIndexes2 = new List<List<int>>();
            for (int i = 0; i < Program.model.modelGekko.m2.endogenous.Count; i++)
            {
                rowsIndexes2.Add(new List<int>());
                columnsIndexes2.Add(new List<int>());
            }

            //sparseInfoLeftRightSeparated has lhs at [0], content is b[]-type numbers
            foreach (List<int> al2 in Program.model.modelGekko.m2.sparseInfoLeftRightSeparated)
            {
                bool shouldAddOnRightHandSide = false;
                int lhs2 = al2[0];

                if (Program.model.modelGekko.m2.endoSubstitutionBNumbers.ContainsKey(lhs2))
                {
                    shouldAddOnRightHandSide = true;
                    lhs2 = Program.model.modelGekko.m2.endoSubstitutionBNumbers[lhs2];
                    for (int i = 1; i < al2.Count; i++)
                    {
                        if (al2[i] == lhs2) shouldAddOnRightHandSide = false;
                    }
                }

                int lhs = Program.model.modelGekko.m2.fromBNumberToEqNumber[lhs2];
                for (int i = 1; i < al2.Count; i++)
                {
                    int rhsi2 = al2[i];
                    int rhsi = Program.model.modelGekko.m2.fromBNumberToEqNumber[rhsi2];
                    PutIntoIndidenceMatrix(rowsIndexes, columnsIndexes, rowsIndexes2, columnsIndexes2, lhs, rhsi);
                }
                if (shouldAddOnRightHandSide == true)
                {
                    PutIntoIndidenceMatrix(rowsIndexes, columnsIndexes, rowsIndexes2, columnsIndexes2, lhs, lhs);
                }
            }

            Program.model.modelGekko.m2.prologue = new List<int>();
            Program.model.modelGekko.m2.epilogue = new List<int>();
            Program.model.modelGekko.m2.simulRecursive = new List<int>();
            Program.model.modelGekko.m2.simulFeedback = new List<int>();

            List<int> simulRecursive = new List<int>();
            List<int> simulEpi = new List<int>();

            FindRecursive(rowsIndexes, columnsIndexes, Program.model.modelGekko.m2.prologue);
            FindRecursive(columnsIndexes, rowsIndexes, Program.model.modelGekko.m2.epilogue);  //note: should be reversed, see next statement
            Program.model.modelGekko.m2.epilogue.Reverse();

            int orderingIterations = 0;
            for (int i6 = 0; i6 < int.MaxValue; i6++)
            {
                FindDiagonal(rowsIndexes, columnsIndexes, Program.model.modelGekko.m2.simulFeedback);
                if (!(Globals.solveNewtonOnlyFeedback && Globals.runningOnTTComputer)) FindRecursive(rowsIndexes, columnsIndexes, simulRecursive);

                //this one is good:
                //These are simple equations with 1 var on right-hand side
                if (!(Globals.solveNewtonOnlyFeedback && Globals.runningOnTTComputer)) FindRowsWithSum1(rowsIndexes, columnsIndexes, simulRecursive);


                //this one is bad:
                //These equations may have many right-hand vars, but impact is only in one other equation
                if (1 == 0)
                {
                    FindColsWithSum1(rowsIndexes, columnsIndexes, simulRecursive);
                }

                Heuristic(rowsIndexes, columnsIndexes, Program.model.modelGekko.m2.simulFeedback);

                bool flag = false;
                for (int i = 0; i < columnsIndexes.Count; i++)
                {
                    if (columnsIndexes[i] != null)
                    {
                        flag = true;
                        orderingIterations = i6;
                        break;
                    }
                }
                if (!flag) break;
            }

            if (!(Globals.solveNewtonOnlyFeedback && Globals.runningOnTTComputer))
            {
                Program.model.modelGekko.m2.simulFeedback.Sort();  //easier comparable to gauss-seidel inner loop
            }

            //A little bit cheating, since these numbers will always relate to the last .m2 model
            //But never mind, typically only shown for non-fixed model
            Program.model.modelGekko.modelInfo.endo3 = Program.model.modelGekko.m2.prologue.Count + Program.model.modelGekko.m2.simulFeedback.Count + simulRecursive.Count + Program.model.modelGekko.m2.epilogue.Count;
            Program.model.modelGekko.modelInfo.prologue = Program.model.modelGekko.m2.prologue.Count;
            Program.model.modelGekko.modelInfo.simultaneous = Program.model.modelGekko.m2.simulFeedback.Count + simulRecursive.Count;
            Program.model.modelGekko.modelInfo.simultaneousFeedback = Program.model.modelGekko.m2.simulFeedback.Count;
            Program.model.modelGekko.modelInfo.simultaneousRecursive = simulRecursive.Count;
            Program.model.modelGekko.modelInfo.epilogue = Program.model.modelGekko.m2.epilogue.Count;

            for (int i = 0; i < rowsIndexes2.Count; i++)
            {
                if (!simulRecursive.Contains(i))
                {
                    DeleteRowAndColumn(rowsIndexes2, columnsIndexes2, i);
                }
            }

            FindRecursive(rowsIndexes2, columnsIndexes2, Program.model.modelGekko.m2.simulRecursive);
            FindRecursive(columnsIndexes2, rowsIndexes2, simulEpi);  //this one is empty and should be!! Good test also.

            if (!(simulEpi.Count == 0))
            {
                new Error("In feedback/recursive", false);
            }

            //=================================
            //=================================
            //======== Ordering end ===========
            //=================================
            //=================================



            simulRecursive = null;  //the ordering is not correct in simulRecursive: we use simulPrologue instead below.
            //at the moment, this is not strictly necessary, but still give a nicer ordering
            //and a good check

            WriteOrderingInfoToFile(rowsIndexes);

            Program.model.modelGekko.m2.fromEqNumberToBNumberRecursiveNEW = new List<int>();
            foreach (int eq in Program.model.modelGekko.m2.simulRecursive)
            {
                Program.model.modelGekko.m2.fromEqNumberToBNumberRecursiveNEW.Add(Program.model.modelGekko.m2.fromEqNumberToBNumber[eq]);
            }
            Program.model.modelGekko.m2.fromBNumberToEqNumberRecursiveNEW = G.CreateArray(Program.model.modelGekko.varsBType.Count, -12345);
            for (int i = 0; i < Program.model.modelGekko.m2.fromEqNumberToBNumberRecursiveNEW.Count; i++)
            {
                int j = (int)Program.model.modelGekko.m2.fromEqNumberToBNumberRecursiveNEW[i];
                Program.model.modelGekko.m2.fromBNumberToEqNumberRecursiveNEW[j] = i;
            }
            Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW = new int[Program.model.modelGekko.m2.simulFeedback.Count];
            int i1 = -1;
            foreach (int eq in Program.model.modelGekko.m2.simulFeedback)
            {
                i1++;
                Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW[i1] = Program.model.modelGekko.m2.fromEqNumberToBNumber[eq];
            }

            try
            {
                //TODO: This is a hack, but probably rare with > 1.000.000 b-elements
                Program.model.modelGekko.m2.fromBNumberToEqNumberFeedbackNEW = G.CreateArray(1000000, -12345);  //slack
            }
            catch (Exception e)
            {
                new Error("Array size problem in MODEL command");
                //throw new GekkoException();
            }

            for (int i = 0; i < Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW.Length; i++)
            {
                int j = Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW[i];
                Program.model.modelGekko.m2.fromBNumberToEqNumberFeedbackNEW[j] = i;
            }

            //at this point in feedb2.frm:
            //===================================
            //simulRecursive = 12,11,10,9,8,7,6,5,4
            //simulFeedback = 3,2
            //fromEqNumberToBNumberRecursiveNEW = 13,12,11,10,9,8,7,6,5
            //fromEqNumberToBNumberFeedbackNEW = 4,3
            //===================================
            //equations 0,1 and 13,14 are prologue and epilogue

            //            sparseInfoSmart
            //fy    0	1,2,3,4
            //fy.1  1	null (!endonolag)
            //tg    2	null
            //fi    3	0,3,4
            //fe    4

            //fy = fy.1 + tg + fi + fe
            //fi = fy + fi + fe

            //sæt fy exo og tg endo: ( 0 og 2 skifter plads)

            //tg = fy – (fy.1 + fi + fe)
            //fi = fy + tg + fi + fe

            //tg    0	1,2,3,4
            //fy.1  1	null (!endonolag)
            //fy    2	null
            //fi    3	2,3,4
            //fe    4

            Dictionary<int, string> fromEqNumberToBNumberRecursiveHelper = new Dictionary<int, string>();
            foreach (int i in Program.model.modelGekko.m2.fromEqNumberToBNumberRecursiveNEW) fromEqNumberToBNumberRecursiveHelper.Add(i, null);

            //transposing matrix
            //never relevant for goals search, since means are feedback type.
            Program.model.modelGekko.m2.sparseInfoSmartCondensedTransposed = new List<List<int>>();
            for (int i = 0; i < Program.model.modelGekko.m2.sparseInfoSmartCondensed.Count; i++)
            {
                Program.model.modelGekko.m2.sparseInfoSmartCondensedTransposed.Add(null);
                if (Program.model.modelGekko.m2.sparseInfoSmartCondensed[i] != null)
                {
                    Program.model.modelGekko.m2.sparseInfoSmartCondensedTransposed[i] = new List<int>();
                }
            }

            //never relevant for goals search, since means are feedback type.
            Program.model.modelGekko.m2.sparseInfoSmartTransposed = new List<List<int>>();
            for (int i = 0; i < Program.model.modelGekko.m2.sparseInfoSmart.Count; i++)
            {
                Program.model.modelGekko.m2.sparseInfoSmartTransposed.Add(null);
                if (Program.model.modelGekko.m2.sparseInfoSmart[i] != null)
                {
                    Program.model.modelGekko.m2.sparseInfoSmartTransposed[i] = new List<int>();
                }
            }

            //never relevant for goals search, since means are feedback type.
            for (int i = 0; i < Program.model.modelGekko.varsBType.Count; i++)
            {
                if (Program.model.modelGekko.m2.sparseInfoSmart[i] != null)
                {
                    List<int> row = Program.model.modelGekko.m2.sparseInfoSmart[i];
                    foreach (int j in row)
                    {
                        if (!(Program.model.modelGekko.m2.sparseInfoSmartTransposed[j]).Contains(i))
                        {
                            //if (fromEqNumberToBNumberFeedbackNEW.Contains(i))
                            if (Array.IndexOf(Program.model.modelGekko.m2.fromEqNumberToBNumberFeedbackNEW, i) != -1)
                            {
                                //this is an extra condition, implying that we only get
                                //FR-type array for all the last n columns
                                //the transposed version is not identical to non-transposed
                                (Program.model.modelGekko.m2.sparseInfoSmartTransposed[j]).Add(i);
                            }
                        }
                    }
                }
            }

            //       feedb    recurs
            // -------------|----------
            // feedb   FF1  |  FR1
            //        ------|------
            // recur   RF1  |  RR1
            //--------------|----------
            //
            //
        }

    }


}
