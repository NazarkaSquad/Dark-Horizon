using System;
using Dark_Horizon.Classes.Entities;

namespace Dark_Horizon.Classes.Items
{
    public class Armor : Item
    {
        public int Defense { get; set; }

        public Armor(string name, int price, int defense, int requiredLevel, string description = "", string iconPath = "")
            : base(name, price, ItemType.Armor, requiredLevel, description, iconPath)
        {
            Defense = defense;
        }

        public override void Use(object player)
        {
            if (player is Player p)
                p.EquipArmor(this);
        }
    }
}