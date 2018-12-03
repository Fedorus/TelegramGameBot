using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace GameLibrary
{
    public class Inventory<T> : IList<T>
    {
        private IList<T> Items;
        private const int BasicSize = 20;
        public int Size { get; private set; }

        public int ChangeSize(int newSize)
        {
            Size = newSize;
            return Size;
        }

        private Inventory()
        {
        }

        public Inventory(IList<T> old)
        {
            Items = old;
        }

        public Inventory(int size = BasicSize)
        {
            Size = size;
            Items = new List<T>(Size);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Items).GetEnumerator();
        }

        public void Add(T item)
        {
            if (Items.Count >= Size) throw new IndexOutOfRangeException("Рюкзак полон =(");
            Items.Add(item);
        }

        private void Add(IList<T> items)
        {
            Items = items;
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return Items.Remove(item);
        }

        public int Count => Items.Count;

        public bool IsReadOnly => Items.IsReadOnly;

        public int IndexOf(T item)
        {
            return Items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }
    }
}