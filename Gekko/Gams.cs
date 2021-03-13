using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using GAMS;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Gekko
{
    public static class GamsModel
    {
        public static void ReadGamsModel(string textInputRaw, string fileName, O.Model o)
        {

            if (G.Equal(Path.GetExtension(fileName), ".csv"))
            {
                new Error("The former .csv reader for GAMS models is obsolete");
                //throw new GekkoException();
            }
            else
            {
                ReadGamsModelNormal(textInputRaw, fileName, o);
            }
        }

        private static void ReadGamsModelNormal(string textInputRaw, string fileName, O.Model o)
        {
            //these objects typically get overridden soon
            Program.model = new Model();
            Program.model.modelGams = new ModelGams();

            Tuple<GekkoDictionary<string, string>, StringBuilder> tup = GetDependentsGams(o);
            GekkoDictionary<string, string> dependents = tup.Item1;
            string dependentsHash = tup.Item2.ToString();
            string modelHash = HandleModelFilesGams(textInputRaw + dependentsHash);

            string mdlFileNameAndPath = Globals.localTempFilesLocation + "\\" + Globals.gekkoVersion + "_" + "gams" + "_" + modelHash + ".mdl";

            if (Program.options.model_cache == true)
            {
                if (File.Exists(mdlFileNameAndPath))
                {
                    try
                    {
                        DateTime dt1 = DateTime.Now;
                        using (FileStream fs = Program.WaitForFileStream(mdlFileNameAndPath, Program.GekkoFileReadOrWrite.Read))
                        {
                            Program.model.modelGams = Serializer.Deserialize<ModelGams>(fs);
                            Program.model.modelGams.modelInfo.loadedFromMdlFile = true;
                        }
                        G.WritelnGray("Loaded known model from cache in: " + G.SecondsFormat((DateTime.Now - dt1).TotalMilliseconds));
                    }
                    catch (Exception e)
                    {
                        if (G.IsUnitTesting())
                        {
                            throw;
                        }
                        else
                        {
                            //do nothing, we then have to parse the file
                            Program.model.modelGams.modelInfo.loadedFromMdlFile = false;
                        }
                    }
                }
            }
            else
            {
                Program.model.modelGams.modelInfo.loadedFromMdlFile = false;
            }

            if (Program.model.modelGams.modelInfo.loadedFromMdlFile)
            {
                //do nothing, also no writing of .mdl file of course
            }
            else
            {

                ReadGamsModelHelper(textInputRaw, fileName, dependents, o);
                if (Globals.runningOnTTComputer) Sniff2();

                DateTime t1 = DateTime.Now;

                try //not the end of world if it fails (should never be done if model is read from zipped protobuffer (would be waste of time))
                {
                    DateTime dt1 = DateTime.Now;

                    //May take a little time to create: so use static serializer if doing serialize on a lot of small objects
                    RuntimeTypeModel serializer = TypeModel.Create();
                    serializer.UseImplicitZeroDefaults = false;  //otherwise an int that has default constructor value -12345 but is set to 0 will reappear as a -12345 (instead of 0). For int, 0 is default, false for bools etc.

                    // ----- SERIALIZE                    
                    string protobufFileName = Globals.gekkoVersion + "_" + "gams" + "_" + modelHash + ".mdl";
                    string pathAndFilename = Globals.localTempFilesLocation + "\\" + protobufFileName;
                    using (FileStream fs = Program.WaitForFileStream(pathAndFilename, Program.GekkoFileReadOrWrite.Write))
                    {
                        serializer.Serialize(fs, Program.model.modelGams);
                    }
                    G.WritelnGray("Created model cache file in " + G.SecondsFormat((DateTime.Now - dt1).TotalMilliseconds));
                }
                catch (Exception e)
                {
                    //do nothing, not the end of the world if it fails
                }
            }

        }

        private static void ReadGamsModelHelper(string textInputRaw, string fileName, GekkoDictionary<string, string> dependents, O.Model o)
        {
            StringBuilder sb1 = new StringBuilder();
            sb1.AppendLine();

            StringBuilder sb2 = new StringBuilder();
            sb2.AppendLine();

            int eqCounter = 0;

            //GAMS comments: star as first char, $ontext/offtext, # as end of line, /* */, 

            //string txt = GetTextFromFileWithWait(Program.options.folder_working + "\\" + "model.gms");
            string txt = textInputRaw;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "!!", "#" };
            var tags3 = new List<Tuple<string, string>>() { new Tuple<string, string>("$ontext", "$offtext") };
            var tags4 = new List<string>() { "*" };

            TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(txt, tags1, tags2, tags3, tags4);
            GekkoDictionary<string, List<ModelGamsEquation>> equationsByVarname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, List<ModelGamsEquation>> equationsByEqname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);

            List<string> problems = new List<string>();

            int counter = 0;

            foreach (TokenHelper tok in tokens2.subnodes.storage)
            {
                if (tok.type == ETokenType.EOL)
                {
                    counter++;
                }

                if (tok.s == "." && tok.Offset(1).s == "." && tok.Offset(1).leftblanks == 0)
                {
                    if (tok.Offset(2).s == "\\" || tok.Offset(2).s == "/")
                    {
                        //this may be part of a path, $GDXIN ..\Data\ADAM\estbk_okt16.gdx
                    }
                    else
                    {
                        eqCounter = ReadGamsEquation(sb1, sb2, eqCounter, equationsByVarname, equationsByEqname, tok, dependents, problems, G.Equal(o.opt_dump, "yes"));
                    }
                }
            }
            Program.model.modelGams = new ModelGams();
            Program.model.modelGams.equationsByVarname = equationsByVarname;
            Program.model.modelGams.equationsByEqname = equationsByEqname;
            Program.model.modelGams.modelInfo = new ModelInfoGams();

            G.Writeln2("MODEL: " + Path.GetFileNameWithoutExtension(fileName));
            G.Writeln("Read " + counter + " lines from " + fileName);
            G.Writeln("Found " + equationsByVarname.Count + " distinct equations (use DISP to display them)");
            if (problems.Count > 0)
            {
                G.Writeln("There were the following problems while reading the model:");
                foreach (string s in problems) G.Writeln("+++ " + s);
            }

            if (G.Equal(o.opt_dump, "yes"))
            {
                using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\dump.gcm", Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(sb1);
                }

                using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\dump.gms", Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(sb2);
                }
            }
        }

        private static int ReadGamsEquation(StringBuilder sb1, StringBuilder sb2, int eqCounter, Dictionary<string, List<ModelGamsEquation>> equationsByVarname, Dictionary<string, List<ModelGamsEquation>> equationsByEqname, TokenHelper tok, GekkoDictionary<string, string> dependents, List<string> problems, bool dump)
        {
            WalkTokensHelper wh = new WalkTokensHelper();

            int iEqStart = 0;
            //searches for '..' with no blank between (could be improved)
            //now we search backwards for start of line
            for (int i2 = -1; i2 > -int.MaxValue; i2--)
            {
                int iLineStart = -12345;
                if (tok.Offset(i2) == null || tok.Offset(i2).type == ETokenType.EOL)
                {
                    iEqStart = i2 + 1;
                    break;
                }
            }

            int i = iEqStart;

            //-----------------------------------------------
            //now we are ready for the equation definition
            //-----------------------------------------------

            //The equation is of this form:

            //e_pi(i,ds,t) $ (tx0(t) and d1i(i,ds,t)) .. pI(i,ds,t)*qI(i,ds,t) =E= vI(i,ds,t);

            //Tokenized in tree structure it looks like this:

            //e_pi(...) $ (...) .. pI(...)*qI(...) =E= vI(...);

            //So the following:
            // eqname
            // maybe a set parenthesis
            // maybe a dollar
            //     if so either a (...) or a variable with a (...)
            // a '..' always
            // a leftside until '=e='
            // a rightside after'=e=' until semicolon

            string eqnameGams = null;
            string dollarGams = null;
            string setsGams = null;
            List<string> setsGamsList = new List<string>();
            string lhsGams = null;
            string rhsGams = null;
            TokenHelper lhsTokensGams = null;
            TokenHelper rhsTokensGams = null;

            string dollar = null;

            eqnameGams = tok.Offset(i)?.s;

            i++;

            //this may be parentheses
            TokenHelper tok2 = tok.Offset(i);
            if (tok2.SubnodesTypeParenthesisStart())
            {
                setsGams = tok2.subnodes.ToString();

                List<TokenHelperComma> split = tok2.SplitCommas(true);
                foreach (TokenHelperComma item in split)
                {
                    string set = item.list.ToString();
                    setsGamsList.Add(set.Trim());
                }

                i++;

                if (tok.Offset(i).s == "$")
                {
                    i++;

                    TokenHelper tok3 = tok.Offset(i);
                    dollarGams = null;
                    if (tok3.subnodes != null)
                    {
                        dollarGams = tok3.subnodes.ToString();
                    }

                    // see also #9872034985732, removing stray " and"
                    if (tok3.SubnodesTypeParenthesisStart())
                    {

                        TokenList list = new TokenList();
                        for (int ii = 0; ii < tok3.subnodes.storage.Count; ii++)
                        {
                            if (ii < tok3.subnodes.Count() - 1 && tok3.subnodes[ii].HasNoChildren() && tok3.subnodes[ii + 1] != null && tok3.subnodes[ii + 1].HasChildren())
                            {
                                //Remove anything that looks like time restriction
                                List<TokenHelperComma> temp = tok3.subnodes[ii + 1].SplitCommas(true);
                                if (temp.Count == 1 && G.Equal(temp[0].list.ToString().Trim(), wh.t))
                                {
                                    ii += 2;
                                    if (G.Equal(tok3.subnodes[ii]?.s, "and"))
                                    {
                                        ii++;  //also check before
                                    }
                                    ii--;  //will get 1 added at loop start
                                    continue;
                                }
                            }
                            list.storage.Add(tok3.subnodes[ii]);
                        }

                        WalkTokensHelper wh2 = new WalkTokensHelper();
                        wh2.checkIfVariableIsASet = true;

                        WalkTokensHandleParentheses(list);
                        WalkTokens(list, wh2);

                        dollar = list.ToStringTrim();

                        if (dollar.StartsWith("(") && dollar.EndsWith(")"))
                        {
                            dollar = dollar.Substring(1, dollar.Length - 2).Trim();
                            if (dollar.StartsWith("and ")) dollar = dollar.Substring("and ".Length).Trim();
                        }

                        i++;
                    }
                    else
                    {
                        string s7 = tok.Offset(i).ToString();
                        if (!G.IsIdent(s7))
                        {
                            new Error("Expected a name instead of '" + s7 + "' , " + tok.Offset(i).LineAndPosText());
                            //throw new GekkoException();
                        }
                        i++;

                        string s8 = tok.Offset(i).ToString();
                        if (!(tok.Offset(i).SubnodesTypeParenthesisStart()))
                        {
                            new Error("Expected a (...) parenthesis instead of '" + s8 + "' , " + tok.Offset(i).LineAndPosText());
                            //throw new GekkoException();
                        }
                        i++;
                    }
                }
            }

            if (tok.Offset(i)?.s == "." && tok.Offset(i + 1)?.s == ".")
            {
                //good, we are at the '..' part, now comes the LHS expression
            }
            else
            {
                new Error("Expected '..' in eq definition, " + tok.Offset(i).LineAndPosText());
                //throw new GekkoException();
            }
            i++;
            i++;

            //now ready for the contents of the equation

            //find lhs of equation -----------------------------------------
            int i1Start = i;

            List<string> eqsign = new List<string>() { "=", "e", "=" };

            int iEqual = tok.Search(i1Start, eqsign);

            if (iEqual == -12345)
            {
                new Error("Could not find '=e=' in eq definition, " + tok.Offset(i).LineAndPosText());
                //throw new GekkoException();
            }

            int i1End = iEqual - 1;
            int i2Start = i1End + eqsign.Count + 1;
            int iSemi = tok.Search(i2Start, new List<string>() { ";" });

            if (iSemi == -12345)
            {
                new Error("Could not find ending ';' in eq definition, " + tok.Offset(i).LineAndPosText());
                //throw new GekkoException();
            }

            int iEqEnd = iSemi;

            lhsGams = tok.OffsetInterval(i1Start, i1End).ToString().Trim();
            lhsTokensGams = tok.OffsetInterval(i1Start, i1End);

            rhsGams = tok.OffsetInterval(i2Start, iSemi - 1).ToString().Trim();
            rhsTokensGams = tok.OffsetInterval(i2Start, iSemi - 1);

            eqCounter++;

            if (false && eqCounter < 10)
            {
                G.Writeln2("Eqname:  " + eqnameGams);
                G.Writeln("Sets:    " + setsGams);
                G.Writeln("Dollar:  " + dollarGams);
                G.Writeln("LHS:     " + lhsGams);
                G.Writeln("RHS:     " + rhsGams);
            }

            ModelGamsEquation equation = new ModelGamsEquation();

            equation.nameGams = eqnameGams;
            equation.setsGams = setsGams;
            equation.setsGamsList = setsGamsList;
            equation.conditionalsGams = dollarGams;
            equation.lhsGams = lhsGams;
            equation.rhsGams = rhsGams;
            equation.lhsTokensGams = lhsTokensGams;
            equation.rhsTokensGams = rhsTokensGams;

            TokenHelper lhsTokensGekko = equation.lhsTokensGams.DeepClone(null);
            TokenHelper rhsTokensGekko = equation.rhsTokensGams.DeepClone(null);

            WalkTokensHandleParentheses(lhsTokensGekko); //changes '[' and '{' into '('
            WalkTokensHandleParentheses(rhsTokensGekko); //changes '[' and '{' into '('

            WalkTokensHelper wt1 = new WalkTokensHelper();
            WalkTokens(lhsTokensGekko, wt1);

            WalkTokensHelper wt2 = new WalkTokensHelper();
            WalkTokens(rhsTokensGekko, wt2);

            //sb.Append(lhsTokensGekko.ToStringTrim() + " = " + rhsTokensGekko.ToStringTrim() + ";" + G.NL);

            string lhs = lhsTokensGekko.ToStringTrim();
            string rhs = rhsTokensGekko.ToStringTrim();

            if (false && (eqCounter > 10 || lhs.Contains("*")))
            {
                //skip
            }
            else
            {
                int v = 3;
                if (v == 1)
                {
                    sb1.Append("PRT " + lhs + ";" + G.NL);
                    sb1.Append("PRT " + rhs + ";" + G.NL);
                    sb1.AppendLine();
                }
                else if (v == 2)
                {
                    sb1.Append("PRT<n> " + lhs + " - ( " + rhs + " );" + G.NL);
                }
                else
                {
                    string dollar2 = null;
                    if (dollar != null && dollar.Trim() != "" && dollar.Trim() != "()")
                    {
                        dollar2 = dollar.Trim();
                    }

                    sb1.AppendLine("Equation: " + eqnameGams);
                    if (dollar2 != null)
                    {
                        sb1.Append("(" + lhs + ") $ (" + dollar2 + ") = " + rhs + ";" + G.NL);  //always add parentheses
                    }
                    else
                    {
                        sb1.Append(lhs + " = " + rhs + ";" + G.NL);
                    }

                    sb2.AppendLine("" + equation.nameGams);
                    sb2.AppendLine("" + equation.setsGams);
                    sb2.AppendLine("" + equation.conditionalsGams);
                    sb2.AppendLine(equation.lhsGams + "  =  ");
                    sb2.AppendLine(equation.rhsGams);
                    sb2.AppendLine();
                    sb2.AppendLine("--------------------------------------");
                    sb2.AppendLine();




                    //if (dollar2 != null)
                    //{
                    //    sb.Append("(" + lhs + ") $ (" + dollar2 + ")" + G.NL);  //always add parentheses
                    //}
                    //else
                    //{
                    //    sb.Append(lhs + " = " + G.NL);
                    //}
                }
            }

            if (true)
            {
                equation.lhs = lhs;
                equation.rhs = rhs;

                // ------------- conditionals ---------------
                // see also #9872034985732

                string conditionals2 = null;
                if (dollar != null) conditionals2 = dollar.Trim();
                if (!G.NullOrEmpty(conditionals2))
                {
                    //removes a stray ending " and" that may be left after removing time conditionals
                    if (conditionals2.EndsWith(" and", StringComparison.OrdinalIgnoreCase)) conditionals2 = conditionals2.Substring(0, conditionals2.Length - " and".Length);
                }
                equation.conditionals = conditionals2;
            }

            bool fromList = false;
            string lhsVariable = ReadGamsModelGetLhsName(equationsByVarname, equationsByEqname, lhsTokensGekko, equation, eqnameGams, dependents, problems, ref fromList);
            string s = null;
            if (fromList) s = ", designated from list";
            if (lhsVariable == null) lhsVariable = "[not identified]";
            sb1.AppendLine("--> " + lhsVariable + " (dependent" + s + ")");
            sb1.AppendLine();
            sb1.AppendLine("----------------------------------------------------------------------------------------------------------------");
            sb1.AppendLine();

            return eqCounter;
        }

        private static string ReadGamsModelGetLhsName(Dictionary<string, List<ModelGamsEquation>> equationsByVarname, Dictionary<string, List<ModelGamsEquation>> equationsByEqname, TokenHelper lhsTokensGams2, ModelGamsEquation e, string eqnameGams, GekkoDictionary<string, string> dependents, List<string> problems, ref bool fromList)
        {

            string lhs = null;

            if (G.Equal(Program.options.model_gams_dep_method, "lhs"))
            {
                Program.GetLhsVariable(lhsTokensGams2, ref lhs);
            }
            else if (G.Equal(Program.options.model_gams_dep_method, "eqname"))
            {
                if (eqnameGams.Contains("__"))
                {
                    new Error("Eqname '" + eqnameGams + "': did not expect '__' substring in name");
                    //throw new GekkoException();
                }
                string[] ss = eqnameGams.Split('_');
                if (ss.Length <= 1)
                {
                    new Error("Eqname '" + eqnameGams + "': did not find any '_' separators");
                    //throw new GekkoException();
                }
                if (!G.Equal(ss[0], "e"))
                {
                    new Error("Eqname '" + eqnameGams + "': expected it to start with 'e_'");
                    //throw new GekkoException();
                }
                if (!G.IsIdent(ss[1]))  //we use the e_{here}_..._..._... part
                {
                    new Error("Eqname '" + eqnameGams + "': could not resolve variable name");
                    //throw new GekkoException();
                }
                lhs = ss[1];
            }
            else
            {
                new Error("option model gams dep method = lhs|eqname.");
                //throw new GekkoException();
            }

            string d = null; if (dependents != null) dependents.TryGetValue(eqnameGams, out d);
            string varnameFound = null;
            if (d != null)
            {
                //found in #dependents
                varnameFound = d;
                fromList = true;
            }

            if (varnameFound == null && lhs != null) varnameFound = lhs;

            if (varnameFound == null)
            {
                problems.Add("Could not find lhs variable in equation '" + eqnameGams + "' (line " + lhsTokensGams2.line + ")");
            }
            else
            {
                if (equationsByVarname.ContainsKey(varnameFound))
                {
                    equationsByVarname[varnameFound].Add(e);  //can have more than one eq with same lhs variable                
                }
                else
                {
                    List<ModelGamsEquation> e2 = new List<ModelGamsEquation>();
                    e2.Add(e);
                    equationsByVarname.Add(varnameFound, e2);
                }

                if (equationsByEqname.ContainsKey(eqnameGams))
                {
                    new Error("The equation name '" + eqnameGams + "' appears multiple times");
                    //throw new GekkoException();
                }
                else
                {
                    List<ModelGamsEquation> e2 = new List<ModelGamsEquation>();
                    e2.Add(e);
                    equationsByEqname.Add(eqnameGams, e2);
                }
            }
            return varnameFound;
        }

        public static ModelGamsEquation DecompEvalGams(string eqname, string varname)
        {
            //find the equation, either looking up eqname or varname

            List<ModelGamsEquation> eqs = null;
            ModelGamsEquation found = null;
            if (eqname != null)
            {
                eqs = GetGamsEquationsByEqname(eqname);
                if (eqs == null || eqs.Count == 0)
                {
                    new Error("Equation '" + eqname + "' was not found");
                    //throw new GekkoException();
                }
                if (eqs.Count > 1)
                {
                    G.Writeln2("*** ERROR: Internal error #809735208375");
                }
                found = eqs[0];  //pick the first one, probably always only one here, cf. #820948324: 
            }
            else
            {
                eqs = GetGamsEquationsByVarname(varname);
                if (eqs == null || eqs.Count == 0)
                {
                    new Error("Variable '" + varname + "' was not found");
                    //throw new GekkoException();
                }
                if (eqs.Count > 1)
                {
                    G.Writeln2("+++ WARNING: Variable '" + varname + "' appears in several equations, first one is picked");
                }
                found = eqs[0];  //#820948324: pick the first one, a variable name may point to several equations, for instance if y is present on the lhs in several equations.
            }

            string rhs = found.rhs.Trim();
            string lhs = found.lhs.Trim();

            string s1 = Program.EquationLhsRhs(lhs, rhs, true) + ";";  //this is a generic method, not just a GAMS method

            if (found.expressions == null || found.expressions.Count == 0)
            {
                Globals.expressions = null;  //maybe not necessary
                Program.CallEval(found.conditionals, s1);
                found.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here, similar to found.expressions = Globals.expressions
                Globals.expressions = null;  //maybe not necessary
            }
            else
            {
                //has already been done
            }

            return found;
        }
        
        private static void Sniff2()
        {
            DateTime dt = DateTime.Now;
            double ms1 = 0;
            double ms2 = 0;
            int n1 = 0;
            int n2 = 0;
            int n3 = 0;

            int counterA = 0;
            int counterError1 = 0;
            int counterError2 = 0;

            foreach (KeyValuePair<string, List<ModelGamsEquation>> kvp in Program.model.modelGams.equationsByEqname)
            {
                //if (counterA > 6) break;
                if (counterA % 50 == 0) G.Writeln2("--> " + counterA);

                counterA++;
                ModelGamsEquation eq = kvp.Value[0];

                eq.expressionVariablesWithSets = new List<EquationVariablesGams>();

                string rhs = eq.rhs.Trim();
                string lhs = eq.lhs.Trim();
                string s1 = Program.EquationLhsRhs(lhs, rhs, true) + ";";

                if (eq.expressions == null || eq.expressions.Count == 0)
                {
                    Globals.expressions = null;  //maybe not necessary

                    try
                    {
                        DateTime dt1 = DateTime.Now;
                        Program.CallEval(eq.conditionals, s1);
                        ms1 += (dt1 - DateTime.Now).TotalMilliseconds;
                        n1++;
                    }
                    catch (Exception e)
                    {
                        counterError1++;
                        if (e.Message.Contains("System.OutOfMemoryException"))
                        {
                            G.Writeln2("+++ ERROR: MEMORY in equation (type 2): " + eq.nameGams);
                        }
                        else
                        {
                            G.Writeln2("+++ ERROR: in equation  (type 2): " + eq.nameGams);
                        }
                        continue;
                    }
                    eq.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here

                    DateTime dt2 = DateTime.Now;
                    foreach (Func<GekkoSmpl, IVariable> expression in eq.expressions)
                    {

                        //Function call start --------------
                        //O.AdjustSmplForDecomp(smpl, 0);
                        //TODO: can be deleted, #p24234oi32      

                        try
                        {
                            string op = "d";
                            GekkoTime per1 = new GekkoTime(EFreq.A, 2020, 1);
                            GekkoTime per2 = new GekkoTime(EFreq.A, 2020, 1);
                            string residualName = "residual___";
                            int funcCounter = 0;
                            DecompData dd = Gekko.Decomp.DecompLowLevel(per1, per2, expression, Gekko.Decomp.DecompBanks(op), residualName, ref funcCounter);

                            List<string> m1 = new List<string>();
                            List<string> m2 = new List<string>();
                            foreach (string s in dd.cellsContribD.storage.Keys)
                            {
                                string ss5 = Program.DecompGetNameFromContrib(s);
                                if (!m1.Contains(ss5, StringComparer.OrdinalIgnoreCase))
                                {
                                    m1.Add(ss5);
                                }
                            }
                            EquationVariablesGams temp = new EquationVariablesGams();
                            temp.equationVariables = m1;
                            eq.expressionVariablesWithSets.Add(temp);
                        }
                        catch (Exception e)
                        {
                            counterError2++;
                            eq.expressionVariablesWithSets.Add(null); //keep alignment                            
                            if (e.Message.Contains("System.OutOfMemoryException"))
                            {
                                G.Writeln2("+++ ERROR: MEMORY in equation: " + eq.nameGams);
                            }
                            else
                            {
                                G.Writeln2("+++ ERROR: in equation: " + eq.nameGams);
                            }
                            break;
                        }
                        n2++;
                    }
                    ms2 += (dt2 - DateTime.Now).TotalMilliseconds;
                    n3++;
                    Globals.expressions = null;  //maybe not necessary
                }
            }
            G.Writeln2("EVAL on " + counterA + " eqs, errors in " + counterError1 + "/" + counterError2 + " of these, " + (dt - DateTime.Now).TotalMilliseconds / 1000d + " " + (-ms1 / 1000d) + " " + (-ms2 / 1000d));
            G.Writeln2("n1 " + n1 + " n2 " + n2 + " n3 " + n3);
        }

        private static List<ModelGamsEquation> GetGamsEquationsByEqname(string variable)
        {
            if (Program.model.modelGams.equationsByEqname == null || Program.model.modelGams.equationsByEqname.Count == 0)
            {
                new Error("No GAMS equations found");
                //throw new GekkoException();
            }
            List<ModelGamsEquation> eqs = null; Program.model.modelGams.equationsByEqname.TryGetValue(variable, out eqs);
            return eqs;
        }

        public static List<ModelGamsEquation> GetGamsEquationsByVarname(string variable)
        {
            if (Program.model.modelGams.equationsByVarname == null || Program.model.modelGams.equationsByVarname.Count == 0)
            {
                new Error("No GAMS equations found");
                //throw new GekkoException();
            }
            List<ModelGamsEquation> eqs = null; Program.model.modelGams.equationsByVarname.TryGetValue(variable, out eqs);
            return eqs;
        }
        
        private static Tuple<GekkoDictionary<string, string>, StringBuilder> GetDependentsGams(O.Model o)
        {
            GekkoDictionary<string, string> dependents = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            //hashHelper: will get the format: "--- dependents ---<NL>a;b;c<NL>c,d,e<NL>"
            //the dependents list does not change the model per se, but it changes how DISP and other commands
            //like DECOMP show stuff.
            StringBuilder hashHelper = new StringBuilder();
            hashHelper.AppendLine();
            hashHelper.AppendLine("--- dependents ---");

            IVariable lhsList = o.opt_dep;
            if (lhsList != null)
            {
                List lhsList_list = lhsList as List;
                if (lhsList_list == null)
                {
                    new Error("Variable #dependents should be of list type");
                    //throw new GekkoException();
                }
                int c = 0;
                foreach (IVariable x in lhsList_list.list)
                {
                    c++;
                    if (x.Type() != EVariableType.List)
                    {
                        new Error("#dependents sublist line " + c + ": should be of list type");
                        //throw new GekkoException();
                    }
                    List x_list = x as List;

                    List<string> ss = null;

                    try
                    {
                        ss = Stringlist.GetListOfStringsFromList(x_list);
                    }
                    catch
                    {
                        new Error("#dependents sublist line " + c + ": all elements should be strings");
                        throw;
                    }

                    foreach (string s in ss)
                    {
                        hashHelper.Append(s.ToLower()).Append(";");
                    }
                    hashHelper.AppendLine();

                    if (ss.Count < 2)
                    {
                        new Error("#dependents sublist line " + c + ": must have > 1 elements");
                        //throw new GekkoException();
                    }
                    string lhs = ss[0];
                    for (int i = 1; i < ss.Count; i++)
                    {
                        //The ss list has this form for each line:
                        //qG; E_qG; E_qG_tot    --> first the lhs name, then the equations where it is a lhs variable
                        //Since each equation can only have 1 lhs, the eqnames (E_qG etc.) can at most appear 1 time in
                        //the ss list.

                        string temp = null; dependents.TryGetValue(ss[i], out temp);
                        if (temp != null)
                        {
                            new Error("#dependents sublist line " + c + ": The equation '" + ss[i] + "' already assigns '" + temp + "' as lhs");
                            //throw new GekkoException();
                        }
                        dependents.Add(ss[i], lhs);
                    }
                }
            }

            return new Tuple<GekkoDictionary<string, string>, StringBuilder>(dependents, hashHelper);
        }

        private static bool CheckIfVarIsASet(string name, WalkTokensHelper th)
        {
            bool isSetWithIndexer = false;
            if (th.checkIfVariableIsASet)
            {

                IVariable iv = Program.databanks.GetFirst().GetIVariable("#" + name);
                if (iv != null && iv.Type() == EVariableType.List)
                {
                    isSetWithIndexer = true;
                }
            }
            return isSetWithIndexer;
        }

        public static void WalkTokens(TokenList nodes, WalkTokensHelper th)
        {
            foreach (TokenHelper child in nodes.storage)
            {
                WalkTokens(child, th);
            }
        }

        public static void WalkTokens(TokenHelper node, WalkTokensHelper th)
        {
            //Performs these transformations:
            //- GAMS functions are not touched (log, etc)
            //-      but sqr() becomes sqrt()
            //- sum() function has # put in on sets
            //- parameter t is removed, and lags/leads like t-1 are transformed into [-1] etc. So x(a, t) --> x[#a], and x(t) --> x not x().
            //- tBase handled
            //- strings have quotes removed, x['a'] --> x[a]
            //- stuff like a.val becomes #a.val(), whereas t.val is ignored for now
            //- sameas(i,j) and sameas(i,'a') become #i==#j and #i=='a'
            //- single '=' becomes '=='
            //
            //- all t or t+1 or t-1 etc. are recorded, together with any tBase

            if (node.HasNoChildren())
            {
                //not a sub-node
                if (node.s != "" && node.type == ETokenType.Word)
                {
                    //an IDENT-type leaf node, not symbols etc.
                    //patterns like "log(" or "exp(" or "sum(" are skipped, also stuff like "*(" is avoided

                    string word = node.s;

                    TokenHelper nextNode = node.Offset(1);
                    if (nextNode != null && nextNode.HasChildren() && nextNode.SubnodesType() == "(" && nextNode.subnodes[0].leftblanks == 0)
                    {
                        //a pattern like "x(" with no blanks in between                    

                        if (G.Equal(node.s, "sameas"))
                        {
                            List<TokenHelperComma> split = nextNode.SplitCommas(false);
                            if (split.Count != 2)
                            {
                                new Error("Expected sameas() function with 2 arguments");
                                //throw new GekkoException();
                            }

                            node.s = "";
                            split[1].comma.s = "==";


                        }
                        else if (Globals.gamsFunctions.ContainsKey(node.s))
                        {
                            string x = Globals.gamsFunctions[node.s];
                            if (x != null)
                            {
                                node.s = x;  //sqr() --> sqrt()
                            }

                            //"sum(" or "log(" or "exp(" etc.
                            if (G.Equal(node.s, "sum"))
                            {
                                if (nextNode.subnodes.Count() > 0)
                                {
                                    if (nextNode.subnodes[1].HasNoChildren())
                                    {
                                        //stuff like "sum(i, x(i))"
                                        if (nextNode.subnodes[1].type == ETokenType.Word && (G.Equal(nextNode.subnodes[2].s, ",") || G.Equal(nextNode.subnodes[2].s, "$")))
                                        {
                                            //checks that it has "sum(x," or sum(x$" pattern
                                            nextNode.subnodes[1].s = "#" + nextNode.subnodes[1].s;
                                        }
                                    }
                                    else
                                    {
                                        //stuff like "sum((i, j), x(i, j))"
                                        List<TokenHelperComma> list2 = nextNode.subnodes[1].SplitCommas(true);
                                        foreach (TokenHelperComma item in list2)
                                        {
                                            //TODO CHECK
                                            //TODO CHECK
                                            //TODO CHECK
                                            //TODO CHECK
                                            //TODO CHECK
                                            //TODO CHECK
                                            //TODO CHECK
                                            //TODO CHECK
                                            //TODO CHECK
                                            //TODO CHECK
                                            if (item.list.Count() == 1 && item.list[0].type == ETokenType.Word)
                                            {
                                                item.list[0].s = "#" + item.list[0].s;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //a "sum()" --> not handled
                                }
                            }
                        }
                        else
                        {
                            //first we check for stuff like a15t100(a), where a15t100 is a set, not a variable
                            //so it should be #a15t100[#a], not a15t100[#a]

                            bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                            if (isSetWithIndexer) node.s = "#" + node.s;

                            bool removeParenthesis = false;
                            if (true)
                            {
                                //now we look at the arguments, x(a1, a2, 's', t) or x(a1, a2, 's', t-1) or x(a1, a2, 's')
                                List<TokenHelperComma> split = nextNode.SplitCommas(true);

                                for (int iSplit = 0; iSplit < split.Count; iSplit++)
                                {
                                    TokenHelperComma helper = split[iSplit];
                                    if (helper.list.storage.Count == 0)
                                    {
                                        //empty parenthesis, how is that possible?
                                    }
                                    else if (helper.list.storage.Count == 1)
                                    {
                                        //a single token in the slot , .... , so this is not an expression like t+1 etc.

                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //helper.list[0] is the single token

                                            if (iSplit == split.Count - 1 && (G.Equal(helper.list[0].s, th.t) || G.Equal(helper.list[0].s, th.tBase)))
                                            {
                                                //t or tBase at last position

                                                if (G.Equal(helper.list[0].s, th.t))
                                                {
                                                    //normal t
                                                    //remove the trailing t
                                                    helper.list[0].Clear();
                                                    if (helper.comma == null)
                                                    {
                                                        removeParenthesis = true;  //t is the only argument as in "x(t)" which becomes "x" not "x()"
                                                    }
                                                    else
                                                    {
                                                        helper.comma.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    //tBase
                                                    //x(i, tBase) --> x[#i][%tBase]                                                
                                                    //we need to transform one []-subnode into two consequtive
                                                    //see also #89075203489

                                                    TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                    //[%tBase]
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper(Globals.symbolScalar + helper.list[0].s));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                    TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                    if (split.Count > 1)
                                                    {
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                        for (int iii = 0; iii < split.Count - 1; iii++)
                                                        {
                                                            if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                            nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                        }
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("]"));
                                                    }
                                                    else
                                                    {
                                                        //x(i, tBase) --> x[#i][%tBase], but x(tBase) --> x[%tBase]
                                                    }

                                                    int id = nextNode.id;
                                                    TokenHelper parent = nextNode.parent;

                                                    parent.subnodes.storage.RemoveAt(id);
                                                    parent.subnodes.storage.Insert(id, nextNode2);
                                                    parent.subnodes.storage.Insert(id, nextNode1);
                                                    parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok                                                   

                                                }
                                            }
                                            else
                                            {
                                                //x(i) --> x(#i) --actually--> x[#i]
                                                helper.list[0].s = "#" + helper.list[0].s;
                                            }
                                        }
                                        else if (helper.list[0].type == ETokenType.QuotedString)
                                        {
                                            //remove the quotes
                                            helper.list[0].s = G.StripQuotes(helper.list[0].s);
                                        }
                                    }
                                    else if (helper.list.storage.Count == 3)  //x and plusminus and number
                                    {

                                        //the ... argument in (... , ... , ... , ...) is an expression, for instance t-1 etc.
                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //if (iSplit == split.Count - 1 && helper.list[0].s == "t")                                        
                                            if (true)
                                            {
                                                //does not need to be last. Can be "t" in "x(a, 'b', t-1)", but also "a" in "x(y, a-1, t)"
                                                if (helper.list[1] != null && (helper.list[1].s == "-" || helper.list[1].s == "+"))
                                                {
                                                    //...t+... or ...t-...
                                                    if (helper.list[2] != null && (helper.list[2].type == ETokenType.Number))
                                                    {
                                                        string plusMinus = helper.list[1].s;
                                                        if (plusMinus != "+" && plusMinus != "-")
                                                        {
                                                            new Error("Expected t plus/minus an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        string number = helper.list[2].s;
                                                        int iNumber = -12345;
                                                        bool ok = int.TryParse(number, out iNumber);
                                                        if (!ok)
                                                        {
                                                            new Error("Expected '" + number + "' to be an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        //if (plusMinus == "-") iNumber = -iNumber;

                                                        if (iSplit == split.Count - 1 && G.Equal(helper.list[0].s, th.t))
                                                        {
                                                            if (iSplit == 0)
                                                            {
                                                                //x(t-1) --> x[-1]
                                                                //helper.comma will be = null
                                                                helper.list[0].Clear(); //kill the 't'completely including blanks
                                                                helper.list[1].leftblanks = 0; //no blanks to the left of for instance '-1'
                                                            }
                                                            else
                                                            {
                                                                //x(i, t-1) --> x[#i][-1]
                                                                //we need to transform one []-subnode into two consequtive
                                                                //see also #89075203489
                                                                TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 1; iii < helper.list.storage.Count; iii++)
                                                                {
                                                                    nextNode2.subnodes.storage.Add(helper.list[iii]);
                                                                }
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                                TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 0; iii < split.Count - 1; iii++)
                                                                {
                                                                    if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                                    nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                                }
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("]"));

                                                                int id = nextNode.id;
                                                                TokenHelper parent = nextNode.parent;

                                                                parent.subnodes.storage.RemoveAt(id);
                                                                parent.subnodes.storage.Insert(id, nextNode2);
                                                                parent.subnodes.storage.Insert(id, nextNode1);
                                                                parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                                                            }
                                                        }
                                                        else
                                                        {
                                                            helper.list[0].s = "#" + helper.list[0].s;
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (removeParenthesis)
                            {
                                nextNode.subnodes[0].Clear();
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].Clear();
                            }
                            else
                            {
                                nextNode.subnodes[0].s = "[";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].s = "]";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].leftblanks = 0; //we do not want x[#i, #j ], x[#i, #j] is nicer.
                            }
                        }
                    }
                    else
                    {

                        //could be a standalone a here: ... $ (sameas(a, '15'))
                        bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                        if (isSetWithIndexer) node.s = "#" + node.s;

                        TokenHelper nextNode1 = node.Offset(1);
                        TokenHelper nextNode2 = node.Offset(2);

                        if (nextNode1 != null && nextNode2 != null)
                        {

                            if (nextNode1.s == "." && G.Equal(nextNode2.s, "val"))
                            {
                                //a pattern like a.val or t.val, used in for instance a.val > 15 etc.
                                //now we transform a.val into #a.val().
                                //it must use val(), since the #a elements are strings.
                                //the fact that x[#a+1] works is a special exception.
                                //node.s = "#" + node.s;
                                nextNode2.s = nextNode2.s + "()";
                            }
                        }
                    }
                }
                else if (node.s == "=")
                {
                    TokenHelper prevNode1 = node.Offset(-1);
                    if (prevNode1 != null && (prevNode1.s == "<" || prevNode1.s == ">"))
                    {
                        //do nothing, we do not want <= to become <== !
                    }
                    else
                    {
                        node.s = "==";  //stuff like ... $ (a.val = 15) 
                    }
                }
                else if (node.s == "$")
                {
                    TokenHelper nextNode = node.Offset(1);  //b
                    TokenHelper nextNode2 = node.Offset(2);  //(i, j)
                    //We look for the pattern "a $ b(i, j)", where Gekko does not allow simply a $ b[#i, #j], but must use a $ (b[#i, #j])
                    if (nextNode != null && nextNode.s != "" && nextNode.type == ETokenType.Word)
                    {
                        if (nextNode2 != null && nextNode2.HasChildren() && nextNode2.SubnodesType() == "(" && nextNode2.subnodes[0].leftblanks == 0)
                        {
                            int id = nextNode.id;
                            TokenHelper parent = nextNode.parent;
                            TokenHelper newNode = new TokenHelper(); newNode.subnodes = new TokenList();
                            newNode.subnodes.storage.Add(new TokenHelper("("));
                            newNode.subnodes.storage.Add(nextNode);
                            newNode.subnodes.storage.Add(nextNode2);
                            newNode.subnodes.storage.Add(new TokenHelper(")"));
                            parent.subnodes.storage.RemoveAt(id);
                            parent.subnodes.storage.Insert(id, newNode);
                            parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                        }
                    }
                }
            }
            else
            {
                //an empty node with children

                //foreach (TokenHelper child in node.subnodes.storage)
                //{
                //    WalkTokens(child);
                //}

                for (int i = 0; i < node.subnodes.storage.Count; i++)  //the count may increase, because subnodes may be added dynamically (translating x[i, t-1] into x[#i][-1])
                {
                    WalkTokens(node.subnodes.storage[i], th);
                }
            }
        }

        public static void WalkTokensHandleParentheses(TokenList nodes)
        {
            foreach (TokenHelper child in nodes.storage)
            {
                WalkTokensHandleParentheses(child);
            }
        }


        public static void WalkTokensHandleParentheses(TokenHelper node)
        {
            //All [, {, } and ] are changed into soft parentheses ( )
            if (node.HasNoChildren())
            {
                //not a sub-node
                if (node.s == "[") node.s = "(";
                else if (node.s == "{") node.s = "(";
                else if (node.s == "]") node.s = ")";
                else if (node.s == "}") node.s = ")";
                return;
            }
            else
            {
                //an empty node with children

                foreach (TokenHelper child in node.subnodes.storage)
                {
                    WalkTokensHandleParentheses(child);
                }
            }
        }


        public static string HandleModelFilesGams(string input)
        {
            List<string> lines = G.ExtractLinesFromText(input);
            return GetModelHashGams(lines);
        }

        private static string GetModelHashGams(List<string> lines)
        {
            string trueHash = Program.GetMD5Hash(G.ExtractTextFromLines(lines).ToString()); //Pretty unlikely that two different gams files could produce the same hash.
            trueHash = trueHash.Trim();  //probably not necessary
            return trueHash;
        }
    }

    public static class GamsData
    {
        public static void ReadGdx(Databank databank, Program.ReadInfo readInfo, string fileLocal)
        {
            //merge and date truncation:
            //do this by first reading into a Gekko databank, and then merge that with the merge facilities from gbk read

            // ---------------------------------------
            // gdx              no t               has t
            // dims
            // ---------------------------------------
            // 0                normal timeless    NA
            //
            //
            // 1                gdim = 1           normal series
            //                  timeless
            //
            // 2                gdim = 2           gdim = 1
            //                  timeless
            //
            //3                 gdim = 3           gdim = 2
            //                  timeless

            // gdxdim = gdim + (1-istimeless)

            // only complication is that Gekko may mix timeless and non-timeless
            // subseries, maybe that should not be allowed?
            // maybe the array-superseries should know if it is timeless or not?

            string prefix = Program.options.gams_time_prefix.Trim().ToLower();
            bool hasPrefix = prefix.Length > 0;
            string file = G.AddExtension(fileLocal, "." + "gdx");
            int offset = (int)Program.options.gams_time_offset;
            DateTime dt1 = DateTime.Now;
            int skippedSets = 0;
            int importedSets = 0;
            int counterVariables = 0;
            int counterParameters = 0;
            int yearMax = int.MinValue;
            int yearMin = int.MaxValue;

            int counterFixed = 0;

            EFreq freq = EFreq.A;
            if (G.Equal(Program.options.gams_time_freq, "u")) freq = EFreq.U;
            else if (G.Equal(Program.options.gams_time_freq, "q")) freq = EFreq.Q;
            else if (G.Equal(Program.options.gams_time_freq, "m")) freq = EFreq.M;

            string gamsDir = null; GAMSWorkspace ws = null;
            GetGAMSWorkspace(ref gamsDir, ref ws);

            if (Program.options.gams_fast)
            {
                ReadGdxFast(databank, prefix, hasPrefix, file, offset, ref skippedSets, ref importedSets, ref counterVariables, ref counterParameters, ref yearMax, ref yearMin, freq, ref gamsDir);
            }
            else
            {
                new Error("The slow gdx reader is not maintained, try the faster GDX reader with: OPTION gams fast = yes;");
            }
            G.Writeln2("Finished GAMS import of " + counterVariables + " variables, " + counterParameters + " parameters and " + importedSets + " sets (" + G.Seconds(dt1) + ")");
            if (skippedSets > 0) G.Writeln("+++ NOTE: " + skippedSets + " sets with dim > 1 were not imported");

            readInfo.startPerInFile = yearMin;
            readInfo.endPerInFile = yearMax;
            readInfo.nanCounter = 0;

            readInfo.variables = counterVariables + counterParameters;
            readInfo.time = (DateTime.Now - dt1).TotalMilliseconds;

            readInfo.startPerResultingBank = readInfo.startPerInFile;
            readInfo.endPerResultingBank = readInfo.endPerInFile;

            databank.FileNameWithPath = readInfo.fileName;

            //TODO: Maybe only do this on the gdx variables if possible
            //Anyway, the speed penalty is small anyway.
            databank.Trim();

            //if (Globals.runningOnTTComputer) G.Writeln2("FIXED::: " + counterFixed);

        }

        private static void ReadGdxFast(Databank databank, string prefix, bool hasPrefix, string file, int offset, ref int skippedSets, ref int importedSets, ref int counterVariables, ref int counterParameters, ref int yearMax, ref int yearMin, EFreq freq, ref string gamsDir)
        {
            if (Program.options.gams_time_detect_auto)
            {
                G.Writeln2("+++ NOTE: 'OPTION gams time detect_auto = yes' ignored in 'OPTION gams fast = yes' mode");
            }
            try
            {
                string msg = string.Empty;
                string producer = string.Empty;
                int errNr = 0;
                int rc;
                int[] index = new int[gamsglobals.maxdim];
                string[] indexString = new string[gamsglobals.maxdim];
                double[] values = new double[gamsglobals.val_max];
                int[] domainSyNrs = new int[gamsglobals.maxdim];
                string[] domainStrings = new string[gamsglobals.maxdim];
                int varNr = 0;
                int nrRecs = 0;
                int n = 0;
                int gdxDimensions = 0;
                string varName = string.Empty;
                int varType = 0;
                int d;
                if (gamsDir == null) gamsDir = "";

                GdxFast gdx = new GdxFast(gamsDir, ref msg);  //it seems ok if gamsSysDir = "", then it will autolocate it (but there may be a 64-bit problem...)
                if (msg != string.Empty)
                {
                    G.Writeln("*** ERROR: Could not load GDX library");
                    G.Writeln("*** ERROR: " + msg);
                    GdxErrorMessage();
                    throw new GekkoException();
                }
                if (true)
                {
                    rc = gdx.gdxOpenRead(file, ref errNr);
                    if (errNr != 0)
                    {
                        {
                            new Error("gdx io error");
                            //throw new GekkoException();
                        }
                    }
                    int timeIndex = -12345;
                    int uelCount = -1; int uelHighest = -1;
                    gdx.gdxUMUelInfo(ref uelCount, ref uelHighest);
                    if (uelHighest != 0)
                    {
                        new Error("Internal UEL problem (GDX)");
                        //throw new GekkoException();
                    }
                    string[] uel = new string[uelCount + 1];
                    for (int u = 1; u <= uelCount; u++)
                    {
                        string s = null;
                        int error = -1;
                        int error2 = gdx.gdxUMUelGet(u, ref s, ref error);
                        uel[u] = s;  //remember that uel[0] is empty and not meaningful                     
                    }

                    timeIndex = -12345; gdx.gdxFindSymbol(Program.options.gams_time_set, ref timeIndex);
                    if (timeIndex == 0 || Program.options.gams_time_set == "")
                    {
                        new Error("Could not find the time set ('" + Program.options.gams_time_set + "')");
                        //throw new GekkoException();
                    }

                    //varType = 0: SET
                    //varType = 1: PARAM
                    //varType = 2: VARIABLE
                    //varType = 3: EQU
                    //varType = 4: ALIAS
                    for (int i = 1; i < int.MaxValue; i++)
                    {

                        gdx.gdxSymbolInfo(i, ref varName, ref gdxDimensions, ref varType);

                        string label = null; int records = -12345; int userInfo = -12345;
                        gdx.gdxSymbolInfoX(i, ref records, ref userInfo, ref label);

                        if (gdxDimensions == -1)
                        {
                            break;  //no more symbols
                        }
                        if (varType == 0 || varType == 4)
                        {
                            //
                            //  ======================================
                            //              sets
                            //  ======================================
                            //                  

                            if (gdxDimensions != 1)
                            {
                                skippedSets++;
                                continue;
                            }
                            List<string> setData = new List<string>();  //contains names of sets (entryNr --> symbolName)
                            if (gdx.gdxDataReadRawStart(i, ref nrRecs) == 0)
                            {
                                new Error("gdx error");
                                //throw new GekkoException();
                            }
                            while (gdx.gdxDataReadRaw(ref index, ref values, ref n) != 0)
                            {
                                string s = null;
                                s = uel[index[0]];
                                setData.Add(s);

                            }
                            gdx.gdxDataReadDone();

                            //add the list to databank
                            string name = Globals.symbolCollection + varName;
                            if (databank.ContainsIVariable(name))
                            {
                                databank.RemoveIVariable(name);
                            }
                            List ml = new List(setData);
                            databank.AddIVariable(name, ml);

                            importedSets++;
                        }
                        //else if (varType == 3) //equ
                        //{
                        //    if (gdx.gdxDataReadRawStart(i, ref nrRecs) == 0)
                        //    {
                        //        G.Writeln2("*** ERROR: gdx error");
                        //        throw new GekkoException();
                        //    }
                        //    while (gdx.gdxDataReadRaw(ref index, ref values, ref n) != 0)
                        //    {
                        //        string s = null;

                        //    }
                        //    gdx.gdxDataReadDone();
                        //}
                        else if (varType == 1 || varType == 2) //parameter or variable
                        {
                            //
                            //  ======================================
                            //       parameters (1) and variables (2)
                            //  ======================================
                            //

                            string varNameWithFreq = varName + Globals.freqIndicator + G.ConvertFreq(freq);

                            //if (varName.ToLower().Contains("d10"))
                            //{

                            //}

                            //always fetched, since we use it for domains
                            gdx.gdxSymbolGetDomainX(i, ref domainStrings);
                            int timeDimNr = GdxGetTimeDimNumber(ref domainSyNrs, domainStrings, gdxDimensions, gdx, timeIndex, i);

                            if (gdx.gdxDataReadRawStart(i, ref nrRecs) == 0)
                            {
                                new Error("gdx error");
                                //throw new GekkoException();
                            }

                            int hasTimeDimension = 0;
                            if (timeDimNr != -12345) hasTimeDimension = 1;

                            int gekkoDimensions = gdxDimensions - hasTimeDimension;

                            bool isMultiDim = true;
                            if (gekkoDimensions == 0)
                            {
                                isMultiDim = false;
                            }

                            Series ts = null;
                            if (isMultiDim)
                            {
                                //Multi-dim timeseries
                                string[] domains = new string[gekkoDimensions];
                                int counter = 0;
                                for (d = 0; d < gdxDimensions; d++)
                                {
                                    if (d == timeDimNr) continue; //skipping time dimension
                                    if (domainStrings[counter] == "*") domains[counter] = domainStrings[d];
                                    else domains[counter] = Globals.symbolCollection + domainStrings[d];
                                    counter++;
                                }
                                if (databank.ContainsIVariable(varNameWithFreq)) databank.RemoveIVariable(varNameWithFreq);  //should not be possible, since merging is not allowed...
                                ts = new Series(freq, varNameWithFreq);
                                ts.meta.label = label;
                                ts.meta.domains = domains;
                                if (hasTimeDimension == 0) ts.type = ESeriesType.Timeless;
                                ts.SetArrayTimeseries(gdxDimensions, hasTimeDimension == 1);
                                if (varType == 1) ts.meta.fix = EFixedType.Parameter;
                                databank.AddIVariable(ts.name, ts);
                            }
                            else
                            {
                                //Zero-dimensional timeseries (that is, normal timeseries)                                    
                                //A zero-dim timeseries in the Gekko sense can be timeless (scalar) or non-timeless (normal timeseries)
                                //in this case, we just construct a normal timeseries
                                if (databank.ContainsIVariable(varNameWithFreq)) databank.RemoveIVariable(varNameWithFreq);  //should not be possible, since merging is not allowed...
                                ts = new Series(freq, varNameWithFreq);
                                ts.meta.label = label;
                                if (hasTimeDimension == 0) ts.type = ESeriesType.Timeless;
                                if (varType == 1) ts.meta.fix = EFixedType.Parameter;
                                databank.AddIVariable(ts.name, ts);
                            }

                            if (varType == 1)
                            {
                                counterParameters++;
                            }
                            if (varType == 2)
                            {
                                counterVariables++;
                            }

                            List<string> oldDims = new List<string>() { "     " }; //will not match anything

                            Series ts2 = null;  //the subseries in one of the dimension coordinates

                            while (gdx.gdxDataReadRaw(ref index, ref values, ref n) != 0)
                            {
                                //Reading the dimension coordinates

                                int tt = -12345;
                                //StringBuilder sb = new StringBuilder();
                                List<string> dims = new List<string>();
                                for (d = 0; d < gdxDimensions; d++)
                                {
                                    if (d == timeDimNr)
                                    {
                                        //FIXME
                                        //FIXME
                                        //FIXME
                                        //FIXME pre-construct an uel_time with uel --> GekkoTime.
                                        //FIXME if there is a prefix and offset, handle that too!
                                        //FIXME
                                        //FIXME

                                        string timeElement = uel[index[d]];
                                        if (hasPrefix)
                                        {
                                            if (!timeElement.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                                            {
                                                new Error("GAMS variable/parameter " + varName + " has element '" + timeElement + "' in the time dimension (" + Program.options.gams_time_set + ")");
                                                G.Writeln("    The time elements are expected to start with '" + prefix + "'", Color.Red);
                                                G.Writeln("    See 'OPTION gams time set' and 'OPTION gams time prefix", Color.Red);
                                                throw new GekkoException();
                                            }
                                            timeElement = timeElement.Substring(prefix.Length);
                                        }

                                        tt = G.IntParse(timeElement);
                                        if (tt == -12345)
                                        {
                                            string txt = null;
                                            if (hasPrefix)
                                            {
                                                txt = ". Original time element name: '" + uel[index[d]] + "'";
                                            }
                                            new Error("Could not convert '" + timeElement + "' into an annual time period" + txt);
                                        }
                                        tt = tt + offset;
                                        continue;  //do not add it to the dims
                                    }
                                    string s = uel[index[d]];

                                    dims.Add(s);
                                }

                                bool equal = CompareDims(oldDims, dims);

                                if (equal)
                                {
                                    //keep the same ts2
                                    //if time is the last dimension, the hash is the same for all periods
                                    //this avoids getting the same Gekko variable over and over
                                }
                                else
                                {
                                    //create it
                                    if (isMultiDim)
                                    {
                                        MultidimItem mmi = new MultidimItem(dims.ToArray(), ts);
                                        IVariable iv = null; ts.dimensionsStorage.TryGetValue(mmi, out iv); //probably never present, if merging is not allowed
                                        if (iv == null)
                                        {
                                            ts2 = new Series(ESeriesType.Normal, freq, Globals.seriesArraySubName + Globals.freqIndicator + G.ConvertFreq(freq));
                                            if (timeDimNr == -12345) ts2.type = ESeriesType.Timeless;
                                            ts.dimensionsStorage.AddIVariableWithOverwrite(mmi, ts2);
                                        }
                                        else
                                        {
                                            ts2 = iv as Series;
                                        }
                                    }
                                    else
                                    {
                                        //zero-dimensional series
                                        ts2 = ts;  //just use that for this purpose
                                    }
                                }

                                double value = values[gamsglobals.val_level];

                                if (value == Globals.gamsEps)
                                {
                                    value = 0d;  //infinitely small value, in Gekko it is a real zero
                                }
                                else if (value == Globals.gamsNegInf)
                                {
                                    value = double.NegativeInfinity;
                                }
                                else if (value == Globals.gamsPosInf)
                                {
                                    value = double.PositiveInfinity;
                                }
                                else if (value == Globals.gamsNA)
                                {
                                    value = double.NaN;
                                }
                                else if (value == Globals.gamsUndf)
                                {
                                    value = double.NaN;
                                }

                                if (tt == -12345)
                                {
                                    ts2.SetTimelessData(value);
                                    if (GamsIsFixed(values, value))
                                    {
                                        ts2.meta.fix = EFixedType.Timeless;
                                    }
                                }
                                else
                                {
                                    //TODO
                                    //TODO
                                    //TODO record data in an array, and use setDataSequence().
                                    //TODO
                                    //TODO

                                    GekkoTime gt = new GekkoTime(freq, tt, 1);
                                    ts2.SetData(gt, value);
                                    yearMax = Math.Max(tt, yearMax);
                                    yearMin = Math.Min(tt, yearMin);

                                    if (varType == 2 && GamsIsFixed(values, value))  //not for varType == 1 (parameter)
                                    {
                                        ts2.meta.fix = EFixedType.Normal;  //will overwrite a lot, but never mind it is fast
                                        if (ts2.meta.fixedNormal == null) ts2.meta.fixedNormal = new GekkoTimeSpans();
                                        if (ts2.meta.fixedNormal.data.Count == 0)
                                        {
                                            //the very first
                                            ts2.meta.fixedNormal.data.Add(new GekkoTimeSpan(gt, gt));
                                        }
                                        else
                                        {
                                            GekkoTimeSpan gts = ts2.meta.fixedNormal.data[ts2.meta.fixedNormal.data.Count - 1];
                                            if (gts.tEnd.IsSamePeriod(gt.Add(-1)))
                                            {
                                                gts.tEnd = gt;
                                            }
                                            else
                                            {
                                                ts2.meta.fixedNormal.data.Add(new GekkoTimeSpan(gt, gt));
                                            }
                                        }
                                    }
                                }

                                oldDims = dims; //ok to point, dims will be created from scratch at beginning of loop                                 
                            }  //end of records/dimensions for the variable or parameter

                            gdx.gdxDataReadDone();

                        }
                        else
                        {
                            //do nothing, skip this symbol
                        }
                    }
                }
                errNr = gdx.gdxClose();
                if (errNr != 0)
                {
                    new Error("gdx io error");
                    //throw new GekkoException();
                }
            }
            catch (Exception e)
            {
                G.Writeln2("*** ERROR: GDX import failed with an unexpected error.");
                throw;
            }
        }

        public static void WriteGdx(Databank databank, GekkoTime t1, GekkoTime t2, string pathAndFilename, List<ToFrom> list)
        {
            //merge and date truncation:
            //do this by first reading into a Gekko databank, and then merge that with the merge facilities from gbk read

            DateTime t = DateTime.Now;
            double[] gdxValues = G.CreateArrayDouble(gamsglobals.val_max, 0d);
            gdxValues[gamsglobals.val_scale] = 1d;

            string prefix = Program.options.gams_time_prefix.Trim().ToLower();
            bool hasPrefix = prefix.Length > 0;
            //string file = AddExtension(file2, "." + "gdx");
            int offset = (int)Program.options.gams_time_offset;
            DateTime dt1 = DateTime.Now;
            int skippedSets = 0;
            int exportedSets = 0;
            int counterVariables = 0;
            int counterParameters = 0;
            int yearMax = int.MinValue;
            int yearMin = int.MaxValue;

            string gamsDir = null; GAMSWorkspace ws = null;
            GetGAMSWorkspace(ref gamsDir, ref ws);

            EFreq freq = EFreq.A;
            if (G.Equal(Program.options.gams_time_freq, "u")) freq = EFreq.U;
            else if (G.Equal(Program.options.gams_time_freq, "q")) freq = EFreq.Q;
            else if (G.Equal(Program.options.gams_time_freq, "m")) freq = EFreq.M;

            double[] d = new double[1];  //used for sets

            int syCnt = 0, uelCnt = 0;

            //GAMSWorkspace ws = null;

            if (true)
            {
                string Msg = string.Empty;

                string Sysdir;
                string Producer = string.Empty;
                int ErrNr = 0;
                int rc;
                string[] Indx = new string[gamsglobals.maxdim];
                double[] Values = new double[gamsglobals.val_max];
                int VarNr = 0;
                int NrRecs = 0;
                int N = 0;
                int Dimen = 0;
                string VarName = string.Empty;
                int VarTyp = 0;
                int D;

                GdxFast gdx = new GdxFast(gamsDir, ref Msg);  //it seems ok if gamsSysDir = "", then it will autolocate it (but there may be a 64-bit problem...)
                //GdxFast gdx = new gdxcs(Sysdir, ref Msg);
                if (Msg != string.Empty)
                {
                    Console.WriteLine("**** Could not load GDX library");
                    Console.WriteLine("**** " + Msg);
                    //return 1;
                }
                gdx.gdxGetDLLVersion(ref Msg);
                Console.WriteLine("Using GDX DLL version: " + Msg);

                if (false)
                {
                    int n = 3000;

                    DateTime tt = DateTime.Now;

                    //write demand data
                    gdx.gdxOpenWrite("demanddata.gdx", "Gekko", ref ErrNr);
                    if (ErrNr != 0)
                    {
                        //xp_example1.ReportIOError(ErrNr);
                        throw new GekkoException();
                    }

                    if (gdx.gdxDataWriteStrStart("x", "label", 2, gamsglobals.dt_var, 0) == 0)
                    {
                        //ReportGDXError();
                        throw new GekkoException();
                    }

                    string[] Indx2 = new string[2];
                    int[] index = new int[2];
                    for (int i = 1; i < n + 1; i++)
                    {
                        for (int j = 1; j < n + 1; j++)
                        {
                            Values[gamsglobals.val_level] = (n * 1000) * i + j;

                            if (true)
                            {
                                Indx2[0] = i.ToString();
                                Indx2[1] = j.ToString();
                                if (gdx.gdxDataWriteStr(Indx2, Values) == 0)
                                {
                                    G.Writeln2("OOPS");
                                }
                            }
                            else
                            {
                                index[0] = i;
                                index[1] = j;
                                if (gdx.gdxDataWriteRaw(index, Values) == 0)
                                {
                                    G.Writeln2("OOPS");
                                }
                            }

                            //gdx.gdxDataWriteRaw()
                        }
                    }

                    if (gdx.gdxDataWriteDone() == 0)
                    {
                        //ReportGDXError();
                        throw new GekkoException();
                    }
                    gdx.gdxClose();
                    //Console.WriteLine("Demand data written by xp_example1");

                    G.Writeln2("TIME: " + G.Seconds(tt));

                    return;
                }


                if (true)
                {
                    gdx.gdxOpenWrite(pathAndFilename, "Gekko", ref ErrNr);
                    if (ErrNr != 0)
                    {
                        //xp_example1.ReportIOError(ErrNr);
                        throw new GekkoException();
                    }
                    //int counter = 0;
                    foreach (ToFrom bnv in list)
                    {

                        IVariable iv = O.GetIVariableFromString(bnv.s1, O.ECreatePossibilities.NoneReportError, true);

                        string name = bnv.s2;
                        string nameWithoutFreq = G.Chop_GetName(name);

                        if (iv.Type() == EVariableType.Series)
                        {

                            Series ts = iv as Series;

                            string label = ""; if (ts.meta?.label != null) label = ts.meta.label;  //label = null will fail with weird error later on

                            int timeDimension = 1;
                            if (ts.type == ESeriesType.Timeless)
                            {
                                timeDimension = 0;
                            }
                            else if (ts.type == ESeriesType.ArraySuper)
                            {
                                int ntimeless = 0;
                                int nnontimeless = 0;
                                foreach (IVariable iv2 in ts.dimensionsStorage.storage.Values)
                                {
                                    if ((iv2 as Series).type == ESeriesType.Timeless) ntimeless++;
                                    else nnontimeless++;
                                }
                                if (ntimeless > 0 && nnontimeless > 0)
                                {
                                    new Error("The array-timeseries " + ts.name + " has subseries that are both");
                                    G.Writeln("           timeless and non-timeless --> cannot write to GDX.");
                                    throw new GekkoException();
                                }
                                if (ntimeless > 0) timeDimension = 0;
                                //if ntimeless + nnontimeless == 0 it will be assumed to have time-dim in GAMS --> hard to know.
                            }

                            string[] domains = new string[ts.dimensions + timeDimension];
                            for (int i = 0; i < domains.Length; i++) domains[i] = "*";
                            if (timeDimension == 1) domains[domains.Length - 1] = Program.options.gams_time_set;  //we alway put the t domain last

                            //counter++;

                            if (gdx.gdxDataWriteStrStart(nameWithoutFreq, label, domains.Length, gamsglobals.dt_var, 0) == 0)
                            {
                                //ReportGDXError();
                                throw new GekkoException();
                            }

                            gdx.gdxSystemInfo(ref syCnt, ref uelCnt);

                            if (gdx.gdxSymbolSetDomainX(syCnt, domains) == 0)
                            {
                                new Error("Could not write domain names");
                                //throw new GekkoException();
                            }

                            if (ts.type == ESeriesType.ArraySuper)
                            {
                                foreach (KeyValuePair<MultidimItem, IVariable> kvp in ts.dimensionsStorage.storage)
                                {
                                    string[] ss = kvp.Key.storage;
                                    WriteGdxHelper2(t1, t2, hasPrefix, gdx, kvp.Value as Series, ss, gdxValues);
                                }
                            }
                            else
                            {
                                //normal timeseries
                                WriteGdxHelper2(t1, t2, hasPrefix, gdx, ts, new string[0], gdxValues);
                            }

                            if (gdx.gdxDataWriteDone() == 0)
                            {
                                //ReportGDXError();
                                throw new GekkoException();
                            }
                            counterVariables++;
                        }
                        else if (iv.Type() == EVariableType.List)
                        {
                            if (gdx.gdxDataWriteStrStart(nameWithoutFreq.Replace(Globals.symbolCollection.ToString(), ""), "", 1, gamsglobals.dt_set, 0) == 0)
                            {
                                //ReportGDXError();
                                throw new GekkoException();
                            }

                            List l = iv as List;

                            foreach (string s in Stringlist.GetListOfStringsFromListOfIvariables(l.list.ToArray()))
                            {
                                if (gdx.gdxDataWriteStr(new string[] { s }, d) == 0)
                                {
                                    new Error("Problem writing set for gdx");
                                    //throw new GekkoException();
                                }
                            }

                            if (gdx.gdxDataWriteDone() == 0)
                            {
                                //ReportGDXError();
                                throw new GekkoException();
                            }
                            exportedSets++;
                        }
                        else continue;


                    }

                }


                ErrNr = gdx.gdxClose();
                if (ErrNr != 0)
                {
                    //ReportIOError(ErrNr);
                    throw new GekkoException();
                }

                G.Writeln2("Wrote " + counterVariables + " variables and " + exportedSets + " sets to " + pathAndFilename + " (" + G.Seconds(t) + ")");
                if (skippedSets > 0) G.Writeln("+++ NOTE: " + skippedSets + " sets with dim > 1 were not imported");


            }


        }

        public static void WriteGdxSlow(Databank databank, GekkoTime t1, GekkoTime t2, string pathAndFilename, List<ToFrom> list)
        {
            //TODO: try-catch if writing fails    

            bool usePrefix = false;
            if (Program.options.gams_time_prefix.Length > 0) usePrefix = true;

            DateTime t00 = DateTime.Now;
            int counterVariables = 0;
            int timelessCounter = 0;

            DateTime dt1 = DateTime.Now;

            string gamsDir = Program.options.gams_exe_folder.Trim();
            if (gamsDir.EndsWith("\\")) gamsDir = gamsDir.Substring(0, gamsDir.Length - "\\".Length);
            if (gamsDir.Trim() == "") gamsDir = null;  //must be so and not an empty string in the GAMSWorkspace call later on

            GAMSWorkspace ws = null;
            try
            {
                ws = new GAMSWorkspace(workingDirectory: Program.options.folder_working, systemDirectory: gamsDir);
            }
            catch (Exception e)
            {

                G.Writeln2("*** ERROR: Import of gdx file (GAMS) failed. GAMSWorkspace problem.");
                G.Writeln("           Technical error:");
                G.Writeln("           " + e.Message);
                G.Writeln("+++ NOTE:  You may manually indicate the GAMS program folder with 'OPTION gams exe folder = ...;'");
                throw;
            }

            GAMSDatabase db = ws.AddDatabase();

            foreach (ToFrom bnv in list)
            {
                string name = bnv.s2;  // bnv.name;


                string nameWithoutFreq = G.Chop_RemoveFreq(name);


                IVariable iv = O.GetIVariableFromString(bnv.s1, O.ECreatePossibilities.NoneReportError, true);


                Series ts = iv as Series;
                if (ts == null) continue;  //only write timeseries at the moment

                string label = ""; if (ts.meta?.label != null) label = ts.meta.label;  //label = null will fail with weird error later on

                int timeDimension = 1;
                if (ts.type == ESeriesType.Timeless)
                {
                    timeDimension = 0;
                }
                else if (ts.type == ESeriesType.ArraySuper)
                {
                    int ntimeless = 0;
                    int nnontimeless = 0;
                    foreach (IVariable iv2 in ts.dimensionsStorage.storage.Values)
                    {
                        if ((iv2 as Series).type == ESeriesType.Timeless) ntimeless++;
                        else nnontimeless++;
                    }
                    if (ntimeless > 0 && nnontimeless > 0)
                    {
                        new Error("The array-timeseries " + ts.name + " has subseries that are both timeless and non-timeless --> cannot write to GDX.");

                        //throw new GekkoException();
                    }
                    if (ntimeless > 0) timeDimension = 0;
                    //if ntimeless + nnontimeless == 0 it will be assumed to have time-dim in GAMS --> hard to know.
                }

                string[] domains = new string[ts.dimensions + timeDimension];
                for (int i = 0; i < domains.Length; i++) domains[i] = "*";
                if (timeDimension == 1) domains[domains.Length - 1] = Program.options.gams_time_set;  //we alway put the t domain last

                GAMSVariable gvar = db.AddVariable(nameWithoutFreq, VarType.Free, label, domains);

                counterVariables = WriteGdxHelperSlow(t1, t2, usePrefix, counterVariables, ts, gvar);

            }

            db.Export(pathAndFilename);

            G.Writeln2("Exported " + counterVariables + " variables to " + pathAndFilename + " (" + G.SecondsFormat((DateTime.Now - t00).TotalMilliseconds) + ")");
            if (timelessCounter > 0) G.Writeln("+++ NOTE: " + timelessCounter + " timeless timeseries skipped");
        }

        private static void WriteGdxHelper2(GekkoTime t1, GekkoTime t2, bool usePrefix, GdxFast gdx, Series ts2, string[] ss, double[] gdxValues)
        {

            if (ts2.type == ESeriesType.Timeless)
            {
                try
                {
                    gdxValues[gamsglobals.val_level] = ts2.GetTimelessData();
                    gdx.gdxDataWriteStr(ss, gdxValues);
                    //gvar.AddRecord(ss).Level = ts2.GetTimelessData();  //timeless data location   
                }
                catch
                {

                }
            }
            else
            {
                GekkoTime gt1 = t1;
                GekkoTime gt2 = t2;
                if (t1.IsNull())
                {
                    gt1 = ts2.GetRealDataPeriodFirst();
                    gt2 = ts2.GetRealDataPeriodLast();
                }
                if (gt1.IsNull())
                {
                    //do not write a weird record if the timeseries has no data
                }
                else
                {
                    string[] ss2 = new string[ss.Length + 1];
                    foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                    {
                        Array.Copy(ss, 0, ss2, 0, ss.Length);
                        string date = null;
                        if (usePrefix && t.freq == EFreq.A)
                        {
                            date = Program.options.gams_time_prefix + (t.super - (int)Program.options.gams_time_offset).ToString();
                        }
                        else
                        {
                            date = t.ToString();
                        }
                        ss2[ss2.Length - 1] = date;

                        gdxValues[gamsglobals.val_level] = ts2.GetDataSimple(t);
                        gdx.gdxDataWriteStr(ss2, gdxValues);

                        //gdx.gdxDataWriteRaw()  --> more efficient, see https://www.gams.com/~bussieck/LohBusWesReb.pdf, but then we need to maintain an UEL (each label has a number).
                        //and it seems that gdxDataWriteRaw() recuires lexical ordering of the array of indices??

                    }
                }
            }

            return;
        }

        private static int WriteGdxHelperSlow(GekkoTime t1, GekkoTime t2, bool usePrefix, int counterVariables, Series ts, GAMSVariable gvar)
        {

            if (ts.type == ESeriesType.ArraySuper)
            {
                foreach (KeyValuePair<MultidimItem, IVariable> kvp in ts.dimensionsStorage.storage)
                {
                    string[] ss = kvp.Key.storage;
                    WriteGdxHelperSlow2(t1, t2, usePrefix, gvar, kvp.Value as Series, ss);
                }
            }

            else
            {
                //normal timeseries
                WriteGdxHelperSlow2(t1, t2, usePrefix, gvar, ts, new string[0]);
            }
            counterVariables++;
            return counterVariables;
        }

        private static void WriteGdxHelperSlow2(GekkoTime t1, GekkoTime t2, bool usePrefix, GAMSVariable gvar, Series ts2, string[] ss)
        {
            if (ts2.type == ESeriesType.Timeless)
            {
                try
                {
                    gvar.AddRecord(ss).Level = ts2.GetTimelessData();  //timeless data location   
                }
                catch
                {

                }
            }
            else
            {
                GekkoTime gt1 = t1;
                GekkoTime gt2 = t2;
                if (t1.IsNull())
                {
                    gt1 = ts2.GetRealDataPeriodFirst();
                    gt2 = ts2.GetRealDataPeriodLast();
                }
                if (gt1.IsNull())
                {
                    //do not write a weird record if the timeseries has no data
                }
                else
                {
                    foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                    {
                        string[] ss2 = new string[ss.Length + 1];
                        Array.Copy(ss, 0, ss2, 0, ss.Length);
                        string date = null;
                        if (usePrefix && t.freq == EFreq.A)
                        {
                            date = Program.options.gams_time_prefix + (t.super - (int)Program.options.gams_time_offset).ToString();
                        }
                        else
                        {
                            date = t.ToString();
                        }
                        ss2[ss2.Length - 1] = date;

                        gvar.AddRecord(ss2).Level = ts2.GetDataSimple(t);

                    }
                }
            }

            return;
        }

        private static bool CompareDims(List<string> oldDims, List<string> dims)
        {
            //no test if they are null            
            if (dims.Count != oldDims.Count) return false;
            for (int i = 0; i < dims.Count; i++)
            {
                if (!G.Equal(dims[i], oldDims[i])) return false;
            }
            return true;
        }


        private static bool GamsIsFixed(double[] values, double value)
        {
            return value == values[gamsglobals.val_lower] || value == values[gamsglobals.val_upper];
        }

        private static void GdxErrorMessage()
        {
            G.Writeln("+++ NOTE:  You may manually indicate the GAMS program folder with 'OPTION gams exe folder',");
            G.Writeln("           for instance 'OPTION gams exe folder = c:\\GAMS\\win32\\24.8;'. In general, the");
            G.Writeln("           GAMS component is pretty good at auto-detecting the location of GAMS on the pc,");
            G.Writeln("           including finding a 32-bit GAMS if 32-bit Gekko is used, and a 64-bit GAMS if 64-bit");
            G.Writeln("           Gekko is used. It is probably not possible to use a 32-bit GAMS from a 64-bit Gekko,");
            G.Writeln("           but the inverse may be possible. In general, consider the bitness of both GAMS and");
            G.Writeln("           Gekko. Newer GAMS versions are 64-bit only, and in general, using Gekko 64-bit is");
            G.Writeln("           advised, too.");
            G.Writeln("           Bitness info: " + Program.Get64Bitness());            
        }

        private static void GetGAMSWorkspace(ref string gamsDir, ref GAMSWorkspace ws)
        {
            gamsDir = Program.options.gams_exe_folder.Trim();
            if (gamsDir.EndsWith("\\")) gamsDir = gamsDir.Substring(0, gamsDir.Length - "\\".Length);
            if (gamsDir.Trim() == "") gamsDir = null;  //must be so and not an empty string in the GAMSWorkspace call later on                        
            if (Program.options.gams_fast && gamsDir != null)
            {
                //do nothing
            }
            else
            {
                try
                {
                    if (Globals.gamsWorkspace == null || Globals.gamsWorkspaceHelper != gamsDir)
                    {
                        ws = new GAMSWorkspace(workingDirectory: Program.options.folder_working, systemDirectory: gamsDir);
                        Globals.gamsWorkspace = ws;
                        Globals.gamsWorkspaceHelper = gamsDir;  //record the param it was called with
                    }
                    else ws = Globals.gamsWorkspace;
                    gamsDir = ws.SystemDirectory;
                }
                catch (Exception e)
                {
                    G.Writeln2("*** ERROR: Import of gdx file (GAMS) failed. Could not locate GAMS (GAMSWorkspace problem).");
                    G.Writeln("           Technical error:");
                    G.Writeln("           " + e.Message);
                    GdxErrorMessage();
                    throw;
                }
            }
        }

        private static int GdxGetTimeDimNumber(ref int[] domainSyNrs, string[] domainStrings, int dimensions, GdxFast gdx, int timeIndex, int i)
        {
            int timeDimNr = -12345;
            gdx.gdxSymbolGetDomain(i, ref domainSyNrs);
            //only way to check it properly:
            int success = 1;
            for (int d2 = 0; d2 < dimensions; d2++)
            {
                if (domainSyNrs[d2] == 0)
                {
                    success = 0;
                    break;
                }
            }

            if (success == 1)
            {
                for (int d2 = dimensions - 1; d2 >= 0; d2--)  //backwards is faster since t is typically there
                {
                    if (domainSyNrs[d2] == timeIndex)
                    {
                        timeDimNr = d2;
                        break;
                    }
                }
            }
            else
            {
                //slower, but still not in the innermost loop
                //gdx.gdxSymbolGetDomainX(i, ref domainStrings);
                for (int d2 = dimensions - 1; d2 >= 0; d2--)  //backwards is faster since t is typically there
                {
                    if (G.Equal(domainStrings[d2], Program.options.gams_time_set))
                    {
                        timeDimNr = d2;
                        break;
                    }
                }
            }

            return timeDimNr;
        }

        

    }

    internal class GdxFast : IDisposable
    {
        private IntPtr pgdx;
        private bool extHandle;
        private bool _disposed;

#if __MonoCS__
    private delegate IntPtr DelLoadLibrary (string dllName, int flag);
    private delegate IntPtr DelGetProcAddress (IntPtr hModule, string procedureName);
    private delegate bool DelFreeLibrary (IntPtr hModul);

#if __APPLE__
    [DllImport("libdl.dylib")]
    internal static extern IntPtr dlopen(String dllname, int flags);

    [DllImport("libdl.dylib")]
    internal static extern IntPtr dlsym(IntPtr hModule, String procedureName);

    [DllImport("libdl.dylib")] //int
    internal static extern bool dlclose (IntPtr hModul);
#else
    [DllImport("libdl.so")]
    internal static extern IntPtr dlopen(String dllname, int flags);

    [DllImport("libdl.so")]
    internal static extern IntPtr dlsym(IntPtr hModule, String procedureName);

    [DllImport("libdl.so")]
    internal static extern bool dlclose (IntPtr hModul);
#endif

    DelLoadLibrary LoadLibrary = new DelLoadLibrary(dlopen);
    DelGetProcAddress GetProcAddress = new DelGetProcAddress (dlsym);
    DelFreeLibrary FreeLibrary = new DelFreeLibrary (dlclose);
#else
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
#endif


        public delegate void TDataStoreProc(IntPtr Indx, IntPtr Vals);
        public delegate int TDataStoreFiltProc(IntPtr Indx, IntPtr Vals, IntPtr Uptr);
        public delegate void TDomainIndexProc(int RawIndex, int MappedIndex, IntPtr Uptr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gdxSetLoadPath_t(string s);
        private static gdxSetLoadPath_t dll_gdxSetLoadPath;
        private static void d_gdxSetLoadPath(string s)
        { }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gdxGetLoadPath_t(ref byte s);
        private static gdxGetLoadPath_t dll_gdxGetLoadPath;
        private static void d_gdxGetLoadPath(ref byte s)
        { }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymAdd_t(IntPtr pgdx, string AName, string Txt, int AIndx);
        private static gdxAcronymAdd_t dll_gdxAcronymAdd;
        private static int d_gdxAcronymAdd(IntPtr pgdx, string AName, string Txt, int AIndx)
        { gdxErrorHandling("gdxAcronymAdd could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymCount_t(IntPtr pgdx);
        private static gdxAcronymCount_t dll_gdxAcronymCount;
        private static int d_gdxAcronymCount(IntPtr pgdx)
        { gdxErrorHandling("gdxAcronymCount could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymGetInfo_t(IntPtr pgdx, int N, StringBuilder AName, StringBuilder Txt, ref int AIndx);
        private static gdxAcronymGetInfo_t dll_gdxAcronymGetInfo;
        private static int d_gdxAcronymGetInfo(IntPtr pgdx, int N, StringBuilder AName, StringBuilder Txt, ref int AIndx)
        { gdxErrorHandling("gdxAcronymGetInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymGetMapping_t(IntPtr pgdx, int N, ref int orgIndx, ref int newIndx, ref int autoIndex);
        private static gdxAcronymGetMapping_t dll_gdxAcronymGetMapping;
        private static int d_gdxAcronymGetMapping(IntPtr pgdx, int N, ref int orgIndx, ref int newIndx, ref int autoIndex)
        { gdxErrorHandling("gdxAcronymGetMapping could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymIndex_t(IntPtr pgdx, double V);
        private static gdxAcronymIndex_t dll_gdxAcronymIndex;
        private static int d_gdxAcronymIndex(IntPtr pgdx, double V)
        { gdxErrorHandling("gdxAcronymIndex could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymName_t(IntPtr pgdx, double V, StringBuilder AName);
        private static gdxAcronymName_t dll_gdxAcronymName;
        private static int d_gdxAcronymName(IntPtr pgdx, double V, StringBuilder AName)
        { gdxErrorHandling("gdxAcronymName could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymNextNr_t(IntPtr pgdx, int NV);
        private static gdxAcronymNextNr_t dll_gdxAcronymNextNr;
        private static int d_gdxAcronymNextNr(IntPtr pgdx, int NV)
        { gdxErrorHandling("gdxAcronymNextNr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymSetInfo_t(IntPtr pgdx, int N, string AName, string Txt, int AIndx);
        private static gdxAcronymSetInfo_t dll_gdxAcronymSetInfo;
        private static int d_gdxAcronymSetInfo(IntPtr pgdx, int N, string AName, string Txt, int AIndx)
        { gdxErrorHandling("gdxAcronymSetInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gdxAcronymValue_t(IntPtr pgdx, int AIndx);
        private static gdxAcronymValue_t dll_gdxAcronymValue;
        private static double d_gdxAcronymValue(IntPtr pgdx, int AIndx)
        { gdxErrorHandling("gdxAcronymValue could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAddAlias_t(IntPtr pgdx, string Id1, string Id2);
        private static gdxAddAlias_t dll_gdxAddAlias;
        private static int d_gdxAddAlias(IntPtr pgdx, string Id1, string Id2)
        { gdxErrorHandling("gdxAddAlias could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAddSetText_t(IntPtr pgdx, string Txt, ref int TxtNr);
        private static gdxAddSetText_t dll_gdxAddSetText;
        private static int d_gdxAddSetText(IntPtr pgdx, string Txt, ref int TxtNr)
        { gdxErrorHandling("gdxAddSetText could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAutoConvert_t(IntPtr pgdx, int NV);
        private static gdxAutoConvert_t dll_gdxAutoConvert;
        private static int d_gdxAutoConvert(IntPtr pgdx, int NV)
        { gdxErrorHandling("gdxAutoConvert could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxClose_t(IntPtr pgdx);
        private static gdxClose_t dll_gdxClose;
        private static int d_gdxClose(IntPtr pgdx)
        { gdxErrorHandling("gdxClose could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataErrorCount_t(IntPtr pgdx);
        private static gdxDataErrorCount_t dll_gdxDataErrorCount;
        private static int d_gdxDataErrorCount(IntPtr pgdx)
        { gdxErrorHandling("gdxDataErrorCount could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataErrorRecord_t(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values);
        private static gdxDataErrorRecord_t dll_gdxDataErrorRecord;
        private static int d_gdxDataErrorRecord(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values)
        { gdxErrorHandling("gdxDataErrorRecord could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataErrorRecordX_t(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values);
        private static gdxDataErrorRecordX_t dll_gdxDataErrorRecordX;
        private static int d_gdxDataErrorRecordX(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values)
        { gdxErrorHandling("gdxDataErrorRecordX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadDone_t(IntPtr pgdx);
        private static gdxDataReadDone_t dll_gdxDataReadDone;
        private static int d_gdxDataReadDone(IntPtr pgdx)
        { gdxErrorHandling("gdxDataReadDone could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadFilteredStart_t(IntPtr pgdx, int SyNr, int[] FilterAction, ref int NrRecs);
        private static gdxDataReadFilteredStart_t dll_gdxDataReadFilteredStart;
        private static int d_gdxDataReadFilteredStart(IntPtr pgdx, int SyNr, int[] FilterAction, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadFilteredStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadMap_t(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values, ref int DimFrst);
        private static gdxDataReadMap_t dll_gdxDataReadMap;
        private static int d_gdxDataReadMap(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values, ref int DimFrst)
        { gdxErrorHandling("gdxDataReadMap could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadMapStart_t(IntPtr pgdx, int SyNr, ref int NrRecs);
        private static gdxDataReadMapStart_t dll_gdxDataReadMapStart;
        private static int d_gdxDataReadMapStart(IntPtr pgdx, int SyNr, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadMapStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadRaw_t(IntPtr pgdx, int[] KeyInt, double[] Values, ref int DimFrst);
        private static gdxDataReadRaw_t dll_gdxDataReadRaw;
        private static int d_gdxDataReadRaw(IntPtr pgdx, int[] KeyInt, double[] Values, ref int DimFrst)
        { gdxErrorHandling("gdxDataReadRaw could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadRawFast_t(IntPtr pgdx, int SyNr, TDataStoreProc DP, ref int NrRecs);
        private static gdxDataReadRawFast_t dll_gdxDataReadRawFast;
        private static int d_gdxDataReadRawFast(IntPtr pgdx, int SyNr, TDataStoreProc DP, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadRawFast could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadRawFastFilt_t(IntPtr pgdx, int SyNr, string[] UelFilterStr, TDataStoreFiltProc DP);
        private static gdxDataReadRawFastFilt_t dll_gdxDataReadRawFastFilt;
        private static int d_gdxDataReadRawFastFilt(IntPtr pgdx, int SyNr, string[] UelFilterStr, TDataStoreFiltProc DP)
        { gdxErrorHandling("gdxDataReadRawFastFilt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadRawStart_t(IntPtr pgdx, int SyNr, ref int NrRecs);
        private static gdxDataReadRawStart_t dll_gdxDataReadRawStart;
        private static int d_gdxDataReadRawStart(IntPtr pgdx, int SyNr, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadRawStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadSlice_t(IntPtr pgdx, string[] UelFilterStr, ref int Dimen, TDataStoreProc DP);
        private static gdxDataReadSlice_t dll_gdxDataReadSlice;
        private static int d_gdxDataReadSlice(IntPtr pgdx, string[] UelFilterStr, ref int Dimen, TDataStoreProc DP)
        { gdxErrorHandling("gdxDataReadSlice could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadSliceStart_t(IntPtr pgdx, int SyNr, int[] ElemCounts);
        private static gdxDataReadSliceStart_t dll_gdxDataReadSliceStart;
        private static int d_gdxDataReadSliceStart(IntPtr pgdx, int SyNr, int[] ElemCounts)
        { gdxErrorHandling("gdxDataReadSliceStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadStr_t(IntPtr pgdx, byte[,] KeyStr, double[] Values, ref int DimFrst);
        private static gdxDataReadStr_t dll_gdxDataReadStr;
        private static int d_gdxDataReadStr(IntPtr pgdx, byte[,] KeyStr, double[] Values, ref int DimFrst)
        { gdxErrorHandling("gdxDataReadStr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadStrStart_t(IntPtr pgdx, int SyNr, ref int NrRecs);
        private static gdxDataReadStrStart_t dll_gdxDataReadStrStart;
        private static int d_gdxDataReadStrStart(IntPtr pgdx, int SyNr, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadStrStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataSliceUELS_t(IntPtr pgdx, int[] SliceKeyInt, byte[,] KeyStr);
        private static gdxDataSliceUELS_t dll_gdxDataSliceUELS;
        private static int d_gdxDataSliceUELS(IntPtr pgdx, int[] SliceKeyInt, byte[,] KeyStr)
        { gdxErrorHandling("gdxDataSliceUELS could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteDone_t(IntPtr pgdx);
        private static gdxDataWriteDone_t dll_gdxDataWriteDone;
        private static int d_gdxDataWriteDone(IntPtr pgdx)
        { gdxErrorHandling("gdxDataWriteDone could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteMap_t(IntPtr pgdx, int[] KeyInt, double[] Values);
        private static gdxDataWriteMap_t dll_gdxDataWriteMap;
        private static int d_gdxDataWriteMap(IntPtr pgdx, int[] KeyInt, double[] Values)
        { gdxErrorHandling("gdxDataWriteMap could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteMapStart_t(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo);
        private static gdxDataWriteMapStart_t dll_gdxDataWriteMapStart;
        private static int d_gdxDataWriteMapStart(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        { gdxErrorHandling("gdxDataWriteMapStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteRaw_t(IntPtr pgdx, int[] KeyInt, double[] Values);
        private static gdxDataWriteRaw_t dll_gdxDataWriteRaw;
        private static int d_gdxDataWriteRaw(IntPtr pgdx, int[] KeyInt, double[] Values)
        { gdxErrorHandling("gdxDataWriteRaw could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteRawStart_t(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo);
        private static gdxDataWriteRawStart_t dll_gdxDataWriteRawStart;
        private static int d_gdxDataWriteRawStart(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        { gdxErrorHandling("gdxDataWriteRawStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteStr_t(IntPtr pgdx, string[] KeyStr, double[] Values);
        private static gdxDataWriteStr_t dll_gdxDataWriteStr;
        private static int d_gdxDataWriteStr(IntPtr pgdx, string[] KeyStr, double[] Values)
        { gdxErrorHandling("gdxDataWriteStr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteStrStart_t(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo);
        private static gdxDataWriteStrStart_t dll_gdxDataWriteStrStart;
        private static int d_gdxDataWriteStrStart(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        { gdxErrorHandling("gdxDataWriteStrStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetDLLVersion_t(IntPtr pgdx, StringBuilder V);
        private static gdxGetDLLVersion_t dll_gdxGetDLLVersion;
        private static int d_gdxGetDLLVersion(IntPtr pgdx, StringBuilder V)
        { gdxErrorHandling("gdxGetDLLVersion could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxErrorCount_t(IntPtr pgdx);
        private static gdxErrorCount_t dll_gdxErrorCount;
        private static int d_gdxErrorCount(IntPtr pgdx)
        { gdxErrorHandling("gdxErrorCount could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxErrorStr_t(IntPtr pgdx, int ErrNr, StringBuilder ErrMsg);
        private static gdxErrorStr_t dll_gdxErrorStr;
        private static int d_gdxErrorStr(IntPtr pgdx, int ErrNr, StringBuilder ErrMsg)
        { gdxErrorHandling("gdxErrorStr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFileInfo_t(IntPtr pgdx, ref int FileVer, ref int ComprLev);
        private static gdxFileInfo_t dll_gdxFileInfo;
        private static int d_gdxFileInfo(IntPtr pgdx, ref int FileVer, ref int ComprLev)
        { gdxErrorHandling("gdxFileInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFileVersion_t(IntPtr pgdx, StringBuilder FileStr, StringBuilder ProduceStr);
        private static gdxFileVersion_t dll_gdxFileVersion;
        private static int d_gdxFileVersion(IntPtr pgdx, StringBuilder FileStr, StringBuilder ProduceStr)
        { gdxErrorHandling("gdxFileVersion could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFilterExists_t(IntPtr pgdx, int FilterNr);
        private static gdxFilterExists_t dll_gdxFilterExists;
        private static int d_gdxFilterExists(IntPtr pgdx, int FilterNr)
        { gdxErrorHandling("gdxFilterExists could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFilterRegister_t(IntPtr pgdx, int UelMap);
        private static gdxFilterRegister_t dll_gdxFilterRegister;
        private static int d_gdxFilterRegister(IntPtr pgdx, int UelMap)
        { gdxErrorHandling("gdxFilterRegister could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFilterRegisterDone_t(IntPtr pgdx);
        private static gdxFilterRegisterDone_t dll_gdxFilterRegisterDone;
        private static int d_gdxFilterRegisterDone(IntPtr pgdx)
        { gdxErrorHandling("gdxFilterRegisterDone could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFilterRegisterStart_t(IntPtr pgdx, int FilterNr);
        private static gdxFilterRegisterStart_t dll_gdxFilterRegisterStart;
        private static int d_gdxFilterRegisterStart(IntPtr pgdx, int FilterNr)
        { gdxErrorHandling("gdxFilterRegisterStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFindSymbol_t(IntPtr pgdx, string SyId, ref int SyNr);
        private static gdxFindSymbol_t dll_gdxFindSymbol;
        private static int d_gdxFindSymbol(IntPtr pgdx, string SyId, ref int SyNr)
        { gdxErrorHandling("gdxFindSymbol could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetElemText_t(IntPtr pgdx, int TxtNr, StringBuilder Txt, ref int Node);
        private static gdxGetElemText_t dll_gdxGetElemText;
        private static int d_gdxGetElemText(IntPtr pgdx, int TxtNr, StringBuilder Txt, ref int Node)
        { gdxErrorHandling("gdxGetElemText could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetLastError_t(IntPtr pgdx);
        private static gdxGetLastError_t dll_gdxGetLastError;
        private static int d_gdxGetLastError(IntPtr pgdx)
        { gdxErrorHandling("gdxGetLastError could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int64 gdxGetMemoryUsed_t(IntPtr pgdx);
        private static gdxGetMemoryUsed_t dll_gdxGetMemoryUsed;
        private static Int64 d_gdxGetMemoryUsed(IntPtr pgdx)
        { gdxErrorHandling("gdxGetMemoryUsed could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetSpecialValues_t(IntPtr pgdx, double[] AVals);
        private static gdxGetSpecialValues_t dll_gdxGetSpecialValues;
        private static int d_gdxGetSpecialValues(IntPtr pgdx, double[] AVals)
        { gdxErrorHandling("gdxGetSpecialValues could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetUEL_t(IntPtr pgdx, int UelNr, StringBuilder Uel);
        private static gdxGetUEL_t dll_gdxGetUEL;
        private static int d_gdxGetUEL(IntPtr pgdx, int UelNr, StringBuilder Uel)
        { gdxErrorHandling("gdxGetUEL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxMapValue_t(IntPtr pgdx, double D, ref int sv);
        private static gdxMapValue_t dll_gdxMapValue;
        private static int d_gdxMapValue(IntPtr pgdx, double D, ref int sv)
        { gdxErrorHandling("gdxMapValue could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxOpenAppend_t(IntPtr pgdx, string FileName, string Producer, ref int ErrNr);
        private static gdxOpenAppend_t dll_gdxOpenAppend;
        private static int d_gdxOpenAppend(IntPtr pgdx, string FileName, string Producer, ref int ErrNr)
        { gdxErrorHandling("gdxOpenAppend could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxOpenRead_t(IntPtr pgdx, string FileName, ref int ErrNr);
        private static gdxOpenRead_t dll_gdxOpenRead;
        private static int d_gdxOpenRead(IntPtr pgdx, string FileName, ref int ErrNr)
        { gdxErrorHandling("gdxOpenRead could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxOpenWrite_t(IntPtr pgdx, string FileName, string Producer, ref int ErrNr);
        private static gdxOpenWrite_t dll_gdxOpenWrite;
        private static int d_gdxOpenWrite(IntPtr pgdx, string FileName, string Producer, ref int ErrNr)
        { gdxErrorHandling("gdxOpenWrite could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxOpenWriteEx_t(IntPtr pgdx, string FileName, string Producer, int Compr, ref int ErrNr);
        private static gdxOpenWriteEx_t dll_gdxOpenWriteEx;
        private static int d_gdxOpenWriteEx(IntPtr pgdx, string FileName, string Producer, int Compr, ref int ErrNr)
        { gdxErrorHandling("gdxOpenWriteEx could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxResetSpecialValues_t(IntPtr pgdx);
        private static gdxResetSpecialValues_t dll_gdxResetSpecialValues;
        private static int d_gdxResetSpecialValues(IntPtr pgdx)
        { gdxErrorHandling("gdxResetSpecialValues could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetHasText_t(IntPtr pgdx, int SyNr);
        private static gdxSetHasText_t dll_gdxSetHasText;
        private static int d_gdxSetHasText(IntPtr pgdx, int SyNr)
        { gdxErrorHandling("gdxSetHasText could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetReadSpecialValues_t(IntPtr pgdx, double[] AVals);
        private static gdxSetReadSpecialValues_t dll_gdxSetReadSpecialValues;
        private static int d_gdxSetReadSpecialValues(IntPtr pgdx, double[] AVals)
        { gdxErrorHandling("gdxSetReadSpecialValues could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetSpecialValues_t(IntPtr pgdx, double[] AVals);
        private static gdxSetSpecialValues_t dll_gdxSetSpecialValues;
        private static int d_gdxSetSpecialValues(IntPtr pgdx, double[] AVals)
        { gdxErrorHandling("gdxSetSpecialValues could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetTextNodeNr_t(IntPtr pgdx, int TxtNr, int Node);
        private static gdxSetTextNodeNr_t dll_gdxSetTextNodeNr;
        private static int d_gdxSetTextNodeNr(IntPtr pgdx, int TxtNr, int Node)
        { gdxErrorHandling("gdxSetTextNodeNr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetTraceLevel_t(IntPtr pgdx, int N, string s);
        private static gdxSetTraceLevel_t dll_gdxSetTraceLevel;
        private static int d_gdxSetTraceLevel(IntPtr pgdx, int N, string s)
        { gdxErrorHandling("gdxSetTraceLevel could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbIndxMaxLength_t(IntPtr pgdx, int SyNr, int[] LengthInfo);
        private static gdxSymbIndxMaxLength_t dll_gdxSymbIndxMaxLength;
        private static int d_gdxSymbIndxMaxLength(IntPtr pgdx, int SyNr, int[] LengthInfo)
        { gdxErrorHandling("gdxSymbIndxMaxLength could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbMaxLength_t(IntPtr pgdx);
        private static gdxSymbMaxLength_t dll_gdxSymbMaxLength;
        private static int d_gdxSymbMaxLength(IntPtr pgdx)
        { gdxErrorHandling("gdxSymbMaxLength could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolAddComment_t(IntPtr pgdx, int SyNr, string Txt);
        private static gdxSymbolAddComment_t dll_gdxSymbolAddComment;
        private static int d_gdxSymbolAddComment(IntPtr pgdx, int SyNr, string Txt)
        { gdxErrorHandling("gdxSymbolAddComment could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolGetComment_t(IntPtr pgdx, int SyNr, int N, StringBuilder Txt);
        private static gdxSymbolGetComment_t dll_gdxSymbolGetComment;
        private static int d_gdxSymbolGetComment(IntPtr pgdx, int SyNr, int N, StringBuilder Txt)
        { gdxErrorHandling("gdxSymbolGetComment could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolGetDomain_t(IntPtr pgdx, int SyNr, int[] DomainSyNrs);
        private static gdxSymbolGetDomain_t dll_gdxSymbolGetDomain;
        private static int d_gdxSymbolGetDomain(IntPtr pgdx, int SyNr, int[] DomainSyNrs)
        { gdxErrorHandling("gdxSymbolGetDomain could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolGetDomainX_t(IntPtr pgdx, int SyNr, byte[,] DomainIDs);
        private static gdxSymbolGetDomainX_t dll_gdxSymbolGetDomainX;
        private static int d_gdxSymbolGetDomainX(IntPtr pgdx, int SyNr, byte[,] DomainIDs)
        { gdxErrorHandling("gdxSymbolGetDomainX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolDim_t(IntPtr pgdx, int SyNr);
        private static gdxSymbolDim_t dll_gdxSymbolDim;
        private static int d_gdxSymbolDim(IntPtr pgdx, int SyNr)
        { gdxErrorHandling("gdxSymbolDim could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolInfo_t(IntPtr pgdx, int SyNr, StringBuilder SyId, ref int Dimen, ref int Typ);
        private static gdxSymbolInfo_t dll_gdxSymbolInfo;
        private static int d_gdxSymbolInfo(IntPtr pgdx, int SyNr, StringBuilder SyId, ref int Dimen, ref int Typ)
        { gdxErrorHandling("gdxSymbolInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolInfoX_t(IntPtr pgdx, int SyNr, ref int RecCnt, ref int UserInfo, StringBuilder ExplTxt);
        private static gdxSymbolInfoX_t dll_gdxSymbolInfoX;
        private static int d_gdxSymbolInfoX(IntPtr pgdx, int SyNr, ref int RecCnt, ref int UserInfo, StringBuilder ExplTxt)
        { gdxErrorHandling("gdxSymbolInfoX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolSetDomain_t(IntPtr pgdx, string[] DomainIDs);
        private static gdxSymbolSetDomain_t dll_gdxSymbolSetDomain;
        private static int d_gdxSymbolSetDomain(IntPtr pgdx, string[] DomainIDs)
        { gdxErrorHandling("gdxSymbolSetDomain could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolSetDomainX_t(IntPtr pgdx, int SyNr, string[] DomainIDs);
        private static gdxSymbolSetDomainX_t dll_gdxSymbolSetDomainX;
        private static int d_gdxSymbolSetDomainX(IntPtr pgdx, int SyNr, string[] DomainIDs)
        { gdxErrorHandling("gdxSymbolSetDomainX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSystemInfo_t(IntPtr pgdx, ref int SyCnt, ref int UelCnt);
        private static gdxSystemInfo_t dll_gdxSystemInfo;
        private static int d_gdxSystemInfo(IntPtr pgdx, ref int SyCnt, ref int UelCnt)
        { gdxErrorHandling("gdxSystemInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELMaxLength_t(IntPtr pgdx);
        private static gdxUELMaxLength_t dll_gdxUELMaxLength;
        private static int d_gdxUELMaxLength(IntPtr pgdx)
        { gdxErrorHandling("gdxUELMaxLength could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterDone_t(IntPtr pgdx);
        private static gdxUELRegisterDone_t dll_gdxUELRegisterDone;
        private static int d_gdxUELRegisterDone(IntPtr pgdx)
        { gdxErrorHandling("gdxUELRegisterDone could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterMap_t(IntPtr pgdx, int UMap, string Uel);
        private static gdxUELRegisterMap_t dll_gdxUELRegisterMap;
        private static int d_gdxUELRegisterMap(IntPtr pgdx, int UMap, string Uel)
        { gdxErrorHandling("gdxUELRegisterMap could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterMapStart_t(IntPtr pgdx);
        private static gdxUELRegisterMapStart_t dll_gdxUELRegisterMapStart;
        private static int d_gdxUELRegisterMapStart(IntPtr pgdx)
        { gdxErrorHandling("gdxUELRegisterMapStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterRaw_t(IntPtr pgdx, string Uel);
        private static gdxUELRegisterRaw_t dll_gdxUELRegisterRaw;
        private static int d_gdxUELRegisterRaw(IntPtr pgdx, string Uel)
        { gdxErrorHandling("gdxUELRegisterRaw could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterRawStart_t(IntPtr pgdx);
        private static gdxUELRegisterRawStart_t dll_gdxUELRegisterRawStart;
        private static int d_gdxUELRegisterRawStart(IntPtr pgdx)
        { gdxErrorHandling("gdxUELRegisterRawStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterStr_t(IntPtr pgdx, string Uel, ref int UelNr);
        private static gdxUELRegisterStr_t dll_gdxUELRegisterStr;
        private static int d_gdxUELRegisterStr(IntPtr pgdx, string Uel, ref int UelNr)
        { gdxErrorHandling("gdxUELRegisterStr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterStrStart_t(IntPtr pgdx);
        private static gdxUELRegisterStrStart_t dll_gdxUELRegisterStrStart;
        private static int d_gdxUELRegisterStrStart(IntPtr pgdx)
        { gdxErrorHandling("gdxUELRegisterStrStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUMFindUEL_t(IntPtr pgdx, string Uel, ref int UelNr, ref int UelMap);
        private static gdxUMFindUEL_t dll_gdxUMFindUEL;
        private static int d_gdxUMFindUEL(IntPtr pgdx, string Uel, ref int UelNr, ref int UelMap)
        { gdxErrorHandling("gdxUMFindUEL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUMUelGet_t(IntPtr pgdx, int UelNr, StringBuilder Uel, ref int UelMap);
        private static gdxUMUelGet_t dll_gdxUMUelGet;
        private static int d_gdxUMUelGet(IntPtr pgdx, int UelNr, StringBuilder Uel, ref int UelMap)
        { gdxErrorHandling("gdxUMUelGet could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUMUelInfo_t(IntPtr pgdx, ref int UelCnt, ref int HighMap);
        private static gdxUMUelInfo_t dll_gdxUMUelInfo;
        private static int d_gdxUMUelInfo(IntPtr pgdx, ref int UelCnt, ref int HighMap)
        { gdxErrorHandling("gdxUMUelInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetDomainElements_t(IntPtr pgdx, int SyNr, int DimPos, int FilterNr, TDomainIndexProc DP, ref int NrElem, IntPtr Uptr);
        private static gdxGetDomainElements_t dll_gdxGetDomainElements;
        private static int d_gdxGetDomainElements(IntPtr pgdx, int SyNr, int DimPos, int FilterNr, TDomainIndexProc DP, ref int NrElem, IntPtr Uptr)
        { gdxErrorHandling("gdxGetDomainElements could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxCurrentDim_t(IntPtr pgdx);
        private static gdxCurrentDim_t dll_gdxCurrentDim;
        private static int d_gdxCurrentDim(IntPtr pgdx)
        { gdxErrorHandling("gdxCurrentDim could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxRenameUEL_t(IntPtr pgdx, string OldName, string NewName);
        private static gdxRenameUEL_t dll_gdxRenameUEL;
        private static int d_gdxRenameUEL(IntPtr pgdx, string OldName, string NewName)
        { gdxErrorHandling("gdxRenameUEL could not be loaded"); return 0; }


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void xcreate_t(ref IntPtr pgdx);
        private static xcreate_t xcreate;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void xfree_t(ref IntPtr pgdx);
        private static xfree_t xfree;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int xapiversion_t(int api, StringBuilder msg, ref int cl);
        private static xapiversion_t dll_xapiversion;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int xcheck_t(string ep, int nargs, int[] s, StringBuilder msg);
        private static xcheck_t dll_xcheck;

        public delegate bool gdxErrorCallback_t(int ErrCount, string Msg);

        static bool isLoaded = false;
        static IntPtr h;
        static bool ScreenIndicator = true;
        static bool ExceptionIndicator = false;
        static bool ExitIndicator = true;
        static gdxErrorCallback_t ErrorCallBack = null;
        static int APIErrorCount = 0;

        private bool XLibraryLoad(string dllName, ref string errBuf)
        {
            string symName;
            int cl = 0;
            IntPtr pAddressOfFunctionToCall;

            if (isLoaded)
                return true;

#if __MonoCS__
        h = LoadLibrary(@dllName,2);
#else
            h = LoadLibrary(@dllName);
#endif

            if (IntPtr.Zero == h)
            {
                errBuf = "Could not load shared library " + dllName;
                return false;
            }

            pAddressOfFunctionToCall = GetProcAddress(h, "xcreate");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                xcreate = (xcreate_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(xcreate_t));
            else
            {
                symName = "xcreate"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "xfree");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                xfree = (xfree_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(xfree_t));
            else
            {
                symName = "xfree"; goto symMissing;
            }

            pAddressOfFunctionToCall = GetProcAddress(h, "cxcheck");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_xcheck = (xcheck_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(xcheck_t));
            else
            {
                symName = "cxcheck"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "cxapiversion");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_xapiversion = (xapiversion_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(xapiversion_t));
            else
            {
                symName = "cxapiversion"; goto symMissing;
            }

            if (xapiversion(7, ref errBuf, ref cl) == 0)
                return false;

            pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsetloadpath");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_gdxSetLoadPath = (gdxSetLoadPath_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetLoadPath_t));
            pAddressOfFunctionToCall = GetProcAddress(h, "cgdxgetloadpath");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_gdxGetLoadPath = (gdxGetLoadPath_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetLoadPath_t));
            {
                int[] s = { 3, 11, 11, 3 };
                if (xcheck("gdxAcronymAdd", 3, s, ref errBuf) == 0)
                    dll_gdxAcronymAdd = d_gdxAcronymAdd;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxacronymadd");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymAdd = (gdxAcronymAdd_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymAdd_t));
                    else
                    {
                        symName = "cgdxAcronymAdd"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxAcronymCount", 0, s, ref errBuf) == 0)
                    dll_gdxAcronymCount = d_gdxAcronymCount;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymcount");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymCount = (gdxAcronymCount_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymCount_t));
                    else
                    {
                        symName = "gdxAcronymCount"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12, 12, 4 };
                if (xcheck("gdxAcronymGetInfo", 4, s, ref errBuf) == 0)
                    dll_gdxAcronymGetInfo = d_gdxAcronymGetInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxacronymgetinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymGetInfo = (gdxAcronymGetInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymGetInfo_t));
                    else
                    {
                        symName = "cgdxAcronymGetInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4, 4, 4 };
                if (xcheck("gdxAcronymGetMapping", 4, s, ref errBuf) == 0)
                    dll_gdxAcronymGetMapping = d_gdxAcronymGetMapping;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymgetmapping");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymGetMapping = (gdxAcronymGetMapping_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymGetMapping_t));
                    else
                    {
                        symName = "gdxAcronymGetMapping"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 13 };
                if (xcheck("gdxAcronymIndex", 1, s, ref errBuf) == 0)
                    dll_gdxAcronymIndex = d_gdxAcronymIndex;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymindex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymIndex = (gdxAcronymIndex_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymIndex_t));
                    else
                    {
                        symName = "gdxAcronymIndex"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 13, 12 };
                if (xcheck("gdxAcronymName", 2, s, ref errBuf) == 0)
                    dll_gdxAcronymName = d_gdxAcronymName;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxacronymname");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymName = (gdxAcronymName_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymName_t));
                    else
                    {
                        symName = "cgdxAcronymName"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxAcronymNextNr", 1, s, ref errBuf) == 0)
                    dll_gdxAcronymNextNr = d_gdxAcronymNextNr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymnextnr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymNextNr = (gdxAcronymNextNr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymNextNr_t));
                    else
                    {
                        symName = "gdxAcronymNextNr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 11, 11, 3 };
                if (xcheck("gdxAcronymSetInfo", 4, s, ref errBuf) == 0)
                    dll_gdxAcronymSetInfo = d_gdxAcronymSetInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxacronymsetinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymSetInfo = (gdxAcronymSetInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymSetInfo_t));
                    else
                    {
                        symName = "cgdxAcronymSetInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (xcheck("gdxAcronymValue", 1, s, ref errBuf) == 0)
                    dll_gdxAcronymValue = d_gdxAcronymValue;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymvalue");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymValue = (gdxAcronymValue_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymValue_t));
                    else
                    {
                        symName = "gdxAcronymValue"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11 };
                if (xcheck("gdxAddAlias", 2, s, ref errBuf) == 0)
                    dll_gdxAddAlias = d_gdxAddAlias;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxaddalias");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAddAlias = (gdxAddAlias_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAddAlias_t));
                    else
                    {
                        symName = "cgdxAddAlias"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4 };
                if (xcheck("gdxAddSetText", 2, s, ref errBuf) == 0)
                    dll_gdxAddSetText = d_gdxAddSetText;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxaddsettext");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAddSetText = (gdxAddSetText_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAddSetText_t));
                    else
                    {
                        symName = "cgdxAddSetText"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxAutoConvert", 1, s, ref errBuf) == 0)
                    dll_gdxAutoConvert = d_gdxAutoConvert;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxautoconvert");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAutoConvert = (gdxAutoConvert_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAutoConvert_t));
                    else
                    {
                        symName = "gdxAutoConvert"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxClose", 0, s, ref errBuf) == 0)
                    dll_gdxClose = d_gdxClose;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxclose");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxClose = (gdxClose_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxClose_t));
                    else
                    {
                        symName = "gdxClose"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxDataErrorCount", 0, s, ref errBuf) == 0)
                    dll_gdxDataErrorCount = d_gdxDataErrorCount;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdataerrorcount");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataErrorCount = (gdxDataErrorCount_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataErrorCount_t));
                    else
                    {
                        symName = "gdxDataErrorCount"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52, 54 };
                if (xcheck("gdxDataErrorRecord", 3, s, ref errBuf) == 0)
                    dll_gdxDataErrorRecord = d_gdxDataErrorRecord;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdataerrorrecord");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataErrorRecord = (gdxDataErrorRecord_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataErrorRecord_t));
                    else
                    {
                        symName = "gdxDataErrorRecord"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52, 54 };
                if (xcheck("gdxDataErrorRecordX", 3, s, ref errBuf) == 0)
                    dll_gdxDataErrorRecordX = d_gdxDataErrorRecordX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdataerrorrecordx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataErrorRecordX = (gdxDataErrorRecordX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataErrorRecordX_t));
                    else
                    {
                        symName = "gdxDataErrorRecordX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxDataReadDone", 0, s, ref errBuf) == 0)
                    dll_gdxDataReadDone = d_gdxDataReadDone;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareaddone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadDone = (gdxDataReadDone_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadDone_t));
                    else
                    {
                        symName = "gdxDataReadDone"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 51, 4 };
                if (xcheck("gdxDataReadFilteredStart", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadFilteredStart = d_gdxDataReadFilteredStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadfilteredstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadFilteredStart = (gdxDataReadFilteredStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadFilteredStart_t));
                    else
                    {
                        symName = "gdxDataReadFilteredStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52, 54, 4 };
                if (xcheck("gdxDataReadMap", 4, s, ref errBuf) == 0)
                    dll_gdxDataReadMap = d_gdxDataReadMap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadmap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadMap = (gdxDataReadMap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadMap_t));
                    else
                    {
                        symName = "gdxDataReadMap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4 };
                if (xcheck("gdxDataReadMapStart", 2, s, ref errBuf) == 0)
                    dll_gdxDataReadMapStart = d_gdxDataReadMapStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadmapstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadMapStart = (gdxDataReadMapStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadMapStart_t));
                    else
                    {
                        symName = "gdxDataReadMapStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 52, 54, 4 };
                if (xcheck("gdxDataReadRaw", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadRaw = d_gdxDataReadRaw;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadraw");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadRaw = (gdxDataReadRaw_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadRaw_t));
                    else
                    {
                        symName = "gdxDataReadRaw"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 59, 4 };
                if (xcheck("gdxDataReadRawFast", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadRawFast = d_gdxDataReadRawFast;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadrawfast");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadRawFast = (gdxDataReadRawFast_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadRawFast_t));
                    else
                    {
                        symName = "gdxDataReadRawFast"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 55, 59 };
                if (xcheck("gdxDataReadRawFastFilt", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadRawFastFilt = d_gdxDataReadRawFastFilt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatareadrawfastfilt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadRawFastFilt = (gdxDataReadRawFastFilt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadRawFastFilt_t));
                    else
                    {
                        symName = "cgdxDataReadRawFastFilt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4 };
                if (xcheck("gdxDataReadRawStart", 2, s, ref errBuf) == 0)
                    dll_gdxDataReadRawStart = d_gdxDataReadRawStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadrawstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadRawStart = (gdxDataReadRawStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadRawStart_t));
                    else
                    {
                        symName = "gdxDataReadRawStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 55, 4, 59 };
                if (xcheck("gdxDataReadSlice", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadSlice = d_gdxDataReadSlice;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatareadslice");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadSlice = (gdxDataReadSlice_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadSlice_t));
                    else
                    {
                        symName = "cgdxDataReadSlice"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52 };
                if (xcheck("gdxDataReadSliceStart", 2, s, ref errBuf) == 0)
                    dll_gdxDataReadSliceStart = d_gdxDataReadSliceStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadslicestart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadSliceStart = (gdxDataReadSliceStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadSliceStart_t));
                    else
                    {
                        symName = "gdxDataReadSliceStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 56, 54, 4 };
                if (xcheck("gdxDataReadStr", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadStr = d_gdxDataReadStr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "bgdxdatareadstr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadStr = (gdxDataReadStr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadStr_t));
                    else
                    {
                        symName = "bgdxDataReadStr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4 };
                if (xcheck("gdxDataReadStrStart", 2, s, ref errBuf) == 0)
                    dll_gdxDataReadStrStart = d_gdxDataReadStrStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadstrstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadStrStart = (gdxDataReadStrStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadStrStart_t));
                    else
                    {
                        symName = "gdxDataReadStrStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 51, 56 };
                if (xcheck("gdxDataSliceUELS", 2, s, ref errBuf) == 0)
                    dll_gdxDataSliceUELS = d_gdxDataSliceUELS;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "bgdxdatasliceuels");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataSliceUELS = (gdxDataSliceUELS_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataSliceUELS_t));
                    else
                    {
                        symName = "bgdxDataSliceUELS"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxDataWriteDone", 0, s, ref errBuf) == 0)
                    dll_gdxDataWriteDone = d_gdxDataWriteDone;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatawritedone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteDone = (gdxDataWriteDone_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteDone_t));
                    else
                    {
                        symName = "gdxDataWriteDone"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 51, 53 };
                if (xcheck("gdxDataWriteMap", 2, s, ref errBuf) == 0)
                    dll_gdxDataWriteMap = d_gdxDataWriteMap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatawritemap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteMap = (gdxDataWriteMap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteMap_t));
                    else
                    {
                        symName = "gdxDataWriteMap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 3, 3, 3 };
                if (xcheck("gdxDataWriteMapStart", 5, s, ref errBuf) == 0)
                    dll_gdxDataWriteMapStart = d_gdxDataWriteMapStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatawritemapstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteMapStart = (gdxDataWriteMapStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteMapStart_t));
                    else
                    {
                        symName = "cgdxDataWriteMapStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 51, 53 };
                if (xcheck("gdxDataWriteRaw", 2, s, ref errBuf) == 0)
                    dll_gdxDataWriteRaw = d_gdxDataWriteRaw;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatawriteraw");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteRaw = (gdxDataWriteRaw_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteRaw_t));
                    else
                    {
                        symName = "gdxDataWriteRaw"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 3, 3, 3 };
                if (xcheck("gdxDataWriteRawStart", 5, s, ref errBuf) == 0)
                    dll_gdxDataWriteRawStart = d_gdxDataWriteRawStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatawriterawstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteRawStart = (gdxDataWriteRawStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteRawStart_t));
                    else
                    {
                        symName = "cgdxDataWriteRawStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 55, 53 };
                if (xcheck("gdxDataWriteStr", 2, s, ref errBuf) == 0)
                    dll_gdxDataWriteStr = d_gdxDataWriteStr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatawritestr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteStr = (gdxDataWriteStr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteStr_t));
                    else
                    {
                        symName = "cgdxDataWriteStr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 3, 3, 3 };
                if (xcheck("gdxDataWriteStrStart", 5, s, ref errBuf) == 0)
                    dll_gdxDataWriteStrStart = d_gdxDataWriteStrStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatawritestrstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteStrStart = (gdxDataWriteStrStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteStrStart_t));
                    else
                    {
                        symName = "cgdxDataWriteStrStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 12 };
                if (xcheck("gdxGetDLLVersion", 1, s, ref errBuf) == 0)
                    dll_gdxGetDLLVersion = d_gdxGetDLLVersion;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxgetdllversion");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetDLLVersion = (gdxGetDLLVersion_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetDLLVersion_t));
                    else
                    {
                        symName = "cgdxGetDLLVersion"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxErrorCount", 0, s, ref errBuf) == 0)
                    dll_gdxErrorCount = d_gdxErrorCount;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxerrorcount");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxErrorCount = (gdxErrorCount_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxErrorCount_t));
                    else
                    {
                        symName = "gdxErrorCount"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (xcheck("gdxErrorStr", 2, s, ref errBuf) == 0)
                    dll_gdxErrorStr = d_gdxErrorStr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxerrorstr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxErrorStr = (gdxErrorStr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxErrorStr_t));
                    else
                    {
                        symName = "cgdxErrorStr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 4, 4 };
                if (xcheck("gdxFileInfo", 2, s, ref errBuf) == 0)
                    dll_gdxFileInfo = d_gdxFileInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfileinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFileInfo = (gdxFileInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFileInfo_t));
                    else
                    {
                        symName = "gdxFileInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 12, 12 };
                if (xcheck("gdxFileVersion", 2, s, ref errBuf) == 0)
                    dll_gdxFileVersion = d_gdxFileVersion;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxfileversion");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFileVersion = (gdxFileVersion_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFileVersion_t));
                    else
                    {
                        symName = "cgdxFileVersion"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxFilterExists", 1, s, ref errBuf) == 0)
                    dll_gdxFilterExists = d_gdxFilterExists;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfilterexists");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFilterExists = (gdxFilterExists_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFilterExists_t));
                    else
                    {
                        symName = "gdxFilterExists"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxFilterRegister", 1, s, ref errBuf) == 0)
                    dll_gdxFilterRegister = d_gdxFilterRegister;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfilterregister");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFilterRegister = (gdxFilterRegister_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFilterRegister_t));
                    else
                    {
                        symName = "gdxFilterRegister"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxFilterRegisterDone", 0, s, ref errBuf) == 0)
                    dll_gdxFilterRegisterDone = d_gdxFilterRegisterDone;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfilterregisterdone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFilterRegisterDone = (gdxFilterRegisterDone_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFilterRegisterDone_t));
                    else
                    {
                        symName = "gdxFilterRegisterDone"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxFilterRegisterStart", 1, s, ref errBuf) == 0)
                    dll_gdxFilterRegisterStart = d_gdxFilterRegisterStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfilterregisterstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFilterRegisterStart = (gdxFilterRegisterStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFilterRegisterStart_t));
                    else
                    {
                        symName = "gdxFilterRegisterStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4 };
                if (xcheck("gdxFindSymbol", 2, s, ref errBuf) == 0)
                    dll_gdxFindSymbol = d_gdxFindSymbol;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxfindsymbol");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFindSymbol = (gdxFindSymbol_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFindSymbol_t));
                    else
                    {
                        symName = "cgdxFindSymbol"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12, 4 };
                if (xcheck("gdxGetElemText", 3, s, ref errBuf) == 0)
                    dll_gdxGetElemText = d_gdxGetElemText;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxgetelemtext");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetElemText = (gdxGetElemText_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetElemText_t));
                    else
                    {
                        symName = "cgdxGetElemText"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxGetLastError", 0, s, ref errBuf) == 0)
                    dll_gdxGetLastError = d_gdxGetLastError;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxgetlasterror");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetLastError = (gdxGetLastError_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetLastError_t));
                    else
                    {
                        symName = "gdxGetLastError"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 23 };
                if (xcheck("gdxGetMemoryUsed", 0, s, ref errBuf) == 0)
                    dll_gdxGetMemoryUsed = d_gdxGetMemoryUsed;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxgetmemoryused");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetMemoryUsed = (gdxGetMemoryUsed_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetMemoryUsed_t));
                    else
                    {
                        symName = "gdxGetMemoryUsed"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 58 };
                if (xcheck("gdxGetSpecialValues", 1, s, ref errBuf) == 0)
                    dll_gdxGetSpecialValues = d_gdxGetSpecialValues;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxgetspecialvalues");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetSpecialValues = (gdxGetSpecialValues_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetSpecialValues_t));
                    else
                    {
                        symName = "gdxGetSpecialValues"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (xcheck("gdxGetUEL", 2, s, ref errBuf) == 0)
                    dll_gdxGetUEL = d_gdxGetUEL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxgetuel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetUEL = (gdxGetUEL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetUEL_t));
                    else
                    {
                        symName = "cgdxGetUEL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 13, 4 };
                if (xcheck("gdxMapValue", 2, s, ref errBuf) == 0)
                    dll_gdxMapValue = d_gdxMapValue;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxmapvalue");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxMapValue = (gdxMapValue_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxMapValue_t));
                    else
                    {
                        symName = "gdxMapValue"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 4 };
                if (xcheck("gdxOpenAppend", 3, s, ref errBuf) == 0)
                    dll_gdxOpenAppend = d_gdxOpenAppend;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxopenappend");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxOpenAppend = (gdxOpenAppend_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxOpenAppend_t));
                    else
                    {
                        symName = "cgdxOpenAppend"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4 };
                if (xcheck("gdxOpenRead", 2, s, ref errBuf) == 0)
                    dll_gdxOpenRead = d_gdxOpenRead;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxopenread");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxOpenRead = (gdxOpenRead_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxOpenRead_t));
                    else
                    {
                        symName = "cgdxOpenRead"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 4 };
                if (xcheck("gdxOpenWrite", 3, s, ref errBuf) == 0)
                    dll_gdxOpenWrite = d_gdxOpenWrite;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxopenwrite");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxOpenWrite = (gdxOpenWrite_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxOpenWrite_t));
                    else
                    {
                        symName = "cgdxOpenWrite"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 3, 4 };
                if (xcheck("gdxOpenWriteEx", 4, s, ref errBuf) == 0)
                    dll_gdxOpenWriteEx = d_gdxOpenWriteEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxopenwriteex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxOpenWriteEx = (gdxOpenWriteEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxOpenWriteEx_t));
                    else
                    {
                        symName = "cgdxOpenWriteEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxResetSpecialValues", 0, s, ref errBuf) == 0)
                    dll_gdxResetSpecialValues = d_gdxResetSpecialValues;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxresetspecialvalues");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxResetSpecialValues = (gdxResetSpecialValues_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxResetSpecialValues_t));
                    else
                    {
                        symName = "gdxResetSpecialValues"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxSetHasText", 1, s, ref errBuf) == 0)
                    dll_gdxSetHasText = d_gdxSetHasText;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsethastext");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetHasText = (gdxSetHasText_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetHasText_t));
                    else
                    {
                        symName = "gdxSetHasText"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 57 };
                if (xcheck("gdxSetReadSpecialValues", 1, s, ref errBuf) == 0)
                    dll_gdxSetReadSpecialValues = d_gdxSetReadSpecialValues;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsetreadspecialvalues");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetReadSpecialValues = (gdxSetReadSpecialValues_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetReadSpecialValues_t));
                    else
                    {
                        symName = "gdxSetReadSpecialValues"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 57 };
                if (xcheck("gdxSetSpecialValues", 1, s, ref errBuf) == 0)
                    dll_gdxSetSpecialValues = d_gdxSetSpecialValues;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsetspecialvalues");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetSpecialValues = (gdxSetSpecialValues_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetSpecialValues_t));
                    else
                    {
                        symName = "gdxSetSpecialValues"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 3 };
                if (xcheck("gdxSetTextNodeNr", 2, s, ref errBuf) == 0)
                    dll_gdxSetTextNodeNr = d_gdxSetTextNodeNr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsettextnodenr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetTextNodeNr = (gdxSetTextNodeNr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetTextNodeNr_t));
                    else
                    {
                        symName = "gdxSetTextNodeNr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 11 };
                if (xcheck("gdxSetTraceLevel", 2, s, ref errBuf) == 0)
                    dll_gdxSetTraceLevel = d_gdxSetTraceLevel;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsettracelevel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetTraceLevel = (gdxSetTraceLevel_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetTraceLevel_t));
                    else
                    {
                        symName = "cgdxSetTraceLevel"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52 };
                if (xcheck("gdxSymbIndxMaxLength", 2, s, ref errBuf) == 0)
                    dll_gdxSymbIndxMaxLength = d_gdxSymbIndxMaxLength;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsymbindxmaxlength");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbIndxMaxLength = (gdxSymbIndxMaxLength_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbIndxMaxLength_t));
                    else
                    {
                        symName = "gdxSymbIndxMaxLength"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxSymbMaxLength", 0, s, ref errBuf) == 0)
                    dll_gdxSymbMaxLength = d_gdxSymbMaxLength;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsymbmaxlength");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbMaxLength = (gdxSymbMaxLength_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbMaxLength_t));
                    else
                    {
                        symName = "gdxSymbMaxLength"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 11 };
                if (xcheck("gdxSymbolAddComment", 2, s, ref errBuf) == 0)
                    dll_gdxSymbolAddComment = d_gdxSymbolAddComment;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymboladdcomment");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolAddComment = (gdxSymbolAddComment_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolAddComment_t));
                    else
                    {
                        symName = "cgdxSymbolAddComment"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 3, 12 };
                if (xcheck("gdxSymbolGetComment", 3, s, ref errBuf) == 0)
                    dll_gdxSymbolGetComment = d_gdxSymbolGetComment;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolgetcomment");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolGetComment = (gdxSymbolGetComment_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolGetComment_t));
                    else
                    {
                        symName = "cgdxSymbolGetComment"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52 };
                if (xcheck("gdxSymbolGetDomain", 2, s, ref errBuf) == 0)
                    dll_gdxSymbolGetDomain = d_gdxSymbolGetDomain;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsymbolgetdomain");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolGetDomain = (gdxSymbolGetDomain_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolGetDomain_t));
                    else
                    {
                        symName = "gdxSymbolGetDomain"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 56 };
                if (xcheck("gdxSymbolGetDomainX", 2, s, ref errBuf) == 0)
                    dll_gdxSymbolGetDomainX = d_gdxSymbolGetDomainX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "bgdxsymbolgetdomainx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolGetDomainX = (gdxSymbolGetDomainX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolGetDomainX_t));
                    else
                    {
                        symName = "bgdxSymbolGetDomainX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxSymbolDim", 1, s, ref errBuf) == 0)
                    dll_gdxSymbolDim = d_gdxSymbolDim;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsymboldim");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolDim = (gdxSymbolDim_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolDim_t));
                    else
                    {
                        symName = "gdxSymbolDim"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12, 4, 4 };
                if (xcheck("gdxSymbolInfo", 4, s, ref errBuf) == 0)
                    dll_gdxSymbolInfo = d_gdxSymbolInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolInfo = (gdxSymbolInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolInfo_t));
                    else
                    {
                        symName = "cgdxSymbolInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4, 4, 12 };
                if (xcheck("gdxSymbolInfoX", 4, s, ref errBuf) == 0)
                    dll_gdxSymbolInfoX = d_gdxSymbolInfoX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolinfox");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolInfoX = (gdxSymbolInfoX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolInfoX_t));
                    else
                    {
                        symName = "cgdxSymbolInfoX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 55 };
                if (xcheck("gdxSymbolSetDomain", 1, s, ref errBuf) == 0)
                    dll_gdxSymbolSetDomain = d_gdxSymbolSetDomain;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolsetdomain");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolSetDomain = (gdxSymbolSetDomain_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolSetDomain_t));
                    else
                    {
                        symName = "cgdxSymbolSetDomain"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 55 };
                if (xcheck("gdxSymbolSetDomainX", 2, s, ref errBuf) == 0)
                    dll_gdxSymbolSetDomainX = d_gdxSymbolSetDomainX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolsetdomainx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolSetDomainX = (gdxSymbolSetDomainX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolSetDomainX_t));
                    else
                    {
                        symName = "cgdxSymbolSetDomainX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 4, 4 };
                if (xcheck("gdxSystemInfo", 2, s, ref errBuf) == 0)
                    dll_gdxSystemInfo = d_gdxSystemInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsysteminfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSystemInfo = (gdxSystemInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSystemInfo_t));
                    else
                    {
                        symName = "gdxSystemInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELMaxLength", 0, s, ref errBuf) == 0)
                    dll_gdxUELMaxLength = d_gdxUELMaxLength;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelmaxlength");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELMaxLength = (gdxUELMaxLength_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELMaxLength_t));
                    else
                    {
                        symName = "gdxUELMaxLength"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELRegisterDone", 0, s, ref errBuf) == 0)
                    dll_gdxUELRegisterDone = d_gdxUELRegisterDone;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelregisterdone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterDone = (gdxUELRegisterDone_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterDone_t));
                    else
                    {
                        symName = "gdxUELRegisterDone"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 11 };
                if (xcheck("gdxUELRegisterMap", 2, s, ref errBuf) == 0)
                    dll_gdxUELRegisterMap = d_gdxUELRegisterMap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxuelregistermap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterMap = (gdxUELRegisterMap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterMap_t));
                    else
                    {
                        symName = "cgdxUELRegisterMap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELRegisterMapStart", 0, s, ref errBuf) == 0)
                    dll_gdxUELRegisterMapStart = d_gdxUELRegisterMapStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelregistermapstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterMapStart = (gdxUELRegisterMapStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterMapStart_t));
                    else
                    {
                        symName = "gdxUELRegisterMapStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11 };
                if (xcheck("gdxUELRegisterRaw", 1, s, ref errBuf) == 0)
                    dll_gdxUELRegisterRaw = d_gdxUELRegisterRaw;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxuelregisterraw");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterRaw = (gdxUELRegisterRaw_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterRaw_t));
                    else
                    {
                        symName = "cgdxUELRegisterRaw"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELRegisterRawStart", 0, s, ref errBuf) == 0)
                    dll_gdxUELRegisterRawStart = d_gdxUELRegisterRawStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelregisterrawstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterRawStart = (gdxUELRegisterRawStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterRawStart_t));
                    else
                    {
                        symName = "gdxUELRegisterRawStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4 };
                if (xcheck("gdxUELRegisterStr", 2, s, ref errBuf) == 0)
                    dll_gdxUELRegisterStr = d_gdxUELRegisterStr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxuelregisterstr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterStr = (gdxUELRegisterStr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterStr_t));
                    else
                    {
                        symName = "cgdxUELRegisterStr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELRegisterStrStart", 0, s, ref errBuf) == 0)
                    dll_gdxUELRegisterStrStart = d_gdxUELRegisterStrStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelregisterstrstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterStrStart = (gdxUELRegisterStrStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterStrStart_t));
                    else
                    {
                        symName = "gdxUELRegisterStrStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4, 4 };
                if (xcheck("gdxUMFindUEL", 3, s, ref errBuf) == 0)
                    dll_gdxUMFindUEL = d_gdxUMFindUEL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxumfinduel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUMFindUEL = (gdxUMFindUEL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUMFindUEL_t));
                    else
                    {
                        symName = "cgdxUMFindUEL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12, 4 };
                if (xcheck("gdxUMUelGet", 3, s, ref errBuf) == 0)
                    dll_gdxUMUelGet = d_gdxUMUelGet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxumuelget");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUMUelGet = (gdxUMUelGet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUMUelGet_t));
                    else
                    {
                        symName = "cgdxUMUelGet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 4, 4 };
                if (xcheck("gdxUMUelInfo", 2, s, ref errBuf) == 0)
                    dll_gdxUMUelInfo = d_gdxUMUelInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxumuelinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUMUelInfo = (gdxUMUelInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUMUelInfo_t));
                    else
                    {
                        symName = "gdxUMUelInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 3, 3, 59, 4, 1 };
                if (xcheck("gdxGetDomainElements", 6, s, ref errBuf) == 0)
                    dll_gdxGetDomainElements = d_gdxGetDomainElements;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxgetdomainelements");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetDomainElements = (gdxGetDomainElements_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetDomainElements_t));
                    else
                    {
                        symName = "gdxGetDomainElements"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxCurrentDim", 0, s, ref errBuf) == 0)
                    dll_gdxCurrentDim = d_gdxCurrentDim;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxcurrentdim");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxCurrentDim = (gdxCurrentDim_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxCurrentDim_t));
                    else
                    {
                        symName = "gdxCurrentDim"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11 };
                if (xcheck("gdxRenameUEL", 2, s, ref errBuf) == 0)
                    dll_gdxRenameUEL = d_gdxRenameUEL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxrenameuel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxRenameUEL = (gdxRenameUEL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxRenameUEL_t));
                    else
                    {
                        symName = "cgdxRenameUEL"; goto symMissing;
                    }
                }
            }

            return true;

            symMissing:
            errBuf = "Could not load symbol '" + symName + "'";
            return false;

        } /* XLibraryLoad */

        private bool libloader(string dllPath, string dllName, ref string msgBuf)
        {
#if __MonoCS__
#if __APPLE__
        const string libStem = "libgdxdclib", libExt = ".dylib";
#else
        const string libStem = "libgdxdclib", libExt = ".so";
#endif
#else
            const string libStem = "gdxdclib", libExt = ".dll";
#endif
            string dllNameBuf = string.Empty;
            int myrc = 0;
            string GMS_DLL_SUFFIX = string.Empty;

            msgBuf = string.Empty;
            if (!isLoaded)
            {
                if (string.Empty != dllPath)
                {
                    dllNameBuf = dllPath;
                    if (Path.DirectorySeparatorChar != dllNameBuf[dllNameBuf.Length - 1]) dllNameBuf = dllNameBuf + Path.DirectorySeparatorChar;
                }
                if (string.Empty != dllName)
                    dllNameBuf = dllNameBuf + dllName;
                else
                {
                    if (8 == IntPtr.Size)
                        GMS_DLL_SUFFIX = "64";
                    dllNameBuf = dllNameBuf + libStem + GMS_DLL_SUFFIX + libExt;
                }
                isLoaded = XLibraryLoad(dllNameBuf, ref msgBuf);
                if (isLoaded)
                {
                    if (null != dll_gdxSetLoadPath && string.Empty != dllPath)
                    {
                        gdxSetLoadPath(dllPath);
                    }
                    else
                    {                            /* no setLoadPath call found */
                        myrc |= 2;
                    }
                }
                else                          /* library load failed */
                    myrc |= 1;
            }
            return (myrc & 1) == 0;
        } /* libloader */

        public bool gdxGetReady(ref string msgBuf)
        {
            return libloader(string.Empty, string.Empty, ref msgBuf);
        }
        public bool gdxGetReadyD(string dirName, ref string msgBuf)
        {
            return libloader(dirName, string.Empty, ref msgBuf);
        }
        public bool gdxGetReadyL(string dirName, string libName, ref string msgBuf)
        {
            return libloader(dirName, libName, ref msgBuf);
        }

        public GdxFast(ref string msgBuf)
        {
            bool gdxIsReady;

            extHandle = false;
            _disposed = false;
            gdxIsReady = gdxGetReady(ref msgBuf);
            if (!gdxIsReady) return;
            xcreate(ref pgdx);
            if (pgdx != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public GdxFast(string dirName, ref string msgBuf)
        {
            bool gdxIsReady;

            extHandle = false;
            _disposed = false;
            gdxIsReady = gdxGetReadyD(dirName, ref msgBuf);
            if (!gdxIsReady) return;
            xcreate(ref pgdx);
            if (pgdx != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public GdxFast(string dirName, string libName, ref string msgBuf)
        {
            bool gdxIsReady;

            extHandle = false;
            _disposed = false;
            gdxIsReady = gdxGetReadyL(dirName, libName, ref msgBuf);
            if (!gdxIsReady) return;
            xcreate(ref pgdx);
            if (pgdx != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public GdxFast(IntPtr gdxHandle, ref string msgBuf)
        {
            bool gdxIsReady;

            if (gdxHandle == IntPtr.Zero)
            {
                msgBuf = "gdxHandle is empty";
                return;
            }
            extHandle = true;
            _disposed = false;
            gdxIsReady = gdxGetReady(ref msgBuf);
            if (!gdxIsReady) return;
            pgdx = gdxHandle;
        }
        public GdxFast(IntPtr gdxHandle, string dirName, ref string msgBuf)
        {
            bool gdxIsReady;

            if (gdxHandle == IntPtr.Zero)
            {
                msgBuf = "gdxHandle is empty";
                return;
            }
            extHandle = true;
            _disposed = false;
            gdxIsReady = gdxGetReadyD(dirName, ref msgBuf);
            if (!gdxIsReady) return;
            pgdx = gdxHandle;
        }

        ~GdxFast()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (pgdx != IntPtr.Zero)
                        gdxFree();
                }
                // Indicate that the instance has been disposed.
                _disposed = true;
            }
            GC.KeepAlive(this);
        }

        public int gdxFree()
        {
            if (!extHandle && pgdx != IntPtr.Zero) xfree(ref pgdx);
            return 1;
        }

        public bool gdxLibraryUnload()
        {
            return FreeLibrary(h);
        }

        public IntPtr GetgdxPtr()
        {
            return pgdx;
        }

        public bool gdxGetScreenIndicator()
        {
            return ScreenIndicator;
        }

        public void gdxSetScreenIndicator(bool scrind)
        {
            ScreenIndicator = scrind;
        }

        public bool gdxGetExceptionIndicator()
        {
            return ExceptionIndicator;
        }

        public void gdxSetExceptionIndicator(bool excind)
        {
            ExceptionIndicator = excind;
        }

        public bool gdxGetExitIndicator()
        {
            return ExitIndicator;
        }

        public void gdxSetExitIndicator(bool extind)
        {
            ExitIndicator = extind;
        }

        public gdxErrorCallback_t gdxGetErrorCallback()
        {
            return ErrorCallBack;
        }

        public void gdxSetErrorCallback(gdxErrorCallback_t func)
        {
            ErrorCallBack = func;
        }

        public int gdxGetAPIErrorCount()
        {
            return APIErrorCount;
        }

        public void gdxSetAPIErrorCount(int ecnt)
        {
            APIErrorCount = ecnt;
        }

        private static void gdxErrorHandling(string Msg)
        {
            APIErrorCount++;
            if (ScreenIndicator) Console.WriteLine(Msg);
            if (ErrorCallBack != null)
                if (ErrorCallBack(APIErrorCount, Msg)) Environment.Exit(123);
            if (ExceptionIndicator) throw new ArgumentNullException();
            if (ExitIndicator) Environment.Exit(123);
        }

        private void ConvertC2CS(byte[] b, ref string s)
        {
            int i;
            s = "";
            i = 0;
            while (b[i] != 0)
            {
                s = s + (char)(b[i]);
                i = i + 1;
            }
        }

        private void ConvertArrayC2CS(byte[,] b, ref string s, int k)
        {
            int i;
            s = "";
            i = 0;
            while (b[k, i] != 0)
            {
                s = s + (char)(b[k, i]);
                i = i + 1;
            }
        }

        private int xapiversion(int api, ref string msg, ref int cl)
        {
            int rc_xapiversion;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_xapiversion = dll_xapiversion(api, cpy_msg, ref cl);
            msg = cpy_msg.ToString();
            return rc_xapiversion;
        }

        private int xcheck(string ep, int nargs, int[] s, ref string msg)
        {
            int rc_xcheck;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_xcheck = dll_xcheck(ep, nargs, s, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_xcheck;
        }

        public void gdxSetLoadPath(string s)
        {
            dll_gdxSetLoadPath(s);
        }

        public void gdxGetLoadPath(ref string s)
        {
            byte[] cpy_s = new byte[gamsglobals.str_len];
            dll_gdxGetLoadPath(ref cpy_s[0]);
            ConvertC2CS(cpy_s, ref s);
        }

        public int gdxAcronymAdd(string AName, string Txt, int AIndx)
        {
            return dll_gdxAcronymAdd(pgdx, AName, Txt, AIndx);
        }

        public int gdxAcronymCount()
        {
            return dll_gdxAcronymCount(pgdx);
        }

        public int gdxAcronymGetInfo(int N, ref string AName, ref string Txt, ref int AIndx)
        {
            int rc_gdxAcronymGetInfo;
            StringBuilder cpy_AName = new StringBuilder(gamsglobals.str_len);
            StringBuilder cpy_Txt = new StringBuilder(gamsglobals.str_len);
            rc_gdxAcronymGetInfo = dll_gdxAcronymGetInfo(pgdx, N, cpy_AName, cpy_Txt, ref AIndx);
            AName = cpy_AName.ToString();
            Txt = cpy_Txt.ToString();
            return rc_gdxAcronymGetInfo;
        }

        public int gdxAcronymGetMapping(int N, ref int orgIndx, ref int newIndx, ref int autoIndex)
        {
            return dll_gdxAcronymGetMapping(pgdx, N, ref orgIndx, ref newIndx, ref autoIndex);
        }

        public int gdxAcronymIndex(double V)
        {
            return dll_gdxAcronymIndex(pgdx, V);
        }

        public int gdxAcronymName(double V, ref string AName)
        {
            int rc_gdxAcronymName;
            StringBuilder cpy_AName = new StringBuilder(gamsglobals.str_len);
            rc_gdxAcronymName = dll_gdxAcronymName(pgdx, V, cpy_AName);
            AName = cpy_AName.ToString();
            return rc_gdxAcronymName;
        }

        public int gdxAcronymNextNr(int NV)
        {
            return dll_gdxAcronymNextNr(pgdx, NV);
        }

        public int gdxAcronymSetInfo(int N, string AName, string Txt, int AIndx)
        {
            return dll_gdxAcronymSetInfo(pgdx, N, AName, Txt, AIndx);
        }

        public double gdxAcronymValue(int AIndx)
        {
            return dll_gdxAcronymValue(pgdx, AIndx);
        }

        public int gdxAddAlias(string Id1, string Id2)
        {
            return dll_gdxAddAlias(pgdx, Id1, Id2);
        }

        public int gdxAddSetText(string Txt, ref int TxtNr)
        {
            return dll_gdxAddSetText(pgdx, Txt, ref TxtNr);
        }

        public int gdxAutoConvert(int NV)
        {
            return dll_gdxAutoConvert(pgdx, NV);
        }

        public int gdxClose()
        {
            return dll_gdxClose(pgdx);
        }

        public int gdxDataErrorCount()
        {
            return dll_gdxDataErrorCount(pgdx);
        }

        public int gdxDataErrorRecord(int RecNr, ref int[] KeyInt, ref double[] Values)
        {
            return dll_gdxDataErrorRecord(pgdx, RecNr, KeyInt, Values);
        }

        public int gdxDataErrorRecordX(int RecNr, ref int[] KeyInt, ref double[] Values)
        {
            return dll_gdxDataErrorRecordX(pgdx, RecNr, KeyInt, Values);
        }

        public int gdxDataReadDone()
        {
            return dll_gdxDataReadDone(pgdx);
        }

        public int gdxDataReadFilteredStart(int SyNr, int[] FilterAction, ref int NrRecs)
        {
            return dll_gdxDataReadFilteredStart(pgdx, SyNr, FilterAction, ref NrRecs);
        }

        public int gdxDataReadMap(int RecNr, ref int[] KeyInt, ref double[] Values, ref int DimFrst)
        {
            return dll_gdxDataReadMap(pgdx, RecNr, KeyInt, Values, ref DimFrst);
        }

        public int gdxDataReadMapStart(int SyNr, ref int NrRecs)
        {
            return dll_gdxDataReadMapStart(pgdx, SyNr, ref NrRecs);
        }

        public int gdxDataReadRaw(ref int[] KeyInt, ref double[] Values, ref int DimFrst)
        {
            return dll_gdxDataReadRaw(pgdx, KeyInt, Values, ref DimFrst);
        }

        public int gdxDataReadRawFast(int SyNr, TDataStoreProc DP, ref int NrRecs)
        {
            return dll_gdxDataReadRawFast(pgdx, SyNr, DP, ref NrRecs);
        }

        public int gdxDataReadRawFastFilt(int SyNr, string[] UelFilterStr, TDataStoreFiltProc DP)
        {
            return dll_gdxDataReadRawFastFilt(pgdx, SyNr, UelFilterStr, DP);
        }

        public int gdxDataReadRawStart(int SyNr, ref int NrRecs)
        {
            return dll_gdxDataReadRawStart(pgdx, SyNr, ref NrRecs);
        }

        public int gdxDataReadSlice(string[] UelFilterStr, ref int Dimen, TDataStoreProc DP)
        {
            return dll_gdxDataReadSlice(pgdx, UelFilterStr, ref Dimen, DP);
        }

        public int gdxDataReadSliceStart(int SyNr, ref int[] ElemCounts)
        {
            return dll_gdxDataReadSliceStart(pgdx, SyNr, ElemCounts);
        }

        public int gdxDataReadStr(ref string[] KeyStr, ref double[] Values, ref int DimFrst)
        {
            int rc_gdxDataReadStr;
            byte[,] cpy_KeyStr = new byte[gamsglobals.maxdim, gamsglobals.str_len];
            int i_KeyStr;
            int sidim_KeyStr;
            rc_gdxDataReadStr = dll_gdxDataReadStr(pgdx, cpy_KeyStr, Values, ref DimFrst);
            sidim_KeyStr = dll_gdxCurrentDim(pgdx);
            if (rc_gdxDataReadStr != 0)
                for (i_KeyStr = 0; i_KeyStr < sidim_KeyStr; i_KeyStr++)
                    ConvertArrayC2CS(cpy_KeyStr, ref KeyStr[i_KeyStr], i_KeyStr);
            return rc_gdxDataReadStr;
        }

        public int gdxDataReadStrStart(int SyNr, ref int NrRecs)
        {
            return dll_gdxDataReadStrStart(pgdx, SyNr, ref NrRecs);
        }

        public int gdxDataSliceUELS(int[] SliceKeyInt, ref string[] KeyStr)
        {
            int rc_gdxDataSliceUELS;
            byte[,] cpy_KeyStr = new byte[gamsglobals.maxdim, gamsglobals.str_len];
            int i_KeyStr;
            int sidim_KeyStr;
            rc_gdxDataSliceUELS = dll_gdxDataSliceUELS(pgdx, SliceKeyInt, cpy_KeyStr);
            sidim_KeyStr = dll_gdxCurrentDim(pgdx);
            if (rc_gdxDataSliceUELS != 0)
                for (i_KeyStr = 0; i_KeyStr < sidim_KeyStr; i_KeyStr++)
                    ConvertArrayC2CS(cpy_KeyStr, ref KeyStr[i_KeyStr], i_KeyStr);
            return rc_gdxDataSliceUELS;
        }

        public int gdxDataWriteDone()
        {
            return dll_gdxDataWriteDone(pgdx);
        }

        public int gdxDataWriteMap(int[] KeyInt, double[] Values)
        {
            return dll_gdxDataWriteMap(pgdx, KeyInt, Values);
        }

        public int gdxDataWriteMapStart(string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        {
            return dll_gdxDataWriteMapStart(pgdx, SyId, ExplTxt, Dimen, Typ, UserInfo);
        }

        public int gdxDataWriteRaw(int[] KeyInt, double[] Values)
        {
            return dll_gdxDataWriteRaw(pgdx, KeyInt, Values);
        }

        public int gdxDataWriteRawStart(string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        {
            return dll_gdxDataWriteRawStart(pgdx, SyId, ExplTxt, Dimen, Typ, UserInfo);
        }

        public int gdxDataWriteStr(string[] KeyStr, double[] Values)
        {
            return dll_gdxDataWriteStr(pgdx, KeyStr, Values);
        }

        public int gdxDataWriteStrStart(string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        {
            return dll_gdxDataWriteStrStart(pgdx, SyId, ExplTxt, Dimen, Typ, UserInfo);
        }

        public int gdxGetDLLVersion(ref string V)
        {
            int rc_gdxGetDLLVersion;
            StringBuilder cpy_V = new StringBuilder(gamsglobals.str_len);
            rc_gdxGetDLLVersion = dll_gdxGetDLLVersion(pgdx, cpy_V);
            V = cpy_V.ToString();
            return rc_gdxGetDLLVersion;
        }

        public int gdxErrorCount()
        {
            return dll_gdxErrorCount(pgdx);
        }

        public int gdxErrorStr(int ErrNr, ref string ErrMsg)
        {
            int rc_gdxErrorStr;
            StringBuilder cpy_ErrMsg = new StringBuilder(gamsglobals.str_len);
            rc_gdxErrorStr = dll_gdxErrorStr(pgdx, ErrNr, cpy_ErrMsg);
            ErrMsg = cpy_ErrMsg.ToString();
            return rc_gdxErrorStr;
        }

        public int gdxFileInfo(ref int FileVer, ref int ComprLev)
        {
            return dll_gdxFileInfo(pgdx, ref FileVer, ref ComprLev);
        }

        public int gdxFileVersion(ref string FileStr, ref string ProduceStr)
        {
            int rc_gdxFileVersion;
            StringBuilder cpy_FileStr = new StringBuilder(gamsglobals.str_len);
            StringBuilder cpy_ProduceStr = new StringBuilder(gamsglobals.str_len);
            rc_gdxFileVersion = dll_gdxFileVersion(pgdx, cpy_FileStr, cpy_ProduceStr);
            FileStr = cpy_FileStr.ToString();
            ProduceStr = cpy_ProduceStr.ToString();
            return rc_gdxFileVersion;
        }

        public int gdxFilterExists(int FilterNr)
        {
            return dll_gdxFilterExists(pgdx, FilterNr);
        }

        public int gdxFilterRegister(int UelMap)
        {
            return dll_gdxFilterRegister(pgdx, UelMap);
        }

        public int gdxFilterRegisterDone()
        {
            return dll_gdxFilterRegisterDone(pgdx);
        }

        public int gdxFilterRegisterStart(int FilterNr)
        {
            return dll_gdxFilterRegisterStart(pgdx, FilterNr);
        }

        public int gdxFindSymbol(string SyId, ref int SyNr)
        {
            return dll_gdxFindSymbol(pgdx, SyId, ref SyNr);
        }

        public int gdxGetElemText(int TxtNr, ref string Txt, ref int Node)
        {
            int rc_gdxGetElemText;
            StringBuilder cpy_Txt = new StringBuilder(gamsglobals.str_len);
            rc_gdxGetElemText = dll_gdxGetElemText(pgdx, TxtNr, cpy_Txt, ref Node);
            Txt = cpy_Txt.ToString();
            return rc_gdxGetElemText;
        }

        public int gdxGetLastError()
        {
            return dll_gdxGetLastError(pgdx);
        }

        public Int64 gdxGetMemoryUsed()
        {
            return dll_gdxGetMemoryUsed(pgdx);
        }

        public int gdxGetSpecialValues(ref double[] AVals)
        {
            return dll_gdxGetSpecialValues(pgdx, AVals);
        }

        public int gdxGetUEL(int UelNr, ref string Uel)
        {
            int rc_gdxGetUEL;
            StringBuilder cpy_Uel = new StringBuilder(gamsglobals.str_len);
            rc_gdxGetUEL = dll_gdxGetUEL(pgdx, UelNr, cpy_Uel);
            Uel = cpy_Uel.ToString();
            return rc_gdxGetUEL;
        }

        public int gdxMapValue(double D, ref int sv)
        {
            return dll_gdxMapValue(pgdx, D, ref sv);
        }

        public int gdxOpenAppend(string FileName, string Producer, ref int ErrNr)
        {
            return dll_gdxOpenAppend(pgdx, FileName, Producer, ref ErrNr);
        }

        public int gdxOpenRead(string FileName, ref int ErrNr)
        {
            return dll_gdxOpenRead(pgdx, FileName, ref ErrNr);
        }

        public int gdxOpenWrite(string FileName, string Producer, ref int ErrNr)
        {
            return dll_gdxOpenWrite(pgdx, FileName, Producer, ref ErrNr);
        }

        public int gdxOpenWriteEx(string FileName, string Producer, int Compr, ref int ErrNr)
        {
            return dll_gdxOpenWriteEx(pgdx, FileName, Producer, Compr, ref ErrNr);
        }

        public int gdxResetSpecialValues()
        {
            return dll_gdxResetSpecialValues(pgdx);
        }

        public int gdxSetHasText(int SyNr)
        {
            return dll_gdxSetHasText(pgdx, SyNr);
        }

        public int gdxSetReadSpecialValues(double[] AVals)
        {
            return dll_gdxSetReadSpecialValues(pgdx, AVals);
        }

        public int gdxSetSpecialValues(double[] AVals)
        {
            return dll_gdxSetSpecialValues(pgdx, AVals);
        }

        public int gdxSetTextNodeNr(int TxtNr, int Node)
        {
            return dll_gdxSetTextNodeNr(pgdx, TxtNr, Node);
        }

        public int gdxSetTraceLevel(int N, string s)
        {
            return dll_gdxSetTraceLevel(pgdx, N, s);
        }

        public int gdxSymbIndxMaxLength(int SyNr, ref int[] LengthInfo)
        {
            return dll_gdxSymbIndxMaxLength(pgdx, SyNr, LengthInfo);
        }

        public int gdxSymbMaxLength()
        {
            return dll_gdxSymbMaxLength(pgdx);
        }

        public int gdxSymbolAddComment(int SyNr, string Txt)
        {
            return dll_gdxSymbolAddComment(pgdx, SyNr, Txt);
        }

        public int gdxSymbolGetComment(int SyNr, int N, ref string Txt)
        {
            int rc_gdxSymbolGetComment;
            StringBuilder cpy_Txt = new StringBuilder(gamsglobals.str_len);
            rc_gdxSymbolGetComment = dll_gdxSymbolGetComment(pgdx, SyNr, N, cpy_Txt);
            Txt = cpy_Txt.ToString();
            return rc_gdxSymbolGetComment;
        }

        public int gdxSymbolGetDomain(int SyNr, ref int[] DomainSyNrs)
        {
            return dll_gdxSymbolGetDomain(pgdx, SyNr, DomainSyNrs);
        }

        public int gdxSymbolGetDomainX(int SyNr, ref string[] DomainIDs)
        {
            int rc_gdxSymbolGetDomainX;
            byte[,] cpy_DomainIDs = new byte[gamsglobals.maxdim, gamsglobals.str_len];
            int i_DomainIDs;
            int sidim_DomainIDs;
            rc_gdxSymbolGetDomainX = dll_gdxSymbolGetDomainX(pgdx, SyNr, cpy_DomainIDs);
            sidim_DomainIDs = dll_gdxSymbolDim(pgdx, SyNr);
            if (rc_gdxSymbolGetDomainX != 0)
                for (i_DomainIDs = 0; i_DomainIDs < sidim_DomainIDs; i_DomainIDs++)
                    ConvertArrayC2CS(cpy_DomainIDs, ref DomainIDs[i_DomainIDs], i_DomainIDs);
            return rc_gdxSymbolGetDomainX;
        }

        public int gdxSymbolDim(int SyNr)
        {
            return dll_gdxSymbolDim(pgdx, SyNr);
        }

        public int gdxSymbolInfo(int SyNr, ref string SyId, ref int Dimen, ref int Typ)
        {
            int rc_gdxSymbolInfo;
            StringBuilder cpy_SyId = new StringBuilder(gamsglobals.str_len);
            rc_gdxSymbolInfo = dll_gdxSymbolInfo(pgdx, SyNr, cpy_SyId, ref Dimen, ref Typ);
            SyId = cpy_SyId.ToString();
            return rc_gdxSymbolInfo;
        }

        public int gdxSymbolInfoX(int SyNr, ref int RecCnt, ref int UserInfo, ref string ExplTxt)
        {
            int rc_gdxSymbolInfoX;
            StringBuilder cpy_ExplTxt = new StringBuilder(gamsglobals.str_len);
            rc_gdxSymbolInfoX = dll_gdxSymbolInfoX(pgdx, SyNr, ref RecCnt, ref UserInfo, cpy_ExplTxt);
            ExplTxt = cpy_ExplTxt.ToString();
            return rc_gdxSymbolInfoX;
        }

        public int gdxSymbolSetDomain(string[] DomainIDs)
        {
            return dll_gdxSymbolSetDomain(pgdx, DomainIDs);
        }

        public int gdxSymbolSetDomainX(int SyNr, string[] DomainIDs)
        {
            return dll_gdxSymbolSetDomainX(pgdx, SyNr, DomainIDs);
        }

        public int gdxSystemInfo(ref int SyCnt, ref int UelCnt)
        {
            return dll_gdxSystemInfo(pgdx, ref SyCnt, ref UelCnt);
        }

        public int gdxUELMaxLength()
        {
            return dll_gdxUELMaxLength(pgdx);
        }

        public int gdxUELRegisterDone()
        {
            return dll_gdxUELRegisterDone(pgdx);
        }

        public int gdxUELRegisterMap(int UMap, string Uel)
        {
            return dll_gdxUELRegisterMap(pgdx, UMap, Uel);
        }

        public int gdxUELRegisterMapStart()
        {
            return dll_gdxUELRegisterMapStart(pgdx);
        }

        public int gdxUELRegisterRaw(string Uel)
        {
            return dll_gdxUELRegisterRaw(pgdx, Uel);
        }

        public int gdxUELRegisterRawStart()
        {
            return dll_gdxUELRegisterRawStart(pgdx);
        }

        public int gdxUELRegisterStr(string Uel, ref int UelNr)
        {
            return dll_gdxUELRegisterStr(pgdx, Uel, ref UelNr);
        }

        public int gdxUELRegisterStrStart()
        {
            return dll_gdxUELRegisterStrStart(pgdx);
        }

        public int gdxUMFindUEL(string Uel, ref int UelNr, ref int UelMap)
        {
            return dll_gdxUMFindUEL(pgdx, Uel, ref UelNr, ref UelMap);
        }

        public int gdxUMUelGet(int UelNr, ref string Uel, ref int UelMap)
        {
            int rc_gdxUMUelGet;
            StringBuilder cpy_Uel = new StringBuilder(gamsglobals.str_len);
            rc_gdxUMUelGet = dll_gdxUMUelGet(pgdx, UelNr, cpy_Uel, ref UelMap);
            Uel = cpy_Uel.ToString();
            return rc_gdxUMUelGet;
        }

        public int gdxUMUelInfo(ref int UelCnt, ref int HighMap)
        {
            return dll_gdxUMUelInfo(pgdx, ref UelCnt, ref HighMap);
        }

        public int gdxGetDomainElements(int SyNr, int DimPos, int FilterNr, TDomainIndexProc DP, ref int NrElem, IntPtr Uptr)
        {
            return dll_gdxGetDomainElements(pgdx, SyNr, DimPos, FilterNr, DP, ref NrElem, Uptr);
        }

        public int gdxCurrentDim()
        {
            return dll_gdxCurrentDim(pgdx);
        }

        public int gdxRenameUEL(string OldName, string NewName)
        {
            return dll_gdxRenameUEL(pgdx, OldName, NewName);
        }

    }
}
