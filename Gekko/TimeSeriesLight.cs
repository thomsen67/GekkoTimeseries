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
        //public GekkoError ge = null;                
        public GekkoTime anchorPeriod;
        public int anchorPeriodPositionInArray;     

        public TimeSeriesLight()
        {

        }
        
        public TimeSeriesLight(GekkoSmpl smpl, TimeSeries ts)
        {
            if (Globals.timeSeriesLightShallowCopy)
            {
                this.isPointerToRealTimeseriesArray = true;
                int i1 = -12345;
                int i2 = -12345;
                //just a pointer to the real timeseries. The real timeseries will not change during the
                //lifetime of this object, so it is safe to consider the array fixed.
                //one benefit of this is that if we are out of bounds of the array, we just return a NaN.
                double[] dataArray = ts.GetDataSequence(out i1, out i2, smpl.t1, smpl.t2);                
                this.storage = dataArray;
                this.anchorPeriodPositionInArray = ts.anchorPeriodPositionInArray;
                this.anchorPeriod = new GekkoTime(ts.freq, ts.anchorSuperPeriod, ts.anchorSubPeriod);
            }
            else
            {
                //a full copy of the data
                int i1 = -12345;
                int i2 = -12345;
                double[] dataPointer = ts.GetDataSequence(out i1, out i2, smpl.t1, smpl.t2);
                this.storage = new double[i2 - i1 + 1];
                Array.Copy(dataPointer, i1, storage, 0, (i2 - i1 + 1));
                this.anchorPeriodPositionInArray = 0;
                this.anchorPeriod = smpl.t1;
            }
        }
        
        public IVariable Indexer(GekkoSmpl smpl, bool isLhs, params IVariable[] indexes)
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
                        TimeSeriesLight z = new Gekko.TimeSeriesLight();

                        z.isPointerToRealTimeseriesArray = x.isPointerToRealTimeseriesArray;
                        z.storage = x.storage;
                        z.anchorPeriod = x.anchorPeriod;
                        z.anchorPeriodPositionInArray = x.anchorPeriodPositionInArray + ival;


                        //// =================================== window test start ================================================
                        //int ix1; GekkoError ge;
                        //Check1Smpl(smpl, ival, x, out ix1, out ge);  //offset corresponding to lag
                        //if (ge != null)
                        //{
                        //    z.ge = ge;                            
                        //}
                        //else
                        //{
                        //    // =================================== window test end ===================================================

                        //    //lag or lead
                            
                        //    z.isPointerToRealTimeseriesArray = x.isPointerToRealTimeseriesArray;
                        //    z.storage = x.storage;
                        //    z.anchorPeriodPositionInArray = x.anchorPeriodPositionInArray;
                        //    z.anchorPeriod = x.anchorPeriod.Add(ival);


                        //    //double[] data = new double[this.storage.Length];
                        //    //if (ival < 0)  //lag
                        //    //{
                        //    //    int lags = -ival;
                        //    //    if (lags > this.storage.Length) lags = this.storage.Length;
                        //    //    for (int i = 0; i < lags; i++)
                        //    //    {
                        //    //        data[i] = double.NaN;
                        //    //    }
                        //    //    if (lags < this.storage.Length) Array.Copy(this.storage, 0, data, -ival, this.storage.Length - lags);
                        //    //    z.storage = data;                               

                        //    //}
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

       

        public IVariable Indexer(GekkoSmpl smpl, IVariablesFilterRange indexRange)
        {
            G.Writeln2("*** ERROR: You are trying to use an [] index range on timeseries");
            throw new GekkoException();
        }

        public IVariable Indexer(GekkoSmpl smpl, IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(GekkoSmpl smpl, IVariable index, IVariablesFilterRange indexRange)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(GekkoSmpl smpl, IVariablesFilterRange indexRange, IVariable index)
        {
            throw new GekkoException();
        }

        public IVariable Negate(GekkoSmpl smpl)
        {
            return null;
        }

        public void InjectAdd(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            G.Writeln2("*** ERROR: error #734632321 regarding timeseries");
            throw new GekkoException();
        }

        public double GetVal(GekkoSmpl smpl)
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

        public List<IVariable> GetList()
        {
            G.Writeln2("*** ERROR: You are trying to extract a LIST from timeseries");
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.TimeSeriesLight;
        }

        public IVariable Add(GekkoSmpl smpl, IVariable input)
        {
            switch(input.Type())
            {
                case EVariableType.TimeSeries:
                    {

                        //FIXME: IF TimeSeries, not light

                        TimeSeriesLight x = this;
                        TimeSeriesLight y = (TimeSeriesLight)input;
                        TimeSeriesLight z = new TimeSeriesLight();

                        int ix1, ix2; GekkoError ge; SpmlCheck(smpl, x, out ix1, out ix2, out ge);
                        if (ge != null) return ge;

                        int iy1 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t1, y.anchorPeriod, y.anchorPeriodPositionInArray);
                        int iy2 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t2, y.anchorPeriod, y.anchorPeriodPositionInArray);

                        if (!y.isPointerToRealTimeseriesArray)
                        {
                            if (iy1 < 0 || iy2 >= y.storage.Length)
                            {
                                return new GekkoError(Math.Max(0, -iy1), Math.Max(0, iy2 - y.storage.Length + 1));
                            }
                        }

                        //Now, if it is a small double[] data array (not pointing to real one), we know that it is not
                        //out of bounds. So if anything is out of bounds, it can be set to NaN.

                        int n = ix2 - ix1 + 1;  //same as iy2-iy1 or GekkoTime.Observations(smpl.t1, smpl.t2)

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

                        //if (x.anchorPeriod.freq != y.anchorPeriod.freq)
                        //{
                        //    G.Writeln2("*** ERROR: Frequency mismatch");
                        //    throw new GekkoException();
                        //}                        

                        ////1. two ts
                        ////2. one ts and one tsl
                        ////3. two tsl

                        ////1: use smpl, and use NaN if out of bounds. No uoverflow.
                        ////2: Periods overlap or no overlap.
                        ////3: smpl irrelevant. Periods overlap or no overlap. If not overlap -> uoverflow.


                        ////=================

                        ////These are the dates spanning the double[] arrays
                        //GekkoTime tx1 = TimeSeries.FromArrayIndexToGekkoTime(0, x.anchorPeriod, x.anchorPeriodPositionInArray);
                        //GekkoTime ty1 = TimeSeries.FromArrayIndexToGekkoTime(0, y.anchorPeriod, y.anchorPeriodPositionInArray);
                        //GekkoTime tx2 = TimeSeries.FromArrayIndexToGekkoTime(0 + x.storage.Length, x.anchorPeriod, x.anchorPeriodPositionInArray);
                        //GekkoTime ty2 = TimeSeries.FromArrayIndexToGekkoTime(0 + y.storage.Length, y.anchorPeriod, y.anchorPeriodPositionInArray);
                        //if (x.isPointerToRealTimeseriesArray)
                        //{
                        //    if (tx1.StrictlySmallerThan(smpl.t1)) tx1 = smpl.t1;
                        //    if (tx2.StrictlyLargerThan(smpl.t2)) tx2 = smpl.t2;
                        //}
                        //if (y.isPointerToRealTimeseriesArray)
                        //{
                        //    if (ty1.StrictlySmallerThan(smpl.t1)) ty1 = smpl.t1;
                        //    if (ty2.StrictlyLargerThan(smpl.t2)) ty2 = smpl.t2;
                        //}
                        //GekkoTime t1 = tx1; if (ty1.StrictlyLargerThan(tx1)) t1 = ty1;
                        //GekkoTime t2 = tx2; if (ty2.StrictlyLargerThan(tx2)) t2 = ty2;

                        ////=================

                        //if (t1.StrictlyLargerThan(t2))
                        //{
                        //    GekkoError gerr = new Gekko.GekkoError();
                        //    int underflow = Math.Max(0, GekkoTime.Observations(smpl.t1, t1) - 1);  //0 or larger
                        //    int overflow = Math.Max(0, GekkoTime.Observations(t2, smpl.t2) - 1);  //0 or larger
                        //    return gerr;
                        //}

                        //int ix1 = TimeSeries.FromGekkoTimeToArrayIndex(t1, x.anchorPeriod, x.anchorPeriodPositionInArray);
                        //int iy1 = TimeSeries.FromGekkoTimeToArrayIndex(t1, y.anchorPeriod, y.anchorPeriodPositionInArray);

                        //int n = GekkoTime.Observations(t1, t2);

                        //z.isPointerToRealTimeseriesArray = false;
                        //z.storage = new double[n];
                        //z.anchorPeriod = smpl.t1;
                        //z.anchorPeriodPositionInArray = 0;

                        //for (int i = 0; i < n; i++)
                        //{
                        //    double xVal = double.NaN;
                        //    double yVal = double.NaN;
                        //    int ix = ix1 + i;
                        //    int iy = iy1 + i;
                        //    if (ix >= 0 && ix < x.storage.Length) xVal = x.storage[ix];
                        //    if (iy >= 0 && iy < y.storage.Length) yVal = y.storage[iy];
                        //    z.storage[i] = xVal + yVal;
                        //}

                        //return z;
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

        public static void SpmlCheck(GekkoSmpl smpl, TimeSeriesLight x, out int ix1, out int ix2, out GekkoError ge)
        {
            ix1 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t1, x.anchorPeriod, x.anchorPeriodPositionInArray);
            ix2 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t2, x.anchorPeriod, x.anchorPeriodPositionInArray);
            ge = null;
            if (!x.isPointerToRealTimeseriesArray)
            {
                if (ix1 < 0 || ix2 >= x.storage.Length)
                {
                    ge = new GekkoError(Math.Max(0, -ix1), Math.Max(0, ix2 - x.storage.Length + 1));
                }
            }
        }

        //private static void Check1Smpl(IVariableHelper smpl, int offset, TimeSeriesLight x, out int ix1, out GekkoError ge)
        //{
        //    ix1 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t1.Add(offset), x.anchorPeriod, x.anchorPeriodPositionInArray);
        //    int ix2 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t2.Add(offset), x.anchorPeriod, x.anchorPeriodPositionInArray);
        //    int underflow = 0; int overflow = 0;
        //    UnderOverflow(x, ix1, ix2, ref underflow, ref overflow);
        //    ge = Program.CheckGekkoError(underflow, overflow);
        //}

        //private static void Check2Smpl(IVariableHelper smpl, int offset, TimeSeriesLight x, TimeSeriesLight y, out int ix1, out int iy1, out GekkoError ge)
        //{
        //    ix1 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t1.Add(offset), x.anchorPeriod, x.anchorPeriodPositionInArray);
        //    int ix2 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t2.Add(offset), x.anchorPeriod, x.anchorPeriodPositionInArray);
        //    iy1 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t1.Add(offset), y.anchorPeriod, y.anchorPeriodPositionInArray);
        //    int iy2 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t2.Add(offset), y.anchorPeriod, y.anchorPeriodPositionInArray);
        //    int underflow = 0; int overflow = 0;
        //    UnderOverflow(x, ix1, ix2, ref underflow, ref overflow);
        //    UnderOverflow(y, iy1, iy2, ref underflow, ref overflow);
        //    ge = Program.CheckGekkoError(underflow, overflow);
        //}

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

        public IVariable Subtract(GekkoSmpl smpl, IVariable x)
        {
            return null;
        }

        public IVariable Multiply(GekkoSmpl smpl, IVariable x)
        {
            return null;
        }

        public IVariable Divide(GekkoSmpl smpl, IVariable x)
        {
            return null;
        }

        public IVariable Power(GekkoSmpl smpl, IVariable x)
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
