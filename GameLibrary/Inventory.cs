using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace GameLibrary
{
    public class Inventory
    {
        private const int BasicSize = 20;
        private readonly List<Item> Items;
        public int Size { get; private set; }

        public Inventory(int size = BasicSize)
        {
            Items = new List<Item>(size);
            Size = size;
        }

        public Item Add(Item item)
        {
            if (Items.Count >= Size) return null;
            Items.Add(item);
            return item;
        }

        public Item Remove(Item item)
        {
            var deleted = Items.First(x => x.Name == item.Name) ?? throw new ArgumentNullException("Items.First(x => x.Name == item.Name)");
            Items.Remove(deleted);
            return deleted;
        }

        public ImmutableArray<Item> GetItems()
        {
            return Items.AsReadOnly().ToImmutableArray();
        }

        internal void ChangeSize(int newSize)
        {
            Size = newSize;
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
    }
}