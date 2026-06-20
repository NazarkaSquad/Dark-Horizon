using Dark_Horizon.Classes.Core;
using Dark_Horizon.Classes.Entities;
using Dark_Horizon.Classes.Items;
using Dark_Horizon.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Dark_Horizon.Views
{
    public partial class Alchemist : UserControl
    {
        private Market _market = new Market();
        private Player? _player = null;
        private MainWindow? _mainWindow = null;

        public Alchemist()
        {
            InitializeComponent();
            this.Loaded += Alchemist_Loaded;
        }

        private void Alchemist_Loaded(object sender, RoutedEventArgs e)
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
            // Якщо каталог порожній — просто додаємо наше єдине зілля
            if (_market.Stock == null || _market.Stock.Count == 0)
            {
                _market.AddToStock(new Food(
                    "Зілля здоров'я",
                    50,
                    999,
                    "Повністю відновлює HP.",
                    "/Assets/Images/Items/HealPotion.png"
                ));
            }

            RefreshMarketUI();
        }

        private void RefreshMarketUI()
        {
            // Оскільки тут НІЧОГО крім зіль немає, просто перетворюємо весь Stock у список
            LstPotionStock.ItemsSource = _market.Stock.ToList();
        }

        private void BtnInlineBuy_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Item selectedItem && _player != null)
            {
                string resultMessage = _market.Buy(_player, selectedItem);

                // Твоя логіка нескінченного товару
                if (!_market.Stock.Contains(selectedItem))
                {
                    _market.AddToStock(selectedItem);
                }

                UpdateUI();
                RefreshMarketUI();
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