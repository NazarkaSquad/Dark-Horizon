using System;
using System.Collections.Generic;
using System.Text;

namespace Dark_Horizon.Classes.Items
{
    public class Inventory
    {
        public List<Item> Items { get; private set; } = new();
        public int MaxSize { get; set; } = 20;

        public bool AddItem(Item item)
        {
            if (Items.Count >= MaxSize) return false;
            Items.Add(item);
            return true;
        }

        public void RemoveItem(Item item) => Items.Remove(item);

        public List<Item> GetByType(ItemType type) =>
            Items.Where(i => i.Type == type).ToList();
    }
}
