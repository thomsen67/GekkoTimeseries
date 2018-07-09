using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    class GekkoNull : IVariable
    {
        // ----------------------------------------------------
        // --------------object functions start----------------
        // ----------------------------------------------------

        public IVariable append(bool isLhs, GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method .append() not available for type " + G.GetTypeString(this));
            throw new GekkoException();
        }

        // ----------------------------------------------------
        // --------------object functions end------------------
        // ----------------------------------------------------

        public double GetValOLD(GekkoSmpl t)
        {
            throw new GekkoException();
        }

        public double GetVal(GekkoTime t)
        {
            throw new GekkoException();
        }

        public double ConvertToVal()
        {
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            throw new GekkoException();
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(GekkoSmpl t, params IVariable[] indexes)
        {
            throw new GekkoException();
        }

        public List<IVariable> ConvertToList()
        {
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.GekkoNull;
        }

        public IVariable Negate(GekkoSmpl t)
        {
            return this;
        }
                        
        public IVariable Add(GekkoSmpl smpl, IVariable input)
        {
            return this;
        }

        public IVariable Subtract(GekkoSmpl smpl, IVariable input)
        {
            return this;
        }

        public IVariable Multiply(GekkoSmpl smpl, IVariable input)
        {
            return this;
        }

        public IVariable Divide(GekkoSmpl smpl, IVariable input)
        {
            return this;
        }

        public IVariable Power(GekkoSmpl smpl, IVariable input)
        {
            return this;
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] dims)
        {
            return; //the "this" object stays untouched (that is, GekkoNull)
        }

        public IVariable DeepClone()
        {
            throw new GekkoException();
        }

        public void DeepTrim()
        {
            throw new GekkoException();
        }

        public void DeepCleanup()
        {
            throw new GekkoException();
        }
    }
}
