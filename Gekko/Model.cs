/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
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
using System.Text;
using System.Collections;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using MathNet.Numerics.LinearAlgebra.Sparse;
using ProtoBuf;
using System.IO;
using System.Linq;

namespace Gekko
{
    [Serializable]
    public class Model2Cache
    {        
        public LruCache lru = null;
        public Model2Cache()
        {            
            lru = new LruCache(Program.options.model_cache_max);
        }        
    }
    
    [Serializable]
    [ProtoContract]
    public class Model2
    {        
        //see also endogenousOriginallyInModel       
        [ProtoMember(1)]
        public GekkoDictionary<string, string> endogenous = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);        
        [ProtoMember(2)]
        public GekkoDictionary<string, string> endoSubstitution = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        [ProtoMember(3)]
        public GekkoDictionary<int, int> endoSubstitutionBNumbers = new GekkoDictionary<int, int>();
        [ProtoMember(4)]
        public int[] fromEqNumberToBNumber;
        [ProtoMember(5)]
        public int[] fromBNumberToEqNumber;
        //a matrix containing info for each of the equations about which endogenous (without lags)
        //are present in that equation. Saves work regarding jacoby matrix etc.
        [ProtoMember(6)]
        public List<int>[] sparseInfo;
        [ProtoMember(7)]
        public List<int>[] sparseInfoLeftRightSeparated; //used for ordering
        [ProtoMember(8)]
        public List<List<int>> sparseInfoSmart;
        [ProtoMember(9)]
        public List<List<int>> sparseInfoSmartCondensed;
        [ProtoMember(10)]
        public List<int> prologue = new List<int>();
        [ProtoMember(11)]
        public List<int> epilogue = new List<int>();
        [ProtoMember(12)]
        public List<int> simulRecursive = new List<int>();
        [ProtoMember(13)]
        public List<int> simulFeedback = new List<int>();
        [ProtoMember(14)]
        public List<int> fromEqNumberToBNumberRecursiveNEW;
        [ProtoMember(15)]
        public int[] fromBNumberToEqNumberRecursiveNEW;
        [ProtoMember(16)]
        public int[] fromEqNumberToBNumberFeedbackNEW;
        [ProtoMember(17)]
        public int[] fromBNumberToEqNumberFeedbackNEW;
        [ProtoMember(18)]
        public List<List<int>> sparseInfoSmartCondensedTransposed;
        [ProtoMember(19)]
        public List<List<int>> sparseInfoSmartTransposed;
        
