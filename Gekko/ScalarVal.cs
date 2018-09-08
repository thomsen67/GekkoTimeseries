using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class ScalarVal : IVariable
    {
        [ProtoMember(1)]
        public double val;
        public bool hasRepStar = false;  //to indicate for instance x = 1, 2 rep 2, 3 rep *; ---> 1, 2, 2, 3, where 3 has this flag set.

        private ScalarVal()
        {
            //only because protobuf needs it, not for outside use
        }

        public ScalarVal(double d)
        {
            val = d;
        }

        public IVariable append(bool isLhs, GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method .append() not available for type " + G.GetTypeString(this));
            throw new GekkoException();
            List<string> xx = null;
        }

        public IVariable extend(bool isLhs, GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method .extend() not available for type " + G.GetTypeString(this));
            throw new GekkoException();
        }

        public double GetValOLD(GekkoSmpl t)
        {
            return this.val;
        }

        public double GetVal(GekkoTime t)
        {
            return this.val;
        }

        public double ConvertToVal()
        {
            return this.val;
        }

        public string ConvertToString()
        {
            G.Writeln2("*** ERROR: Could not convert the VAL " + this.val + " directly into STRING.");
            G.Writeln("           You may try the string() conversion function.");            
            throw new GekkoException();
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            GekkoTime gt = GekkoTime.tNull;
            int intValue = O.ConvertToInt(this);  //will issue error if the VAL is not an integer            
            if (c == O.GetDateChoices.Strict || (c != O.GetDateChoices.Strict && (Program.options.freq == EFreq.A || Program.options.freq == EFreq.U)))
            {
                if (Program.options.freq == EFreq.U)
                {                    
                    gt = new GekkoTime(EFreq.U, intValue, 1); //here, the context matters!
                }
                else
                {
                    int year = G.findYear(intValue);  //error if the year is crazy
                    gt = new GekkoTime(EFreq.A, year, 1);  //for a, q, m
                    //so date d = 2000 in freq=m will not turn this into 2000m1 or 2000m12
                }
            }
            else
            {
                //that is, FlexibleStart or FlexibleEnd
                //For Annual and Undated, this has been handled above
                //typically for TIME 2000 2010 or SERIES<2000 2010> which are turned into
                //for instance 2000m1 to 2000m12.
                int year = G.findYear(intValue);  //error is the year is crazy
                if (Program.options.freq == EFreq.Q)
                {                    
                    int sub = 1;
                    if (c == O.GetDateChoices.FlexibleStart) sub = 1;
                    else if (c == O.GetDateChoices.FlexibleEnd) sub = 4;
                    gt = new GekkoTime(EFreq.Q, year, sub);
                }
                else if (Program.options.freq == EFreq.M)
                {                    
                    int sub = 1;
                    if (c == O.GetDateChoices.FlexibleStart) sub = 1;
                    else if (c == O.GetDateChoices.FlexibleEnd) sub = 12;
                    gt = new GekkoTime(EFreq.M, year, sub);
                }
            }            
            return gt;
        }        

        public IVariable Indexer(GekkoSmpl t, params IVariable[] indexes)
        {
            G.Writeln2("*** ERROR: Cannot use []-indexer on VAL");
            throw new GekkoException();
        }
        
                

        public List<IVariable> ConvertToList()
        {
            //See similar comment: #slkfhas
            G.Writeln2("*** ERROR: You are trying to convert/use the value " + this.val + " as a STRING/NAME item in a list");
            G.Writeln("           In LIST commands, you must for example use '5' instead of 5, and '01' instead of 01.");
            G.Writeln("           Alternatively, for lists with such elements, you can use the LIST<direct> option.");
            G.Writeln("           If you are using a VAL scalar %v, you may try to use string(%v) instead.");
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.Val;
        }
        
        public IVariable Negate(GekkoSmpl t)
        {            
            return new ScalarVal(-this.val);
        }        

        //public void InjectAdd(GekkoSmpl t, IVariable x, IVariable y)
        //{
        //    if (x.Type() == EVariableType.Val)
        //    {
        //        if (y.Type() == EVariableType.Val)
        //        {
        //            //Probably good to have this case first, to speed up the (VAL, VAL) combination

        //            //We have to inject the value into the object. If we were to return a new object (or use the Add()
        //            //method), the Dictionary holding the %-names would point to an old object with an old value.
        //            //And this way, we also save 1 object creation which is good for performance.
        //            this.val = ((ScalarVal)x).val + ((ScalarVal)y).val;
        //        }
        //        else if (y.Type() == EVariableType.Date)
        //        {
        //        }
        //        else if (y.Type() == EVariableType.String)
        //        {
        //        }
        //    }
        //    else if (x.Type() == EVariableType.Date)
        //    {
        //        G.Writeln("*** ERROR: Cannot convert from date to value");
        //        throw new GekkoException();
        //    }
        //    else if (x.Type() == EVariableType.String)
        //    {
        //        G.Writeln("*** ERROR: Cannot convert from string to value");
        //        throw new GekkoException();
        //    }
        //    else
        //    {
        //        G.Writeln("*** ERROR: Internal Gekko error #98734543");
        //        throw new GekkoException();
        //    }
        //}

        public IVariable Add(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;
            switch (input.Type())
            {
                case EVariableType.Val:
                    {
                        //Strangely, using a simple object pool for these objects does not do any speedup (maybe from 14.5s to 14.4s on one case).
                        //Apparently, small scalar objects like these are quickly created and swept. They probably stay in
                        //the first generation memory, and not much fragmentation occurs.
                        //All in all, it is pretty impressive from C#, and we avoid object pooling complexities and other horrors.                        
                        return new ScalarVal(this.val + ((ScalarVal)input).val);

                    }
                case EVariableType.Series:
                    {
                        //-------------------------------------
                        //x1 = VAL (this)
                        //x2 = SERIES (input)
                        //-------------------------------------
                        Series x2_series = (Series)input;
                        double x1_val = ((ScalarVal)this).val;
                        Func<double, double, double> a = Globals.arithmentics[1];  //(x1, x2) => x2 + x1;  //NOTE: SWAP
                        Series  rv_series = Series.ArithmeticsSeriesVal(smpl, x2_series, x1_val, a);
                        return rv_series;
                    }
                case EVariableType.String:
                    {
                        return Operators.StringVal.Add((ScalarString)input, this, true);
                    }
                case EVariableType.Matrix:
                    {
                        G.Writeln2("*** ERROR: You cannot add a MATRIX and a SCALAR.");
                        throw new GekkoException();
                    }
                default:
                    {
                        G.Writeln2("*** ERROR: Memory variable conversion error.");
                        throw new GekkoException();
                    }
            }
        }

        public IVariable Subtract(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;
            switch (input.Type())
            {
                case EVariableType.Val:
                    {                        
                        return new ScalarVal(this.val - ((ScalarVal)input).val);
                    }
                case EVariableType.Series:
                    {
                        //-------------------------------------
                        //x1 = VAL (this)
                        //x2 = SERIES (input)
                        //-------------------------------------
                        Series x2_series = (Series)input;
                        double x1_val = ((ScalarVal)this).val;
                        Func<double, double, double> a = Globals.arithmentics[3]; //(x1, x2) => x2 - x1;  //NOTE: SWAP
                        Series rv_series = Series.ArithmeticsSeriesVal(smpl, x2_series, x1_val, a);
                        return rv_series;
                    }
                default:
                    {
                        G.Writeln2("*** ERROR: Memory variable conversion error.");
                        throw new GekkoException();
                    }
            }
        }

        public IVariable Multiply(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;
            switch (input.Type())
            {
                case EVariableType.Val:
                    {
                        return new ScalarVal(this.val * ((ScalarVal)input).val);
                    }
                case EVariableType.Series:
                    {
                        //-------------------------------------
                        //x1 = VAL (this)
                        //x2 = SERIES (input)
                        //-------------------------------------
                        Series x2_series = (Series)input;
                        double x1_val = ((ScalarVal)this).val;
                        Func<double, double, double> a = Globals.arithmentics[5]; //(x1, x2) => x2 * x1;  //NOTE: SWAP
                        Series rv_series = Series.ArithmeticsSeriesVal(smpl, x2_series, x1_val, a);
                        return rv_series;
                    }
                case EVariableType.Matrix:
                    {
                        //This is allowed in AREMOS, too
                        double[,] a = O.ConvertToMatrix(input).data;
                        double b = O.ConvertToVal(this);
                        int m = a.GetLength(0);
                        int k = a.GetLength(1);
                        double[,] c = O.MultiplyMatrixScalar(a, b, m, k);
                        Matrix z = new Matrix();
                        z.data = c;
                        return z;
                    }                
                default:
                    {
                        G.Writeln2("*** ERROR: Memory variable conversion error.");
                        throw new GekkoException();
                    }
            }
        }

        public IVariable Divide(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;
            switch (input.Type())
            {
                case EVariableType.Val:
                    {
                        return new ScalarVal(this.val / ((ScalarVal)input).val);
                    }
                case EVariableType.Series:
                    {
                        //-------------------------------------
                        //x1 = VAL (this)
                        //x2 = SERIES (input)
                        //-------------------------------------
                        Series x2_series = (Series)input;
                        double x1_val = ((ScalarVal)this).val;
                        Func<double, double, double> a = Globals.arithmentics[7]; //(x1, x2) => x2 / x1;  //NOTE: SWAP
                        Series rv_series = Series.ArithmeticsSeriesVal(smpl, x2_series, x1_val, a);
                        return rv_series;
                    }
                default:
                    {
                        G.Writeln2("*** ERROR: Memory variable conversion error.");
                        throw new GekkoException();
                    }
            }
        }

        public IVariable Power(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;
            switch (input.Type())
            {
                case EVariableType.Val:
                    {
                        return new ScalarVal(Math.Pow(this.val, ((ScalarVal)input).val));
                    }
                case EVariableType.Series:
                    {
                        //-------------------------------------
                        //x1 = VAL (this)
                        //x2 = SERIES (input)
                        //-------------------------------------
                        Series x2_series = (Series)input;
                        double x1_val = ((ScalarVal)this).val;
                        Func<double, double, double> a = Globals.arithmentics[9]; //(x1, x2) => Math.Pow(x2, x1);  //NOTE: SWAP
                        Series rv_series = Series.ArithmeticsSeriesVal(smpl, x2_series, x1_val, a);
                        return rv_series;
                    }
                default:
                    {
                        G.Writeln2("*** ERROR: Memory variable conversion error.");
                        throw new GekkoException();
                    }
            }
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] dims)
        {
            G.Writeln2("*** ERROR: You cannot use an indexer [] on a VAL");
            throw new GekkoException();
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            return new ScalarVal(this.val);
        }

        public void DeepTrim()
        {
            //do nothing, nothing to trim
        }

        public void DeepCleanup()
        {
            //do nothing
        }

    }
}
