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

        // Нижня стрілочка
        private void BtnToBottomLoc_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.StartTransition(new Forest());
            }
        }
    }
