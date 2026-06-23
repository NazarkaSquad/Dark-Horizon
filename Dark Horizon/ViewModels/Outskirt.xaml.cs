using System.Windows;
using System.Windows.Controls;
using Dark_Horizon.Classes.Core;

namespace Dark_Horizon.Views;

    public partial class Outskirt : UserControl
    {
        public Outskirt()
        {
            InitializeComponent();
            GameManager.Instance.LocationManager.SetCurrentLocation(LocationType.Outskirt);
        }

        private void BtnToCity_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.StartTransition(new Town());
            }
        }

        private void BtnToLeftLoc_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Тут буде перехід в ліву локацію!");
        }

        private void BtnToRightLoc_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Тут буде перехід в праву локацію!");
        }

        private void BtnToBottomLoc_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.StartTransition(new Forest());
            }
        }
    }
