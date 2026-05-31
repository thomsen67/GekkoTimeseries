using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ProtoBuf;
using System.Threading;
using System.Text;

namespace Gekko
{

    public enum EStorage
    {
        cellsQuo,
        cellsGradQuo,
        cellsContribD,
        cellsChangeD,
        // -------------------------------------
        cellsRef,
        cellsGradRef,
        cellsContribDRef,
        cellsChangeDRef,
        // -------------------------------------
        cellsContribM,
        cellsChangeM,
        None
}
    
    /// <summary>
    /// Helper class to chop up the key from the DecompDict (containing decomp results)
    /// </summary>
    public class ChopFullVariableName
    {
        public string varName;
        public string fullName;
        public string lag;
        public string[] indexes;
        public string[] domains;
        public int iLag;
        public bool isLhs;
    }

    /// <summary>
    /// Stores all equations (non-unrolled)
    /// </summary>
    public class DecompDatas
    {
        public List<List<DecompData>> storage = null; //decomps of equations
        public DecompData MAIN_data = null;  //combined results
        public bool hasD = false;
        public bool hasM = false;
        public bool hasRD = false;
    }

    /// <summary>
    /// Decomp of 1 unrolled equation
    /// </summary>
    public class DecompData
    {
        public DecompDict cellsQuo = null;
        public DecompDict cellsGradQuo = null;
        public DecompDict cellsContribD = null;
        public DecompDict cellsChangeD = null;
        // -------------------------------------
        public DecompDict cellsRef = null;
        public DecompDict cellsGradRef = null;
        public DecompDict cellsContribDRef = null;
        public DecompDict cellsChangeDRef = null;
        // -------------------------------------
        public DecompDict cellsContribM = null;
        public DecompDict cellsChangeM = null;
        // -------------------------------------

        public string lhs = null;  //name of the LHS variable, for instance "Work:y2¤[0]

        public DecompData DeepClone()
        {
            DecompData dd = new DecompData();
            dd.cellsQuo = this.cellsQuo.DeepClone();
            dd.cellsGradQuo = this.cellsGradQuo.DeepClone();
            dd.cellsContribD = this.cellsContribD.DeepClone();
            dd.cellsChangeD = this.cellsChangeD.DeepClone();
            dd.cellsRef = this.cellsRef.DeepClone();
            dd.cellsGradRef = this.cellsGradRef.DeepClone();
            dd.cellsContribDRef = this.cellsContribDRef.DeepClone();
            dd.cellsChangeDRef = this.cellsChangeDRef.DeepClone();
            dd.cellsContribM = this.cellsContribM.DeepClone();
            dd.cellsChangeM = this.cellsChangeM.DeepClone();
            dd.lhs = this.lhs;
            return dd;
        }
    }

    public class GekkoPivotTable
    {
        // Function to create a pivot table with filtering
        public static Dictionary<string, Dictionary<string, AggContainer>> Compute(
            FrameLight dataframe,                          // The generic data rows
            List<int> pivotRowIndexes,                     // Indices of the elements to use for row dimensions
            List<int> pivotColIndexes,                     // Indices of the elements to use for column dimensions            
            Func<IEnumerable<AggContainer>, AggContainer> agg,         // Aggregation function (e.g., sum)
            DecompOptions2 decompOptions2,
            Func<FrameLightRow, bool> filter = null,       // Optional filter function
            Func<FrameLightRow, int, string> group = null  // Optional grouping function            
        )
        {
            // Initialize the pivot table as a nested dictionary
            Dictionary<string, Dictionary<string, List<AggContainer>>> pivotTable = new Dictionary<string, Dictionary<string, List<AggContainer>>>();
                        
            // Step 1: Iterate over each row in the data
            foreach (FrameLightRow dataframeRow in dataframe.data)
            {
                // Apply the filter if one is provided
                if (filter != null && !filter(dataframeRow))
                {
                    continue;  // Skip this dataframe row if it doesn't match the filter
                }

                // Construct row and column keys, and optionally group them
                int frameLhsCol = dataframe.frameDimensionNames[Globals.col_lhs];
                string rowKey = GekkoPivotGroup(pivotRowIndexes, group, dataframeRow, frameLhsCol, decompOptions2);
                string colKey = GekkoPivotGroup(pivotColIndexes, group, dataframeRow, frameLhsCol, decompOptions2);

                // Initialize a row in the pivot table if it doesn't exist
                if (!pivotTable.ContainsKey(rowKey)) pivotTable[rowKey] = new Dictionary<string, List<AggContainer>>();

                // Initialize a column in the row if it doesn't exist
                if (!pivotTable[rowKey].ContainsKey(colKey)) pivotTable[rowKey][colKey] = new List<AggContainer>();

                // Step 5: Add the value (from the values part of the data row)

                //double dFirstLevelLag, double dFirstLevelLag2, double dFirstLevelRef, double dFirstLevelRefLag, double dFirstLevelRefLag2
                AggContainer ac = new AggContainer(dataframeRow.storageValues[Globals.d].data, dataframeRow.storageValues[Globals.dAlternative].data, dataframeRow.storageValues[Globals.dLevel].data, dataframeRow.storageValues[Globals.dLevelLag].data, dataframeRow.storageValues[Globals.dLevelLag2].data, dataframeRow.storageValues[Globals.dLevelRef].data, dataframeRow.storageValues[Globals.dLevelRefLag].data, dataframeRow.storageValues[Globals.dLevelRefLag2].data, 1, new List<string>() { dataframeRow.storageValues[Globals.dNames].text }, dataframeRow.storageValues[Globals.dPrimeShare].data,      
                    dataframeRow.storageValues[Globals.dFirstLevelLag].data,
                    dataframeRow.storageValues[Globals.dFirstLevelLag2].data,
                    dataframeRow.storageValues[Globals.dFirstLevelRef].data, 
                    dataframeRow.storageValues[Globals.dFirstLevelRefLag].data, 
                    dataframeRow.storageValues[Globals.dFirstLevelRefLag2].data);
                pivotTable[rowKey][colKey].Add(ac);  //The list of these values will be aggregated later on
            }
            
            // Apply the aggregation function to each cell
            var resultTable = new Dictionary<string, Dictionary<string, AggContainer>>();
            foreach (var rowEntry in pivotTable)
            {
                resultTable[rowEntry.Key] = new Dictionary<string, AggContainer>();
                foreach (var colEntry in rowEntry.Value)
                {
                    resultTable[rowEntry.Key][colEntry.Key] = agg(colEntry.Value);
                }
            }

            return resultTable;
        }

        /// <summary>
        /// This basically renames elements, for instance "30", "31", "32" ... could each become "30..39", for this interval.
        /// Can also be used to rename stuff like "99-" into "99" or "99..".
        /// </summary>
        /// <param name="selectedIndexes"></param>
        /// <param name="group"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private static string GekkoPivotGroup(List<int> selectedIndexes, Func<FrameLightRow, int, string> group, FrameLightRow row, int lhsFrameCol, DecompOptions2 decompOptions2)
        {
            string rowKey = null;

            bool hasVarsSelected = false;
            int iVars = -12345; row.parent.frameDimensionNames.TryGetValue(Globals.col_variable, out iVars);
            if (selectedIndexes.Contains(iVars)) hasVarsSelected = true;

            bool hasTimeSelected = false;
            int iTime = -12345; row.parent.frameDimensionNames.TryGetValue(Globals.col_t, out iVars);
            if (selectedIndexes.Contains(iTime)) hasTimeSelected = true;

            if (decompOptions2.useBracketNames && hasVarsSelected && !hasTimeSelected)
            {
                //What about group()??

                string variableName = row.GetDimension(row.parent, Globals.col_variable).text;

                bool hasLagsSelected = false;
                int iLags = -12345; row.parent.frameDimensionNames.TryGetValue(Globals.col_lag, out iLags);
                if (selectedIndexes.Contains(iLags)) hasLagsSelected = true;
                string lag = null;
                if (hasLagsSelected) lag = row.GetDimension(row.parent, Globals.col_lag).text;                
                
                //TODO: have cols that share dimensions.
                List<int> shownDimensions = GetShownDimensions(row, variableName, selectedIndexes);
                int dims = (int)row.GetDimension(row.parent, Globals.decompDimension2).data;
                string prettyName = variableName;
                if (dims > 0) prettyName += "[";
                for (int dim = 1; dim <= dims; dim++)
                {
                    if (dim > 1) prettyName += ", ";
                    if (shownDimensions.Contains(dim)) prettyName += row.GetDimension(row.parent, variableName + Globals.decompDimension + dim).text;
                    else prettyName += "*";  //This is not conceptually the same as a domainless dimension
                }
                if (dims > 0) prettyName += "]";  //For bracket-style, "[0]" are removed late in the table processing, so that [-1] gets sorted before [0] and [+1] etc.                
                prettyName += lag;

                if (row.GetDimension(row.parent, Globals.col_lhs).text == Globals.pivotHelper2New)
                {
                    prettyName = Globals.pivotHelper2New + prettyName;
                }               

                rowKey = prettyName;
            }
            else
            {
                string s = null;
                foreach (int i in selectedIndexes)
                {
                    string groupName = group(row, i);
                    if (groupName == null) groupName = Globals.decompNull;                    
                    if (groupName == Globals.pivotHelper2New)
                    {
                        s = groupName + s;  //Just put "00000000 " on the left
                    }
                    else
                    {
                        if(i != lhsFrameCol)s += groupName + Globals.pivotTableDelimiter;  //skip lhs dimension
                    }                    
                }
                if (s != null) rowKey = G.Substring(s, 0, s.Length - Globals.pivotTableDelimiter.Length - 1);
                else rowKey = Globals.decompNull;
            }            
            return rowKey;
        }

        /// <summary>
        /// For a dataframe row, a variable name and the selected columns, the method returns the dimensions of the variable 
        /// that are selected ("active"), so the others can be shown as "*". Note: the returned list is 1-based.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="variableName"></param>
        /// <param name="selectedIndexes"></param>
        /// <returns></returns>
        private static List<int> GetShownDimensions(FrameLightRow row, string variableName, List<int> selectedIndexes)
        {
            List<int> chosenDims = new List<int>();            
            foreach (KeyValuePair<string, int> kvp in row.parent.frameDimensionNames)
            {
                string colName = kvp.Key;
                int colI = kvp.Value;
                if (selectedIndexes.Contains(colI))
                {
                    if (G.StartsWith(colName, variableName))  //"x1 dim 1" starts with x1
                    {
                        int variableDim = int.Parse(colName.Split(Globals.decompDimension)[1]);
                        if (!chosenDims.Contains(variableDim)) chosenDims.Add(variableDim);
                    }
                    else if (G.StartsWith(colName, "#"))
                    {
                        int dimNumber = (int)row.GetDimension(row.parent, colName.Replace('#', Globals.decompSetDimNumberChar)).data;
                        if (!chosenDims.Contains(dimNumber)) chosenDims.Add(dimNumber);                        
                    }
                }
            }            
            return chosenDims;
        }

        public static void CreatePivotTable2()
        {
            // Example data with arbitrary number of elements in each row

            Program.RunGekkoCommands(@"reset; time 1970 2024; import <all px array> c:\Thomas\Gekko\regres\Models\Decomp\befolk1.px;", "", 0, new P());
            Series ts = O.GetIVariableFromString("befolk1!a", O.ECreatePossibilities.NoneReportError) as Series;
            GekkoTime t1 = new GekkoTime(EFreq.A, 2024, 1, 1);
            GekkoTime t2 = new GekkoTime(EFreq.A, 2024, 1, 1);

            List<FrameLightRow> data = new List<FrameLightRow>();
            List<MultidimElement> keys1 = ts.dimensionsStorage.storage.Keys.ToList();
            keys1.Sort(Multidim.CompareMultidimElements);
            for (int i = 0; i < keys1.Count; i++)
            {
                MultidimElement mm1 = keys1[i];
                Series sub1 = ts.dimensionsStorage.storage[mm1] as Series;
                foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
                {
                    FrameLightRow r = new FrameLightRow();
                    foreach (string s in mm1.storage)
                    {
                        r.storageDimensions.Add(new CellLight(s));
                    }
                    double v = sub1.GetDataSimple(t);
                    r.storageValues.Add(new CellLight(v));
                    data.Add(r);
                }
            }
        }
    }


    public class DecompOutput
    {
        public Table table = null;
        public string ignore = null;
        public List<double> red = null; //lamps
        public List<List<string>> black = null;  //expand/collapse arrows
        public Tuple<bool, bool> rowsOrColsSumUp = null;
        public string invertError = null;

        public DecompOutput(Table table, string ignore, List<double> red, List<List<string>> black)
        {
            this.table = table;
            this.ignore = ignore;
            this.red = red;
            this.black = black;
        }
    }

    public class FindHelper
    {
        public bool mustReturnFromParentMethod = false;
    }

    public class SortHelper
    {        
        public int position = -12345;
        public double value = double.NaN;
        public string name = null;
        public override string ToString()
        {
            return position + " --- " + value + " ---name--- " + name;
        }
    }

    public class DecompOperator
    {
        //remember Clone()

        //A bit stupid that the "x" is part of operatorLower whereas "r" is not.
        //Maybe remove the "x" and use isRaw like isReference.

        //--------------------------------------------------------------- 
        //----- These GUI elements are controllable from Gekko syntax -------- cf. #8yuads79afyghr in DecompOptions2
        //--------------------------------------------------------------- 
        private string operatorLower = null;
        //--------------------------------------------------------------- 

        public bool isPercentageType = false; //for formatting        
        public bool isRaw = false;
        public bool isReference = false;

        public bool isDoubleDifQuo = false;  //codes that contain 'dp'
        public bool isDoubleDifRef = false;  //codes that contain 'rdp'
        public Decomp.ELowLevel lowLevel = Decomp.ELowLevel.Unknown; //.BothQuoAndRef --> <mp> or <xmp> type
        public List<int> lagData = new List<int>() { 0, 0 };
        public List<int> lagGradient = new List<int>() { 0, 0 };
        public Decomp.EContribType type = Decomp.EContribType.Unknown;

        public DecompOperator()
        {
        }

        /// <summary>
        /// Returns for instance 'xrd' or 'rd' or 'xd' or 'd'. Never contains 's'.
        /// </summary>
        /// <returns></returns>
        public string OperatorLower()
        {
            return this.operatorLower;
        }        

