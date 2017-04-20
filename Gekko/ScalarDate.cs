using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public class ScalarDate : IVariable
    {
        public GekkoTime date;

        public ScalarDate(GekkoTime gt)
        {
            date = gt;
        }

        public double GetVal(GekkoTime t)
        {
            G.Writeln2("*** ERROR: Could not convert the DATE " + this.date + " directly into a VAL.");
            G.Writeln("           You may try the date() conversion function.");
            throw new GekkoException();            
        }

        public string GetString()
        {
            G.Writeln2("*** ERROR: Could not convert the DATE " + this.date + " directly into a STRING.");
            G.Writeln("           You may try the string() conversion function.");            
            throw new GekkoException();
        }

        public GekkoTime GetDate(O.GetDateChoices c)
        {
            //If the input is a date, the raw date is always returned here
            return this.date;
        }

        public List<string> GetList()
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

        public IVariable Indexer(GekkoTime t, bool isLhs, params IVariable[] index1)
        {
            G.Writeln2("*** ERROR: Cannot use []-indexer on DATE");
            throw new GekkoException();
        }
        
        public IVariable Indexer(IVariablesFilterRange indexRange, GekkoTime t)
        {
            G.Writeln2("*** ERROR: Cannot use []-indexer on DATE");
            throw new GekkoException();
        }

        public IVariable Indexer(IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2, GekkoTime t)
        {
            G.Writeln2("*** ERROR: Cannot use []-indexer on DATE");
            throw new GekkoException();
        }

        public IVariable Indexer(IVariable index, IVariablesFilterRange indexRange, GekkoTime t)
        {
            G.Writeln2("*** ERROR: Cannot use []-indexer on DATE");
            throw new GekkoException();
        }

        public IVariable Indexer(IVariablesFilterRange indexRange, IVariable index, GekkoTime t)
        {
            G.Writeln2("*** ERROR: Cannot use []-indexer on DATE");
            throw new GekkoException();
        }        

        public IVariable Negate(GekkoTime t)
        {
            G.Writeln2("*** ERROR: You cannot use minus on DATE");                
            throw new GekkoException();
        }

        public void InjectAdd(IVariable x, IVariable y, GekkoTime t)
        {
            G.Writeln2("*** ERROR: You cannot use add on DATE");                
            throw new GekkoException();
        }

        public IVariable Add(IVariable x, GekkoTime t)
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

        public IVariable Subtract(IVariable x, GekkoTime t)
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

        public IVariable Multiply(IVariable x, GekkoTime t)
        {
            G.Writeln2("*** ERROR: %x*%y (multiply) is not allowed if %x is a DATE scalar.");
            throw new GekkoException();
        }

        public IVariable Divide(IVariable x, GekkoTime t)
        {
            G.Writeln2("*** ERROR: %x/%y (divide) is not allowed if %x is a DATE scalar.");
            throw new GekkoException();
        }

        public IVariable Power(IVariable x, GekkoTime t)
        {
            G.Writeln2("*** ERROR: %x^%y or %x**%y (power) is not allowed if %x is a DATE scalar.");
            throw new GekkoException();
        }
    }
}
