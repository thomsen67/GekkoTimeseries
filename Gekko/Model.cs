/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2025, Thomas Thomsen, T-T Analyse.

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
using MathNet.Numerics.LinearAlgebra.Sparse;
using ProtoBuf;
using System.IO;
using System.Linq;

namespace Gekko
{
    public class GetEquationTextHelper2
    {
        public string s1;
        public string s2;
        public string s3;
        public List<string> mathRename;
    }


    public class GetEquationTextHelper
    {
        public string resultingText;
        public string s_scalarModel;  //is actually just .s_scalarModelMathRename with .mathRename inserted, but we keep it for now. Maybe remove it in Gekko 4.0.
        public string s_scalarModelMathRename;
        public string s_gekkoSyntax;
        public string s_gamsOrFrnSyntax;
        public bool hasHit = true;
        public List<string> mathRename = null;
    }

    /// <summary>
    /// Because protobuf does not allow nested lists
    /// </summary>
    [ProtoContract]
    public class EquationNameChunks
    {
        [ProtoMember(1)]
        public List<string> chunks = new List<string>();
        [ProtoMember(2)]
        public GamsWalkerInfo info = new GamsWalkerInfo();
    }

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
        //Do not add fields here, in that case use ModelCommon class.
        public ModelCommon modelCommon = new ModelCommon();
        public ModelGekko modelGekko = null;
        public ModelGams modelGams = null;
        public ModelGamsScalar modelGamsScalar = null;                

