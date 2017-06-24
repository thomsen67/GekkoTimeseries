using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    class GekkoError : IVariable
    {
        public int underflow = 0;
        public int overflow = 0;
        
        public GekkoError()
        {

        }        

        public IVariable Indexer(IVariableHelper t, bool isLhs, params IVariable[] indexes)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariablesFilterRange indexRange, IVariableHelper t)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2, IVariableHelper t)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariable index, IVariablesFilterRange indexRange, IVariableHelper t)
        {
            throw new GekkoException();
        }

        public IVariable Indexer(IVariablesFilterRange indexRange, IVariable index, IVariableHelper t)
        {
            throw new GekkoException();
        }

        public IVariable Negate(IVariableHelper t)
        {
            return null;
        }

        public void InjectAdd(IVariable x, IVariable y, IVariableHelper t)
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

        public IVariable Add(IVariable x, IVariableHelper t)
        {            
            throw new GekkoException();
        }

        public IVariable Subtract(IVariable x, IVariableHelper t)
        {
            return null;
        }

        public IVariable Multiply(IVariable x, IVariableHelper t)
        {
            return null;
        }

        public IVariable Divide(IVariable x, IVariableHelper t)
        {
            return null;
        }

        public IVariable Power(IVariable x, IVariableHelper t)
        {
            return null;
        }
    }
}

