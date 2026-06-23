using System;
using System.Collections.Generic;
using System.Linq;
using Dark_Horizon.Classes.Entities;
using Dark_Horizon.Classes.Core.Database.Models;
using Dark_Horizon.Classes.Items;

namespace Dark_Horizon.Classes.Core;

public class GameManager
{
    private static GameManager? _instance;
    public static GameManager Instance => _instance ??= new GameManager();

    public Player?         Player          { get; private set; }
    public BattleSystem?   CurrentBattle   { get; private set; }
    public LocationManager LocationManager { get; private set; }
    public bool            IsInBattle      { get; private set; }

    public GameManager()
    {
        Player          = new Player("Hero", 100, 15, 5, 10);
        CurrentBattle   = new BattleSystem();
        LocationManager = new LocationManager();
    }

    public void StartGame(string playerName, PlayerClass playerClass, RaceType raceType)
    {
        Player = new Player(playerName, playerClass, raceType);
        GameState.StartAutoSave();
    }

    public void LoadFromSave(SaveData save)
    {
        var race  = Enum.TryParse<RaceType>   (save.PlayerRace,  out var rt) ? rt : RaceType.Human;
        var cls   = Enum.TryParse<PlayerClass>(save.PlayerClass, out var pc) ? pc : PlayerClass.Warrior;
        Player = new Player(save.PlayerName, cls, race)
        {
            Health    = save.Health,
            MaxHealth = save.MaxHealth,
            Gold      = save.Gold,
            Level     = save.Level,
        };
        GameState.StartAutoSave();
    }

    public void StartNewBattle(List<Enemy> enemies)
    {
        if (Player == null || CurrentBattle == null) return;

        CurrentBattle = new BattleSystem();
        CurrentBattle.OnBattleEnded += HandleBattleResult;

        IsInBattle = true;
        CurrentBattle.StartBattle(Player, enemies);
    }

    public string? StartAdventure()
    {
        if (Player == null) return "Спочатку створи персонажа.";

        var loc = LocationManager.CurrentLocation;

        if (loc.Type == LocationType.City || loc.Type == LocationType.Tavern || loc.Type == LocationType.Outskirt)
            return $"Ти в мирній зоні ({loc.Name}). Пошук пригод тут неможливий!";

        List<Enemy> pool = loc.Type switch
        {
            LocationType.Forest => Enemy.ForestPool(),
            LocationType.Swamp  => Enemy.SwampPool(),
            _                   => new List<Enemy>()
        };

        if (pool.Count == 0)
            return "Тут поки тихо — ворогів немає.";

        var rng  = new Random();
        int count = rng.Next(1, 3);
        var chosen = pool.OrderBy(_ => rng.Next()).Take(count).ToList();

        StartNewBattle(chosen);
        return null;
    }

    public TurnReport ResolveTurn(BodyPart attack, BodyPart defend)
    {
        if (CurrentBattle == null)
            return new TurnReport { FinalResult = BattleResult.Ongoing };
        return CurrentBattle.ResolveTurn(attack, defend);
    }

    public TurnReport UseItemInBattle(Food food)
    {
        if (CurrentBattle == null)
            return new TurnReport { FinalResult = BattleResult.Ongoing };
        return CurrentBattle.UseItem(food);
    }

    public TurnReport TryEscape()
    {
        if (CurrentBattle == null)
            return new TurnReport { FinalResult = BattleResult.Ongoing };

        var report = CurrentBattle.TryEscape();
        if (report.EscapedSuccessfully)
        {
            IsInBattle    = false;
            CurrentBattle = new BattleSystem();
        }
        return report;
    }

    private void HandleBattleResult(bool victory)
    {
        IsInBattle = false;
        if (victory && CurrentBattle != null && Player != null)
        {
            var (exp, gold) = CurrentBattle.GetRewards();
            Player.Experience += exp;
            Player.Gold       += gold;
        }
        else if (Player != null)
        {
            Player.Health = 1;
        }
    }
    public Location CurrentLocation => LocationManager.CurrentLocation;
    public Location? MoveNorth()    => LocationManager.MoveNorth();
    public Location? MoveSouth()    => LocationManager.MoveSouth();
    public Location? MoveEast()     => LocationManager.MoveEast();
    public Location? MoveWest()     => LocationManager.MoveWest();

    public string RestInTavern(int cost = 20)
    {
        if (Player == null)              return "Гравець не створений.";
        if (!CurrentLocation.HasTavern)  return "Тут немає де прилягти.";
        if (Player.Gold < cost)          return $"Потрібно {cost} золота.";
        Player.Gold   -= cost;
        Player.Health  = Player.MaxHealth;
        Player.Stamina = Player.MaxStamina;
        return "Ти відпочив у таверні — HP та витривалість відновлено!";
    }
}