        /// <summary>
        /// Type used for decomp. This is not always the same as GetModelSourceType(), which is the "born" type.
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
        /// This gets raw equations from GAMS code scalar model.
        /// </summary>
        /// <param name="decompOptions"></param>
        /// <returns></returns>
        public TwoStrings GetEquationTextRawScalar(List<DName>eqNames)
        {
            //See also how to get unfolded equations: #jseds78hsd33.
            string rv = "";
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            if (this.modelGams != null)
            {
                int i = -1;
                foreach (DName eqName in eqNames)
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

                        string sets = null;
                        if (!G.NullOrBlanks(eq.setsGams)) sets = "over sets: " + eq.setsGams;
                        string dollar = null;
                        if (!G.NullOrBlanks(eq.conditionalsGams)) dollar = "with $-condition: " + eq.conditionalsGams;
                        if (sets != null && dollar != null) sb2.AppendLine(sets + ", " + dollar);
                        else sb2.AppendLine(sets + dollar);                        
                    }
                }
            }
            else if (this.modelGekko != null)
            {
                foreach (DName eqName in eqNames)
                {
                    if (eqName == null) continue;  //probably not necessary
                    if (!G.StartsWith(eqName.GetName(), Globals.gekkoEquationPrefix)) continue;
                    string s1 = null;
                    EquationHelper eh = Program.FindEquationByMeansOfVariableName(new DNameSimplest(eqName.GetName().Substring(Globals.gekkoEquationPrefix.Length)));
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
        public static string GetEquationTextHelper(List<Link>links, EquationTextHelper helper, GekkoTime t0, Model model)
        {
            GekkoTime tUsedHere = t0;
            if (model.modelGamsScalar != null) tUsedHere = model.modelGamsScalar.Maybe2000GekkoTime(t0);
            string s = null;
            List<DName> eqNames = new List<DName>();            
            foreach (Link link in links)
            {
                if (link.GAMS_dsh != null && link.GAMS_dsh.Count > 0) eqNames.Add(link.GAMS_dsh[0].fullName.AddTime(tUsedHere));
            }
            s = model.GetEquationText(eqNames, helper, t0).resultingText;
            s += Program.SetBlanks();  //hack so that the yellow box always has enough width, also if the text is not wide and there are few years. The hack seems to work nicely so that the box glues horizontally to the splitter.
            return s;
        }

        /// <summary>
        /// The central equation text method. Gets equation text from both raw and unfolded equations.
        /// Returns the resulting text, but also the three parts of it (fields s_...).
        /// </summary>
        /// <param name="eq"></param>
        /// <param name="showTime"></param>
        /// <param name="t0"></param>
        /// <returns></returns>
        public GetEquationTextHelper GetEquationText(List<DName> eqs, EquationTextHelper helper, GekkoTime t0)
        {
            GetEquationTextHelper rv = new GetEquationTextHelper();

            bool hit = false;  //if anything is found
            List<DName> eqs2 = new List<DName>();
            foreach (DName s in eqs)
            {
                eqs2.Add(new DNameSimplest(s.GetName()));
            }
            TwoStrings two = this.GetEquationTextRawScalar(eqs2);

            if (!G.NullOrBlanks(two.s1) || !G.NullOrBlanks(two.s2)) hit = true;                        

            //Now, we are creating these three (and the resulting text)
            // -- s_scalarModel
            // -- s_gekkoSyntax
            // -- s_gamsOrFrnSyntax
            
            int i = -1;
            foreach (DName eq in eqs)
            {
                i++;
                if (i > 0) rv.s_scalarModel += G.NL;
                if (this.modelGamsScalar != null)
                {
                    GetEquationTextHelper2 two2a = null;
                    try
                    {
                        two2a = this.modelGamsScalar.GetEquationTextUnfolded(eq, helper, false, t0, null);
                    }
                    catch
                    {
                        if (Globals.greuHack)
                        {
                            two2a = new GetEquationTextHelper2();
                            two2a.s1 = "<could not obtain equation>";
                            two2a.s2 = "<could not obtain equation>";
                        }
                        else
                        {
                            throw;
                        }
                    }
                    rv.s_scalarModel += two2a.s2 + G.NL;
                    if (Program.options.model_gams_scalar_normalize)
                    {
                        GetEquationTextHelper2 two2b = this.modelGamsScalar.GetEquationTextUnfolded(eq, helper, true, t0, null);
                        rv.s_scalarModelMathRename += two2b.s2 + G.NL;
                        rv.mathRename = two2b.mathRename;
                    }                    
                    if (!rv.s_scalarModel.Contains(Globals.eqs6)) hit = true;
                }
                else
                {
                    rv.s_scalarModel += Globals.eqs5 + G.NL;
                }
            }            
            rv.s_gekkoSyntax = two.s1;     //For ADAM-like it is raw .frm equation. For GAMS-like it is GAMS translated into Gekko.
            rv.s_gamsOrFrnSyntax = two.s2; //For ADAM-like it is .frn equation.     For GAMS-like it is raw GAMS.
            if (this.modelGekko != null)
            {
                rv.resultingText += rv.s_gekkoSyntax;
                rv.resultingText += Globals.eqs4 + G.NL + G.NL + rv.s_gamsOrFrnSyntax;
            }
            else
            {                
                if (G.NullOrBlanks(rv.s_gekkoSyntax)) rv.s_gekkoSyntax = Globals.eqs2 + G.NL;
                if (G.NullOrBlanks(rv.s_gamsOrFrnSyntax)) rv.s_gamsOrFrnSyntax = Globals.eqs2 + G.NL;
                
                string scalarText = rv.s_scalarModel;
                if (false && Program.options.model_gams_scalar_normalize)
                {
                    scalarText = Program.MathNormalize1(null, rv.s_scalarModelMathRename, rv.mathRename, rv.s_scalarModel);
                    //Maybe use GamsModel.ScoreEquationGivenVariable(), over incoming vars, to get the LHS variable???
                }

                if (G.Equal(Program.options.decomp_equation_style, "gekko"))
                {
                    rv.resultingText += rv.s_gekkoSyntax + G.NL;
                    rv.resultingText += Globals.eqs1 + G.NL + G.NL + scalarText + G.NL;
                    rv.resultingText += Globals.eqs3 + G.NL + G.NL + rv.s_gamsOrFrnSyntax + G.NL;
                }
                else
                {
                    rv.resultingText += rv.s_gamsOrFrnSyntax + G.NL;
                    rv.resultingText += Globals.eqs1 + G.NL + G.NL + scalarText + G.NL;
                    rv.resultingText += Globals.eqs3a + G.NL + G.NL + rv.s_gekkoSyntax + G.NL;
                }
            }            
            
            if (!hit) rv.hasHit = false;            
            return rv;
        }

        /// <summary>
        /// This gets raw equations from GAMS code nonscalar .gms file.
        /// </summary>
        /// <param name="decompOptions"></param>
        /// <returns></returns>
        public static string GetEquationTextRawNonScalar(EModelType modelType, List<Link> links)
        {
            //See also how to get raw equations: #jseds78hsd33.
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
        public Dictionary<DName, DName> endogenized = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);  //for use when doing goal-search, only keys are used
        [ProtoMember(4)]
        public Dictionary<DName, DName> exogenized = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);  //for use when doing goal-search, only keys are used
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
        public Dictionary<DName, DName> varsJTypeAutoGenerated = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);
        /// <summary>
        /// Contains only auto-generated D-vars
        /// </summary>
        [ProtoMember(11)]
        public Dictionary<DName, DName> varsDTypeAutoGenerated = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);
        /// <summary>
        /// Contains only auto-generated Z-vars
        /// </summary>
        [ProtoMember(12)]
        public Dictionary<DName, DName>  varsZTypeAutoGenerated = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);
        [ProtoMember(13)]
        public int numberOfEndo = 0;
        [ProtoMember(14)]
        public int numberOfExo = 0;
        [ProtoMember(15)]
        public int numberOfDjz = 0;
        //varsBType (stor, med lags) bruges i array i frml.cs når der simuleres
        //varsAType (lille, uden lags) bruges i varnavn x år data matrix
        [ProtoMember(16)]
        public Dictionary<DName, BTypeData> varsBType = new Dictionary<DName, BTypeData>(Multidim2Comparer.IgnoreCase);
        [ProtoMember(17)]
        public Dictionary<int, DName> varsBTypeInverted = new Dictionary<int, DName>();
        [ProtoMember(18)]
        public Dictionary<DName, ATypeData> varsAType = new Dictionary<DName, ATypeData>(Multidim2Comparer.IgnoreCase);
        [ProtoMember(19)]
        public Dictionary<int, int> leadedVariables = new Dictionary<int, int>();  //note: corresponds to .largestLeadOutsideRevertedPart rather than .largestLead
        //corresponds to endo when no EXO/ENDO is done (cf. endogenous).
        [ProtoMember(20)]       
        public Dictionary<DName, DName> endogenousOriginallyInModel = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);  //only keys are used
        //the following 3 contain the same numbers when no endo/exo is done (the two last are always identical) 
        [ProtoMember(21)]
        public Dictionary<int, DName> endogenousBNumbersOriginallyInModel = new Dictionary<int, DName>();  //only keys are used
        [ProtoMember(22)]
        public List<int> endogenousBNumbersOriginalInModelList = null;  //for convergence check in gauss
                
        /// <summary>
        /// This is what Gekko considers the after model (epilogue), NOT what is found after
        /// 'AFTER$' in the model file. To the epilogue, Y-vars after 'AFTER$' are added. These
        /// are typically hand-made J- and Z-variables and the like.
        /// </summary>
        [ProtoMember(23)]
        public Dictionary<DName, DName> reverted = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);
        [ProtoMember(24)]
        public List<EquationHelper> equations = new List<EquationHelper>();
        [ProtoMember(25)]
        public List<EquationHelper> equationsReverted = new List<EquationHelper>();
        [ProtoMember(32)]
        public List<EquationHelper> equationsNotRunAtAll = new List<EquationHelper>();
        [ProtoMember(26)]
        public GekkoDictionary<DName, DependentsHelper> dependents = new GekkoDictionary<DName, DependentsHelper>(Multidim2Comparer.IgnoreCase);
        [ProtoMember(27)]
        public GekkoDictionary<DName, int> fromVariableToEquationNumber = new GekkoDictionary<DName, int>(Multidim2Comparer.IgnoreCase);
        public string generateResults = "";  //for genr statement
        public double[] b;  //used for simulation
        public double[] bVariance;  //how much b typical differs from year to year historically
        public Dictionary<GekkoTime, List<IterMemory>> bMemory = new Dictionary<GekkoTime, List<IterMemory>>();  //for showing with itershow
                
        public IElementalAccessMatrix jacobiMatrix = null;  //these matrices change for each period simulated (possibly several times per period)
        public double[,] jacobiMatrixDense = null;
        public double[,] jacobiMatrixInverted = null;  //not exactly inverted: rather LUD (dense)
        public int[] jacobiMatrixInvertedIndex = null;
        [ProtoMember(28)]
        public Dictionary<DName, DName> dampVariables = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);
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
        [ProtoMember(9)]
        public byte[] model__info = null;  //recurrect <model>__info.zip.
    }

    [ProtoContract]
    public class DependentsHelper
    {
        [ProtoMember(1)]        
        public Dictionary<DName, DName> storage = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);
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
        public string freq;
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

            string sfreq = null;
            if (freq == "q" || freq == "m")
            {
                sfreq = " (freq = " + freq + ")";
            }

            tab.CurRow.SetText(1, "MODEL " + Path.GetFileNameWithoutExtension(fileName));
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.Next();

            tab.CurRow.SetText(1, "Model     : " + fileName + sfreq);
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
    public class ModelInfoGamsScalar
    {
        [ProtoMember(1)]        
        public string modelName = null;

        [ProtoMember(2)]
        public GekkoTime periodT1 = GekkoTime.tNull;

        [ProtoMember(3)]
        public GekkoTime periodT2 = GekkoTime.tNull;

        [ProtoMember(4)]
        public int countEqs1 = -12345;

        [ProtoMember(5)]
        public int countEqs2 = -12345;

        [ProtoMember(6)]
        public int countEqs3 = -12345;

        [ProtoMember(7)]
        public int countVars1 = -12345;

        [ProtoMember(8)]
        public int countVars2 = -12345;

        [ProtoMember(9)]
        public int countVars3 = -12345;

        [ProtoMember(10)]
        public int hasReadSomeData = -12345;

        public void Print(bool loadedFromCacheFile, bool hasResVariables, DateTime t)
        {
            Table tab = new Table();

            tab.CurRow.SetTopBorder(1, 1);

            tab.CurRow.SetText(1, "MODEL " + Path.GetFileNameWithoutExtension(this.modelName));
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.Next();

            tab.CurRow.SetText(1, "Model   : " + this.modelName);
            tab.CurRow.Next();

            tab.CurRow.SetText(1, "Periods : " + this.periodT1.ToString() + "-" + this.periodT2.ToString() + " = " + GekkoTime.Observations(this.periodT1, this.periodT2) + " periods");

            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "All eqs         = " + this.countEqs1 + " (all dimensions)");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Eqs per period  = " + this.countEqs2 + " (no time dimension)");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Eq names        = " + this.countEqs3 + " (no dimensions)");
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "All vars        = " + this.countVars1 + " (all dimensions)");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Vars per period = " + this.countVars2 + " (no time dimension)");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Var names       = " + this.countVars3 + " (no dimensions)");
            if (this.hasReadSomeData > 0)
            {
                tab.CurRow.SetBottomBorder(1, 1);
                tab.CurRow.Next();
                tab.CurRow.SetText(1, "Var data        = " + this.hasReadSomeData + " observations");
            }
            if (hasResVariables)
            {
                tab.CurRow.SetBottomBorder(1, 1);
                tab.CurRow.Next();
                tab.CurRow.SetText(1, "Decomp          : " + Globals.decompResidualPrefix + "...-variables active");
            }
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.SetLeftBorder(1);
            tab.CurRow.SetRightBorder(1);

            int widthRemember = Program.options.print_width;
            Program.options.print_width = int.MaxValue;
            try
            {
                List<string> ss = tab.Print();
                using (Writeln txt = new Writeln())
                {
                    foreach (string s in ss)
                    {
                        txt.MainAdd(s);
                        txt.MainNewLineTight();
                    }
                    if (loadedFromCacheFile)
                    {
                        txt.MainAdd("Loaded from cache, time: " + G.Seconds(t));
                    }
                    else
                    {
                        txt.MainAdd("Extracting from files, time: " + G.Seconds(t));
                    }
                    txt.MainNewLineTight();                    
                }
            }
            finally
            {
                //resetting, also if there is an error
                Program.options.print_width = widthRemember;
            }
        }        
    }

    [ProtoContract]
    public class ModelGams
    {
        
        [ProtoMember(2)]
        public Dictionary<DName, List<ModelGamsEquation>> equationsByVarname = new Dictionary<DName, List<ModelGamsEquation>>(Multidim2Comparer.IgnoreCase);
        [ProtoMember(3)]
        public Dictionary<DName, List<ModelGamsEquation>> equationsByEqname = new Dictionary<DName, List<ModelGamsEquation>>(Multidim2Comparer.IgnoreCase);  //The value is always a list with 1 element. Just easier that it is similar to equationsByVarname        

        public Dictionary<DName, EquationLhsPoints> lhsVariables = null;  //Is created when FIND is first used -- at that point we have sets/lists, too.
        
        public Model parent = null;  //is not protobuffed, is set while reading from protobuf

        [ProtoMember(4)]
        public string rawGmsFile = null; //mostly used when creating a scalar model from another scalar model, copiying this over, too.

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

        [ProtoMember(3)]
        public ModelCacheParams cacheParameters =null;

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

        /// <summary>
        /// Sets to Program.options.freq if not set.
        /// </summary>
        /// <returns></returns>
        public EFreq GetFreq()
        {
            if (this.freq == EFreq.None) return Program.options.freq;
            else return this.freq;
        }

        public EFreq GetRealFreq()
        {            
            return this.freq;
        }

        public void SetModelSourceType(EModelType type)
        {
            this.type = type;
        }
    }

    [ProtoContract]
    public class ModelCacheParams
    {
        //Note: for Gekko models m2 type, the lists of endo/exo are included in the hash (added as text to the frm before hashing)
        //For MODEL<dep=#m> we really should consider the #m list. In principle it should be baked into 
        //    this object...
        //
        [ProtoMember(1)]
        public string dep; //is flattened here, comma and semicolon-separated, no blanks.

        [ProtoMember(2)]
        public bool option_model_gams_dep_current;

        [ProtoMember(3)]
        public string option_model_gams_dep_method;

        [ProtoMember(5)]
        public bool option_solve_gauss_reorder;

        [ProtoMember(6)]
        public bool option_model_gams_scalar_data;

        public bool IsSame(ModelCacheParams other)
        {
            if (!G.Equal(this.dep, other.dep)) return false;
            if (this.option_model_gams_dep_current != other.option_model_gams_dep_current) return false;
            if (!G.Equal(this.option_model_gams_dep_method, other.option_model_gams_dep_method)) return false;
            if (this.option_solve_gauss_reorder != other.option_solve_gauss_reorder) return false;
            if (this.option_model_gams_scalar_data != other.option_model_gams_scalar_data) return false;
            return true;
        }
    }

    [ProtoContract]
    public class ModelGamsScalar
    {
        [ProtoMember(1)]
        public bool isPerpetualModel = false;  //only defined for phoney period 2000, other periods use offsets

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
        [ProtoMember(32)]
        public ByteArray[] fixTemp = null; //because protobuf does not support jagged arrays
        public double[][] a = null;
        public byte[][] fix = null;

        public Dictionary<int, int> nonExisting = new Dictionary<int, int>();
        public Dictionary<int, int> nonExisting_ref = new Dictionary<int, int>();

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
        /// Gekko period corresponding to time index = 0 in GAMS (at the moment, tBasis = t1).
        /// Later on, tBasis may become smaller than t1.
        /// </summary>
        [ProtoMember(11)]
        public GekkoTime tBasis = GekkoTime.tNull;

        /// <summary>
        /// First observed period in scalar model (at the moment equal to tBasis). 
        /// Note: for a static scalar model, this shows the largest lag compared with year 2000/2000q1/2000m1.
        /// </summary>
        [ProtoMember(12)]
        public GekkoTime absoluteT1 = GekkoTime.tNull;

        /// <summary>
        /// Last observed period in scalar model (at the moment equal to t0). 
        /// Note: for a static scalar model, this shows the largest lead compared with year 2000/2000q1/2000m1.
        /// </summary>
        [ProtoMember(13)]
        public GekkoTime absoluteT2 = GekkoTime.tNull;

        // ---------- dicts etc. ------------

        //variable names without time dimension 

        [ProtoMember(14)]
        public DName[] dict_FromANumberToVarName = null;

        [ProtoMember(15)]
        public Dictionary<DName, int> dict_FromVarNameToANumber = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);

        //eq numbers in raw model, corresponds to i/ii dimension
        [ProtoMember(16)]        
        public DName[] dict_FromEqChunkNumberToEqName = null;

        [ProtoMember(17)]        
        public Dictionary<DName, int> dict_FromEqNameToEqChunkNumber = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);

        //lowest level equation numbers (in unfolded/unrolled model), corresponds to j/jj dimension (but do not start over at each i/ii, so these numbers are global).
        [ProtoMember(18)]        
        public DName[] dict_FromEqNumberToEqName = null;

        [ProtoMember(19)]        
        public Dictionary<DName, int> dict_FromEqNameToEqNumber = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);

        //lowest level variable numbers (in unfolded/unrolled model)
        [ProtoMember(20)]
        public Multidim2Element[] dict_FromVarNumberToVarName = null;

        [ProtoMember(21)]        
        public Dictionary<DName, int> dict_FromVarNameToVarNumber = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);

        //from lowest level equation number to chunk equations number
        [ProtoMember(22)]        
        public int[] dict_FromEqNumberToEqChunkNumber = null;

        [ProtoMember(23)]
        public List<string> csCodeLines = null; //C# source code
        
        /// <summary>
        /// Points a period-and-variable to the unfolded equations (equation numbers) it is part of.
        /// </summary>
        [ProtoMember(25)]
        public Dictionary<long, List<int>> dependents = null;

        /// <summary>
        /// Points an equation number to a list of period-and-variables, that is, the variables
        /// that are part of the equation. Does not contain dublets, in contrast to the
        /// bb array.b e
        /// </summary>
        [ProtoMember(27)]
        public List<ModelScalarEquation> precedents = null;

        [ProtoMember(28)]
        public bool[] isTimeless = null;        

        public Model parent = null;  //is not protobuffed, is set while reading from protobuf

        /// <summary>
        /// Start of data for static perpetual model, not protobuffed.
        /// </summary>
        public GekkoTime perpetualT1 = GekkoTime.tNull;

        /// <summary>
        /// End of data for static perpetual model, not protobuffed.
        /// </summary>
        public GekkoTime perpetualT2 = GekkoTime.tNull;

        public List<string> varnamesWithoutDimensions = null;  //cache, not protobuffed

        [ProtoMember(29)]
        public ModelInfoGamsScalar modelInfoGamsScalar = null; //contains just statistics for when the model loads from cache. Nothing serious here.

        [ProtoMember(30)]
        public int fakeEqCounts = -12345;

        [ProtoMember(31)]
        public int fakeVarCounts = -12345;

        [ProtoMember(33)]  //(32) is above
        public bool hasResVariables = false;  //if a variable res_... is encountered

        [ProtoMember(34)]  //Only used when there are no res_... Has equation name as keys and LHS variable name as value
        public Dictionary<DName, DName> depNames = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);

        //These are not protobuffed for now, used to speed up html browser etc. and perhaps flowgraph
        public Dictionary<DName, DName> depNames2 = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);
        public Dictionary<DName, List<DName>> depNames2Inverted = new Dictionary<DName, List<DName>>(Multidim2Comparer.IgnoreCase);

        [ProtoMember(35)]
        public int hasReadSomeData = 0; //if Program.options.model_gams_scalar_data == true, AND at least one data value was read from gams.gms.

        [ProtoMember(36)]
        public GekkoTime t1 = GekkoTime.tNull; //Local option MODEL<%t1 %t2>...

        [ProtoMember(37)]
        public GekkoTime t2 = GekkoTime.tNull; //Local option MODEL<%t1 %t2>...

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

        public int GetEqNumber(DName eqName)
        {
            //TODO: handle errors
            int i; if (!this.dict_FromEqNameToEqNumber.TryGetValue(eqName, out i))
            {
                i = -12345;
            }
            return i;
        }

        /// <summary>
        /// Input an eq number and get an equation name (string) returned, for instance "E_tIOy[tje,tje,2027]", where
        /// the last part is the year.        /// 
        /// </summary>
        /// <param name="eqNumber"></param>
        /// <returns></returns>
        public DName GetEqName(int eqNumber)
        {
            //TODO: handle errors
            return this.dict_FromEqNumberToEqName[eqNumber];
        }

        /// <summary>
        /// Here, varNumber is number without time dimension, used in the a array.
        /// The string does not contain blanks around commas.
        /// </summary>
        /// <param name="varNumber"></param>
        /// <returns></returns>
        public string GetVarNameA_OLD(int varNumber)
        {
            //TODO: handle errors
            return this.dict_FromANumberToVarName[varNumber].ToString();
        }

        /// <summary>
        /// Here, varNumber is number without time dimension, used in the a array.
        /// The string does not contain blanks around commas.
        /// </summary>
        /// <param name="varNumber"></param>
        /// <returns></returns>
        public DName GetVarNameA(int varNumber)
        {
            //TODO: handle errors
            return this.dict_FromANumberToVarName[varNumber];
        }

        /// <summary>
        /// For an equation number, get the string names of precedents. If showTime is false, a list like "x", "x[-1]"
        /// is returned, else a list like "x[2001]", "x[2000]" is returned. In the latter case, t0 can be set to TNull.
        /// The names do not contain blanks around commas.
        /// </summary>
        /// <param name="eqNumber"></param>
        /// <param name="showTime"></param>
        /// <param name="t0"></param>
        /// <returns></returns>
        public List<string> GetPrecedentsNames(int eqNumber, EquationTextHelper helper, GekkoTime t0)
        {
            List<string> precedents = new List<string>();
            bool b = false; // G.Equal(Program.options.decomp_equation_style, "gams");
            foreach (long dp in this.precedents[eqNumber].vars)
            {
                if (Globals.greuHack && ModelGamsScalar.UnpackVariable(dp) == -12345)
                {
                    continue; //Some vars with " in element names (E_qCO2e_BU_energy_Corp_es_e_i["energy_Corp",heating,Other oil products,01011]). Should be handled with DName etc. later on
                }
                //see also #as7f3læaf9                
                Tuple<DName, GekkoTime> tup = this.GetVariableAndPeriod(dp);
                string name2 = null;
                if (helper.showTime)
                {
                    name2 = G.Chop_DimensionAddLast(tup.Item1.ToString(), tup.Item2.ToString());
                }
                else
                {                    
                    name2 = G.Chop_DimensionAddLag(tup.Item1.ToString(), this.Maybe2000GekkoTime(t0), tup.Item2, b, b, " "); //qwerty
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
            if (this.isPerpetualModel) tTemp = new GekkoTime(this.parent.modelCommon.GetFreq(), Globals.decomp2000, 1);            
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
        public double GetData(int period, int t, int variable, bool missingAsZero, bool isRef)
        {
            //Beware: does missingAsZero==false still slow this down for simations??
            if (this.isTimeless[variable])
            {
                if (isRef)
                {
                    double d = this.a_ref[Globals.decompTimelessNumber][variable];
                    if (missingAsZero && double.IsNaN(d)) d = 0d;
                    return d;
                }
                else
                {
                    double d = this.a[Globals.decompTimelessNumber][variable];
                    if (missingAsZero && double.IsNaN(d)) d = 0d;
                    return d;
                }
            }
            else
            {
                if (isRef)
                {
                    double d = this.a_ref[period + t][variable];
                    if (missingAsZero && double.IsNaN(d)) d = 0d;
                    return d;
                }
                else
                {
                    double d = this.a[period + t][variable];
                    if (missingAsZero && double.IsNaN(d)) d = 0d;
                    return d;
                }
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
            if (this.isTimeless[variable])
            {
                if (isRef)
                {
                    this.a_ref[Globals.decompTimelessNumber][variable] = value;
                }
                else
                {
                    this.a[Globals.decompTimelessNumber][variable] = value;
                }
            }
            else
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
        /// Input a varname without freq (possibly with indexes) for a GAMS scalar model, and it returns the periods that are fixed.
        /// Returns null if no scalar model is loaded. May crash if the name is wrong.
        /// </summary>
        /// <param name="varnameWithLag"></param>
        /// <returns></returns>
        public List<GekkoTime> GetFixedPeriods(DName varnameWithLag)
        {
            int aNumber; 
            if (!this.dict_FromVarNameToANumber.TryGetValue(varnameWithLag.RemoveTime(), out aNumber)) aNumber = -12345;
            if (aNumber == -12345)
            {
                //hov
            }
            List<GekkoTime> list = new List<GekkoTime>();
            for (int timeIndex = 0; timeIndex < this.fix.Length; timeIndex++)
            {
                byte fix = this.fix[timeIndex][aNumber];
                GekkoTime t = this.FromTimeIntegerToGekkoTime(timeIndex);
                if (fix == 1) list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// Sets all elements in a, a_ref, r and r_ref to NaN (unless they are already == null).
        /// </summary>
        public void FlushAAndRArrays()
        {
            if (this.isPerpetualModel)
            {
                //perpetual model, fill data in according to time period
                //non-perpetual model, fill all data in
                //NOTE: when widening with new periods, it would be possible to reuse existing period,
                //      but probably does not matter regarding performance.                

                int n = GekkoTime.Observations(this.perpetualT1, this.perpetualT2);
                this.a = new double[n][];
                this.a_ref = new double[n][];
                for (int j = 0; j < n; j++) this.a[j] = new double[this.dict_FromANumberToVarName.Length];
                for (int j = 0; j < n; j++) this.a_ref[j] = new double[this.dict_FromANumberToVarName.Length];
            }
            else
            {
                //non-perpetual model, fill all data in                               
            }

            if (this.a != null)
            {
                for (int j = 0; j < this.a.Length; j++) G.SetNaN(this.a[j]);
            }
            if (this.a_ref != null)
            {
                for (int j = 0; j < this.a_ref.Length; j++) G.SetNaN(this.a_ref[j]);
            }

            if (this.r != null)
            {
                G.SetNaN(this.r);
            }
            if (this.r_ref != null)
            {
                G.SetNaN(this.r_ref);
            }
        }        

        /// <summary>
        /// For any eval of GAMS scalar model, we must consider loading
        /// data into the model arrays. The 'Refresh' button can refresh the data from databanks to the values stored in the scalar model.        
        /// 
        /// </summary>
        /// <returns></returns>
        public void MaybeLoadDataIntoModel(int depth, GekkoTime gt1, GekkoTime gt2, bool ignoreMissing, bool forceRefresh)
        {
            bool hasPeriodChanged = false;
            bool hasDatabankChanged = true;  //in the longer run, keep track of that

            if (this.isPerpetualModel)
            {
                int largestLag = this.Maybe2000GekkoTime(GekkoTime.tNull).Subtract(this.absoluteT1);
                int largestLead = this.absoluteT2.Subtract(this.Maybe2000GekkoTime(GekkoTime.tNull));
                GekkoTime staticT1Probe = gt1.Add(-largestLag - Globals.decompLagAddition);
                GekkoTime staticT2Probe = gt2.Add(largestLead);

                if (this.perpetualT1.IsNull() || this.perpetualT2.IsNull())
                {                    
                    hasPeriodChanged = true;
                    this.perpetualT1 = staticT1Probe;
                    this.perpetualT2 = staticT2Probe;
                }
                else
                {
                    if (staticT1Probe.StrictlySmallerThan(this.perpetualT1) || staticT2Probe.StrictlyLargerThan(this.perpetualT2))
                    {
                        hasPeriodChanged = true;
                        this.perpetualT1 = staticT1Probe;
                        this.perpetualT2 = staticT2Probe;
                    }
                    else
                    {                    
                    }
                }                
            }            

            bool shouldUpdate = false;
            if (hasDatabankChanged || hasPeriodChanged) shouldUpdate = true;
            if (depth > 0) shouldUpdate = false;  //do not update sub-windows (unless refreshing, see below)
            if (forceRefresh) shouldUpdate = true;  //always overrides
            if (!shouldUpdate) return; //never returns

            DateTime t0 = DateTime.Now;
            this.FlushAAndRArrays();
            this.FromDatabankToAScalarModel(Program.databanks.GetFirst(), false, ignoreMissing);
            this.FromDatabankToAScalarModel(Program.databanks.GetRef(), true, ignoreMissing);
            if (Globals.runningOnTTComputer) G.Writeln2("TTH: Loading data to a array: " + G.Seconds(t0), System.Drawing.Color.Gray);  //writeln2 to avoid popup

            if (false)
            {
                //?? how to report this ??
                //   if activated, it will become a popup...

                //Will have depth = 0 here, so ok to write it to screen.
                if (this.nonExisting.Count > 0)
                {
                    this.WriteMissingModelGamsScalarVariables(false);
                }
                if (this.nonExisting_ref.Count > 0)
                {
                    this.WriteMissingModelGamsScalarVariables(true);
                }
            }
        }


        /// <summary>
        /// Used to pack period+variable, both i1 and i2 are expected to be >= 0. See .UnpackPeriod() and .UnpackVariable().
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static long PackPeriodAndVariable(int i1, int i2)
        {
            return ((long)i1 << 32) | (uint)i2;
        }

        /// <summary>
        /// Used to pack period+variable, the return value is expected to be >= 0. See PackPeriodAndVariable() and GetVariableAndPeriod().
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static int UnpackPeriod(long l)
        {
            return (int)(l >> 32);
        }

        /// <summary>
        /// Used to pack period+variablee, the return value is expected to be >= 0. See PackPeriodAndVariable() and GetVariableAndPeriod().
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static int UnpackVariable(long l)
        {
            return (int)l;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public Tuple<DName, GekkoTime> GetVariableAndPeriod(long l)
        {            
            GekkoTime gt = this.FromTimeIntegerToGekkoTime(ModelGamsScalar.UnpackPeriod(l));
            DName varName = this.GetVarNameA(ModelGamsScalar.UnpackVariable(l));
            Tuple<DName, GekkoTime> tup = new Tuple<DName, GekkoTime>(varName, gt);
            return tup;
        }

        public void WriteMissingModelGamsScalarVariables(bool isRef)
        {
            using (Writeln txt = new Writeln())
            {
                string s = "first-position";
                if (isRef) s = "reference";
                txt.MainAdd(this.nonExisting.Count + " model variables were not found in the " + s + " databank");
                txt.MoreAdd("Some model variables were not found in the databank. For DECOMP, this is only a problem if the variables are part of the decomposed equations.");
                txt.MoreAdd("The " + this.nonExisting.Count + " missing variables/timeseries are the following:");
                txt.MoreNewLine();
                List<int> list = this.nonExisting.Keys.ToList();
                if (isRef) list = this.nonExisting_ref.Keys.ToList();
                List<string> names = new List<string>();
                foreach (int aNumber in list)
                {
                    names.Add(this.dict_FromANumberToVarName[aNumber].ToString());
                }
                names = names.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default)).ToList();
                foreach (string name in names)
                {
                    txt.MoreAdd(name);
                    txt.MoreNewLineTight();
                }
            }
        }

        /// <summary>
        /// Obtains an a[][] data array from a Databank. This array is from model.a.
        /// if model is a ModelGamsScalar. The second dimension of a is
        /// .dict_FromANumberToVarName, so it is the variables actually present
        /// in the model. If a variable does not exist (for instance, if qM[tot] is
        /// present in the model and either qM or qM[tot] does not exist, the qM[tot]
        /// "slot" will have missing values.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public void FromDatabankToAScalarModel(Databank db, bool isRef, bool decompIgnoreMissing)
        {
            GekkoTime tStart = this.absoluteT1;
            GekkoTime tEnd = this.absoluteT2;
            if (this.isPerpetualModel)
            {
                tStart = this.perpetualT1;
                tEnd = this.perpetualT2;
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
                this.nonExisting = new Dictionary<int, int>();
            }
            else
            {
                this.nonExisting_ref = new Dictionary<int, int>();
            }

            for (int i = 0; i < this.CountVars(2); i++)
            {
                string name = this.dict_FromANumberToVarName[i].ToString(); //#kkkasafasf7 qwerty, the DatabankAHelperScalarModel() could just use the DName directly 

                if (Globals.greuHack && name.Contains("'")) continue;

                Series ts = DatabankAHelperScalarModel(db, i, name, true, isRef, false);  //Later on, it is checked wheather the ts series is timeless or not. So ok to keep last argument false, since this has worked implicitly = false for a long time.
                if (ts == null)
                {
                    //Will have missing values
                }
                else
                {

                    //This runs pretty fast, operating directly on the internal timeseries array
                    //Cannot use array copy, because a has time dimension first.
                    //
                    // NB: beware of OPTION series data missing, if it is set.
                    int index1 = -12345;
                    int index2 = -12345;

                    if (ts.type == ESeriesType.Timeless)
                    {                        
                        double data = ts.GetTimelessData();                            
                        a[Globals.decompTimelessNumber][i] = data;                        
                    }
                    else
                    {
                        double[] data = ts.GetDataSequenceUnsafePointerAlterBEWARE(out index1, out index2, tStart, tEnd);
                        for (int t = 0; t < n; t++)
                        {
                            a[t][i] = data[index1 + t];
                        }
                    }
                }

                bool canSetResToZero = Program.options.decomp_res_missing == ESeriesMissing.Zero && G.StartsWith(name, Globals.decompResidualPrefix);

                if (canSetResToZero || G.DecompShouldHandleMissings(decompIgnoreMissing, false) == ESeriesMissing.Zero)
                {
                    //In principle, here we could distinguish between a missing sub-series or a missing normal series,
                    //but for simplicity in DECOMP we just use one option: option series data missing = zero, and we
                    //perform the replacement at the very end of the method.
                    //
                    //The reason Program.options.series_data_missing has not already changed the data is that we are using
                    //the non-cloning GetDataSequenceUnsafePointerAlterBEWARE().
                    //
                    for (int t = 0; t < n; t++)
                    {
                        if (G.IsNumericalError(a[t][i])) a[t][i] = 0d;
                    }
                }
            }
        }

        /// <summary>
        /// Equations. Type 1 = all (unrolled). Type 2 = omit time dimension. Type 3 = omit all dimensions.
        /// Beware for type != 1 that it takes a tiny bit of time, do not use the function inside a long loop or as end in a long loop...
        /// See also GetEqs().
        /// </summary>
        /// <returns></returns>
        public int CountEqs(int type)
        {            
            if (type == 1) return this.dict_FromEqNumberToEqName.Length;
            Dictionary<DName, int> temp = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);
            foreach (DName s2 in this.dict_FromEqNumberToEqName)
            {
                if (!this.t1.IsNull() && s2 == null) continue;
                
                if (type == 2)
                {
                    if (!temp.ContainsKey(s2)) temp.Add(s2, 0);
                }
                else if (type == 3)
                {
                    DName xx = new DNameSimplest(s2.GetName());
                    if (!temp.ContainsKey(xx)) temp.Add(xx, 0);
                }
                else new Error("Unexpected");
            }
            return temp.Count;
        }

        /// <summary>
        /// Equations. Type 1 = all (unrolled). Type 2 = omit time dimension. Type 3 = omit all dimensions.
        /// Beware for type != 1 that it takes a tiny bit of time, do not use the function inside a long loop or as end in a long loop...
        /// See also CountEqs().
        /// </summary>
        /// <returns></returns>
        public List<DName> GetEqs(int type)
        {
            List<DName> rv = null;
            if (type == 1)
            {                
                rv = new List<DName>();
                foreach (DName mm in this.dict_FromEqNumberToEqName) rv.Add(mm);
                return rv;
            }
            else
            {
                Dictionary<DName, int> temp = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);
                foreach (DName s2 in this.dict_FromEqNumberToEqName)
                {                    
                    if (type == 2)
                    {
                        if (!temp.ContainsKey(s2)) temp.Add(s2, 0);
                    }
                    else if (type == 3)
                    {
                        DName xx = new DNameSimplest(s2.GetName());
                        if (!temp.ContainsKey(xx)) temp.Add(xx, 0);
                    }
                    else new Error("Unexpected");
                }
                rv = temp.Keys.ToList();
            }
            //rv.Sort(); //Users can use sort() themselves
            return rv;
        }

        /// <summary>
        /// Variables. Type 1 = all. Type 2 = omit time dimension (corresponds to a-array variables). Type 3 = omit all dimensions.
        /// See also GetVars().
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
                Dictionary<DName, int> temp = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);
                foreach (DName s2 in this.dict_FromVarNumberToVarName)
                {                    
                    DName xx = new DNameSimplest(s2.GetName());
                    if (!temp.ContainsKey(xx)) temp.Add(xx, 0);
                }
                return temp.Count;
            }
            else new Error("Unexpected");
            return -12345;
        }

        /// <summary>
        /// Variables. Type 1 = all. Type 2 = omit time dimension (corresponds to a-array variables). Type 3 = omit all dimensions.
        /// See also CountVars(). Type 3 takes time, but the result is cached for later reuse.
        /// </summary>
        /// <returns></returns>
        public List<string> GetVars(int type)
        {
            List<string> rv = null;
            if (type == 1)
            {
                rv = new List<string>();
                foreach (Multidim2Element mm in this.dict_FromVarNumberToVarName) rv.Add(mm.ToString());
                return rv;
            }
            else if (type == 2)
            {
                rv = new List<string>();
                foreach (Multidim2Element mm in this.dict_FromANumberToVarName) rv.Add(mm.ToString());
                return rv;                
            }
            else if (type == 3)
            {
                //Now uses a cache to speed up
                if (this.varnamesWithoutDimensions != null)
                {
                    rv = this.varnamesWithoutDimensions;  //retrieve from cache
                }
                else
                {
                    GekkoDictionary<string, int> temp = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    foreach (Multidim2Element s2 in this.dict_FromANumberToVarName)
                    {
                        string name = G.Chop_GetName(s2.ToString());
                        if (!temp.ContainsKey(name)) temp.Add(name, 0);
                    }
                    rv = temp.Keys.ToList();
                    this.varnamesWithoutDimensions = new List<string>(rv);  //cloned and put into cache, 958
                }
            }
            else new Error("Unexpected");
            rv.Sort();
            return rv;
        }

        /// <summary>
        /// Takes data from a[][] array and puts it into a Databank (this array is from model.a).
        /// If model is a ModelGamsScalar.        
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public void FromAToDatabankScalarModel(Databank db, bool isRef)
        {
            GekkoTime tStart = this.absoluteT1;
            GekkoTime tEnd = this.absoluteT2;
            if (this.isPerpetualModel)
            {
                tStart = this.perpetualT1;
                tEnd = this.perpetualT2;
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
                string name = this.dict_FromANumberToVarName[i].ToString();
                Series ts = DatabankAHelperScalarModel(db, i, name, false, isRef, this.isTimeless[i]);
                //This runs pretty fast, operating directly on the internal timeseries array
                //Cannot use array copy, because a has time dimension first.
                // NB: beware of OPTION series data missing, if it is set.
                if (this.isTimeless[i])
                {
                    ts.SetTimelessData(a[Globals.decompTimelessNumber][i]);
                }
                else
                {
                    int index1 = -12345;
                    int index2 = -12345;
                    double[] data = ts.GetDataSequenceUnsafePointerReadOnlyBEWARE(out index1, out index2, tStart, tEnd);
                    for (int t = 0; t < n; t++)
                    {
                        data[index1 + t] = a[t][i];
                    }
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
        private Series DatabankAHelperScalarModel(Databank db, int aNumber, string name, bool fromDatabankToA, bool isRef, bool isTimeless)
        {            
            Series ts = null;

            string firstRef = "first-position";
            if (isRef) firstRef = "reference";

            //See also #asf87aufkdh where similar loading is done regarding reading gdx files  
            int hasTimeDimension = 1;
            if (isTimeless) hasTimeDimension = 0;
            EFreq freq = this.parent.modelCommon.GetFreq();
            List<string> dims = G.Chop_GetIndex(name);
            string varNameWithFreqAndIndexes = G.Chop_AddFreq(name, freq);
            string varNameWithFreq = G.Chop_GetNameAndFreq(varNameWithFreqAndIndexes);
            int gdxDimensions = dims.Count + hasTimeDimension;
            int gekkoDimensions; bool isMultiDimInModel;            
            GamsData.IsMultiDim(gdxDimensions, hasTimeDimension, out gekkoDimensions, out isMultiDimInModel);  //calling this is overkill, but binds neatly with other use of the method
            
            if (isMultiDimInModel)
            {
                Series ats = null;
                if (fromDatabankToA)
                {
                    //only reading
                    if (!db.ContainsIVariable(varNameWithFreq))
                    {
                        if (!isRef)
                        {
                            if (!this.nonExisting.ContainsKey(aNumber)) this.nonExisting.Add(aNumber, 0);
                        }
                        else
                        {
                            if (!this.nonExisting_ref.ContainsKey(aNumber)) this.nonExisting_ref.Add(aNumber, 0);
                        }
                        return ts;
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
                                
                if (ats.type != ESeriesType.ArraySuper)
                {
                    //If a model has the array-series x[a], but the databank only has the normal series x.
                    if (!isRef)
                    {
                        if (!this.nonExisting.ContainsKey(aNumber)) this.nonExisting.Add(aNumber, 0);
                    }
                    else
                    {
                        if (!this.nonExisting_ref.ContainsKey(aNumber)) this.nonExisting_ref.Add(aNumber, 0);
                    }
                    return ts;
                }

                MultidimElement mmi = new MultidimElement(dims.ToArray(), ats);
                IVariable iv = null;                
                ats.dimensionsStorage.TryGetValue(mmi, out iv); //probably never present, if merging is not allowed
                
                if (iv == null)
                {
                    if (fromDatabankToA)
                    {
                        if (!isRef)
                        {
                            if (!this.nonExisting.ContainsKey(aNumber)) this.nonExisting.Add(aNumber, 0);
                        }
                        else
                        {
                            if (!this.nonExisting_ref.ContainsKey(aNumber)) this.nonExisting_ref.Add(aNumber, 0);
                        }
                        return ts;
                    }
                    if (isTimeless)
                    {
                        ts = new Series(ESeriesType.Timeless, freq, Globals.seriesArraySubName + Globals.freqIndicator + G.ConvertFreq(freq), double.NaN);
                    }
                    else
                    {
                        ts = new Series(ESeriesType.Normal, freq, Globals.seriesArraySubName + Globals.freqIndicator + G.ConvertFreq(freq));
                    }
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
                    if (ts.type == ESeriesType.ArraySuper)
                    {
                        //If a model has the normal series x, but the databank only has the array-series x[...]
                        if (!isRef)
                        {
                            if (!this.nonExisting.ContainsKey(aNumber)) this.nonExisting.Add(aNumber, 0);
                        }
                        else
                        {
                            if (!this.nonExisting_ref.ContainsKey(aNumber)) this.nonExisting_ref.Add(aNumber, 0);
                        }
                        return ts;
                    }
                }
                else
                {
                    if (fromDatabankToA)
                    {
                        if (!isRef)
                        {
                            if (!this.nonExisting.ContainsKey(aNumber)) this.nonExisting.Add(aNumber, 0);
                        }
                        else
                        {
                            if (!this.nonExisting_ref.ContainsKey(aNumber)) this.nonExisting_ref.Add(aNumber, 0);                         
                        }
                        return ts;
                    }
                    if (isTimeless)
                    {
                        ts = new Series(ESeriesType.Timeless, freq, varNameWithFreq, double.NaN);
                    }
                    else
                    {
                        ts = new Series(freq, varNameWithFreq);
                    }
                    db.AddIVariable(ts.name, ts);
                }                
            }

            return ts;
        }

        /// <summary>
        /// Gets human-readable equation text corresponding to (unfolded) equation name.
        /// Uses equationChunks list, which is only about 1% of full scalar model size.
        /// The result uses the C# code (modified a bit), where stuff like a[b[0]][b[1]] and
        /// c[d[0]] is replaced with "real" variable[period]. By avoiding storing the full 
        /// scalar model in human-readable form (up to 1 mio eqs),
        /// a lot of RAM is saved.
        /// The ScalarDictionary is for re-emitting a scalar model, this arg can be set to null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="showTime"></param>
        /// <param name="t0"></param>
        /// <returns></returns>
        public GetEquationTextHelper2 GetEquationTextUnfolded(DName name, EquationTextHelper helper, bool useMathRename, GekkoTime t0, ScalarDictionary sd)
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

            bool b = G.Equal(Program.options.decomp_equation_style, "gams");

            List<string> mathRename = null;
            if (useMathRename) mathRename = new List<string>();

            int eq;
            if(!this.dict_FromEqNameToEqNumber.TryGetValue(name, out eq))            
            {
                if (sd == null)
                {
                    GetEquationTextHelper2 tmp2 = new GetEquationTextHelper2();
                    tmp2.s1 = null;
                    tmp2.s2 = "...equation '" + name + "' " + Globals.eqs6;
                    tmp2.s3 = null;
                    tmp2.mathRename = null;
                    return tmp2;
                }
                else
                {
                    new Error("Could not find equation " + name + " in period " + t0.ToString());
                }
            }

            //Beware of this: for a scalar-2000 model, time basis is always 2000.
            GekkoTime tUsedHere = this.Maybe2000GekkoTime(t0);

            int ii = this.ee[eq];
            string ss = this.csCodeLines[ii];
            StringBuilder sb = new StringBuilder();
            int more = 20;
            TokenList tokens = StringTokenizer.GetTokensWithLeftBlanks(ss, more);

            if (this.isPerpetualModel)
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

            DName resName = null;
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
                    if (this.isTimeless[i2]) gt = t0;  //otherwise, this timeless variable will show with a large lag...
                    DName dName= this.GetVarNameA(i2);
                    DName dName2 = null;

                    DName varname = null; //this.GetVarNameA_OLD(i2);
                    DName varname2 = null;
                    if (G.StartsWith(dName.GetName(), Globals.decompResidualPrefix)) resName = varname;
                    if (helper.showTime)
                    {
                        dName2 = dName.AddTime(gt);
                        varname2 = dName2;
                    }
                    else
                    {
                        if (sd != null) new Error("Not showing time not expected");
                        dName2 = dName.AddTime(new GekkoTime(EFreq.Lag, gt.Subtract(tUsedHere)));
                        varname2 = dName2;
                        if (false && Globals.greuHack) //Switched off for now, need to do the fix
                        {
                            //The equation text really ought to be math + DName showhow.
                            //Maybe some List of string-or-DName, but how? Maybe List<object> that
                            //is then unfolded?
                            //ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK 
                            //ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK 
                            //ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK 
                            //ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK 
                            //ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK ULTRAHACK 
                            varname2 = new DNameSimplest(dName2.ToString(Globals.varFormatInScalarEquation));
                        }
                    }
                    
                    if (mathRename != null)
                    {
                        varname2 = new DNameSimplest(Program.MathPutIntoDict(mathRename, varname2.ToString()));
                    }
                    
                    if (sd != null) //Used for special identities model
                    {
                        if (!sd.vars.ContainsKey(varname2))
                        {
                            DName xName = new DNameSimplest("x" + (sd.vars.Count + 1));
                            sd.vars.Add(varname2, xName);  //Starts with x1
                            sd.varsList.Add(varname2);  //Will start at slot 0
                            varname2 = xName;
                        }
                        else
                        {
                            varname2 = sd.vars[varname2];
                        }                        
                    }
                    sb.Append(G.Blanks(tokens[i].leftblanks) + varname2);
                    i += 14;
                }
                else if (tokens[i].s == "c" && tokens[i + 1].s == "[" && tokens[i + 2].s == "d" && tokens[i + 3].s == "[" && tokens[i + 5].s == "]" && tokens[i + 6].s == "]")
                {
                    //constants
                    int i1 = int.Parse(tokens[i + 4].s);
                    double c = this.cc[this.dd[eq][i1]];
                    
                    string sC = c.ToString();
                    if (false)
                    {
                        //Doing constants may result in fC*0.8, not 0.8*fC
                        if (mathRename != null)
                        {
                            sC = Program.MathPutIntoDict(mathRename, sC);
                        }
                    }

                    sb.Append(G.Blanks(tokens[i].leftblanks) + sC);
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

            DName rv1 = null;
            if (helper.showEq)
            {                
                if (helper.showTime)
                {
                    //do nothing, time is already there

                    if (sd != null)
                    {
                        if (!sd.eqs.ContainsKey(name))
                        {
                            DName eName = new DNameSimplest("e" + (sd.eqs.Count + 1));
                            sd.eqs.Add(name, eName);  //Starts with e1
                            sd.eqsList.Add(name);  //Will start with slot 0
                            rv1 = eName;
                        }
                        else
                        {
                            new Error("Dublet equation");  //Should not be possible.
                        }
                    }
                    else
                    {
                        rv1 = name;
                    }
                }
                else
                {
                    if (sd != null) new Error("Not showing time not expected");
                    rv1 = name.RemoveTime();
                }
            }
            GetEquationTextHelper2 tmp = new GetEquationTextHelper2();
            tmp.s1 = rv1.ToString();
            tmp.s2 = sb.ToString().Trim();
            if (resName != null) tmp.s3 = resName.ToString();
            tmp.mathRename = mathRename;
            return tmp;
        }        

        public string GamsModelDefinedString()
        {
            return "The GAMS model is defined over the period " + this.absoluteT1.ToString() + " to " + this.absoluteT2.ToString();
        }

        public GekkoTime GetDecompT()
        {
            if (!this.t1.IsNull()) //Then .t2 is also non-null!
            {
                //A GAMS scalar model with local time period like MODEL<%t1 %t2>... --> we use %t2-1
                return this.t2.Add(Globals.decompPeriodDistanceFromEndPeriod);
            }
            else
            {                
                return this.absoluteT2.Add(Globals.decompPeriodDistanceFromEndPeriod);
            }            
        }

        /// <summary>
        /// For a variable (possibly with indexes excluding time), it returns equation names (possibly with indexes) where
        /// the variable is considered dependent (from the eq name). For a Gekko model, the variable name is returned with
        /// prefix "e_".
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="isModelGekko"></param>
        /// <returns></returns>
        public List<DName> GetDependentEquations(DName variableName, bool isModelGekko)
        {
            List<DName> lhsEqs = new List<DName>();
            if (isModelGekko)
            {
                lhsEqs.Add(variableName.SetNamePrefix("e_"));
            }
            else
            {
                foreach (KeyValuePair<DName, DName> kvp in this.depNames)
                {                    
                    if (G.Equal(kvp.Value, variableName))
                    {
                        lhsEqs.Add(kvp.Key);
                    }
                }
            }
            return lhsEqs;
        }


    }

    [ProtoContract]
    public class ModelScalarEquation
    {
        [ProtoMember(1)]
        //Each period+variable is packed as a long, in that order.
        public List<long> vars = new List<long>(); 
    }    

    [ProtoContract]
    public class ModelGamsEquation
    {
        [ProtoMember(1)]
        public DName nameGams = null;

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

        [ProtoMember(11)]
        public List<DName> lhsVars = new List<DName>();

        [ProtoMember(12)]
        public List<EquationNameChunks> lhsVarsChunks = new List<EquationNameChunks>();

        [ProtoMember(13)]
        public List<DName> rhsVars = new List<DName>();

        [ProtoMember(14)]
        public List<EquationNameChunks> rhsVarsChunks = new List<EquationNameChunks>();

        // Gekko variant 1 (Gekko syntax) ----------------------------------

        [ProtoMember(7)]
        public string conditionals = null;

        [ProtoMember(8)]
        public string lhs = null;

        [ProtoMember(9)]
        public string rhs = null;        

        [ProtoMember(10)]
        public List<EquationVariablesGams> expressionVariablesWithSets = new List<EquationVariablesGams>(); //for each expression in .expressions: contains the list of variables in the eq        
        
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
    public class ByteArray
    {
        [ProtoMember(1)]
        public byte[] storage = null;
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
