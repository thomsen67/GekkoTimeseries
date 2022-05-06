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
        
    public class Model
    {
        public ModelGekko modelGekko = null;
        public ModelGams modelGams = null;
        public ModelGamsScalar modelGamsScalar = null;
    }

    [ProtoContract]
    public class ModelGekko
    {
        //[ProtoMember(1)]  --> we don't serialize this: takes only about 0.7 sec to recreate for dec09 (gauss simulation) when it is missing. Reading from file, unzipping .dll files will eat up a lot of that
        public Model2 m2 = new Model2();
        
        [ProtoMember(2)]
        public ModelInfo modelInfo = new ModelInfo();  //contains just statistics regarding number of exo, endo etc. Nothing serious here.
                
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

        [ProtoMember(29)]
        public int subPeriods = -12345;  //1 for a, 4 for q, 12 for m. The value -12345 means inactive. This is only relevant regarding the pchy() function

        [ProtoMember(30)]
        public string runBefore = null;

        [ProtoMember(31)]
        public string runAfter = null;

        public ModelGekko()
        { 
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
        public bool loadedFromMdlFile;

        public void Print()
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
            if (this.loadedFromMdlFile) cache = " (model loaded from cache file)";
            G.Writeln("Model statement ended succesfully with no errors in " + timeUsedTotal + extra + cache);
        }
    }

    [ProtoContract]
    public class ModelGams
    {
        [ProtoMember(1)]
        public ModelInfoGams modelInfo = new ModelInfoGams();
        [ProtoMember(2)]
        public GekkoDictionary<string, List<ModelGamsEquation>> equationsByVarname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);
        [ProtoMember(3)]
        public GekkoDictionary<string, List<ModelGamsEquation>> equationsByEqname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);  //The value is always a list with 1 element. Just easier that it is similar to equationsByVarname        
    }

    [ProtoContract]
    public class ModelGamsScalar
    {        
        public Func<int, double[], double[][], double[], int[][], int[][], double>[] functions = null;     
        public double[] r = null;
        public double[][] a = null;
        public int[][] bb = null;
        public double[] cc = null;
        public int[][] dd = null;
        public int[] ee = null;
        public int eqCounts = -12345;
        public int count = -12345;
        public int known = -12345;
        public int unique = -12345;
    }

    [ProtoContract]
    public class ModelInfoGams
    {
        public bool loadedFromMdlFile = false;  //do not protobuf
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
}
