using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    class TimeSeriesLight : IVariable
    {
        public double[] storage = null;
        //public GekkoTime t1 = Globals.tNull;
        //public GekkoTime t2 = Globals.tNull;

        public TimeSeriesLight()
        {

        }

        public TimeSeriesLight(TimeSeries ts, GekkoTime gt1, GekkoTime gt2)
        {
            int i1 = -12345;
            int i2 = -12345;
            double[] dataPointer = ts.GetDataSequence(out i1, out i2, gt1, gt2);
            storage = new double[i2 - i1 + 1];
            Array.Copy(dataPointer, i1, storage, 0, (i2 - i1 + 1));
        }

        public IVariable Indexer(GekkoTime t, bool isLhs, params IVariable[] indexes)
        {
            if (indexes.Length > 0 && indexes[0].Type() == EVariableType.String)
            {
                return null;
            }
            else
            {
                //y[2010] or y[-1]
                IVariable index = indexes[0];
                if (index.Type() == EVariableType.Val)
                {
                    //TODO
                    //TODO
                    //TODO
                    //Introduce broken lags here??
                    int ival = O.GetInt(index);
                    if (ival >= 1900)
                    {
                        //return new ScalarVal(this.ts.GetData(new GekkoTime(EFreq.Annual, ival + this.offset, 1)));
                        return null;
                    }
                    else
                    {
                        //lag or lead
                        if (ival == 0) return this;  //no lag, x[-0] or x[0]
                        else
                        {                            
                            double[] data = new double[this.storage.Length];
                            if (ival < 0)  //lag
                            {

                            }
                        }
                    }
                }
                else if (index.Type() == EVariableType.Date)
                {
                    //return new ScalarVal(this.ts.GetData(((ScalarDate)index).date.Add(this.offset)));
                    return null;
                }
                return null;
            }
        }

        public IVariable Indexer(IVariablesFilterRange indexRange, GekkoTime t)
        {
            G.Writeln2("*** ERROR: You are trying to use an [] index range on timeseries");
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
            return null;
        }

        public void InjectAdd(IVariable x, IVariable y, GekkoTime t)
        {
            G.Writeln2("*** ERROR: error #734632321 regarding timeseries");
            throw new GekkoException();
        }

        public double GetVal(GekkoTime t)
        {
            return double.NaN;
        }

        public string GetString()
        {
            G.Writeln2("*** ERROR: You are trying to extract STRING from timeseries");
            throw new GekkoException();
        }

        public GekkoTime GetDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: You are trying to extract a DATE from a timeseries");
            throw new GekkoException();
        }

        public List<string> GetList()
        {
            G.Writeln2("*** ERROR: You are trying to extract a LIST from timeseries");
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.TimeSeries;
        }

        public IVariable Add(IVariable x, GekkoTime t)
        {
            TimeSeriesLight tsl = x as TimeSeriesLight;
            if (tsl != null)
            {
                if (tsl.storage.Length != this.storage.Length)
                {
                    G.Writeln2("*** ERROR: #0985093284");
                    throw new GekkoException();
                }
                TimeSeriesLight tsl2 = new TimeSeriesLight();
                tsl2.storage = new double[this.storage.Length];
                for (int i = 0; i < this.storage.Length; i++)
                {
                    tsl2.storage[i] = tsl.storage[i] + this.storage[i];
                }
                return tsl2;
            }
            G.Writeln2("*** ERROR: Unknown type");
            throw new GekkoException();
        }

        public IVariable Subtract(IVariable x, GekkoTime t)
        {
            return null;
        }

        public IVariable Multiply(IVariable x, GekkoTime t)
        {
            return null;
        }

        public IVariable Divide(IVariable x, GekkoTime t)
        {
            return null;
        }

        public IVariable Power(IVariable x, GekkoTime t)
        {
            return null;
        }
    }
}
