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

        public IVariable Indexer(IVariable index1, IVariable index2, GekkoTime t)
        {
            G.Writeln2("Timeseries cannot used with [i, j] indexer");
            throw new GekkoException();
        }

        public IVariable Indexer(IVariable index, GekkoTime t)
        {
            if (index.Type() == EVariableType.String)
            {
                string s = ((ScalarString)index)._string2;
                throw new GekkoException();  //FIXME
            }
            else if (index.Type() == EVariableType.Val)
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
                G.Writeln2("*** ERROR: Expected indexer to be DATE or VAL");
                throw new GekkoException();
            }

        }

        public IVariable Indexer(IVariablesFilterRange indexRange, GekkoTime t)
        {
            G.Writeln2("*** ERROR: You are trying to use an [] index range on timeseries: " + this.ts.variableName + ".");            
            throw new GekkoException();
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

        public IVariable Negate(GekkoTime t)
        {            
            double val = O.GetVal(this, t);
            return new ScalarVal(-val);            
        }

        public void InjectAdd(IVariable x, IVariable y, GekkoTime t)
        {
            G.Writeln2("*** ERROR: error #734632321 regarding timeseries: " + this.ts.variableName + ".");            
            throw new GekkoException();
        }        

        public double GetVal(GekkoTime t)
        {
            //Asking a general timeless GetVal() from a timeseries (and not getting a val with a particular GekkoTime) is used in GENR, PRT etc. in an internal GekkoTime loop. Hence the use of Globals.globalGekkoTimeIterator_DO_NOT_ALTER.
            if (t.IsNull())
            {
                G.Writeln2("*** ERROR: You are trying to extract a single value from timeseries: " + this.ts.variableName + ".");
                G.Writeln("           Did you forget []-brackets to pick out an observation, for instance x[2020]?");
                throw new GekkoException();
            }
            return this.ts.GetData(t.Add(this.offset));
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

        public IVariable Add(IVariable x, GekkoTime t)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(O.GetVal(this, t) + O.GetVal(x, t));
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

        public IVariable Subtract(IVariable x, GekkoTime t)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(O.GetVal(this, t) - O.GetVal(x, t));
            }
            else if (x.Type() == EVariableType.Val)
            {
                return new ScalarVal(O.GetVal(this, t) - O.GetVal(x, t));
            }
            else
            {
                G.Writeln2("*** ERROR: Type mismatch regarding subtract");                
                throw new GekkoException();
            }
        }

        public IVariable Multiply(IVariable x, GekkoTime t)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(O.GetVal(this, t) * O.GetVal(x, t));
            }
            else if (x.Type() == EVariableType.Val)
            {
                return new ScalarVal(O.GetVal(this, t) * O.GetVal(x, t));
            }
            else
            {
                G.Writeln2("*** ERROR: Type mismatch regarding multiply");                
                throw new GekkoException();
            }
        }

        public IVariable Divide(IVariable x, GekkoTime t)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(O.GetVal(this, t) / O.GetVal(x, t));
            }
            else if (x.Type() == EVariableType.Val)
            {
                return new ScalarVal(O.GetVal(this, t) / O.GetVal(x, t));
            }
            else
            {
                G.Writeln2("*** ERROR: Type mismatch regarding division");                
                throw new GekkoException();
            }
        }

        public IVariable Power(IVariable x, GekkoTime t)
        {
            if (x.Type() == EVariableType.TimeSeries)
            {                
                return new ScalarVal(Math.Pow(O.GetVal(this, t), O.GetVal(x, t)));
            }
            else if (x.Type() == EVariableType.Val)
            {
                return new ScalarVal(Math.Pow(O.GetVal(this, t), O.GetVal(x, t)));
            }
            else
            {
                G.Writeln2("*** ERROR: Type mismatch regarding power function");                
                throw new GekkoException();
            }
        }
    }
}
