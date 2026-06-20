using System;
using Dark_Horizon.Classes.Entities;

namespace Dark_Horizon.Classes.Items
{
    public class Weapon : Item
    {
        public int Damage { get; set; }

        public Weapon(string name, int price, int damage, int requiredLevel, string description = "", string iconPath = "")
            : base(name, price, ItemType.Weapon, requiredLevel, description, iconPath)
        {
            Damage = damage;
        }

        public override void Use(object player)
        {
            if (player is Player p)
                p.EquipWeapon(this);
        }
    }
}