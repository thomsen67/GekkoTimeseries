using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class Map : IVariable, IBank
    {
        //Abstract class containing a List
        //Used for pointing to Lists without having to create/clone them.      

        [ProtoMember(1)]
        public GekkoDictionary<string, IVariable> storage = null;

        public Map()
        {
            //only because protobuf needs it, not for outside use
            //without below, we get protobuf errore
            storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
        }

        public Map(GekkoDictionary<string, IVariable> map)
        {
            this.storage = map;
        }

        //!!!This has nothing to do #m1+#m2 etc., see Add(GekkoSmpl t, IVariable x) instead.
        //   This method is just to avoid x.list.Add(...)        
        public void Add(string s, IVariable x)
        {
            if (this.storage == null) this.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
            AddIvariableHelper(s, x);
        }

        public int Count()
        {
            return this.storage.Count;
        }

        /// <summary>
        /// Similar to Databank.GetIVariable()
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IVariable GetIVariable(string variable)
        {
            return GetIVariable(variable, false);
        }

        /// <summary>
        /// Similar to Databank.GetIVariable()
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="isLhs"></param>
        /// <returns></returns>
        public IVariable GetIVariable(string variable, bool isLhs)
        {
            IVariable iv = null;
            this.storage.TryGetValue(variable, out iv);
            Program.Trace(iv, this, variable, isLhs, false);
            return iv;
        }

        public bool ContainsIVariable(string variable)
        {            
            return storage.ContainsKey(variable);         
        }

        public void AddIVariableWithOverwrite(string name, IVariable x)
        {            
            if (this.ContainsIVariable(name))
            {
                this.RemoveIVariable(name);
                this.AddIVariable(name, x);
            }
        }
        public void AddIVariable(string name, IVariable x)
        {
            AddIVariable(name, x, false);        
        }

        public void AddIVariable(string name, IVariable x, bool isSimpleName)
        {
            //Simpler than the corresponding method in Databank.cs            
            if (!isSimpleName) G.CheckIVariableNameAndType(x, G.CheckIVariableName(name));
            AddIvariableHelper(name, x);
        }

        private void AddIvariableHelper(string name, IVariable x)
        {
            //See also #0893543895
            Series x_ts = x as Series;
            if (x_ts != null) x_ts.name = name;
            this.storage.Add(name, x);
        }

        public void RemoveIVariable(string name)
        {
            if (this.storage.ContainsKey(name)) this.storage.Remove(name);
        }

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            if (indexes.Length == 1)
            {
                IVariable index = indexes[0];
                //Indices run from 1, 2, 3, ... n. Element 0 is length of list.
                if (index.Type() == EVariableType.String)
                {
                    string s = (index as ScalarString).string2;
                    string varnameWithFreq = G.AddFreq(s, null, EVariableType.Var, O.ELookupType.RightHandSide);  //we do not know the freq. So if s has no '!', current freq will be added.
                    IVariable rv = null; this.storage.TryGetValue(varnameWithFreq, out rv);
                    if (rv == null)
                    {
                        new Error("The MAP does not contain the name '" + varnameWithFreq + "'");
                    }
                    return rv;
                }                
                else
                {
                    new Error("Type mismatch regarding MAP []-index (only STRING is allowed)"); return null;
                }
            }
            else
            {
                new Error("Cannot use " + indexes.Length + "-dimensional []-index on MAP"); return null;
            }
        }

        public IVariable Concat(GekkoSmpl t, IVariable x)
        {
            new Error("Type error regarding concat and MAP"); return null;
            
        }

        public IVariable Negate(GekkoSmpl t)
        {
            new Error("You cannot use minus with MAP"); return null;
        }

        public double GetValOLD(GekkoSmpl t)
        {
            new Error("Type mismatch: you are trying to extract a VAL from a MAP."); return double.NaN;
        }

        public double GetVal(GekkoTime t)
        {
            new Error("Type mismatch: you are trying to extract a VAL from a MAP."); return double.NaN;
        }

        public double ConvertToVal()
        {
            G.Writeln2("*** ERROR: Cannot extract a val from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            new Error("Trying to convert a MAP into a STRING."); return null;
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            new Error("Type mismatch: you are trying to extract a DATE from a MAP."); return GekkoTime.tNull;
        }

        public List<IVariable> ConvertToList()
        {
            new Error("Type mismatch: you are trying to extract a LIST from a MAP."); return null;
        }

        public EVariableType Type()
        {
            return EVariableType.Map;
        }

        public string GetName()
        {
            return null;  //has no name
        }

        public string GetFileNameWithPath()
        {
            return null;  //has no filename
        }

        public string GetStamp()
        {
            return null;  //has no stamp
        }

        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            new Error("You cannot use add with MAPs"); return null;
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            new Error("You cannot use subtract with MAPs"); return null;
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            new Error("You cannot use multiply with MAPs"); return null;
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            new Error("You cannot use divide with MAPs"); return null;
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            new Error("You cannot use power function with MAPs"); return null;
        }

        public string Message()
        {
            return "map";
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            if (dims.Length == 1 && dims[0].Type() == EVariableType.String)
            {
                string s = O.ConvertToString(dims[0]);
                string dbName, varName, freq; string[] indexes; O.Chop(s, out dbName, out varName, out freq, out indexes);  
                if(dbName!=null)
                {
                    new Error("You cannot state bank name (with colon) on the left-hand side in a MAP element");
                }
                string varnameWithFreq = G.AddFreq(varName, freq, EVariableType.Var, O.ELookupType.LeftHandSide);
                O.LookupHelperLeftside(smpl, this, varnameWithFreq, freq, rhsExpression, EVariableType.Var, options);
                
            }
            else
            {
                new Error("Unexpected indexer type on MAP (left-hand side)");
            }
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            Map temp = new Map();
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                if (!Object.ReferenceEquals(this, kvp.Value))  //if it contains itself
                {
                    temp.storage.Add(kvp.Key, kvp.Value.DeepClone(truncate));
                }
                else
                {
                    //map containing itself
                    temp.storage.Add(kvp.Key, kvp.Value);
                }
            }
            return temp;
        }

        public void DeepCount(Count count)
        {
            count.n += Globals.count1;
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                if (!Object.ReferenceEquals(this, kvp.Value)) //if it contains itself
                {
                    kvp.Value.DeepCount(count);
                }
            }
        }

        public void DeepTrim()
        {            
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                if (!Object.ReferenceEquals(this, kvp.Value)) //if it contains itself
                {                    
                    kvp.Value.DeepTrim();
                }                
            }            
        }

        public void DeepCleanup(TwoInts yearMinMax)
        {
            if (this.storage == null) this.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                if (!Object.ReferenceEquals(this, kvp.Value)) //if it contains itself
                {
                    kvp.Value.DeepCleanup(yearMinMax);
                }
            }
        }

        public EBankType BankType()
        {
            return EBankType.Map;
        }
    }
}
