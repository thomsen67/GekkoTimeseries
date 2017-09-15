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

        public IVariable Indexer(GekkoSmpl t, params IVariable[] indexes)
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

        public void InjectAdd(GekkoSmpl t, IVariable x, IVariable y)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

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

        public double GetVal()
        {
            G.Writeln2("*** ERROR: Cannot extract a scalar value from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public string GetString()
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public GekkoTime GetDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: Wrong use of Range class");
            throw new GekkoException();
        }

        public List<IVariable> GetList()
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

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] dims)
        {
            G.Writeln2("*** ERROR: You cannot use an indexer [] on a range");
            throw new GekkoException();
        }

        public IVariable DeepClone()
        {
            G.Writeln2("*** ERROR: Clone error");
            throw new GekkoException();
        }

    }
}

