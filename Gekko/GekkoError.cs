using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public class GekkoError : IVariable
    {
        public int underflow = 0;
        public int overflow = 0;        
        
        public GekkoError()
        {

        }

        public GekkoError(int underflow, int overflow)
        {
            this.underflow = underflow;
            this.overflow = overflow;
        }

        public IVariable Indexer(IVariableHelper t, bool isLhs, params IVariable[] indexes)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariableHelper t, IVariablesFilterRange indexRange)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariableHelper t, IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariableHelper t, IVariable index, IVariablesFilterRange indexRange)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariableHelper t, IVariablesFilterRange indexRange, IVariable index)
        {
            throw new GekkoException();
        }

        public IVariable Negate(IVariableHelper t)
        {
            return null;
        }

        public void InjectAdd(IVariableHelper t, IVariable x, IVariable y)
        {
            throw new GekkoException();
        }

        public double GetVal(IVariableHelper t)
        {
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

        public List<string> GetList()
        {
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.GekkoError;
        }

        public IVariable Add(IVariableHelper t, IVariable x)
        {            
            throw new GekkoException();
        }

        public IVariable Subtract(IVariableHelper t, IVariable x)
        {
            return null;
        }

        public IVariable Multiply(IVariableHelper t, IVariable x)
        {
            return null;
        }

        public IVariable Divide(IVariableHelper t, IVariable x)
        {
            return null;
        }

        public IVariable Power(IVariableHelper t, IVariable x)
        {
            return null;
        }
    }
}

