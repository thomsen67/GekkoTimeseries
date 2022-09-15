using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace Gekko
{

    public enum EVariableType
    {
        String,
        Date,
        Val,
        Series,        
        List,
        Map,
        Matrix,
        Var,
        Range,              
        Null,
        Name
    }

    public enum EScalarRefType
    {
        String,
        Date,
        Val,
        Matrix,
        OnRightHandSide
    }

    //IMPORTANT
    //IMPORTANT see #70324327984
    //IMPORTANT
    [ProtoContract]    
    [ProtoInclude(1, typeof(Series))]
    [ProtoInclude(2, typeof(ScalarVal))]
    [ProtoInclude(3, typeof(ScalarDate))]
    [ProtoInclude(4, typeof(ScalarString))]
    [ProtoInclude(5, typeof(Map))]
    [ProtoInclude(6, typeof(Matrix))]
    [ProtoInclude(7, typeof(List))]
    [ProtoInclude(8, typeof(Range))]            //HMMM is an IVariable, but could never be protobuffed
    [ProtoInclude(9, typeof(GekkoNull))]        //---> this is a real reference to NULL, do not change!!
    [ProtoInclude(10, typeof(GekkoDataFrame))]
    [ProtoInclude(11, typeof(GekkoNull))]       //---> the following are for backwards compatibility, can be changed
    [ProtoInclude(12, typeof(GekkoNull))]
    [ProtoInclude(13, typeof(GekkoNull))]
    [ProtoInclude(14, typeof(GekkoNull))]
    [ProtoInclude(15, typeof(GekkoNull))]
    [ProtoInclude(16, typeof(GekkoNull))]
    [ProtoInclude(17, typeof(GekkoNull))]
    [ProtoInclude(18, typeof(GekkoNull))]
    [ProtoInclude(19, typeof(GekkoNull))]
    [ProtoInclude(20, typeof(GekkoNull))]

    public interface IVariable
    {
        //The following classes implement this interface:
        //Series, ScalarVal, ScalarString, ScalarDate, List, Matrix, Map
        //
        //NOTE: At some point we need to create a lot of overloads, InjectAdd() with double as 1. or 2. or both arguments,
        //      same for InjectMinus() etc. etc. Maybe also with dates and strings.
        //      That will speed up i=i+1 stuff up a lot.
        //      How to do this consistently without duplicating code???        

        IVariable Add(GekkoSmpl smpl, IVariable x); //returns a new object! z = x.Add(y) creates a new z (does not alter x or y).        
        
        IVariable Subtract(GekkoSmpl smpl, IVariable x);

        IVariable Multiply(GekkoSmpl smpl, IVariable x); //returns a new object!

        IVariable Divide(GekkoSmpl smpl, IVariable x); //returns a new object!

        IVariable Power(GekkoSmpl smpl, IVariable x); //returns a new object!

        IVariable Negate(GekkoSmpl smpl); //returns a new object!
        
        IVariable Indexer(GekkoSmpl smpl, O.EIndexerType indexerType, params IVariable[] index); //returns a new object! t needs to be 1. argument.

        IVariable Concat(GekkoSmpl smpl, IVariable x); //returns a new object!
        
        //void InjectAdd(GekkoSmpl smpl, IVariable x, IVariable y); //z.InjectAdd(x,y) inserts the sum of x and y into the z object.        

        double GetValOLD(GekkoSmpl smpl);

        double GetVal(GekkoTime t);

        double ConvertToVal();
        
        string ConvertToString();

        GekkoTime ConvertToDate(O.GetDateChoices c);

        List<IVariable> ConvertToList();

        EVariableType Type();
        
        IVariable DeepClone(GekkoSmplSimple truncate);

        void DeepCount(Count count);

        void DeepTrim();

        void DeepCleanup(TwoInts yearsMinMax);

        void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims);
    }
}
