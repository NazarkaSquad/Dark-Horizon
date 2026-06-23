using System;

namespace Dark_Horizon.Classes.Entities;

public abstract class Character
{
    public string Name { get; set; } = "Unknown";
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Level { get; set; } = 1;

    public bool IsAlive => Health > 0;

    public int Heal(int amount)
    {
        if (amount <= 0 || !IsAlive) return 0;
        int before = Health;
        Health = Math.Min(MaxHealth, Health + amount);
        return Health - before;
    }

    protected Character() { }
}
