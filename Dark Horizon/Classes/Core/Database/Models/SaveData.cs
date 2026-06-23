namespace Dark_Horizon.Classes.Core.Database.Models
{
    public class SaveData
    {
        public int Id { get; set; }
        public string PlayerName { get; set; } = "";
        public string PlayerClass { get; set; } = "";
        public string PlayerRace { get; set; } = "";
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Gold { get; set; }
        public int Level { get; set; }
        public string CurrentLocation { get; set; } = "";
        public string InventoryJson { get; set; } = "[]";
        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}