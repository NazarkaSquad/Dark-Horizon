using Dark_Horizon.Classes.Core;
using Dark_Horizon.Classes.Entities;
using Dark_Horizon.Classes.Items;
using Dark_Horizon.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Dark_Horizon.Views
{
    public partial class Forge : UserControl
    {
        private Market _market = new Market();
        private Player? _player = null;
        private MainWindow? _mainWindow = null;

        public Forge()
        {
            InitializeComponent();
            this.Loaded += Forge_Loaded;
        }

        private void Forge_Loaded(object sender, RoutedEventArgs e)
        {
            _mainWindow = Window.GetWindow(this) as MainWindow;
            GameManager.Instance.LocationManager.SetCurrentLocation(LocationType.City);
            if (_mainWindow != null)
            {
                _player = GameManager.Instance.Player;
                UpdateUI();
                LoadCatalog();
            }
        }

        private void UpdateUI()
        {
            if (_player != null)
            {
                TxtPlayerGold.Text = _player.Gold.ToString();
            }
        }

        private void LoadCatalog()
        {
            try
            {
                if (_market.Stock == null || _market.Stock.Count == 0)
                {
                    _market.AddToStock(new Armor("Шкіряна броня", 30, 5, 1, "Легкий захист. Підходить для початківців.", "/Assets/Images/Items/LeatherArmor.png"));
                    _market.AddToStock(new Armor("Кольчуга", 120, 15, 3, "Середній захист. Для досвідчених воїнів.", "/Assets/Images/Items/ChainmailArmor.png"));

                    _market.AddToStock(new Weapon("Іржавий меч", 20, 10, 1, "Старий меч. Краще ніж нічого.", "/Assets/Images/Items/RustySword.png"));
                    _market.AddToStock(new Weapon("Сталевий меч", 80, 25, 3, "Надійний меч середньої якості.", "/Assets/Images/Items/IronSword.png"));
                    _market.AddToStock(new Weapon("Кинджал", 60, 18, 2, "Швидка зброя. Ідеальна для злодія.", "/Assets/Images/Items/Dagger.png"));
                }

                RefreshMarketUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка каталогу: {ex.Message}", "Кузня");
            }
        }

        private void RefreshMarketUI()
        {
            LstArmorStock.ItemsSource = null;
            LstArmorStock.ItemsSource = _market.Stock.Where(item => item is Armor).ToList();

            LstWeaponStock.ItemsSource = null;
            LstWeaponStock.ItemsSource = _market.Stock.Where(item => item is Weapon).ToList();
        }

        private void BtnInlineBuy_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Item selectedItem && _player != null)
            {
                // Купуємо предмет за твоєю стандартною логікою
                string resultMessage = _market.Buy(_player, selectedItem);

                // ХИТРІСТЬ ДЛЯ БЕЗКІНЕЧНОСТІ:
                // Якщо маркет внутрішньо видалив предмет із Stock, ми його одразу закидаємо назад
                if (!_market.Stock.Contains(selectedItem))
                {
                    _market.AddToStock(selectedItem);
                }

                UpdateUI();
                RefreshMarketUI(); // Оновлюємо вітрину (предмет залишиться на місці)
            }
        }

        private void BtnBackToTown_Click(object sender, RoutedEventArgs e)
        {
            if (_mainWindow != null)
            {
                _mainWindow.StartTransition(new Town());
            }
        }
    }
}