        public Type assemblyGauss = null;
        public Type assemblyGaussFailSafe = null;        
        public Type assemblyRes = null;        
        public Type assemblyNewton = null;                
        public Type assemblyPrologueEpilogue = null;
        public Type assemblyPrologueEpilogueFailSafe = null;
        //public Type assemblyEigen = null;        
        
    }

    [ProtoContract]
    public class ModelNull
    {        
        //just used to signal that one of modelGamsScalar, modelGams or modelGekko is = null.
        //its size will be 0, and this fact is used. See test, #ddgfcs78yusdj
    }

    [ProtoContract]
    public class Model
    {
        //Do not add fields here, in that case use ModelCommon classe.
        public ModelCommon modelCommon = new ModelCommon();
        public ModelGekko modelGekko = null;
        public ModelGams modelGams = null;
        public ModelGamsScalar modelGamsScalar = null;                

        /// <summary>
        /// Type used for decomp. This is not always the same as ModelSourceType(), which is the "born" type.
        /// </summary>
        /// <returns></returns>
        public EModelType DecompType()
        {
            if (this.modelGamsScalar != null) return EModelType.GAMSScalar;
            if (this.modelGams != null) return EModelType.GAMSRaw;
            if (this.modelGekko != null) return EModelType.Gekko;
            return EModelType.Unknown;
        }

        /// <summary>
        /// This gets folded equations from GAMS code scalar model.
        /// </summary>
        /// <param name="decompOptions"></param>
        /// <returns></returns>
        public TwoStrings GetEquationTextFoldedScalar(List<string>eqNames)
        {
            //See also how to get unfolded equations: #jseds78hsd33.
            string rv = "";
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            if (this.modelGams != null)
            {
                int i = -1;
                foreach (string eqName in eqNames)
                {
                    i++;
                    List<ModelGamsEquation> temp = null; this.modelGams.equationsByEqname.TryGetValue(eqName, out temp);
                    if (temp == null) continue;
                    foreach (ModelGamsEquation eq in temp)
                    {
                        if (i > 0) sb1.AppendLine();
                        sb1.AppendLine(eq.lhs + " = " + eq.rhs + ";");
                        if (!G.NullOrBlanks(eq.conditionals)) sb1.AppendLine("with $-condition: " + eq.conditionals);

                        if (i > 0) sb2.AppendLine();
                        sb2.AppendLine(eq.lhsGams + " =E= " + eq.rhsGams + ";");
                        if (!G.NullOrBlanks(eq.conditionalsGams)) sb2.AppendLine("with $-condition: " + eq.conditionalsGams);
                    }
                }
            }
            else if (this.modelGekko != null)
            {
                foreach (string eqName in eqNames)
                {
                    string s1 = null;
                    EquationHelper eh = Program.FindEquationByMeansOfVariableName(eqName.Substring(Globals.gekkoEquationPrefix.Length));
                    if (eh != null) s1 = eh.equationText + G.NL + G.NL;
                    sb1.Append(s1);
                    string s2 = null;
                    if (eh != null) s2 = Program.GetHumanReadableDetailedEquation(eh) + ";" + G.NL + G.NL;
                    sb2.Append(s2);
                }
            }

            TwoStrings two = new TwoStrings(sb1.ToString(), sb2.ToString());

            return two;
        }        

        /// <summary>
        /// Helper, for the DECOMP window not the FIND window.
        /// </summary>
        /// <returns></returns>
        public static string GetEquationTextHelper(List<Link>links, bool showTime, GekkoTime t0, Model model)
        {
            GekkoTime tUsedHere = t0;
            if (model.modelGamsScalar != null) tUsedHere = model.modelGamsScalar.Maybe2000GekkoTime(t0);
            string s = null;
            List<string> eqNames = new List<string>();
            foreach (Link link in links)
            {
                eqNames.Add(G.Chop_DimensionAddLast(link.GAMS_dsh[0].fullName, tUsedHere.ToString(), false));
            }
            s = model.GetEquationText(eqNames, showTime, t0);
            s += Program.SetBlanks();  //hack so that the yellow box always has enough width, also if the text is not wide and there are few years. The hack seems to work nicely so that the box glues horizontally to the splitter.
            return s;
        }

        /// <summary>
        /// The central equation text method. Gets equation text from both folded and unfolded equations.
        /// </summary>
        /// <param name="eq"></param>
        /// <param name="showTime"></param>
        /// <param name="t0"></param>
        /// <returns></returns>
        public string GetEquationText(List<string> eqs, bool showTime, GekkoTime t0)
        {            
            List<string> eqs2 = new List<string>();
            foreach (string s in eqs)
            {
                eqs2.Add(G.Chop_RemoveIndex(s));
            }
            TwoStrings two = this.GetEquationTextFoldedScalar(eqs2);

            string s2 = null;
            int i = -1;
            foreach (string s in eqs)
            {
                i++;
                if (i > 0) s2 += G.NL;
                s2 += this.modelGamsScalar.GetEquationTextUnfolded(s, showTime, t0) + G.NL;                
            }
            string rv = null;
            if (this.modelGams != null)
            {
                //rv += two.s1 + "\r";
                //rv += "------------- scalar -------------" + "\r" + "\r" + s2 + "\r";
                //rv += "-------------- GAMS --------------" + "\r" + "\r" + two.s2 + "\r";
                rv += two.s1 + G.NL;
                rv += "------------- scalar -------------" + G.NL + G.NL + s2 + G.NL;
                rv += "-------------- GAMS --------------" + G.NL + G.NL + two.s2 + G.NL;
            }
            else if (this.modelGekko != null)
            {
                rv += two.s1;
                rv += "------------- detailed -------------" + G.NL + G.NL + two.s2;
                if (false && Globals.runningOnTTComputer) rv += "------------- TTH: detailed2 -------------" + G.NL + G.NL + s2 + G.NL;
            }
            return rv;
        }

        /// <summary>
        /// This gets folded equations from GAMS code nonscalar model.
        /// </summary>
        /// <param name="decompOptions"></param>
        /// <returns></returns>
        public static string GetEquationTextFoldedNonScalar(EModelType modelType, List<Link> links)
        {
            //See also how to get unfolded equations: #jseds78hsd33.
            string rv = "";
            List<string> eqNames = new List<string>();
            foreach (Link link in links)
            {
                rv += "Equation: " + link.eqname + G.NL + G.NL + link.expressionText + G.NL + G.NL;
            }
            return rv;

        }
    }

    [ProtoContract]
    public class ModelGekko
    {
        //[ProtoMember(1)]  --> we don't serialize this: takes only about 0.7 sec to recreate for dec09 (gauss simulation) when it is missing. Reading from file, unzipping .dll files will eat up a lot of that
        public Model2 m2 = new Model2();

        [ProtoMember(2)]
        public ModelInfo modelInfo = null; //contains just statistics regarding number of exo, endo etc. Nothing serious here.
                
        //TODO: This is superflous for tempModel used for GENR statemens
        public Model2Cache m2cache = new Model2Cache();        

        public Type assemblyAfter = null;  //contains after and after2 equations        
        public Type assemblyAfterFailSafe = null;  //contains after and after2 equations        
        public Type assemblyReverted = null;
        public Type assemblyRevertedFailSafe = null;
        [ProtoMember(3)]
        public GekkoDictionary<string, string> endogenized = new GekkoDictionary<string, string> (StringComparer.OrdinalIgnoreCase);  //for use when doing goal-search, only keys are used
        [ProtoMember(4)]
        public GekkoDictionary<string, string> exogenized = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);  //for use when doing goal-search, only keys are used
        [ProtoMember(5)]
        public int largestLag = 0;  //always 0 or positive        
        [ProtoMember(6)]
        public int largestLead = 0;  //always 0 or positive
        [ProtoMember(7)]
        public int largestLeadOutsideRevertedPart = 0;  //always 0 or positive, does not count leads in RevertedX, RevertedY, RevertedAuto equations. Corresponds to .leadedVariables.
        public string modelHashTrue = null;
        [ProtoMember(8)]
        public bool fatalEndogenousError = false;  //set true if two equal auto-generated J-factors        
        //Information regarding compilation is kept here
        //Correspond to enumeration Program.ECompiledModelType                
        //residuals when Newton solving are kept here
        public double[] r;
        //to absorb info from simulating a single year (like #iterations)
        //awkward to make as a class, so double[] used. Kind of a hack.
        //0: iterations, 1: 12345 if produces NaN, 2: equation number if NaN, 3+4: iterations breakup (probe and real)
        //7: distance to end (used for leaded variables), 8: indicates terminal type
        public double[] simulateResults = new double[10];        
        /// <summary>
        /// Contains only auto-generated J-vars
        /// </summary>
        [ProtoMember(10)]
        public GekkoDictionary<string, string> varsJTypeAutoGenerated = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// Contains only auto-generated D-vars
        /// </summary>
        [ProtoMember(11)]
        public GekkoDictionary<string, string> varsDTypeAutoGenerated = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// Contains only auto-generated Z-vars
        /// </summary>
        [ProtoMember(12)]
        public GekkoDictionary<string, string>  varsZTypeAutoGenerated  = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        [ProtoMember(13)]
        public int numberOfEndo = 0;
        [ProtoMember(14)]
        public int numberOfExo = 0;
        [ProtoMember(15)]
        public int numberOfDjz = 0;
        //varsBType (stor, med lags) bruges i array i frml.cs når der simuleres
        //varsAType (lille, uden lags) bruges i varnavn x år data matrix
        [ProtoMember(16)]
        public GekkoDictionary<string, BTypeData> varsBType = new GekkoDictionary<string, BTypeData>(StringComparer.OrdinalIgnoreCase);
        [ProtoMember(17)]
        public GekkoDictionary<int, string> varsBTypeInverted = new GekkoDictionary<int, string>();
        [ProtoMember(18)]
        public GekkoDictionary<string, ATypeData> varsAType = new GekkoDictionary<string, ATypeData>(StringComparer.OrdinalIgnoreCase);
        [ProtoMember(19)]
        public GekkoDictionary<int, int> leadedVariables = new GekkoDictionary<int, int>();  //note: corresponds to .largestLeadOutsideRevertedPart rather than .largestLead
        //corresponds to endo when no EXO/ENDO is done (cf. endogenous).
        [ProtoMember(20)]       
        public GekkoDictionary<string, string> endogenousOriginallyInModel = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);  //only keys are used
        //the following 3 contain the same numbers when no endo/exo is done (the two last are always identical) 
        [ProtoMember(21)]
        public GekkoDictionary<int, string> endogenousBNumbersOriginallyInModel = new GekkoDictionary<int, string>();  //only keys are used
        [ProtoMember(22)]
        public List<int> endogenousBNumbersOriginalInModelList = null;  //for convergence check in gauss
                
        /// <summary>
        /// This is what Gekko considers the after model (epilogue), NOT what is found after
        /// 'AFTER$' in the model file. To the epilogue, Y-vars after 'AFTER$' are added. These
        /// are typically hand-made J- and Z-variables and the like.
        /// </summary>
        [ProtoMember(23)]
        public GekkoDictionary<string, string> reverted = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        [ProtoMember(24)]
        public List<EquationHelper> equations = new List<EquationHelper>();
        [ProtoMember(25)]
        public List<EquationHelper> equationsReverted = new List<EquationHelper>();
        [ProtoMember(32)]
        public List<EquationHelper> equationsNotRunAtAll = new List<EquationHelper>();
        [ProtoMember(26)]
        public GekkoDictionary<string, DependentsHelper> dependents = new GekkoDictionary<string, DependentsHelper>(StringComparer.OrdinalIgnoreCase);
        [ProtoMember(27)]
        public GekkoDictionary<string, int> fromVariableToEquationNumber = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public string generateResults = "";  //for genr statement
        public double[] b;  //used for simulation
        public double[] bVariance;  //how much b typical differs from year to year historically
        public GekkoDictionary<string, List<IterMemory>> bMemory = new GekkoDictionary<string, List<IterMemory>>(StringComparer.OrdinalIgnoreCase);  //for showing with itershow
                
        public IElementalAccessMatrix jacobiMatrix = null;  //these matrices change for each period simulated (possibly several times per period)
        public double[,] jacobiMatrixDense = null;
        public double[,] jacobiMatrixInverted = null;  //not exactly inverted: rather LUD (dense)
        public int[] jacobiMatrixInvertedIndex = null;
        [ProtoMember(28)]
        public GekkoDictionary<string, string> dampVariables = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        //public int hasBeenModelStatement = 0;
        public double[] bOld = null;
        public ESignatureStatus signatureStatus;
        public string signatureFoundInFileHeader;
        public ModelGekko oldModel = null; //used when doing stacked time
        public ModelGekko stackedModel = null; //used when doing stacked time
        public List<Dictionary<int, int>> terminalHelper = null;

        public GekkoTime lastSimPer1 = GekkoTime.tNull;
        public GekkoTime lastSimPer2 = GekkoTime.tNull;
        public string lastSimStamp = null;        

        [ProtoMember(30)]
        public string runBefore = null;

        [ProtoMember(31)]
        public string runAfter = null;
                
        public Model parent = null;  //is not protobuffed, is set while reading from protobuf

        /// <summary>
        /// Only for protobuf, use ModelGekko(Model...).
        /// </summary>
        private ModelGekko() { }

        /// <summary>
        /// Use this, do not use ModelGekko()
        /// </summary>
        /// <param name="parent"></param>
        public ModelGekko(Model parent)
        {
            this.parent = parent;
            if(parent!=null) parent.modelGekko = this;            
        }
    }

    [ProtoContract]
    public class ModelListHelper
    {
        //This class is only used as a temporary wrapper for lists that are to be saved and loaded from protobuffer file
        [ProtoMember(1)]
        public List<string> all = null;
        [ProtoMember(2)]
        public List<string> endo = null;
        [ProtoMember(3)]
        public List<string> exo = null;
        [ProtoMember(4)]
        public List<string> exod = null;
        [ProtoMember(5)]
        public List<string> exodjz = null;
        [ProtoMember(6)]
        public List<string> exoj = null;
        [ProtoMember(7)]
        public List<string> exotrue = null;
        [ProtoMember(8)]
        public List<string> exoz = null;
    }

    [ProtoContract]
    public class DependentsHelper
    {
        [ProtoMember(1)]
        public GekkoDictionary<string, string> storage = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    [ProtoContract]
    public class ModelInfo
    {
        [ProtoMember(1)]
        public int total;
        [ProtoMember(2)]
        public int endo;
        [ProtoMember(3)]
        public int exoTrue;
        [ProtoMember(4)]
        public int exoDJZ;
        [ProtoMember(5)]
        public int endo2;
        [ProtoMember(6)]
        public int endoNoAfter;
        [ProtoMember(7)]
        public int endoAfter;
        [ProtoMember(8)]
        public int endoAfter2;
        [ProtoMember(9)]
        public int endo3;
        [ProtoMember(10)]
        public int prologue;
        [ProtoMember(11)]
        public int simultaneous;
        [ProtoMember(12)]
        public int simultaneousFeedback;
        [ProtoMember(13)]
        public int simultaneousRecursive;
        [ProtoMember(14)]
        public int epilogue;
        [ProtoMember(15)]
        public string fileName;
        [ProtoMember(16)]
        public ModelListHelper modelListHelper = null;
        [ProtoMember(17)]
        public List<Program.Item> varlist = null;

        //-------------------------------------------------------------------------------------------
        //Do not save the following in protofile! (they will be created even if there is a cache hit)
        public string info;
        public string date;        
        public string timeUsedTotal;
        public string timeUsedParsing;
        //public ESignatureStatus signatureStatus;
        //public string signatureFoundInFileHeader;
        public string signatureTrue;
        public string varlistStatus;
        public string lastCompileDuration;        

        public ModelInfo()
        {            
        }

        public void Print(ModelCommon modelCommon)
        {

            string extra = "";
            if (timeUsedParsing != null)
            {
                extra = " (parse: " + timeUsedParsing + ", compile: " + Program.model.modelGekko.modelInfo.lastCompileDuration + ")";
            }
            string note = "";
            if (Program.model.modelGekko.largestLeadOutsideRevertedPart > 0) note += " (NOTE: Forward-looking model)";
            if (Program.model.modelGekko.largestLead > Program.model.modelGekko.largestLeadOutsideRevertedPart) note += " (NOTE: has table vars with lead = "+ Program.model.modelGekko.largestLead + ")";

            Table tab = new Table();

            tab.CurRow.SetTopBorder(1, 1);

            tab.CurRow.SetText(1, "MODEL " + Path.GetFileNameWithoutExtension(fileName));
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.Next();

            tab.CurRow.SetText(1, "Model     : " + fileName);
            tab.CurRow.Next();
            if (info != null)
            {
                tab.CurRow.SetText(1, "Info      : " + info);
                tab.CurRow.Next();
            }
            if (date != null)
            {
                tab.CurRow.SetText(1, "Date      : " + date);
                tab.CurRow.Next();
            }
            if (varlistStatus != null)
            {
                tab.CurRow.SetText(1, "Varlist   : " + varlistStatus);
                tab.CurRow.Next();
            }
                        
            if (Program.model.modelGekko.signatureStatus == ESignatureStatus.Ok)
            {
                tab.CurRow.SetText(1, "Signature : OK (" + Program.model.modelGekko.signatureFoundInFileHeader + ")");
                tab.CurRow.Next();
            }
            else if (Program.model.modelGekko.signatureStatus == ESignatureStatus.SignatureNotFoundInModelFile)
            {
                tab.CurRow.SetText(1, "Signature : NOT FOUND in model file (see the SIGN command)");
                tab.CurRow.Next();                
            }
            else if (Program.model.modelGekko.signatureStatus == ESignatureStatus.SignaturesDoNotMatch)
            {
                tab.CurRow.SetText(1, "Signature : INVALID SIGNATURE (" + Program.model.modelGekko.signatureFoundInFileHeader + "): See the SIGN command");
                tab.CurRow.Next();                
            }
            
            tab.CurRow.SetText(1, "Lags      : Largest lag = " + Program.model.modelGekko.largestLag + ", largest lead = " + Program.model.modelGekko.largestLeadOutsideRevertedPart + note);
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Total vars     = " + G.IntFormat(this.total, 7) + "     " + "Endogenous  = " + G.IntFormat(this.endo2, 7) + "     " + "Endogenous   = " + G.IntFormat(this.endo3, 7));
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "------------------------" + "     " + "---------------------" + "     " + "----------------------");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Endogenous     = " + G.IntFormat(this.endo, 7) + "     " + "Main vars   = " + G.IntFormat(this.endoNoAfter, 7) + "     " + "Prologue     = " + G.IntFormat(this.prologue, 7));
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "True exogenous = " + G.IntFormat(this.exoTrue, 7) + "     " + "After vars  = " + G.IntFormat(this.endoAfter, 7) + "     " + "Simultaneous = " + G.IntFormat(this.simultaneous, 7) + " (fb: " + this.simultaneousFeedback + ")");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "DJZ-type exo   = " + G.IntFormat(this.exoDJZ, 7) + "     " + "After2 vars = " + G.IntFormat(this.endoAfter2, 7) + "     " + "Epilogue     = " + G.IntFormat(this.epilogue, 7));
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.SetLeftBorder(1);
            tab.CurRow.SetRightBorder(1);            

            int widthRemember = Program.options.print_width;
            Program.options.print_width = int.MaxValue;
            try
            {
                List<string> ss = tab.Print();
                foreach (string s in ss) G.Writeln(s);
            }
            finally
            {
                //resetting, also if there is an error
                Program.options.print_width = widthRemember;
            }
            
            string cache = "";
            if (modelCommon.loadedFromCacheFile) cache = " (model loaded from cache file)";
            G.Writeln("Model statement ended succesfully with no errors in " + timeUsedTotal + extra + cache);
        }
    }

    [ProtoContract]
    public class ModelGams
    {
        
        [ProtoMember(2)]
        public GekkoDictionary<string, List<ModelGamsEquation>> equationsByVarname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);
        [ProtoMember(3)]
        public GekkoDictionary<string, List<ModelGamsEquation>> equationsByEqname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);  //The value is always a list with 1 element. Just easier that it is similar to equationsByVarname        
        
        public Model parent = null;  //is not protobuffed, is set while reading from protobuf

        /// <summary>
        /// Only for protobuf, use ModelGams(Model...).
        /// </summary>
        private ModelGams() { }

        /// <summary>
        /// Use this, do not use ModelGams()
        /// </summary>
        /// <param name="model"></param>
        public ModelGams(Model model)
        {
            this.parent = model;            
            if (model != null) model.modelGams = this;
        }
    }

    /// <summary>
    /// Common stuff for models
    /// </summary>
    [ProtoContract]
    public class ModelCommon
    {
        [ProtoMember(1)]
        private EModelType type = EModelType.Unknown;

        //See also GetFreq()
        [ProtoMember(2)]
        public EFreq freq = EFreq.None; //Used for .modelGekko. The value .None means inactive. This is only relevant regarding the pchy() and similar functions

        //not protobuffed
        public bool loadedFromCacheFile = false;  //not protobuffed

        /// <summary>
        /// Where did the model come from? This is not always the same as DecompType().
        /// </summary>
        /// <returns></returns>
        public EModelType GetModelSourceType()
        {
            return this.type;
        }

        public EFreq GetFreq()
        {
            if (this.freq == EFreq.None) return Program.options.freq;
            else return this.freq;
        }

        public void SetModelSourceType(EModelType type)
        {
            this.type = type;
        }
    }

    [ProtoContract]
    public class ModelGamsScalar
    {
        [ProtoMember(1)]
        public bool is2000Model = false;  //only defined for 2000, other periods use offsets

        //not protobuffed
        public Func<int, double[], double[][], double[], int[][], int[][], int, double>[] functions = null;

        [ProtoMember(2)]
        public IntArray[] bbTemp = null; //because protobuf does not support jagged arrays
        public int[][] bb = null;  //precedents, one array per equation. For each equation the values come in pairs (period, variable)

        [ProtoMember(3)]
        public double[] cc = null;

        [ProtoMember(4)]
        public IntArray[] ddTemp = null; //because protobuf does not support jagged arrays
        public int[][] dd = null;

        [ProtoMember(5)]
        public int[] ee = null;

        // ------------------------------------

        //not protobuffed
        public double[] r = null;

        //this protobuf is often not needed. Suppose it contains 
        //the values from the scalar model.
        [ProtoMember(6)]
        public DoubleArray[] aTemp = null; //because protobuf does not support jagged arrays
        public double[][] a = null;

        public List<int> nonExisting = null;
        public List<int> nonExisting_ref = null;

        // ------------------------------------

        //not protobuffed
        public double[] r_ref = null;

        //not protobuffed
        public double[][] a_ref = null;

        // ------------------------------------

        [ProtoMember(7)]
        public int eqCounts = -12345;

        [ProtoMember(8)]
        public int count = -12345;

        [ProtoMember(9)]
        public int known = -12345;

        [ProtoMember(10)]
        public int unique = -12345;

        /// <summary>
        /// Gekko period corresponding to time index = 0 in GAMS (at the moment, t0 = t1).
        /// Later on, t0 may become smaller than t1.
        /// </summary>
        [ProtoMember(11)]
        public GekkoTime tBasis = GekkoTime.tNull;

        /// <summary>
        /// First observed period in scalar model (at the moment equal to t0). Note: for a static scalar model, this
        /// shows the largest lag compared with year 2000.
        /// </summary>
        [ProtoMember(12)]
        public GekkoTime t1 = GekkoTime.tNull;

        /// <summary>
        /// Last observed period in scalar model (at the moment equal to t0). Note: for a static scalar model, this
        /// shows the largest lead compared with year 2000 (probably, not 100% sure).
        /// </summary>
        [ProtoMember(13)]
        public GekkoTime t2 = GekkoTime.tNull;

        // ---------- dicts etc. ------------

        //variable names without time dimension 

        [ProtoMember(14)]
        public string[] dict_FromANumberToVarName = null;

        [ProtoMember(15)]
        public GekkoDictionaryDimensional dict_FromVarNameToANumber = new GekkoDictionaryDimensional();

        //eq numbers in folded model, corresponds to i/ii dimension
        [ProtoMember(16)]
        public string[] dict_FromEqChunkNumberToEqName = null;

        [ProtoMember(17)]
        public GekkoDictionaryDimensional dict_FromEqNameToEqChunkNumber = new GekkoDictionaryDimensional();

        //lowest level equation numbers (in unfolded/unrolled model), corresponds to j/jj dimension (but do not start over at each i/ii, so these numbers are global).
        [ProtoMember(18)]
        public string[] dict_FromEqNumberToEqName = null;

        [ProtoMember(19)]
        public GekkoDictionaryDimensional dict_FromEqNameToEqNumber = new GekkoDictionaryDimensional();

        //lowest level variable numbers (in unfolded/unrolled model)
        [ProtoMember(20)]
        public string[] dict_FromVarNumberToVarName = null;

        [ProtoMember(21)]
        public GekkoDictionaryDimensional dict_FromVarNameToVarNumber = new GekkoDictionaryDimensional();

        //from lowest level equation number to chunk equations number
        [ProtoMember(22)]
        public int[] dict_FromEqNumberToEqChunkNumber = null;

        [ProtoMember(23)]
        public List<string> csCodeLines = null; //C# source code
        
        /// <summary>
        /// Points a period-and-variable to the unfolded equations it is part of. This could
        /// be a bit faster and use a bit less ram if PeriodAndVariable was a long and by using
        /// modulo.
        /// </summary>
        [ProtoMember(25)]
        public GekkoDictionary<PeriodAndVariable, List<int>> dependents = null;

        /// <summary>
        /// Points an equation number to a list of period-and-variables, that is, the variables
        /// that are part of the equation. Does not contain dublets, in contrast to the
        /// bb array.b e
        /// </summary>
        [ProtoMember(27)]
        public List<ModelScalarEquation> precedents = null;
                
        public Model parent = null;  //is not protobuffed, is set while reading from protobuf

        // =============================================
        // =============================================
        // =============================================

        /// <summary>
        /// Only for protobuf, use ModelGamsScalar(Model...).
        /// </summary>
        private ModelGamsScalar() { }

        /// <summary>
        /// Use this, do not use ModelGamsScalar()
        /// </summary>
        /// <param name="model"></param>
        public ModelGamsScalar(Model model)
        {
            this.parent = model;
            if (model != null) model.modelGamsScalar = this;
        }

        public int GetEqNumber(string eqName)
        {
            //TODO: handle errors
            return this.dict_FromEqNameToEqNumber.Get(eqName);
        }

        public string GetEqName(int eqNumber)
        {
            //TODO: handle errors
            return this.dict_FromEqNumberToEqName[eqNumber];
        }

        /// <summary>
        /// Here, varNumber is number without time dimension, used in the a array.
        /// </summary>
        /// <param name="varNumber"></param>
        /// <returns></returns>
        public string GetVarNameA(int varNumber)
        {
            //TODO: handle errors
            return this.dict_FromANumberToVarName[varNumber];
        }

        /// <summary>
        /// For an equation number, get the string names of precedents. If showTime is false, a list like "x", "x[-1]"
        /// is returned, else a list like "x[2001]", "x[2000]" is returned. In the latter case, t0 can be set to TNull.
        /// </summary>
        /// <param name="eqNumber"></param>
        /// <param name="showTime"></param>
        /// <param name="t0"></param>
        /// <returns></returns>
        public List<string> GetPrecedentsNames(int eqNumber, bool showTime, GekkoTime t0)
        {
            List<string> precedents = new List<string>();
            foreach (PeriodAndVariable dp in this.precedents[eqNumber].vars)
            {
                //see also #as7f3læaf9                
                Tuple<string, GekkoTime> tup = dp.GetVariableAndPeriod(this);
                string name2 = null;
                if (showTime)
                {
                    name2 = G.Chop_DimensionAddLast(tup.Item1, tup.Item2.ToString());
                }
                else
                {                    
                    name2 = G.Chop_DimensionAddLag(tup.Item1, this.Maybe2000GekkoTime(t0), tup.Item2, false);
                }
                precedents.Add(name2);
            }
            return precedents;
        }

        /// <summary>
        /// For a 2000-model, we need to use 2000 as the period.
        /// </summary>
        /// <param name="t0"></param>
        /// <returns></returns>
        public GekkoTime Maybe2000GekkoTime(GekkoTime t0)
        {
            GekkoTime tTemp = t0;
            if (this.is2000Model) tTemp = new GekkoTime(this.parent.modelCommon.GetFreq(), Globals.decomp2000, 1);
            return tTemp;
        }

        /// <summary>
        /// Predict GAMS scalar model equation i, and returns the evaluation. As a side-effect also puts the result into modelGamsScalar.r array,
        /// at the slot i. Note that this is slightly slower than calling functions[...] directly, so beware
        /// if calling it in a tight loop.
        /// </summary>
        /// <param name="i"></param>
        public double Eval(int i, bool isRef, int t, ref int funcCounter)
        {            
            //NOTE: this.functions() can return a sum (with illegals signal).
            funcCounter++;
            if (isRef)
            {
                this.functions[this.ee[i]](i, this.r_ref, this.a_ref, this.cc, this.bb, this.dd, t);
                return this.r_ref[i];
            }
            else
            {
                //if (i == i + 1 - 0)
                //{
                //    // y[2002] = 536 (year before 504)
                //    // if t0 = 1999, we should ask a[3][...]
                //    int[] b = bb[i];
                //    int[] d = dd[i];
                //    double[] c = cc;
                //    double ylag = a[b[2] + t][b[3]];
                //    double y = a[b[4] + t][b[5]];
                //    double ylead = a[b[6] + t][b[7]];
                //    if (i == 0) r[i] = a[b[0] + t][b[1]] - ((a[b[2] + t][b[3]]) + (a[b[4] + t][b[5]]));                    
                //    if (i == 1) r[i] = a[b[0] + t][b[1]] - ((((((c[d[0]]) * (a[b[2] + t][b[3]])) + ((c[d[1]]) * (a[b[4] + t][b[5]]))) + ((c[d[2]]) * (a[b[6] + t][b[7]])))));                    
                //}
                this.functions[this.ee[i]](i, this.r, this.a, this.cc, this.bb, this.dd, t);
                return this.r[i];
            }
        }    

        /// <summary>
        /// Get data in GAMS scalar model.
        /// </summary>
        /// <param name="period"></param>
        /// <param name="variable"></param>
        /// <param name="isRef"></param>
        /// <returns></returns>
        public double GetData(int period, int t, int variable, bool isRef)
        {
            if (isRef)
            {
                return this.a_ref[period + t][variable];
            }
            else
            {
                return this.a[period + t][variable];
            }
        }

        /// <summary>
        /// Set data in GAMS scalar model.
        /// </summary>
        /// <param name="period"></param>
        /// <param name="variable"></param>
        /// <param name="isRef"></param>
        /// <param name="value"></param>
        public void SetData(int period, int t, int variable, bool isRef, double value)
        {
            if (isRef)
            {
                this.a_ref[period + t][variable] = value;
            }
            else
            {
                this.a[period + t][variable] = value;
            }
        }

        /// <summary>
        /// Converts to internal integer representation of time period (using t0 from scalar model)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int FromGekkoTimeToTimeInteger(GekkoTime t)
        {
            return t.Subtract(this.tBasis);
        }

        /// <summary>
        /// Converts from internal integer representation of time period (using t0 from scalar model)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public GekkoTime FromTimeIntegerToGekkoTime(int t)
        {
            return this.tBasis.Add(t);
        }

        /// <summary>
        /// Obtains an a[][] data array from a Databank. This array is from model.a.
        /// if model is a ModelGamsScalar.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public void FromDatabankToAScalarModel(Databank db, bool isRef)
        {
            //Beware of OPTION series data missing, if it is set.
            //Beware of timeless series -- not handled...

            GekkoTime tStart = this.t1;
            GekkoTime tEnd = this.t2;
            if (this.is2000Model)
            {                
                tStart = new GekkoTime(this.parent.modelCommon.GetFreq(), Globals.decompHackt1, 1);
                tEnd = new GekkoTime(this.parent.modelCommon.GetFreq(), Globals.decompHackt2, 1);
                if (isRef) this.a_ref = null;
                else this.a = null;
            }

            int n = GekkoTime.Observations(tStart, tEnd);
            double[][] a = null;
            if (isRef) a = this.a_ref;
            else a = this.a;

            if (a == null)
            {
                a = new double[n][];
                //This is necessary because of the "new" above
                if (isRef)
                {
                    this.a_ref = a;
                }
                else
                {
                    this.a = a;
                }
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    a[i] = G.CreateNaN(this.CountVars(2));
                }
            }
            else
            {
                for (int i = 0; i < a.Length; i++) G.SetNaN(a[i]);  //wipe it completely
            }

            if (isRef)
            {                
                this.r_ref = G.CreateNaN(this.CountEqs(1));
            }
            else
            {             
                this.r = G.CreateNaN(this.CountEqs(1));
            }

            if (!isRef)
            {
                this.nonExisting = new List<int>();
            }
            else
            {
                this.nonExisting_ref = new List<int>();
            }

            for (int i = 0; i < this.CountVars(2); i++)
            {
                string name = this.dict_FromANumberToVarName[i];
                
                Series ts = DatabankAHelperScalarModel(db, i, name, true, isRef);
                if (ts == null)
                    continue;  //If not in the databank, it will contain NaN's

                //This runs pretty fast, operating directly on the internal timeseries array
                //Cannot use array copy, because a has time dimension first.
                //
                // NB: beware of OPTION series data missing, if it is set.
                int index1 = -12345;
                int index2 = -12345;                
                double[] data = ts.GetDataSequenceUnsafePointerAlterBEWARE(out index1, out index2, tStart, tEnd);
                for (int t = 0; t < n; t++)
                {
                    a[t][i] = data[index1 + t];
                }
            }            
        }

        /// <summary>
        /// Equations. Type 1 = all (unrolled). Type 2 = omit time dimension. Type 3 = omit all dimensions.
        /// Beware for type == 1 that it takes a tiny bit of time, do not use the function inside a long loop or as end in a long loop...
        /// </summary>
        /// <returns></returns>
        public int CountEqs(int type)
        {            
            if (type == 1) return this.dict_FromEqNumberToEqName.Length;            
            GekkoDictionary<string, int> temp = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (string s2 in this.dict_FromEqNumberToEqName)
            {                
                ExtractTimeDimensionHelper helper = GamsModel.ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, s2, false);
                if (type == 2)
                {
                    if (!temp.ContainsKey(helper.resultingFullName)) temp.Add(helper.resultingFullName, 0);
                }
                else if (type == 3)
                {
                    if (!temp.ContainsKey(helper.name)) temp.Add(helper.name, 0);
                }
                else new Error("Unexpected");
            }
            return temp.Count;
        }

        /// <summary>
        /// Variables. Type 1 = all. Type 2 = omit time dimension (corresponds to a-array variables). Type 3 = omit all dimensions.
        /// </summary>
        /// <returns></returns>
        public int CountVars(int type)
        {
            if (type == 1)
            {
                return this.dict_FromVarNumberToVarName.Length;
            }
            else if (type == 2)
            {
                return this.dict_FromANumberToVarName.Length;
            }
            else if (type == 3)
            {
                GekkoDictionary<string, int> temp = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                foreach (string s2 in this.dict_FromVarNumberToVarName)
                {                    
                    ExtractTimeDimensionHelper helper = GamsModel.ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, s2, false);
                    if (!temp.ContainsKey(helper.name)) temp.Add(helper.name, 0);
                }
                return temp.Count;
            }
            else new Error("Unexpected");
            return -12345;
        }

        /// <summary>
        /// Takes data from a Databank and puts it into an a[][] data array (this array is from model.a).
        /// If model is a ModelGamsScalar.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public void FromAToDatabankScalarModel(Databank db, bool isRef)
        {
            GekkoTime tStart = this.t1;
            GekkoTime tEnd = this.t2;
            if (this.is2000Model)
            {
                tStart = new GekkoTime(this.parent.modelCommon.GetFreq(), Globals.decompHackt1, 1);
                tEnd = new GekkoTime(this.parent.modelCommon.GetFreq(), Globals.decompHackt2, 1);
            }

            //Beware of OPTION series data missing, if it is set.
            //Beware of timeless series -- not handled...
            int n = GekkoTime.Observations(tStart, tEnd);
            double[][] a = null;
            if (isRef) a = this.a_ref;
            else a = this.a;
            
            string freq = G.ConvertFreq(Program.options.freq);
            for (int i = 0; i < this.CountVars(2); i++)
            {
                string name = this.dict_FromANumberToVarName[i];
                Series ts = DatabankAHelperScalarModel(db, i, name, false, isRef);
                //This runs pretty fast, operating directly on the internal timeseries array
                //Cannot use array copy, because a has time dimension first.
                // NB: beware of OPTION series data missing, if it is set.
                int index1 = -12345;
                int index2 = -12345;                
                double[] data = ts.GetDataSequenceUnsafePointerReadOnlyBEWARE(out index1, out index2, tStart, tEnd);
                for (int t = 0; t < n; t++)
                {
                    data[index1 + t] = a[t][i];
                }
            }
        }

        /// <summary>
        /// Helper that gets a timeseries from a databank and a name.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="name"></param>
        /// <param name="fromDatabankToA"></param>
        /// <returns></returns>
        private Series DatabankAHelperScalarModel(Databank db, int aNumber, string name, bool fromDatabankToA, bool isRef)
        {
            Series ts = null;

            string firstRef = "first-position";
            if (isRef) firstRef = "reference";

            //See also #asf87aufkdh where similar loading is done regarding reading gdx files  
            EFreq freq = this.parent.modelCommon.GetFreq();
            List<string> dims = G.Chop_GetIndex(name);
            string varNameWithFreqAndIndexes = G.Chop_AddFreq(name, freq);
            string varNameWithFreq = G.Chop_GetNameAndFreq(varNameWithFreqAndIndexes);
            int gdxDimensions = dims.Count + 1;
            int gekkoDimensions; bool isMultiDim;
            int hasTimeDimension = 1;
            GamsData.IsMultiDim(gdxDimensions, hasTimeDimension, out gekkoDimensions, out isMultiDim);  //calling this is overkill, but binds neatly with other use of the method
                        
            if (isMultiDim)
            {
                Series ats = null;
                if (fromDatabankToA)
                {
                    //only reading
                    if (!db.ContainsIVariable(varNameWithFreq))
                    {
                        if (!isRef) this.nonExisting.Add(aNumber);
                        else this.nonExisting_ref.Add(aNumber);
                        return ts;
                        //new Error("Could not find array-series '" + varNameWithFreq + "' in the " + firstRef + " databank.");
                    }
                    ats = (Series)db.GetIVariable(varNameWithFreq);
                }
                else
                {
                    //from a array to databank
                    if (!db.ContainsIVariable(varNameWithFreq))
                    {
                        string[] domains = new string[gekkoDimensions];
                        ats = new Series(freq, varNameWithFreq);
                        ats.meta.domains = domains;
                        ats.SetArrayTimeseries(gdxDimensions, hasTimeDimension == 1);
                        db.AddIVariable(ats.name, ats);
                    }
                    else
                    {
                        ats = (Series)db.GetIVariable(varNameWithFreq, true);  //for a scalar model, puts simulated data back to a databank
                    }
                }
                
                MultidimItem mmi = new MultidimItem(dims.ToArray(), ats);
                IVariable iv = null; ats.dimensionsStorage.TryGetValue(mmi, out iv); //probably never present, if merging is not allowed
                if (iv == null)
                {
                    if (fromDatabankToA)
                    {
                        if (!isRef) this.nonExisting.Add(aNumber);
                        else this.nonExisting_ref.Add(aNumber);
                        return ts;
                        //new Error("In array-series '" + varNameWithFreq + "' in the " + firstRef + " databank, could not find sub-series '" + varNameWithFreqAndIndexes + "'");
                    }
                    ts = new Series(ESeriesType.Normal, freq, Globals.seriesArraySubName + Globals.freqIndicator + G.ConvertFreq(freq));
                    ats.dimensionsStorage.AddIVariableWithOverwrite(mmi, ts);
                }
                else
                {
                    ts = (Series)iv;
                }
            }
            else
            {
                //Zero-dimensional timeseries (that is, normal timeseries)
                if (db.ContainsIVariable(varNameWithFreq))
                {
                    ts = (Series)db.GetIVariable(varNameWithFreq);
                }
                else
                {
                    if (fromDatabankToA)
                    {
                        if (!isRef) this.nonExisting.Add(aNumber);
                        else this.nonExisting_ref.Add(aNumber);
                        return ts;
                        //new Error("Could not find series '" + varNameWithFreq + "' in the " + firstRef + " databank.");
                    }
                    ts = new Series(freq, varNameWithFreq);
                    db.AddIVariable(ts.name, ts);
                }                
            }

            return ts;
        }

        /// <summary>
        /// Sets all elements in a, a_ref, r and r_ref to NaN (unless they are already == null).
        /// </summary>
        public static void FlushAAndRArrays(ModelGamsScalar modelGamsScalar)
        {
            //FLUSH! We flush a and r arrays taken from GAMS scalar model zip.
            if (modelGamsScalar.a != null)
            {
                for (int j = 0; j < modelGamsScalar.a.Length; j++) G.SetNaN(modelGamsScalar.a[j]);
            }
            if (modelGamsScalar.a_ref != null)
            {
                for (int j = 0; j < modelGamsScalar.a_ref.Length; j++) G.SetNaN(modelGamsScalar.a_ref[j]);
            }
            if (modelGamsScalar.r != null)
            {
                G.SetNaN(modelGamsScalar.r);
            }
            if (modelGamsScalar.r_ref != null)
            {
                G.SetNaN(modelGamsScalar.r_ref);
            }
        }

        /// <summary>
        /// Gets human-readable equation text corresponding to (unfolded) equation name.
        /// Uses equationChunks list, which is only about 1% of full scalar model size.
        /// The result uses the C# code (modified a bit), where stuff like a[b[0]][b[1]] and
        /// c[d[0]] is replaced with "real" variable[period]. By avoiding storing the full scalar model in human-readable form (up to 1 mio eqs),
        /// a lot of RAM is saved.
        /// </summary>
        /// <param name="eq"></param>
        /// <returns></returns>
        public string GetEquationTextUnfolded(string name, bool showTime, GekkoTime t0)
        {
            //See also #jseds78hsd33.
            //Remember: this code is dependent upon the exact format of 
            //the C# code used for the functions. Cf. #af931klljaf89efw.

            // --------------- NOTE ----------------------
            // with .is2000Model, this CAN actually produce
            // resonable "frn" kind of equation syntax.
            // But it will suffer from parentheses, so better
            // to use the "human" code produced by the .frm
            // model parser, which is designed to avoid the
            // superfluous parentheses.
            // -------------------------------------------

            int eq = this.dict_FromEqNameToEqNumber.Get(name);
            if (eq == -12345)
            {
                return "...equation '" + name + "' could not be found...";
            }

            //Beware of this: for a scalar-2000 model, time basis is always 2000.
            GekkoTime tUsedHere = this.Maybe2000GekkoTime(t0);

            int ii = this.ee[eq];
            string ss = this.csCodeLines[ii];
            StringBuilder sb = new StringBuilder();
            int more = 20;
            TokenList tokens = StringTokenizer.GetTokensWithLeftBlanks(ss, more);

            if (this.is2000Model)
            {
                //handle translation of "x1-(x2-2);" into "x1=x2-2" , where () represents RHS

                for (int i = 0; i < tokens.Count() - more - 1; i++)
                {
                    //remove last (;
                    if (tokens[i].s == ")" && tokens[i + 1].s == ";")
                    {
                        tokens[i].s = "";
                        tokens[i + 1].s = "";
                        break;
                    }
                }

                for (int i = 0; i < tokens.Count() - more - 1; i++)
                {
                    //find first "-("
                    if (tokens[i].s == "-" && tokens[i + 1].s == "(")
                    {
                        tokens[i].s = "";
                        tokens[i + 1].s = "=";
                        tokens[i + 1].leftblanks = 1;
                        tokens[i + 2].leftblanks = 1;
                        goto Lbl1a;
                    }
                }
            Lbl1a:;
            }
            else
            {
                //look backwards to find first "-("
                for (int i = tokens.Count() - more - 1; i >= 1; i--)
                {
                    //handle translation of "-x1 + x2 - (2);" into "-x1 + x2 = 2" , where () represents RHS
                    //which is always constant  
                    if (tokens[i].s == ";" && tokens[i - 1].s == ")")
                    {
                        tokens[i].s = "";
                        tokens[i - 1].s = "";
                        for (int j = i - 1; j >= 1; j--)
                        {
                            if (tokens[j].s == "(" && tokens[j - 1].s == "-")
                            {
                                tokens[j].s = "";
                                tokens[j - 1].s = "=";
                                tokens[j].leftblanks = 1;
                                tokens[j - 1].leftblanks = 1;
                                goto Lbl1b;
                            }
                        }
                    }
                }
            Lbl1b:;
            }        

            bool start = false;
            for (int i = 0; i < tokens.Count() - more; i++)
            {
                if (start == false && tokens[i].s == "=")
                {
                    start = true;
                    continue;
                }
                
                if (!start) continue;

                if (tokens[i].s == "+" || tokens[i].s == "-" || tokens[i].s == "*" || tokens[i].s == "/")
                {
                    tokens[i].leftblanks = 1;
                    tokens[i + 1].leftblanks = 1;
                }
                
                if (tokens[i].s == "a" && tokens[i + 1].s == "[" && tokens[i + 2].s == "b" && tokens[i + 3].s == "[" && tokens[i + 5].s == "]" && tokens[i + 6].s == "+" && tokens[i + 7].s == "t" && tokens[i + 8].s == "]" && tokens[i + 9].s == "[" && tokens[i + 10].s == "b" && tokens[i + 11].s == "[" && tokens[i + 13].s == "]" && tokens[i + 14].s == "]")
                {
                    int i1 = this.bb[eq][int.Parse(tokens[i + 4].s)];
                    int i2 = this.bb[eq][int.Parse(tokens[i + 12].s)];
                    GekkoTime gt = this.FromTimeIntegerToGekkoTime(i1);
                    string varname = this.GetVarNameA(i2);
                    string varname2 = null;
                    if (showTime)
                    {
                        varname2 = G.Chop_DimensionAddLast(varname, gt.ToString());
                    }
                    else
                    {
                        varname2 = G.Chop_DimensionAddLag(varname, tUsedHere, gt, false);
                    }
                    sb.Append(G.Blanks(tokens[i].leftblanks) + varname2);
                    i += 14;
                }
                else if (tokens[i].s == "c" && tokens[i + 1].s == "[" && tokens[i + 2].s == "d" && tokens[i + 3].s == "[" && tokens[i + 5].s == "]" && tokens[i + 6].s == "]")
                {
                    //constants
                    int i1 = int.Parse(tokens[i + 4].s);
                    double c = this.cc[this.dd[eq][i1]];
                    sb.Append(G.Blanks(tokens[i].leftblanks) + c.ToString());
                    i += 6;
                }
                else if (tokens[i].s == "M" && tokens[i + 1].s == ".")
                {
                    //M.Log(...) etc.
                    tokens[i].s = "";
                    tokens[i + 1].s = "";
                    Gekko.GamsModel.RenameFunctions(tokens[i + 2], false);
                    sb.Append(tokens[i + 2].ToString());
                    i += 2;
                }                               
                else
                {
                    sb.Append(tokens[i].ToString());
                }                
            }
            return sb.ToString().Trim();
        }

        public string GamsModelDefinedString()
        {
            return "GAMS model is defined over the period " + this.t1.ToString() + " to " + this.t2.ToString();
        }

    }

    [ProtoContract]
    public class ModelScalarEquation
    {
        [ProtoMember(1)]
        public List<PeriodAndVariable> vars = new List<PeriodAndVariable>();
    }    

    [ProtoContract]
    public class ModelGamsEquation
    {
        [ProtoMember(1)]
        public string nameGams = null;

        [ProtoMember(2)]
        public string setsGams = null;

        [ProtoMember(3)]
        public List<string> setsGamsList = null;  //list of uncontrolled sets, no #-indicator

        [ProtoMember(4)]
        public string conditionalsGams = null;

        [ProtoMember(5)]
        public string lhsGams = null;

        [ProtoMember(6)]
        public string rhsGams = null;

        // Gekko variant 1 (Gekko syntax) ----------------------------------

        [ProtoMember(7)]
        public string conditionals = null;

        [ProtoMember(8)]
        public string lhs = null;

        [ProtoMember(9)]
        public string rhs = null;        

        [ProtoMember(10)]
        public List<EquationVariablesGams> expressionVariablesWithSets = new List<EquationVariablesGams>(); //for each expression in .expressions: contains the list of variables in the eq        
        
        // Gekko variant 2 (C# syntax) ----------------------------------

        [ProtoMember(11)]
        public string conditionalsCs = null;

        [ProtoMember(12)]
        public string allCs = null;       
        
        // ===========================================
        // ===========================================
        // ===========================================

        public TokenHelper lhsTokensGams = null;
        public TokenHelper rhsTokensGams = null;
        public TokenHelper allTokensGams = null;

        public List<Func<GekkoSmpl, IVariable>> expressions = new List<Func<GekkoSmpl, IVariable>>();

    }

    [ProtoContract]
    public class EquationVariablesGams
    {
        [ProtoMember(1)]
        public List<string> equationVariables = new List<string>();
    }

    [ProtoContract]
    public class ModelGamsScalarEquation
    {
    }

    [ProtoContract]
    public class IntArray
    {
        [ProtoMember(1)]
        public int[] storage = null;
    }

    [ProtoContract]
    public class DoubleArray
    {
        [ProtoMember(1)]
        public double[] storage = null;
    }
}
