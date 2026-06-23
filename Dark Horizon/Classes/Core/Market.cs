using System;
using System.Collections.Generic;
using System.Text;

using Dark_Horizon.Classes.Items;
using Dark_Horizon.Classes.Entities;

namespace Dark_Horizon.Classes.Core
{
    public class Market
    {
        public List<Item> Stock { get; private set; } = new();

        public Market()
        {

        }

        public string Buy(Player player, Item item)
        {
            if (!Stock.Contains(item))
                return "Цього товару немає в магазині.";

            if (player.Gold < item.Price)
                return $"Недостатньо золота! Треба {item.Price}, є {player.Gold}.";

            if (!player.Inventory.AddItem(item))
                return "Інвентар повний!";

            player.Gold -= item.Price;
            Stock.Remove(item);
            return $"Куплено: {item.Name} за {item.Price} золота.";
        }

        public string Sell(Player player, Item item)
        {
            if (!player.Inventory.Items.Contains(item))
                return "Цього предмета немає в інвентарі.";

            player.Gold += item.SellPrice;
            player.Inventory.RemoveItem(item);
            Stock.Add(item);
            return $"Продано: {item.Name} за {item.SellPrice} золота.";
        }

        public void AddToStock(Item item) => Stock.Add(item);
    }
}