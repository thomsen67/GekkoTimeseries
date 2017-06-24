using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{

    public enum EVariableType
    {
        String,
        Date,
        Val,
        TimeSeries,
        List,
        Matrix,         
        GekkoError
    }

    public enum EScalarRefType
    {
        String,
        Date,
        Val,
        Matrix,
        OnRightHandSide
    }    

    public class IVariablesFilterRange
    {
        //Used for a[1:4] or a[1:4, 2:3].        
        public IVariable first = null;
        public IVariable last = null;
        public IVariablesFilterRange(IVariable v1, IVariable v2)
        {        
            this.first = v1;
            this.last = v2;
        }
    }    

    public interface IVariable
    {
        //The following classes implement this interface:
        // ScalarVal, ScalarString, ScalarDate, MetaList, MetaTimeSeries. (matrix/vector will come...)
        //
        //NOTE: At some point we need to create a lot of overloads, InjectAdd() with double as 1. or 2. or both arguments,
        //      same for InjectMinus() etc. etc. Maybe also with dates and strings.
        //      That will speed up i=i+1 stuff up a lot.
        //      How to do this consistently without duplicating code???

        IVariable Add(IVariable x, IVariableHelper t); //returns a new object! z = x.Add(y) creates a new z (does not alter x or y).        
        
        IVariable Subtract(IVariable x, IVariableHelper t);

        IVariable Multiply(IVariable x, IVariableHelper t); //returns a new object! z = x.Add(y) creates a new z (does not alter x or y).        

        IVariable Divide(IVariable x, IVariableHelper t); //returns a new object! z = x.Add(y) creates a new z (does not alter x or y).        

        IVariable Power(IVariable x, IVariableHelper t); //returns a new object! z = x.Add(y) creates a new z (does not alter x or y).        

        IVariable Negate(IVariableHelper t); //returns a new object!
        
        IVariable Indexer(IVariableHelper t, bool isLhs, params IVariable[] index); //returns a new object! t needs to be 1. argument.

        IVariable Indexer(IVariablesFilterRange index, IVariableHelper t); //x[2..3]: returns a new object with sub-items

        IVariable Indexer(IVariablesFilterRange index1, IVariablesFilterRange index2, IVariableHelper t); //x[2..3, 3..5]: returns a new object with sub-items

        IVariable Indexer(IVariable index, IVariablesFilterRange indexRange, IVariableHelper t);

        IVariable Indexer(IVariablesFilterRange indexRange, IVariable index, IVariableHelper t);

        void InjectAdd(IVariable x, IVariable y, IVariableHelper t); //z.InjectAdd(x,y) inserts the sum of x and y into the z object.        

        double GetVal(IVariableHelper t);

        string GetString();

        GekkoTime GetDate(O.GetDateChoices c);

        List<string> GetList();

        EVariableType Type();
    }
}
