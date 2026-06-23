using Dark_Horizon.Classes.Core;
using System.Windows;
using System.Windows.Controls;

namespace Dark_Horizon.Views;

public partial class Town : UserControl
{
    public Town()
    {
        InitializeComponent();
        GameManager.Instance.LocationManager.SetCurrentLocation(LocationType.City);
    }

        private void BtnTavern_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnForge_Click(object sender, RoutedEventArgs e)
        {
        if (Application.Current.MainWindow is MainWindow mainWindow)
            mainWindow.StartTransition(new Forge());
    }

        private void BtnAlchemist_Click(object sender, RoutedEventArgs e)
        {
        if (Application.Current.MainWindow is MainWindow mainWindow)
            mainWindow.StartTransition(new Alchemist());
    }

        private void BtnLeaveTown_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
                mainWindow.StartTransition(new Outskirt());
        }
    }