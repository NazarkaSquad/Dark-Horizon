using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Dark_Horizon.Classes.Items;

namespace Dark_Horizon.Classes.Core.Database
{
    public static class DataLoader
    {
        private static string DataPath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        public static List<Weapon> LoadWeapons()
        {
            string json = File.ReadAllText(Path.Combine(DataPath, "weapons.json"));
            return JsonSerializer.Deserialize<List<Weapon>>(json) ?? new List<Weapon>();
        }

        public static List<Armor> LoadArmors()
        {
            string json = File.ReadAllText(Path.Combine(DataPath, "armors.json"));
            return JsonSerializer.Deserialize<List<Armor>>(json) ?? new List<Armor>();
        }

        public static List<Food> LoadFoods()
        {
            string json = File.ReadAllText(Path.Combine(DataPath, "foods.json"));
            return JsonSerializer.Deserialize<List<Food>>(json) ?? new List<Food>();
        }

        public static List<EnemyData> LoadEnemies()
        {
            string json = File.ReadAllText(Path.Combine(DataPath, "enemies.json"));
            return JsonSerializer.Deserialize<List<EnemyData>>(json) ?? new List<EnemyData>();
        }
    }

    public class EnemyData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public int Health { get; set; }
        public int Level { get; set; }
        public int Damage { get; set; }
        public int Defense { get; set; }
        public int GoldDrop { get; set; }
        public int ExpDrop { get; set; }
        public List<string> Locations { get; set; } = new List<string>();
    }
}