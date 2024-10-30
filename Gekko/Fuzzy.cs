using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekko
{
    public class FuzzyVarName
    {        
        public List<string> storage = new List<string>();
        public GamsWalkerInfo info = new GamsWalkerInfo();
        public string simple = null;
        public double score = double.NaN;
        public string ToString()
        {
            string s = score + ":" + " [" + string.Join(", ", storage) + "]";
            List<string> ss = Fuzzy.Cleanup(storage, Fuzzy.ECleanupType.VariableNameFromRaw);
            if (!ss.SequenceEqual(storage))
            {
                s += " --> " + " [" + string.Join(", ", ss) + "]";
            }
            if (simple != null) s += " SIMPLE: " + simple;
            return s;
        }
    }

    //public static List<EquationBrowser>

    public class FuzzyEquation
    {
        public List<string> eqName = new List<string>();
        public string eqContents = null;
        public List<FuzzyVarName> varNamesLhs = new List<FuzzyVarName>();
        public List<FuzzyVarName> varNamesRhs = new List<FuzzyVarName>();
        public string ToString()
        {            
            List<string> ss = Fuzzy.Cleanup(eqName, Fuzzy.ECleanupType.EquationNameFromRaw);            
            string s = "[" + string.Join(", ", ss) + "] --> ";            
            s += this.eqContents;
            return s;
        }
    }

    public class Fuzzy
    {
        // e_y_tot(j, t) .. y['tot', j, t] = x['tot', j, t] + sum(i, y[i, j, t]);
        // e_y(i, j, t) .. y[i, j, t] = x[i, j, t] + 0.00001 * y['tot', j, t];
        //                

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
        /// 

        public enum ECleanupType
        {
            EquationNameFromRaw,
            VariableNameFromRaw,
            ChosenVariable
        }

        public static SortedDictionary<double, List<string>> OrderLhs(bool testLhs, List<FuzzyEquation> equations, List<string> chosen, bool print)
        {
            SortedDictionary<double, List<string>> order = new SortedDictionary<double, List<string>>(); //score, eqName            
            int nE = 0;
            foreach (FuzzyEquation equation in equations)
            {
                nE++;
                double bestScore = double.MaxValue;
                FuzzyEquation bestEquation = null;
                int nVLhs = 0;
                foreach (FuzzyVarName varName in equation.varNamesLhs)
                {
                    nVLhs++;
                    double score = EquationPoints(true, equation, chosen, varName, nE, nVLhs, print);
                    if (chosen == null || G.Equal(chosen[0], varName.storage[0]))
                    {
                        if (varName.score < bestScore)
                        {
                            bestScore = varName.score;
                            bestEquation = equation;
                        }
                    }
                }
                int nVRhs = 0;
                foreach (FuzzyVarName varName in equation.varNamesRhs)
                {
                    nVRhs++;
                    double score = EquationPoints(false, equation, chosen, varName, nE, nVLhs, print);
                    if (chosen == null || G.Equal(chosen[0], varName.storage[0]))
                    {
                        if (varName.score < bestScore)
                        {
                            bestScore = varName.score;
                            bestEquation = equation;
                        }
                    }                    
                }
                List<string> eqsNames = null;
                order.TryGetValue(bestScore, out eqsNames);
                if (eqsNames == null)
                {
                    eqsNames = new List<string>();
                    order.Add(bestScore, eqsNames);
                }
                eqsNames.Add(equation.eqContents);
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

        public static SortedDictionary<double, List<FuzzyEquation>> TestLhs(List<FuzzyEquation> equations, bool print)
        {
            SortedDictionary<double, List<FuzzyEquation>> order = new SortedDictionary<double, List<FuzzyEquation>>(); //score, eqName            
            int nE = 0;
            foreach (FuzzyEquation equation in equations)
            {
                nE++;
                double bestScore = double.MaxValue;
                FuzzyEquation bestEquation = null;
                int nVLhs = 0;
                foreach (FuzzyVarName varName in equation.varNamesLhs)
                {
                    string simple = varName.simple;
                    nVLhs++;
                    varName.score = EquationPoints(true, equation, null, varName, nE, nVLhs, print);
                    if (varName.score < bestScore)
                    {
                        bestScore = varName.score;
                        bestEquation = equation;
                    }
                }
                int nVRhs = 0;
                foreach (FuzzyVarName varName in equation.varNamesRhs)
                {
                    string simple = varName.simple;
                    nVRhs++;
                    varName.score = EquationPoints(false, equation, null, varName, nE, nVLhs, print);
                    if (varName.score < bestScore)
                    {
                        bestScore = varName.score;
                        bestEquation = equation;
                    }
                }
                SortedAddEquation(order, bestScore, equation);
            }
            return order;
        }

        public static void SortedAddEquation(SortedDictionary<double, List<FuzzyEquation>> sortedDict, double score, FuzzyEquation equation)
        {
            List<FuzzyEquation> eqsNames = null;
            sortedDict.TryGetValue(score, out eqsNames);
            if (eqsNames == null)
            {
                eqsNames = new List<FuzzyEquation>();
                sortedDict.Add(score, eqsNames);
            }
            eqsNames.Add(equation);
        }

        public static void SortedAdd1(SortedDictionary<double, int> sortedDict, double score)
        {
            int n;
            if (!sortedDict.TryGetValue(score, out n))
            {                
                sortedDict.Add(score, 1);
            }
            else
            {
                sortedDict[score]++;
            }            
        }

        private static double EquationPoints(bool isLhs, FuzzyEquation equation, List<string> chosen, FuzzyVarName varName, int nE, int nVLhs, bool print)
        {
            double penaltyRhs = 0.5d;
            double penaltyInsideSum = 1d;  //1 typically gets rhs penalty too. Should it be larger?
            double penaltyInsideDollar = 10d;  //Really cannot be relevant, but penalty does not seem to have any effect on orderings
            
            string s = "Lhs"; if (!isLhs) s = "Rhs";
                     
            varName.score = EditDistance(Cleanup(varName.storage, ECleanupType.VariableNameFromRaw), Cleanup(equation.eqName, ECleanupType.EquationNameFromRaw));
            if (!isLhs) varName.score += penaltyRhs;
            if (varName.info.isInsideSum) 
                varName.score += penaltyInsideSum;
            if (varName.info.isInsideDollar) varName.score += penaltyInsideDollar;

            double score;
            if (chosen == null)
            {
                score = varName.score;                
                if (print) new Writeln("Eq " + nE + " Var" + s + " " + nVLhs + " " + varName.simple + " Score = " + score);                            
            }
            else 
            {
                score = varName.score + EditDistance(Cleanup(chosen, ECleanupType.ChosenVariable), Cleanup(varName.storage, ECleanupType.VariableNameFromRaw));
                if (G.Equal(chosen[0], varName.storage[0]))
                {
                    if (print) new Writeln("Eq " + nE + " Var" + s + " " + nVLhs + " " + varName.simple + " Score = " + score);                    
                }
            }
            return score;
        }        

        /// <summary>
        /// Creates a new list where blanks are removed in elements, "Born" --> "Boern", and "t1End", "tEnd", "End", "aEnd" are removed.
        /// Anything after a "via" is removed (including the "via").
        /// </summary>
        /// <param name="m"></param>
        /// <param name="isEqName"></param>
        /// <returns></returns>
        public static List<string> Cleanup(List<string> m5, ECleanupType type)
        {
            bool quotes = false;
            List<string> copy = m5.ToList();

            for (int i = 0; i < copy.Count; i++)
            {
                copy[i] = copy[i].Replace(" ", "");
            }

            for (int i = 1; i < copy.Count - 1; i++)  //skip first, skip last
            {
                if (!IsElement(copy[i]))
                {
                    bool hit = false;
                    List list = O.GetIVariableFromString("#" + copy[i], O.ECreatePossibilities.NoneReturnNullAlways) as List;
                    if (list != null && list.Count() == 1 && list.list[0].Type() == EVariableType.String)
                    {
                        if (!quotes) copy[i] = O.ConvertToString(list.list[0]);
                        else copy[i] = "'" + O.ConvertToString(list.list[0]) + "'";
                        hit = true;
                    }
                    if (hit == false)
                    {
                        //Special handling of tot stuff
                        if (copy[i].EndsWith("tot", StringComparison.OrdinalIgnoreCase))  //atot --> tot, xtot --> tot
                        {
                            if (!quotes) copy[i] = "tot";
                            else copy[i] = "'tot'";
                        }
                    }
                }
            }

            if (type == ECleanupType.EquationNameFromRaw)
            {
                for (int i = 0; i < copy.Count; i++)
                {
                    copy[i] = copy[i].Replace("Born", "Boern");  //Do before null-setting. Do something about a18 --> a?, but does not improve it
                    if (G.Equal(copy[i], "t1End")) copy[i] = null;
                    else if (G.Equal(copy[i], "tEnd")) copy[i] = null;
                    else if (G.Equal(copy[i], "End")) copy[i] = null;
                    else if (G.Equal(copy[i], "aEnd")) copy[i] = null;
                }
                for (int i = 0; i < copy.Count; i++)
                {
                    if (copy[i] != null && G.Equal(copy[i], "via"))
                    {
                        for (int j = i; j < copy.Count; j++)
                        {
                            copy[j] = null;
                        }
                        break;
                    }
                }
            }
            copy.RemoveAll(x => x == null);

            for (int i = 0; i < copy.Count; i++)  //skip first, skip last
            {
                copy[i] = G.StripQuotes2(G.StripQuotes(copy[i]));
            }

            return copy;
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
