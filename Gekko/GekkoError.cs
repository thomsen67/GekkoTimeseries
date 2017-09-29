using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public class GekkoError : IVariable
    {
        public int uoverflow = 0;
        //public int underflow = 0;
        //public int overflow = 0;        
        
        //public GekkoError()
        //{
        //}

        public GekkoError(int uoverflow)
        {
            //this.underflow = underflow;
            //this.overflow = overflow;
            this.uoverflow = uoverflow;
        }

        public IVariable Indexer(GekkoSmpl t, params IVariable[] indexes)
        {
            return this;
        }
                
        public IVariable Negate(GekkoSmpl t)
        {
            return this;
        }

        public void InjectAdd(GekkoSmpl t, IVariable x, IVariable y)
        {
            throw new GekkoException();
        }

        public double GetValOLD(GekkoSmpl t)
        {
            throw new GekkoException();
        }

        public double GetVal(GekkoTime t)
        {
            throw new GekkoException();
        }

        public double GetVal()
        {
            G.Writeln2("*** ERROR: Cannot extract a scalar value from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public string GetString()
        {
            throw new GekkoException();
        }

        public GekkoTime GetDate(O.GetDateChoices c)
        {
            throw new GekkoException();
        }

        public List<IVariable> GetList()
        {
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.GekkoError;
        }

        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            return this;
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            return this;
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            return this;
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            return this;
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            return this;
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] dims)
        {
            G.Writeln2("*** ERROR: You cannot use an indexer [] on an error");
            throw new GekkoException();
        }

        public IVariable DeepClone()
        {
            G.Writeln2("*** ERROR: Clone error");
            throw new GekkoException();
        }
    }
}

