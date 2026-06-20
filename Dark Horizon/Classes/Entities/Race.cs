using System;
using System.Collections.Generic;
using System.Text;

namespace Dark_Horizon.Classes.Entities
{
    public enum RaceType { Human, Elf, Orc }

    public class Race
    {
        public RaceType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Характеристики
        public int BonusHP { get; set; }
        public int BonusStrength { get; set; }
        public int BonusMagic { get; set; }
        public int BonusAgility { get; set; }
        public string SpecialAbility { get; set; }

        private Race(RaceType type, string name, string description,
                     int bonusHP, int bonusStrength, int bonusMagic,
                     int bonusAgility, string specialAbility)
        {
            Type = type;
            Name = name;
            Description = description;
            BonusHP = bonusHP;
            BonusStrength = bonusStrength;
            BonusMagic = bonusMagic;
            BonusAgility = bonusAgility;
            SpecialAbility = specialAbility;
        }

        // Готові раси — викликаєш Race.GetHuman() і отримуєш готовий об'єкт
        public static Race GetHuman() => new Race(
            type: RaceType.Human,
            name: "Людина",
            description: "Збалансована раса без яскравих слабкостей.\nОтримує на 10% більше золота з мобів.",
            bonusHP: 0,
            bonusStrength: 0,
            bonusMagic: 0,
            bonusAgility: 0,
            specialAbility: "Жадібність: +10% золота з мобів"
        );

        public static Race GetElf() => new Race(
            type: RaceType.Elf,
            name: "Ельф",
            description: "Крихкий але магічно сильний.\nБачить слабкі місця ворогів.",
            bonusHP: -20,
            bonusStrength: -5,
            bonusMagic: 15,
            bonusAgility: 10,
            specialAbility: "Гостре зорі: бачить слабкі місця ворога"
        );

        public static Race GetOrc() => new Race(
            type: RaceType.Orc,
            name: "Орк",
            description: "Найміцніша раса.\n10% шанс повністю ігнорувати удар.",
            bonusHP: 30,
            bonusStrength: 15,
            bonusMagic: -10,
            bonusAgility: -5,
            specialAbility: "Груба шкіра: 10% шанс ігнорувати удар"
        );

        // Зручний метод — отримати расу по enum
        public static Race GetByType(RaceType type) => type switch
        {
            RaceType.Human => GetHuman(),
            RaceType.Elf => GetElf(),
            RaceType.Orc => GetOrc(),
            _ => GetHuman()
        };
    }
}
