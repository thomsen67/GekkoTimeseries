/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Gekko
{
    /// <summary>
    /// This class is under construction...
    /// </summary>
    [ProtoContract]
    public class GekkoDataFrame : IVariable
    {
        [ProtoMember(1)]
        public double val;
        
        private GekkoDataFrame()
        {
            //only because protobuf needs it, not for outside use
        }

        public GekkoDataFrame(double d)
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
            new Error("Could not convert the dateframe " + this.val + " directly into string. You may try the string() conversion function.");
            return null;
            //throw new GekkoException();
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

        public void DeepCount(Count count)
        {
            //do nothing
        }

        public void DeepTrim()
        {
            //do nothing, nothing to trim
        }

        public void DeepCleanup(TwoInts yearsMinMax)
        {
            //do nothing
        }

    }
}
