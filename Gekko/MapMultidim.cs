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

        //#parentpointer
        private Series parent = null;

        //#parentpointer
        public MapMultidim(Series ts)
        {
            this.parent = ts;
        }

        public MapMultidim()
        {
            //only for protobuf use
        }

        //#parentpointer
        public Series GetParent()
        {
            return this.parent;
        }

        //#parentpointer
        public void SetParent(Series ts)
        {
            this.parent = ts;
        }

        public IVariable this[params string[] s]
        {
            get
            {
                MapMultidimItem gmi = new MapMultidimItem(s);
                return storage[gmi];
            }
            set
            {
                MapMultidimItem gmi = new MapMultidimItem(s);
                storage[gmi] = value;
            }
        }

        public bool TryGetValue(MapMultidimItem gmi, out IVariable iv)
        {            
            return this.storage.TryGetValue(gmi, out iv);
        }

        public void AddIVariableWithOverwrite(MapMultidimItem gmi, IVariable iv)
        {            
            if (iv.Type() == EVariableType.Series && ((Series)iv).type == ESeriesType.Light)
            {
                throw new GekkoException(); //this check can be removed at some point
            }
            if (this.storage.ContainsKey(gmi)) this.storage.Remove(gmi);
            this.storage.Add(gmi, iv);
        }
    }

    [ProtoContract]
    public class MapMultidimItem
    {
        [ProtoMember(1)]
        public string[] storage = null;

        private MapMultidimItem()
        {
            //only because protobuf needs it, not for outside use
        }

        public MapMultidimItem(string[] s)
        {
            this.storage = s;
        }        

        public override string ToString()
        {
            string first = null;
            foreach (string s in this.storage)
            {
                //first += "'" + s + "'" + ", ";
                first += s + ", ";
            }
            first = first.Substring(0, first.Length - ", ".Length);
            return first;
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
            //Hmmm maybe not se important, since the strings will have mixed cases. Maybe in principle we should store all of them as
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
            return new MapMultidimItem(ss);
        }

    }
}
