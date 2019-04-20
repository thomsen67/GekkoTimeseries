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

        // ----------------------------------------------------
        // --------------object functions start----------------
        // ----------------------------------------------------

        public IVariable append(bool isLhs, GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method .append() not available for type " + G.GetTypeString(this));
            throw new GekkoException();
        }

        public IVariable extend(bool isLhs, GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method .extend() not available for type " + G.GetTypeString(this));
            throw new GekkoException();
        }

        // ----------------------------------------------------
        // --------------object functions end------------------
        // ----------------------------------------------------     

        //!!!This has nothing to do #m1+#m2 etc., see Add(GekkoSmpl t, IVariable x) instead.
        //   This method is just to avoid x.list.Add(...)
        public void Add(string s, IVariable x)
        {
            if (this.storage == null) this.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
            this.storage.Add(s, x);
        }

        public int Count()
        {
            return this.storage.Count;
        }

        public IVariable GetIVariable(string variable)
        {
            IVariable iv = null;
            this.storage.TryGetValue(variable, out iv);
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
            this.storage.Add(name, x);
        }

        public void RemoveIVariable(string name)
        {
            if (this.storage.ContainsKey(name)) this.storage.Remove(name);
        }

        public IVariable Indexer(GekkoSmpl smpl, O.EIndexerType indexerType, params IVariable[] indexes)
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
                        G.Writeln2("*** ERROR: The MAP does not contain the name '" + varnameWithFreq + "'");
                        throw new GekkoException();
                    }
                    O.DynamicHelperRhs(smpl, rv);
                    return rv;
                }                
                else
                {
                    G.Writeln2("*** ERROR: Type mismatch regarding MAP []-index (only STRING is allowed)");
                    throw new GekkoException();
                }
            }
            else
            {
                G.Writeln2("*** ERROR: Cannot use " + indexes.Length + "-dimensional []-index on MAP");
                throw new GekkoException();
            }
        }

        public IVariable Concat(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: Type error regarding concat and MAP");
            throw new GekkoException();
        }

        public IVariable Negate(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: You cannot use minus with MAP");
            throw new GekkoException();
        }

        //public void InjectAdd(GekkoSmpl t, IVariable x, IVariable y)
        //{
        //    G.Writeln2("*** ERROR: #8703458724");
        //    throw new GekkoException();
        //}

        public double GetValOLD(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: Type mismatch: you are trying to extract a VAL from a MAP.");
            throw new GekkoException();
        }

        public double GetVal(GekkoTime t)
        {
            G.Writeln2("*** ERROR: Type mismatch: you are trying to extract a VAL from a MAP.");
            throw new GekkoException();
        }

        public double ConvertToVal()
        {
            G.Writeln2("*** ERROR: Cannot extract a val from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            G.Writeln2("*** ERROR: Trying to convert a MAP into a STRING.");
            throw new GekkoException();
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: Type mismatch: you are trying to extract a DATE from a MAP.");
            throw new GekkoException();
        }

        public List<IVariable> ConvertToList()
        {
            G.Writeln2("*** ERROR: Type mismatch: you are trying to extract a LIST from a MAP.");
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.Map;
        }

        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: You cannot use add with MAPs");
            throw new GekkoException();
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: You cannot use subtract with MAPs");
            throw new GekkoException();
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: You cannot use multiply with MAPs");
            throw new GekkoException();
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: You cannot use divide with MAPs");
            throw new GekkoException();
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: You cannot use power function with MAPs");
            throw new GekkoException();
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
                    G.Writeln2("*** ERROR: You cannot state bank name (with colon) on the left-hand side in a MAP element");
                    throw new GekkoException();
                }
                string varnameWithFreq = G.AddFreq(varName, freq, EVariableType.Var, O.ELookupType.LeftHandSide);
                O.LookupHelperLeftside(smpl, this, varnameWithFreq, freq, rhsExpression, EVariableType.Var, options);
                
                //IVariable iv = this.GetIVariable(s);
                //if (iv != null) this.RemoveIVariable(s);
                //this.AddIVariable(s, rhsExpression);                
            }
            else
            {
                G.Writeln2("*** ERROR: Unexpected indexer type on MAP (left-hand side)");
                throw new GekkoException();
            }
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            Map temp = new Map();
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                if (!Object.ReferenceEquals(this, kvp.Value))
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

        public void DeepTrim()
        {            
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                if (!Object.ReferenceEquals(this, kvp.Value))
                {
                    kvp.Value.DeepTrim();
                }                
            }            
        }

        public void DeepCleanup()
        {
            if (this.storage == null) this.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                if (!Object.ReferenceEquals(this, kvp.Value))
                {
                    kvp.Value.DeepCleanup();
                }
            }
        }

        public EBankType BankType()
        {
            return EBankType.Map;
        }
    }
}
