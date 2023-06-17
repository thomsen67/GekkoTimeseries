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
        
        public double GetValOLD(GekkoSmpl t)
        {
            new Error("Invalid operation on null/empty value"); return double.NaN;
        }

        public double GetVal(GekkoTime t)
        {
            new Error("Invalid operation on null/empty value"); return double.NaN;
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
            new Error("Invalid operation on null/empty value"); return GekkoTime.tNull;
        }

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public List<IVariable> ConvertToList()
        {
            new Error("Invalid operation on null/empty value"); return null;
        }

        public EVariableType Type()
        {
            return EVariableType.Null;
        }

        public IVariable Negate(GekkoSmpl t)
        {
            new Error("Invalid operation on null/empty value"); return null;
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
                        new Error("Invalid operation on null/empty value"); return null;
                    }
            }            
        }

        public IVariable Subtract(GekkoSmpl smpl, IVariable input)
        {
            new Error("Invalid operation on null/empty value"); return null;
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
                        new Error("Invalid operation on null/empty value"); return null;
                    }
            }            
        }

        public IVariable Multiply(GekkoSmpl smpl, IVariable input)
        {
            new Error("Invalid operation on null/empty value"); return null;
        }

        public IVariable Divide(GekkoSmpl smpl, IVariable input)
        {
            G.Writeln2("*** ERROR: Invalid operation on null/empty value");
            throw new GekkoException();
        }

        public IVariable Power(GekkoSmpl smpl, IVariable input)
        {
            new Error("Invalid operation on null/empty value"); return null;
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            new Error("Invalid operation on null/empty value");
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            return gekkoNull;
        }

        public void DeepTrace(TraceHelper th)
        {
            //Do nothing
        }

        public void DeepCount(Count count)
        {
            //do nothing
        }

        public void DeepTrim()
        {
            return;
        }

        public void DeepCleanup(TwoInts yearMinMax)
        {
            return;
        }

    }
}
