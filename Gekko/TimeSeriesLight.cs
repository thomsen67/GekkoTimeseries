using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public class TimeSeriesLight : IVariable
    {
        public bool isPointerToRealTimeseriesArray = false;
        public double[] storage = null;
        //public int offset = -12345;  //where the data forresponding to anchorPeriod and anchorSubPeriod starts
        
        public GekkoTime anchorPeriod;
        public int anchorPeriodPositionInArray;        

        public TimeSeriesLight()
        {

        }

        public TimeSeriesLight(TimeSeries ts, GekkoTime gt1, GekkoTime gt2, bool shallow)
        {
            if (Globals.timeSeriesLightShallowCopy)
            {
                this.isPointerToRealTimeseriesArray = true;
                int i1 = -12345;
                int i2 = -12345;
                //just a pointer to the real timeseries. The real timeseries will not change during the
                //lifetime of this object, so it is safe to consider the array fixed.
                //one benefit of this is that if we are out of bounds of the array, we just return a NaN.
                double[] dataArray = ts.GetDataSequence(out i1, out i2, gt1, gt2);
                
                this.storage = dataArray;
                this.anchorPeriodPositionInArray = ts.anchorPeriodPositionInArray;
                this.anchorPeriod = new GekkoTime(ts.freqEnum, ts.anchorSuperPeriod, ts.anchorSubPeriod);
            }
            else
            {
                //a full copy of the data
                int i1 = -12345;
                int i2 = -12345;
                double[] dataPointer = ts.GetDataSequence(out i1, out i2, gt1, gt2);
                this.storage = new double[i2 - i1 + 1];
                Array.Copy(dataPointer, i1, storage, 0, (i2 - i1 + 1));
                this.anchorPeriodPositionInArray = 0;
                this.anchorPeriod = gt1;
            }
        }

        public IVariable Indexer(IVariableHelper smpl, bool isLhs, params IVariable[] indexes)
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
                        if (ival == 0) return this;  //no lag, x[-0] or x[0]  

                        TimeSeriesLight x = this;

                        // =================================== window test start ================================================
                        int ix1; GekkoError ge;
                        Check1Smpl(smpl, ival, x, out ix1, out ge);  //offset corresponding to lag
                        if (ge != null) return ge;
                        // =================================== window test end ===================================================
                        
                        //lag or lead
                        TimeSeriesLight z = new Gekko.TimeSeriesLight();
                        z.isPointerToRealTimeseriesArray = x.isPointerToRealTimeseriesArray;
                        z.storage = x.storage;
                        z.anchorPeriodPositionInArray = x.anchorPeriodPositionInArray;
                        z.anchorPeriod = x.anchorPeriod.Add(ival);


                        //double[] data = new double[this.storage.Length];
                        //if (ival < 0)  //lag
                        //{
                        //    int lags = -ival;
                        //    if (lags > this.storage.Length) lags = this.storage.Length;
                        //    for (int i = 0; i < lags; i++)
                        //    {
                        //        data[i] = double.NaN;
                        //    }
                        //    if (lags < this.storage.Length) Array.Copy(this.storage, 0, data, -ival, this.storage.Length - lags);
                        //    z.storage = data;                               

                        //}

                        return z;
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

       

        public IVariable Indexer(IVariableHelper smpl, IVariablesFilterRange indexRange)
        {
            G.Writeln2("*** ERROR: You are trying to use an [] index range on timeseries");
            throw new GekkoException();
        }

        public IVariable Indexer(IVariableHelper smpl, IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariableHelper smpl, IVariable index, IVariablesFilterRange indexRange)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariableHelper smpl, IVariablesFilterRange indexRange, IVariable index)
        {
            throw new GekkoException();
        }

        public IVariable Negate(IVariableHelper smpl)
        {
            return null;
        }

        public void InjectAdd(IVariableHelper smpl, IVariable x, IVariable y)
        {
            G.Writeln2("*** ERROR: error #734632321 regarding timeseries");
            throw new GekkoException();
        }

        public double GetVal(IVariableHelper smpl)
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

        public IVariable Add(IVariableHelper smpl, IVariable input)
        {
            switch(input.Type())
            {
                case EVariableType.TimeSeries:
                    {
                        TimeSeriesLight x = this;
                        TimeSeriesLight y = (TimeSeriesLight)input;

                        // =================================== window test start ================================================
                        int ix1, iy1; GekkoError ge;
                        Check2Smpl(smpl, 0, x, y, out ix1, out iy1, out ge);  //no offset
                        if (ge != null) return ge;
                        // =================================== window test end== ================================================

                        int n = GekkoTime.Observations(smpl.t1, smpl.t2);

                        TimeSeriesLight z = new TimeSeriesLight();
                        z.isPointerToRealTimeseriesArray = false;
                        z.storage = new double[n];
                        z.anchorPeriod = smpl.t1;
                        z.anchorPeriodPositionInArray = 0;

                        for (int i = 0; i < n; i++)
                        {
                            double xVal = double.NaN;
                            double yVal = double.NaN;
                            int ix = ix1 + i;
                            int iy = iy1 + i;
                            if (ix >= 0 && ix < x.storage.Length) xVal = x.storage[ix];
                            if (iy >= 0 && iy < y.storage.Length) yVal = y.storage[iy];
                            z.storage[i] = xVal + yVal;
                        }

                        return z;
                    }
                    break;
                default:
                    {
                        G.Writeln2("*** ERROR: Cannot add SERIES and " + (input.Type().ToString().ToUpper()));
                        throw new GekkoException();
                    }
                    break;
            }           
        }

        private static void Check1Smpl(IVariableHelper smpl, int offset, TimeSeriesLight x, out int ix1, out GekkoError ge)
        {
            ix1 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t1.Add(offset), x.anchorPeriod, x.anchorPeriodPositionInArray);
            int ix2 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t2.Add(offset), x.anchorPeriod, x.anchorPeriodPositionInArray);
            int underflow = 0; int overflow = 0;
            UnderOverflow(x, ix1, ix2, ref underflow, ref overflow);
            ge = Program.CheckGekkoError(underflow, overflow);
        }

        private static void Check2Smpl(IVariableHelper smpl, int offset, TimeSeriesLight x, TimeSeriesLight y, out int ix1, out int iy1, out GekkoError ge)
        {
            ix1 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t1.Add(offset), x.anchorPeriod, x.anchorPeriodPositionInArray);
            int ix2 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t2.Add(offset), x.anchorPeriod, x.anchorPeriodPositionInArray);
            iy1 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t1.Add(offset), y.anchorPeriod, y.anchorPeriodPositionInArray);
            int iy2 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t2.Add(offset), y.anchorPeriod, y.anchorPeriodPositionInArray);
            int underflow = 0; int overflow = 0;
            UnderOverflow(x, ix1, ix2, ref underflow, ref overflow);
            UnderOverflow(y, iy1, iy2, ref underflow, ref overflow);
            ge = Program.CheckGekkoError(underflow, overflow);
        }

        private static void UnderOverflow(TimeSeriesLight x, int xStartIndex, int xEndIndex, ref int underflow, ref int overflow)
        {
            if (!x.isPointerToRealTimeseriesArray)
            {
                if (xStartIndex < 0)
                {
                    underflow = Math.Max(underflow, -xStartIndex);  //now underflow is 1 or more
                }
                if (xEndIndex - x.storage.Length + 1 > 0)
                {
                    overflow = Math.Max(overflow, xEndIndex - x.storage.Length + 1);  //now overflow is 1 or more                        
                }
            }
        }

        public IVariable Subtract(IVariableHelper smpl, IVariable x)
        {
            return null;
        }

        public IVariable Multiply(IVariableHelper smpl, IVariable x)
        {
            return null;
        }

        public IVariable Divide(IVariableHelper smpl, IVariable x)
        {
            return null;
        }

        public IVariable Power(IVariableHelper smpl, IVariable x)
        {
            return null;
        }

        public double GetData(GekkoTime t)
        {
            int index = GetArrayIndex(t);
            if (index < 0 || index >= this.storage.Length)
            {
                G.Writeln2("*** ERROR: Out of bounds");
                throw new GekkoException();
            }
            else
            {
                return this.storage[index];
            }
        }

        private int GetArrayIndex(GekkoTime gt)
        {
            int rv = TimeSeries.FromGekkoTimeToArrayIndex(gt, this.anchorPeriod, this.anchorPeriodPositionInArray);
            return rv;
        }
    }
}
