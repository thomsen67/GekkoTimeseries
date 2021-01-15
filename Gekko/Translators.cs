/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

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
using System.IO;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

//
//                              TRANSLATING FROM GEKKO 1.8, see T1.g
//


namespace Gekko
{
    public class Translators
    {
        public static bool hasBeenEndoExo = false;
        public static bool hasBeenCmdCall = false;
        public static string[] commands2 = new string[]
            {
                "add",
                "after",
                "checkoff",
                "clear",
                "closeall",
                "closebanks",
                "cls",
                "collapse",
                "convertprn",
                "cplot",
                "create",
                "date",
                "delete",
                "defdir",
                "dekomp",
                "decomp",
                "delete",
                "difprt",
                "disp",
                "display",
                "dprt",
                "dumon",
                "dumof",
                "dumoff",
                "echo",
                "efter",
                "else",
                "end",
                "endo",
                "endogenize",
                "exo",
                "exit",
                "exogenize",
                "efter",
                "efter2",
                "findmissingdata",
                "flat",
                "frml",
                "for",
                "genr",
                "gmulprt",
                "gprt",
                "help",
                "hdg",
                "help",
                "if",
                "info",
                "ini",
                "int",
                "itershow",
                "list",
                "log",
                "macro",
                "mem",
                "menu",
                "model",
                "mulbk",
                "mulpct",
                "mulprt",
                "ndifprt",
                "open",
                "option",
                "pctprt",
                "p",
                "pause",
                "print",
                "prt",
                "pri",
                "pipe",
                "prt",
                "pplot",
                "read",
                "res",
                "return",
                "run",
                "set",
                "sign",
                "stop",
                "string",
                "sim",
                "skip",
                "stamp",
                "string",
                "stop",
                "sys",
                "tabel",
                "table",
                "time",
                "timespan",
                "timefilter",                
                "udvalg",
                "undo",
                "unfix",
                "unswap",
                "upd",
                "updprt",
                "pipe",
                "tell",
                "trimvars",
                "val",
                "vers",
                "vprt",
                "val",
                "wplot",
                "wudvalg",
                "write",
                "zero"
            };

        
        public static void PrintAST(CommonTree node, int depth, StringBuilder sb, GekkoDictionary<string, string> scalars)
        {
            if (depth > 0)
            {
                string s = null;
                if (node.Text == "ASTCOMMAND")
                {
                    s = HandleCommand123(node, sb, s, scalars);
                    return;  //all sub-nodes are handled
                }
                else
                {
                    s = ReplaceNames(node.Text);
                }                
             
                //------ handling stuff not in COMMAND OPTION REST form

                if (s != null && s.StartsWith("()")) s = "//" + s.Substring(2);

                if (s != null && s.Trim().ToLower().StartsWith("display"))
                {
                    int i = FindFirstNonBlank(s);
                    string ss = s.Substring(i + 7);
                    if (ss.EndsWith(G.NL)) ss = ss.Substring(0, ss.Length - 2);
                    s = "tell '" + ss + "';" + G.NL; ;
                }

                if (s != null && s.Trim().ToLower().StartsWith("hdg"))
                {
                    int i = FindFirstNonBlank(s);
                    string ss = s.Substring(i + 3);
                    if (ss.EndsWith(G.NL)) ss = ss.Substring(0, ss.Length - 2);
                    s = "hdg '" + ss + "';" + G.NL;
                }                
                
                sb.Append(s);
            }
            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Count; ++i)
                {
                    CommonTree child = (CommonTree)(node.Children[i]);
                    PrintAST(child, depth + 1, sb, scalars);
                }
            }
        }

        private static string HandleCommand123(CommonTree node, StringBuilder sb, string s, GekkoDictionary<string, string> scalars)
        {            
            string comment = null;
            string s1 = "";
            string s2 = "";
            string s3 = "";
            List<string> all1 = new List<string>();
            List<string> all2 = new List<string>();
            List<string> all3 = new List<string>();
            GetTokens(node, all1, all2, all3);
            //bool importHelper = false;

            // =========== command ========================
            
            string type = "";
            foreach (string xx1 in all1) type += xx1.Trim().ToLower();

            string typeOrig = "";
            foreach (string xx1 in all1) typeOrig += xx1.Trim();

            List<string> listTypes = GetListTypes();
            bool isListType = false;
            if (listTypes.Contains(type)) isListType = true;
                        
            List<string> prtTypes = GetPrtTypes();
            bool isPrtType = false;
            if (prtTypes.Contains(type)) isPrtType = true;

            int z = 0;
            if (all1[0].Trim() == "") z = 1;

            if (type == "endo" || type == "exo" || type == "endogenize" || type == "exogenize")
            {
                hasBeenEndoExo = true;
            }
            else if (type == "add")
            {
                all1[z] = Cap("RUN", typeOrig);
                hasBeenCmdCall = true;
            }
            else if (type == "clear")
            {
                if (all2.Contains("all", StringComparer.OrdinalIgnoreCase)) all1[z] = Cap("RESET", typeOrig);
                else all1[z] = Cap("RESTART", typeOrig);
            }
            else if (type == "closeall")
            {
                all1[z] = Cap("RESTART", typeOrig);
            }
            else if (type == "closebanks")
            {
                all1[z] = Cap("RESTART", typeOrig);
            }
            else if (type == "cplot")
            {
                all1[z] = Cap("CLIP", typeOrig);
            }
            else if (type == "difprt")
            {
                all1[z] = Cap("COMPARE", typeOrig);
            }
            else if (type == "ndifprt")
            {
                all1[z] = Cap("COMPARE", typeOrig);
                AddOption(all2, "abs");
            }
            else if (type == "efter")
            {
                all1[z] = Cap("SIM", typeOrig);
                AddOption(all2, "after");
            }
            else if (type == "genr")
            {
                all1[z] = Cap("SERIES", typeOrig);
            }
            else if (type == "mulbk")
            {
                int ifirst = FindNextRealToken(all3, 0);
                if (ifirst == -12345)
                {
                    //"mulbk"
                    all1[z] = Cap("CLONE", typeOrig);                    
                }
                else
                {
                    //"mulbk adbk"
                    all1[z] = Cap("READ", typeOrig);
                    AddOption(all2, "ref");
                }
            }            
            else if (type == "pplot")
            {
                all1[z] = Cap("PLOT", typeOrig);
            }
            else if (type == "string")
            {
                all1[z] = Cap("NAME", typeOrig);  //usually the best type
            }
            else if (type == "res")
            {
                all1[z] = Cap("SIM", typeOrig);
                AddOption(all2, "res");
            }
            else if (type == "sim")
            {
                all1[z] = Cap("SIM", typeOrig);
                if (hasBeenEndoExo)
                {
                    AddOption(all2, "fix");                    
                }                
            }
            else if (type == "udvalg")
            {
                all1[z] = Cap("DECOMP", typeOrig); 
            }            
            else if (type == "wplot")
            {
                all1[z] = Cap("SHEET", typeOrig);
            }
            else if (type == "read")
            {
                
            }
            else if (type == "run")
            {
                hasBeenCmdCall = true;
            }
            else if (type == "trimvars")
            {
                all1[z] = Cap("DELETE", typeOrig);
                AddOption(all2, "nonmodel");
            }
            else if (type == "upd")
            {
                all1[z] = Cap("SERIES", typeOrig);
            }   
            else if (type == "write")
            {
                
            }
            // ----------------------------------------------------
            // ----------------------------------------------------            
            else if (type == "gmulprt")
            {
                all1[z] = Cap("MULPRT", typeOrig);
                AddOption(all2, "v");
            }            

            if (!commands2.Contains(type))
            {
                if (FindNextRealToken(all2, 0) == -12345 && FindNextRealToken(all3, 0) == -12345)
                {                    
                    //standalone unknown ident                    
                    all1[FindNextRealToken(all1, 0)] = "RUN " + all1[FindNextRealToken(all1, 0)];
                    hasBeenCmdCall = true;
                }
            }
            
            for (int i = 0; i < all1.Count; i++)
            {
                s1 += all1[i];
            }            

            // =========== options ========================            

            for (int i = 0; i < all2.Count; i++)
            {
                if (i < all2.Count - 1 && all2[i] == "#" && G.IsIdentTranslate(all2[i + 1]))
                {
                    all2[i] = "%";  //always change # to % in option field i Gekko 1.8
                }
                s2 += all2[i];
            }

            // =========== rest of command ========================                       

            //handle stuff like "PRT 2010 2020 fy fe", but not for TIME!
            int iStart = 0;
            string time = null;
            int first = FindNextRealToken(all3, 0);
            int second = FindNextRealToken(all3, first + 1);
            if (type != "time" && first != -12345 && second != -12345)
            {
                if (G.IsIntegerTranslate(all3[first]) && G.IsIntegerTranslate(all3[second]))
                {
                    time = all3[first] + " " + all3[second];
                    iStart = second + 1;
                }
            }

            //register scalar types
            if (type == "val" || type == "date" || type == "string" || type == "name")
            {
                string name = null;
                if (first != -12345) name = all3[first];
                if (G.IsIdentTranslate(name))
                {
                    if (!scalars.ContainsKey(name)) scalars.Add(name, "");
                }
            }

            for (int i = iStart; i < all3.Count; i++)
            {
                
                //change soft parens
                if (i > 0 && G.Equal(all3[i], "cmd") && G.Equal(all3[i - 1], "."))
                {
                    all3[i] = Globals.extensionCommand;
                }
                
                //Maybe change #x to %x
                if (i < all3.Count - 1 && all3[i] == "#" && G.IsIdentTranslate(all3[i + 1]))
                {
                    bool exist = false;
                    bool isAllEndoExo = false;
                    //if (Program.scalars != null && Program.scalars.ContainsKey(all3[i + 1].Trim())) exist = true;
                    foreach (string ss in new string[] { "all", "endo", "exo", "exod", "exodjz", "exoj", "exotrue", "exoz" })
                    {
                        if (all3[i + 1].Trim().ToLower() == ss) isAllEndoExo = true;
                    }                    
                    if (exist || scalars.ContainsKey(all3[i + 1].Trim()))
                    {
                        if (!isAllEndoExo)
                        {
                            all3[i] = "%";  //only if it has been defined before
                        }
                    }
                }                
                
                if (isListType)
                {
                    //setting commas on stuff that looks like lists of idents, lists or scalars
                    if (G.IsIdentTranslate(all3[i]))
                    {
                        int i2 = FindNextRealToken(all3, i + 1);
                        if (i2 == i + 2)
                        {
                            if (G.IsIdentTranslate(all3[i2]) || all3[i2] == "#")
                            {
                                all3[i + 1] = "," + all3[i + 1];
                            }
                        }
                    }
                    s3 += all3[i];
                }
                else if (isPrtType)
                {
                    //setting commas on stuff that looks like lists of idents, lists or scalars
                    //handles "a13 a14" "a/5 b" "a 5*c" "[*] b" "b [*]" "(a+b) (c+d)"
                    if (G.IsIdentTranslate(all3[i]) || IsNumber(all3[i]) || all3[i] == "]" || all3[i] == ")" || all3[i] == "}")
                    {
                        int i2 = FindNextRealToken(all3, i + 1);
                        if (i2 == i + 2)
                        {
                            if (G.IsIdentTranslate(all3[i2]) || all3[i2] == "#" || IsNumber(all3[i2]) || all3[i2] == "[" || all3[i2] == "(" || all3[i2] == "{" || all3[i2] == "@")
                            {
                                all3[i + 1] = "," + all3[i + 1];
                            }
                        }
                    }
                    s3 += all3[i];
                }
                else
                {
                    s3 += all3[i];
                }
            }

            if (type == "option")  //perhaps overriding the s3 made above
            {
                string s10 = "";                
                bool hit = false;
                string sTemp = "";
                for (int i = 0; i < all3.Count; i++)
                {
                    if (hit) sTemp += all3[i];
                    else
                    {
                        string zz = all3[i].Trim().ToLower();
                        if (zz != "") sTemp += " " + zz;
                    }

                    if (sTemp == " " + "databank file tsdx compress")
                    {
                        sTemp = " " + "databank file gbk compress";
                        hit = true;
                    }

                    if (sTemp == " " + "databank file tsdx version")
                    {
                        sTemp = " " + "databank file gbk version";
                        hit = true;
                    }
                    
                    if (sTemp == " " + "folder cmd")
                    {
                        sTemp = " " + "folder command";
                        hit = true;
                    }
                    if (sTemp == " " + "folder cmd1")
                    {
                        sTemp = " " + "folder command1";
                        hit = true;
                    }
                    if (sTemp == " " + "folder cmd2")
                    {
                        sTemp = " " + "folder command2";
                        hit = true;
                    }
                    if (sTemp == " " + "interface excel decimalseparator")
                    {
                        sTemp = " " + "interface clipboard decimalseparator";
                        hit = true;
                    }
                    if (sTemp == " " + "option graph lines points")
                    {
                        sTemp = " " + "option plot lines points";
                        hit = true;
                    }
                    if (sTemp == " " + "option solve data createvars")
                    {
                        sTemp = " " + "option solve data create auto";
                        hit = true;
                    }
                    if (sTemp == " " + "option forward fair dump")
                    {
                        sTemp = " " + "option forward dump";
                        hit = true;
                    }                    
                }
                if (hit)
                {
                    s3 = sTemp;
                }
                
            }

            // ========================

            if (s1.Trim().ToLower() == "stamp") s1 = "tell currentDateTime();";
            if (s1.Trim().ToLower() == "vers") s1 = "tell gekkoVersion();";
            
            if (s1 == null) s1 = "";
            if (s2 == null) s2 = "";
            if (s3 == null) s3 = "";
            if (s2 == "") s = s1.TrimEnd() + " " + s3.Trim();
            else s = s1.TrimEnd() + " " + s2.Trim() + " " + s3.Trim();
            if (time != null) s = Cap("TIME", typeOrig) + " " + time + "; " + s;

            //if (importHelper)
            //{
            //    s = Cap("CLEAR", typeOrig) + "<prim>; " + s + "; " + Cap("CLONE", typeOrig) + "; ";  //will get a superfluous ';' at the end
            //}            

            sb.Append(s);

            if (comment != null) sb.Append(" /* " + comment + " */ ");

            return s;
        }

        private static List<string> GetFormats()
        {
            List<string> formats = new List<string>();
            formats.Add("tsd");
            formats.Add("pcim");
            formats.Add("csv");
            formats.Add("prn");
            formats.Add("xls");
            formats.Add("xlsx");
            formats.Add("tsp");
            return formats;
        }

        private static string Cap(string input, string model)
        {
            model = model.Trim();
            bool upper = false;
            if (model != null && model.Length > 0 && char.IsUpper(model[0])) upper = true;
            if (upper) input = input.ToUpper();
            else input = input.ToLower();
            return input;
        }

        private static void AddOption(List<string> all2, string extra)
        {
            bool exists = false;
            foreach (string s in all2)
            {
                if (s.ToLower().Trim() == extra.ToLower()) exists = true;
            }
            if (exists) return;
            if (all2.Count == 0)
            {
                all2.Add("<");
                all2.Add(extra);
                all2.Add(">");
            }
            else
            {
                if (all2[all2.Count - 1] == ">")
                {
                    all2[all2.Count - 1] = " ";
                    all2.Add(extra);                    
                    all2.Add(">");
                }
            }
        }

        private static void AddRest(List<string> all2, string extra)
        {
            all2.Add(" ");
            all2.Add(extra);
        }

        

        private static bool IsNumber(string x)
        {
            //not precise, does not handle exponents
            if (x == null) return false;
            x = x.Trim();
            if (x == "") return false;            
            for (int i = 0; i < x.Length; i++)
            {
                if (!(char.IsDigit(x[i]) || x[i] == '.')) return false;
            }
            return true;
        }

        private static int  FindNextRealToken(List<string> all3, int i)
        {
            if (i == -12345 || i == -12344) return -12345;
            int next = -12345;
            for (int ii = i; ii < all3.Count; ii++)
            {
                if (all3[ii].Trim() != "")
                {
                    next = ii;
                    break;
                }

            }
            return next;
        }

        private static List<string> GetListTypes()
        {
            List<string> listTypes = new List<string>();
            listTypes.Add("checkoff");
            listTypes.Add("create");
            listTypes.Add("difprt");
            listTypes.Add("disp");
            listTypes.Add("endo");
            listTypes.Add("exo");
            listTypes.Add("findmissingdata");
            listTypes.Add("info");
            listTypes.Add("itershow");
            listTypes.Add("ndifprt");
            listTypes.Add("write");
            listTypes.Add("udvalg");
            listTypes.Add("decomp");
            listTypes.Add("list");  //but is handled specially
            return listTypes;
        }

        private static List<string> GetPrtTypes()
        {
            List<string> listTypes = new List<string>();
            listTypes.Add("cplot");
            listTypes.Add("mulprt");
            listTypes.Add("pplot");
            listTypes.Add("p");
            listTypes.Add("pri");
            listTypes.Add("prt");
            listTypes.Add("print");
            listTypes.Add("gmulprt");
            listTypes.Add("wplot");            
            return listTypes;
        }

        private static void GetTokens(CommonTree node, List<string> all1, List<string> all2, List<string> all3)
        {
            if (((CommonTree)node.Children[0]).ChildCount > 0)
            {
                foreach (CommonTree child in ((CommonTree)node.Children[0]).Children)
                {
                    string token = ReplaceNames(child.Text);
                    all1.Add(token);
                }
            }
            if (((CommonTree)node.Children[1]).ChildCount > 0)
            {
                List<string> all = new List<string>();
                foreach (CommonTree child in ((CommonTree)node.Children[1]).Children)
                {
                    string token = ReplaceNames(child.Text);
                    all2.Add(token);
                }
            }
            if (((CommonTree)node.Children[2]).ChildCount > 0)
            {
                List<string> all = new List<string>();
                foreach (CommonTree child in ((CommonTree)node.Children[2]).Children)
                {
                    string token = ReplaceNames(child.Text);
                    all3.Add(token);                    
                }
            }            
        }

        private static string ReplaceNames(string s)
        {
            if (s == null) return null;
            if (s == "LEFTBRACKET") s = "[";
            else if (s == "RIGHTBRACKET") s = "]";
            else if (s == "DIV") s = "/";
            else if (s == "STAR") s = "*";
            else if (s == "ASTSERIES") s = "series";
            else if (s == "SEMICOLON") s = ";";
            else if (s == "COMMA") s = ",";
            else if (s == "AST1") s = " ";
            else if (s == "EQUAL") s = "=";
            else if (s == "LEFTANGLE") s = "<";
            else if (s == "RIGHTANGLE") s = ">";
            return s;
        }                

        private static int FindFirstNonBlank(string s)
        {
            int begin = -12345;
            for (int j = 0; j < s.Length; j++)
            {
                if (s[j] == ' ') continue;
                else
                {
                    begin = j;
                    break;
                }
            }
            return begin;
        }        
    }
}



