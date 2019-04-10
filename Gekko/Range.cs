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

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }


        //public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2)
        //{
        //    G.Writeln2("*** ERROR: Wrong use of Range class");
        //    throw new GekkoException();
        //}

        //public IVariable Indexer(GekkoSmpl t, IVariable index, IVariablesFilterRange indexRange)
        //{
        //    G.Writeln2("*** ERROR: Wrong use of Range class");
        //    throw new GekkoException();
        //}

        //public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange, IVariable index)
        //{
        //    G.Writeln2("*** ERROR: Wrong use of Range class");
        //    throw new GekkoException();
        //}

        //public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange)
        //{
        //    G.Writeln2("*** ERROR: Wrong use of Range class");
        //    throw new GekkoException();
        //}

        public IVariable Negate(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        //public void InjectAdd(GekkoSmpl t, IVariable x, IVariable y)
        //{
        //    G.Writeln2("*** ERROR: Wrong use of Range class");
        //    throw new GekkoException();
        //}

        public double GetValOLD(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public double GetVal(GekkoTime t)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public IVariable Concat(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: Type error regarding concat and Range class");
            throw new GekkoException();
        }

        public double ConvertToVal()
        {
            G.Writeln2("*** ERROR: Cannot extract a val from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public List<IVariable> ConvertToList()
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.Range;
        }

        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            G.Writeln2("*** ERROR: You cannot use an indexer [] on a range");
            throw new GekkoException();
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            G.Writeln2("*** ERROR: Clone error");
            throw new GekkoException();
        }

        public void DeepTrim()
        {
            G.Writeln2("*** ERROR: Trim error");
            throw new GekkoException();
        }

        public void DeepCleanup()
        {
            //do nothing
        }

    }
}

