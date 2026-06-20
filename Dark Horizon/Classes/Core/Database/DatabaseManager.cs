using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Dark_Horizon.Classes.Core.Database.Models;

namespace Dark_Horizon.Classes.Core.Database;

public class DatabaseManager
{
    private readonly string _saveFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "savegame.json");
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public async Task SaveGameAsync(SaveData data)
    {
        using FileStream createStream = File.Create(_saveFilePath);
        await JsonSerializer.SerializeAsync(createStream, data, _options);
    }

    public async Task<SaveData?> LoadGameAsync()
    {
        if (!File.Exists(_saveFilePath))
            return null;

        using FileStream openStream = File.OpenRead(_saveFilePath);
        return await JsonSerializer.DeserializeAsync<SaveData>(openStream, _options);
    }
}