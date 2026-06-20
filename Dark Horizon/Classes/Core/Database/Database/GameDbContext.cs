using System.IO;
using Microsoft.EntityFrameworkCore;
using Dark_Horizon.Classes.Core.Database.Models;

namespace Dark_Horizon.Classes.Core.Database
{
    public class GameDbContext : DbContext
    {
        public DbSet<SaveData> Saves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string dbPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "game.db");
            options.UseSqlite($"Data Source={dbPath}");
        }
    }
}