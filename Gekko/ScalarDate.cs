﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class ScalarDate : IVariable
    {
        [ProtoMember(1)]
        public GekkoTime date;

        private ScalarDate()
        {
            //only because protobuf needs it, not for outside use
        }

        public ScalarDate(GekkoTime gt)
        {
            date = gt;
        }        

        public double GetValOLD(GekkoSmpl t)
        {
            new Error("Could not convert the DATE " + this.date + " directly into a VAL. You may try the date() conversion function.");
            return double.NaN;
            //throw new GekkoException();            
        }
        public double GetVal(GekkoTime t)
        {
            return ConvertToVal();
        }

        public double ConvertToVal()
        {
            new Error("Could not convert the DATE " + this.date + " directly into a VAL."); return double.NaN;
            //throw new GekkoException();
        }

        public string ConvertToString()
        {
            new Error("Could not convert the DATE " + this.date + " directly into a STRING. You may try the string() conversion function."); return null;

            //throw new GekkoException();
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            //If the input is a date, the raw date is always returned here
            return this.date;
        }

        public List<IVariable> ConvertToList()
        {
            //See similar comment: #slkfhas
            new Error("You are trying to convert/use the date " + this.date + " as a STRING item in a list. In LIST commands, you must for example use '2015q3' instead of 2015q3. If you are using a DATE scalar %d, you may try to use string(%d) instead."); return null;


            //throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.Date;
        }

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] index1)
        {
            new Error("Cannot use []-indexer on DATE"); return null;
            //throw new GekkoException();
        }



        public IVariable Negate(GekkoSmpl t)
        {
            if (this.date.freq == EFreq.U)
            {
                GekkoTime gt = this.date;
                return new ScalarDate(new GekkoTime(EFreq.U, -gt.super, 1));
            }
            else
            {
                new Error("You cannot use minus on date: " + this.date.ToString()); return null;
                //throw new GekkoException();
            }
        }
        

        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.Val:
                    {
                        return Operators.DateVal.Add(this, (ScalarVal)x);
                    }
                case EVariableType.String:
                    {
                        return Operators.StringDate.Add((ScalarString)x, this, true);
                    }                  
                default:
                    {
                        new Error("Type error regarding add"); return null;
                        //throw new GekkoException();
                    }
            }
        }

        public IVariable Concat(GekkoSmpl t, IVariable x)
        {
            new Error("Type error regarding concat and DATE"); return null;
            //throw new GekkoException();
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.Val:
                    {
                        return Operators.DateVal.Subtract(this, (ScalarVal)x);
                    }
                    break;
                case EVariableType.Date:
                    {
                        int obs = GekkoTime.Observations(((ScalarDate)x).date, this.date) - 1; //can be negative, we allow this                        
                        return new ScalarVal(obs);
                    }
                    break;
                default:
                    {
                        new Error("Type error regarding subtract"); return null;
                        //throw new GekkoException();
                    }
                    break;
            }
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            new Error("%x*%y (multiply) is not allowed if %x is a DATE scalar."); return null;
            //throw new GekkoException();
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            new Error("%x/%y (divide) is not allowed if %x is a DATE scalar."); return null;
            //throw new GekkoException();
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            new Error("%x^%y or %x**%y (power) is not allowed if %x is a DATE scalar."); return null;
            //throw new GekkoException();
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            new Error("You cannot use an indexer [] on a DATE"); return;
            //throw new GekkoException();
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            return new ScalarDate(this.date);
        }

        public void DeepTrim()
        {
            //do nothing, nothing to trim
        }

        public void DeepCleanup(TwoInts yearMinMax)
        {
            //do nothing
        }
    }
}
