using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    class GekkoNull : IVariable
    {
        public static GekkoNull gekkoNull = new GekkoNull();
        
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
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public double GetVal(GekkoTime t)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public double ConvertToVal()
        {
            return double.NaN;
        }

        public string ConvertToString()   //see also #9785278992347
        {
            return null;  //or should it be ""?
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public List<IVariable> ConvertToList()
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.Null;
        }

        public IVariable Negate(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }
                        
        public IVariable Add(GekkoSmpl smpl, IVariable input)
        {
            switch (input.Type())
            {
                case EVariableType.String:
                    {
                        //we allow null + string = string, see #9785278992347
                        return new ScalarString(((ScalarString)input).string2);
                    }
                default:
                    {
                        G.Writeln2("*** ERROR: Invalid operation on null/empty value");
                        throw new GekkoException();
                    }
            }            
        }

        public IVariable Subtract(GekkoSmpl smpl, IVariable input)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public IVariable Concat(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.String:
                    {
                        //we allow null + string = string, see #9785278992347
                        return new ScalarString(((ScalarString)x).string2);
                    }
                default:
                    {
                        G.Writeln2("*** ERROR: Invalid operation on null/empty value");
                        throw new GekkoException();
                    }
            }            
        }

        public IVariable Multiply(GekkoSmpl smpl, IVariable input)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public IVariable Divide(GekkoSmpl smpl, IVariable input)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public IVariable Power(GekkoSmpl smpl, IVariable input)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            return gekkoNull;
        }

        public void DeepTrim()
        {
            return;
        }

        public void DeepCleanup()
        {
            return;
        }

    }
}
