using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dark_Horizon.Classes.Core;
using Dark_Horizon.Classes.Core.Database;
using Dark_Horizon.Classes.Entities;
using Dark_Horizon.Classes.Items;

namespace Dark_Horizon.Views;

public partial class InventoryOverlay : UserControl
{
    private class InventoryRow
    {
        public Item Item { get; set; } = null!;
        public string Name => Item.Name;
        public string Description => Item.Description;
        public string IconPath => Item.IconPath;
        public string StatText { get; set; } = "";
        public string ActionLabel { get; set; } = "";
        public bool CanAct { get; set; } = true;
    }

    private Player? _player;

    public InventoryOverlay()
    {
        InitializeComponent();
    }

    public void Refresh()
    {
        _player = GameManager.Instance.Player;
        if (_player == null) return;

        TxtPlayerName.Text = _player.Name;
        TxtPlayerLevel.Text = $"РІВЕНЬ {_player.Level}";
        TxtGold.Text = _player.Gold.ToString();

        TxtTotalAttack.Text = $"⚔ {_player.Attack}";
        TxtTotalDefense.Text = $"🛡 {_player.Defense}";
        TxtTotalHealth.Text = $"♥ {_player.Health}/{_player.MaxHealth}";

        string imageName = "WElf.png";
        string fullImagePath = $"pack://application:,,,/Assets/Images/Characters/{imageName}";

        try
        {
            ImgPlayerCharacter.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(fullImagePath));
        }
        catch
        {
            ImgPlayerCharacter.Source = new System.Windows.Media.Imaging.BitmapImage(
                new System.Uri("pack://application:,,,/Assets/Images/Characters/MHuman.png"));
        }

        double ratio = _player.MaxHealth > 0 ? (double)_player.Health / _player.MaxHealth : 0;
        ratio = System.Math.Clamp(ratio, 0, 1);
        HpBarFill.Height = 260 * ratio;

        HpBarFill.Fill = ratio > 0.5
            ? new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2E7D32"))
            : ratio > 0.25
                ? new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#D4A017"))
                : new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#C0392B"));

        if (_player.EquippedWeapon != null)
        {
            TxtWeaponSlot.Text = $"{_player.EquippedWeapon.Name} (Урон +{_player.EquippedWeapon.Damage})";
            BtnUnequipWeapon.Visibility = Visibility.Visible;
        }
        else
        {
            TxtWeaponSlot.Text = "Не одягнено";
            BtnUnequipWeapon.Visibility = Visibility.Collapsed;
        }

        if (_player.EquippedArmor != null)
        {
            TxtArmorSlot.Text = $"{_player.EquippedArmor.Name} (Захист +{_player.EquippedArmor.Defense})";
            BtnUnequipArmor.Visibility = Visibility.Visible;
        }
        else
        {
            TxtArmorSlot.Text = "Не одягнено";
            BtnUnequipArmor.Visibility = Visibility.Collapsed;
        }

        RefreshList();
    }

    private void RefreshList()
    {
        if (_player == null) return;

        IEnumerable<Item> items = _player.Inventory.Items;

        if (FilterWeapon.IsChecked == true) items = items.Where(i => i.Type == ItemType.Weapon);
        else if (FilterArmor.IsChecked == true) items = items.Where(i => i.Type == ItemType.Armor);
        else if (FilterFood.IsChecked == true) items = items.Where(i => i.Type == ItemType.Food);

        var rows = new List<InventoryRow>();
        foreach (var item in items)
        {
            var row = new InventoryRow { Item = item };

            switch (item)
            {
                case Weapon w:
                    bool weaponEquipped = _player.EquippedWeapon == w;
                    row.StatText = $"Урон: {w.Damage}";
                    row.ActionLabel = weaponEquipped ? "Одягнено" : "Одягнути";
                    row.CanAct = !weaponEquipped;
                    break;

                case Armor a:
                    bool armorEquipped = _player.EquippedArmor == a;
                    row.StatText = $"Захист: {a.Defense}";
                    row.ActionLabel = armorEquipped ? "Одягнено" : "Одягнути";
                    row.CanAct = !armorEquipped;
                    break;

                case Food f:
                    row.StatText = $"Лікує: {f.HealAmount}";
                    row.ActionLabel = "Їсти";
                    row.CanAct = _player.Health < _player.MaxHealth;
                    break;
            }

            rows.Add(row);
        }

        ItemsList.ItemsSource = rows;
    }

    private void Filter_Changed(object sender, RoutedEventArgs e) => RefreshList();

    private void BtnRowAction_Click(object sender, RoutedEventArgs e)
    {
        if (_player == null) return;
        if (sender is not Button btn || btn.DataContext is not InventoryRow row) return;

        switch (row.Item)
        {
            case Weapon w:
                _player.EquipWeapon(w);
                break;

            case Armor a:
                _player.EquipArmor(a);
                break;

            case Food f:
                f.Use(_player);
                _player.Inventory.RemoveItem(f);
                break;
        }

        Refresh();
    }

    private void BtnUnequipWeapon_Click(object sender, RoutedEventArgs e)
    {
        _player?.UnequipWeapon();
        Refresh();
    }

    private void BtnUnequipArmor_Click(object sender, RoutedEventArgs e)
    {
        _player?.UnequipArmor();
        Refresh();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow is MainWindow mw)
            mw.CloseInventory();
    }

    private void Backdrop_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (Application.Current.MainWindow is MainWindow mw)
            mw.CloseInventory();
    }
}
