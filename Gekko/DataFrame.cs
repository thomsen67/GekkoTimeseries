using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class DataFrame : IVariable
    {
        [ProtoMember(1)]
        public double val;
        
        private DataFrame()
        {
            //only because protobuf needs it, not for outside use
        }

        public DataFrame(double d)
        {
            this.val = d;
        }                

        public double GetValOLD(GekkoSmpl t)
        {
            return this.val;
        }

        public double GetVal(GekkoTime t)
        {
            return this.val;
        }

        public double ConvertToVal()
        {
            return this.val;
        }

        public string ConvertToString()
        {
            G.Writeln2("*** ERROR: Could not convert the dateframe " + this.val + " directly into string.");
            G.Writeln("           You may try the string() conversion function.");
            throw new GekkoException();
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {         
            throw new GekkoException();
        }

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] indexes)
        {            
            throw new GekkoException();
        }

        public List<IVariable> ConvertToList()
        {            
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.Val;
        }

        public IVariable Negate(GekkoSmpl t)
        {
            return new ScalarVal(-this.val);
        }
        
        public IVariable Add(GekkoSmpl smpl, IVariable input)
        {
            throw new GekkoException();
        }

        public IVariable Concat(GekkoSmpl smpl, IVariable input)
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

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            throw new GekkoException();
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            return new ScalarVal(this.val);
        }

        public void DeepTrim()
        {
            //do nothing, nothing to trim
        }

        public void DeepCleanup()
        {
            //do nothing
        }

    }
}
