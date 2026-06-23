using System;

namespace Dark_Horizon.Classes.Items
{
        public enum ItemType { Weapon, Armor, Food }

    public abstract class Item
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int SellPrice => (int)(Price * 0.8);
        public ItemType Type { get; set; }
        public string Description { get; set; }
        public int RequiredLevel { get; set; }

        public string IconPath { get; set; }

        public Item(string name, int price, ItemType type, int requiredLevel = 1, string description = "", string iconPath = "")
        {
            Name = name;
            Price = price;
            Type = type;
            RequiredLevel = requiredLevel;
            Description = description;
            IconPath = iconPath;
        }
        public abstract void Use(object player);
    }
}