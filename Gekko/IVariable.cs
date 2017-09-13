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
        Map,
        Matrix,   
        Range,      
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

    //public class IVariablesFilterRange
    //{
    //    //Used for a[1:4] or a[1:4, 2:3].        
    //    public IVariable first = null;
    //    public IVariable last = null;
    //    public IVariablesFilterRange(IVariable v1, IVariable v2)
    //    {        
    //        this.first = v1;
    //        this.last = v2;
    //    }
    //}    

    public interface IVariable
    {
        //The following classes implement this interface:
        // ScalarVal, ScalarString, ScalarDate, MetaList, MetaTimeSeries. (matrix/vector will come...)
        //
        //NOTE: At some point we need to create a lot of overloads, InjectAdd() with double as 1. or 2. or both arguments,
        //      same for InjectMinus() etc. etc. Maybe also with dates and strings.
        //      That will speed up i=i+1 stuff up a lot.
        //      How to do this consistently without duplicating code???

        IVariable Add(GekkoSmpl smpl, IVariable x); //returns a new object! z = x.Add(y) creates a new z (does not alter x or y).        
        
        IVariable Subtract(GekkoSmpl smpl, IVariable x);

        IVariable Multiply(GekkoSmpl smpl, IVariable x); //returns a new object! z = x.Add(y) creates a new z (does not alter x or y).        

        IVariable Divide(GekkoSmpl smpl, IVariable x); //returns a new object! z = x.Add(y) creates a new z (does not alter x or y).        

        IVariable Power(GekkoSmpl smpl, IVariable x); //returns a new object! z = x.Add(y) creates a new z (does not alter x or y).        

        IVariable Negate(GekkoSmpl smpl); //returns a new object!
        
        IVariable Indexer(GekkoSmpl smpl, params IVariable[] index); //returns a new object! t needs to be 1. argument.
                
        void InjectAdd(GekkoSmpl smpl, IVariable x, IVariable y); //z.InjectAdd(x,y) inserts the sum of x and y into the z object.        

        double GetVal(GekkoSmpl smpl);

        string GetString();

        GekkoTime GetDate(O.GetDateChoices c);

        List<IVariable> GetList();

        EVariableType Type();

        IVariable DeepClone();

        void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] dims);
    }
}
