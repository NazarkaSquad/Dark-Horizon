using System;
using System.Collections.Generic;
using System.Text;

namespace Dark_Horizon.Classes.Items
{
    // Інвентар: список предметів і логіка додавання/видалення
    public class Inventory
    {
        public List<Item> Items { get; private set; } = new();
        public int MaxSize { get; set; } = 20;

        // Додає предмет, якщо є місце. Повертає true при успіху
        public bool AddItem(Item item)
        {
            if (Items.Count >= MaxSize) return false;
            Items.Add(item);
            return true;
        }

        // Видаляє предмет з інвентаря
        public void RemoveItem(Item item) => Items.Remove(item);

        // Повертає список предметів певного типу
        public List<Item> GetByType(ItemType type) =>
            Items.Where(i => i.Type == type).ToList();
    }
}
