using System;
using Dark_Horizon.Classes.Items;

namespace Dark_Horizon.Classes.Entities;

public enum PlayerClass { Warrior, Mage, Rogue }

public class Player : Character
{
    // ВИПРАВЛЕНО: Прибрано "new int Health", який ламав логіку і викликав варнінги
    // ВИПРАВЛЕНО: Прибрано дубльований "Level" — він уже є в базовому класі Character
    // (раніше Player.Level приховував (shadowed) Character.Level, що давало
    // компіляторний warning CS0108 і ризик розсинхрону, якщо десь Player
    // використовується через посилання типу Character)

    public int Experience { get; set; } = 0;
    public int Gold { get; set; } = 100;
    public int Stamina { get; set; } = 100;
    public int MaxStamina { get; set; } = 100;
    public Inventory Inventory { get; set; } = new Inventory();

    // ── Екіпірування ─────────────────────────────────────────────────────────
    // Базові характеристики без урахування екіпіровки (потрібні, щоб можна було
    // знімати/міняти зброю чи броню без накопичення бонусів)
    public int BaseAttack { get; private set; }
    public int BaseDefense { get; private set; }

    public Weapon? EquippedWeapon { get; private set; }
    public Armor? EquippedArmor { get; private set; }

    public Player() : base() { }

    public Player(string name, PlayerClass playerClass, RaceType raceType) : base()
    {
        Name = name;
        MaxHealth = 100;
        Health = 100; // ВИПРАВЛЕНО: замість CurrentHealth
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
        Health = maxHealth; // ВИПРАВЛЕНО: замість CurrentHealth
        Attack = attack;
        Defense = defense;
        Speed = speed;
        BaseAttack  = Attack;
        BaseDefense = Defense;
    }

    // Одягає зброю (замінює попередню, якщо була). Стара зброя лишається в інвентарі.
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