using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dark_Horizon.Classes.Entities;
using Dark_Horizon.Classes.Core.Database.Models;

namespace Dark_Horizon.Classes.Core.Database;

public class SaveManager
{
    private static readonly string SavesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saves");
    private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "player_save.json");

    static SaveManager()
    {
        if (!Directory.Exists(SavesFolder))
        {
            Directory.CreateDirectory(SavesFolder);
        }
    }

    private static string GetPath(int slot) => Path.Combine(SavesFolder, $"save_{slot}.json");
    public void Save(int slot, Player player, string locationName)
    {
        try
        {
            var saveData = new SaveData
            {
                Id = slot,
                PlayerName = player.Name,
                PlayerClass = "Warrior",
                PlayerRace = "Human",
                Health = player.Health,
                MaxHealth = player.MaxHealth,
                Gold = player.Gold,
                Level = player.Level,
                CurrentLocation = locationName,
                InventoryJson = JsonSerializer.Serialize(player.Inventory),
                SavedAt = DateTime.Now
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(saveData, options);

            File.WriteAllText(GetPath(slot), json);
            File.WriteAllText(_path, json);
        }
        catch { }
    }

    public void Save(int slot, Player player)
    {
        Save(slot, player, "Town");
    }

    public List<SaveData> GetAllSaves()
    {
        var saves = new List<SaveData>();
        for (int i = 1; i <= 3; i++)
        {
            string path = GetPath(i);
            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    var saveData = JsonSerializer.Deserialize<SaveData>(json);
                    if (saveData != null)
                    {
                        saveData.Id = i;
                        saves.Add(saveData);
                    }
                }
                catch { }
            }
        }
        return saves;
    }

    public SaveData? Load(int slot)
    {
        string path = GetPath(slot);
        if (!File.Exists(path)) return null;
        try
        {
            string json = File.ReadAllText(path);
            var saveData = JsonSerializer.Deserialize<SaveData>(json);
            if (saveData != null) saveData.Id = slot;
            return saveData;
        }
        catch { return null; }
    }

    public void Delete(int slot)
    {
        string path = GetPath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public async Task SavePlayerAsync(Player player)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            using FileStream createStream = File.Create(_path);
            await JsonSerializer.SerializeAsync(createStream, player, options);
        }
        catch { }
    }

    public async Task<Player?> LoadPlayerAsync()
    {
        if (!File.Exists(_path)) return null;
        try
        {
            using FileStream openStream = File.OpenRead(_path);
            return await JsonSerializer.DeserializeAsync<Player>(openStream);
        }
        catch { return null; }
    }
}