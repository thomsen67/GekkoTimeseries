using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gekko
{
    public class MetaList : IVariable
    {
        //Abstract class containing a List
        //Used for pointing to Lists without having to create/clone them.      

        public bool isNameList = true;

        public List<string> list = null;

        public MetaList(List<string> list)
        {
            this.list = list;
        }
               
        public int Count()
        {
            return this.list.Count;
        }

        public IVariable Indexer(GekkoTime t, bool isLhs, params IVariable[] indexes)
        {
            if (indexes.Length == 1)
            {
                IVariable index = indexes[0];
                //Indices run from 1, 2, 3, ... n. Element 0 is length of list.
                if (index.Type() == EVariableType.Val)
                {
                    int ival = O.GetInt(index);
                    if (ival < 0)
                    {
                        //HACK here!
                        bool ok = true;
                        for (int i = 0; i < this.list.Count; i++)
                        {
                            if (!this.list[i].Contains(Globals.symbolTurtle))
                            {
                                ok = false;
                                break;
                            }
                        }

                        if (ok)
                        {
                            for (int i = 0; i < this.list.Count; i++)
                            {
                                this.list[i] += "<LAGINDICATOR>" + (-ival);
                            }
                            return this;
                        }
                        else
                        {
                            G.Writeln2("*** ERROR: Illegal element access [" + ival + "]: negative number not allowed");
                            throw new GekkoException();
                        }
                        
                    }
                    else if (ival == 0)
                    {
                        ScalarVal a = new ScalarVal(this.list.Count);
                        return a;
                    }
                    else if (ival > this.list.Count)
                    {
                        G.Writeln2("*** ERROR: Illegal element access [" + ival + "]: larger than length of list (" + this.list.Count + ")");
                        throw new GekkoException();
                    }
                    string s = this.list[ival - 1];

                    IVariable ss = null;
                    ss = new ScalarString(s, isNameList);
                    if (Globals.fixIndexerMaybeTransform)
                    {
                        bool didTransform = false;
                        ss = O.MaybeTransform(ref didTransform, ss, true);
                    }
                    return ss;
                }
                else if (index.Type() == EVariableType.String)
                {
                    string s5 = ((ScalarString)index)._string2;
                    List<string> found = Program.MatchWildcard(s5, this.list, null);
                    return new MetaList(found);
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

        public IVariable Indexer(IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2, GekkoTime t)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariable index, IVariablesFilterRange indexRange, GekkoTime t)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariablesFilterRange indexRange, IVariable index, GekkoTime t)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariablesFilterRange indexRange, GekkoTime t)
        {            
            IVariable iv1 = indexRange.first;
            IVariable iv2 = indexRange.last;

            if (iv1.Type() == EVariableType.String && iv2.Type() == EVariableType.String)
            {                
                string s1 = O.GetString(iv1);
                string s2 = O.GetString(iv2);
                List<string> temp = Program.MatchRange(s1, s2, this.list, null);
                return new MetaList(temp);
            }
            else
            {                
                int i1 = O.GetInt(iv1);
                int i2 = O.GetInt(iv2);
                if (i1 < 1)
                {
                    G.Writeln2("*** ERROR: Starting index (" + i1 + ") cannot be < 1");
                    throw new GekkoException();
                }
                if (i1 > this.list.Count)
                {
                    G.Writeln2("*** ERROR: Ending index (" + i2 + ") cannot be > length (" + this.list.Count + ")");
                    throw new GekkoException();
                }
                if (i1 > i2)
                {
                    G.Writeln2("*** ERROR: Starting index (" + i1 + ") cannot be > than ending index (" + i2 + ")");
                    throw new GekkoException();
                }
                return new MetaList(this.list.GetRange(i1 - 1, i2 - i1 + 1)); //GetRange() is a shallow copy, but that is okay since it contains immutable strings            
            }
        }
        
        public IVariable Negate(GekkoTime t)
        {
            G.Writeln2("*** ERROR: You cannot use minus with lists");                
            throw new GekkoException();
        }

        public void InjectAdd(IVariable x, IVariable y, GekkoTime t)
        {
            G.Writeln2("*** ERROR: #8703458724");                
            throw new GekkoException();
        }

        public double GetVal(GekkoTime t)
        {
            G.Writeln2("*** ERROR: Type mismatch: you are trying to extract a VAL from a list.");
            G.Writeln("           Maybe you need an []-indexer on the list, for instance #mylist[2]?");
            throw new GekkoException();
        }        

        public string GetString()
        {
            G.Writeln2("*** ERROR: Trying to convert a LIST into a STRING.");            
            throw new GekkoException();
        }

        public GekkoTime GetDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: Type mismatch: you are trying to extract a DATE from a list.");            
            throw new GekkoException();
        }

        public List<string> GetList()
        {
            return this.list;
        }

        public EVariableType Type()
        {
            return EVariableType.List;
        }

        public IVariable Add(IVariable x, GekkoTime t)
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

        public IVariable Subtract(IVariable x, GekkoTime t)
        {
            G.Writeln2("*** ERROR: You cannot use subtract with lists");                
            throw new GekkoException();
        }

        public IVariable Multiply(IVariable x, GekkoTime t)
        {
            G.Writeln2("*** ERROR: You cannot use multiply with lists");                
            throw new GekkoException();
        }

        public IVariable Divide(IVariable x, GekkoTime t)
        {
            G.Writeln2("*** ERROR: You cannot use divide with lists");                
            throw new GekkoException();
        }

        public IVariable Power(IVariable x, GekkoTime t)
        {
            G.Writeln2("*** ERROR: You cannot use power function with lists");                
            throw new GekkoException();
        }       

    }
}
