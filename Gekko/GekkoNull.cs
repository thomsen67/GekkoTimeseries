using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    class GekkoNull : IVariable
    {

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
            throw new GekkoException();
        }
                
        public IVariable Add(GekkoSmpl smpl, IVariable input)
        {
            throw new GekkoException();
        }

        public IVariable Subtract(GekkoSmpl smpl, IVariable input)
        {
            throw new GekkoException();
        }

        public IVariable Multiply(GekkoSmpl smpl, IVariable input)
        {
            throw new GekkoException();
        }

        public IVariable Divide(GekkoSmpl smpl, IVariable input)
        {
            throw new GekkoException();
        }

        public IVariable Power(GekkoSmpl smpl, IVariable input)
        {
            throw new GekkoException();
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] dims)
        {
            throw new GekkoException();
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
