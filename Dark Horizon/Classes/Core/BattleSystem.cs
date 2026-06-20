using System;
using System.Collections.Generic;
using System.Linq;
using Dark_Horizon.Classes.Entities;
using Dark_Horizon.Classes.Items;

namespace Dark_Horizon.Classes.Core;

// ── Перелічення ──────────────────────────────────────────────────────────────
public enum BattleResult { Ongoing, PlayerWon, PlayerLost }
public enum BodyPart     { Head, Chest, Stomach, Legs }

// ── Підсумок одного ходу ─────────────────────────────────────────────────────
public class TurnReport
{
    public List<string> Log          { get; } = new();
    public bool  PlayerHitLanded     { get; set; }
    public bool  EnemyHitLanded      { get; set; }
    public int   DamageToEnemy       { get; set; }
    public int   DamageToPlayer      { get; set; }
    public bool  PlayerCrit          { get; set; }
    public bool  EnemyCrit           { get; set; }
    public bool  EscapedSuccessfully { get; set; }
    public BattleResult FinalResult  { get; set; } = BattleResult.Ongoing;
}

public class BattleSystem
{
    public event Action<bool>? OnBattleEnded;
    public List<Character> AllParticipants { get; private set; } = new();

    private Player?     _player;
    private List<Enemy> _enemies = new();
    private readonly Random _rng = new();

    // Шанс влучити при базовій атаці (без модифікаторів)
    private const double BASE_HIT = 0.80;

    // Модифікатори влучання по частині тіла (голова — важче влучити, але боляче)
    private static readonly Dictionary<BodyPart, double> HitMod = new()
    {
        { BodyPart.Head,    -0.20 },
        { BodyPart.Chest,   +0.00 },
        { BodyPart.Stomach, +0.05 },
        { BodyPart.Legs,    +0.10 },
    };

    // Множник шкоди
    private static readonly Dictionary<BodyPart, double> DmgMul = new()
    {
        { BodyPart.Head,    1.50 },
        { BodyPart.Chest,   1.00 },
        { BodyPart.Stomach, 0.90 },
        { BodyPart.Legs,    0.75 },
    };

    // Шанс повного блоку, якщо захищають ту саму частину, куди б'ють
    private static readonly Dictionary<BodyPart, double> BlockChance = new()
    {
        { BodyPart.Head,    0.55 },
        { BodyPart.Chest,   0.60 },
        { BodyPart.Stomach, 0.65 },
        { BodyPart.Legs,    0.75 },
    };

    // ── API ───────────────────────────────────────────────────────────────────
    public void StartBattle(Player player, List<Enemy> enemies)
    {
        _player  = player;
        _enemies = enemies;

        AllParticipants.Clear();
        AllParticipants.Add(player);
        AllParticipants.AddRange(enemies);
    }

    private Enemy? LiveTarget => _enemies.FirstOrDefault(e => e.IsAlive);

    /// <summary>
    /// Гравець обирає куди бити і що захищати — одночасно.
    /// Ворог теж обирає випадково. Рахується обмін ударами.
    /// </summary>
    public TurnReport ResolveTurn(BodyPart attackPart, BodyPart defendPart)
    {
        var report = new TurnReport();
        if (_player == null) return report;

        var target = LiveTarget;
        if (target == null)
        {
            report.Log.Add("Усі вороги переможені.");
            report.FinalResult = BattleResult.PlayerWon;
            return report;
        }

        var enemyAtk = RandPart();
        var enemyDef = RandPart();

        ResolveHit(_player, target, attackPart, enemyDef, report, isPlayer: true);

        if (!target.IsAlive)
        {
            report.Log.Add($"{target.Name} переможений!");
        }
        else
        {
            ResolveHit(target, _player, enemyAtk, defendPart, report, isPlayer: false);
        }

        report.FinalResult = CheckResult();
        return report;
    }

