using System;
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

        // ----------------------------------------------------
        // --------------object functions start----------------
        // ----------------------------------------------------

        public IVariable append(bool isLhs, GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method .append() not available for type " + G.GetTypeString(this));
            throw new GekkoException();
        }

        public IVariable extend(bool isLhs, GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method .extend() not available for type " + G.GetTypeString(this));
            throw new GekkoException();
        }

        // ----------------------------------------------------
        // --------------object functions end------------------
        // ----------------------------------------------------

        public double GetValOLD(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: Could not convert the DATE " + this.date + " directly into a VAL.");
            G.Writeln("           You may try the date() conversion function.");
            throw new GekkoException();            
        }
        public double GetVal(GekkoTime t)
        {
            return ConvertToVal();
        }

        public double ConvertToVal()
        {
            G.Writeln2("*** ERROR: Could not convert the DATE " + this.date + " directly into a VAL.");            
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            G.Writeln2("*** ERROR: Could not convert the DATE " + this.date + " directly into a STRING.");
            G.Writeln("           You may try the string() conversion function.");            
            throw new GekkoException();
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            //If the input is a date, the raw date is always returned here
            return this.date;
        }

        public List<IVariable> ConvertToList()
        {
            //See similar comment: #slkfhas
            G.Writeln2("*** ERROR: You are trying to convert/use the date " + this.date + " as a STRING item in a list");
            G.Writeln("           In LIST commands, you must for example use '2015q3' instead of 2015q3.");
            G.Writeln("           If you are using a DATE scalar %d, you may try to use string(%d) instead.");            
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.Date;
        }

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] index1)
        {
            G.Writeln2("*** ERROR: Cannot use []-indexer on DATE");
            throw new GekkoException();
        }
        
                

        public IVariable Negate(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: You cannot use minus on DATE");                
            throw new GekkoException();
        }

        //public void InjectAdd(GekkoSmpl t, IVariable x, IVariable y)
        //{
        //    G.Writeln2("*** ERROR: You cannot use add on DATE");                
        //    throw new GekkoException();
        //}

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
                        G.Writeln2("*** ERROR: Type error regarding add");                
                        throw new GekkoException();
                    }
            }
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
                        int obs = GekkoTime.Observations(((ScalarDate)x).date, this.date) - 1;
                        if (obs < 0)
                        {
                            G.Writeln();
                            G.Writeln("*** ERROR: Subtraction of two dates gave negative number of observations: " + obs);
                            throw new GekkoException();
                        }
                        return new ScalarVal(obs);
                    }
                    break;
                default:
                    {
                        G.Writeln2("*** ERROR: Type error regarding subtract");                
                        throw new GekkoException();
                    }
                    break;
            }
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: %x*%y (multiply) is not allowed if %x is a DATE scalar.");
            throw new GekkoException();
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: %x/%y (divide) is not allowed if %x is a DATE scalar.");
            throw new GekkoException();
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: %x^%y or %x**%y (power) is not allowed if %x is a DATE scalar.");
            throw new GekkoException();
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] dims)
        {
            G.Writeln2("*** ERROR: You cannot use an indexer [] on a DATE");
            throw new GekkoException();
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            return new ScalarDate(this.date);
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
