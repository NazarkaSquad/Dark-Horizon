using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dark_Horizon.Classes.Core;
using Dark_Horizon.Classes.Entities;
using Dark_Horizon.Classes.Items;

namespace Dark_Horizon.Views;

public partial class Battle : UserControl
{
    private BodyPart? _selectedAttack;
    private BodyPart? _selectedDefend;

    private readonly Dictionary<BodyPart, Button> _atkBtns = new();
    private readonly Dictionary<BodyPart, Button> _defBtns = new();
    private readonly string _returnLocation;

    public Battle(string returnLocation = "Forest")
    {
        InitializeComponent();
        _returnLocation = returnLocation;

        Loaded += (_, _) =>
        {
            _atkBtns[BodyPart.Head] = BtnAtkHead;
            _atkBtns[BodyPart.Chest] = BtnAtkChest;
            _atkBtns[BodyPart.Stomach] = BtnAtkStomach;
            _atkBtns[BodyPart.Legs] = BtnAtkLegs;

            _defBtns[BodyPart.Head] = BtnDefHead;
            _defBtns[BodyPart.Chest] = BtnDefChest;
            _defBtns[BodyPart.Stomach] = BtnDefStomach;
            _defBtns[BodyPart.Legs] = BtnDefLegs;

            if (_returnLocation == "Swamp")
                BattleBg.Source = new BitmapImage(
                    new Uri("pack://application:,,,/Assets/Images/Backgrounds/Swamp.png"));

            RefreshUI();
        };
    }

    private void RefreshUI()
    {
        var gm = GameManager.Instance;
        var p = gm.Player;
        var bs = gm.CurrentBattle;
        if (p == null || bs == null) return;

        TxtPlayerName.Text = p.Name;
        TxtPlayerLevel.Text = $"РВ {p.Level}";
        TxtPlayerHp.Text = $"{p.Health} / {p.MaxHealth}";
        SetBar(PlayerHpFill, p.Health, p.MaxHealth, 556);

        Enemy? enemy = null;
        foreach (var ch in bs.AllParticipants)
        {
            if (ch is Enemy e && e.IsAlive)
            {
                enemy = e;
                break;
            }
        }

        if (enemy != null)
        {
            TxtEnemyName.Text = enemy.Name;
            TxtEnemyLevel.Text = $"РВ {enemy.Level}";
            TxtEnemyHp.Text = $"{enemy.Health} / {enemy.MaxHealth}";
            SetBar(EnemyHpFill, enemy.Health, enemy.MaxHealth, 556);

            try
            {
                string spritePath = $"pack://application:,,,/Assets/Images/Enemies/{enemy.SpriteKey}.png";
                EnemySprite.Source = new BitmapImage(new Uri(spritePath));
            }
            catch
            {
                EnemySprite.Source = new BitmapImage(
                    new Uri("pack://application:,,,/Assets/Images/Enemies/Wolf.png"));
            }
        }
        else
        {
            EnemySprite.Source = null;
        }
    }

    private static void SetBar(System.Windows.Shapes.Rectangle rect, int cur, int max, double fullWidth)
    {
        double pct = max > 0 ? (double)cur / max : 0;
        rect.Width = Math.Max(0, fullWidth * pct);

        rect.Fill = pct > 0.5 ? new SolidColorBrush(Color.FromRgb(0xC0, 0x39, 0x2B))
                  : pct > 0.25 ? new SolidColorBrush(Color.FromRgb(0xE6, 0x7E, 0x22))
                  : new SolidColorBrush(Color.FromRgb(0xFF, 0x20, 0x20));
    }

    private void AtkBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        _selectedAttack = Enum.Parse<BodyPart>((string)btn.Tag);

        foreach (var (_, b) in _atkBtns) b.Opacity = 1.0;
        btn.Opacity = 0.5;