    /// <summary>Використання їжі/зілля. Ворог встигає вдарити у відповідь.</summary>
    public TurnReport UseItem(Food food)
    {
        var report = new TurnReport();
        if (_player == null) return report;

        int healed = _player.Heal(food.HealAmount);
        _player.Inventory.RemoveItem(food);

        report.Log.Add(healed > 0
            ? $"Ти використав {food.Name} і відновив {healed} HP."
            : $"Ти використав {food.Name}, але HP вже повне.");

        // Ворог б'є поки ти копаєшся в інвентарі — без захисту
        var target = LiveTarget;
        if (target != null)
            ResolveHit(target, _player, RandPart(), RandPart(), report, isPlayer: false);

        report.FinalResult = CheckResult();
        return report;
    }

    /// <summary>Спроба втечі. 50% шанс. При успіху — -25% MaxHP. При провалі — ворог б'є.</summary>
    public TurnReport TryEscape()
    {
        var report = new TurnReport();
        if (_player == null) return report;

        bool ok = _rng.Next(2) == 1;
        if (ok)
        {
            int pen = (int)Math.Ceiling(_player.MaxHealth * 0.25);
            _player.Health = Math.Max(1, _player.Health - pen);
            report.Log.Add($"Ти втік з бою, але втратив {pen} HP від поспіху.");
            report.EscapedSuccessfully = true;
            report.FinalResult = BattleResult.Ongoing; // бій зупинено зовні
            return report;
        }

        report.Log.Add("Втекти не вдалося!");
        var target = LiveTarget;
        if (target != null)
            ResolveHit(target, _player, RandPart(), RandPart(), report, isPlayer: false);

        report.FinalResult = CheckResult();
        return report;
    }

    public BattleResult CheckResult()
    {
        if (_player != null && !_player.IsAlive)
        {
            OnBattleEnded?.Invoke(false);
            return BattleResult.PlayerLost;
        }
        if (_enemies.Count > 0 && _enemies.All(e => !e.IsAlive))
        {
            OnBattleEnded?.Invoke(true);
            return BattleResult.PlayerWon;
        }
        return BattleResult.Ongoing;
    }

    public (int exp, int gold) GetRewards()
    {
        int exp  = _enemies.Sum(e => e.RewardExp);
        int gold = _enemies.Sum(e => e.RewardGold);
        return (exp, gold);
    }

    // ── Утиліти ──────────────────────────────────────────────────────────────
    private void ResolveHit(Character attacker, Character defender,
                            BodyPart atkPart, BodyPart defPart,
                            TurnReport report, bool isPlayer)
    {
        string partName = PartUa(atkPart);
        double hitChance = BASE_HIT + HitMod[atkPart];
        bool hit = _rng.NextDouble() < hitChance;

        if (!hit)
        {
            report.Log.Add(isPlayer
                ? $"Ти промахнувся по {partName}."
                : $"{attacker.Name} промахнувся по твоїй {partName}.");
            return;
        }

        bool blocked = (atkPart == defPart) && _rng.NextDouble() < BlockChance[atkPart];
        if (blocked)
        {
            report.Log.Add(isPlayer
                ? $"{defender.Name} заблокував твій удар по {partName}!"
                : $"Ти заблокував удар по {partName}!");
            return;
        }

        int raw    = (int)Math.Round(Math.Max(1, attacker.Attack - defender.Defense * 0.5) * DmgMul[atkPart]);
        int damage = Math.Max(1, raw);
        defender.Health = Math.Max(0, defender.Health - damage);

        bool crit = atkPart == BodyPart.Head;
        if (isPlayer)
        {
            report.PlayerHitLanded = true;
            report.DamageToEnemy   = damage;
            report.PlayerCrit      = crit;
            report.Log.Add($"Ти влучив у {partName}{(crit ? " (крит!)" : "")} → {damage} шкоди.");
        }
        else
        {
            report.EnemyHitLanded  = true;
            report.DamageToPlayer  = damage;
            report.EnemyCrit       = crit;
            report.Log.Add($"{attacker.Name} влучив по твоїй {partName}{(crit ? " (крит!)" : "")} → {damage} шкоди.");
        }
    }

    private BodyPart RandPart()
    {
        var vals = Enum.GetValues<BodyPart>();
        return vals[_rng.Next(vals.Length)];
    }

    public static string PartUa(BodyPart p) => p switch
    {
        BodyPart.Head    => "голові",
        BodyPart.Chest   => "грудях",
        BodyPart.Stomach => "животі",
        BodyPart.Legs    => "ногах",
        _                => "тілі",
    };
}
