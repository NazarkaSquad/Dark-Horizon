using System;
using System.Collections.Generic;

namespace Dark_Horizon.Classes.Entities;

public class Enemy : Character
{
    public int RewardExp  { get; set; }
    public int RewardGold { get; set; }

    public string SpriteKey { get; set; } = "";

    public Enemy() : base() { }

    public Enemy(string name, int maxHealth, int attack, int defense,
                 int speed, int rewardExp, int rewardGold = 15,
                 int level = 1, string spriteKey = "") : base()
    {
        Name      = name;
        MaxHealth = maxHealth;
        Health    = maxHealth;
        Attack    = attack;
        Defense   = defense;
        Speed     = speed;
        RewardExp = rewardExp;
        RewardGold = rewardGold;
        Level     = level;
        SpriteKey = spriteKey;
    }

    public static Enemy CreateSlug()     => new("Слимак",   30,  5,  2,  3, 10,  8, 1, "Slime");
    public static Enemy CreateWolf()     => new("Вовк",     50, 12,  4,  8, 25, 15, 2, "Wolf");
    public static Enemy CreateSkeleton() => new("Скелет",   60, 14,  6,  5, 35, 18, 2, "Skeleton");
    public static Enemy CreateZombie()   => new("Зомбі",    55, 10,  3,  4, 30, 12, 2, "Zombie");
    public static Enemy CreateWitch()    => new("Відьма",   80, 20,  4,  6, 55, 28, 4, "Witch");

    public static List<Enemy> ForestPool() => new()
    {   
        CreateWolf(),
        CreateSkeleton(), CreateZombie()
    };

    public static List<Enemy> SwampPool() => new()
    {
        CreateSlug(), CreateSkeleton(),
        CreateZombie(), CreateWitch()
    };
}
