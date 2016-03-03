using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TKey = System.Object;
using TValue = System.Object;

namespace Gekko
{
    public interface ICache : IEnumerable
    {
        int Capacity { get; set; }
        int Count { get; }
        void Clear();
        bool IsReadOnly { get; }
    }
    public interface IListCache : ICache
    {
        int ItemCapacity { get; set; }
        int ItemCount { get; }
    }

    [Serializable]
    public class LruCache : IDictionary, ICache
    {
        [Serializable]
        private class Entry
        {
            public TKey key;
            public TValue value;
            public Entry next;
            public Entry previous;

            public Entry(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }

        private Entry anchor = new Entry(null, null);
        private Hashtable entries = new Hashtable();

        public LruCache(int capacity)
        {
            Capacity = capacity;
            Clear();
        }
        public LruCache(IDictionary dict, int capacity)
            : this(capacity)
        {
            lock (this)
            {
                if (Capacity > dict.Count) Capacity = dict.Count;
                foreach (DictionaryEntry item in dict)
                {
                    Add(item);
                }
            }
        }

        public virtual void Add(TKey key, TValue value)
        {
            lock (this)
            {
                if (ContainsKey(key)) throw new ArgumentException("Key already in dictionary");
                Entry entry = new Entry(key, value);
                entries.Add(key, entry);
                entry.previous = anchor;
                entry.next = anchor.next;
                anchor.next = entry;
                entry.next.previous = entry;
                shrinkToCapacity();
                peakCount = PeakCount;
            }
        }

        private int capacity;
        public int Capacity
        {
            get { return capacity; }
            set { lock (this) { capacity = value; shrinkToCapacity(); } }
        }

        protected virtual bool EvictionNeeded
        {
            get { return Count > Capacity; }
        }

        public int Count { get { lock (this) { return entries.Count; } } }

        private int peakCount;
        public int PeakCount
        {
            get
            {
                lock (this)
                {
                    return Count > peakCount ? Count : peakCount;
                }
            }
        }
        public ICollection Keys { get { lock (this) { return entries.Keys; } } }
        public bool ContainsKey(TKey key) { lock (this) { return entries.ContainsKey(key); } }

        private void moveToTop(Entry entry)
        {
            lock (this)
            {
                entry.previous.next = entry.next;
                entry.next.previous = entry.previous;
                entry.previous = anchor;
                entry.next = anchor.next;
                anchor.next.previous = entry;
                anchor.next = entry;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (this)
                {
                    Entry entry = (Entry)entries[key];
                    moveToTop(entry);
                    return entry.value;
                }
            }
            set
            {
                lock (this)
                {
                    if (!entries.ContainsKey(key))
                    {
                        Add(key, value);
                    }
                    else
                    {
                        Entry entry = (Entry)entries[key];
                        moveToTop(entry);
                        entry.value = value;
                    }
                }
            }
        }

        public void Remove(TKey key)
        {
            lock (this)
            {
                if (!entries.ContainsKey(key)) return;
                remove((Entry)entries[key]);
                return;
            }
        }

        private void remove(Entry entry)
        {
            lock (this)
            {
                entry.previous.next = entry.next;
                entry.next.previous = entry.previous;
                entries.Remove(entry.key);
            }
        }

        protected void shrinkToCapacity()
        {
            lock (this)
            {
                while (EvictionNeeded) remove(anchor.previous);
            }
        }

        public void Clear()
        {
            lock (this)
            {
                if (Count > 0) flushCount++;
                anchor.next = anchor;
                anchor.previous = anchor;
                entries.Clear();
            }
        }
        private int flushCount;
        public int FlushCount { get { lock (this) { return flushCount; } } }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public ICollection Values
        {
            get
            {
                ArrayList values = new ArrayList();
                foreach (DictionaryEntry item in this)
                {
                    values.Add(item.Value);
                }
                return values;
            }
        }

        public void Add(DictionaryEntry item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(DictionaryEntry item)
        {
            lock (this)
            {
                return ContainsKey(item.Key) && object.Equals(this[item.Key], item.Value);
            }
        }

        public void CopyTo(DictionaryEntry[] array, int arrayIndex)
        {
            foreach (DictionaryEntry item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Remove(DictionaryEntry item)
        {
            if (Contains(item)) Remove(item.Key);
        }

        class LruEnumerator : IDictionaryEnumerator
        {
            private IDictionaryEnumerator entryEnumerator;

            internal LruEnumerator(IDictionaryEnumerator entryEnumerator)
            {
                this.entryEnumerator = entryEnumerator;
            }

            public void Reset()
            {
                entryEnumerator.Reset();
            }

            public object Current
            {
                get
                {
                    return Entry;
                }
            }

            public bool MoveNext()
            {
                return entryEnumerator.MoveNext();
            }

            public object Key
            {
                get
                {
                    return Entry.Key;
                }
            }

            public object Value
            {
                get
                {
                    return Entry.Value;
                }
            }

            public DictionaryEntry Entry
            {
                get
                {
                    DictionaryEntry item = entryEnumerator.Entry;
                    Entry entry = (Entry)item.Value;
                    return new DictionaryEntry(item.Key, entry.value);
                }
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IDictionaryEnumerator GetEnumerator()
        {
            return new LruEnumerator(entries.GetEnumerator());
        }

        public bool Contains(object key)
        {
            return ContainsKey(key);
        }


        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public void CopyTo(Array array, int index)
        {
            // TODO:  Add LruCache.System.Collections.ICollection.CopyTo implementation
        }

        public object SyncRoot
        {
            get
            {
                return null;
            }
        }
    }

    public class ListLruCache : LruCache, IListCache
    {
        public ListLruCache(int capacity, int itemCapacity)
            : base(capacity)
        {
            this.itemCapacity = itemCapacity;
        }
        public ListLruCache(IDictionary dict, int capacity, int itemCapacity)
            : this(capacity, itemCapacity)
        {
            this.itemCapacity = itemCapacity;
            lock (this)
            {
                foreach (DictionaryEntry item in dict)
                {
                    if (this.itemCapacity < ItemCount + ((IList)item.Value).Count)
                    {
                        this.itemCapacity = ItemCount + ((IList)item.Value).Count;
                    }
                    Add(item);
                }
            }
        }
        protected override bool EvictionNeeded
        {
            get
            {
                return base.EvictionNeeded && ItemCount > ItemCapacity;
            }
        }
        private int itemCapacity = int.MaxValue;
        public int ItemCapacity
        {
            get { return itemCapacity; }
            set { lock (this) { itemCapacity = value; shrinkToCapacity(); } }
        }
        public int ItemCount
        {
            get
            {
                int count = 0;
                foreach (DictionaryEntry item in this)
                {
                    count += ((IList)item.Value).Count + 1;
                }
                return count;
            }
        }
        private int peakItemCount;
        public int PeakItemCount
        {
            get
            {
                lock (this)
                {
                    return ItemCount > peakItemCount ? ItemCount : peakItemCount;
                }
            }
        }
        public override void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            peakItemCount = PeakItemCount;
        }
    }

}