        TryEnableFight();
    }

    private void DefBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        _selectedDefend = Enum.Parse<BodyPart>((string)btn.Tag);

        foreach (var (_, b) in _defBtns) b.Opacity = 1.0;
        btn.Opacity = 0.5;

        TryEnableFight();
    }

    private void TryEnableFight()
    {
        BtnFight.IsEnabled = _selectedAttack.HasValue && _selectedDefend.HasValue;
    }

    private void BtnFight_Click(object sender, RoutedEventArgs e)
    {
        if (!_selectedAttack.HasValue || !_selectedDefend.HasValue) return;

        var report = GameManager.Instance.ResolveTurn(_selectedAttack.Value, _selectedDefend.Value);
        AppendLog(report.Log);
        RefreshUI();

        foreach (var (_, b) in _atkBtns) b.Opacity = 1.0;
        foreach (var (_, b) in _defBtns) b.Opacity = 1.0;

        _selectedAttack = null;
        _selectedDefend = null;
        BtnFight.IsEnabled = false;

        HandleResult(report.FinalResult, report.EscapedSuccessfully);
    }

    private void BtnInventory_Click(object sender, RoutedEventArgs e)
    {
        FoodList.Children.Clear();
        var items = GameManager.Instance.Player?.Inventory.GetByType(ItemType.Food);
        if (items == null || items.Count == 0)
        {
            var empty = new TextBlock
            {
                Text = "Немає їжі чи зілля.",
                FontFamily = (FontFamily)FindResource("PF"),
                FontSize = 18,
                Foreground = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(0, 12, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            FoodList.Children.Add(empty);
        }
        else
        {
            foreach (var item in items)
            {
                if (item is not Food food) continue;

                var btn = new Button
                {
                    Width = 560,
                    Height = 60,
                    Margin = new Thickness(0, 6, 0, 0),
                    Style = (Style)FindResource("BattleButtonStyle"),
                    Content = $"🍞  {food.Name}  (+{food.HealAmount} HP)   [Використати]",
                    Tag = food
                };
                btn.Click += FoodItem_Click;
                FoodList.Children.Add(btn);
            }
        }

        InvOverlay.Visibility = Visibility.Visible;
    }

    private void FoodItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is Food food)
        {
            InvOverlay.Visibility = Visibility.Collapsed;
            var report = GameManager.Instance.UseItemInBattle(food);
            AppendLog(report.Log);
            RefreshUI();
            HandleResult(report.FinalResult, false);
        }
    }

    private void BtnCloseInv_Click(object sender, RoutedEventArgs e)
        => InvOverlay.Visibility = Visibility.Collapsed;

    private void BtnFlee_Click(object sender, RoutedEventArgs e)
    {
        var report = GameManager.Instance.TryEscape();
        AppendLog(report.Log);
        RefreshUI();
        HandleResult(report.FinalResult, report.EscapedSuccessfully);
    }

    private void HandleResult(BattleResult result, bool escaped)
    {
        if (escaped)
        {
            GoBack();
            return;
        }

        if (result == BattleResult.PlayerWon)
        {
            TxtEndTitle.Text = "ПЕРЕМОГА!";
            TxtEndDetails.Text = "Ти переміг ворога і отримав нагороду.";
            EndOverlay.Visibility = Visibility.Visible;
            LockActions();
        }
        else if (result == BattleResult.PlayerLost)
        {
            TxtEndTitle.Text = "💀  ПОРАЗКА";
            TxtEndDetails.Text = "Ти загинув у бою...";
            EndOverlay.Visibility = Visibility.Visible;
            LockActions();
        }
    }

    private void LockActions()
    {
        BtnFight.IsEnabled = false;
        BtnInventory.IsEnabled = false;
        BtnFlee.IsEnabled = false;
    }

    private void BtnEndContinue_Click(object sender, RoutedEventArgs e)
        => GoBack();

    private void GoBack()
    {
        if (Application.Current.MainWindow is MainWindow mw)
        {
            UserControl next = _returnLocation == "Swamp"
                ? (UserControl)new Swamp()
                : new Forest();
            mw.StartTransition(next);
        }
    }

    private void AppendLog(List<string> lines)
    {
        foreach (var line in lines)
            TxtLog.Text += "\n" + line;

        LogScroll.UpdateLayout();
        LogScroll.ScrollToBottom();
    }
}