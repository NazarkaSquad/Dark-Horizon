using System;

namespace Dark_Horizon.Classes.Items
{
    // Типи предметів у грі
        public enum ItemType { Weapon, Armor, Food }

    // Базовий клас для всіх предметів
    public abstract class Item
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int SellPrice => (int)(Price * 0.8);
        public ItemType Type { get; set; }
        public string Description { get; set; }
        public int RequiredLevel { get; set; }

        // Шлях до картинки для WPF текстур (наприклад, "/Assets/Images/sword.png")
        public string IconPath { get; set; }

        // Конструктор предмета
        public Item(string name, int price, ItemType type, int requiredLevel = 1, string description = "", string iconPath = "")
        {
            Name = name;
            Price = price;
            Type = type;
            RequiredLevel = requiredLevel;
            Description = description;
            IconPath = iconPath;
        }

        // Абстрактний метод для використання предмета грацем.
        // Замість 'object player' потім впишіть ваш реальний клас персонажа (наприклад, Character або Player)
        public abstract void Use(object player);
    }
}