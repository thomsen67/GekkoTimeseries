using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gekko
{
    public class Range : IVariable
    {
        public IVariable first = null;
        public IVariable last = null;

        public Range()
        {            
        }

        public Range(IVariable xx1, IVariable xx2)
        {
            this.first = xx1;
            this.last = xx2;
        }        

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            new Error("Wrong use of Range class"); return null;
        }

        public IVariable Negate(GekkoSmpl t)
        {
            new Error("Wrong use of Range class"); return null;
        }

        public double GetValOLD(GekkoSmpl t)
        {
            new Error("Wrong use of Range class"); return double.NaN;
        }

        public double GetVal(GekkoTime t)
        {
            new Error("Wrong use of Range class"); return double.NaN;
        }

        public IVariable Concat(GekkoSmpl t, IVariable x)
        {
            new Error("Type error regarding concat and Range class"); return null;
        }

        public double ConvertToVal()
        {
            new Error("Cannot extract a val from " + G.GetTypeString(this) + " type"); return double.NaN;
        }

        public string ConvertToString()
        {
            new Error("Wrong use of Range class"); return null;
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            new Error("Wrong use of Range class"); return GekkoTime.tNull;
        }

        public List<IVariable> ConvertToList()
        {
            new Error("Wrong use of Range class"); return null;
            //throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.Range;
        }

        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            new Error("Wrong use of Range class"); return null;
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            new Error("Wrong use of Range class"); return null;
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            new Error("Wrong use of Range class"); return null;
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            new Error("Wrong use of Range class"); return null;
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            new Error("Wrong use of Range class"); return null;
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            new Error("You cannot use an indexer [] on a range"); return;
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            new Error("Clone error"); return null;
        }

        public void DeepTrace(TraceHelper th)
        {
            new Error("Trace error");
        }

        public void DeepCount(Count count)
        {
            new Error("Count error"); return;
        }

        public void DeepTrim()
        {
            new Error("Trim error"); return;
        }

        public void DeepCleanup(TwoInts yearMinMax)
        {
            //do nothing
        }

    }
}

