﻿using System;
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

                        //lag or lead
                        TimeSeriesLight tsl = new Gekko.TimeSeriesLight();

                        if (Globals.timeSeriesLightShallowCopy)
                        {
                            //Indexer() is called with a smpl window. If start of smpl window is < 0 in array or end of smpl window is >= length in error,
                            //we have a problem.

                            tsl.isPointerToRealTimeseriesArray = this.isPointerToRealTimeseriesArray;
                            tsl.storage = this.storage;
                            tsl.anchorPeriodPositionInArray = this.anchorPeriodPositionInArray - ival;
                            tsl.anchorPeriod = this.anchorPeriod;             
                        }
                        else
                        {
                            double[] data = new double[this.storage.Length];
                            if (ival < 0)  //lag
                            {
                                int lags = -ival;
                                if (lags > this.storage.Length) lags = this.storage.Length;
                                for (int i = 0; i < lags; i++)
                                {
                                    data[i] = double.NaN;
                                }
                                if (lags < this.storage.Length) Array.Copy(this.storage, 0, data, -ival, this.storage.Length - lags);
                                tsl.storage = data;                               

                            }
                            else  //lead
                            {
                                throw new GekkoException();
                            }                        
                        }
                        return tsl;
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

        public IVariable Add(IVariableHelper smpl, IVariable x)
        {
            TimeSeriesLight tsl = x as TimeSeriesLight;
                        
            if (tsl != null)
            {
                int startIndex1 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t1, this.anchorPeriod, this.anchorPeriodPositionInArray);
                int startIndex2 = TimeSeries.FromGekkoTimeToArrayIndex(smpl.t2, this.anchorPeriod, this.anchorPeriodPositionInArray);
                int n = GekkoTime.Observations(smpl.t1, smpl.t2);

                //0 1 2 = 3 obs
                int overflow1 = startIndex1 + n - 1 - this.storage.Length;
                int overflow2 = startIndex2 + n - 1 - tsl.storage.Length;
                if (startIndex1 < 0 || startIndex2 < 0 || overflow1 > 0 || overflow2 > 0)
                {
                    GekkoError ge = new Gekko.GekkoError();
                    if (startIndex1 < 0 || startIndex2 < 0) ge.underflow = Math.Max(-startIndex1, -startIndex2);
                    if (overflow1 > 0 || overflow2 > 0) ge.overflow = Math.Max(overflow1, overflow2);
                    return ge;
                }

                TimeSeriesLight tslResult = new TimeSeriesLight();
                tslResult.storage = new double[n];
                tslResult.anchorPeriod = smpl.t1;
                tslResult.anchorPeriodPositionInArray = 0;

                for (int i = 0; i < n; i++)
                {
                    int i1 = startIndex1 + i;
                    int i2 = startIndex2 + i;
                    tslResult.storage[i] = this.storage[i1] + tsl.storage[i2];
                }

                return tslResult;
            }
            G.Writeln2("*** ERROR: Unknown type");
            throw new GekkoException();
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
