using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekko
{
    public class Fuzzy
    {
        // e_y_tot(j, t) .. y['tot', j, t] = x['tot', j, t] + sum(i, y[i, j, t]);
        // e_y(i, j, t) .. y[i, j, t] = x[i, j, t] + 0.00001 * y['tot', j, t];
        //        

        public class VarName
        {
            public List<string> storage = new List<string>();
            public string simple = null;
            public double score = double.NaN;
        }

        //public static List<EquationBrowser>

        public class Equation
        {
            public List<string> eqName = new List<string>();
            public string eqNameSimple = null;
            public List<VarName> varNamesLhs = new List<VarName>();
            public List<VarName> varNamesRhs = new List<VarName>();
        }

        public static void Test()
        {
            List m = new List();
            m.list = new List<IVariable>() { new ScalarString("tot") };
            Program.databanks.GetFirst().AddIVariable("#atot", m);                       

            List<string> chosen = new List<string>() { "y", "'tot'", "'a'", "t" }; //Always plings for middle elements                        
            List<Equation> equations = new List<Equation>();
            Equation e1 = new Equation();
            e1.eqName = new List<string>() { "y", "tot", "j", "t" }; //No "e", and will never have plings
            e1.eqNameSimple = "e_y_tot[j, t]"; //Because eqs cannot be redefined, another eq cannot start with e_y_tot.
            e1.varNamesLhs.Add(new VarName() { simple = "y['tot', j, t]", storage = new List<string>() { "y", "'tot'", "j", "t" } });
            e1.varNamesRhs.Add(new VarName() { simple = "x['tot', j, t]", storage = new List<string>() { "x", "'tot'", "j", "t" } });
            e1.varNamesRhs.Add(new VarName() { simple = "y[i, j, t]", storage = new List<string>() { "y", "i", "j", "t" } });
            equations.Add(e1);
            Equation e2 = new Equation();
            e2.eqName = new List<string>() { "y", "i", "j", "t" };
            e2.eqNameSimple = "e_y[i, j, t]";
            e2.varNamesLhs.Add(new VarName() { simple = "y[i, j, t]", storage = new List<string>() { "y", "i", "j", "t" } });
            e2.varNamesRhs.Add(new VarName() { simple = "x[i, j, t]", storage = new List<string>() { "x", "i", "j", "t" } });
            e2.varNamesRhs.Add(new VarName() { simple = "y['tot', j, t]", storage = new List<string>() { "y", "'tot'", "j", "t" } });
            equations.Add(e2);
                        
            //Keeping the full eqName including [...], perhaps makes it easier to deal with lags/leads?
            SortedDictionary<double, List<string>> order = OrderLhs(equations, chosen, 0.5, true);
        }

        /// <summary>
        /// From equations chunks and chosen chunks, this produces a distance. Method is to loop throug all equations. Then for each
        /// eqution, all variables are looped (their chunks are found). Then first a score is computed between the variable chunks
        /// and the equation name. For instance x[a] has chunks ("x", "a") and eq name E_x_a has chunks ("x", "a"), so there is a distance
        /// = 0. Next the distance between the chosen name, say x[a] and the variable name (also x[a]) is computed, in this case
        /// also = 0. The two distances are summed. This is done for LHS and RHS separately, and the minimum is found.
        /// Note that RHS has 0.5 added for penalty.
        /// </summary>
        /// <param name="equations"></param>
        /// <param name="chosen"></param>
        /// <param name="penalty_rhs"></param>
        /// <param name="print"></param>
        /// <returns></returns>
        public static SortedDictionary<double, List<string>> OrderLhs(List<Equation> equations, List<string> chosen, double penalty_rhs, bool print)
        {
            SortedDictionary<double, List<string>> order = new SortedDictionary<double, List<string>>(); //score, eqName
            Cleanup(chosen, false);
            int nE = 0;
            foreach (Equation equation in equations)
            {
                nE++;
                Cleanup(equation.eqName, true);
                ReplaceSingletons(equation.eqName); //replace ["y", "atot", "j", "t"] with ["y", "'tot'", "j", "t"]
                double bestScore = int.MaxValue;
                int nVLhs = 0;
                foreach (VarName varName in equation.varNamesLhs)
                {
                    nVLhs++;
                    bestScore = EquationPoints(true, equation, chosen, varName, bestScore, nE, nVLhs, print, penalty_rhs);
                }
                int nVRhs = 0;
                foreach (VarName varName in equation.varNamesRhs)
                {
                    nVRhs++;
                    bestScore = EquationPoints(false, equation, chosen, varName, bestScore, nE, nVLhs, print, penalty_rhs);
                }
                List<string> eqsNames = null;
                order.TryGetValue(bestScore, out eqsNames);
                if (eqsNames == null)
                {
                    eqsNames = new List<string>();
                    order.Add(bestScore, eqsNames);
                }
                eqsNames.Add(equation.eqNameSimple);
            }
            if (print)
            {
                foreach (KeyValuePair<double, List<string>> kvp in order)
                {
                    new Writeln(" --- " + kvp.Key + ": " + Stringlist.GetListWithCommas(kvp.Value));
                }
            }
            return order;
        }

        public static SortedDictionary<double, List<string>> TestLhs(List<Equation> equations, double penalty_rhs, bool print)
        {
            SortedDictionary<double, List<string>> order = new SortedDictionary<double, List<string>>(); //score, eqName            
            int nE = 0;
            foreach (Equation equation in equations)
            {
                nE++;
                Cleanup(equation.eqName, true);
                ReplaceSingletons(equation.eqName); //replace ["y", "atot", "j", "t"] with ["y", "'tot'", "j", "t"]
                double bestScore = int.MaxValue;
                int nVLhs = 0;
                foreach (VarName varName in equation.varNamesLhs)
                {
                    nVLhs++;
                    bestScore = EquationPoints(true, equation, null, varName, bestScore, nE, nVLhs, print, penalty_rhs);
                }
                int nVRhs = 0;
                foreach (VarName varName in equation.varNamesRhs)
                {
                    nVRhs++;
                    bestScore = EquationPoints(false, equation, null, varName, bestScore, nE, nVLhs, print, penalty_rhs);
                }
                List<string> eqsNames = null;
                order.TryGetValue(bestScore, out eqsNames);
                if (eqsNames == null)
                {
                    eqsNames = new List<string>();
                    order.Add(bestScore, eqsNames);
                }
                eqsNames.Add(equation.eqNameSimple);
            }
            if (print)
            {
                foreach (KeyValuePair<double, List<string>> kvp in order)
                {
                    new Writeln(" --- " + kvp.Key + ": " + Stringlist.GetListWithCommas(kvp.Value));
                }
            }
            return order;
        }


        private static double EquationPoints(bool isLhs, Equation equation, List<string> chosen, VarName varName, double bestScore, int nE, int nVLhs, bool print, double penalty_rhs)
        {
            string s = "Lhs";
            double p = 0d;
            if (!isLhs)
            {
                s = "Rhs";
                p = penalty_rhs;
            }
            Cleanup(varName.storage, false);
            ReplaceSingletons(varName.storage); //replace ["y", "atot", "j", "t"] with ["y", "'tot'", "j", "t"]            
            varName.score = EditDistance(varName.storage, equation.eqName) + p;
            if (chosen == null)
            {
                double score = varName.score;                
                if (print) new Writeln("Eq " + nE + " Var" + s + " " + nVLhs + " " + varName.simple + " Score = " + score);
                bestScore = Math.Min(bestScore, score);                
            }
            else 
            {
                double score = varName.score + EditDistance(chosen, varName.storage);
                if (G.Equal(chosen[0], varName.storage[0]))
                {
                    if (print) new Writeln("Eq " + nE + " Var" + s + " " + nVLhs + " " + varName.simple + " Score = " + score);
                    bestScore = Math.Min(bestScore, score);
                }
            }

            return bestScore;
        }

        /// <summary>
        /// Replace an element like atot with 'tot', because #atot is found in databank.
        /// If not found in databank, it may still be replaced according to the name.
        /// </summary>
        /// <param name="m"></param>
        private static void ReplaceSingletons(List<string> m)
        {
            for (int i = 1; i < m.Count - 1; i++)  //skip first, skip last
            {
                if (!IsElement(m[i]))
                {
                    bool hit = false;
                    List list = O.GetIVariableFromString("#" + m[i], O.ECreatePossibilities.NoneReturnNullAlways) as List;
                    if (list != null && list.Count() == 1 && list.list[0].Type() == EVariableType.String)
                    {
                        m[i] = "'" + O.ConvertToString(list.list[0]) + "'";
                        hit = true;
                    }
                    if (hit == false)
                    {
                        //Special handling of tot stuff
                        if (m[i].EndsWith("tot", StringComparison.OrdinalIgnoreCase))  //atot --> tot, xtot --> tot
                        {
                            m[i] = "'tot'";
                        }
                    }
                }
            }
        }

        private static void Cleanup(List<string> m, bool isEqName)
        {
            for (int i = 0; i < m.Count; i++)  //skip first, skip last
            {
                m[i] = m[i].Replace(" ", "");
                if (isEqName)
                {
                    m[i] = m[i].Replace("Born", "Boern");  //Do before null-setting. Do something about a18 --> a?, but does not improve it
                    if (G.Equal(m[i], "t1End")) m[i] = null;
                    else if (G.Equal(m[i], "tEnd")) m[i] = null;
                    else if (G.Equal(m[i], "End")) m[i] = null;
                    else if (G.Equal(m[i], "aEnd")) m[i] = null;
                    
                }
            }            
            m.RemoveAll(x => x == null);            
        }

        /// <summary>
        /// Has plings like 'x'.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsElement(string s) 
        {
            if(s.StartsWith("'") && s.EndsWith("'")) return true;
            return false;
        }

        /// <summary>
        /// Damerau–Levenshtein distance. Can delete a word, add a new word, change a word to another word, or swap two words. Immune
        /// to blanks or casing.
        /// https://gist.github.com/wickedshimmy/449595/a17ab0d689623f5e6730eeb1c8606ab771149819
        /// </summary>
        /// <param name="original"></param>
        /// <param name="modified"></param>
        /// <returns></returns>
        public static int EditDistance(List<string> original, List<string> modified)
        {
            if (original == modified)
                return 0;

            int len_orig = original.Count;
            int len_diff = modified.Count;
            if (len_orig == 0 || len_diff == 0) return len_orig == 0 ? len_diff : len_orig;

            var matrix = new int[len_orig + 1, len_diff + 1];

            for (int i = 1; i <= len_orig; i++)
            {
                matrix[i, 0] = i;
                for (int j = 1; j <= len_diff; j++)
                {
                    int cost = G.EqualHandleBlanks(modified[j - 1], original[i - 1]) ? 0 : 1;
                    if (i == 1)
                        matrix[0, j] = j;

                    var vals = new int[] {
                    matrix[i - 1, j] + 1,
                    matrix[i, j - 1] + 1,
                    matrix[i - 1, j - 1] + cost
                };
                    matrix[i, j] = vals.Min();
                    if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[len_orig, len_diff];
        }


    }
}
