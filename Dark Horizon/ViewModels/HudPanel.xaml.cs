using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Dark_Horizon.Classes.Core;

namespace Dark_Horizon.Views;

public partial class HudPanel : UserControl
{
    public string CurrentLocationKey { get; set; } = "Forest";

    public HudPanel()
    {
        InitializeComponent();
    }

    private void BtnQuests_Click(object sender, RoutedEventArgs e)
    {
        ShowToast("Квести — незабаром!");
    }

    private void BtnMap_Click(object sender, RoutedEventArgs e)
    {
        ShowToast("Мапа — незабаром!");
    }

    private void BtnInventory_Click(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow is MainWindow mw)
            mw.OpenInventory();
    }

    private void BtnAdventure_Click(object sender, RoutedEventArgs e)
    {
        string? error = GameManager.Instance.StartAdventure();

        if (error != null)
        {
            ShowToast(error);
            return;
        }

        if (Application.Current.MainWindow is MainWindow mw)
        {
            mw.StartTransition(new Battle(CurrentLocationKey));
        }
    }

    private async void ShowToast(string message)
    {
        TxtToast.Text   = message;
        ToastPopup.IsOpen = true;
        await Task.Delay(2500);
        ToastPopup.IsOpen = false;
    }
}
