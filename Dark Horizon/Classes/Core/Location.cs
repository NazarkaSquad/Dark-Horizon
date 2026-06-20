using System;
using System.Collections.Generic;
using System.Text;

using Dark_Horizon.Classes.Entities;

namespace Dark_Horizon.Classes.Core
{
    public enum LocationType { City, Forest, Tavern, Swamp, Outskirt }

    public class Location
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public LocationType Type { get; set; }
        public int RecommendedLevel { get; set; }
        public List<Enemy> Enemies { get; set; } = new();

        // Сусідні локації (куди можна піти)
        public Location? North { get; set; }
        public Location? South { get; set; }
        public Location? East { get; set; }
        public Location? West { get; set; }

        public bool HasMerchant { get; set; }
        public bool HasTavern { get; set; }

        public Location(string name, string description, LocationType type, int recommendedLevel)
        {
            Name = name;
            Description = description;
            Type = type;
            RecommendedLevel = recommendedLevel;
        }
    }
}