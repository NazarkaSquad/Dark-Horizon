using System;
using System.Collections.Generic;
using System.Text;

using Dark_Horizon.Classes.Items;
using Dark_Horizon.Classes.Entities;

namespace Dark_Horizon.Classes.Core
{
    // Клас, що відповідає за логіку магазину: зберігає товари, дозволяє купувати/продавати
    public class Market
    {
        // Список товарів у магазині
        // Stock — поточний асортимент, з якого гравець може купувати
        public List<Item> Stock { get; private set; } = new();

        // Конструктор магазину — тут можна наповнити початковий асортимент
        public Market()
        {
            // Тут потім додаси реальні предмети від Тохи
            // Stock.Add(new Weapon(...));
        }

        // Купівля — гравець купує предмет у магазину
        // Перевіряє наявність, достатність золота і вільне місце в інвентарі
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

        // Продаж — гравець продає предмет магазину
        // Повертає повідомлення про результат операції
        public string Sell(Player player, Item item)
        {
            if (!player.Inventory.Items.Contains(item))
                return "Цього предмета немає в інвентарі.";

            player.Gold += item.SellPrice; // 80% від ціни
            player.Inventory.RemoveItem(item);
            Stock.Add(item); // Предмет повертається в магазин
            return $"Продано: {item.Name} за {item.SellPrice} золота.";
        }

        // Додає предмет у асортимент магазину
        public void AddToStock(Item item) => Stock.Add(item);
    }
}