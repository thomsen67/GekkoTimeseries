using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public class MetaTimeSeries : IVariable
    {
        //Abstract class containing a TimeSeries
        //Used for pointing to TimeSeries without having to create/clone them.
        //These are not created as often as ScalarVal etc., so ok that they have some more fields...

        public TimeSeries ts = null;
        public int offset = 0;  //offset, for instance a lag like x[-2], in that case index = -2.        

        public MetaTimeSeries(TimeSeries ts)
        {
            this.ts = ts;
            this.offset = 0;  //must always be 0 in simple constructor            
        }

        private MetaTimeSeries(TimeSeries ts, int offset)
        {
            //ONLY use this in Index() function here
            this.ts = ts;
            this.offset = offset;
        }

        public IVariable Indexer(GekkoSmpl t, bool isLhs, params IVariable[] indexes)
        {
            //Check if any indexer elements are LIST type, if not lists = null.
            List<int> lists = null;
            for (int i = 0; i < indexes.Length; i++)
            {
                IVariable iv = indexes[i];
                if (iv.Type() == EVariableType.List)
                {
                    if (lists == null) lists = new List<int>();
                    lists.Add(i);
                }
            }

            if (lists != null)
            {
                List<string> temp = new List<string>();
                if (lists.Count == 1)
                {
                    List<string> m = ((MetaList)indexes[lists[0]]).list;
                    foreach (string s in m)
                    {
                        string ss = null;
                        for (int i = 0; i < indexes.Length; i++)
                        {
                            if (i == lists[0])
                            {
                                ss += Globals.symbolTurtle + s;
                            }
                            else
                            {
                                ss += Globals.symbolTurtle + O.GetString(indexes[i]);
                            }
                        }
                        ss = this.ts.variableName + ss;                        
                        string bankname = null;
                        if (!G.equal(Program.databanks.GetFirst().aliasName, this.ts.parentDatabank.aliasName))
                        {
                            bankname = this.ts.parentDatabank.aliasName + ":";
                        }
                        temp.Add(bankname + ss);
                    }
                }
                else if (lists.Count == 2)
                {
                    List<string> m1 = ((MetaList)indexes[lists[0]]).list;
                    List<string> m2 = ((MetaList)indexes[lists[1]]).list;                    
                    foreach (string s1 in m1)
                    {
                        foreach (string s2 in m2)
                        {
                            string ss = null;
                            for (int i = 0; i < indexes.Length; i++)
                            {
                                if (i == lists[0])
                                {
                                    ss += Globals.symbolTurtle + s1;
                                }
                                else if (i == lists[1])
                                {
                                    ss += Globals.symbolTurtle + s2;
                                }
                                else
                                {
                                    ss += Globals.symbolTurtle + O.GetString(indexes[i]);
                                }
                            }
                            ss = this.ts.variableName + ss;                            
                            string bankname = null;
                            if (!G.equal(Program.databanks.GetFirst().aliasName, this.ts.parentDatabank.aliasName))
                            {
                                bankname = this.ts.parentDatabank.aliasName + ":";
                            }
                            temp.Add(bankname + ss);
                        }
                    }
                }
                else
                {
                    G.Writeln2("*** ERROR: Sorry, unfolding of > 2 dimension not supported at the moment");
                    throw new GekkoException();
                }                
                MetaList ml = new Gekko.MetaList(temp);
                return ml;
            }
            else if (indexes.Length > 0 && indexes[0].Type() == EVariableType.String)
            {
                string hash = TimeSeries.GetHashCodeFromIvariables(indexes);
                O.ECreatePossibilities canCreate = O.ECreatePossibilities.None;
                if (isLhs) canCreate = O.ECreatePossibilities.Can;
                string varHash = this.ts.variableName + Globals.symbolTurtle + hash;
                TimeSeries ts = this.ts.parentDatabank.GetVariable(this.ts.freqEnum, varHash);
                if (ts == null)
                {
                    if (canCreate == O.ECreatePossibilities.None)
                    {
                        string prettyName = this.ts.parentDatabank.aliasName + ":" + this.ts.variableName + "[" + G.PrettifyTimeseriesHash(hash, false, false) + "]";
                        G.Writeln2("*** ERROR: Cannot find " + prettyName);
                        if (prettyName.Contains("["))
                        {
                            Program.ArrayTimeseriesTip(this.ts.variableName);
                        }
                        throw new GekkoException();
                    }
                    ts = new TimeSeries(this.ts.freqEnum, varHash);
                    this.ts.parentDatabank.AddVariable(ts);
                }
                this.ts.SetDirtyGhost(true, true);  //otherwise, an ASER x['a'] = ... will not register 'x' as a ghost.
                return new MetaTimeSeries(ts);
            }
            else
            {
                //y[2010] or y[-1]
                IVariable index = indexes[0];
                if (index.Type() == EVariableType.Val)
                {
                    int ival = O.GetInt(index);
                    if (ival >= 1900)
                    {
                        return new ScalarVal(this.ts.GetData(new GekkoTime(EFreq.Annual, ival + this.offset, 1)));
                    }
                    else
                    {
                        //typically ival numerically < 10 here                    
                        //return new MetaTimeSeries(this.ts, ival, this.bank, this.variable);
                        return new MetaTimeSeries(this.ts, ival + this.offset);
                        //10% faster, but maybe more error prone...
                        //this.offset = ival;
                        //return this;
                    }
                }
                else if (index.Type() == EVariableType.Date)
                {
                    return new ScalarVal(this.ts.GetData(((ScalarDate)index).date.Add(this.offset)));
                }
                else
                {                    
                    //should not be possible
                    G.Writeln2("*** ERROR: SERIES uses []-indexer with wrong variable type");
                    throw new GekkoException();
                }
            }            
        }        

        public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange)
        {
            G.Writeln2("*** ERROR: You are trying to use an [] index range on timeseries: " + this.ts.variableName + ".");            
            throw new GekkoException();
        }

        public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(GekkoSmpl t, IVariable index, IVariablesFilterRange indexRange)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange, IVariable index)
        {
            throw new GekkoException();
        }        

        public IVariable Negate(GekkoSmpl t)
        {            
            double val = O.GetVal(t, this);
            return new ScalarVal(-val);            
        }

        public void InjectAdd(GekkoSmpl t, IVariable x, IVariable y)
        {
            G.Writeln2("*** ERROR: error #734632321 regarding timeseries: " + this.ts.variableName + ".");            
            throw new GekkoException();
        }        

        public double GetVal(GekkoSmpl t)
        {
            //uuu
            return double.NaN;

            ////Asking a general timeless GetVal() from a timeseries (and not getting a val with a particular GekkoTime) is used in GENR, PRT etc. in an internal GekkoTime loop. Hence the use of Globals.globalGekkoTimeIterator_DO_NOT_ALTER.
            //if (t.IsNull())
            //{
            //    G.Writeln2("*** ERROR: You are trying to extract a single value from timeseries: " + this.ts.variableName + ".");
            //    G.Writeln("           Did you forget []-brackets to pick out an observation, for instance x[2020]?");
            //    throw new GekkoException();
            //}
            //return this.ts.GetData(t.Add(this.offset));
        }

        public string GetString()
        {
            G.Writeln2("*** ERROR: You are trying to extract STRING from timeseries: " + this.ts.variableName + ".");            
            throw new GekkoException();
        }

        public GekkoTime GetDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: You are trying to extract a DATE from a timeseries: " + this.ts.variableName + ".");            
            throw new GekkoException();
        }

        public List<string> GetList()
        {
            G.Writeln2("*** ERROR: You are trying to extract a LIST from timeseries: " + this.ts.variableName + ".");            
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.TimeSeries;
        }

        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(O.GetVal(t, this) + O.GetVal(t, x));
            }
            else if (x.Type() == EVariableType.Val)
            {
                return Operators.ValTimeSeries.Add((ScalarVal)x, this, t);
            }
            else if (x.Type() == EVariableType.String)
            {
                G.Writeln2("*** ERROR: You cannot add a timeseries and a string");
                throw new GekkoException();
            }
            else
            {
                G.Writeln2("*** ERROR: Type mismatch regarding add");
                throw new GekkoException();
            }
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(O.GetVal(t, this) - O.GetVal(t, x));
            }
            else if (x.Type() == EVariableType.Val)
            {
                return new ScalarVal(O.GetVal(t, this) - O.GetVal(t, x));
            }
            else
            {
                G.Writeln2("*** ERROR: Type mismatch regarding subtract");                
                throw new GekkoException();
            }
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(O.GetVal(t, this) * O.GetVal(t, x));
            }
            else if (x.Type() == EVariableType.Val)
            {
                return new ScalarVal(O.GetVal(t, this) * O.GetVal(t, x));
            }
            else
            {
                G.Writeln2("*** ERROR: Type mismatch regarding multiply");                
                throw new GekkoException();
            }
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(O.GetVal(t, this) / O.GetVal(t, x));
            }
            else if (x.Type() == EVariableType.Val)
            {
                return new ScalarVal(O.GetVal(t, this) / O.GetVal(t, x));
            }
            else
            {
                G.Writeln2("*** ERROR: Type mismatch regarding division");                
                throw new GekkoException();
            }
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(Math.Pow(O.GetVal(t, this), O.GetVal(t, x)));
            }
            else if (x.Type() == EVariableType.Val)
            {
                return new ScalarVal(Math.Pow(O.GetVal(t, this), O.GetVal(t, x)));
            }
            else
            {
                G.Writeln2("*** ERROR: Type mismatch regarding power function");                
                throw new GekkoException();
            }
        }
    }
}
