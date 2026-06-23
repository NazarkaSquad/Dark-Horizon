using System;
using Dark_Horizon.Classes.Items;

namespace Dark_Horizon.Classes.Entities;

public enum PlayerClass { Warrior, Mage, Rogue }

public class Player : Character
{
    public int Experience { get; set; } = 0;
    public int Gold { get; set; } = 100;
    public int Stamina { get; set; } = 100;
    public int MaxStamina { get; set; } = 100;
    public Inventory Inventory { get; set; } = new Inventory();

    public int BaseAttack { get; private set; }
    public int BaseDefense { get; private set; }

    public Weapon? EquippedWeapon { get; private set; }
    public Armor? EquippedArmor { get; private set; }

    public Player() : base() { }

    public Player(string name, PlayerClass playerClass, RaceType raceType) : base()
    {
        Name = name;
        MaxHealth = 100;
        Health = 100;
        Attack = 15;
        Defense = 5;
        Speed = 10;
        BaseAttack  = Attack;
        BaseDefense = Defense;
    }

    public Player(string name, int maxHealth, int attack, int defense, int speed) : base()
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        BaseAttack  = Attack;
        BaseDefense = Defense;
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
        RecalculateStats();
    }

    public void UnequipWeapon()
    {
        EquippedWeapon = null;
        RecalculateStats();
    }

    public void EquipArmor(Armor armor)
    {
        EquippedArmor = armor;
        RecalculateStats();
    }

    public void UnequipArmor()
    {
        EquippedArmor = null;
        RecalculateStats();
    }

    private void RecalculateStats()
    {
        Attack  = BaseAttack  + (EquippedWeapon?.Damage  ?? 0);
        Defense = BaseDefense + (EquippedArmor?.Defense ?? 0);
    }
}