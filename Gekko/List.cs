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

        public bool isNameList = true;  //no protobuf
        public bool isFromSeqOfBankvarnames = false;  //no protobuf

        [ProtoMember(1)]
        public List<IVariable> list = null;  

        public List()
        {
            this.list = new List<IVariable>(); //empty list with 0 items
        }

        public List(List<IVariable> list)
        {
            this.list = list;
        }

        public List(IVariable[] list)
        {
            this.list = list.ToList<IVariable>();
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

        // ----------------------------------------------------
        // --------------object methods start----------------
        // ----------------------------------------------------

        //public IVariable append(bool isLhs, GekkoSmpl smpl, IVariable x)
        //{
        //    return Functions.append(isLhs, this, x);
        //}

        

        //public IVariable extend(bool isLhs, GekkoSmpl smpl, IVariable x)
        //{
        //    return Functions.extend(isLhs, this, x);
        //}

        

        // ----------------------------------------------------
        // --------------object methods end------------------
        // ----------------------------------------------------

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

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] indexes)
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
                        G.Writeln2("*** ERROR: Illegal [0] list indexing. Use #m.len() or len(#m) instead of #m[0] to get the length of list #m.");
                        throw new GekkoException();                        
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

                    if (index_range.first.Type() == EVariableType.String && index_range.last.Type() == EVariableType.String)
                    {
                        string s1 = O.ConvertToString(index_range.first);
                        string s2 = O.ConvertToString(index_range.last);

                        List<string> m = new List<string>();
                        foreach (IVariable x in this.list)
                        {
                            string s = O.ConvertToString(x);
                            Program.AddIfInRange(null, null, s, s1, s2, m);
                        }
                        return new List(m);
                    }
                    else
                    {
                        //slice like #m[2..5]

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
                            tmp.Add(this.list[i].DeepClone(null));
                        }
                        List m = new List(tmp);
                        m.isNameList = this.isNameList;
                        return m;
                    }
                }
                else if (index.Type() == EVariableType.String)
                {                    
                    string s5 = ((ScalarString)index).string2;
                    if(s5.Contains("?") || s5.Contains("*"))
                    {
                        //Wildcard: return a list of those
                        List<string> found = Program.Search(s5, new List<string>(Program.GetListOfStringsFromListOfIvariables(this.list.ToArray()))); //the list items are sorted, not like a dict                        
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
                                if (G.Equal(x_string.string2, s5)) return Globals.scalarVal1;
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

        

        public IVariable Negate(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: You cannot use minus with lists");                
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
            G.Writeln2("*** ERROR: Cannot extract a val from " + G.GetTypeString(this) + " type");
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

        public IVariable Add(GekkoSmpl smpl, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.List:
                    {
                        return Functions.extend(smpl, null, null, this, x);
                    }
                    break;
                case EVariableType.String:
                case EVariableType.Val:
                case EVariableType.Date:
                    {
                        // #m + %s
                        //This corresponds somewhat to "broadcasting" in numpy
                        //Also a bit similar to x + %v, where %v is a value.
                        //We add scalar x to each element of list ths
                        //This turns up in names like {#m + '!q'} or {#m}!q.
                        //See also #786592387654
                        return Operators.ScalarList.Add(smpl, x, this, true);  //note: swapping
                    }
                    break;
                default:
                    {
                        G.Writeln2("*** ERROR: Add to list not allowed for this type: " + G.GetTypeString(x));
                        throw new GekkoException();
                    }
                    break;
            }

            return Functions.extend(smpl, null, null, this, x);
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.List:
                    {
                        return Functions.except(t, null, null, this, x);
                    }
                    break;
                case EVariableType.String:
                    {
                        return Functions.except(t, null, null, this, new List(new List<IVariable> { x }));
                    }
                    break;
                default:
                    {
                        G.Writeln2("*** ERROR: Subtract from list not allowed for this type: " + G.GetTypeString(x));
                        throw new GekkoException();
                    }
                    break;               
            }
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.List:
                    {
                        return Functions.intersect(t, null, null, this, x);
                    }
                    break;
                case EVariableType.String:
                    {
                        return Functions.intersect(t, null, null, this, new List(new List<IVariable> { x }));
                    }
                    break;
                default:
                    {
                        G.Writeln2("*** ERROR: Intersect with list not allowed for this type: " + G.GetTypeString(x));
                        throw new GekkoException();
                    }
                    break;
            }
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

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            if (dims.Length == 1 && dims[0].Type() == EVariableType.Val)
            {
                int i = O.ConvertToInt(dims[0]);
                if (i < 1 || i > this.list.Count)
                {
                    G.Writeln2("*** ERROR: List index [" + i + "] out of range 1.." + this.list.Count);
                    throw new GekkoException();
                }
                this.list[i - 1] = rhsExpression.DeepClone(null);
            }
            else
            {
                G.Writeln2("*** ERROR: Expected indexer type VAL on LIST object (left-hand side)");
                throw new GekkoException();
            }
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            List<IVariable> temp = new List<IVariable>();
            if (this.list == null)
            {
                //should not happen, but just in case
            }
            else
            {
                foreach (IVariable iv in this.list)
                {
                    if (!Object.ReferenceEquals(this, iv))
                    {                        
                        temp.Add(iv.DeepClone(truncate));
                    }
                    else
                    {
                        //a list containing itself
                        temp.Add(iv);
                    }
                }
            }
            List l = new List(temp);
            l.isFromSeqOfBankvarnames = this.isFromSeqOfBankvarnames;
            return l;
        }

        public void DeepTrim()
        {            
            foreach (IVariable iv in this.list)
            {
                if (!Object.ReferenceEquals(this, iv))
                {
                    iv.DeepTrim();
                }
            }         
        }

        public void DeepCleanup()
        {
            if (this.list == null) this.list = new List<IVariable>();
            foreach (IVariable iv in this.list)
            {
                if (!Object.ReferenceEquals(this, iv))
                {
                    iv.DeepCleanup();
                }
            }
        }
    }
}
