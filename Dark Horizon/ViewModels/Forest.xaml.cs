using System.Windows;
using System.Windows.Controls;
using Dark_Horizon.Classes.Core;

namespace Dark_Horizon.Views;

public partial class Forest : UserControl
{
    public Forest()
    {
        InitializeComponent();
        GameManager.Instance.LocationManager.SetCurrentLocation(LocationType.Forest);
    }

    private void BtnToOutskirt_Click(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow is MainWindow mw)
            mw.StartTransition(new Outskirt());
    }

    private void BtnToSwamp_Click(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow is MainWindow mw)
            mw.StartTransition(new Swamp());
    }
}
