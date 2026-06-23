using System.Linq;
using Dark_Horizon.Classes.Entities;

namespace Dark_Horizon.Classes.Core
{
    public class LocationManager
    {
        public Location CurrentLocation { get; private set; }
        public List<Location> AllLocations { get; private set; } = new();

        public LocationManager()
        {
            var city = new Location(
                name: "Місто",
                description: "Торгове місто. Тут можна купити спорядження і відпочити.",
                type: LocationType.City,
                recommendedLevel: 1
            )
            { HasMerchant = true };

            var tavern = new Location(
                name: "Таверна",
                description: "Затишне місце. Можна відновити HP за золото.",
                type: LocationType.Tavern,
                recommendedLevel: 1
            )
            { HasTavern = true };

            var forest = new Location(
                name: "Темний ліс",
                description: "Небезпечний ліс. Тут водяться вовки, розбійники і скелети.",
                type: LocationType.Forest,
                recommendedLevel: 2
            );

            var swamp = new Location(
                name: "Болото",
                description: "Смердюче болото. Слизняки, скелети і огри чекають на тебе.",
                type: LocationType.Swamp,
                recommendedLevel: 4
            );

            var outskirt = new Location(
                name: "Околиці",
                description: "Перехідна зона біля міста. Тут спокійно — справжня небезпека далі.",
                type: LocationType.Outskirt,
                recommendedLevel: 1
            );
            forest.Enemies.AddRange(new[]
            {
                Enemy.CreateWolf(),
                Enemy.CreateWolf(),
                Enemy.CreateBandit(),
                Enemy.CreateSkeleton()
            });

            swamp.Enemies.AddRange(new[]
            {
                Enemy.CreateSlug(),
                Enemy.CreateSlug(),
                Enemy.CreateSkeleton(),
                Enemy.CreateOgre()
            });

            city.North = tavern;
            city.West = forest;
            city.East = swamp;
            city.South = outskirt;

            tavern.South = city;
            forest.East = city;
            swamp.West = city;
            outskirt.North = city;
            outskirt.South = forest;

            AllLocations.AddRange(new[] { city, tavern, forest, swamp, outskirt });

            CurrentLocation = city;
        }

        public Location? MoveNorth() => TryMove(CurrentLocation.North);
        public Location? MoveSouth() => TryMove(CurrentLocation.South);
        public Location? MoveEast() => TryMove(CurrentLocation.East);
        public Location? MoveWest() => TryMove(CurrentLocation.West);

        private Location? TryMove(Location? target)
        {
            if (target == null) return null;
            CurrentLocation = target;
            return CurrentLocation;
        }

        public bool CanGoNorth => CurrentLocation.North != null;
        public bool CanGoSouth => CurrentLocation.South != null;
        public bool CanGoEast => CurrentLocation.East != null;
        public bool CanGoWest => CurrentLocation.West != null;

        public void SetCurrentLocation(LocationType type)
        {
            var loc = AllLocations.FirstOrDefault(l => l.Type == type);
            if (loc != null)
                CurrentLocation = loc;
        }
    }
}