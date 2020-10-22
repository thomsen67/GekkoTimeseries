using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class MapMultidim
    {
        [ProtoMember(1)]
        public Dictionary<MapMultidimItem, IVariable> storage = new Dictionary<MapMultidimItem, IVariable>();
        
        public MapMultidim()
        {
            //only for protobuf use
        }
        
        public bool TryGetValue(MapMultidimItem gmi, out IVariable iv)
        {            
            return this.storage.TryGetValue(gmi, out iv);
        }

        public void AddIVariableWithOverwrite(MapMultidimItem mmi, IVariable iv)
        {            
            if (iv.Type() == EVariableType.Series && ((Series)iv).type == ESeriesType.Light)
            {
                throw new GekkoException(); //this check can be removed at some point
            }
            if (this.storage.ContainsKey(mmi)) this.storage.Remove(mmi);
            this.storage.Add(mmi, iv);
            Series ts = iv as Series;  //always so
            if (ts != null) ts.mmi = mmi;  //so that the sub-series points to the mmi object, which in turn points to the array-series
        }

        public void RemoveIVariable(MapMultidimItem mmi)
        {
            if (this.storage.ContainsKey(mmi)) this.storage.Remove(mmi);
            else
            {
                G.Writeln2("*** ERROR: Could not remove variable");
                throw new GekkoException();
            }
        }
    }

    [ProtoContract]
    public class MapMultidimItem
    {
        [ProtoMember(1)]
        public string[] storage = null;

        public Series parent = null;  //do not store in protobuf

        private MapMultidimItem()
        {
            //only because protobuf needs it, not for outside use
        }

        //Only used for lookup purposes, is going to be discarded afterwards
        public MapMultidimItem(string[] s)
        {
            this.storage = s;
        }

        //Used for permanent storage, so the mmi must point to its parent
        public MapMultidimItem(string[] s, Series parent)
        {
            this.storage = s;
            this.parent = parent;
        }        

        public override string ToString()
        {
            string first = null;
            foreach (string s in this.storage)
            {                
                first += s + ", ";
            }
            first = first.Substring(0, first.Length - ", ".Length);
            return first;
        }

        public string GetName()
        {            
            return this.parent.name + "[" + this.ToString() + "]";
        }        

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < storage.Length; i++)
            {
                hash = hash * 31 + storage[i].ToLower().GetHashCode();  //the 17 and 31 is a trick (primes) to get the hashcodes as distinct as possible. We need ToLower() so that 'aB' and 'Ab' are equal
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
            //This will run fastest if the strings are interned (cf. string.Intern). But it seems they are so when getting deflated
            //from protobuf file anyway.
            //Hmmm maybe not so important, since the strings will have mixed cases. Maybe in principle we should store all of them as
            //lower-case..... ??

            if (obj == null || obj.GetType() != typeof(MapMultidimItem)) return false;
            MapMultidimItem other = (MapMultidimItem)obj;            
            if (this.storage.Length != other.storage.Length) return false;
            for (int i = 0; i < this.storage.Length; i++)
            {
                if (!G.Equal(this.storage[i], other.storage[i])) return false;
            }
            return true;
        }

        public MapMultidimItem Clone()
        {
            string[] ss = new string[this.storage.Length];
            Array.Copy(this.storage, ss, this.storage.Length);
            MapMultidimItem mmi = new MapMultidimItem(ss, this.parent);
            return mmi;
        }

    }
}
