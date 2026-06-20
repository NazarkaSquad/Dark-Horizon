using System;
using Dark_Horizon.Classes.Entities;

namespace Dark_Horizon.Classes.Items
{
    public class Food : Item
    {
        public int HealAmount { get; set; }

        public Food(string name, int price, int healAmount,
                    string description = "", string iconPath = "")
            : base(name, price, ItemType.Food, 1, description, iconPath)
        {
            HealAmount = healAmount;
        }

        public override void Use(object player)
        {
            if (player is Character ch)
                ch.Heal(HealAmount);
        }
    }
}