        public DecompOperator(string x)
        {
            if (x == "n" || x == "r" || x == "rn")
            {
                new Error("Please use operator 'x" + x + "' instead of '" + x + "'.");
            }
            bool good = false;
            if (x == "x" || x == "xn")
            {
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
            }
            else if (x == "xr" || x == "xrn")
            {
                this.isRaw = true;
                this.isReference = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
            }

            // ----------------------------------------------------
            //col 1
            // ----------------------------------------------------

            else if (x == "xd")
            {
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -1, 0 };
            }
            else if (x == "xp")
            {
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -1, 0 };
                this.isPercentageType = true;
            }
            else if (x == "xdp")
            {
                this.isRaw = true;
                this.isDoubleDifQuo = true;
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -2, 0 };
                this.isPercentageType = true;
            }

            // ----------------------------------------------------
            //col 2
            // ----------------------------------------------------

            else if (x == "d")
            {
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.D;
            }
            else if (x == "p")
            {
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.D;
                this.isPercentageType = true;
            }
            else if (x == "dp")
            {
                this.isDoubleDifQuo = true;
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -2, 0 };
                this.lagGradient = new List<int>() { -2, -1 };
                this.type = Decomp.EContribType.D;
                this.isPercentageType = true;
            }

            // ----------------------------------------------------
            //col 3
            // ----------------------------------------------------
            else if (x == "xm")
            {
                this.lowLevel = Decomp.ELowLevel.Multiplier;
                this.isRaw = true;
            }
            else if (x == "xq")
            {
                this.lowLevel = Decomp.ELowLevel.Multiplier;
                this.isRaw = true;
                this.isPercentageType = true;
            }
            else if (x == "xmp")
            {
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.BothQuoAndRef;
                this.lagData = new List<int>() { -1, 0 };
                this.isPercentageType = true;
            }

            // ----------------------------------------------------
            //col 4
            // ----------------------------------------------------
            else if (x == "m")
            {
                this.lowLevel = Decomp.ELowLevel.Multiplier;
                this.type = Decomp.EContribType.M;
            }
            else if (x == "q")
            {
                this.lowLevel = Decomp.ELowLevel.Multiplier;
                this.type = Decomp.EContribType.M;
                this.isPercentageType = true;
            }
            else if (x == "mp")
            {
                this.lowLevel = Decomp.ELowLevel.BothQuoAndRef;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.M;
                this.isPercentageType = true;
            }


            // ----------------------------------------------------
            //ref col 1
            // ----------------------------------------------------

            else if (x == "xrd")
            {
                this.isRaw = true;
                this.isReference = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -1, 0 };
            }
            else if (x == "xrp")
            {
                this.isRaw = true;
                this.isReference = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -1, 0 };
                this.isPercentageType = true;
            }
            else if (x == "xrdp")
            {
                this.isRaw = true;
                this.isReference = true;
                this.isDoubleDifRef = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -2, 0 };
                this.isPercentageType = true;
            }

            // ----------------------------------------------------
            //ref col 2
            // ----------------------------------------------------

            else if (x == "rd")
            {
                this.isReference = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.RD;
            }
            else if (x == "rp")
            {
                this.isReference = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.RD;
                this.isPercentageType = true;
            }
            else if (x == "rdp")
            {
                this.isReference = true;
                this.isDoubleDifRef = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -2, 0 };
                this.lagGradient = new List<int>() { -2, -1 };
                this.type = Decomp.EContribType.RD;
                this.isPercentageType = true;
            }

            // -------------------------------

            else
            {
                new Error("Illegal operator '" + x + "'");
            }
            this.operatorLower = x;
        }

        public DecompOperator Clone()
        {
            DecompOperator rv = new DecompOperator();
            rv.isPercentageType = this.isPercentageType;
            rv.operatorLower = this.operatorLower;
            rv.isRaw = this.isRaw;
            rv.isDoubleDifQuo = this.isDoubleDifQuo;
            rv.isDoubleDifRef = this.isDoubleDifRef;
            rv.lowLevel = this.lowLevel;
            rv.lagData = new List<int>(); rv.lagData.AddRange(this.lagData);
            rv.lagGradient = new List<int>(); rv.lagGradient.AddRange(this.lagGradient);
            rv.type = this.type;
            return rv;
        }
    }

    public class AggContainer
    {
        public double change;
        public double changeAlternative;
        public double level;
        public double levelLag;
        public double levelLag2;
        public double levelRef;
        public double levelRefLag;
        public double levelRefLag2;
        public int n;
        public List<string> fullVariableNames;
        //public string backgroundColor;
        public double prime; //used to see if elements should sum up
        //Values from lhs/endo variable
        public double dFirstLevelLag;
        public double dFirstLevelLag2;
        public double dFirstLevelRef;
        public double dFirstLevelRefLag;
        public double dFirstLevelRefLag2;

        public AggContainer(double change, double changeAlternative, double level, double levelLag, double levelLag2, double levelRef, double levelRefLag, double levelRefLag2, int n, List<string> fullVariableNames, double primeShare, double dFirstLevelLag, double dFirstLevelLag2, double dFirstLevelRef, double dFirstLevelRefLag, double dFirstLevelRefLag2)
        {
            this.change = change;
            this.changeAlternative = changeAlternative;
            this.level = level;
            this.levelLag = levelLag;
            this.levelLag2 = levelLag2;
            this.levelRef = levelRef;
            this.levelRefLag = levelRefLag;
            this.levelRefLag2 = levelRefLag2;
            this.n = n;
            this.fullVariableNames = fullVariableNames;
            //this.backgroundColor = backgroundColor;
            this.prime = primeShare;
            // -----
            this.dFirstLevelLag = dFirstLevelLag;
            this.dFirstLevelLag2 = dFirstLevelLag2;
            this.dFirstLevelRef = dFirstLevelRef;
            this.dFirstLevelRefLag = dFirstLevelRefLag;
            this.dFirstLevelRefLag2 = dFirstLevelRefLag2;
        }
    }

    public class Decomp
    {
        public enum EContribType
        {
            Unknown,
            N,   //probably not used much
            RN,  //probably not used much
            D,
            RD,
            M
        }

        public enum ERowsCols
        {
            Rows,
            Cols,
            None
        }

        public enum ELowLevel
        {
            Unknown,
            OnlyQuo,
            OnlyRef,
            Multiplier,
            BothQuoAndRef  //only for <mp> and <xmp> type
        }

        /// <summary>
        /// Tells normalize method how to sum up
        /// </summary>
        public enum ENormalizeType
        {
            Normal,     //only takes y[0]
            Lags        //will sum up lags like y[-1], y, y[+1]
        }



        /// <summary>
        /// The starting point of DECOMP, collecting decomp options etc.
        /// </summary>
        /// <param name="o"></param>
        public static void DecompStart(O.Decomp2 o)
        {            
            Model model = Program.model;

            if (model.modelCommon.GetModelSourceType() == EModelType.Unknown) new Error("DECOMP: It seems no model is loaded, cf. the MODEL statement");
            bool isGekko = false; if (model.modelCommon.GetModelSourceType() == EModelType.Gekko) isGekko = true;
            bool isGamsRaw = false; if (model.modelCommon.GetModelSourceType() == EModelType.GAMSRaw) isGamsRaw = true;

            if (G.NullOrEmpty(o.opt_prtcode)) o.opt_prtcode = "xn";

            if (!isGekko && o.from.Count == 0)
            {
                O.Find find = new O.Find();
                find.t1 = o.t1;
                find.t2 = o.t2;
                //find.opt_prtcode = o.opt_prtcode;
                find.oDecomp = o;
                find.iv = o.select[0] as List;
                find.Exe();
                return;
            }

            //In general, uncontrolled sets produce a list of equations. Hard to prune these, it is a bit like the lag problem, only lazy 
            //  eval might help.
            //In an equation like y[#a] = x[#a] + 5, there will be 100 equations if #a is 1..100. For each of these, lags are tried. So
            //it is checked if x[31][2000] affects y[31][2001] --> a lag. If such a lag is detected, x[#a][-1] is added to the variables
            //that contribute.

            //See source code documentation

            Globals.lastDecompTable = null;
            G.CheckLegalPeriod(o.t1, o.t2);

            DecompOptions2 decompOptions2 = null;
            if (o.decompFind != null)
            {                
                decompOptions2 = o.decompFind.decompOptions2;
                if (o.decompFind.depth < 2 && o.decompFind.children.Count == 0) SetSomeDecompOptions(decompOptions2, o);
            }
            else
            {
                decompOptions2 = new DecompOptions2();
                decompOptions2.showErrors = false; //
                decompOptions2.t1 = o.t1;
                decompOptions2.t2 = o.t2;
                decompOptions2.expressionOld = o.label;
                decompOptions2.expression = o.expression;
                decompOptions2.decompOperator = new DecompOperator(o.opt_prtcode.ToLower());
                SetSomeDecompOptions(decompOptions2, o);
                decompOptions2.name = o.name;
                decompOptions2.isNew = true;
                o.decompFind = new DecompFind(EDecompFindNavigation.Decomp, 0, decompOptions2, null, model);
            }
            decompOptions2.guiIsFlowStatement = o.isFlowStatement;

            if (o.rows.Count > 0) decompOptions2.rows = O.Restrict(o.rows[0] as List, false, true, false, false);
            if (o.cols.Count > 0) decompOptions2.cols = O.Restrict(o.cols[0] as List, false, true, false, false);
            if (decompOptions2.rows.Count == 0 && decompOptions2.cols.Count == 0)
            {
                ResetRowsColsSelection(decompOptions2);
            }

            decompOptions2.type = o.type;

            foreach (List<IVariable> liv in o.where)
            {
                //pivotfix 
                //'a' in #i
                string x1 = O.ConvertToString(liv[0]);
                List<string> x2 = O.Restrict(liv[1] as List, false, true, false, false);
                decompOptions2.where.Add(new List<string>() { x1, x2[0] });
            }

            foreach (List<IVariable> liv in o.group)
            {
                //
                List<string> x1 = O.Restrict(liv[0] as List, false, true, false, false);
                List<string> x2 = O.Restrict(liv[1] as List, false, true, false, false);
                string x3 = O.ConvertToString(liv[2]);
                string x4 = O.ConvertToString(liv[3]);
                decompOptions2.group.Add(new List<string>() { x1[0], x2[0], x3, x4 });
            }

            foreach (DecompItems liv in o.decompItems)
            {
                //
                List<string> x1 = O.Restrict(liv.varnames as List, false, true, false, true);
                List<string> x2 = O.Restrict(liv.eqname as List, false, true, false, false);
                Link temp = new Link();
                if (x1 != null)
                {
                    temp.varnames = G.HandleBlanksRemove(x1[0]);
                }
                if (x2 != null) temp.eqname = x2[0];
                temp.expressions = new List<Func<GekkoSmpl, IVariable>>() { liv.expression };
                if (liv.option != null) temp.option = liv.option.ConvertToString(); //"lead"
                decompOptions2.link.Add(temp);
            }

            if (decompOptions2.type == "ASTDECOMP3" || model.DecompType() == EModelType.GAMSScalar)
            {
                //Here, for scalar we need to assemble the equations like this:
                // e1[a, 2001], e1[a, 2001], e1[b, 2002], e1[b, 2002], e2[x, 2001], e2[x, 2001], e2[y, 2002], e2[y, 2002]
                // Produces 2 Link objects, each consisting of a list of 2 sub-objects.
                // These sub-objects should provide params that makes it possible to call
                //       the for instance e1[a] by GekkoTime, so it can call
                //       e1[a][2001a1], e1[a][2002a1], etc.
                // Maybe use an array with distance from t0, and .Observations(...). Faster than dict lookup.
                
                if (o.select.Count > 0) decompOptions2.new_select = O.Restrict(o.select[0] as List, false, false, false, true);
                if (o.from.Count > 0) decompOptions2.new_from = O.Restrict(o.from[0] as List, false, false, false, true);
                if (o.endo.Count > 0) decompOptions2.new_endo = O.Restrict(o.endo[0] as List, false, false, false, true);

                bool handleAsGekko = isGekko && (o.decompFind.parent == null || o.decompFind.parent.type == EDecompFindNavigation.Decomp);
                HandleFromAndEndo(decompOptions2, handleAsGekko, isGamsRaw);

                if (false)
                {
                    if (handleAsGekko)
                    {
                        if (o.from.Count == 0)
                        {
                            decompOptions2.new_from = new List<string>() { Globals.decompGekkoEquationPrefix + decompOptions2.new_select[0] };
                            if (decompOptions2.new_endo == null || decompOptions2.new_endo.Count == 0)
                            {
                                decompOptions2.new_endo = new List<string>() { decompOptions2.new_select[0] };
                            }
                        }
                        else
                        {
                            decompOptions2.new_from = O.Restrict(o.from[0] as List, false, false, false, true);
                            decompOptions2.new_endo = new List<string>() { decompOptions2.new_select[0] };
                        }
                    }
                    else
                    {
                        decompOptions2.new_from = O.Restrict(o.from[0] as List, false, false, false, true);  //eqs may be e[a, b] etc.                                    
                        if (decompOptions2.new_from != null && decompOptions2.new_from.Count == 1 && o.endo.Count == 0)
                        {
                            //For something like "decomp y from e_y ..." we do not need to write "decomp y from e_y endo y ..."
                            decompOptions2.new_endo = new List<string>() { decompOptions2.new_select[0] };
                        }
                        else
                        {
                            decompOptions2.new_endo = O.Restrict(o.endo[0] as List, false, false, false, true);
                        }
                    }
                }

                for (int i = 0; i < decompOptions2.new_select.Count; i++) decompOptions2.new_select[i] = G.HandleBlanksRemove(decompOptions2.new_select[i]);
                for (int i = 0; i < decompOptions2.new_from.Count; i++) decompOptions2.new_from[i] = G.HandleBlanksRemove(decompOptions2.new_from[i]);
                for (int i = 0; i < decompOptions2.new_endo.Count; i++) decompOptions2.new_endo[i] = G.HandleBlanksRemove(decompOptions2.new_endo[i]);

                if (model.DecompType() == EModelType.GAMSScalar)
                {
                    model.modelGamsScalar.MaybeLoadDataIntoModel(o.decompFind.depth, decompOptions2.t1, decompOptions2.t2, decompOptions2.missingAsZero, false);
                }
                else
                {
                    int counter = -1;
                    foreach (string s in decompOptions2.new_from)
                    {
                        counter++;
                        Link link = new Link();
                        link.eqname = s;
                        if (counter == 0)
                        {
                            link.endo = new List<string>();
                            foreach (string s10 in decompOptions2.new_endo)
                            {
                                link.endo.Add(s10);
                            }
                            //link.endo.AddRange(decompOptions2.new_endo);
                            link.varnames = decompOptions2.new_select[0];
                        }
                        else
                        {
                            //is this still necessary?
                            link.varnames = "<not used>"; //strange but necessary further on
                        }
                        link.expressions = new List<Func<GekkoSmpl, IVariable>>();
                        link.expressions.Add(null); //strange but necessary further on
                        decompOptions2.link.Add(link);
                    }
                }
            }

            if (o.decompFind.decompOptions2.isShares && o.decompFind.decompOptions2.decompOperator.isRaw)
            {
                MessageBox.Show("DECOMP: You cannot mix option <shares> with 'Raw' operators like <xn>, <xd>, <xm>, etc.");
                return;
            }
            
            Decomp.DecompGetFuncExpressionsAndRecalc(o.decompFind, null);
        }

        public static void SetSomeDecompOptions(DecompOptions2 decompOptions2, O.Decomp2 o)
        {
            if (o == null) return;
            if (G.Equal(o.opt_shares, "yes")) decompOptions2.isShares = true;
            if (G.Equal(o.opt_count, "yes") && G.Equal(o.opt_names, "yes")) new Error("You cannot use option <count> and <names> at the same time");
            if (G.Equal(o.opt_count, "yes")) decompOptions2.count = ECountType.N;
            if (G.Equal(o.opt_names, "yes")) decompOptions2.count = ECountType.Names;
            if (G.Equal(o.opt_dyn, "yes")) decompOptions2.dyn = true;
            if (G.Equal(o.opt_errors, "yes")) decompOptions2.showErrors = true;
            if (G.Equal(o.opt_missing, "zero") || G.Equal(o.opt_missing, "ignore"))
            {
                //Use <missing=ignore>, not <missing=zero>, but the latter will be allowed for now.
                decompOptions2.missingAsZero = true;
            }
            if (G.Equal(o.opt_sort, "yes")) decompOptions2.sort = true;
            if (G.Equal(o.opt_plot, "yes")) decompOptions2.plot = true;
            if (G.Equal(o.opt_expand, "yes")) decompOptions2.expand = true;
            if (!double.IsNaN(o.opt_ignore))
            {
                if (o.opt_ignore < 0d || o.opt_ignore > 100d)
                {
                    new Error("Option <ignore=...> must be between 0 and 100 (inclusive). The value is " + o.opt_ignore + ".");
                }
                decompOptions2.ignore = o.opt_ignore;
            }
        }

        /// <summary>
        /// Handles the FROM and ENDO arguments, filling them out if needed, and issuing errors.
        /// </summary>
        /// <param name="decompOptions2"></param>
        /// <param name="handleAsGekko"></param>
        private static void HandleFromAndEndo(DecompOptions2 decompOptions2, bool handleAsGekko, bool isGamsRaw)
        {
            //Now we have equation(s) and endo(s). Beware: o.selectnew_select always has 1 element.
            //We then have o.from and o.endo.                
            // 
            //A. decomp y;            
            //B. decomp y            endo y;
            //C. decomp y from e_y;   
            //E. decomp y from e_y   endo y;

            if (!isGamsRaw)
            {
                //GamsRaw is weird anyway!! Probably obsolete sooner or later.                
                if (decompOptions2.new_select.Count != 1) new Error("DECOMP: Expected 1 variable to be selected, not a list");
                if (decompOptions2.new_from.Count > 0 && decompOptions2.new_endo.Count > 0 && decompOptions2.new_from.Count != decompOptions2.new_endo.Count) new Error("DECOMP: The number of elements in FROM and ENDO must match");
            }
            if (decompOptions2.new_from.Count == 0 && decompOptions2.new_endo.Count > 0 && decompOptions2.new_endo.Count != 1) new Error("DECOMP: You must provide 1 ENDO variable");
            if (decompOptions2.new_endo.Count > 0 && !G.EqualHandleBlanks(decompOptions2.new_select[0], decompOptions2.new_endo)) new Error("DECOMP: The decomp variable must be one of the ENDO variables");

            if (decompOptions2.new_from.Count == 0)
            {
                if (decompOptions2.new_endo.Count == 0)
                {
                    //A. decomp y;            
                    if (handleAsGekko)
                    {
                        //Becomes: decomp y from e_y endo y
                        decompOptions2.new_from = new List<string>() { Globals.decompGekkoEquationPrefix + decompOptions2.new_select[0] };
                        decompOptions2.new_endo = new List<string>() { decompOptions2.new_select[0] };
                    }
                    else
                    {
                        //Do nothing (calls find window)
                    }
                }
                else
                {
                    //B. decomp y endo y;
                    new Error("DECOMP: ENDO is only used together with FROM");
                }
            }
            else
            {
                if (decompOptions2.new_endo.Count == 0)
                {
                    //C. decomp y from e_y;                        
                    if (decompOptions2.new_from.Count == 1)
                    {
                        //Becomes: decomp y from e_y endo y
                        decompOptions2.new_endo = new List<string>() { decompOptions2.new_select[0] };
                    }
                    else
                    {
                        new Error("DECOMP: When stating several FROM equations, you must provide ENDO variables");
                    }
                }
                else
                {
                    //D. decomp y from e_y endo y;
                    //Do nothing                    
                }
            }
        }

        /// <summary>
        /// Hooks up to GAMS scalar model
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="s"></param>
        /// <param name="equationName"></param>
        /// <param name="mmi"></param>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <param name="doubleDif"></param>
        private static void FindEquationsForEachRelevantPeriod(GekkoTime t1, GekkoTime t2, string s, string equationName, MultidimElement mmi, DecompStartHelper element, DecompOperator op, bool showErrors, ModelGamsScalar modelGamsScalar)
        {
            int deduct = op.lagGradient[0];
            if (op.isRaw) deduct = op.lagData[0];

            GekkoTime gt1 = t1.Add(deduct);
            GekkoTime gt2 = t2;

            if (Program.options.bugfix_decomp_lagsleads)
            {
                gt1 = gt1.Add(-Globals.decomp_offset);
                gt2 = gt2.Add(-Globals.decomp_offset);
            }

            if (element.offset != 0)
            {
                gt1 = gt1.Add(element.offset);
                gt2 = gt2.Add(element.offset);
            }

            if (modelGamsScalar.isPerpetualModel)
            {
                gt1 = new GekkoTime(modelGamsScalar.parent.modelCommon.GetFreq(), Globals.decomp2000, 1);
                gt2 = new GekkoTime(modelGamsScalar.parent.modelCommon.GetFreq(), Globals.decomp2000, 1);
            }

            foreach (GekkoTime time in new GekkoTimeIterator(gt1, gt2))
            {
                int i = time.Subtract(modelGamsScalar.tBasis);

                if (modelGamsScalar.isPerpetualModel) i = 0;
                if (i < 0 || i > element.periods.Length - 1)
                {
                    if (showErrors) new Error("Period " + time.ToString() + " outside GAMS scalar model period. " + modelGamsScalar.GamsModelDefinedString() + ". You may want to adjust the DECOMP time period.");
                    return;
                }
                if (element.periods[i] != null) new Error("Dublet equation: " + equationName + mmi.GetName() + " in " + time.ToString());
                DecompStartHelperPeriod elementPeriod = new DecompStartHelperPeriod();
                //Below: must be string like "e1[2001]" or "e1[a, 2001]", etc.
                //

                //string s2 = AddTimeToIndexes(s, time);

                string s2 = G.Chop_DimensionAddLast(s, time.ToString(), null);

                int eqNumber = modelGamsScalar.dict_FromEqNameToEqNumber.GetInt(s2);
                if (eqNumber == -12345)
                {
                    string s3 = null;
                    try
                    {
                        if (modelGamsScalar.parent.modelCommon.GetModelSourceType() == EModelType.Gekko)
                        {
                            s3 = " In the decomp window, try using Ctrl+click instead of normal click to find the equations in which the variable appears.";
                        }
                    }
                    catch { }
                    if (showErrors)
                    {                        
                        new Error("Could not find the equation '" + s2 + "'." + s3);                        
                    }
                }
                //int eqNumber = modelGamsScalar.dict_FromEqNameToEqNumber[s2];
                elementPeriod.eqNumber = eqNumber;
                elementPeriod.t = time;
                element.periods[i] = elementPeriod;
            }
        }

        /// <summary>
        /// For a name like "x" and a list like {"a", "b"}, time is added and returns like for instance "x[a,b,2001]".
        /// Note no blanks. If separate==true, it becomes "x[a,b][2001]".
        /// </summary>
        /// <param name="name2"></param>
        /// <param name="indexes2"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string AddTimeToIndexes(string name2, List<string> indexes2, GekkoTime time, bool separate)
        {
            string s2 = null;
            if (separate)
            {
                s2 = G.Chop_GetFullName(null, name2, null, indexes2.ToArray(), null) + "[" + time.ToString() + "]";
            }
            else
            {
                indexes2.Add(time.ToString());
                s2 = G.Chop_GetFullName(null, name2, null, indexes2.ToArray(), null);
            }
            return s2;
        }

        /// <summary>
        /// Main entry to the math part of decomposition. Performs a lot of the hard stuff, including
        /// matrix inversion etc. Calls DecompLowLevel() a lot, where gradients etc. are calculated.
        /// The table returned is a pivot table.
        /// </summary>
        public static DecompOutput DecompMain(GekkoSmpl smpl, GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, ref DecompDatas decompDatas, Model model)
        {
            //See OVERVIEW in DecompGetFuncExpressionsAndRecalc()

            decompOptions2.invertError = null;

            GekkoTime gt1, gt2;
            DecompMainInit(out gt1, out gt2, per1, per2, decompOptions2.decompOperator);

            DateTime t0 = DateTime.Now;

            EContribType operatorOneOf3Types = decompOptions2.decompOperator.type;

            int perLag = -2;
            string lhsString = "Expression value";
            int parentI = 0;

            //MAIN varnames are: decompOptions2.link[parentI].varnames

            // decompDatas THE TEXT BELOW IS OBSOLETE
            // Example: DECOMP x[#a] from e1, e2 endo x[#a], y[#a];
            // 1. dimension corresponds to main chosen decomp variables (x[#a], could be stated like x[18], x[19]).
            //    This dimension corresponds to super = 0, 1 here.
            // 2. dimension is the raw link equations (including main equation with number 0). The raw link
            //    equations are e1 and e2.
            // 3. dimension corresponds to uncontrolled lists like x[#a] or x[#a, #i] in each link equation (including the main equation)
            //    So 3. dimension unfolds the raw equations, for instance x[#a] in e1 and y[#a] in e2.
            // The 1. dimension (super) is kind of like link variables, but where the x[#a] variables have no "mother" equation
            // to be put into. Each super element is adjusted on its own, and stuff sums to 0. In reporting, this is
            // shown as "equ" to choose/pivot from

            int funcCounter = 0;
            //G.Writeln2(">>>Before low level " + DateTime.Now.ToLongTimeString());

            if (model.DecompType() == EModelType.GAMSScalar)
            {
                PrepareEquations(per1, per2, decompOptions2.decompOperator, decompOptions2, true, model.modelGamsScalar);
            }

            if (decompDatas.storage == null) decompDatas.storage = new List<List<DecompData>>();
            decompDatas.MAIN_data = null;

            //MAYBE DO THIS BY LOOKING INSIDE DECOMPDATAS...
            //when putting in raw data (cellsQuo, cellsRef), maybe put them in for the full period (fast anyway)                

            if (decompDatas.storage == null || decompDatas.storage.Count == 0) InitDecompDatas(decompOptions2, decompDatas, model);

            List<string> expressionTexts = new List<string>();
            int ii = -1;
            foreach (Link link in decompOptions2.link)  //including the "mother" non-linked equation
            {
                ii++;
                string residualName = Program.GetDecompResidualName(ii, decompOptions2.link.Count);

                int jj = -1;
                if (model.DecompType() == EModelType.GAMSScalar)
                {
                    foreach (DecompStartHelper dsh in link.GAMS_dsh)  //unrolling: for each uncontrolled #i in x[#i]
                    {
                        jj++;  //will be = 0
                        DecompData dd = Decomp.DecompLowLevelScalar(gt1, gt2, dsh, decompOptions2.decompOperator, residualName, ref funcCounter, decompOptions2.missingAsZero, model);
                        DecompMainMergeOrAdd(decompDatas, dd, ii, jj);
                    }
                }
                else
                {
                    foreach (Func<GekkoSmpl, IVariable> expression in link.expressions)  //unrolling: for each uncontrolled #i in x[#i]
                    {
                        jj++;
                        DecompData dd = Decomp.DecompLowLevel(per1, per2, expression, DecompBanks_OLDREMOVESOON(decompOptions2.decompOperator), residualName, ref funcCounter);
                        DecompMainMergeOrAdd(decompDatas, dd, ii, jj);
                    }
                }
            }

            if (operatorOneOf3Types == EContribType.D) decompDatas.hasD = true;
            else if (operatorOneOf3Types == EContribType.RD) decompDatas.hasRD = true;
            else if (operatorOneOf3Types == EContribType.M) decompDatas.hasM = true;

            if (decompOptions2.link[parentI].varnames == null)
            {
                //does this ever happen?
                decompOptions2.link[parentI].varnames = Globals.decompResidualName;
            }

            if (false)
            {
                DecompPrintDatas(per1, per2, decompDatas.storage, operatorOneOf3Types);
            }

            bool[] used = new bool[decompDatas.storage.Count];
            used[0] = true;  //primary equation

            GekkoDictionary<string, bool> ignore = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            //linking
            //linking
            //linking

            //------------------------
            //Example: e1: y = c + i + g  --> y - (c + i + g)
            //         e2: c = 0.8 * y    --> c - 0.8 * y
            //------------------------

            if (decompOptions2.type == "ASTDECOMP3")
            {
                if (model.DecompType() == EModelType.GAMSScalar)
                {
                    if (decompOptions2.dyn)
                    {
                        //decomp over time, resolving lags/leads                            

                        if (decompOptions2.decompOperator.lowLevel == ELowLevel.BothQuoAndRef)  //<mp>
                        {
                            DecompMainHelperInvertScalar(per1, per2, decompOptions2, decompDatas, EContribType.D, parentI, true, decompOptions2.decompOperator, model.modelGamsScalar);
                            DecompMainHelperInvertScalar(per1, per2, decompOptions2, decompDatas, EContribType.RD, parentI, false, decompOptions2.decompOperator, model.modelGamsScalar);  //Note: refreshObjects = false!
                        }
                        else
                        {
                            DecompMainHelperInvertScalar(per1, per2, decompOptions2, decompDatas, operatorOneOf3Types, parentI, true, decompOptions2.decompOperator, model.modelGamsScalar);
                        }
                    }
                    else
                    {
                        //decomp period by period, showing lags/leads.

                        if (decompOptions2.decompOperator.lowLevel == ELowLevel.BothQuoAndRef)  //<mp>
                        {
                            bool refreshObjects = true;
                            foreach (GekkoTime gt in new GekkoTimeIterator(per1, per2))
                            {
                                DecompMainHelperInvertScalar(gt, gt, decompOptions2, decompDatas, EContribType.D, parentI, refreshObjects, decompOptions2.decompOperator, model.modelGamsScalar);
                                refreshObjects = false;
                            }
                            foreach (GekkoTime gt in new GekkoTimeIterator(per1, per2))
                            {
                                DecompMainHelperInvertScalar(gt, gt, decompOptions2, decompDatas, EContribType.RD, parentI, refreshObjects, decompOptions2.decompOperator, model.modelGamsScalar);
                            }
                        }
                        else
                        {
                            int deduct = 0;
                            //why deduct not enough??
                            if (decompOptions2.decompOperator.isDoubleDifQuo || decompOptions2.decompOperator.isDoubleDifRef) deduct = -1;  //all the data are ready, so we can calc 1 period earlier, so that a 1-period decomp actually shows something for <dp> or <rdp>
                            bool refreshObjects = true;
                            foreach (GekkoTime gt in new GekkoTimeIterator(per1.Add(deduct), per2))
                            {
                                DecompMainHelperInvertScalar(gt, gt, decompOptions2, decompDatas, operatorOneOf3Types, parentI, refreshObjects, decompOptions2.decompOperator, model.modelGamsScalar);
                                refreshObjects = false;
                            }
                        }
                    }
                }
                else
                {
                    DecompMainHelperInvert(per1, per2, decompOptions2, decompDatas, operatorOneOf3Types, parentI, model);
                }
            }

            //At this point, all linked equations i = 1, 2, ... have been merged into
            //the MAIN equation i = 0.    

            if (false)
            {
                int i = -1;
                foreach (List<DecompData> x in decompDatas.storage)
                {
                    i++;
                    int j = -1;
                    foreach (DecompData y in x)
                    {
                        j++;
                        new Writeln("COMBINATION =====> " + i + " " + j);
                        PrintDecompData(y);
                    }
                }
                if (true && decompDatas.MAIN_data != null && decompDatas.MAIN_data != null)
                {
                    new Writeln("...");
                    new Writeln("...");
                    new Writeln("MAIN MAIN MAIN MAIN MAIN MAIN MAIN MAIN ");
                    PrintDecompData(decompDatas.MAIN_data);
                }
            }

            //decompDatas[parentI] is the main equation, the other ones are in-substituted. This decompDatas[parentI] has a member
            //for each uncontrolled set like #a. The main variables (MAIN_varnames) are normalized to 1.
            //decompData.cellsContribD contains keys like "Work:y[19]¤[+1]" with values as timeseries.
            //This example is split into y, #a, 1, t, value --> so we get a dataframe row like this:
            //eq=0, variable=y, #a = 19, lag=1, t=2010, 1.2345
            //We clone the data first, before calling DecompPivotToTable(), because they may be normalized etc.                         
            //We are cloning decompDataMAINClone this, because normalization may take place when doing the table
            DecompData decompDataMAINClone = decompDatas.MAIN_data.DeepClone();

            DecompOutput decompOutput = null;
            if (model.DecompType() == EModelType.GAMSScalar)  //will also include "perpetual" Gekko models, but not GAMS-raw-code models.
            {
                decompOutput = Decomp.DecompPivotToTable(smpl, per1, per2, decompDataMAINClone, decompDatas, lhsString, decompOptions2.decompOperator, operatorOneOf3Types, decompOptions2, model);
            }
            else
            {
                decompOutput = Decomp_OLD.DecompPivotToTable_OLD(smpl, per1, per2, decompDataMAINClone, decompDatas, lhsString, decompOptions2.decompOperator, operatorOneOf3Types, decompOptions2, model);
            }

            if (false)
            {
                DecompPrintDatas(per1, per2, decompDatas.storage, operatorOneOf3Types);
                throw new GekkoException();
            }

            if (Globals.runningOnTTComputer) G.Writeln2("TTH: decomp took " + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + ", function evals = " + funcCounter, System.Drawing.Color.Gray);  //using writeln2 to avoid popup

            decompOutput.invertError = decompOptions2.invertError; //transferring this
            return decompOutput;
        }

        public static void DecompMainInit(out GekkoTime gt1, out GekkoTime gt2, GekkoTime per1, GekkoTime per2, DecompOperator op)
        {
            gt1 = per1;
            gt2 = per2;
            if (op.isRaw)
            {
                gt1 = per1.Add(op.lagData[0]);
                gt2 = per2.Add(op.lagData[1]);
            }
            else
            {
                gt1 = per1.Add(op.lagGradient[0]);
                gt2 = per2.Add(op.lagGradient[1]);
            }
        }

        private static void PrintDecompData(DecompData y)
        {
            new Writeln("cellsQuo --------------------------------");
            PrintDecompDict(y.cellsQuo);

            new Writeln("cellsRef --------------------------------");
            PrintDecompDict(y.cellsRef);

            new Writeln("cellsGradQuo --------------------------------");
            PrintDecompDict(y.cellsGradQuo);

            new Writeln("cellsGradRef --------------------------------");
            PrintDecompDict(y.cellsGradRef);

            new Writeln("cellsContribD --------------------------------");
            PrintDecompDict(y.cellsContribD);

            new Writeln("cellsContribDRef --------------------------------");
            PrintDecompDict(y.cellsContribDRef);

            new Writeln("cellsContribM --------------------------------");
            PrintDecompDict(y.cellsContribM);
        }

        public static ERowsCols VariablesOnRowsOrCols(DecompOptions2 decompOptions2)
        {
            ERowsCols rv = ERowsCols.None;
            if (decompOptions2.rows.Contains(Globals.col_variable, StringComparer.OrdinalIgnoreCase)) rv = ERowsCols.Rows;
            else if (decompOptions2.cols.Contains(Globals.col_variable, StringComparer.OrdinalIgnoreCase)) rv = ERowsCols.Cols;
            if (decompOptions2.expand) rv = ERowsCols.Rows;
            return rv;
        }

        /// <summary>
        /// Make sure the table is suitable for red lamps, or for plotting.
        /// </summary>
        /// <param name="decompOptions2"></param>
        /// <returns></returns>
        public static bool VarsAndTimeDimensionsAreSeparate(DecompOptions2 decompOptions2)
        {
            bool b4 = false;
            if (decompOptions2.rows.Contains(Globals.col_variable, StringComparer.OrdinalIgnoreCase) && decompOptions2.cols.Contains(Globals.col_t, StringComparer.OrdinalIgnoreCase)) b4 = true;
            if (decompOptions2.rows.Contains(Globals.col_t, StringComparer.OrdinalIgnoreCase) && decompOptions2.cols.Contains(Globals.col_variable, StringComparer.OrdinalIgnoreCase)) b4 = true;
            if (decompOptions2.expand) b4 = true;
            return b4;
        }

        public static void InitDecompDatas(DecompOptions2 decompOptions2, DecompDatas decompDatas, Model model)
        {
            decompDatas.storage = new List<List<DecompData>>();
            int ii = -1;
            foreach (Link link in decompOptions2.link)  //including the "mother" non-linked equation
            {
                ii++;
                decompDatas.storage.Add(new List<DecompData>());
                if (model.DecompType() == EModelType.GAMSScalar)
                {
                    foreach (DecompStartHelper dsh in link.GAMS_dsh)  //unrolling: for each uncontrolled #i in x[#i]
                    {
                        DecompData d = new DecompData();
                        DecompInitDict(d);
                        decompDatas.storage[ii].Add(d);
                    }
                }
                else
                {
                    foreach (Func<GekkoSmpl, IVariable> expression in link.expressions)  //unrolling: for each uncontrolled #i in x[#i]
                    {
                        DecompData d = new DecompData();
                        DecompInitDict(d);
                        decompDatas.storage[ii].Add(d);
                    }
                }
            }
        }

        /// <summary>
        /// Gathers info that makes it easier to use the equations for decomp later on.
        /// Only does it for the used equations, not all equations.
        /// Uses lists from .new_from, .new_endo and .new_select.
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="operator1"></param>
        /// <param name="decompOptions2"></param>
        public static void PrepareEquations(GekkoTime per1, GekkoTime per2, DecompOperator operator1, DecompOptions2 decompOptions2, bool showErrors, ModelGamsScalar modelGamsScalar)
        {
            decompOptions2.link = new List<Link>();
            GekkoDictionary<string, Dictionary<MultidimElement, DecompStartHelper>> equations = new GekkoDictionary<string, Dictionary<MultidimElement, DecompStartHelper>>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in decompOptions2.new_from)
            {
                int n = s.Count(c => c == '[');

                if (n > 2) new Error("More than two '[' encountered in equation name");

                string bank = null; string name = null; string freq = null; string[] indexes1 = null; string[] indexes2 = null;

                G.Chop_Chop_Jagged(s, out bank, out name, out freq, out indexes1, out indexes2);

                if (bank != null || freq != null) new Error("Bank or freq not allowed for eq name");

                //string sWithoutLagsLeads = s;
                int i = 0;
                if (indexes2 != null)
                {
                    //Something like e[a,b][-1]
                    if (indexes2.Length != 1) new Error("Expected second index to have 1 element");
                    if (!(indexes2[0].StartsWith("+") || indexes2[0].StartsWith("-"))) new Error("Expected second index to start with '+' or '-'");
                    bool b = int.TryParse(indexes2[0], out i);
                    if (!b) new Error("Expected second index to be an integer lag/lead");
                    //sWithoutLagsLeads = s.Substring(0, s.LastIndexOf('[')); //removes lag/lead
                    indexes2 = null;
                }
                else if (indexes1 != null)
                {
                    //Something like e[a,b], but also e[-1]. We need to check if it is e[-{i}] or e[+{i}] where i is an integer >= 0.
                    //(here, e[-0] or e[+0] will point to the same equation as e, so why would anybody do that?).
                    if (indexes1.Length == 1 && (indexes1[0].StartsWith("+") || indexes1[0].StartsWith("-")))
                    {
                        bool b = int.TryParse(indexes1[0], out i);
                        if (!b) new Error("Expected index to be an integer lag/lead");
                        //sWithoutLagsLeads = s.Substring(0, s.LastIndexOf('[')); //removes lag/lead
                        indexes1 = null;
                    }
                }

                string sWithoutLagsLeads = name;
                if (indexes1 != null) sWithoutLagsLeads += "[" + Stringlist.GetListWithCommas(indexes1, "") + "]";
                if (indexes2 != null) sWithoutLagsLeads += "[" + Stringlist.GetListWithCommas(indexes2, "") + "]";

                //if (indexes1 == null) indexes1 = new string[0];  //a null array is standard way of saying "no indexes", like x having no dimensions unlike x[a,b].
                //For each equation stated
                //Actually there is no time extracted below: the s string hos no time element
                //GekkoTime trash = GekkoTime.tNull;
                //ExtractTimeDimensionHelper helper = GamsModel.ExtractTimeDimension(true, EExtractTimeDimension.Full, s, false);

                Dictionary <MultidimElement, DecompStartHelper> elements = null;
                equations.TryGetValue(name, out elements);
                if (elements == null)
                {
                    elements = new Dictionary<MultidimElement, DecompStartHelper>();
                    equations.Add(name, elements);
                }

                MultidimElement mmi = new MultidimElement(indexes1 == null ? new string[0] : indexes1);
                DecompStartHelper element = null;
                elements.TryGetValue(mmi, out element);
                if (element == null)
                {
                    element = new DecompStartHelper();
                    element.name = name;
                    element.indexes = mmi;
                    //
                    // TODO: add [-1] or [+1] ???
                    //                    
                    element.fullName = element.name + element.indexes.GetName();
                    int periods = GekkoTime.Observations(modelGamsScalar.absoluteT1, modelGamsScalar.absoluteT2);
                    if (modelGamsScalar.isPerpetualModel) periods = 1;
                    element.periods = new DecompStartHelperPeriod[periods];
                    element.offset = i;
                    elements.Add(mmi, element);
                }
                FindEquationsForEachRelevantPeriod(per1, per2, sWithoutLagsLeads, name, mmi, element, operator1, showErrors, modelGamsScalar);
            }

            int counter = -1;
            foreach (KeyValuePair<string, Dictionary<MultidimElement, DecompStartHelper>> kvp in equations)
            {
                //for each equation name                
                counter++;
                Link link = new Link();
                link.GAMS_dsh = new List<DecompStartHelper>();
                foreach (KeyValuePair<MultidimElement, DecompStartHelper> kvp2 in kvp.Value)
                {
                    //for each index combination
                    link.GAMS_dsh.Add(kvp2.Value);
                    link.GAMS_eqNumber = counter;
                }

                //    O.Decomp2 o0 = new O.Decomp2();
                //    o0.type = @"ASTDECOMP3";
                //    o0.label = o.rv;
                //    o0.t1 = o.t1;
                //    o0.t2 = o.t2;
                //    o0.opt_prtcode = o.opt_prtcode;

                //    o0.decompItems = new List<DecompItems>();                    

                //    o0.select.Add(O.FlattenIVariablesSeq(false, new
                //     List(new List<IVariable> { new ScalarString(var) })));

                //    o0.from.Add(O.FlattenIVariablesSeq(false,
                //     new List(new List<IVariable> { new ScalarString(o.rv) })));

                //    o0.endo.Add(O.FlattenIVariablesSeq(false, new List(new
                //     List<IVariable> { new ScalarString(var) })));

                if (counter == 0)
                {
                    if (decompOptions2.new_endo != null)
                    {
                        link.endo = new List<string>();
                        foreach (string s10 in decompOptions2.new_endo)
                        {
                            link.endo.Add(s10);
                        }
                        //link.endo.AddRange(decompOptions2.new_endo);
                    }
                    if (decompOptions2.new_select != null)
                    {
                        link.varnames = decompOptions2.new_select[0];
                    }
                }
                else
                {
                    //is this still necessary?                    
                    link.varnames = "<not used>"; //strange but necessary further on
                }

                decompOptions2.link.Add(link);
            }
        }

        private static void DecompMainHelperInvert(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, DecompDatas decompDatas, EContribType operatorOneOf3Types, int parentI, Model model)
        {
            GekkoDictionary<string, int> endo = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in decompOptions2.link[0].endo)
            {
                string s2 = DecompFirst() + ":" + ConvertToTurtleName(s, 0);
                if (!endo.ContainsKey(s2)) endo.Add(s2, endo.Count); //why if here?
            }

            DecompCheckNumberOfEqsAndEndo(decompDatas, endo);

            GekkoDictionary<string, int> exo = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < decompDatas.storage.Count; i++) //for each linked eq, including the first one
            {
                for (int j = 0; j < decompDatas.storage[i].Count; j++) //for each uncontrolled set in eq
                {
                    foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(decompDatas.storage[i][j], operatorOneOf3Types).storage)
                    {
                        if (exo.ContainsKey(kvp.Key) || endo.ContainsKey(kvp.Key))
                        {
                        }
                        else
                        {
                            exo.Add(kvp.Key, exo.Count);
                        }
                    }
                }
            }

            int n = endo.Count + exo.Count;

            //now we have ENDO = decompOptions2.link[parentI].varnames, and EXO = exo

            //consider this: 
            //1 x1 + 2 x2 + 3 x3 + 4 x4 + 5 x5 = 0 
            //2 x1 + 3 x2 + 4 x3 + 5 x4 + 6 x5 = 0

            //now if x2, x4, x5 are exo, we skip these in Jacobi, getting:
            //
            // [1 3] [x1]  +  [2 4 5] [x2]   =   0
            // [2 4] [x3]     [3 5 6] [x4]     
            //                        [x5]
            //
            // [x1]  =  - [. .] [2 4 5] [x2]  
            // [x3]  =    [. .] [3 5 6] [x4]   
            //                          [x5]

            if (false)
            {
                DecompPrintDatas(per1, per2, decompDatas.storage, operatorOneOf3Types);
            }
            decompDatas.MAIN_data = new DecompData();  //this is where the results end up

            foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
            {

                double[,] mEndo = new double[endo.Count, endo.Count];
                double[,] mExo = new double[endo.Count, exo.Count];

                int row = -1;
                for (int i = 0; i < decompDatas.storage.Count; i++) //for each linked eq, including the first one
                {
                    //In some cases, there is no interaction between the sets, for instance in
                    //an equation like y[#a] = c[#a] + g[#a]. In that case, we could solve for each age separately,
                    //but we cannot rule out eqs like y[#a] = (c[#a] + c[#a+1])/2 + g[#a]. This is still recursive,
                    //if we start to solve for the largest #a, but it illustrates the problem. Perhaps a sparse solver
                    //would not care anyway. Note that c[#a+1][+1] would be more common, and if this is treated as an
                    //exogenous, there is no age lead problem. Note also that combinations of #a-1, #a and #a+1 become
                    //simultaneous (like for time, t).
                    for (int j = 0; j < decompDatas.storage[i].Count; j++) //for each uncontrolled set in eq
                    {
                        row++;
                        foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(decompDatas.storage[i][j], operatorOneOf3Types).storage)
                        {
                            double d = kvp.Value.GetDataSimple(t);
                            if (endo.ContainsKey(kvp.Key))
                            {
                                int col = endo[kvp.Key];
                                if (!(row < mEndo.GetLength(0) && col < mEndo.GetLength(1)))
                                {
                                    new Error("DECOMP matrix invert problem");
                                    //throw new GekkoException();
                                }
                                mEndo[row, col] = d;
                            }
                            else if (exo.ContainsKey(kvp.Key))
                            {
                                int col = exo[kvp.Key];
                                if (!(row < mExo.GetLength(0) && col < mExo.GetLength(1)))
                                {
                                    new Error("DECOMP matrix invert problem");
                                    //throw new GekkoException();
                                }
                                mExo[row, col] = d;
                            }
                            else
                            {
                                throw new GekkoException();
                            }
                        }
                    }
                }

                //ENDO  0 -- 0  demand[18]¤[0] 2021 = 8.88890000004245
                //ENDO  0 -- 0  supply[18]¤[0] 2021 = -8.88890000004245
                //ENDO  0 -- 1  demand[19]¤[0] 2021 = 3.33330000001592
                //ENDO  0 -- 1  supply[19]¤[0] 2021 = -3.33330000001592

                //ENDO  1 -- 0  c[18]¤[0]      2021 = -6.8889000000329
                //ENDO  1 -- 0  demand[18]¤[0] 2021 = 8.88890000004245
                //      1 -- 0  g[18]¤[0]      2021 = -2.00000000000955
                //ENDO  1 -- 1  c[19]¤[0]      2021 = -1.33330000000637
                //ENDO  1 -- 1  demand[19]¤[0] 2021 = 3.33330000001592
                //      1 -- 1  g[19]¤[0]      2021 = -2.00000000000955

                //ENDO  2 -- 0  supply[18]¤[0] 2021 = 8.88890000004245
                //ENDO  2 -- 0  y[18]¤[0]      2021 = -8.88890000004245
                //ENDO  2 -- 1  supply[19]¤[0] 2021 = 3.33330000001592
                //ENDO  2 -- 1  y[19]¤[0]      2021 = -3.33330000001592

                //ENDO  3 -- 0  c[18]¤[0]      2021 = 6.8889000000329
                //ENDO  3 -- 0  y[18]¤[0]      2021 = -3.55556000011804
                //      3 -- 0  y[19]¤[+1]     2021 = -3.33336000011065
                //ENDO  3 -- 1  c[19]¤[0]      2021 = 1.33330000000637
                //ENDO  3 -- 1  y[19]¤[0]      2021 = -1.33331999994953
                //      3 -- 1  y[20]¤[+1]     2021 = 0

                //      y18    y19    dem18    dem19   sup18   sup19   c18    c19
                // ---------------------------------------------------------------------
                //  1                 8.88             -8.88
                //  2                           3.33            -3.33
                //  3                 8.88                             -6.88                   -2 (g18)
                //  4                           3.33                          -1.33            -2 (g19)
                //  5 -8.88                              8.88
                //  6         -3.33                              3.33
                //  7 -3.55                                             6.88                   -3.33 (y19[+1])
                //  8         -1.33                                            1.33

                double[,] inverse = null;

                try
                {
                    double[,] temp = (double[,])mEndo.Clone();
                    bool fail;  inverse = Program.InvertMatrix(temp, false, false, out fail);
                }
                catch (Exception e)
                {
                    bool nan = false;
                    foreach (double d in mEndo)
                    {
                        if (G.IsNumericalError(d))
                        {
                            nan = true;
                            break;
                        }
                    }
                    if (!nan)
                    {
                        new Error("Matrix inversion for DECOMP failed for period " + t.ToString(), false);
                        throw;
                    }
                    else
                    {
                        //We allow this, may just be some missing data
                        inverse = G.CreateArrayDouble(mEndo.GetLength(0), mEndo.GetLength(1), double.NaN);
                    }
                }

                double[,] effect = Program.MultiplyMatrices(inverse, mExo);

                //the effect matrix is #endo x #exo

                int varnamesCounter = -1;

                string s = decompOptions2.link[parentI].varnames;

                if (true)
                {
                    //these are the ones being reported. Is a subset of endo.

                    varnamesCounter++;

                    if (t.EqualsGekkoTime(per1))
                    {
                        DecompData dd = new DecompData();
                        DecompInitDict(dd);
                        decompDatas.MAIN_data = dd;
                    }

                    string s3 = DecompFirst() + ":" + ConvertToTurtleName(s, 0);

                    Series ts = GetDecompDatas(decompDatas.MAIN_data, operatorOneOf3Types)[s3];
                    ts.SetData(t, 1d);

                    int i = endo[s3];  //row
                    for (int j = 0; j < effect.GetLength(1); j++)
                    {
                        //this != 0 originates from the Gekko non-scalar decomp, and only makes sense when excact precedents are not known
                        //see also #sf94lkjsdjæ
                        if (model.DecompType() == EModelType.GAMSScalar || effect[i, j] != 0d)
                        {
                            foreach (KeyValuePair<string, int> kvp in exo)
                            {
                                if (kvp.Value == j)
                                {
                                    Series ts2 = GetDecompDatas(decompDatas.MAIN_data, operatorOneOf3Types)[kvp.Key];
                                    ts2.SetData(t, effect[i, j]);
                                }
                            }
                        }
                    }
                }
            }   //foreach t

            DecompRemoveResidualsIfZero(per1, per2, decompDatas, operatorOneOf3Types);
        }

        /// <summary>
        /// Inversion of contributions, for GAMS scalar model
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="decompDatas"></param>
        /// <param name="operatorOneOf3Types"></param>
        /// <param name="parentI"></param>
        public static void DecompMainHelperInvertScalar(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, DecompDatas decompDatas, EContribType operatorOneOf3Types, int parentI, bool refreshObjects, DecompOperator op, ModelGamsScalar modelGamsScalar)
        {
            //See OVERVIEW in DecompGetFuncExpressionsAndRecalc()

            GekkoDictionaryBlanks<int> endo = new GekkoDictionaryBlanks<int>();
            GekkoDictionaryBlanks<int> exo = new GekkoDictionaryBlanks<int>();
            GekkoDictionaryBlanks<int> all = new GekkoDictionaryBlanks<int>();  //all variables that are present in 1 or more equations
            Dictionary<int, string> endoReverse = new Dictionary<int, string>();  //just inverted
            Dictionary<int, string> exoReverse = new Dictionary<int, string>();  //just inverted

            foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
            {
                foreach (string s in decompOptions2.link[0].endo)
                {
                    //Transforms from for instance Work:x¤[+1] into Work:x¤[2002].
                    string x = DecompFirst() + ":" + ConvertToTurtleName(s, 0, t);
                    if (!endo.ContainsKey(x))
                    {
                        int c = endo.Count();
                        endo.Add(x, c);
                        endoReverse.Add(c, x);
                    }
                }
            }            

            //residuals are not part of precedents here

            double[,] mEndo = null;
            double[,] mEndo2 = null;  //gradients
            double[,] mEndo3 = null;  //differences
            double[,] mExo = null;
            List<string> eqNames = new List<string>();
            List<string> eqNamesPretty = new List<string>(); //Only used for an error message

            //The loop here actually runs 2 times (over k). First time it just gathers elements for exo and exoReverse,
            //because the size of exo is used the second time.
            //Maybe a bit inefficient?

            int kMax = 2;
            if (op.isRaw) kMax = 1;

            for (int k = 0; k < kMax; k++)  //k=0 just counts endo/exo sizes, so the arrays can be defined
            {
                if (k == 0)
                {
                    //do nothing                    
                }
                else
                {
                    if (endo.Count() != eqNames.Count)
                    {
                        using (Error txt = new Error())
                        {
                            txt.MainAdd("The numbers of total equations (" + eqNames.Count + ") and the number of endogenous variables (" + endo.Count() + ") do not match");
                            List<string> temp2 = eqNames;
                            temp2.Sort(G.CompareNaturalIgnoreCase);
                            txt.MoreAdd("There are the following " + eqNames.Count + " equations given:");
                            txt.MoreNewLineTight();
                            txt.MoreAdd(Stringlist.GetListWithCommas(temp2));
                            txt.MoreNewLine();
                            List<string> temp1 = endo.GetKeys();
                            for (int i = 0; i < temp1.Count; i++) { temp1[i] = temp1[i].Replace("¤", ""); }
                            temp1.Sort(G.CompareNaturalIgnoreCase);
                            txt.MoreAdd("There are the following " + endo.Count() + " endo variables given:");
                            txt.MoreNewLineTight();
                            txt.MoreAdd(Stringlist.GetListWithCommas(temp1));
                        }
                    }

                    mEndo = new double[endo.Count(), endo.Count()];
                    mEndo2 = new double[endo.Count(), endo.Count()];
                    mEndo3 = new double[endo.Count(), endo.Count()];
                    mExo = new double[endo.Count(), exo.Count()];
                }
                int row = -1;
                int ii = -1;
                foreach (Link link in decompOptions2.link)  //including the "mother" non-linked equation
                {
                    ii++;
                    int jj = -1;
                    foreach (DecompStartHelper eqPeriods in link.GAMS_dsh)  //unrolling: for each uncontrolled #i in x[#i]
                    {
                        jj++;                        
                        DecompDict dd = null;
                        if (!op.isRaw) dd = GetDecompDatas(decompDatas.storage[ii][jj], operatorOneOf3Types);

                        foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                        {
                            row++;

                            //see also #as7f3læaf9
                            GekkoTime tTemp = t;
                            int add = 0;
                            tTemp = modelGamsScalar.Maybe2000GekkoTime(t);
                            add = t.Subtract(tTemp);

                            string eqName = AddTimeToIndexes(eqPeriods.name, new List<string>(eqPeriods.indexes.storage), tTemp, false);
                            if (k == 0)
                            {
                                eqNames.Add(eqName);
                                string eqNamePretty = AddTimeToIndexes(eqPeriods.name, new List<string>(eqPeriods.indexes.storage), tTemp, true);
                                eqNamesPretty.Add(eqNamePretty);
                            }
                            int eqNumber = modelGamsScalar.dict_FromEqNameToEqNumber.GetInt(eqName);

                            List<TwoStrings> variables = new List<TwoStrings>();

                            foreach (PeriodAndVariable dp in modelGamsScalar.precedents[eqNumber].vars)
                            {
                                //foreach precedent variable
                                string varName = modelGamsScalar.GetVarNameA(dp.variable);

                                int add2 = 0;

                                int date = dp.date;
                                int tt1 = date + add + add2;
                                int tt2 = date + add + add2 - t.Subtract(modelGamsScalar.tBasis);

                                if (modelGamsScalar.isTimeless[dp.variable])
                                {
                                    if (Globals.runningOnTTComputer && add != 0) G.WarningInternal("TTH: Expected add = 0 here");
                                    tt2 = 0;  //always show as if unlagged, even if it really points back to .tBasis.
                                }

                                //int extraLag = 0;
                                if (Program.options.bugfix_decomp_lagsleads)
                                {
                                    tt1 -= Globals.decomp_offset;
                                    tt2 -= Globals.decomp_offset;
                                }

                                if (eqPeriods.offset != 0)
                                {
                                    tt1 += eqPeriods.offset;
                                    tt2 += eqPeriods.offset;
                                }

                                string x1 = DecompFirst() + ":" + ConvertToTurtleName(varName, tt1, modelGamsScalar.tBasis);
                                string x2 = DecompFirst() + ":" + ConvertToTurtleName(varName, tt2);

                                TwoStrings two = new TwoStrings(x1, x2);
                                variables.Add(two);
                            }
                            string xx2 = Program.GetDecompResidualName(ii, decompOptions2.link.Count);
                            string xx1 = ConvertToTurtleName(xx2.Replace("¤[0]", ""), 0, t);
                            variables.Add(new TwoStrings(xx1, xx2));

                            //foreach precedent variable
                            foreach (TwoStrings two in variables)
                            {
                                string x1 = two.s1;
                                string x2 = two.s2;

                                if (k == 0)
                                {
                                    // -----------
                                    // First time
                                    // -----------

                                    all.AddIfNotAlreadyThere(x1, -12345);

                                    if (exo.ContainsKey(x1) || endo.ContainsKey(x1))
                                    {
                                        //endo or already in exo                                        
                                    }
                                    else
                                    {
                                        //exo
                                        int c = exo.Count();
                                        exo.Add(x1, c);
                                        exoReverse.Add(c, x1);
                                    }
                                }
                                else
                                {
                                    // ------------
                                    // Second time
                                    // ------------

                                    //k == 1
                                    if (endo.ContainsKey(x1))
                                    {
                                        int col = endo.GetInt(x1);
                                        if (!(row < mEndo.GetLength(0) && col < mEndo.GetLength(1)))
                                        {
                                            new Error("DECOMP matrix invert problem");
                                        }

                                        Series ts = dd.storage[x2];
                                        double d1 = double.NaN;
                                        if (ts != null) d1 = ts.GetDataSimple(t);
                                        mEndo[row, col] = d1;

                                        double d2 = InvertGetGradient(decompDatas.storage[ii][jj], x2, t, operatorOneOf3Types);
                                        mEndo2[row, col] = d2;

                                        double d3 = InvertGetDifference(decompDatas.storage[ii][jj], x2, t, operatorOneOf3Types);
                                        mEndo3[row, col] = d3;
                                    }
                                    else if (exo.ContainsKey(x1))
                                    {
                                        int col = exo.GetInt(x1);
                                        if (!(row < mExo.GetLength(0) && col < mExo.GetLength(1)))
                                        {
                                            new Error("DECOMP matrix invert problem");
                                        }
                                        Series ts = null;
                                        try
                                        {
                                            ts = dd.storage[x2];
                                        }
                                        catch
                                        {
                                            //exudl, forsøger her at finde en [-2], der er noget rotten
                                            //omkring 2030. Måske lave en liste over de tidsløse
                                        }
                                        double d = double.NaN;
                                        if (ts != null) d = ts.GetDataSimple(t);
                                        mExo[row, col] = d;
                                    }
                                    else
                                    {
                                        new Error("DECOMP matrix problem");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //TODO: check that number of endo and number of eqs match
            //TODO: check that number of endo and number of eqs match
            //TODO: check that number of endo and number of eqs match

            int n = endo.Count() + exo.Count();

            List<string> problem = new List<string>();
            foreach (string x in endo.GetKeys())
            {
                if (!all.ContainsKey(x)) problem.Add(x);
            }
            if (problem.Count > 0)
            {
                EndoVariableNotFoundInEquations(per1, per2, all, eqNamesPretty, problem);
            }

            if (refreshObjects)
            {
                DecompData dd2 = new DecompData();
                DecompInitDict(dd2);
                decompDatas.MAIN_data = dd2;
            }

            double[,] inverse = null;
            double[,] effect = null;

            if (!op.isRaw)
            {
                if (Program.options.bugfix_decomp_jacobi)
                {
                    bool fail = false;
                    try
                    {
                        double[,] temp = (double[,])mEndo2.Clone();  //gradients
                        inverse = Program.InvertMatrix(temp, false, false, out fail);
                    }
                    catch { fail = true; }
                    
                    if (fail)
                    {
                        bool nan = false;
                        foreach (double d in mEndo2)
                        {
                            if (G.IsNumericalError(d))
                            {
                                nan = true;
                                break;
                            }
                        }
                        if (!nan)
                        {
                            string extra = null;
                            if (CheckIfEverythingIsZero(mEndo2)) extra = " The " + mEndo2.GetLength(0) + " x " + mEndo2.GetLength(1) + " matrix contains only zeroes.";
                            if (decompOptions2.invertError == null) decompOptions2.invertError = "Matrix inversion failed for period " + per1.ToString() + "-" + per2.ToString() + "." + extra;  //we prefer to show the first error
                        }
                        //We allow this, may be some missing data (if nan == true)
                        inverse = G.CreateArrayDouble(mEndo2.GetLength(0), mEndo2.GetLength(1), double.NaN);
                    }
                    effect = Program.MultiplyMatrices(inverse, mExo);  //endo.Count x exo.Count, //the effect matrix is #endo x #exo   

                }
                else
                {
                    //NOT USED
                    //NOT USED
                    //NOT USED
                    if (CheckIfEverythingIsZero(mEndo) && CheckIfEverythingIsZero(mExo))
                    {
                        //nothing happens, so we can say that the effect is also zeroes...
                        effect = new double[endo.Count(), exo.Count()];
                    }
                    else
                    {
                        try
                        {
                            double[,] temp = (double[,])mEndo.Clone();
                            bool fail;
                            inverse = Program.InvertMatrix(temp, false, false, out fail);
                        }
                        catch (Exception e)
                        {
                            bool nan = false;
                            foreach (double d in mEndo)
                            {
                                if (G.IsNumericalError(d))
                                {
                                    nan = true;
                                    break;
                                }
                            }
                            if (!nan)
                            {
                                string extra = null;
                                if (CheckIfEverythingIsZero(mEndo)) extra = " Note that the " + mEndo.GetLength(0) + " x " + mEndo.GetLength(1) + " matrix to invert contains only zeroes, so it seems the endogenous variable(s) do not change at all, and hence the effects cannot be calculated.";
                                new Error("Matrix inversion for DECOMP failed for period " + per1.ToString() + "-" + per2.ToString() + "." + extra, false);
                                throw;
                            }
                            else
                            {
                                //We allow this, may just be some missing data
                                inverse = G.CreateArrayDouble(mEndo.GetLength(0), mEndo.GetLength(1), double.NaN);
                            }
                        }

                        effect = Program.MultiplyMatrices(inverse, mExo);  //endo.Count x exo.Count, //the effect matrix is #endo x #exo   
                    }
                }
            }

            for (int row = 0; row < endo.Count(); row++)
            {
                //
                // Not extremely pretty, but what else to do?
                // The period of the variable is not checked/matched at all (only the name), 
                // but that is perhaps not
                // necessary, since the period has already been filtered by the DECOMP time period.
                GekkoTime gtNotUsed; string name;
                ConvertFromTurtleName(endoReverse[row], true, out name, out gtNotUsed);
                if (!decompOptions2.new_select.Contains(name.Split(':')[1], StringComparer.OrdinalIgnoreCase)) continue;

                for (int col = 0; col < exo.Count(); col++)
                {
                    string endoName = endoReverse[row];
                    GekkoTime etime; string ename;
                    ConvertFromTurtleName(endoName, true, out ename, out etime);

                    string exoName = exoReverse[col];
                    GekkoTime xtime; string xname;
                    ConvertFromTurtleName(exoName, true, out xname, out xtime);

                    string enewName = ConvertToTurtleName(ename, 0);
                    int xlag = xtime.Subtract(etime);
                    GekkoTime time = etime;

                    int aNumber = modelGamsScalar.dict_FromVarNameToANumber.GetInt(G.Chop_RemoveBank(xname));
                    if (aNumber != -12345 && modelGamsScalar.isTimeless[aNumber])
                    {
                        xlag = 0;  //always show as if unlagged, even if it really points back to .tBasis.
                    }

                    string xnewName = ConvertToTurtleName(xname, xlag);

                    int ZERO = 0;
                    DecompDict dd = null;
                    if (op.isRaw)
                    {
                        //???? Why is this ever necessary: are such variables not already done beforehand???                        
                        DecompMainStoreRawVariable(decompDatas, xnewName, ZERO, modelGamsScalar, decompOptions2);                                            
                        if (col == 0) DecompMainStoreRawVariable(decompDatas, enewName, ZERO, modelGamsScalar, decompOptions2);                        
                    }
                    else
                    {
                        dd = GetDecompDatas(decompDatas.MAIN_data, operatorOneOf3Types);
                        Series ts2 = dd[xnewName];
                        ts2.SetData(time, effect[row, col]);
                        if (col == 0)  //just once
                        {
                            Series ts3 = dd[enewName];
                            if (Program.options.bugfix_decomp_jacobi)
                            {

                                EStorage type = EStorage.None;
                                if (operatorOneOf3Types == EContribType.D) type = EStorage.cellsChangeD;
                                else if (operatorOneOf3Types == EContribType.RD) type = EStorage.cellsChangeDRef;
                                else if (operatorOneOf3Types == EContribType.M) type = EStorage.cellsChangeM;
                                Series ts = GetRealTimeseries2(decompDatas, enewName, type);
                                double ddd2 = double.NaN;
                                if (ts != null) ddd2 = ts.GetDataSimple(time);
                                double ddd1 = mEndo3[row, row];  //IS THIS ALWAYS RIGHT??? Cannot be mEndo3[row, col] because mEndo2 is only over endo x endo.

                                if (Globals.runningOnTTComputer)
                                {
                                    bool bad = false;
                                    if (ddd1 != ddd2) bad = true;
                                    if (G.IsBothNumericalError(ddd1, ddd2)) bad = false;
                                    if (bad) MessageBox.Show("Decomp problem, check that!");
                                }

                                ts3.SetData(time, ddd2);
                            }
                            else
                            {
                                ts3.SetData(time, 1d);
                            }
                        }
                    }
                }
            }
        }

        private static void EndoVariableNotFoundInEquations(GekkoTime per1, GekkoTime per2, GekkoDictionaryBlanks<int> all, List<string> eqNames, List<string> problem)
        {
            for (int i = 0; i < problem.Count; i++)
            {                
                int idx = problem[i].LastIndexOf(':'); 
                if (idx != -1) problem[i] = problem[i].Substring(idx + 1);
                problem[i] = G.ReplaceTurtle(problem[i]).Replace(", ", ",");
            }

            List<string> all2 = new List<string>();
            foreach (string s2 in all.GetKeys())
            {
                string s5 = G.ReplaceTurtle(s2);
                //G.Chop... will not work because there may be two "["
                int idx = s5.LastIndexOf(':'); 
                if (idx != -1) s5 = s5.Substring(idx + 1);
                s5 = G.ReplaceTurtle(s5).Replace(", ", ",");
                if (!s2.Contains(Globals.decompResidualName)) all2.Add(s5);
            }

            string extra0 = "For the period " + per1.ToString() + "-" + per2.ToString();            
            string extra3 = null;
            if (eqNames.Count > 0)
            {
                extra3 = "Equation" + G.S(eqNames.Count) + ":" + G.NL;
                foreach (string s6 in eqNames.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default)).ToList())
                {
                    extra3 += "  " + s6 + G.NL;
                }
            }

            string extra5 = null;
            if (all2.Count > 0)
            {
                extra5 = "Variable" + G.S(all2.Count) + ":" + G.NL;
                foreach (string s6 in all2.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default)).ToList())
                {
                    extra5 += "  " + s6 + G.NL;
                }                
            }            
            string extra1 = "not appear in any equations. You may possibly need to lag/lead one or more equations with a suffix like for instance '[-1]' or '[+1]'." + G.NL + G.NL + extra3 + G.NL + extra5;
            string s = null;
            if (problem.Count == 1)
            {
                s = extra0 + ", the endogenous variable " + Stringlist.GetListWithCommas(problem.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default)).ToList()) + " does " + extra1;
            }
            else
            {
                s = extra0 + ", the endogenous variables: " + Stringlist.GetListWithCommas(problem.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default)).ToList()) + " do " + extra1;
            }

            WindowMessageBox w = new WindowMessageBox(EMessageBox.Normal);
            w.Height = 500;
            w.Width = 800;
            w.textBox1.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            w.textBox1.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            w.textBox1.TextWrapping = System.Windows.TextWrapping.Wrap;
            w.textBox1.Text = s;
            w.textBox1.FontFamily = new System.Windows.Media.FontFamily("Courier New");
            w.textBox1.FontSize = 11;
            w.ShowDialog();

            new Error("DECOMP aborted");
        }

        private static double InvertGetGradient(DecompData d, string x2, GekkoTime t, EContribType operatorOneOf3Types)
        {
            double x = double.NaN;
            if (operatorOneOf3Types == EContribType.D)
            {
                x = d.cellsGradQuo[x2].GetDataSimple(t.Add(-1));
            }
            else if (operatorOneOf3Types == EContribType.RD)
            {
                x = d.cellsGradRef[x2].GetDataSimple(t.Add(-1));
            }
            else if (operatorOneOf3Types == EContribType.M)
            {
                x = d.cellsGradRef[x2].GetDataSimple(t);
            }
            else throw new GekkoException("Hov");
            return x;
        }

        private static double InvertGetDifference(DecompData d, string x2, GekkoTime t, EContribType operatorOneOf3Types)
        {
            double x = double.NaN;
            if (operatorOneOf3Types == EContribType.D)
            {
                double vQuo = d.cellsQuo[x2].GetDataSimple(t);
                double vQuoLag = d.cellsQuo[x2].GetDataSimple(t.Add(-1));
                x = vQuo - vQuoLag;
            }
            else if (operatorOneOf3Types == EContribType.RD)
            {
                double vRef = d.cellsRef[x2].GetDataSimple(t);
                double vRefLag = d.cellsRef[x2].GetDataSimple(t.Add(-1));
                x = vRef - vRefLag;
            }
            else if (operatorOneOf3Types == EContribType.M)
            {
                double vQuo = d.cellsQuo[x2].GetDataSimple(t);
                double vRef = d.cellsRef[x2].GetDataSimple(t);
                x = vQuo - vRef;
            }
            else throw new GekkoException("Hov");
            return x;
        }

        private static void DecompMainStoreRawVariable(DecompDatas decompDatas, string name, int eq, ModelGamsScalar modelGamsScalar, DecompOptions2 decompOptions2)
        {
            // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
            // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
            // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
            // ==> Why are these variables not taken from the a or a_ref array at least? And they are probably also present in other parts of DecompDatas!!
            // ==> Fix this in Gekko 4.0, make it more clean. Here we have to check for null, etc.
            // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
            // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
            // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
            
            int lag2; string name2;
            ConvertFromTurtleName(name, true, out name2, out lag2);

            Tuple<Series, Series> tup = null;
            if (IsDecompResidualName(name))
            {
                tup = GetRealTimeseries(decompDatas, name);
                if (!decompDatas.MAIN_data.cellsQuo.ContainsKey(name)) decompDatas.MAIN_data.cellsQuo.Add(name, tup.Item1);
                if (!decompDatas.MAIN_data.cellsRef.ContainsKey(name)) decompDatas.MAIN_data.cellsRef.Add(name, tup.Item2);
            }
            else
            {
                if (!decompDatas.MAIN_data.cellsQuo.ContainsKey(name))
                {
                    // HACK HACK HACK HACK HACK
                    // Why taken from databank?
                    // HACK HACK HACK HACK HACK
                    Series ts = null;
                    ts = O.GetIVariableFromString(name2, O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                    if (ts != null)
                    {
                        if (ts.type == ESeriesType.ArraySuper)
                        {
                            new Error("Did not expect variable '" + name2 + "' to be an array-series");
                        }
                        ts = ts.DeepClone(0, null, null) as Series;
                        if (Globals.runningOnTTComputer && ts.type == ESeriesType.Timeless)
                        {
                            //This works ok, since lag2 is always == 0, so .anchorPeriod is not touched (and if it were, that would still be ok)
                        }
                        ts.Lag(lag2);
                        if (G.DecompShouldHandleMissings(decompOptions2.missingAsZero, false) == ESeriesMissing.Zero)
                        {
                            DecompMainStoreRawVariableHelper(ts);
                        }
                    }
                    else
                    {
                        if (Globals.decompFix2)
                        {
                            ts = new Series(modelGamsScalar.parent.modelCommon.GetFreq(), null);
                        }
                    }
                    decompDatas.MAIN_data.cellsQuo.Add(name, ts);
                }

                if (!decompDatas.MAIN_data.cellsRef.ContainsKey(name))
                {
                    // HACK HACK HACK HACK HACK
                    // Why taken from databank?
                    // HACK HACK HACK HACK HACK

                    Series ts = O.GetIVariableFromString(name2.Replace(DecompFirst() + ":", "Ref:"), O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                    if (ts != null)
                    {
                        if (ts.type == ESeriesType.ArraySuper)
                        {
                            new Error("Did not expect variable '" + name2 + "' to be an array-series");
                        }
                        ts = (ts.DeepClone(0, null, null) as Series);
                        if (Globals.runningOnTTComputer && ts.type == ESeriesType.Timeless)
                        {
                            //This works ok, since lag2 is always == 0, so .anchorPeriod is not touched (and if it were, that would still be ok)
                        }
                        ts.Lag(lag2);
                        if (G.DecompShouldHandleMissings(decompOptions2.missingAsZero, false) == ESeriesMissing.Zero)
                        {
                            DecompMainStoreRawVariableHelper(ts);
                        }
                    }
                    else
                    {
                        if (Globals.decompFix2)
                        {
                            ts = new Series(modelGamsScalar.parent.modelCommon.GetFreq(), null);
                        }
                    }
                    decompDatas.MAIN_data.cellsRef.Add(name, ts);
                }
            }
        }

        /// <summary>
        /// Handles M --> 0, ok to change the array values because the series has already been cloned.
        /// Will also work for timeless series.
        /// </summary>
        /// <param name="ts"></param>
        private static void DecompMainStoreRawVariableHelper(Series ts)
        {            
            double[] data = ts.GetDataSequenceUnsafePointerReadOnlyBEWARE();
            for (int i = 0; i < data.Length; i++)
            {
                if (G.IsNumericalError(data[i])) data[i] = 0d;
            }
        }

        /// <summary>
        /// Checks if both matrices contain only zeroes
        /// </summary>
        /// <param name="x"></param>
        /// <param name="mExo"></param>
        /// <returns></returns>
        private static bool CheckIfEverythingIsZero(double[,] x)
        {
            bool nul = true;
            foreach (double d in x)
            {
                if (d == 0d) continue;
                nul = false;
                break;
            }
            return nul;
        }

        public static void DecompMainMergeOrAdd(DecompDatas decompDatas, DecompData dd, int ii, int jj)
        {
            MergeDecompDict(dd.cellsContribD, decompDatas.storage[ii][jj].cellsContribD);
            MergeDecompDict(dd.cellsContribDRef, decompDatas.storage[ii][jj].cellsContribDRef);
            MergeDecompDict(dd.cellsContribM, decompDatas.storage[ii][jj].cellsContribM);

            MergeDecompDict(dd.cellsChangeD, decompDatas.storage[ii][jj].cellsChangeD);
            MergeDecompDict(dd.cellsChangeDRef, decompDatas.storage[ii][jj].cellsChangeDRef);
            MergeDecompDict(dd.cellsChangeM, decompDatas.storage[ii][jj].cellsChangeM);

            MergeDecompDict(dd.cellsGradQuo, decompDatas.storage[ii][jj].cellsGradQuo);
            MergeDecompDict(dd.cellsGradRef, decompDatas.storage[ii][jj].cellsGradRef);
            MergeDecompDict(dd.cellsQuo, decompDatas.storage[ii][jj].cellsQuo);
            MergeDecompDict(dd.cellsRef, decompDatas.storage[ii][jj].cellsRef);
        }

        public static bool IsDecompResidualName(string name)
        {
            if (name == null) return false;
            return name.Contains(Globals.decompResidualName);
        }

        /// <summary>
        /// Helper method.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <param name="simple"></param>
        /// <returns></returns>
        public static string EquationLhsRhs(string lhs, string rhs, bool simple)
        {
            //This method is just so that we keep the two ways of decomposing together,
            //that is, calling an equation like DECOMP eq1, or DECOMP y = x1 + x2.
            //The former has simple = true, the latter simple = false.
            //if (simple) return "-(" + lhs + ") + " + rhs;
            //else return "O.Add(" + Globals.smpl + ", O.Negate(" + Globals.smpl + ", " + lhs + "), " + rhs + ")";
            if (simple)
            {
                return lhs + " - (" + rhs + ")";
            }
            else
            {
                return "O.Add(" + Globals.smpl + ", " + lhs + ", O.Negate(" + Globals.smpl + ", (" + rhs + ")))";
            }
        }

        private static void MergeDecompDict(DecompDict d, DecompDict dStorage)
        {
            //Gekko 4.0: Do some proper logic regarding "windows" of data getting updated
            //The following is hacky

            //foreach (KeyValuePair<string, Series> kvp in d.storage)
            //{
            //    Series ts = kvp.Value;
            //    Series tsStorage = dStorage[kvp.Key]; //may be created

            //    GekkoTime t1 = ts.GetRealDataPeriodFirst();
            //    GekkoTime t2 = ts.GetRealDataPeriodLast();
            //    if (!t1.IsNull())
            //    {
            //        foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
            //        {
            //            //the following if is probably not necessary
            //            //The IF is dropped ... if (G.IsNumericalError(tsStorage.GetDataSimple(t))) tsStorage.SetData(t, ts.GetDataSimple(t));
            //            tsStorage.SetData(t, ts.GetDataSimple(t));
            //        }
            //    }
            //}

            foreach (KeyValuePair<string, Series> kvp in d.storage)
            {
                Series ts = kvp.Value;
                Series tsClone = ts.DeepClone(0, null, null) as Series;
                if (dStorage.ContainsKey(kvp.Key)) dStorage.Remove(kvp.Key);
                dStorage.Add(kvp.Key, tsClone);                
            }
        }

        private static void PrintDecompDict(DecompDict d)
        {
            foreach (KeyValuePair<string, Series> kvp in d.storage)
            {
                Series ts = kvp.Value;
                GekkoTime t1 = ts.GetRealDataPeriodFirst();
                GekkoTime t2 = ts.GetRealDataPeriodLast();
                if (!t1.IsNull())
                {
                    int missings = 0;
                    foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
                    {
                        //the following if is probably not necessary
                        if (G.IsNumericalError(ts.GetDataSimple(t))) missings++;
                    }
                    string m = null;
                    if (missings > 0) m = ", !!!!! missings = " + missings;
                    new Writeln(kvp.Key + " ---> data for " + t1.ToString() + "-" + t2.ToString() + m);
                }
                else
                {
                    new Writeln(kvp.Key + " ---> all missings");
                }
            }
        }

        private static void DecompRemoveResidualsIfZero(GekkoTime per1, GekkoTime per2, DecompDatas decompDatas, EContribType operatorOneOf3Types)
        {
            List<string> remove = new List<string>();
            DecompDict dd = GetDecompDatas(decompDatas.MAIN_data, operatorOneOf3Types);
            foreach (KeyValuePair<string, Series> kvp in dd.storage)
            {
                string s = kvp.Key;
                string[] ss = s.Split('¤');
                string s2 = G.Chop_RemoveBank(ss[0], DecompFirst());
                if (IsDecompResidualName(s2))
                {
                    //TODO TODO TODO
                    //TODO TODO TODO
                    //TODO TODO TODO
                    //TODO TODO TODO
                    //TODO TODO TODO
                    //TODO TODO TODO threshold should be decimals used in GUI!!
                    //TODO TODO TODO
                    //TODO TODO TODO
                    //TODO TODO TODO
                    //TODO TODO TODO
                    //TODO TODO TODO
                    //TODO TODO TODO
                    if (IsAlmostZeroTimeseries(per1, per2, kvp.Value, 1e-5d))
                    {
                        remove.Add(kvp.Key);
                    }
                }
            }
            foreach (string s in remove)
            {
                bool b = dd.Remove(s);
            }
        }

        private static void DecompCheckNumberOfEqsAndEndo(DecompDatas decompDatas, GekkoDictionary<string, int> endo)
        {
            int nEqs = 0;
            for (int i = 0; i < decompDatas.storage.Count; i++) //for each linked eq, including the first one
            {
                for (int j = 0; j < decompDatas.storage[i].Count; j++) //for each uncontrolled set in eq
                {
                    nEqs++;
                }
            }

            if (nEqs != endo.Count)
            {
                using (Error e = new Error())
                {
                    e.MainAdd("The number of equations and endogenous variables do not match (" + nEqs + " vs " + endo.Count + "). ");
                    e.MainAdd("Equations (unrolled over sets):");
                    for (int i = 0; i < decompDatas.storage.Count; i++) //for each linked eq, including the first one
                    {
                        e.MainNewLineTight();
                        e.MainAdd("Equation #" + (i + 1) + " has " + decompDatas.storage[i].Count + " unrolled equations");
                    }
                }
            }
        }

        // ----------------------------
        // Turtle name start
        // ----------------------------

        public static string ConvertToTurtleName(string s, int lag, GekkoTime t)
        {
            return G.HandleBlanksRemove(s + "¤[" + t.Add(lag).ToString() + "]");
        }

        public static string ConvertToTurtleName(string s, int lag)
        {
            string slag = lag.ToString();
            if (lag > 0) slag = "+" + slag;
            slag = "[" + slag + "]";
            return G.HandleBlanksRemove(s + "¤" + slag);
        }

        /// <summary>
        /// Splits something like x[a,b]¤[1999q3] up into 1999q3 and x[a,b]. If no ¤, an error is issued.
        /// Splits at the '¤' no matter what is before. If strict==true and > one '¤', it will fail.
        /// See overload for lags like x[-1].
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="gt"></param>
        /// <param name="name"></param>
        public static void ConvertFromTurtleName(string varname, bool strict, out string name, out GekkoTime gt)
        {
            gt = GekkoTime.tNull;
            name = null;
            if (varname == null)
            {
                //do nothing
            }
            else
            {
                string[] ss = varname.Split('¤');
                if (strict && ss.Length != 2) new Error("Turtle error");
                if (ss.Length == 1)
                {
                    new Error("Turtle error");
                }
                else if (ss.Length == 2)
                {
                    //do something for Q and M...
                    gt = GekkoTime.FromStringToGekkoTime(ss[1].Substring(1, ss[1].Length - 2));
                    name = ss[0];
                }
                else new Error("Turtle error");
            }
        }

        /// <summary>
        /// Splits something like x[a,b]¤[-1] up into -1 and x[a,b]. If no ¤, the lag is 0.
        /// Splits at the '¤' no matter what is before. If strict==true and > one '¤', it will fail.
        /// See overload for periods like x[1999q3].
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="lag"></param>
        /// <param name="name"></param>
        public static void ConvertFromTurtleName(string varname, bool strict, out string name, out int lag)
        {
            lag = -12345;
            name = null;
            if (varname == null)
            {
                //do nothing
            }
            else
            {
                string[] ss = varname.Split('¤');
                if (strict && ss.Length != 2) new Error("Turtle error");
                if (ss.Length == 1)
                {
                    lag = 0;
                    name = varname;
                }
                else if (ss.Length == 2)
                {
                    lag = int.Parse(ss[1].Substring(1, ss[1].Length - 2));
                    name = ss[0];
                }
                else new Error("Turtle error");
            }
        }


        // ----------------------------
        // Turtle name end
        // ----------------------------

        public static bool IsOperatorOneOf3Types(EContribType operatorOneOf3Types)
        {
            if (operatorOneOf3Types == EContribType.D) return true;
            else if (operatorOneOf3Types == EContribType.RD) return true;
            else if (operatorOneOf3Types == EContribType.M) return true;
            return false;
        }

        public static DecompDict GetDecompDatas(DecompData decompData, EContribType operatorOneOf3Types)
        {
            if (operatorOneOf3Types == EContribType.D) return decompData.cellsContribD;
            else if (operatorOneOf3Types == EContribType.RD) return decompData.cellsContribDRef;
            else if (operatorOneOf3Types == EContribType.M) return decompData.cellsContribM;
            else
                new Error("Wrong type. Note: for decomposition you need to use operator <d>, <p>, <dp>, <m>, <q> or <mp>, because decomposition cannot be done for other types. In the DECOMP window, use operators from the 'Decomp' columns, not the 'Raw' columns.");
            return null;
        }

        private static bool IsAlmostZeroTimeseries(GekkoTime per1, GekkoTime per2, Series xx, double eps)
        {
            bool isZero = true;
            foreach (GekkoTime t in new GekkoTimeIterator(per1.Add(Globals.decompPerLag), per2))
            {
                double d = xx.GetDataSimple(t);
                if (!G.IsNumericalError(d) && Math.Abs(d) > eps)
                {
                    isZero = false;
                    break;
                }
            }

            return isZero;
        }

        /// <summary>
        /// Kind of an entry point for decomposition, also called when buttons are clicked etc.
        /// For a GAMS model, this calls DecompEvalGams(), and for a Gekko model, this calls 
        /// DecompEvalGekko(). Opens up a new window, unless windowDecomp != null. If windowDecomp is used,
        /// make sure that decompFind corresponds to the windowDecomp window (perhaps taken as windowDecomp.decompFind).
        /// </summary>
        /// <param name="o"></param>
        public static void DecompGetFuncExpressionsAndRecalc(DecompFind decompFind, WindowDecomp windowDecomp)
        {            
            //OVERVIEW, #overview
            // +++ Fixed that it looks for variable explanations (labels)
            // +++ Make sure ok regarding domains
            // 
            //
            //DecompGetFuncExpressionsAndRecalc()
            //  thread: CreateDecompWindow()
            //    RecalcCellsWithNewType()
            //      RecalcCellsWithNewTypeHelper()
            //        DecompMain()
            //          DecompMainInit()
            //          PrepareEquations()
            //          foreach (Link link in decompOptions2.link) //for each equation if they are linked
            //            foreach (DecompStartHelper dsh in link.GAMS_dsh) //for each uncontrolled #i in x[#i] --> is that used??
            //              DecompLowLevelScalar()
            //                foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
            //                  foreach (PeriodAndVariable dp in modelGamsScalar.precedents[eqNumber].vars) //for each precedent variable
            //                    //decomposition gradients
            //                foreach (GekkoTime t2 in new GekkoTimeIterator(gt1, gt2))
            //                  foreach (string s in vars.Keys)
            //                    //contributinons, and residuals
            //              DecompMainMergeOrAdd()
            //          foreach (GekkoTime gt in new GekkoTimeIterator(per1.Add(deduct), per2)) //no time loop with <dyn>
            //            DecompMainHelperInvertScalar()
            //              //figure out endo and exo etc.
            //              //IN LOOPS, MATRIX VALUES ARE GATHERED, POSSIBLY "STACKED" OVER TIME
            //              //INVERT MATRIX
            //              //CALCULATE EFFECTS AFTER INVERTING
            //          DecompPivotToTable()
            //
            // Regarding data, in MaybeLoadDataIntoModel(), the databanks are represented by double[][] arrays. So each
            // model variable (from the GAMS dict) has a number. If a variable from the model does not exist in the model array,
            // (for instance, if qM[tot] is present in the model and either qM or qM[tot] does not exist), the variable
            // "slot" in the model will have missing values.
            // BUT:     
            // + For <xn>, DecompMainStoreRawVariable() gets series from db, called from DecompMainHelperInvertScalar(), but only for .isRaw.
            // + Then afterwards, series from db are gotten from DecompPivotGetDomains() line 5038, both .isRaw and not
            // + Then afterwards, series from db are gotten from DecompPivotCreateDataframe() line 4575, but only for .isRaw
            //
            // Regarding missing values, in the GUI this can be clicked:
            // this.decompFind.decompOptions2.missingAsZero = true;
            //
            //Pivot is probably ok, so where it can go wrong is
            // (1) missing series or sub-series
            // (2) matrix inversion
            //In (1) show as "N" for raw, NaN for decomp
            //In (2) set matrix values NaN
            //Make missing=zero work good
            //Error about missing equation --> truncate time window until ok (perhaps with "N" for such columns in raw, <dyn> is spcielal here)
            //



            DecompOptions2 decompOptions2 = decompFind.decompOptions2;
            if (decompFind.model.DecompType() == EModelType.Unknown)
            {
                new Error("It seems no model is loaded, cf. the MODEL command.");
            }

            int count = -1;
            foreach (Link link in decompOptions2.link)
            {
                count++;

                EModelType type = decompFind.model.DecompType();

                if (type == EModelType.GAMSScalar)
                {
                    //do nothing
                }
                else if (type == EModelType.GAMSRaw)
                {
                    //GAMS model
                    //GAMS model
                    //GAMS model
                    //GAMS model
                    //GAMS model
                    if (link.expressions.Count != 1) new Error("Expected 1 link expression");
                    if (link.expressions[0] == null)
                    {
                        //
                        // NEW GAMS MODEL DECOMP
                        //
                        ModelGamsEquation found = GamsModel.DecompEvalGams(link.eqname, link.varnames, decompFind.model);  //if link.eqname != null, link.varnames[0] is not used at all
                        link.expressions = found.expressions;
                        link.expressionText = found.lhs + " = " + found.rhs;
                    }
                }
                else if (type == EModelType.Gekko)
                {
                    //Gekko model
                    //Gekko model
                    //Gekko model
                    //Gekko model
                    //Gekko model                    

                    if (link.expressions.Count != 1) new Error("Expected 1 link expression");
                    if (link.expressions[0] == null)
                    {
                        // NEW GEKKO MODEL DECOMP
                        // NEW GEKKO MODEL DECOMP
                        // NEW GEKKO MODEL DECOMP
                        EquationHelper found = DecompEvalGekko(link.varnames);
                        link.expressions = found.expressions;
                        decompOptions2.expressionOld = found.equationText;
                    }
                    else
                    {
                        new Error("Expected 1 link expression");
                    }
                }
                else new Error("Model type error");
            }            

            if (windowDecomp == null)
            {
                //new DECOMP window

                DecompFind df2 = decompFind.SearchUpwards(EDecompFindNavigation.Decomp);
                WindowDecomp parent = null;
                if (df2?.window != null) parent = df2.window as WindowDecomp;
                if (parent != null)
                {
                    CrossThreadStuff.GetDecompSizes(parent);
                }

                if (Globals.batchType!=EBatchType.PyGekko && (G.IsUnitTestingOrNotShowingGUI() && Globals.showDecompTable == true))
                {                    
                    //Skip the "Decomp" thread stuff when unit testing -- will give TreadAbortedException for some reason not understood.
                    CreateDecompWindow(decompFind);
                }
                else
                {                    
                    Thread thread = new Thread(new ParameterizedThreadStart(CreateDecompWindow));
                    thread.Name = "Decomp";
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                    thread.IsBackground = true;
                    thread.Start(decompFind);
                    //if (Globals.python) System.Threading.Thread.Sleep(int.MaxValue);  //HACK
                }

                //Also see #9237532567
                //This stuff makes sure we wait for the window to open, before we move on with the code.
                for (int i = 0; i < 6000; i++)  //up to 60 s, then we move on anyway
                {
                    System.Threading.Thread.Sleep(10);  //0.01s
                                                        //not sure why decompFind.decompOptions2 can become == null in the other thread...?
                    if ((decompFind.decompOptions2 != null && decompFind.decompOptions2.numberOfRecalcs > 0) || decompFind.hasException)
                    {
                        break;
                    }
                }

                DecompFind df = decompFind.SearchUpwards(EDecompFindNavigation.Find);
                if (df != null)
                {
                    WindowFind w = df.window as WindowFind;
                    w.Close();
                }
            }
            else
            {
                windowDecomp.RecalcCellsWithNewType(decompFind.model);
            }
        }

        /// <summary>
        /// Shows the DECOMP window. Uses an object argument because it can be called from a new thread.
        /// It really uses DecompFind as object.
        /// </summary>
        /// <param name="o2"></param>
        private static void CreateDecompWindow(object o2)
        {
            //See OVERVIEW in DecompGetFuncExpressionsAndRecalc()

            DecompFind decompFind = o2 as DecompFind;

            if (decompFind.decompOptions2.guiIsFlowStatement && decompFind.depth == 0)
            {
                //Flowgraph, and only if called from statement "FLOW ...;", which will have depth == 0.
                decompFind.decompOptions2.guiFlowName = decompFind.decompOptions2.new_select[0];
                decompFind.decompOptions2.guiIsFlowUseEquationName = true; //hack
                WindowFlow.CallFlowGraph(decompFind);
            }
            else
            {
                //Normal decomp window
                WindowDecomp windowDecomp = null;

                try
                {
                    windowDecomp = new WindowDecomp(decompFind);
                    windowDecomp.decompFind.SetWindow(windowDecomp);
                    Globals.windowsDecomp2.Add(windowDecomp);
                    windowDecomp.isInitializing = true;  //so we don't get a recalc here because of setting radio buttons
                    windowDecomp.SetRadioButtons();
                    windowDecomp.isInitializing = false;

                    windowDecomp.RecalcCellsWithNewType(decompFind.model);  //With fail, we get                 

                    decompFind.decompOptions2.numberOfRecalcs++;  //signal for Decomp() method to move on            
                    if (Globals.batchType != EBatchType.PyGekko && G.IsUnitTestingOrNotShowingGUI() && Globals.showDecompTable == false)
                    {
                        Globals.windowsDecomp2.Clear();
                        windowDecomp = null;
                    }
                    else
                    {
                        if (windowDecomp.isClosing)  //if something goes wrong, .isClosing will be true
                        {
                            //The line below removes the window from the global list of active windows.
                            //Without this line, this half-dead window will mess up automatic closing of windows (Window -> Close -> Close all...)
                            if (Globals.windowsDecomp2.Count > 0) Globals.windowsDecomp2.RemoveAt(Globals.windowsDecomp2.Count - 1);
                        }
                        else
                        {
                            windowDecomp.ShowDialog();
                            if (Globals.showDecompTable)
                            {
                                Globals.showDecompTable = false;
                                new Error("Debug, tables aborted. Set Globals.showDecompTable = false.");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    //we ignore the exception here, so that Gekko and other windows are not crashing.
                    if (Globals.runningOnTTComputer)
                    {
                        MessageBox.Show(e.Message + " --decomptrace-> " + e.StackTrace);
                    }
                    decompFind.hasException = true;
                }
            }
        }

        public static void Decomp2ThreadFunction(Object o)
        {
            CrossThreadStuff.Decomp2(o);
        }

        /// <summary>
        /// Called by DecompMain() and performs the low-level math, obtaining and preparing data and
        /// calculating gradients. Used for "normal" Gekko models, possibly with raw equations.
        /// </summary>
        /// <param name="tt1"></param>
        /// <param name="tt2"></param>
        /// <param name="expression"></param>
        /// <param name="workOrRefOrBoth"></param>
        /// <param name="residualName"></param>
        /// <param name="funcCounter"></param>
        /// <returns></returns>
        public static DecompData DecompLowLevel(GekkoTime tt1, GekkoTime tt2, Func<GekkoSmpl, IVariable> expression, EDecompBanks workOrRefOrBoth, string residualName, ref int funcCounter)
        {
            //See #kljaf89usafasdf for scalar model
            //
            //
            //
            //                  Ref     -- m -->     Work
            //
            //                   ^                    ^
            //                   |                    |
            //                  rd                    d
            //                   |                    |
            //
            //                 Ref[-1]             Work[-1]
            //
            //
            //DECOMP2 <2010 2012 q> sum((#a, #s), pop[#a, #s, #o]) 
            //  SELECT #a, #s
            //  WHERE  'se' in #o, 'se' in #o  // ... , date = 2011    
            //  AGG   #a as #a_agg level '10-year' zoom '27', #a as #a_agg level '10-year' zoom '27'  
            //  SORT #s, #a
            //  LINK   x1 from e2, x3 from e1    
            //  COLS  #a, #o;

            // DECOMP a where a in b agg x as y level 1 zoom 2 link a from b;
            // y   $   'a' in #a and 'b' in #j --> y[a, b]
            // comma could be used in $
            // level 3 --> level '5-year'
            // It should be possible to select agg level. Later on, more sophisticated opening of sub-nodes:
            //    level '10-year' zoom '25..29'   or   level '10-year' zoom '27'
            // when zooming, sibling nodes for zoomed level are shown, same for parents up to the current aggregation level.
            // Like this, we can both have an aggregation level and a deeper zoom.
            //
            // Per default, we have time in cols. It should be possible to put other dimensions on the cols (for fixed time).
            // even having time on rows should be possible
            //                        
            // #a_agg = nested list with this structure:            
            //
            //  '5-year'       --> level2
            //    '20..24'
            //      '20'
            //      '21'
            //      '22'
            //      '23'
            //      '24'
            //    '25..29'
            //      '25'
            //      '26'
            //      '27'
            //      '28'
            //      '29'
            // '10-year'        --> level 3
            //    '20..29'
            //      '20..24'
            //      '25..29'
            //    '30..39'
            //      '30..34'
            //      '35..39'
            // 'total'              --> level 4
            //   'tot'
            //     '20..29'
            //     '30..39'
            //
            //
            // 
            //
            // we could later on use groupbyavg, groupby is implicitly groupbysum here
            //
            //      50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 66 67 68 69 70 71
            //       ------------   ------------  -------------   ------------   ----...
            //       ===========================  ============================   ====...
            //       +++++++++++++++++++++++++++++++++++++++++++++++++++++++++   ++++...
            //      
            //       -------------------------------------------------  -------------... arb.marked
            //
            // The real output from this are these dicts:            
            //   cellsContribD = new DecompDict();
            //   cellsContribDRef = new DecompDict();
            //   cellsContribM = new DecompDict();
            //
            // They may not all be created: depends upon workOrRefOrBoth parameter
            // If the equation is y = sum(#i, x[#i]) + x[c][+1] + z[-1], the keys will be:
            //   Work:y
            //   Work:x[a]
            //   Work:x[b]
            //   Work:x[c]¤[+1]            
            //   Work:z¤[-1]
            //
            // The contributions sum to zero. These may link up to other DecompTables. If we have this:
            //
            // x[a][-1] = z[-2] + x[c]
            //
            // we may put in x[a][-1] into x[a] in the former equation, but then we need to lead these contributions:
            //
            // x[a] = z[-2][-1] + x[c][+1]
            //
            // We can just use the same table where we add 1 to the ¤[...] lags, and change the offset in all the series in the DecompTables.
            // For instance:
            //
            //   Work:y             -18
            //   Work:x[a]           20
            //   Work:x[b]            5
            //   Work:x[c]¤[+1]     -12    
            //   Work:z¤[-1]          5
            // ---------------------------  NOTE: the table below has been leaded
            //   Work:x[a]          -10     --> we join on x[a]
            //   Work:z¤[-1]          3          
            //   Work:x[c]¤[+1]       7
            // ===========================
            //   Work:y             -10     --> divide with 10 to get the final effects on y
            //     Work:z¤[-1]        6     --> the x[a] = 20 is removed by multiplying the second table with 2 and adding the two tables     
            //     Work:x[c]¤[+1]    14
            //   Work:x[b]            5
            //   Work:x[c]¤[+1]     -12    
            //   Work:z¤[-1]          5

            List<int> mm = new List<int>();
            if (workOrRefOrBoth == EDecompBanks.Work) mm.Add(0);
            else if (workOrRefOrBoth == EDecompBanks.Ref) mm.Add(1);
            else if (workOrRefOrBoth == EDecompBanks.Multiplier)
            {
                mm.Add(0);
                mm.Add(1);
            }

            DecompData d = new DecompData();

            DateTime dt = DateTime.Now;

            GekkoSmpl smpl = new GekkoSmpl(tt1, tt2);
            IVariable y0a = null;
            IVariable y0aRef = null;

            try
            {  //resets Globals.precedents afterwards
                DecompInitDict(d);

                Globals.precedentsContainer = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                //Function call start --------------
                O.AdjustSmplForDecomp(smpl, 0);
                //TODO: can be deleted, #p24234oi32
                string s5 = Globals.expressionText;
                y0a = expression(smpl); funcCounter++;  //this call fills Globals.precedents with variables
                O.AdjustSmplForDecomp(smpl, 1);
                //Function call end   --------------

                List<DecompPrecedent> decompPrecedents = new List<DecompPrecedent>();

                List<string> ss = Globals.precedentsContainer.Keys.ToList<string>();
                ss.Sort(StringComparer.OrdinalIgnoreCase);
                foreach (string s in ss)
                {
                    IVariable x = O.GetIVariableFromString(s, O.ECreatePossibilities.NoneReportError);

                    if (x.Type() == EVariableType.Series)
                    {
                        Series ivTemp_series = x as Series;
                        if (ivTemp_series.type == ESeriesType.ArraySuper) continue;  //skipped: we are only looking at sub-series
                        decompPrecedents.Add(new DecompPrecedent(s, x));
                    }
                    else if (x.Type() == EVariableType.Val)
                    {
                        decompPrecedents.Add(new DecompPrecedent(s, x));
                    }
                }


                //IMPORTANT
                //IMPORTANT
                //IMPORTANT
                Globals.precedentsContainer = null;  //!!! This is important: if not set to null, afterwards there will be a lot of superfluous lookup in the dictionary
                //IMPORTANT
                //IMPORTANT
                //IMPORTANT

                Series y0a_series = y0a as Series;
                if (y0a == null)
                {
                    new Error("DECOMP expects the expression to be of series type");
                }
                Series y0_series = y0a_series;
                if (y0a_series.type != ESeriesType.Light)
                {
                    y0_series = y0a.DeepClone(0, null, null) as Series;  //a lag like "DECOMP x[-1]" may just move a pointer to real timeseries x, and x is changed with shocks... //No need for CloneHelper dict.
                }

                d.cellsQuo.storage.Add(residualName, y0_series);

                Series y0aRef_series = null;
                Series y0Ref_series = null;
                if (mm.Contains(1))
                {
                    //Function call start --------------
                    O.AdjustSmplForDecomp(smpl, 0);
                    smpl.bankNumber = 1;
                    y0aRef = expression(smpl); funcCounter++;
                    smpl.bankNumber = 0;
                    O.AdjustSmplForDecomp(smpl, 1);
                    //Function call end   --------------

                    y0aRef_series = y0aRef as Series;
                    if (y0aRef == null)
                    {
                        new Error("DECOMP expects the expression to be of series type");
                    }
                    y0Ref_series = y0aRef_series;
                    if (y0aRef_series.type != ESeriesType.Light)
                    {
                        y0Ref_series = y0aRef.DeepClone(0, null, null) as Series;  //a lag like "DECOMP x[-1]" may just move a pointer to real timeseries x, and x is changed with shocks... //No need for CloneHelper dict.
                    }
                    d.cellsRef.storage.Add(residualName, y0Ref_series);
                }

                double eps = Globals.newtonSmallNumber;

                if (decompPrecedents.Count > 0)
                {
                    GekkoDictionary<string, int> vars = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                    int iVar = -1;

                    foreach (DecompPrecedent dp in decompPrecedents)
                    {
                        iVar++;

                        Series xRef_series = null;
                        IVariable dpx = O.GetIVariableFromString(dp.s, O.ECreatePossibilities.NoneReportError);

                        if (dpx.Type() == EVariableType.Series)
                        {
                            if ((dpx as Series).type == ESeriesType.Timeless) continue;  //skip timeless series, #2983473298472
                                                                                         //could also use smpl.bankNumber = 1 to do this, but then GetIVariableFromString should use smpl.bankNumbe
                            if (mm.Contains(1))
                            {
                                xRef_series = O.GetIVariableFromString(G.Chop_SetBank(dp.s, "Ref"), O.ECreatePossibilities.NoneReportError) as Series;
                            }
                        }
                        else
                        {
                            //else what?
                        }

                        foreach (GekkoTime t1 in new GekkoTimeIterator(tt1.Add(-O.MaxLag()), tt2.Add(O.MaxLead())))
                        {

                            // --------------------------------------------
                            // This is where the decomposition takes place
                            // --------------------------------------------

                            foreach (int j in mm)
                            {
                                if (dpx.Type() == EVariableType.Series)
                                {
                                    Series x_series = null;
                                    Series y_series = null;
                                    if (j == 0)
                                    {
                                        x_series = dpx as Series;
                                        y_series = y0_series;
                                    }
                                    else
                                    {
                                        x_series = xRef_series;
                                        y_series = y0Ref_series;
                                    }
                                    double x_before = x_series.GetDataSimple(t1);

                                    try
                                    {
                                        double x_after = x_before + eps;
                                        x_series.SetData(t1, x_after);

                                        //Function call start --------------
                                        O.AdjustSmplForDecomp(smpl, 0);  //no reason to enlarge this smpl with 10 pers at both ends, since it is only t2 that is written afterwards
                                        if (j == 1) smpl.bankNumber = 1;
                                        IVariable y1 = null;

                                        if (true)  //this is what takes most of the time in DECOMP
                                        {
                                            y1 = expression(smpl); funcCounter++;  // <============================ THIS TAKES TIME!
                                        }

                                        if (j == 1) smpl.bankNumber = 0;
                                        O.AdjustSmplForDecomp(smpl, 1);
                                        //Function call end   --------------

                                        Series y1_series = y1 as Series;
                                        string nameOriginal = G.Chop_RemoveFreq(dp.s, tt1.freq);

                                        if (true)  //this does not seem to cost any time...?
                                        {
                                            foreach (GekkoTime t2 in new GekkoTimeIterator(tt1.Add(Globals.decompPerLag), tt2.Add(0)))
                                            {
                                                double y0_double = y_series.GetDataSimple(t2);
                                                double y1_double = y1_series.GetDataSimple(t2);
                                                double grad = (y1_double - y0_double) / eps;

                                                if (!G.IsNumericalError(grad) && grad != 0d)
                                                {
                                                    //For the gradient to be a real number <> 0, the expression must evaluate
                                                    //before shock (y0) in the year considered (t2)
                                                    //If it does evaluate, but there is no effect, it is skipped too.

                                                    int lag = -t2.Subtract(t1);  //x[-1] --> lag = -1                                                                                        
                                                    string lag2 = null;
                                                    if (lag >= 1)
                                                    {
                                                        lag2 = "+" + lag.ToString();
                                                    }
                                                    else
                                                    {
                                                        lag2 = lag.ToString();
                                                    }
                                                    string name = nameOriginal + "¤[" + lag2 + "]";

                                                    if (lag == 0 || (lag < 0 && -lag <= Program.options.decomp_maxlag) || (lag > 0 && lag <= Program.options.decomp_maxlead))
                                                    {

                                                        if (j == 0)
                                                        {
                                                            d.cellsQuo[name].SetData(t2, x_before);
                                                        }
                                                        else
                                                        {
                                                            d.cellsRef[name].SetData(t2, x_before);  // for j != 0, x_before is from Ref bank.
                                                        }

                                                        if (j == 0)
                                                        {
                                                            d.cellsGradQuo[name].SetData(t2, grad);
                                                        }
                                                        else
                                                        {
                                                            d.cellsGradRef[name].SetData(t2, grad);
                                                        }
                                                    }

                                                    if (!vars.ContainsKey(name))
                                                    {
                                                        //list of relevant variables to handle later on
                                                        //in decomp pivot
                                                        vars.Add(name, 0);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        x_series.SetData(t1, x_before);
                                    }
                                }
                                else if (dpx.Type() == EVariableType.Val)
                                {
                                    //TODO
                                }
                                else
                                {
                                    //skip other types, this includes matrices
                                    //so an expression with a matrix that changes from Work to Ref is
                                    //not decomoposed as regards to this matrix
                                    //(we would have to shock each cell in the matrix...)
                                }
                            }
                        }
                    }

                    //Here, cellsQuo + cellsRef + cellsGradQuo + cellsGradRef are calculated.
                    //Grad tells us which lags are actually active.
                    //If we know that lags beforehand, we could limit the lag loop and save time here.

                    int i = 0;
                    foreach (GekkoTime t2 in new GekkoTimeIterator(tt1, tt2))
                    {
                        i++;
                        int j = 0;
                        foreach (string s in vars.Keys)
                        {
                            j++;

                            double vQuo = d.cellsQuo[s].GetDataSimple(t2);
                            double vQuoLag = d.cellsQuo[s].GetDataSimple(t2.Add(-1));
                            double vGradQuoLag = d.cellsGradQuo[s].GetDataSimple(t2.Add(-1));
                            //double vGradQuo = d.cellsGradQuo[s].GetData(smpl, t2); --> not used at the moment
                            double dContribD = vGradQuoLag * (vQuo - vQuoLag);
                            d.cellsContribD[s].SetData(t2, dContribD);

                            if (Globals.runningOnTTComputer && false) G.Writeln2(s + " quo " + vQuo + " quo.1 " + vQuoLag + " grad.1 " + vGradQuoLag + " " + dContribD);

                            if (mm.Contains(1))
                            {
                                double vRef = d.cellsRef[s].GetDataSimple(t2);
                                double vRefLag = d.cellsRef[s].GetDataSimple(t2.Add(-1));
                                double vGradRef = d.cellsGradRef[s].GetDataSimple(t2);
                                double vGradRefLag = d.cellsGradRef[s].GetDataSimple(t2.Add(-1));
                                double dContribM = vGradRef * (vQuo - vRef);
                                double dContribDRef = vGradRefLag * (vRef - vRefLag);
                                d.cellsContribM[s].SetData(t2, dContribM);
                                d.cellsContribDRef[s].SetData(t2, dContribDRef);
                            }
                        }
                        d.cellsContribD[residualName].SetData(t2, -(d.cellsQuo[residualName].GetDataSimple(t2) - d.cellsQuo[residualName].GetDataSimple(t2.Add(-1))));
                        d.cellsContribDRef[residualName].SetData(t2, -(d.cellsRef[residualName].GetDataSimple(t2) - d.cellsRef[residualName].GetDataSimple(t2.Add(-1))));
                        d.cellsContribM[residualName].SetData(t2, -(d.cellsQuo[residualName].GetDataSimple(t2) - d.cellsRef[residualName].GetDataSimple(t2)));
                    }
                }
            }
            finally
            {
                //Important: makes sure is is *always* nulled after a DECOMP
                Globals.precedentsContainer = null;
            }

            return d;

        }

        /// <summary>
        /// Called by DecompMain() and performs the low-level math, obtaining and preparing data and
        /// calculating gradients. Used for GAMS scalar models.
        /// </summary>
        /// <param name="tt1"></param>
        /// <param name="tt2"></param>
        /// <param name="eq"></param>
        /// <param name="workOrRefOrBoth"></param>
        /// <param name="residualName"></param>
        /// <param name="funcCounter"></param>
        /// <returns></returns>
        public static DecompData DecompLowLevelScalar(GekkoTime gt1, GekkoTime gt2, DecompStartHelper eqPeriods, DecompOperator op, string residualName, ref int funcCounter, bool missingAsZero, Model model)
        {
            //See OVERVIEW in DecompGetFuncExpressionsAndRecalc()

            ModelGamsScalar modelGamsScalar = model.modelGamsScalar;

            int tZero = 0;
            int ONE = 0; //some of the queries below asks for an eval of a leaded [+1] period, therefore this variable, which is used for scalar-2000 models.

            //See #kljaf89usafasdf for Gekko  model

            double eps = Globals.newtonSmallNumber;

            DecompData d = new DecompData();

            DecompInitDict(d);

            GekkoDictionary<string, int> vars = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            //foreach time period
            foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
            {
                // TODO TODO TODO
                // TODO TODO TODO
                // TODO TODO TODO
                // TODO TODO TODO skip via a dictionary if already done
                // TODO TODO TODO
                // TODO TODO TODO
                // TODO TODO TODO                

                int timeIndex1 = modelGamsScalar.FromGekkoTimeToTimeInteger(t);
                if (Program.options.bugfix_decomp_lagsleads)
                {
                    timeIndex1 += -Globals.decomp_offset;
                }

                if (eqPeriods.offset != 0)
                {
                    timeIndex1 += eqPeriods.offset;
                }
                
                int timeIndex2 = -timeIndex1; //will be added to timeIndex1 later on

                int offset = 0;
                if (modelGamsScalar.isPerpetualModel)
                {
                    ONE = 1;
                    timeIndex1 = 0;
                    timeIndex2 = modelGamsScalar.tBasis.Subtract(new GekkoTime(model.modelCommon.GetFreq(), Globals.decomp2000, 1));
                    tZero = t.Subtract(modelGamsScalar.perpetualT1) + timeIndex2;
                }
                else
                {                    
                    if (Program.options.bugfix_decomp_lagsleads)
                    {
                        offset = Globals.decomp_offset;
                    }

                    if (eqPeriods.offset != 0)
                    {
                        offset = -eqPeriods.offset;
                    }
                }                
                
                string s = AddTimeToIndexes(eqPeriods.name, new List<string>(eqPeriods.indexes.storage), modelGamsScalar.Maybe2000GekkoTime(t.Add(-offset)), false);
                int eqNumber = modelGamsScalar.dict_FromEqNameToEqNumber.GetInt(s);
                if (eqNumber == -12345)
                {
                    new Error("Could not find equation '" + s + "'");
                }

                double y0 = double.NaN;
                double y0a = double.NaN;
                double y0b = double.NaN;
                double y0c = double.NaN;

                double y1 = double.NaN;

                //foreach precedent variable
                int i = -1;
                foreach (PeriodAndVariable dp in modelGamsScalar.precedents[eqNumber].vars)
                {                                        
                    // --------------------------------------------
                    // This is where the decomposition takes place
                    // --------------------------------------------

                    i++;
                    string varName = modelGamsScalar.GetVarNameA(dp.variable);                    

                    if (op.isRaw)
                    {
                        //raw data.
                        //a bit of a hack here, since all data is fetched (extra will contain all periods),
                        //and both quo and ref are fetched.
                        //but it should be fast anyway
                        //normal multiplier like <m>

                        if (i == 0)
                        {
                            y0 = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1].eqNumber, true, tZero, ref funcCounter);
                            d.cellsRef[residualName].SetData(t, y0);
                            y1 = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1].eqNumber, false, tZero, ref funcCounter);
                            d.cellsQuo[residualName].SetData(t, y1);
                        }
                        double x0 = modelGamsScalar.GetData(dp.date, tZero, dp.variable, missingAsZero, true);
                        double x1 = modelGamsScalar.GetData(dp.date, tZero, dp.variable, missingAsZero, false);
                        int lag2 = dp.date + timeIndex2;
                        if (modelGamsScalar.isTimeless[dp.variable])
                        {
                            lag2 = 0;
                        }
                        else if (Program.options.bugfix_decomp_lagsleads)
                        {
                            lag2 -= Globals.decomp_offset;
                        }
                        else if (eqPeriods.offset != 0)
                        {
                            lag2 += eqPeriods.offset;
                        }
                        string name = DecompFirst() + ":" + ConvertToTurtleName(varName, lag2);
                        d.cellsRef[name].SetData(t, x0);
                        d.cellsQuo[name].SetData(t, x1);
                        if (!vars.ContainsKey(name))  //for decomp pivot
                        {
                            vars.Add(name, 0);
                        }
                    }
                    else
                    {
                        if (op.lowLevel == ELowLevel.OnlyQuo || op.lowLevel == ELowLevel.BothQuoAndRef)
                        {
                            //work difference like <d> ... or the special <mp>
                            if (i == 0)
                            {
                                y0a = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1].eqNumber, false, tZero, ref funcCounter);
                                d.cellsQuo[residualName].SetData(t, y0a);
                                y1 = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1 + (1 - ONE)].eqNumber, false, tZero + ONE, ref funcCounter);
                                d.cellsQuo[residualName].SetData(t.Add(1), y1);
                            }
                            double x0_before = modelGamsScalar.GetData(dp.date, tZero, dp.variable, missingAsZero, false);
                            double x1 = modelGamsScalar.GetData(dp.date + 1, tZero, dp.variable, missingAsZero, false);

                            try
                            {
                                double x0_after = x0_before + eps;                                
                                modelGamsScalar.SetData(dp.date, tZero, dp.variable, false, x0_after);                                
                                double y0_after = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1].eqNumber, false, tZero, ref funcCounter);

                                double grad = (y0_after - y0a) / eps;

                                //if (!G.IsNumericalError(grad) && grad != 0d)        //this grad != 0 originates from the Gekko decomp, and only makes sense when excact precedents are not known
                                //see also #sf94lkjsdjæ
                                if (Globals.decompFix || !G.IsNumericalError(grad))
                                {
                                    int lag2 = dp.date + timeIndex2;                                    
                                    if (modelGamsScalar.isTimeless[dp.variable])
                                    {
                                        lag2 = 0;
                                    }
                                    else if (Program.options.bugfix_decomp_lagsleads)
                                    {
                                        lag2 -= Globals.decomp_offset;
                                    }
                                    else if (eqPeriods.offset != 0)
                                    {
                                        lag2 += eqPeriods.offset;
                                    }
                                    string name = DecompFirst() + ":" + ConvertToTurtleName(varName, lag2);
                                    d.cellsQuo[name].SetData(t, x0_before); //for decomp period <2002 2002>, this will be 2001
                                    d.cellsQuo[name].SetData(t.Add(1), x1); //for decomp period <2002 2002>, this will be 2002
                                    d.cellsGradQuo[name].SetData(t, grad);  //for decomp period <2002 2002>, this will be 2001
                                    if (!vars.ContainsKey(name))  //for decomp pivot
                                    {
                                        vars.Add(name, 0);
                                    }
                                }
                            }
                            finally
                            {
                                modelGamsScalar.SetData(dp.date, tZero, dp.variable, false, x0_before);
                            }
                        }

                        if (op.lowLevel == ELowLevel.OnlyRef || op.lowLevel == ELowLevel.BothQuoAndRef)
                        {
                            //ref difference like <rd> ... or the special <mp>
                            if (i == 0)
                            {
                                y0b = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1].eqNumber, true, tZero, ref funcCounter);
                                d.cellsRef[residualName].SetData(t, y0b);
                                y1 = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1 + (1 - ONE)].eqNumber, true, tZero + ONE, ref funcCounter);
                                d.cellsRef[residualName].SetData(t.Add(1), y1);
                            }
                            double x0_before = modelGamsScalar.GetData(dp.date, tZero, dp.variable, missingAsZero, true);
                            double x1 = modelGamsScalar.GetData(dp.date + 1, tZero, dp.variable, missingAsZero, true);

                            try
                            {
                                double x0_after = x0_before + eps;
                                modelGamsScalar.SetData(dp.date, tZero, dp.variable, true, x0_after);
                                double y0_after = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1].eqNumber, true, tZero, ref funcCounter);
                                double grad = (y0_after - y0b) / eps;

                                //if (!G.IsNumericalError(grad) && grad != 0d)        //this grad != 0 originates from the Gekko decomp, and only makes sense when excact precedents are not known
                                //see also #sf94lkjsdjæ
                                if (Globals.decompFix || !G.IsNumericalError(grad))
                                {
                                    int lag2 = dp.date + timeIndex2;
                                    if (modelGamsScalar.isTimeless[dp.variable])
                                    {
                                        lag2 = 0;
                                    }
                                    else if (Program.options.bugfix_decomp_lagsleads)
                                    {
                                        lag2 -= Globals.decomp_offset;
                                    }
                                    else if (eqPeriods.offset != 0)
                                    {
                                        lag2 += eqPeriods.offset;
                                    }
                                    string name = DecompFirst() + ":" + ConvertToTurtleName(varName, lag2);
                                    d.cellsRef[name].SetData(t, x0_before); //for decomp period <2002 2002>, this will be 2001
                                    d.cellsRef[name].SetData(t.Add(1), x1); //for decomp period <2002 2002>, this will be 2002
                                    d.cellsGradRef[name].SetData(t, grad);  //for decomp period <2002 2002>, this will be 2001
                                    if (!vars.ContainsKey(name))  //for decomp pivot
                                    {
                                        vars.Add(name, 0);
                                    }
                                }
                            }
                            finally
                            {
                                modelGamsScalar.SetData(dp.date, tZero, dp.variable, true, x0_before);
                            }
                        }

                        if (op.lowLevel == ELowLevel.Multiplier)
                        {
                            //normal multiplier like <m>
                            if (i == 0)
                            {
                                y0c = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1].eqNumber, true, tZero, ref funcCounter);
                                d.cellsRef[residualName].SetData(t, y0c);
                                y1 = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1].eqNumber, false, tZero, ref funcCounter);
                                d.cellsQuo[residualName].SetData(t, y1);
                            }
                            double x0_before = modelGamsScalar.GetData(dp.date, tZero, dp.variable, missingAsZero, true);
                            double x1 = modelGamsScalar.GetData(dp.date, tZero, dp.variable, missingAsZero, false);

                            try
                            {
                                double x0_after = x0_before + eps;
                                modelGamsScalar.SetData(dp.date, tZero, dp.variable, true, x0_after);
                                double y0_after = modelGamsScalar.Eval(eqPeriods.periods[timeIndex1].eqNumber, true, tZero, ref funcCounter);
                                double grad = (y0_after - y0c) / eps;

                                //if (!G.IsNumericalError(grad) && grad != 0d)    //this grad != 0 originates from the Gekko decomp, and only makes sense when excact precedents are not known
                                //see also #sf94lkjsdjæ
                                if (Globals.decompFix || !G.IsNumericalError(grad))
                                {
                                    int lag2 = dp.date + timeIndex2;
                                    if (modelGamsScalar.isTimeless[dp.variable])
                                    {
                                        lag2 = 0;
                                    }
                                    else if (Program.options.bugfix_decomp_lagsleads)
                                    {
                                        lag2 -= Globals.decomp_offset;
                                    }
                                    else if (eqPeriods.offset != 0)
                                    {
                                        lag2 += eqPeriods.offset;
                                    }
                                    string name = DecompFirst() + ":" + ConvertToTurtleName(varName, lag2);
                                    d.cellsRef[name].SetData(t, x0_before);
                                    d.cellsQuo[name].SetData(t, x1);
                                    d.cellsGradRef[name].SetData(t, grad);
                                    if (!vars.ContainsKey(name))  //for decomp pivot
                                    {
                                        vars.Add(name, 0);
                                    }
                                }
                            }
                            finally
                            {
                                modelGamsScalar.SetData(dp.date, tZero, dp.variable, true, x0_before);
                            }
                        }
                    }
                }
            }

            //Here, cellsQuo + cellsRef + cellsGradQuo + cellsGradRef are calculated.
            //Grad tells us which lags are actually active.
            //If we know that lags beforehand, we could limit the lag loop and save time here.

            if (!op.isRaw)
            {
                foreach (GekkoTime t2 in new GekkoTimeIterator(gt1, gt2))
                {
                    int add = 1; if (op.lowLevel == ELowLevel.Multiplier) add = 0;
                    GekkoTime t = t2.Add(add);
                    foreach (string s in vars.Keys)
                    {
                        if (op.lowLevel == ELowLevel.OnlyQuo || op.lowLevel == ELowLevel.BothQuoAndRef)
                        {
                            double vQuo = d.cellsQuo[s].GetDataSimple(t);
                            double vQuoLag = d.cellsQuo[s].GetDataSimple(t.Add(-1));
                            double vGradQuoLag = d.cellsGradQuo[s].GetDataSimple(t.Add(-1));
                            double dContribD = vGradQuoLag * (vQuo - vQuoLag);
                            d.cellsContribD[s].SetData(t, dContribD);
                            d.cellsChangeD[s].SetData(t, vQuo - vQuoLag);
                        }

                        if (op.lowLevel == ELowLevel.OnlyRef || op.lowLevel == ELowLevel.BothQuoAndRef)
                        {
                            double vRef = d.cellsRef[s].GetDataSimple(t);
                            double vRefLag = d.cellsRef[s].GetDataSimple(t.Add(-1));
                            double vGradRefLag = d.cellsGradRef[s].GetDataSimple(t.Add(-1));
                            double dContribDRef = vGradRefLag * (vRef - vRefLag);
                            d.cellsContribDRef[s].SetData(t, dContribDRef);
                            d.cellsChangeDRef[s].SetData(t, vRef - vRefLag);
                        }

                        if (op.lowLevel == ELowLevel.Multiplier)
                        {
                            double vQuo = d.cellsQuo[s].GetDataSimple(t);
                            double vRef = d.cellsRef[s].GetDataSimple(t);
                            double vGradRef = d.cellsGradRef[s].GetDataSimple(t);
                            double dContribM = vGradRef * (vQuo - vRef);
                            d.cellsContribM[s].SetData(t, dContribM);
                            d.cellsChangeM[s].SetData(t, vQuo - vRef);
                        }
                    }

                    if (op.lowLevel == ELowLevel.OnlyQuo || op.lowLevel == ELowLevel.BothQuoAndRef)
                    {
                        d.cellsContribD[residualName].SetData(t, -(d.cellsQuo[residualName].GetDataSimple(t) - d.cellsQuo[residualName].GetDataSimple(t.Add(-1))));
                    }

                    if (op.lowLevel == ELowLevel.OnlyRef || op.lowLevel == ELowLevel.BothQuoAndRef)
                    {
                        d.cellsContribDRef[residualName].SetData(t, -(d.cellsRef[residualName].GetDataSimple(t) - d.cellsRef[residualName].GetDataSimple(t.Add(-1))));
                    }

                    if (op.lowLevel == ELowLevel.Multiplier)
                    {
                        d.cellsContribM[residualName].SetData(t, -(d.cellsQuo[residualName].GetDataSimple(t) - d.cellsRef[residualName].GetDataSimple(t)));
                    }
                }
            }

            return d;
        }

        public static string DecompFirst()
        {
            return Program.databanks.GetFirst().name;
        }

        private static void DecompInitDict(DecompData d)
        {
            if (d.cellsGradQuo == null) d.cellsGradQuo = new DecompDict();
            if (d.cellsQuo == null) d.cellsQuo = new DecompDict();
            if (d.cellsContribD == null) d.cellsContribD = new DecompDict();
            if (d.cellsChangeD == null) d.cellsChangeD = new DecompDict();
            if (d.cellsGradRef == null) d.cellsGradRef = new DecompDict();
            if (d.cellsRef == null) d.cellsRef = new DecompDict();
            if (d.cellsContribDRef == null) d.cellsContribDRef = new DecompDict();
            if (d.cellsChangeDRef == null) d.cellsChangeDRef = new DecompDict();
            if (d.cellsContribM == null) d.cellsContribM = new DecompDict();
            if (d.cellsChangeM == null) d.cellsChangeM = new DecompDict();
        }

        /// <summary>
        /// Transforms a pivot table from DecompMain() into a table suitable for showing in the Gekko GUI.
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="decompDataMAINClone"></param>
        /// <param name="format"></param>
        /// <param name="operator1"></param>
        /// <param name="isShares"></param>
        /// <param name="smpl"></param>
        /// <param name="lhs"></param>
        /// <param name="expressionText"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="frame"></param>
        /// <param name="operatorOneOf3Types"></param>
        /// 
        /// <returns></returns>


        public static DecompOutput DecompPivotToTable(GekkoSmpl smpl, GekkoTime per1, GekkoTime per2, DecompData decompDataMAINClone, DecompDatas decompDatas, string lhs, DecompOperator op, EContribType operatorOneOf3Types, DecompOptions2 decompOptions2, Model model)
        {
            //See OVERVIEW in DecompGetFuncExpressionsAndRecalc()

            string lhs2 = G.HandleBlanksRemove(decompOptions2.link[0].varnames);  //Seems lhs here just is "Expression value"
            ERowsCols rowsCols = VariablesOnRowsOrCols(decompOptions2);

            string format2 = GetNumberFormat(decompOptions2);
            int parentI = 0;

            ENormalizeType normalize = ENormalizeType.Lags;
            if (op.lowLevel == ELowLevel.BothQuoAndRef)
            {
                DecompAdjust(per1, per2, decompOptions2, parentI, decompDataMAINClone, decompDatas, EContribType.D, normalize, op);
                DecompAdjust(per1, per2, decompOptions2, parentI, decompDataMAINClone, decompDatas, EContribType.RD, normalize, op);
            }
            else
            {
                int deduct = 0;
                if (op.isDoubleDifQuo || op.isDoubleDifRef) deduct = -1;
                DecompAdjust(per1.Add(deduct), per2, decompOptions2, parentI, decompDataMAINClone, decompDatas, operatorOneOf3Types, normalize, op);
            }

            FrameLight frame = DecompPivotCreateDataframe(smpl, per1, per2, lhs, lhs2, decompDataMAINClone, decompDatas, op, operatorOneOf3Types, decompOptions2, model);

            DecomposeReplaceVars(decompOptions2.rows, Globals.col_t, Globals.col_variable, Globals.col_lag, Globals.col_universe, Globals.col_equ);
            DecomposeReplaceVars(decompOptions2.cols, Globals.col_t, Globals.col_variable, Globals.col_lag, Globals.col_universe, Globals.col_equ);

            List<int> rowIndexes = ChooseVarsLagsTimeSetsEtc(decompOptions2.rows, frame.frameDimensionNames);
            List<int> colIndexes = ChooseVarsLagsTimeSetsEtc(decompOptions2.cols, frame.frameDimensionNames);

            Func<FrameLightRow, bool> filter = dataframeRow =>
            {
                //false if it must be filtered
                bool b = true;
                //if (G.Equal(dataframeRow.storageDimensions[0].text, "tot") || G.Equal(dataframeRow.storageDimensions[1].text, "tot") || G.Equal(dataframeRow.storageDimensions[2].text, "tot")) b = false;
                //if (dataframeRow.storageDimensions[1].text == "20" || dataframeRow.storageDimensions[1].text == "21" || dataframeRow.storageDimensions[1].text == "22") b = false;
                return b;
            };

            // Group
            Func<FrameLightRow, int, string> group = (dataframeRow, i) =>
            {
                string s = dataframeRow.storageDimensions[i].text;

                if (false)
                {
                    //SLACK SLACK SLACK
                    //SLACK SLACK SLACK
                    //SLACK SLACK SLACK Should do this lookup before calling .Compute(). But never mind: not speed critical code.
                    //SLACK SLACK SLACK
                    //SLACK SLACK SLACK
                    int iAge = -12345;
                    if (dataframeRow.parent.frameDimensionNames.TryGetValue("a_", out iAge)) //#a_
                    {
                        if (i == iAge)
                        {
                            //MessageBox.Show("Age aggregation...?");
                            if (s == "99-") s = "99";
                            s = AgeIntervals(s);
                        }
                    }
                }
                return s;
            };

            Func<IEnumerable<AggContainer>, AggContainer> agg = (m) =>
            {
                AggContainer aggregate = new AggContainer(0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0, new List<string>(), 0d, 0d, 0d, 0d, 0d, 0d);
                int n = 0;
                foreach (AggContainer x in m)
                {
                    n++;
                    aggregate.change += x.change;
                    aggregate.changeAlternative += x.changeAlternative;
                    aggregate.level += x.level;
                    aggregate.levelLag += x.levelLag;
                    aggregate.levelLag2 += x.levelLag2;
                    aggregate.levelRef += x.levelRef;
                    aggregate.levelRefLag += x.levelRefLag;
                    aggregate.levelRefLag2 += x.levelRefLag2;
                    aggregate.n += x.n;
                    aggregate.fullVariableNames.AddRange(x.fullVariableNames);
                    aggregate.prime += x.prime;
                    // -----
                    aggregate.dFirstLevelLag += x.dFirstLevelLag;
                    aggregate.dFirstLevelLag2 += x.dFirstLevelLag2;
                    aggregate.dFirstLevelRef += x.dFirstLevelRef;
                    aggregate.dFirstLevelRefLag += x.dFirstLevelRefLag;
                    aggregate.dFirstLevelRefLag2 += x.dFirstLevelRefLag2;
                }
                //For the LHS values we take averages.
                //Think percentages like 17/50 = 10/50 + 3/50 + 4/50. If we sum 3+4 = 7, we would like
                //to see it as (3+4)/50, not (3+4)/(50+50). This is obvious intra a period, summing
                //variables. When for instance removing time dimension, this average rule is perhaps less
                //obvious. Or is it? We are generally *summing*, so shouldn't percentages sum up when
                //removing time dimension (which they do when doing averages).
                aggregate.dFirstLevelLag /= n;
                aggregate.dFirstLevelLag2 /= n;
                aggregate.dFirstLevelRef /= n;
                aggregate.dFirstLevelRefLag /= n;
                aggregate.dFirstLevelRefLag2 /= n;

                return aggregate;

            };

            if (decompOptions2.expand)
            {
                //TODO: What if we want it on cols? Maybe decomp <expandcols> ? 
                //      So that <expand> and <expandrows> is for rows, and <expandcols> is for cols.
                rowIndexes = new List<int>();
                colIndexes = new List<int>();
                rowIndexes.Add(frame.frameDimensionNames["expand"]);
                rowIndexes.Add(frame.frameDimensionNames["lhs"]);
                colIndexes.Add(frame.frameDimensionNames["time"]);
            }

            Dictionary<string, Dictionary<string, AggContainer>> pivotTable = GekkoPivotTable.Compute(frame, rowIndexes, colIndexes, agg, decompOptions2, filter, group);

            decompOptions2.all.Clear();
            foreach (string s in frame.frameDimensionNames.Keys)
            {
                decompOptions2.all.Add(s);
            }

            if (false)
            {
                int xlag = 0; string temp = null;
                ConvertFromTurtleName(decompDataMAINClone.lhs, true, out temp, out xlag);
                string normalizerVariableWithIndex = null;
                if (temp != null)
                {
                    normalizerVariableWithIndex = G.HandleBlanksRemove(G.Chop_RemoveBank(temp));
                }
            }

            GekkoDictionary<string, bool> rownames2 = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, bool> colnames2 = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, Dictionary<string, AggContainer>> row in pivotTable)
            {
                if (!rownames2.ContainsKey(row.Key)) rownames2.Add(row.Key, false);
                foreach (KeyValuePair<string, AggContainer> column in row.Value)
                {
                    if (!colnames2.ContainsKey(column.Key)) colnames2.Add(column.Key, false);
                }
            }

            List<string> rownames, colnames, rownamesWithResiduals, colnamesWithResiduals;
            DecompOrderRowAndColNames(rownames2, colnames2, decompOptions2.showErrors, out rownames, out colnames, out rownamesWithResiduals, out colnamesWithResiduals);
            Table table = DecompGetTableFromPivot(pivotTable, op, decompOptions2, format2, rownames, colnames);
            Tuple<bool, bool> decompRowsOrColsPrimeBased = DecompRowsOrColsPrimeBased(rownamesWithResiduals, colnamesWithResiduals, pivotTable, table, decompOptions2, op, format2);
            DecompTablePostProcessing(table, rownames, colnames, decompOptions2, model);
            //table.PrintCellsForDebug();

            DecompOutput decompOutput = null;
            DecompTableHandleSignAndShares(table, decompOptions2);
            decompOutput = DecompTableHandleSortAndIgnoreAndErrors(table, decompOptions2, model);
            decompOutput.rowsOrColsSumUp = decompRowsOrColsPrimeBased;
            return decompOutput;
        }

        private static string AgeIntervals(string s)
        {
            int span = 20;
            int ii = -12345;
            if (int.TryParse(s, out ii))
            {
                int intervalStart = (ii / span) * span;
                int intervalEnd = intervalStart + span - 1;
                s = intervalStart + ".." + intervalEnd;
            }
            return s;
        }

        /// <summary>
        /// Checks if the rows or columns of the generated table conceptually add up or not, so
        /// that the sum of elements #2 and on is equal to element #1, in either the row or
        /// col orientation (or both, in principle). Uses prime numbers internally for that.
        /// NOTE: Normal decomp table will return {true, false}, because the rows sum up (for each column).
        /// </summary>
        /// <param name="rownamesWithResiduals"></param>
        /// <param name="colnamesWithResiduals"></param>
        /// <param name="pivotTable"></param>
        /// <param name="table"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="op"></param>
        /// <param name="format2"></param>
        /// <returns></returns>
        private static Tuple<bool, bool> DecompRowsOrColsPrimeBased(List<string> rownamesWithResiduals, List<string> colnamesWithResiduals, Dictionary<string, Dictionary<string, AggContainer>> pivotTable, Table table, DecompOptions2 decompOptions2, DecompOperator op, string format2)
        {
            Table tableWithErrors;
            if (decompOptions2.showErrors)
            {
                tableWithErrors = table;  //no need to recalculate it: residuals are already present
            }
            else
            {
                //We have to calc it again, but that should be pretty fast
                tableWithErrors = DecompGetTableFromPivot(pivotTable, op, decompOptions2, format2, rownamesWithResiduals, colnamesWithResiduals);
            }

            bool rowsSumUp = true;
            for (int j = 2; j <= tableWithErrors.GetColMaxNumber(); j++)
            {
                double primeSum = 0d;
                int count = 0;
                for (int i = 2; i <= tableWithErrors.GetRowMaxNumber(); i++)
                {
                    count++;
                    Cell c = tableWithErrors.Get(i, j);
                    if (c != null) primeSum += c.prime_hack;
                }
                bool match = IsPrimeMatch(primeSum);
                if (!match)
                {
                    rowsSumUp = false;
                    break;
                }
            }

            bool colsSumUp = true;
            for (int i = 2; i <= tableWithErrors.GetRowMaxNumber(); i++)
            {
                double primeSum = 0d;
                int count = 0;
                for (int j = 2; j <= tableWithErrors.GetColMaxNumber(); j++)
                {
                    count++;
                    Cell c = tableWithErrors.Get(i, j);
                    if (c != null) primeSum += c.prime_hack;
                }

                bool match = IsPrimeMatch(primeSum);
                if (!match)
                {
                    colsSumUp = false;
                    break;
                }
            }

            return new Tuple<bool, bool>(rowsSumUp, colsSumUp);

            bool IsPrimeMatch(double rowPrimeSum)
            {
                if (Math.Abs(rowPrimeSum) < 0.01d) return true;
                return false;
            }
        }

        /// <summary>
        /// Transforms a list like ("vars", "lags") into a list like (0, 2), where these integers correspond to the dataframe column.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="frameDimensionNames"></param>
        /// <returns></returns>
        private static List<int> ChooseVarsLagsTimeSetsEtc(List<string> names, GekkoDictionary<string, int> frameDimensionNames)
        {
            List<int> indexes = new List<int>();
            foreach (string name in names)
            {
                int i = -12345;
                if (frameDimensionNames.TryGetValue(name, out i))
                {
                    indexes.Add(i);
                    if (name == Globals.col_variable) indexes.Add(frameDimensionNames[Globals.col_lhs]); //so lhs will separate it from other vars
                }
                else
                {
                    new Error("Unrecognized name '" + name + "' selected in ROW or COL");
                }

                //if (name == Globals.col_variable)
                //{
                //    indexes.Add(frameDimensionNames[Globals.col_variable]);
                //    indexes.Add(frameDimensionNames[Globals.col_lhs]);  //so lhs will separate it from other vars
                //}
                //else if (name == Globals.col_lag)
                //{
                //    indexes.Add(frameDimensionNames[Globals.col_lag]);
                //}
                //else if (name == Globals.col_t)
                //{
                //    indexes.Add(frameDimensionNames[Globals.col_t]);
                //}
                //else new Error("Unrecognized name '" + name + "' selected in ROW or COL");
            }
            return indexes;
        }

        /// <summary>
        /// Order alphabetically.
        /// </summary>
        /// <param name="rownames2"></param>
        /// <param name="colnames2"></param>
        /// <param name="showErrors"></param>
        /// <param name="rownames"></param>
        /// <param name="colnames"></param>
        /// <param name="rownamesWithResiduals"></param>
        /// <param name="colnamesWithResiduals"></param>
        private static void DecompOrderRowAndColNames(GekkoDictionary<string, bool> rownames2, GekkoDictionary<string, bool> colnames2, bool showErrors, out List<string> rownames, out List<string> colnames, out List<string> rownamesWithResiduals, out List<string> colnamesWithResiduals)
        {
            List<string> rownames3 = rownames2.Keys.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default)).ToList();
            List<string> colnames3 = colnames2.Keys.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default)).ToList();

            MoveResToEnd(rownames3);
            MoveResToEnd(colnames3);

            rownames = new List<string>();
            colnames = new List<string>();
            rownamesWithResiduals = new List<string>();
            colnamesWithResiduals = new List<string>();
            int rowResiduals = 0;
            foreach (string rowname in rownames3)
            {
                if (!showErrors && rowname.Contains(Globals.decompResidualName))
                {
                    rowResiduals++;
                    rownamesWithResiduals.Add(rowname);
                    continue;
                }
                rownames.Add(rowname);
                rownamesWithResiduals.Add(rowname);
            }
            int colResiduals = 0;
            foreach (string colname in colnames3)
            {
                if (!showErrors && colname.Contains(Globals.decompResidualName))
                {
                    colResiduals++;
                    colnamesWithResiduals.Add(colname);
                    continue;
                }
                colnames.Add(colname);
                colnamesWithResiduals.Add(colname);
            }
            if (rownames3.Count != rownames.Count + rowResiduals) new Error("Decomp pivot: count problem (lhs variable)");
            if (colnames3.Count != colnames.Count + colResiduals) new Error("Decomp pivot: count problem (lhs variable)");
            if (rownames3.Count != rownamesWithResiduals.Count) new Error("Decomp pivot: count problem (lhs variable)");
            if (colnames3.Count != colnamesWithResiduals.Count) new Error("Decomp pivot: count problem (lhs variable)");
        }

        private static void MoveResToEnd(List<string> m)
        {
            List<string> temp = new List<string>();
            for (int i = m.Count - 1; i >= 0; i--)
            {
                if (G.StartsWith(m[i], Globals.decompResidualPrefix))
                {
                    temp.Add(m[i]);
                    m.RemoveAt(i); // Remove from the original list
                }
            }
            int ii = -12345;
            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].StartsWith(Globals.decompResidualName))
                {
                    ii = i; break;
                }
            }
            if (ii != -12345) m.InsertRange(ii, temp);
            else m.AddRange(temp);
        }

        public static string GetNumberFormat(DecompOptions2 decompOptions2)
        {
            int decimals = 0;
            if (decompOptions2.decompOperator.isPercentageType || decompOptions2.isShares) decimals = decompOptions2.decimalsPch;
            else decimals = decompOptions2.decimalsLevel;
            return "f16." + decimals.ToString();
        }

        /// <summary>
        /// For a data cell, it finds the list of variables inside (.vars_hack). Will return the first of these,
        /// with any "¤[-1]" etc. stripped off, so "x¤[-1]" becomes "x". If onlyIfUnique==true and the list count
        /// is not 1, null is returned. May return null.
        /// </summary>
        /// <param name="c2"></param>
        /// <param name="onlyIfUnique"></param>
        /// <returns></returns>
        public static string HiddenVariableHelper(Cell c2, bool onlyIfUnique)
        {
            if (c2 == null) return null;
            List<string> vars = c2.vars_hack;  //See also GetVarsHack().

            if (vars == null || vars.Count == 0)
            {
                return null;
            }
            if (onlyIfUnique)
            {
                if (vars.Count != 1) return null;
            }
            string var = null;
            if (vars.Count > 0) var = vars[0];  //#dskla8asjkdfa
            int lag; string name;
            Decomp.ConvertFromTurtleName(var, false, out name, out lag);
            return name;
        }

        /// <summary>
        /// Setup filters.
        /// </summary>
        /// <param name="decompOptions2"></param>
        public static void DecompPivotHandleFilters(DecompOptions2 decompOptions2)
        {
            if (decompOptions2.filters == null)
            {
                decompOptions2.filters = new List<FrameFilter>();
                if (decompOptions2.where != null)
                {
                    foreach (List<string> filter in decompOptions2.where)
                    {
                        FrameFilter filter1 = new FrameFilter();
                        filter1.active = true;
                        filter1.name = filter[filter.Count - 1];
                        filter1.selected = filter.GetRange(0, filter.Count - 1);
                        decompOptions2.filters.Add(filter1);
                    }
                }
            }
        }


        /// <summary>
        /// Create table object from pivot (which is dictionary-based)
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="op"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="format2"></param>
        /// <param name="rownames"></param>
        /// <param name="colnames"></param>
        /// <returns></returns>
        private static Table DecompGetTableFromPivot(Dictionary<string, Dictionary<string, AggContainer>> pivot, DecompOperator op, DecompOptions2 decompOptions2, string format2, List<string> rownames, List<string> colnames)
        {
            Table table = new Table();
            table.writeOnce = true;

            int firstI = -12345;
            int firstJ = -12345;

            for (int i = 0; i < rownames.Count; i++)
            {
                if (rownames[i].Contains(Globals.pivotHelper2New)) { firstI = i; break; }
            }
            for (int j = 0; j < colnames.Count; j++)
            {
                if (colnames[j].Contains(Globals.pivotHelper2New)) { firstJ = j; break; }
            }
            if (firstI != -12345 && firstJ != -12345 && Globals.runningOnTTComputer) MessageBox.Show("First identification problem");

            for (int i = 0; i < rownames.Count; i++)
            {
                Dictionary<string, AggContainer> rowDict = null; pivot.TryGetValue(rownames[i], out rowDict);

                for (int j = 0; j < colnames.Count; j++)
                {
                    AggContainer agg = null; if (rowDict != null) rowDict.TryGetValue(colnames[j], out agg);
                    if (agg != null)
                    {
                        double value = double.NaN;

                        // ----- first start -----------------------------------------------                        
                        //double dFirstLevelLag = double.NaN;
                        //double dFirstLevelLag2 = double.NaN;
                        //double dFirstLevelRef = double.NaN;
                        //double dFirstLevelRefLag = double.NaN;
                        //double dFirstLevelRefLag2 = double.NaN;

                        //AggContainer tdFirst = null;

                        //if (firstJ != -12345)
                        //{
                        //    if (rowDict != null) rowDict.TryGetValue(colnames[firstJ], out tdFirst);
                        //    if (tdFirst != null)
                        //    {
                        //        dFirstLevelLag = tdFirst.levelLag;
                        //        dFirstLevelLag2 = tdFirst.levelLag2;
                        //        dFirstLevelRef = tdFirst.levelRef;
                        //        dFirstLevelRefLag = tdFirst.levelRefLag;
                        //        dFirstLevelRefLag2 = tdFirst.levelRefLag2;
                        //    }
                        //}
                        //else if (firstI != -12345)
                        //{
                        //    Dictionary<string, AggContainer> rowDictFirst = null; pivot.TryGetValue(rownames[firstI], out rowDictFirst);
                        //    if (rowDictFirst != null) rowDictFirst.TryGetValue(colnames[j], out tdFirst);
                        //    dFirstLevelLag = tdFirst.levelLag;
                        //    dFirstLevelLag2 = tdFirst.levelLag2;
                        //    dFirstLevelRef = tdFirst.levelRef;
                        //    dFirstLevelRefLag = tdFirst.levelRefLag;
                        //    dFirstLevelRefLag2 = tdFirst.levelRefLag2;
                        //}

                        if (op.OperatorLower() == "n" || op.OperatorLower() == "xn")
                        {
                            value = agg.level;
                        }
                        else if (op.OperatorLower() == "rn" || op.OperatorLower() == "r" || op.OperatorLower() == "xrn" || op.OperatorLower() == "xr")
                        {
                            value = agg.levelRef;
                        }
                        else if (op.OperatorLower() == "d" || op.OperatorLower() == "sd")
                        {
                            value = agg.change;
                        }
                        else if (op.OperatorLower() == "p" || op.OperatorLower() == "sp")
                        {
                            value = agg.change / agg.dFirstLevelLag * 100d;
                        }
                        else if (op.OperatorLower() == "dp" || op.OperatorLower() == "sdp")
                        {
                            value = agg.change / agg.dFirstLevelLag * 100d - agg.changeAlternative / agg.dFirstLevelLag2 * 100d;
                        }
                        else if (op.OperatorLower() == "m" || op.OperatorLower() == "sm")
                        {
                            value = agg.change;
                        }
                        else if (op.OperatorLower() == "q" || op.OperatorLower() == "sq")
                        {
                            value = agg.change / agg.dFirstLevelRef * 100d;
                        }
                        else if (op.OperatorLower() == "mp" || op.OperatorLower() == "smp")
                        {
                            value = agg.change / agg.dFirstLevelLag * 100d - agg.changeAlternative / agg.dFirstLevelRefLag * 100d;
                        }
                        else if (op.OperatorLower() == "xd")
                        {
                            value = agg.level - agg.levelLag;
                        }
                        else if (op.OperatorLower() == "xp")
                        {
                            value = (agg.level - agg.levelLag) / agg.levelLag * 100d;
                        }
                        else if (op.OperatorLower() == "xdp")
                        {
                            value = (agg.level - agg.levelLag) / agg.levelLag * 100d - (agg.levelLag - agg.levelLag2) / agg.levelLag2 * 100d;
                        }
                        else if (op.OperatorLower() == "xm")
                        {
                            value = agg.level - agg.levelRef;
                        }
                        else if (op.OperatorLower() == "xq")
                        {
                            value = (agg.level - agg.levelRef) / agg.levelRef * 100d;
                        }
                        else if (op.OperatorLower() == "xmp")
                        {
                            value = (agg.level - agg.levelLag) / agg.levelLag * 100d - (agg.levelRef - agg.levelRefLag) / agg.levelRefLag * 100d;
                        }
                        // -----------------
                        else if (op.OperatorLower() == "rd" || op.OperatorLower() == "srd")
                        {
                            value = agg.change;
                        }
                        else if (op.OperatorLower() == "rp" || op.OperatorLower() == "srp")
                        {
                            value = agg.change / agg.dFirstLevelRefLag * 100d;
                        }
                        else if (op.OperatorLower() == "rdp" || op.OperatorLower() == "srdp")
                        {
                            value = agg.change / agg.dFirstLevelRefLag * 100d - agg.changeAlternative / agg.dFirstLevelRefLag2 * 100d;
                        }
                        else if (op.OperatorLower() == "xrd")
                        {
                            value = agg.levelRef - agg.levelRefLag;
                        }
                        else if (op.OperatorLower() == "xrp")
                        {
                            value = (agg.levelRef - agg.levelRefLag) / agg.levelRefLag * 100d;
                        }
                        else if (op.OperatorLower() == "xrdp")
                        {
                            value = (agg.levelRef - agg.levelRefLag) / agg.levelRefLag * 100d - (agg.levelRefLag - agg.levelRefLag2) / agg.levelRefLag2 * 100d;
                        }

                        if (decompOptions2.count == ECountType.N)
                        {
                            table.SetNumber(i + 2, j + 2, agg.n, "f16.0");
                        }
                        else if (decompOptions2.count == ECountType.Names)
                        {
                            string tmp2 = null;
                            if (agg.fullVariableNames != null)
                            {
                                List<string> tmp = new List<string>();
                                foreach (string s in agg.fullVariableNames)
                                {
                                    string s2 = FullVariableNamePretty(s, true);
                                    string s3 = TrimAndRemoveLag0(s2);
                                    tmp.Add(s3);
                                }
                                tmp2 = Stringlist.GetListWithCommas(tmp, "  ");  //x[i, j], x[i, k] --> x[i, j],  x[i, k]
                            }
                            else
                            {
                                tmp2 = Text1(0);
                            }
                            table.Set(i + 2, j + 2, tmp2);
                        }
                        else
                        {
                            table.SetNumber(i + 2, j + 2, value, format2);
                        }

                        Cell c = table.Get(i + 2, j + 2);
                        c.vars_hack = agg.fullVariableNames;
                        c.value_hack = value;  //stored for sort and ignore later on
                        c.backgroundColor = "Transparent";
                        c.prime_hack = agg.prime;
                    }
                }
            }

            //Replace null-cells with 0-cells.
            for (int i = 2; i <= table.GetRowMaxNumber(); i++)
            {
                for (int j = 2; j <= table.GetColMaxNumber(); j++)
                {
                    if (table.Get(i, j) == null)
                    {
                        table.SetNumber(i, j, 0d, format2);
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// Converts something like "x[a,b]¤[-1]" --> "x[a, b][-1]". Also converts "ZZZZZZZZ_residal" to "Residual".
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FullVariableNamePretty(string s, bool replaceResidualName)
        {            
            if (s == null) return s;
            //s = s.Replace("¤", "").Replace(",", ", ");
            s = s.Replace("¤", "");
            if (replaceResidualName) s = s.Replace(Globals.decompResidualName, Globals.decompResidualName2);
            return s;
        }

        /// <summary>
        /// This method does not do much.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="rownames"></param>
        /// <param name="colnames"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="model"></param>
        private static void DecompTablePostProcessing(Table tab, List<string> rownames, List<string> colnames, DecompOptions2 decompOptions2, Model model)
        {
            ERowsCols rowsCols = VariablesOnRowsOrCols(decompOptions2);
            if (!Decomp.VarsAndTimeDimensionsAreSeparate(decompOptions2)) rowsCols = ERowsCols.None;

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
                if (s != null) s = s.Replace(Globals.pivotHelper1, "").Replace(Globals.pivotHelper2New, "").Replace(Globals.decompResidualName, Globals.decompResidualName2);
                tab.Set(i + 2, 1, s);
                if (rowsCols == ERowsCols.Cols) tab.Get(i + 2, 1).date_hack = GekkoTime.FromStringToGekkoTime(s, false, false);
            }

            for (int j = 0; j < colnames.Count; j++)
            {
                string s = colnames[j];
                if (s != null) s = s.Replace(Globals.pivotHelper1, "").Replace(Globals.pivotHelper2New, "").Replace(Globals.decompResidualName, Globals.decompResidualName2); ;
                tab.Set(1, j + 2, s);
                if (rowsCols == ERowsCols.Rows) tab.Get(1, j + 2).date_hack = GekkoTime.FromStringToGekkoTime(s, false, false);
            }
        }

        /// <summary>
        /// Tries 'm' if databanks are reasonable, else 'd'
        /// </summary>
        /// <returns></returns>
        public static DecompOperator GetFindOperator()
        {
            bool canUseM = false;
            int w = Program.databanks.GetFirst().storage.Count;
            int r = Program.databanks.GetRef().storage.Count;
            if (w != 0 && r != 0)
            {
                double rel = (double)w / (double)r;
                if (rel > 0.9d && rel < 1.1d) canUseM = true;
            }

            DecompOperator z = null;
            if (Globals.useMAsDefaultOperatorInFindWindow && canUseM)
            {
                //if Work or Ref are empty, or if Ref is not of approximate same size
                //as Work, we will not use "m" but instead "d".                            
                z = new DecompOperator("m");
            }
            else
            {
                z = new DecompOperator("d");
            }
            return z;
        }

        public static bool DecompPivotAggregateGetFreeValues(FrameLight_OLD frame, DecompOptions2 decompOptions2)
        {
            bool getFreeValues = false;
            if (decompOptions2.freeValues == null)
            {
                decompOptions2.freeValues = new List<GekkoDictionary<string, string>>();
                getFreeValues = true;
            }

            for (int i = 0; i < frame.frameColNames.Count; i++)
            {
                decompOptions2.freeValues.Add(new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase));
            }

            return getFreeValues;
        }

        public static string Text1(int i)
        {
            if (i == 0) return "";  //corresponds to a cell with count = 0
            else if (i == 1) return "Cannot determine exact variable name, perhaps because some parts of the name is on rows, and other parts on columns. You may try rearranging via the Rows/Cols selector.";
            else return "";
        }

        private static FrameLight DecompPivotCreateDataframe(GekkoSmpl smpl, GekkoTime per1, GekkoTime per2, string lhs, string lhs2, DecompData decompDataMAINClone, DecompDatas decompDatas, DecompOperator op, EContribType operatorOneOf3Types, DecompOptions2 decompOptions2, Model model)
        {
            FrameLight frame = new FrameLight();
            int prime = Globals.startPrime;  //1013: next one is 1019
            DecompDict dd = DecompPivotCreateDataframeGetDict(decompDataMAINClone, op, operatorOneOf3Types);

            //We start ordring the list, so that the LHS is in position 1.
            int hit = 0;
            List<string> orderedNames = new List<string>();
            foreach (string fullVariableName in dd.storage.Keys)
            {
                if (ChopFullVariableName(lhs2, fullVariableName).isLhs)
                {
                    orderedNames.Add(fullVariableName); hit++;
                }
            }
            foreach (string fullVariableName in dd.storage.Keys)
            {
                if (!ChopFullVariableName(lhs2, fullVariableName).isLhs) orderedNames.Add(fullVariableName);
            }
            if (Globals.runningOnTTComputer && !Globals.browser && hit != 1)
            {                
                MessageBox.Show("LHS problem: hit number is: " + hit);                
            }         

            // ------------------------------------------------------------------------------
            // Loop over PERIODS
            // ------------------------------------------------------------------------------

            foreach (GekkoTime t2 in new GekkoTimeIterator(per1, per2))
            {

                double primeSumWithoutLhs = 0d;
                int lhsFrameRow = -12345;

                // ------------------------------------------------------------------------------
                // Loop over VARIABLES: these variables sum to 0 for the "d" and "dAlternative" types
                // ------------------------------------------------------------------------------

                FrameLightRow frameRowLhs = null;

                foreach (string fullVariableName in orderedNames)
                {
                    ChopFullVariableName chop = ChopFullVariableName(lhs2, fullVariableName);

                    double dLevel = double.NaN;
                    double dLevelLag = double.NaN;
                    double dLevelLag2 = double.NaN;
                    double dLevelRef = double.NaN;
                    double dLevelRefLag = double.NaN;
                    double dLevelRefLag2 = double.NaN;

                    FrameLightRow frameRow = new FrameLightRow(frame);
                    if (frameRowLhs == null) frameRowLhs = frameRow;  //The first row for this period (the row is a LHS variable).
                    prime = G.NextPrime(prime);  //first time: 1019
                    if (!chop.isLhs) primeSumWithoutLhs += prime;

                    if (IsDecompResidualName(fullVariableName))
                    {
                        Tuple<Series, Series> tup = GetRealTimeseries(decompDatas, fullVariableName);
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
                        //Maybe turn this off for x-type...
                        //a little bit of waste here, if not both series are needed for non-x decomp. But penalty must be really small.

                        string fullNameRef = G.Chop_SetBank(chop.fullName, "Ref");

                        if (op.isRaw)
                        {
                            Series tsFirst = O.GetIVariableFromString(chop.fullName, O.ECreatePossibilities.NoneReturnNullAlways) as Series; //#overview
                            if (tsFirst != null)
                            {
                                dLevel = tsFirst.GetDataSimple(t2.Add(chop.iLag));
                                dLevelLag = tsFirst.GetDataSimple(t2.Add(-1 + chop.iLag));
                                dLevelLag2 = tsFirst.GetDataSimple(t2.Add(-2 + chop.iLag));
                                if (G.DecompShouldHandleMissings(decompOptions2.missingAsZero, false) == ESeriesMissing.Zero) 
                                {
                                    if (G.IsNumericalError(dLevel)) dLevel = 0d;
                                    if (G.IsNumericalError(dLevelLag)) dLevelLag = 0d;
                                    if (G.IsNumericalError(dLevelLag2)) dLevelLag2 = 0d;                                    
                                }
                            }
                            Series tsRef = O.GetIVariableFromString(fullNameRef, O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                            if (tsRef != null)
                            {
                                dLevelRef = tsRef.GetDataSimple(t2.Add(chop.iLag));
                                dLevelRefLag = tsRef.GetDataSimple(t2.Add(-1 + chop.iLag));
                                dLevelRefLag2 = tsRef.GetDataSimple(t2.Add(-2 + chop.iLag));
                                if (G.DecompShouldHandleMissings(decompOptions2.missingAsZero, false) == ESeriesMissing.Zero)
                                {
                                    if (G.IsNumericalError(dLevelRef)) dLevelRef = 0d;
                                    if (G.IsNumericalError(dLevelRefLag)) dLevelRefLag = 0d;
                                    if (G.IsNumericalError(dLevelRefLag2)) dLevelRefLag2 = 0d;
                                }
                            }
                        }
                        else
                        {
                            if (operatorOneOf3Types == EContribType.N || operatorOneOf3Types == EContribType.M || operatorOneOf3Types == EContribType.D)
                            {
                                Series tsFirst = null;
                                tsFirst = O.GetIVariableFromString(chop.fullName, O.ECreatePossibilities.NoneReturnNullAlways) as Series; //#overview
                                bool isMissingResVariable = false;
                                if (tsFirst == null)
                                {
                                    if (Program.options.decomp_res_missing == ESeriesMissing.Zero && G.StartsWith(chop.varName, Globals.decompResidualPrefix))
                                    {
                                        isMissingResVariable = true;
                                    }
                                    else if (G.DecompShouldHandleMissings(decompOptions2.missingAsZero, true) == ESeriesMissing.Zero)
                                    {
                                        tsFirst = DecompCreateArtificialSeries(model.modelGamsScalar, 0d);
                                    }
                                    else
                                    {
                                        tsFirst = DecompCreateArtificialSeries(model.modelGamsScalar, double.NaN);
                                    }
                                }
                                if (!isMissingResVariable)
                                {
                                    dLevel = tsFirst.GetDataSimple(t2.Add(chop.iLag));
                                    dLevelLag = tsFirst.GetDataSimple(t2.Add(-1 + chop.iLag));
                                    dLevelLag2 = tsFirst.GetDataSimple(t2.Add(-2 + chop.iLag));
                                }
                                if (isMissingResVariable || G.DecompShouldHandleMissings(decompOptions2.missingAsZero, false) == ESeriesMissing.Zero)
                                {
                                    if (G.IsNumericalError(dLevel)) dLevel = 0d;
                                    if (G.IsNumericalError(dLevelLag)) dLevelLag = 0d;
                                    if (G.IsNumericalError(dLevelLag2)) dLevelLag2 = 0d;
                                }
                            }

                            if (operatorOneOf3Types == EContribType.RN || operatorOneOf3Types == EContribType.M || operatorOneOf3Types == EContribType.RD)
                            {
                                Series tsRef = null;
                                tsRef = O.GetIVariableFromString(fullNameRef, O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                                bool missingResVariable = false;
                                if (tsRef == null)
                                {
                                    if (Program.options.decomp_res_missing == ESeriesMissing.Zero && G.StartsWith(chop.varName, Globals.decompResidualPrefix))  //fullNameRef is made from chop anyways.
                                    {
                                        missingResVariable = true;
                                    }                                    
                                    else if (G.DecompShouldHandleMissings(decompOptions2.missingAsZero, true) == ESeriesMissing.Zero)
                                    {
                                        tsRef = DecompCreateArtificialSeries(model.modelGamsScalar, 0d);
                                    }
                                    else
                                    {
                                        tsRef = DecompCreateArtificialSeries(model.modelGamsScalar, double.NaN);
                                    }
                                }
                                if (!missingResVariable)
                                {
                                    dLevelRef = tsRef.GetDataSimple(t2.Add(chop.iLag));
                                    dLevelRefLag = tsRef.GetDataSimple(t2.Add(-1 + chop.iLag));
                                    dLevelRefLag2 = tsRef.GetDataSimple(t2.Add(-2 + chop.iLag));
                                }
                                if (missingResVariable || G.DecompShouldHandleMissings(decompOptions2.missingAsZero, false) == ESeriesMissing.Zero)
                                {
                                    if (G.IsNumericalError(dLevelRef)) dLevelRef = 0d;
                                    if (G.IsNumericalError(dLevelRefLag)) dLevelRefLag = 0d;
                                    if (G.IsNumericalError(dLevelRefLag2)) dLevelRefLag2 = 0d;
                                }
                            }
                        }
                    }

                    double d = double.NaN;
                    double dAlternative = double.NaN;
                    if (op.isDoubleDifQuo)  //dp
                    {
                        d = DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "d", smpl, lhs, t2, fullVariableName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        dAlternative = DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "d", smpl, lhs, t2.Add(-1), fullVariableName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                    }
                    else if (op.isDoubleDifRef) //rdp
                    {
                        d = DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "rd", smpl, lhs, t2, fullVariableName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        dAlternative = DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "rd", smpl, lhs, t2.Add(-1), fullVariableName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                    }
                    else if (op.lowLevel == ELowLevel.BothQuoAndRef) //mp
                    {
                        d = DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "d", smpl, lhs, t2, fullVariableName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        dAlternative = DecomposePutIntoTable2HelperOperators(decompDataMAINClone, "rd", smpl, lhs, t2, fullVariableName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                    }
                    else
                    {
                        d = DecomposePutIntoTable2HelperOperators(decompDataMAINClone, op.OperatorLower(), smpl, lhs, t2, fullVariableName, model.DecompType() == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        dAlternative = double.NaN;
                    }

                    //string dictName2 = fullVariableName.Replace(DecompFirst() + ":", "").Replace("¤[0]", "");
                    string dictName2 = fullVariableName.Replace(DecompFirst() + ":", "");

                    frameRow.AddDimension(frame, Globals.col_t, new CellLight(t2.ToString()));
                    frameRow.AddDimension(frame, Globals.col_variable, new CellLight(chop.varName));
                    frameRow.AddDimension(frame, Globals.col_lag, new CellLight(chop.lag));

                    //Clean this up sometime, so we do not have gekkodim_x1&1 for a dimension, but just gekkodim_1, and
                    //all the vars share this gekkodim_1. Problem is the pivot selector, etc.
                    //Maybe have fixed positions for variable, lags, time, lhs, etc., and then a integer column with n "dims",
                    //follwed by the n dimension values, then a integer column withm  "sets", followed by the m set values, 
                    //then followed m integers showing which dimension number the set is for.
                    //All these could have almost-fixed positions, given then "dims" and "sets" integers.
                    //For now we use names (dictionary lookup), and gekkodim_x1&1 is essentially superfluous because
                    //it could be calculated from the cols following "dims".
                    //

                    //     dims     x1-dim-1     x1-dim-2     dim-1     dim-2      #i     #j     #i     #j
                    //     3        a            m            a         m          a      m      1      2
                    //

                    int dims = 0;
                    if (chop.indexes != null) dims = chop.indexes.Length;
                    //dims = 2
                    frameRow.AddDimension(frame, Globals.decompDimension2, new CellLight(dims));
                    if (dims > 0)
                    {
                        for (int ii = 0; ii < chop.indexes.Length; ii++)
                        {
                            string index = chop.indexes[ii];
                            //x1 dim 1 = "a", variable specific
                            frameRow.AddDimension(frame, chop.varName + Globals.decompDimension + (ii + 1), new CellLight(index));
                            // dim 1 = "a", common for all variables
                            frameRow.AddDimension(frame, "" + Globals.decompDimension + (ii + 1), new CellLight(index));  //Note: keep "" !!

                            if (chop.domains != null)
                            {
                                //#i = "a"
                                //¤i = 1, what dimension number does #i have?
                                string domain = chop.domains[ii];  //has corresponding dimensions                              
                                if (domain == null || domain == Globals.dimensionWithoutDomain)
                                {
                                    frameRow.AddDimension(frame, Globals.decompUniversal, new CellLight(index));
                                    frameRow.AddDimension(frame, Globals.decompUniversal.Replace('#', Globals.decompSetDimNumberChar), new CellLight(ii + 1));
                                }
                                else
                                {
                                    frameRow.AddDimension(frame, domain, new CellLight(index));
                                    frameRow.AddDimension(frame, domain.Replace('#', Globals.decompSetDimNumberChar), new CellLight(ii + 1));
                                }
                            }
                        }
                    }

                    if (chop.isLhs)
                    {
                        frameRow.AddDimension(frame, Globals.col_lhs, new CellLight(Globals.pivotHelper2New));
                    }

                    if (decompOptions2.expand)
                    {
                        frameRow.AddDimension(frame, Globals.col_expand, new CellLight(FullVariableNamePretty(fullVariableName, false).Replace("Work:", "")));
                    }

                    frameRow.AddValue(frame, Globals.col_value, new CellLight(d));
                    frameRow.AddValue(frame, Globals.col_valueAlternative, new CellLight(dAlternative));
                    frameRow.AddValue(frame, Globals.col_valueLevel, new CellLight(dLevel));
                    frameRow.AddValue(frame, Globals.col_valueLevelLag, new CellLight(dLevelLag));
                    frameRow.AddValue(frame, Globals.col_valueLevelLag2, new CellLight(dLevelLag2));
                    frameRow.AddValue(frame, Globals.col_valueLevelRef, new CellLight(dLevelRef));
                    frameRow.AddValue(frame, Globals.col_valueLevelRefLag, new CellLight(dLevelRefLag));
                    frameRow.AddValue(frame, Globals.col_valueLevelRefLag2, new CellLight(dLevelRefLag2));
                    frameRow.AddValue(frame, Globals.col_fullVariableName, new CellLight(dictName2));
                    frameRow.AddValue(frame, Globals.col_prime, new CellLight(prime));
                    // -----                    
                    frameRow.AddValue(frame, Globals.col_firstValueLevelLag, new CellLight(frameRowLhs.GetValue(frame, Globals.col_valueLevelLag).data));
                    frameRow.AddValue(frame, Globals.col_firstValueLevelLag2, new CellLight(frameRowLhs.GetValue(frame, Globals.col_valueLevelLag2).data));
                    frameRow.AddValue(frame, Globals.col_firstValueLevelRef, new CellLight(frameRowLhs.GetValue(frame, Globals.col_valueLevelRef).data));
                    frameRow.AddValue(frame, Globals.col_firstValueLevelRefLag, new CellLight(frameRowLhs.GetValue(frame, Globals.col_valueLevelRefLag).data));
                    frameRow.AddValue(frame, Globals.col_firstValueLevelRefLag2, new CellLight(frameRowLhs.GetValue(frame, Globals.col_valueLevelRefLag2).data));
                    // -----
                    frame.data.Add(frameRow);
                }

                if (frameRowLhs != null) frameRowLhs.AddValue(frame, Globals.col_prime, new CellLight(-primeSumWithoutLhs)); //For each period, the LHS row gets the sum of all the other row's primes.
            }

            //Fill out any "holes" in the dataframe columns
            int maxDimension = 0;
            int maxValue = 0;
            foreach (FrameLightRow frameRow in frame.data)
            {
                maxDimension = Math.Max(maxDimension, frameRow.storageDimensions.Count);
                maxValue = Math.Max(maxDimension, frameRow.storageValues.Count);  //probably never a problem with these, mostly for dimensions
            }
            foreach (FrameLightRow frameRow in frame.data)
            {
                int i1 = frameRow.storageDimensions.Count;
                int i2 = frameRow.storageValues.Count;
                for (int i = 0; i < maxDimension - i1; i++) frameRow.storageDimensions.Add(new CellLight());
                for (int i = 0; i < maxValue - i2; i++) frameRow.storageValues.Add(new CellLight());
            }

            if (decompOptions2.groupAge)
            {
                int a1 = 20; int a2 = 67; int aMax = 102;
                if (decompOptions2.expand)
                {
                    //uses "expand" column
                    int ie = frame.frameDimensionNames[Globals.col_expand];

                    bool isAge1 = false;
                    bool isAge2 = false;
                    bool isAge3 = false;
                    int[] hits1 = new int[aMax];
                    int[] hits2 = new int[aMax];
                    int[] hits3 = new int[aMax];
                    foreach (FrameLightRow frameRow in frame.data)
                    {
                        string varname = frameRow.storageDimensions[ie].text;
                        //we do not use O.Chop(), Unchop() because it is just a hack
                        int i1 = varname.IndexOf('[');
                        int i2 = varname.IndexOf(']');
                        string x = G.Substring(varname, i1 + 1, i2 - 1);
                        string[] ss = x.Split(',');
                        int cnt = 0;
                        foreach (string s in ss)
                        {
                            cnt++;
                            if (cnt == 1) AgeHelper1(s, a1, a2, aMax, hits1);
                            else if (cnt == 2) AgeHelper1(s, a1, a2, aMax, hits2);
                            else if (cnt == 3) AgeHelper1(s, a1, a2, aMax, hits3);
                        }
                    }

                    isAge1 = AgeHelper2(a1, a2, hits1);
                    isAge2 = AgeHelper2(a1, a2, hits2);
                    isAge3 = AgeHelper2(a1, a2, hits3);

                    if (isAge1 || isAge2 || isAge3)
                    {
                        foreach (FrameLightRow frameRow in frame.data)
                        {
                            string varname = frameRow.storageDimensions[ie].text;
                            int i1 = varname.IndexOf('[');
                            int i2 = varname.IndexOf(']');
                            string x = G.Substring(varname, i1 + 1, i2 - 1);
                            string[] ss = x.Split(',');
                            string sIndex = null;
                            int cnt = 0;
                            foreach (string s in ss)
                            {
                                cnt++;
                                if ((cnt == 1 && isAge1) || (cnt == 2 && isAge2) || (cnt == 3 && isAge3))
                                {
                                    sIndex += AgeIntervals(s) + ", ";
                                }
                                else
                                {
                                    sIndex += s + ", ";
                                }
                            }
                            sIndex = G.Substring(sIndex, 0, sIndex.Length - 1 - ", ".Length);
                            string s2 = G.Substring(varname, 0, i1) + sIndex + G.Substring(varname, i2, varname.Length - 1);
                            frameRow.storageDimensions[ie] = new CellLight(s2);
                        }
                    }

                    foreach (KeyValuePair<string, int> kvp in frame.frameDimensionNames)
                    {
                        bool isAge = false;
                        if ((kvp.Key.Contains("#") && !kvp.Key.Contains(Globals.decompSetDimNumberChar)))
                        {
                            int[] hits = new int[aMax];
                            foreach (FrameLightRow frameRow in frame.data)
                            {
                                string s = frameRow.storageDimensions[kvp.Value].text;
                                AgeHelper1(s, a1, a2, aMax, hits);
                            }
                            isAge = AgeHelper2(a1, a2, hits);

                            if (isAge)
                            {
                                foreach (FrameLightRow frameRow in frame.data)
                                {
                                    CellLight c = frameRow.storageDimensions[kvp.Value];
                                    frameRow.storageDimensions[kvp.Value] = new CellLight(AgeIntervals(c.text));
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, int> kvp in frame.frameDimensionNames)
                    {
                        bool isAge = false;
                        if ((kvp.Key.Contains("#") && !kvp.Key.Contains(Globals.decompSetDimNumberChar)) || (decompOptions2.expand && kvp.Key == "expand"))
                        {
                            int[] hits = new int[aMax];
                            foreach (FrameLightRow frameRow in frame.data)
                            {
                                string s = frameRow.storageDimensions[kvp.Value].text;
                                AgeHelper1(s, a1, a2, aMax, hits);
                            }
                            isAge = AgeHelper2(a1, a2, hits);

                            if (isAge)
                            {
                                foreach (FrameLightRow frameRow in frame.data)
                                {
                                    string s = frameRow.storageDimensions[kvp.Value].text;
                                    int i = -12345;
                                    if (int.TryParse(s, out i))
                                    {
                                        CellLight c = frameRow.storageDimensions[kvp.Value];
                                        frameRow.storageDimensions[kvp.Value] = new CellLight(AgeIntervals(c.text));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return frame;
        }

        /// <summary>
        /// Creates an artifial helper series (with null name), from the scalar model start to end (with 20 periods added at each end for safety)
        /// </summary>
        /// <param name="modelGamsScalar"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static Series DecompCreateArtificialSeries(ModelGamsScalar modelGamsScalar, double d)
        {
            //Not sure if timeless series would work here
            Series tsRef = new Series(modelGamsScalar.absoluteT1.freq, null);
            foreach (GekkoTime t in new GekkoTimeIterator(modelGamsScalar.absoluteT1.Add(-Globals.decompExtraPeriods), modelGamsScalar.absoluteT2.Add(Globals.decompLagAddition)))
            {
                tsRef.SetData(t, d);
            }

            return tsRef;
        }

        private static bool AgeHelper2(int a1, int a2, int[] hits)
        {
            bool isAge = false;
            int span = 0;
            foreach (int i in hits) span += Math.Min(i, 1);
            double ratio = (double)span / (a2 - a1 + 1);
            if (ratio > 0.80) isAge = true;  //over 80% of ages 20-67 are represented
            return isAge;
        }

        private static void AgeHelper1(string s, int a1, int a2, int aMax, int[] hits)
        {
            bool ok = false;
            int i = -12345;
            if (int.TryParse(s, out i))
            {
                if (i >= 0 && i <= aMax - 1)
                {
                    ok = true;
                    if (i >= a1 && i <= a2) hits[i]++;
                }
            }
        }

        /// <summary>
        /// Helper method chop up the key from the DecompDict (containing decomp results)
        /// </summary>
        /// <param name="lhs2"></param>
        /// <param name="dictName"></param>
        /// <param name="varName"></param>
        /// <param name="indexes"></param>
        /// <param name="fullName"></param>
        /// <param name="lag"></param>
        /// <param name="iLag"></param>
        /// <param name="isLhs"></param>
        /// <param name="domains"></param>
        public static ChopFullVariableName ChopFullVariableName(string lhs2, string dictName)
        {
            ChopFullVariableName chop = new ChopFullVariableName();
            string[] ss = dictName.Split('¤');
            chop.fullName = ss[0];
            chop.lag = ss[1];
            chop.iLag = int.Parse(G.Substring(chop.lag, 1, chop.lag.Length - 2));
            string dbName, freq;
            O.Chop(chop.fullName, out dbName, out chop.varName, out freq, out chop.indexes);
            chop.isLhs = false;
            if (chop.iLag == 0 && G.Equal(G.HandleBlanksRemove(G.Chop_RemoveBank(chop.fullName)), lhs2)) chop.isLhs = true;
            chop.domains = DecompPivotGetDomains(chop.fullName, chop.indexes);
            return chop;
        }

        private static DecompDict DecompPivotCreateDataframeGetDict(DecompData decompDataMAINClone, DecompOperator op, EContribType operatorOneOf3Types)
        {
            DecompDict dd = null;
            if (op.isRaw)
            {
                //data is not used from here, it is just to get the list of
                //relevant variables. For multiplier type, both if's are true,
                //and in that case we just use the first.
                dd = decompDataMAINClone.cellsQuo;
                if (op.lowLevel == ELowLevel.OnlyRef) dd = decompDataMAINClone.cellsRef;
            }
            else
            {
                if (op.lowLevel == ELowLevel.BothQuoAndRef)
                {
                    dd = GetDecompDatas(decompDataMAINClone, EContribType.D);  //could just as well be .RD, we are only using the keys
                }
                else
                {
                    dd = GetDecompDatas(decompDataMAINClone, operatorOneOf3Types);
                }
            }

            return dd;
        }

        /// <summary>
        /// Gets domain names like ["#i", "*", "#j]
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="indexes"></param>
        /// <returns></returns>
        private static string[] DecompPivotGetDomains(string fullName, string[] indexes)
        {
            string[] domains = null;
            if (indexes != null)
            {
                domains = new string[indexes.Length];
                for (int i = 0; i < domains.Length; i++) domains[i] = Globals.decompUniversal;
            }
            if (domains != null)
            {
                //Adding domain info. We may have x[18, gov] which is part of x[#a, #sector].
                //So in this case, #a and #sector would be added as columns
                IVariable iv = O.GetIVariableFromString(fullName, O.ECreatePossibilities.NoneReturnNullAlways); //#overview
                if (iv != null)
                {
                    Series ts = iv as Series;
                    if (ts?.mmi?.parent?.meta?.domains != null)
                    {
                        for (int ii = 0; ii < ts.mmi.parent.meta.domains.Length; ii++)
                        {
                            domains[ii] = ConvertSetname(ts.mmi.parent.meta.domains[ii]);
                        }
                    }
                }
            }

            return domains;
        }



        /// <summary>
        /// Sorting and pruning. Uses .value_hack of each cell, which stores value no matter what is shown in cell.
        /// At the end, if decompOptions2.useBracketNames == true, any "[0]" is also removed, and for bracket-names
        /// singleton sets x[*] are shown with their real varname x[tot].
        /// </summary>
        /// <param name="table1"></param>
        /// <param name="decompOptions2"></param>
        private static DecompOutput DecompTableHandleSortAndIgnoreAndErrors(Table table1, DecompOptions2 decompOptions2, Model model)
        {
            string numberFormat = GetNumberFormat(decompOptions2);

            ERowsCols rowsOrCols = VariablesOnRowsOrCols(decompOptions2);
            if (rowsOrCols == ERowsCols.None) return new DecompOutput(table1, null, null, null); //fast return 

            string ignoredText = null;
            List<double> red = new List<double>();
            List<List<string>> black = new List<List<string>>();

            // --------------------------------
            // SORT AND IGNORE START
            // --------------------------------

            List<SortHelper> sortHelperStart = new List<SortHelper>();

            if (rowsOrCols == ERowsCols.Rows)
            {
                for (int i = 3; i <= table1.GetRowMaxNumber(); i++)  //ignore first 2 rows
                {
                    Cell c5 = table1.Get(i, 2);
                    //string name2 = c5?.vars_hack?[0];
                    string name2 = GetVarsHack(c5);
                    double max = 0d;
                    for (int j = 2; j <= table1.GetColMaxNumber(); j++)
                    {
                        Cell c1 = table1.Get(i, j);
                        Cell c2 = table1.Get(2, j);
                        double d = 0d;
                        if (decompOptions2.decompOperator.isRaw) d = Math.Abs(c1.value_hack);
                        else d = Math.Abs(c1.value_hack / c2.value_hack * 100d);
                        if (!G.IsNumericalError(d)) max = Math.Max(max, d);
                        if (IsDecompResidualName(name2)) c1.backgroundColor = "LightYellow";
                    }
                    sortHelperStart.Add(new SortHelper() { position = i, value = max, name = name2 });
                }
            }
            else if (rowsOrCols == ERowsCols.Cols)
            {
                for (int j = 3; j <= table1.GetColMaxNumber(); j++)  //ignore first two cols                 
                {
                    Cell c5 = table1.Get(2, j);
                    //string name2 = c5?.vars_hack?[0];
                    string name2 = GetVarsHack(c5);
                    if (IsDecompResidualName(name2)) c5.backgroundColor = "LightYellow";
                    double max = 0d;
                    for (int i = 2; i <= table1.GetRowMaxNumber(); i++)
                    {
                        Cell c1 = table1.Get(i, j);
                        Cell c2 = table1.Get(i, 2);
                        double d = 0d;
                        if (decompOptions2.decompOperator.isRaw) d = Math.Abs(c1.value_hack);
                        else d = Math.Abs(c1.value_hack / c2.value_hack * 100d);
                        if (!G.IsNumericalError(d)) max = Math.Max(max, d);
                        if (IsDecompResidualName(name2)) c1.backgroundColor = "LightYellow";
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
                if (rowsOrCols == ERowsCols.Cols) x = "col" + G.S(ignoreCount);
                ignoredText = ignoreCount + " " + x + " ignored";
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
            if (rowsOrCols == ERowsCols.Rows)
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
            else if (rowsOrCols == ERowsCols.Cols)
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

                if (rowsOrCols == ERowsCols.Rows)
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
                else if (rowsOrCols == ERowsCols.Cols)
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

            if (false) //This has given som problem, so it is now (february 2026 switched off).
            {
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
                                if (xx != null)
                                {
                                    foreach (string s in xx)
                                    {
                                        int a = model.modelGamsScalar.dict_FromVarNameToANumber.GetInt(s);
                                        if (a == -12345) continue;

                                        bool b1 = decompOptions2.decompOperator.lowLevel == ELowLevel.OnlyQuo || decompOptions2.decompOperator.lowLevel == ELowLevel.BothQuoAndRef || decompOptions2.decompOperator.lowLevel == ELowLevel.Multiplier;
                                        bool b2 = decompOptions2.decompOperator.lowLevel == ELowLevel.OnlyRef || decompOptions2.decompOperator.lowLevel == ELowLevel.BothQuoAndRef || decompOptions2.decompOperator.lowLevel == ELowLevel.Multiplier;

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
            }

            // ------------------------------------------
            // ERRORS
            // ------------------------------------------

            //Set error row/column, as a sum of rows 2 and on. Also sets count/names on that row/col.
            if (decompOptions2.showErrors && !decompOptions2.decompOperator.isRaw)
            {
                int rowmax = table2.GetRowMaxNumber();  //because it changes dynamically later on
                int colmax = table2.GetColMaxNumber();  //because it changes dynamically later on
                if (rowsOrCols == ERowsCols.Rows)
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
                else if (rowsOrCols == ERowsCols.Cols)
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

            // --------------------------------------------------------------------------------------------
            // Calculate yellow/orange/red lamps + black arrows as the very last step before rewriting with bracket names
            // --------------------------------------------------------------------------------------------

            if (rowsOrCols == ERowsCols.Rows)
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
                    else if (target == 0d || double.IsNaN(target)) error = Globals.redNaN; //just some large number
                    red.Add(error);  //one for each period
                }
            }
            else if (rowsOrCols == ERowsCols.Cols)
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

            // ---------------- black, collapse arrows -----------------------

            if (rowsOrCols == ERowsCols.Rows)
            {
                for (int i = 2; i <= table2.GetRowMaxNumber(); i++)
                {
                    Cell c = table2.Get(i, 2);
                    if (c == null) black.Add(new List<string>());
                    else
                    {
                        black.Add(c.vars_hack);  //We mostly use the count though
                    }
                }
            }
            else if (rowsOrCols == ERowsCols.Cols)
            {
                for (int j = 2; j <= table2.GetColMaxNumber(); j++)
                {
                    Cell c = table2.Get(2, j);
                    if (c == null) black.Add(new List<string>());
                    else
                    {
                        black.Add(c.vars_hack);  //We mostly use the count though
                    }
                }
            }

            // ==================================================================================
            // For bracket names, remove "[0]". Also, for singleton sets #a_ = ['tot'] we take
            // the real variable, for instance x[tot].
            // ==================================================================================

            if (decompOptions2.useBracketNames || decompOptions2.expand)
            {
                if (rowsOrCols == ERowsCols.Rows)  //will never be null, if so it will have been returned above
                {
                    for (int i = 2; i <= table2.GetRowMaxNumber(); i++)
                    {
                        Cell c1 = table2.Get(i, 1);
                        Cell c2 = table2.Get(i, 2);
                        string s = c1.CellText.TextData[0];
                        if (Globals.decompShowSingletonSet && !decompOptions2.expand && c2.vars_hack != null) s = ReplaceStars(c2.vars_hack, s);
                        s = TrimAndRemoveLag0(s);
                        c1.CellText.TextData = new List<string> { s };
                    }
                }
                else if (rowsOrCols == ERowsCols.Cols)  //will never be null, if so it will have been returned above
                {
                    for (int j = 2; j <= table2.GetColMaxNumber(); j++)
                    {
                        Cell c1 = table2.Get(1, j);
                        Cell c2 = table2.Get(2, j);
                        string s = c1.CellText.TextData[0];
                        if (Globals.decompShowSingletonSet && !decompOptions2.expand && c2.vars_hack != null && c2.vars_hack.Count == 1) s = ReplaceStars(c2.vars_hack, s);
                        s = TrimAndRemoveLag0(s);
                        c1.CellText.TextData = new List<string> { s };
                    }
                }
            }

            DecompOutput decompOutput = new DecompOutput(table2, ignoredText, red, black);
            return decompOutput;
        }

        /// <summary>
        /// For a variable like "x[a, *, m] with one or more "*", it will loook into the vars_hack vars to see if the second dimension varies or not.
        /// If it does not vary, the concrete non-varying element is set instead of the "*".
        /// </summary>
        /// <param name="vars_hack"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string ReplaceStars(List<string> vars_hack, string s)
        {
            VariableDims variableDims = new VariableDims();

            string name = null;
            foreach (string var in vars_hack)
            {
                string s2 = var.Split('¤')[0];
                string input2, dbName, varName, freq; string[] indexes;
                O.Chop(s2, out dbName, out varName, out freq, out indexes);
                if (name == null) name = varName;
                Dims dims = new Dims();
                if (indexes != null)
                {
                    foreach (string index in indexes) dims.storage.Add(index);
                }
                variableDims.storage.Add(dims);
            }

            GekkoDictionary<string, bool>[] span = GamsModel.GetIndexesFromScalarEquations(variableDims);

            //
            // Inject this instead of a possible '*'
            //

            StringBuilder sb = new StringBuilder();
            bool bracket = false;
            int commaCounter = 0;
            bool hit = false;
            foreach (char c in s)
            {
                bool addChar = true;
                if (c == '[')
                {
                    bracket = true;
                }
                else if (c == ',')
                {
                    commaCounter++;
                }
                else if (c == '*')
                {
                    try
                    {
                        if (bracket && span[commaCounter].Count == 1)
                        {
                            sb.Append(span[commaCounter].First().Key);
                            hit = true;
                            addChar = false;
                        }
                    }
                    catch { }
                }
                if (addChar) sb.Append(c);
            }

            if (hit) s = sb.ToString();
            return s;
        }

        /// <summary>
        /// Converts something like "npop[0][0]" into "npop[0]", only removing [0] if it is last (after trimming).
        /// If lags are known to be always last, this is safe. Beware that "npop[0]¤[0]" becomes "npop[0]¤".
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimAndRemoveLag0(string s)
        {
            if (s == null) return s;
            s = s.Trim();
            if (s.EndsWith(Globals.decompNoLag)) s = G.Substring(s, 0, s.Length - 1 - Globals.decompNoLag.Length).Trim();
            return s;
        }

        /// <summary>
        /// May return null!
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetVarsHack(Cell c)
        {
            string name2 = null; if (c != null && c.vars_hack != null && c.vars_hack.Count > 0) name2 = c.vars_hack[0];
            return name2;
        }



        /// <summary>
        /// At this point, decomp rows sum to 0, so we change the sign of the first row, so the rest sum
        /// to the first. Also, percentages can be set, so first row is 100%. Also works for columns.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="decompOptions2"></param>
        private static void DecompTableHandleSignAndShares(Table tab, DecompOptions2 decompOptions2)
        {
            ERowsCols rowsOrCols = VariablesOnRowsOrCols(decompOptions2);

            //
            // SIGN AND SHARES
            //
            //change sign on row/col 2 (dependent var), and calcuate share values for rows/cols 2 and on.
            //no adding of rows/columns.
            if (!decompOptions2.decompOperator.isRaw)
            {
                string formatSShares = "f16." + decompOptions2.decimalsPch;
                if (decompOptions2.count == ECountType.N || decompOptions2.count == ECountType.Names) return;

                //int w = 0;

                if (rowsOrCols == ERowsCols.Rows)
                {
                    for (int j = 2; j <= tab.GetColMaxNumber(); j++)
                    {
                        Cell cFirst = tab.Get(2, j);
                        if (cFirst == null)
                        {
                            //Cannot be
                        }
                        else
                        {
                            double value = cFirst.number;
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
                }
                else if (rowsOrCols == ERowsCols.Cols)
                {
                    for (int i = 2; i <= tab.GetRowMaxNumber(); i++)
                    {
                        Cell cFirst = tab.Get(i, 2);
                        if (cFirst == null)
                        {
                            //Cannot be
                        }
                        else
                        {
                            double value = cFirst.number;
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
        /// Checks if rows or cols (or in principle both, even if this is maybe not possible) may sum up
        /// in the decomp/differentiation sense. Uses prime number fractions, so aggregation over periods
        /// may still be summable.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="rowsSumUp"></param>
        /// <param name="colsSumUp"></param>
        private static Tuple<bool, bool> PrimeRowsOrColsAddUp(Table tab)
        {
            bool rowsSumUp = true;
            for (int j = 2; j <= tab.GetColMaxNumber(); j++)
            {
                double primeSum = 0d;
                int count = 0;
                for (int i = 2; i <= tab.GetRowMaxNumber(); i++)
                {
                    count++;
                    double prime = tab.Get(i, j).prime_hack;
                    primeSum += prime;
                }
                bool match = IsPrimeMatch(primeSum);
                if (!match)
                {
                    rowsSumUp = false;
                    break;
                }
            }

            bool colsSumUp = true;
            for (int i = 2; i <= tab.GetRowMaxNumber(); i++)
            {
                double primeSum = 0d;
                int count = 0;
                for (int j = 2; j <= tab.GetColMaxNumber(); j++)
                {
                    count++;
                    double prime = tab.Get(i, j).prime_hack;
                    primeSum += prime;
                }

                bool match = IsPrimeMatch(primeSum);
                if (!match)
                {
                    colsSumUp = false;
                    break;
                }
            }

            return new Tuple<bool, bool>(rowsSumUp, colsSumUp);

            bool IsPrimeMatch(double rowPrimeSum)
            {
                if (Math.Abs(rowPrimeSum) < 0.01d) return true;
                return false;
            }
        }


        /// <summary>
        /// Set back to vars and lags on rows, and time on cols
        /// </summary>
        /// <param name="decompOptions2"></param>
        public static void ResetRowsColsSelection(DecompOptions2 decompOptions2)
        {
            decompOptions2.rows = new List<string>() { "vars", "lags" };
            decompOptions2.cols = new List<string>() { "time" };
        }


        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO -----------------------------------------------------------------------------
        // TODO TODO why seach for these ref/quo timeseries, why not just have them in 1 dictionary?
        // TODO TODO for unrolled eqs, this searching may take time.
        // TODO TODO -----------------------------------------------------------------------------
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        /// <summary>
        /// The decomp provides a linearization where the contributions sum to 0.
        /// Here, this is "translated" into the normal decomp way of showing it.
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="parentI"></param>
        /// <param name="decompDatasSupremeClone"></param>
        /// <param name="operatorOneOf3Types"></param>
        public static void DecompAdjust(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, int parentI, DecompData decompDatasSupremeClone, DecompDatas decompDatas, EContribType operatorOneOf3Types, ENormalizeType normalize, DecompOperator op)
        {
            // Decomp provides a linearization where the contributions sum to 0. Here we identify those
            // vars (contributions) that are moved to the LHS.
            //
            //Decomp decomposes into "atoms", like x[a], x[b], x[a][-1], x[b][+1], etc.
            //Regarding the LHS of decompose, this can only meaningfully be an atom, so it makes no
            //particular sense to aggregate for instance x[a] and x[a][-1], or x[a] and x[b]. Regarding
            //the latter, these may no even have the same units!
            //Consider the equation x[a] + 500 * x[b] = 1000 * x[a][-1]/x[b][+1]. 
            //How would you meaningfully aggregate into x[a] and x[b] on LHS? If we are only decomposing
            //x[a], we can look at how x[a] - @x[a] can be decomposed, which is easy enough. But 
            //decomposing x[a] and x[b] at the same time on LHS? This would only make sense if the derivatives
            //of x[a] and x[b] are equal, for instance x[a] + x[b] = 1000 * x[a][-1]/x[b][+1]. Then we could
            //state the decomposition of x[a] + x[b] - (@x[a] + @x[b]).
            //If the user wants this, he should add a real equation x = x[a] + x[b] to the system, and then
            //decompose x, choosing if x[a] or x[b] is considered endogenous. 
            //Gekko could make an easy interface regarding that. If the equation really is 
            //x[a] + x[b] = 1000 * x[a][-1]/x[b][+1], the choice of x[a] or x[b] as endogenous will not matter.
            //Because of all this, we cannot decompose x[a] + 500 * x[b] = 1000 * x[a][-1]/x[b][+1] with
            //x[a] on the RHS, excluding lags. If we exclude lags, we will get x[a][0] on the LHS, 
            //and x[a][-1] and x[b] on the LHS, where lags only disappear regarding x[b].
            //
            //This is similar to ADAM-style xa = -500 * xb + 1000 * xa[-1]/xb[+1]. Here, aggregating the RHS
            //lags would only aggregate xb and xb[+1], not xa and xa[-1].

            DecompData d = decompDatasSupremeClone;
            string name = decompOptions2.link[parentI].varnames;
            d.lhs = DecompFirst() + ":" + ConvertToTurtleName(name, 0);  //lag = 0

            if (Program.options.bugfix_decomp_jacobi)
            {
                if (!decompOptions2.decompOperator.isRaw)
                {
                    foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(d, operatorOneOf3Types).storage)
                    {
                        foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                        {
                            kvp.Value.SetData(t, -kvp.Value.GetDataSimple(t));
                        }
                    }
                }
            }
            else
            {
                if (!decompOptions2.decompOperator.isRaw)
                {
                    Series lhs2 = GetDecompDatas(decompDatasSupremeClone, operatorOneOf3Types)[d.lhs];

                    Tuple<Series, Series> tsTuple = GetRealTimeseries(decompDatas, d.lhs);  //May contain null's, at least when doing html browser

                    foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                    {
                        double d1 = lhs2.GetDataSimple(t);
                        double d2 = double.NaN;
                        if (operatorOneOf3Types == EContribType.D)
                        {
                            if (tsTuple.Item1 != null)
                            {
                                d2 = tsTuple.Item1.GetDataSimple(t) - tsTuple.Item1.GetDataSimple(t.Add(-1));
                            }
                        }
                        else if (operatorOneOf3Types == EContribType.RD)
                        {
                            if (tsTuple.Item2 != null)
                            {
                                d2 = tsTuple.Item2.GetDataSimple(t) - tsTuple.Item2.GetDataSimple(t.Add(-1));
                            }
                        }
                        else if (operatorOneOf3Types == EContribType.M)
                        {
                            if (tsTuple.Item1 != null && tsTuple.Item2 != null)
                            {
                                d2 = tsTuple.Item1.GetDataSimple(t) - tsTuple.Item2.GetDataSimple(t);
                            }
                        }
                        double factor = d2 / d1;
                        bool found = false;
                        foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(d, operatorOneOf3Types).storage)
                        {
                            kvp.Value.SetData(t, -factor * kvp.Value.GetDataSimple(t));
                        }
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Search for real observed timeseries inside the DecompData objects (so as not having to find them in databanks).
        /// Returns a tuple with quo (Work) as first element, and ref (Ref) as last element.
        /// </summary>
        /// <param name="decompDatas"></param>
        /// <param name="operatorOneOf3Types"></param>
        /// <param name="s"></param>
        /// <param name="tsQuo"></param>
        /// <param name="tsRef"></param>
        public static Tuple<Series, Series> GetRealTimeseries(DecompDatas decompDatas, string s)
        {
            //Find the real values of the series for normalization
            Series tsQuo = GetRealTimeseries2(decompDatas, s, EStorage.cellsQuo);
            Series tsRef = GetRealTimeseries2(decompDatas, s, EStorage.cellsRef);            
            Tuple<Series, Series> ts = new Tuple<Series, Series>(tsQuo, tsRef);
            return ts;
        }

        /// <summary>
        /// May return null.
        /// </summary>
        /// <param name="decompDatas"></param>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Series GetRealTimeseries2(DecompDatas decompDatas, string s, EStorage type)
        {
            Series ts = null;
            foreach (List<DecompData> temp in decompDatas.storage)
            {
                foreach (DecompData decompData in temp)
                {
                    if (type == EStorage.cellsQuo) decompData.cellsQuo.storage.TryGetValue(s, out ts);
                    else if (type == EStorage.cellsRef) decompData.cellsRef.storage.TryGetValue(s, out ts);
                    else if (type == EStorage.cellsChangeD) decompData.cellsChangeD.storage.TryGetValue(s, out ts);
                    else if (type == EStorage.cellsChangeDRef) decompData.cellsChangeDRef.storage.TryGetValue(s, out ts);
                    else if (type == EStorage.cellsChangeM) decompData.cellsChangeM.storage.TryGetValue(s, out ts);
                    else new Error("Type error: EStorage");                    
                    if (ts != null) goto Label1;
                }
            }
        Label1:;
            return ts;
        }

        public static bool DecompMatchWord(string colnames3, string varnames)
        {
            if (colnames3 == null) return false;
            return G.ContainsWord(colnames3, G.Chop_GetName(varnames));
        }

        /// <summary>
        /// Translate from for instance "time" to "gekkopivot__time". The inverse method exists.
        /// </summary>
        /// <param name="vars"></param>
        /// <param name="col_t"></param>
        /// <param name="col_variable"></param>
        /// <param name="col_lag"></param>
        /// <param name="col_universe"></param>
        /// <param name="col_equ"></param>
        public static void DecomposeReplaceVars(List<string> vars, string col_t, string col_variable, string col_lag, string col_universe, string col_equ)
        {
            //Do nothing
        }

        /// <summary>
        /// Translate from for instance "gekkopivot__time" to "time". The inverse method exists.
        /// </summary>
        /// <param name="vars"></param>
        /// <param name="col_t"></param>
        /// <param name="col_variable"></param>
        /// <param name="col_lag"></param>
        /// <param name="col_universe"></param>
        /// <param name="col_equ"></param>
        public static void DecomposeReplaceVars(List<FrameFilter> vars, string col_t, string col_variable, string col_lag, string col_universe, string col_equ)
        {
            //Do nothing
        }

        public static string ConvertSetname(string domain)
        {
            string rv = null;
            if (domain == null || domain == Globals.dimensionWithoutDomain)
            {
                rv = Globals.decompUniversal;
            }
            else
            {
                rv = domain;
            }
            return rv;
        }

        public static string DecompAddText(FrameLight_OLD frame, FrameLightRow row, string s1, string s)
        {
            CellLight c = row.Get(frame, s);
            if (c.type == ECellLightType.None)
            {
                s1 += Globals.pivotTableDelimiter + Globals.decompNull;
            }
            else if (c.type == ECellLightType.String)
            {
                s1 += Globals.pivotTableDelimiter + c.text;
            }
            else
            {
                throw new GekkoException();
            }
            return s1;
        }

        public static double DecomposePutIntoTable2HelperOperators(DecompData decompTables, string operatorLower, GekkoSmpl smpl, string lhs, GekkoTime t2, string colname, bool isScalarModel, bool missingAsZero)
        {

            double d = double.NaN;

            if (operatorLower == "d" || operatorLower == "p" || operatorLower == "sd" || operatorLower == "sp")
            {
                d = decompTables.cellsContribD[colname].GetData(smpl, t2);
            }
            else if (operatorLower == "rd" || operatorLower == "rp" || operatorLower == "srd" || operatorLower == "srp")
            {
                d = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
            }
            else if (operatorLower == "m" || operatorLower == "q" || operatorLower == "sm" || operatorLower == "sq")
            {
                d = decompTables.cellsContribM[colname].GetData(smpl, t2);
            }
            else
            {
                //do nothing
            }

            if (missingAsZero && isScalarModel)
            {
                if (G.IsNumericalError(d))
                {
                    d = 0d;
                }
            }

            return d;
        }

        public static EDecompBanks DecompBanks_OLDREMOVESOON(DecompOperator op)
        {
            EDecompBanks banks = EDecompBanks.Work;
            string operator1 = op.OperatorLower();
            if (operator1 == "r" || operator1 == "xr" || operator1 == "xrn" || operator1 == "rd" || operator1 == "xrd" || operator1 == "rp" || operator1 == "xrp" || operator1 == "rdp" || operator1 == "xrdp") banks = EDecompBanks.Ref;
            if (operator1 == "m" || operator1 == "xm" || operator1 == "q" || operator1 == "xq" || operator1 == "mp" || operator1 == "xmp") banks = EDecompBanks.Multiplier;
            return banks;
        }

        public static void DecompPrintDatas(GekkoTime gt1, GekkoTime gt2, List<List<DecompData>> decompDatas, EContribType operatorOneOf3Types)
        {
            int c1 = -1;
            foreach (List<DecompData> dd in decompDatas)
            {
                c1++;
                int c2 = -1;
                foreach (DecompData d in dd)
                {
                    c2++;
                    DecompDict dict = GetDecompDatas(d, operatorOneOf3Types);
                    foreach (KeyValuePair<string, Series> kvp in dict.storage)
                    {
                        string nme = kvp.Key;
                        Series ts = kvp.Value;
                        foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                        {
                            double v = ts.GetVal(t);
                            G.Writeln(c1 + " -- " + c2 + "  name " + nme + " " + t.ToString() + " = " + v);
                        }
                    }
                }
            }
        }

        public static EquationHelper DecompEvalGekko(string variable)
        {
            EquationHelper found = Program.FindEquationByMeansOfVariableName(variable);
            if (found == null)
            {
                new Error("DECOMP: Could not find variable '" + variable + "' as left-hand side in model");
            }
            string[] ss = found.equationText.Split('=');

            string rhs = ss[1].Trim();

            string lhsText = ss[0].Trim();
            string[] ss0 = lhsText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!G.Equal(ss0[0], "frml"))
            {
                new Error("Model equation '" + variable + "': Equation does not start with 'frml'");
                //throw new GekkoException();
            }

            string lhs = null;
            for (int i = 2; i < ss0.Length; i++)
            {
                lhs += ss0[i];
            }
            lhs = lhs.Trim();  //trimmed with no blanks                                                              

            if (rhs.EndsWith("$")) rhs = rhs.Substring(0, rhs.Length - 1) + ";";  //only replace last $, not other $

            rhs = rhs.Trim();
            if (rhs.EndsWith(";")) rhs = rhs.Substring(0, rhs.Length - 1);

            for (int i = 1; i < 20; i++)
            {
                rhs = rhs.Replace("(-" + i + ")", "[-" + i + "]");
                rhs = rhs.Replace("(+" + i + ")", "[+" + i + "]");
            }

            string type = "none";  //dlog, dif, diff, log
            if (lhs.StartsWith("dlog(", StringComparison.OrdinalIgnoreCase))
            {
                type = "dlog";
                rhs = found.lhs + "[-1] * exp(" + rhs + ")";
            }
            else if (lhs.StartsWith("dif(", StringComparison.OrdinalIgnoreCase))
            {
                type = "dif";
                rhs = found.lhs + "[-1] + (" + rhs + ")";
            }
            else if (lhs.StartsWith("diff(", StringComparison.OrdinalIgnoreCase))
            {
                type = "diff";
                rhs = found.lhs + "[-1] + (" + rhs + ")";
            }
            else if (lhs.StartsWith("diff(", StringComparison.OrdinalIgnoreCase))
            {
                type = "log";
                rhs = "exp(" + rhs + ")";
            }

            if (found.equationCodeJ != "" && found.equationCodeJ != "_" && found.equationCodeJ != "__")
            {
                if (found.equationCodeJadditive)
                {
                    rhs = rhs + " + " + found.Jname;
                }
                else if (found.equationCodeJmultiplicative)
                {
                    rhs = "(" + rhs + ") * (1 + " + found.Jname + ")";
                }
                else
                {
                    //should not happen
                    new Error("Problem with J-factors in equation " + found.lhs);
                    //throw new GekkoException();
                }
            }

            if (found.equationCodeD != "" && found.equationCodeD != "_")
            {
                rhs = "(1 - " + found.Dname + ") * (" + rhs + ") + " + found.Dname + " * " + found.Zname;
            }

            string temp2 = found.lhs + "-(" + rhs + ");";
            //rhs = found.lhs + " = " + rhs + ";";
            rhs = rhs + ";";
            //string tmp = rhs;
            string tmp = temp2;

            try
            {
                if (Globals.printAST)
                {
                    G.Writeln2("-------------- EVAL ---------------");
                    G.Writeln2("EVAL " + G.ReplaceGlueSymbols(tmp));
                    G.Writeln2("-----------------------------------");
                }

                Globals.expressions = null;  //maybe not necessary

                Program.CallEval(null, tmp);

                found.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here
                Globals.expressions = null;  //maybe not necessary               

            }
            catch (Exception e)
            {

            }

            return found;
        }

        public static void Find(O.Find o)
        {
            if (Globals.batchType != EBatchType.PyGekko && G.IsUnitTestingOrNotShowingGUI() && Globals.showFind == true)
            {
                //Skip the "Decomp" thread stuff when unit testing -- will give TreadAbortedException for some reason not understood.
                CreateFindWindow(o);
            }
            else
            {
                //Open FIND window in a new thread
                Thread thread = new Thread(new ParameterizedThreadStart(CreateFindWindow));
                thread.Name = "Find";
                thread.SetApartmentState(ApartmentState.STA);
                thread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                thread.IsBackground = true;
                thread.Start(o);
            }

            if (true)
            {
                //Also see #9237532567
                //This stuff makes sure we wait for the window to open, before we move on with the code.
                for (int i = 0; i < 6000; i++)  //up to 60 s, then we move on anyway
                {
                    System.Threading.Thread.Sleep(10);  //0.01s
                                                        //TODO
                                                        //TODO
                                                        //TODO find a way to measure that the FIND window has been "calculated".
                                                        //TODO --> problem would be if a new model was loaded in the meantime...
                                                        //TODO Do it same way as for decomp, also testing if it has exception
                                                        //TODO
                                                        //TODO
                    if (1 /* o.decompFind.decompOptions2.numberOfRecalcs */ > 0)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Shows the FIND window. Uses an object argument because it can be called from a new thread.
        /// It really uses O.Find as argument.
        /// </summary>
        /// <param name="o2"></param>
        public static void CreateFindWindow(object o2)
        {
            try
            {
                //Done the same modern way regardless of Program.options.model_gams_dep_method == "both" or not.
                //This option only does stuff for DISP.

                O.Find o = o2 as O.Find;

                Model model = Program.model;
                if (model == null)
                {
                    new Error("It seems no model is loaded, cf. the MODEL command.");
                    return;
                }
                ModelGamsScalar modelGamsScalar = model.modelGamsScalar;                

                if (modelGamsScalar == null)
                {                    
                    using (Error txt = new Error())
                    {
                        txt.MainAdd("DECOMP/FIND cannot access the model equations in a userful form (scalar representation).");
                        txt.MainAdd("This error usually only happes when a .gms file is loaded with for instance 'model <gms> model.gms;',");
                        txt.MainAdd("where you may use DISP to show equations, but DECOMP and FIND does not work. For DECOMP/FIND to work,");
                        txt.MainAdd("you need to use a GAMS scalar model (in form of a zip-file), loaded with 'model <gms> model.zip'.");  //!!! Done this way because it becomes a popup-window !!!

                        txt.MainAdd("The zip-file is produced by GAMS and usually contains the three files gams.gms, dict.txt, and raw.gms inside.");
                        txt.MainAdd("The file raw.gms may be omitted (raw GAMS eqautions), whereas gams.gms/dict.txt contains 'unrolled' (scalar) GAMS equations.");
                        txt.MainAdd("To produce a GAMS scalar model, GAMS must solve the model using the so-called 'convert' option.");
                        txt.MainAdd("Read mere about scalar models in the help system regarding the MODEL statement.");
                    }

                    return;
                }
                ModelGams modelGams = model.modelGams;

                modelGamsScalar.MaybeLoadDataIntoModel(o.decompFind.depth, o.decompFind.decompOptions2.t1, o.decompFind.decompOptions2.t2, o.decompFind.decompOptions2.missingAsZero, false);

                Globals.itemHandler = new ItemHandler();  //hack

                o.tSelected = o.decompFind.decompOptions2.t1;  //selected time
                List<string> vars = O.Restrict(o.iv, false, false, false, true);

                if (o.iv2 != null) { List<string> vars2 = O.Restrict(o.iv2, false, false, false, true); FindConnection(o.tSelected, vars[0], vars2[0], modelGamsScalar); return; }

                string variableName = vars[0]; //.Replace(" ", "");  //no blanks
                
                List<EqInfoSimple> eqsNew = GamsModel.GetSortedEquations(variableName, GekkoTime.tNull, model, false, true, false);

                List<string> firstList = new List<string>();

                double maxScore = double.MinValue;
                foreach (EqInfoSimple eqHelper in eqsNew)
                {
                    if (eqHelper.score > maxScore) maxScore = eqHelper.score;
                }

                //This seems to just gather material for the GUI representation                
                int lineCounter = -1;
                foreach (EqInfoSimple eqHelper in eqsNew)
                {
                    lineCounter++;                    
                    string eqName = eqHelper.eqName;
                    string eqName3 = eqHelper.eqNameWithLag;
                    EquationTextHelper helper2 = new EquationTextHelper();
                    helper2.showTime = o.decompFind.decompOptions2.showTime;
                    List<string> precedents = modelGamsScalar.GetPrecedentsNames(eqHelper.eqNumber, helper2, modelGamsScalar.GetDecompT());
                    string boolLhs = "";  //lhs                        
                    if (eqHelper.score % 1 == 0) boolLhs = Globals.protectSymbol;
                    string boolName = "";  //name
                    boolName = "";
                    if (eqHelper.score >= 100d && eqHelper.score == maxScore) boolName = Globals.protectSymbol;  //We may get two eqs where the first has score 101 and the second 102, and in that case we show the second as "Name" (based on res_... variable).                    
                    string tt = "tx0";
                    int selectedRow = 0;  //can be changed...  (cf. #jk8dsfa7yauewfh)
                    string textColor = "Black";
                    if (o.decompFind.decompOptions2.new_from != null)
                    {
                        if (o.decompFind.decompOptions2.new_from.Contains(eqName3, StringComparer.OrdinalIgnoreCase))
                        {
                            textColor = "Gray";
                        }
                    }
                    //This is where the contents of each GUI line is set
                    //Hack that it is a global variable...
                    Globals.itemHandler.Add(new EquationListItem(eqName3, " ", boolName, boolLhs, tt, Stringlist.GetListWithCommas(precedents, " "), "Black", textColor, lineCounter == selectedRow, eqName));
                }

                if (eqsNew == null || eqsNew.Count == 0)
                {
                    new Error("Could not find any equation(s) containing the variable '" + variableName + "'");
                }
                string firstEqName2 = eqsNew[0].eqName;
                WindowFind windowFind = new WindowFind(o);
                windowFind.Title = variableName + " - " + "Gekko equations";
                windowFind.FindSetButtons(firstEqName2, firstList, model);
                windowFind.FindSetLabel(variableName);
                windowFind._activeEquation = firstEqName2;
                windowFind._activeVariable = null;
                EquationTextHelper helper = new EquationTextHelper();
                helper.showTime = o.decompFind.decompOptions2.showTime;
                windowFind.FindSetEquation(firstEqName2, helper, modelGamsScalar.GetDecompT(), model);
                windowFind.decompFind.SetWindow(windowFind);
                windowFind.ShowDialog();

                return;


            }
            catch (Exception e)
            {
                //we ignore the exception here, so that Gekko and other windows are not crashing.
                if (Globals.runningOnTTComputer)
                {
                    MessageBox.Show("TTH: " + e.Message + " --findtrace-> " + e.StackTrace);
                }
            }
        }        

        /// <summary>
        /// Returns the chosen variable as chunks
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        private static List<string> GetChosenVariable(string variableName)
        {
            string dbName, variableName2; string freq; string[] indexes;
            O.Chop(variableName, out dbName, out variableName2, out freq, out indexes);
            List<string> chosen = new List<string>();
            chosen.Add(variableName2);
            if (indexes != null)
            {
                foreach (string s in indexes) chosen.Add("'" + s + "'");
            }
            chosen.Add("t");
            return chosen;
        }

        private static bool CheckOk2(string s)
        {
            string s2 = s.Replace(" ", "").ToLower();
            bool b = s2 == "t" || s2.StartsWith("t-") || s2.StartsWith("t+");
            return b;
        }        
        
        ///// <summary>
        ///// Input is a list of equations (represented as integer values) that contain the variableName. This info, the integers, is part of the scalarModel object.
        ///// The equations are sorted after "relevance".
        ///// A list of EqHelper objects is returned: basically the equation names.
        ///// </summary>
        ///// <param name="o"></param>
        ///// <param name="model"></param>
        ///// <param name="modelGamsScalar"></param>
        ///// <param name="vars"></param>
        ///// <param name="eqNumbers"></param>
        ///// <returns></returns>
        //public static List<EqInfoSimple> FindEquationsThatContainGivenVariableSorted(string variableName, GekkoTime tSelected, List<int> eqNumbers, Model model)
        //{
        //    //Get a list of helper objects corresponding to each scalar equation the variable is part of
        //    List<EqInfoSimple> scalarEquations = new List<EqInfoSimple>();
        //    foreach (int eqNumber in eqNumbers)
        //    {
        //        string eqName = model.modelGamsScalar.GetEqName(eqNumber);
        //        string eqNameWithLag = null;
        //        eqNameWithLag = G.Chop_DimensionConvertToLag(eqName, model.modelGamsScalar.Maybe2000GekkoTime(tSelected), false);
        //        EqInfoSimple e = new EqInfoSimple();
        //        e.eqName = eqName;
        //        e.eqNameWithLag = eqNameWithLag;
        //        e.eqNumber = eqNumber;
        //        scalarEquations.Add(e);
        //    }

        //    if (true)
        //    {
        //        return scalarEquations;
        //    }
        //    else
        //    {

        //        List<EqInfoSimple> eqsNew = new List<EqInfoSimple>();
        //        List<EqInfoSimple> eqsNew1 = new List<EqInfoSimple>();
        //        List<EqInfoSimple> eqsNew2 = new List<EqInfoSimple>();

        //        string s2 = G.Chop_RemoveIndex(variableName);

        //        List<string> eqNames = new List<string>();

        //        if (model.modelGekko != null)
        //        {
        //            eqNames.Add(Globals.decompGekkoEquationPrefix + s2);
        //            // -------> do something so e_fy is first
        //        }
        //        else if (model.modelGams != null)
        //        {
        //            List<ModelGamsEquation> foldedEquations = null;
        //            model.modelGams.equationsByVarname.TryGetValue(s2, out foldedEquations);
        //            if (foldedEquations != null)
        //            {
        //                foreach (ModelGamsEquation foldedEquation in foldedEquations)
        //                {
        //                    eqNames.Add(foldedEquation.nameGams);
        //                }
        //            }
        //        }

        //        // For instance, when doing FIND vtBund in MAKRO model, we have these:
        //        // - scalarEquation.eqNameWithLag = E_vtHhx_tot, E_vtKilde, E_ftBund_tot, E_vtBund_tot
        //        // - foldedEquation.nameGams      = E_vtBund, E_ftBund_tot, E_vtBund_tot
        //        // ---> this gives two hits: E_ftBund_tot and E_vtBund_tot.                                

        //        foreach (EqInfoSimple scalarEquation in scalarEquations)
        //        {
        //            foreach (string eq in eqNames)
        //            {
        //                if (G.Equal(scalarEquation.eqNameWithLag, eq))
        //                {
        //                    scalarEquation.best = true;
        //                }
        //            }
        //        }

        //        foreach (EqInfoSimple helper in scalarEquations)
        //        {
        //            if (helper.best) eqsNew1.Add(helper);
        //        }

        //        foreach (EqInfoSimple helper in scalarEquations)
        //        {
        //            if (!helper.best) eqsNew2.Add(helper);
        //        }

        //        var eqsNew1a = eqsNew1.OrderBy(x => x.eqNameWithLag, new G.NaturalComparer(G.NaturalComparerOptions.Default));
        //        var eqsNew2a = eqsNew2.OrderBy(x => x.eqNameWithLag, new G.NaturalComparer(G.NaturalComparerOptions.Default));
        //        eqsNew.AddRange(eqsNew1a);
        //        eqsNew.AddRange(eqsNew2a);
        //        return eqsNew;
        //    }
        //}

        ///// <summary>
        ///// Get scalar equations in simple text form.
        ///// </summary>
        ///// <param name="o"></param>
        ///// <param name="model"></param>
        ///// <param name="modelGamsScalar"></param>
        ///// <param name="vars"></param>
        ///// <param name="eqNumbers"></param>
        ///// <returns></returns>
        //public static List<EqInfoSimple> GetScalarEquations(string variableName, GekkoTime tSelected, List<int> eqNumbers, Model model)
        //{
        //    //Get a list of helper objects corresponding to each scalar equation the variable is part of
        //    List<EqInfoSimple> scalarEquations = new List<EqInfoSimple>();
        //    foreach (int eqNumber in eqNumbers)
        //    {
        //        string eqName = model.modelGamsScalar.GetEqName(eqNumber);
        //        string eqNameWithLag = null;
        //        eqNameWithLag = G.Chop_DimensionConvertToLag(eqName, model.modelGamsScalar.Maybe2000GekkoTime(tSelected), false);
        //        EqInfoSimple e = new EqInfoSimple();
        //        e.eqName = eqName;
        //        e.eqNameWithLag = eqNameWithLag;
        //        e.eqNumber = eqNumber;
        //        scalarEquations.Add(e);
        //    }
        //    return scalarEquations;
        //}

        public static Rich GetColoredEquations(string s)
        {
            int more = 20;
            TokenList tokens = StringTokenizer.GetTokensWithLeftBlanks(s, more);
            
            Rich r = new Rich();

            int n = Globals.RainbowParentheses.Count;
            int depth = 0;

            for (int i = 0; i < tokens.Count() - more - 1; i++)
            {
                //replace for instance x(-1) with x[-1]
                if (i > 0 && (tokens[i - 1].type == ETokenType.Word || tokens[i - 1].s == "]") && tokens[i].leftblanks == 0 && tokens[i].s == "(" && (tokens[i + 1].s == "-" || tokens[i + 1].s == "+") && G.IsInteger(tokens[i + 2].s) && tokens[i + 3].s == ")")
                {
                    tokens[i].s = "[";
                    tokens[i + 3].s = "]";
                    i += 3;
                }
            }

            for (int i = 0; i < tokens.Count() - more - 1; i++)
            {

                if (i > 0 && (tokens[i - 1].type == ETokenType.Word || tokens[i - 1].s == "]") && tokens[i].leftblanks == 0 && tokens[i].s == "[" && (tokens[i + 1].s == "-" || tokens[i + 1].s == "+") && G.IsInteger(tokens[i + 2].s) && tokens[i + 3].s == "]")
                {
                    string x = tokens[i].ToString() + tokens[i + 1].ToString() + tokens[i + 2].ToString() + tokens[i + 3].ToString();
                    r.Add(x, Globals.RainbowNumber);
                    i += 3;
                }
                else if (tokens[i].type == ETokenType.Number || tokens[i].type == ETokenType.QuotedString)
                {
                    r.Add(tokens[i].ToString(), Globals.RainbowNumber);
                }
                else if (tokens[i].s == "(" || tokens[i].s == "[" || tokens[i].s == "{")
                {
                    r.Add(tokens[i].ToString(), Globals.RainbowParentheses[DepthHelper(depth, n)]);
                    depth++;
                }
                else if (tokens[i].s == ")" || tokens[i].s == "]" || tokens[i].s == "}")
                {
                    depth--;
                    r.Add(tokens[i].ToString(), Globals.RainbowParentheses[DepthHelper(depth, n)]);
                }
                else
                {
                    r.Add(tokens[i].ToString());
                }
            }
            return r;
        }

        private static int DepthHelper(int depth, int n)
        {
            int i = depth % n;
            if (i < 0) i = 0;
            if (i >= n) i = n - 1;
            return i;
        }

        public static string NonFoundInModelError(string variableName, ModelGamsScalar modelGamsScalar)
        {
            bool variableExists = false;
            bool variableExistsAndHasIndex = false;
            string error = null;
            foreach (KeyValuePair<string, int> kvp in modelGamsScalar.dict_FromVarNameToANumber.GetDictionaryForIteration())
            {
                if (G.Equal(G.Chop_RemoveIndex(variableName), G.Chop_RemoveIndex(kvp.Key)))
                {
                    variableExists = true;
                    if (G.Chop_HasIndex(kvp.Key)) variableExistsAndHasIndex = true;
                    break;
                }
            }

            if (!variableExists)
            {
                error = "The variable '" + G.Chop_RemoveIndex(variableName) + "' does not exist in the model. You may use the INDEX command to search for variable names, or DISP '...' to search descriptions.";
                return error;
            }

            if (G.Chop_HasIndex(variableName))
            {
                if (variableExistsAndHasIndex)
                {
                    error = "The '" + G.Chop_GetName(variableName) + "' element [" + Stringlist.GetListWithCommas(G.Chop_GetIndex(variableName)) + "] was not found in the model, even though the variable '" + G.Chop_GetName(variableName) + "' does exist. You may use 'DISP " + G.Chop_GetName(variableName) + ";' to see/view the elements of the variable.";
                    return error;
                }
                else
                {
                    error = "The model variable " + G.Chop_GetName(variableName) + " has no index/dimensions. Try 'FIND " + G.Chop_GetName(variableName) + ";'";
                    return error;
                }
            }
            else
            {
                if (variableExistsAndHasIndex)
                {
                    error = "The variable " + variableName + " exists in the model, but has index/dimensions. You may use 'DISP " + variableName + ";' to see the elements of the variable.";
                    return error;
                }
                else
                {
                    //...how could we ever end here?
                    error = "The variable " + variableName + " cannot be found in the model.";
                    return error;
                }
            }
        }

        /// <summary>
        /// Find shortest model connection between two vars and print out the connection
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        private static void FindConnection(GekkoTime t0, string x1, string x2, ModelGamsScalar modelGamsScalar)
        {
            //Speed-up: doing flood-fill from the endpoint and make them meet?         

            int timeIndex = modelGamsScalar.FromGekkoTimeToTimeInteger(modelGamsScalar.Maybe2000GekkoTime(t0));

            Dictionary<PeriodAndVariable, Flood> colors = new Dictionary<PeriodAndVariable, Flood>();

            int a1 = modelGamsScalar.dict_FromVarNameToANumber.GetInt(x1);
            if (a1 == -12345) new Error(NonFoundInModelError(x1, modelGamsScalar));

            int a2 = modelGamsScalar.dict_FromVarNameToANumber.GetInt(x2);
            if (a2 == -12345) new Error(NonFoundInModelError(x2, modelGamsScalar));

            PeriodAndVariable pv1 = new PeriodAndVariable(timeIndex, a1);
            PeriodAndVariable pv2 = new PeriodAndVariable(timeIndex, a2);            

            Flood start = new Flood();
            start.color = 0;
            start.parent = null;
            start.pv = pv1;

            Flood end = new Flood();
            end.color = -12345;
            end.parent = null;
            end.pv = pv2;

            List<Flood> xxx = new List<Flood>();
            xxx.Add(start);

            while (true)
            {
                bool done = false;
                List<Flood> yyy = new List<Flood>();
                foreach (Flood x in xxx)
                {
                    yyy.AddRange(Program.Flood1Color(x, end, colors, out done, modelGamsScalar));
                    if (done) break;
                }
                if (done) break;
                if (yyy.Count == 0) break;
                xxx = yyy;
            }

            List<string> temp = new List<string>();
            Flood f = colors[pv2];

            bool b = false; // G.Equal(Program.options.decomp_equation_style, "gams");

            while (true)
            {
                //#6irhwakery7
                string name = G.Chop_DimensionAddLag(f.pv.GetVariableAndPeriod(modelGamsScalar).Item1, modelGamsScalar.Maybe2000GekkoTime(t0), f.pv.GetVariableAndPeriod(modelGamsScalar).Item2, b, b, "");

                string label = Program.GetVariableExplanation1Line(name, false);

                string lbl = null;
                if (!G.NullOrEmpty(label)) lbl = " (" + label + ")";
                temp.Add(name + lbl);
                if (f.eq != -12345)
                {
                    temp.Add("--> " + modelGamsScalar.GetEqName(f.eq) + " --> ");
                }
                if (f.parent == null) break;
                f = f.parent;
            }
            temp.Reverse();

            //string eqName3 = G.Chop_DimensionSetLag(eqName, o.t0, false);

            string txt = null;
                        
            foreach (string s2 in temp)
            {
                // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
                // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
                // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK see also hack below
                // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
                // HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK HACK
                //string s3 = s2.Replace("[2025]", "[-2]").Replace("[2026]", "[-1]").Replace("[2027]", "").Replace("[2028]", "[+1]").Replace("[2029]", "[+2]");
                txt += s2 + G.NL + G.NL;
            }            

            WindowMessageBox w = new WindowMessageBox(EMessageBox.Normal);
            w.Height = 500;
            w.Width = 800;
            w.textBox1.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            w.textBox1.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            w.textBox1.TextWrapping = System.Windows.TextWrapping.NoWrap;            
            w.textBox1.Text = txt;
            w.textBox1.FontFamily = new System.Windows.Media.FontFamily("Courier New");
            w.textBox1.FontSize = 11;
            w.ShowDialog();
        }

        /// <summary>
        /// A bit like the same engine as used for html browser, this code finds precedents
        /// to a given variable from a given equation. It returns a compact object which has 
        /// a list of connections. The first item in .children is always the variableName item itself (first row of decomp table)
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="equationName"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public static FlowInfo GetFlowInfoFromDecomp(GekkoTime t1, GekkoTime t2, string variableName, string equationName, DecompFind decompFind, WalkInfo walkInfo)
        {            
            FlowInfo flowInfo = new FlowInfo();

            flowInfo.variableName = variableName;
            flowInfo.equationName = equationName;
            //flowInfo.period = t1.Add(offset);  //2030

            ModelGamsScalar modelGamsScalar = Program.model.modelGamsScalar;
            Model model = Program.model;
            //modelGamsScalar.MaybeLoadDataIntoModel(0, t1, t2, false);
            DecompOptions2 decompOptions2 = new DecompOptions2();
            decompOptions2.t1 = t1;
            decompOptions2.t2 = t2;            
            string op2 = decompFind.decompOptions2.decompOperator.OperatorLower().Replace("x", "");
            string op = "d";
            if (op2 == "m" || op2 == "q" || op2 == "mp") op = "m";
            decompOptions2.decompOperator = new DecompOperator(op);
            decompOptions2.new_select = new List<string>() { variableName };
            decompOptions2.new_from = new List<string>() { equationName };
            decompOptions2.new_endo = new List<string>() { variableName };            
            decompOptions2.rows = new List<string>() { "vars", "lags" };
            decompOptions2.cols = new List<string>() { "time" };
            decompOptions2.expand = true;
            decompOptions2.ignore = decompFind.decompOptions2.ignore;
            GekkoSmpl smpl = new GekkoSmpl(t1, t2);
            DecompDatas decompDatas = new DecompDatas();
            GekkoTime gt1, gt2;
            Gekko.Decomp.DecompMainInit(out gt1, out gt2, t1, t2, decompOptions2.decompOperator);
            Gekko.Decomp.EContribType operatorOneOf3Types = decompOptions2.decompOperator.type;
            string lhsString = "Expression value";
            Gekko.Decomp.PrepareEquations(t1, t2, decompOptions2.decompOperator, decompOptions2, false, modelGamsScalar);
            if (decompDatas.storage == null) decompDatas.storage = new List<List<DecompData>>();
            decompDatas.MAIN_data = null;
            if (decompDatas.storage == null || decompDatas.storage.Count == 0) Gekko.Decomp.InitDecompDatas(decompOptions2, decompDatas, model);
            decompOptions2.decompOperator = new DecompOperator(op);
            decompOptions2.showErrors = true;
            string residualName = Program.GetDecompResidualName(0, 1);
            int funcCounter = 0;
            DecompData dd = Gekko.Decomp.DecompLowLevelScalar(gt1, gt2, decompOptions2.link[0].GAMS_dsh[0], decompOptions2.decompOperator, residualName, ref funcCounter, decompOptions2.missingAsZero, model);
            Decomp.DecompMainMergeOrAdd(decompDatas, dd, 0, 0);  //probably superfluous when looking a abs differences?
            decompDatas.MAIN_data = dd; decompDatas.storage[0][0] = dd;
            DecompOutput decompOutput = Decomp.DecompPivotToTable(smpl, t1, t2, dd, decompDatas, lhsString, decompOptions2.decompOperator, operatorOneOf3Types, decompOptions2, model);
            Table decompTable = decompOutput.table;

            //Hack, because after expand, removing lags does not work in pivot (maybe it should...!)
            GekkoDictionary<string, double> poolingFrom = new GekkoDictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            for (int i2 = 2; i2 <= decompTable.GetRowMaxNumber(); i2++)
            {
                Cell cellVariableName = decompTable.Get(i2, 1);
                List<string> vars = new List<string>();
                Cell cellFirstData = decompTable.Get(i2, 2);
                string uniqueName = null;
                if (cellFirstData != null)
                {
                    vars = cellFirstData.vars_hack;
                    uniqueName = Decomp.HiddenVariableHelper(cellFirstData, true);
                }
                //string sVarsInside = Stringlist.GetListWithCommas(vars).Replace("¤", "");
                //string label = null;
                //if (uniqueName != null) label = Program.SpecialXmlChars(Program.GetVariableExplanation1Line(uniqueName));
                string name = cellVariableName.CellText.TextData[0];
                name = name.Trim();

                string name2 = name;
                if (walkInfo.ignoreLags) name2 = G.Chop_RemoveLagOrLead(name);
                if (name2 != name) walkInfo.lagsOrLeadsWereEncountered = true;

                Cell cellData = decompTable.Get(i2, 2);
                double value = cellData.number;

                if (poolingFrom.ContainsKey(name2))
                {
                    poolingFrom[name2] += value;
                }
                else poolingFrom.Add(name2, value);
            }

            foreach (KeyValuePair<string, double> kvp in poolingFrom)
            {
                FlowItem flowItem = new FlowItem();
                flowItem.from = kvp.Key;
                flowItem.to = flowInfo.variableName;
                flowItem.v = kvp.Value;
                flowInfo.children.Add(flowItem);
            }

            return flowInfo;
        }

        public enum ENormalizerType
        {
            None,
            Normalizer,
            NormalizerWithLagOrLead
        }
    }

    /// <summary>
    /// This is probably only for equation names
    /// </summary>
    public class DecompStartHelper
    {
        public string name = null; //the "x" in "x[a, b, <time>]"
        public string fullName = null; //the "x[a, b]" in "x[a, b, <time>]"
        public MultidimElement indexes = null; //the ["a", "b"] in "x[a, b, <time>]"
        public DecompStartHelperPeriod[] periods = null; //all the <time> periods found
        public int offset = 0;                                                 //
    }

    public class DecompStartHelperPeriod
    {
        public GekkoTime t = GekkoTime.tNull;
        public int eqNumber = -12345;
    }

    /// <summary>
    /// Simple helper class
    /// </summary>
    [ProtoContract]
    public class PeriodAndVariable
    {
        [ProtoMember(1)]
        public int date;

        [ProtoMember(2)]
        public int variable;

        public PeriodAndVariable() //for protobuf
        {
        }

        public PeriodAndVariable(int timeIndex, int aNumber)
        {
            this.date = timeIndex;
            this.variable = aNumber;
        }

        /// <summary>
        /// Converts from ints into something understandable. The name does not contain blanks around commas.
        /// </summary>
        /// <returns></returns>
        public Tuple<string, GekkoTime> GetVariableAndPeriod(ModelGamsScalar modelGamsScalar)
        {
            string varName = modelGamsScalar.GetVarNameA(this.variable);
            GekkoTime gt = modelGamsScalar.FromTimeIntegerToGekkoTime(this.date);
            Tuple<string, GekkoTime> tup = new Tuple<string, GekkoTime>(varName, gt);
            return tup;
        }

        public string ToStringPretty(ModelGamsScalar modelGamsScalar)
        {
            Tuple<string, GekkoTime> xx = this.GetVariableAndPeriod(modelGamsScalar);
            return xx.Item1 + "[" + xx.Item2.ToString() + "]";
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.date; //the 17 and 31 is a trick (primes) to get the hashcodes as distinct as possible.
            hash = hash * 31 + this.variable;
            return hash;
        }

        public override bool Equals(object obj)
        {
            PeriodAndVariable other = (PeriodAndVariable)obj;
            if (other == null) return false;
            if (this.date == other.date && this.variable == other.variable) return true;
            return false;
        }
    }
}
