using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class List : IVariable
    {
        //Abstract class containing a List
        //Used for pointing to Lists without having to create/clone them.      

        public bool isNameList = true;        

        [ProtoMember(1)]
        public List<IVariable> list = null;

        public List()
        {
        }

        public List(List<IVariable> list)
        {
            this.list = list;
        }

        public List(List<string> list)
        {
            List<IVariable> m = new List<Gekko.IVariable>();
            foreach (string s in list)
            {
                m.Add(new ScalarString(s));
            }
            this.list = m;
        }

        //!!!This has nothing to do #m1+#m2 etc., see Add(GekkoSmpl t, IVariable x) instead.
        //   This method is just to avoid x.list.Add(...)
        public void Add(IVariable x)
        {
            if (this.list == null) this.list = new List<IVariable>();
            this.list.Add(x);
        }

        public int Count()
        {
            return this.list.Count;
        }

        public IVariable Indexer(GekkoSmpl t, params IVariable[] indexes)
        {
            if (indexes.Length == 1)
            {
                IVariable index = indexes[0];
                //Indices run from 1, 2, 3, ... n. Element 0 is length of list.
                if (index.Type() == EVariableType.Val)
                {
                    int ival = O.ConvertToInt(index);
                    if (ival < 0)
                    {
                        G.Writeln2("*** ERROR: Illegal LIST indexer [" + ival + "]: negative number not allowed");
                        throw new GekkoException();
                    }
                    else if (ival == 0)
                    {
                        ScalarVal a = new ScalarVal(this.list.Count);
                        return a;
                    }
                    else if (ival > this.list.Count)
                    {
                        G.Writeln2("*** ERROR: Illegal LIST indexer [" + ival + "]: larger than length of list (" + this.list.Count + ")");
                        throw new GekkoException();
                    }
                    
                    return this.list[ival - 1];                    
                    //return this.list[ival - 1].DeepClone();
                    
                }
                else if (index.Type() == EVariableType.Range)
                {
                    Range index_range = index as Range;
                    int ival1 = O.ConvertToInt(index_range.first);
                    int ival2 = O.ConvertToInt(index_range.last);
                    if (ival1 > this.list.Count || ival2 > this.list.Count || ival2 < ival1 || ival1 < 1 || ival2 < 1)
                    {
                        G.Writeln2("*** ERROR: Invalid range, [" + ival1 + " .. " + ival2 + "]");
                        throw new GekkoException();
                    }
                    List<IVariable> tmp = new List<Gekko.IVariable>();
                    for (int i = ival1 - 1; i <= ival2 - 1; i++)
                    {
                        tmp.Add(this.list[i].DeepClone());
                    }
                    List m = new List(tmp);
                    m.isNameList = this.isNameList;
                    return m;
                }
                else if (index.Type() == EVariableType.String)
                {                    
                    string s5 = ((ScalarString)index)._string2;
                    if(s5.Contains("?") || s5.Contains("*"))
                    {
                        //Wildcard: return a list of those
                        List<string> found = Program.MatchWildcard(s5, this.list, null);
                        return new List(found);
                    }
                    else
                    {
                        //Request if the list has the item
                        foreach (IVariable x in this.list)
                        {
                            ScalarString x_string = x as ScalarString;
                            if (x_string != null)
                            {
                                if (G.Equal(x_string._string2, s5)) return Globals.scalarVal1;
                            }
                        }
                        return Globals.scalarVal0;
                    }                    
                }
                else
                {
                    G.Writeln2("*** ERROR: Type mismatch regarding []-index");
                    throw new GekkoException();
                }
            }
            else
            {
                G.Writeln2("*** ERROR: Cannot use " + indexes.Length + "-dimensional indexer on LIST");
                throw new GekkoException();
            }
        }
               

        //public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2)
        //{
        //    throw new GekkoException();
        //}

        //public IVariable Indexer(GekkoSmpl t, IVariable index, IVariablesFilterRange indexRange)
        //{
        //    throw new GekkoException();
        //}

        //public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange, IVariable index)
        //{
        //    throw new GekkoException();
        //}

        //public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange)
        //{            
        //    IVariable iv1 = indexRange.first;
        //    IVariable iv2 = indexRange.last;

        //    if (iv1.Type() == EVariableType.String && iv2.Type() == EVariableType.String)
        //    {                
        //        string s1 = O.GetString(iv1);
        //        string s2 = O.GetString(iv2);
        //        List<string> temp = Program.MatchRange(s1, s2, this.list, null);
        //        return new List(temp);
        //    }
        //    else
        //    {                
        //        int i1 = O.ConvertToInt(iv1);
        //        int i2 = O.ConvertToInt(iv2);
        //        if (i1 < 1)
        //        {
        //            G.Writeln2("*** ERROR: Starting index (" + i1 + ") cannot be < 1");
        //            throw new GekkoException();
        //        }
        //        if (i1 > this.list.Count)
        //        {
        //            G.Writeln2("*** ERROR: Ending index (" + i2 + ") cannot be > length (" + this.list.Count + ")");
        //            throw new GekkoException();
        //        }
        //        if (i1 > i2)
        //        {
        //            G.Writeln2("*** ERROR: Starting index (" + i1 + ") cannot be > than ending index (" + i2 + ")");
        //            throw new GekkoException();
        //        }
        //        return new List(this.list.GetRange(i1 - 1, i2 - i1 + 1)); //GetRange() is a shallow copy, but that is okay since it contains immutable strings            
        //    }
        //}
        
        public IVariable Negate(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: You cannot use minus with lists");                
            throw new GekkoException();
        }

        public void InjectAdd(GekkoSmpl t, IVariable x, IVariable y)
        {
            G.Writeln2("*** ERROR: #8703458724");                
            throw new GekkoException();
        }

        public double GetValOLD(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: Type mismatch: you are trying to extract a VAL from a list.");
            G.Writeln("           Maybe you need an []-indexer on the list, for instance #mylist[2]?");
            throw new GekkoException();
        }

        public double GetVal(GekkoTime t)
        {
            G.Writeln2("*** ERROR: Type mismatch: you are trying to extract a VAL from a list.");            
            throw new GekkoException();
        }

        public double ConvertToVal()
        {
            G.Writeln2("*** ERROR: Cannot extract a VAL from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            G.Writeln2("*** ERROR: Trying to convert a LIST into a STRING.");            
            throw new GekkoException();
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: Type mismatch: you are trying to extract a DATE from a list.");            
            throw new GekkoException();
        }

        public List<IVariable> ConvertToList()
        {
            return this.list;
        }

        public EVariableType Type()
        {
            return EVariableType.List;
        }

        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {                    
                case EVariableType.String:
                    {
                        //HMMM do we want this?
                        return Operators.StringList.Add((ScalarString)x, this, true);
                    }
                default:
                    {
                        G.Writeln2("*** ERROR: Type mismatch regarding add");                
                        throw new GekkoException();
                    }
            }
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: You cannot use subtract with lists");                
            throw new GekkoException();
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: You cannot use multiply with lists");                
            throw new GekkoException();
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: You cannot use divide with lists");                
            throw new GekkoException();
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: You cannot use power function with lists");                
            throw new GekkoException();
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] dims)
        {
            if (dims.Length == 1 && dims[0].Type() == EVariableType.Val)
            {
                int i = O.ConvertToInt(dims[0]);
                if (i < 1 || i > this.list.Count)
                {
                    G.Writeln2("*** ERROR: Illegal LIST indexer [" + i + "]");
                    throw new GekkoException();
                }
                this.list[i - 1] = rhsExpression.DeepClone();
            }
            else
            {
                G.Writeln2("*** ERROR: Unexpected indexer type on LIST (left-hand side)");
                throw new GekkoException();
            }
        }

        public IVariable DeepClone()
        {            
            List<IVariable> temp = new List<IVariable>();
            foreach (IVariable iv in this.list)
            {
                temp.Add(iv.DeepClone());
            }
            return new List(temp);
        }

        public void DeepTrim()
        {            
            foreach (IVariable iv in this.list)
            {
                iv.DeepClone();
            }         
        }
    }
}
