using System;
using System.Windows;
using System.Windows.Controls;
using Dark_Horizon.Classes.Core;

namespace Dark_Horizon.Views;

public partial class Settings : UserControl
{
    public Settings()
    {
        InitializeComponent();
    }

    public event EventHandler? OnSettingsClosed;

    private int tempSelectedMode = 0;

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyActualModeText();

        SliderAutoSave.Value = GameState.AutoSaveMinutes;
        TxtAutoSaveValue.Text = $"{GameState.AutoSaveMinutes} хв";
    }

    private void ApplyActualModeText()
    {
        if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) ||
            Application.Current?.MainWindow == null)
            return;

        if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
        {
            tempSelectedMode = 0;
            TxtCurrentMode.Text = "Повноекранний";
        }
        else
        {
            tempSelectedMode = 1;
            TxtCurrentMode.Text = "Віконний";
        }
    }

    private void SliderAutoSave_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        int minutes = (int)e.NewValue;
        if (TxtAutoSaveValue != null)
            TxtAutoSaveValue.Text = $"{minutes} хв";

        GameState.AutoSaveMinutes = minutes;
    }

    private void BtnScreenModeTrigger_Click(object sender, RoutedEventArgs e)
    {
        ComboDropdownPanel.Visibility = BtnScreenModeTrigger.IsChecked == true
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    private void BtnSelectFullscreen_Click(object sender, RoutedEventArgs e)
    {
        tempSelectedMode = 0;
        TxtCurrentMode.Text = "Повноекранний";
        CloseDropdown();
    }

    private void BtnSelectWindowed_Click(object sender, RoutedEventArgs e)
    {
        tempSelectedMode = 1;
        TxtCurrentMode.Text = "Віконний";
        CloseDropdown();
    }

    private void BtnConfirm_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = Application.Current.MainWindow;
        if (mainWindow != null)
        {
            if (tempSelectedMode == 0)
            {
                mainWindow.WindowStyle = WindowStyle.None;
                mainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWindow.Width = 1296;
                mainWindow.Height = 759;
                mainWindow.Left = (SystemParameters.PrimaryScreenWidth - mainWindow.Width) / 2;
                mainWindow.Top = (SystemParameters.PrimaryScreenHeight - mainWindow.Height) / 2;
            }
        }

        CloseDropdown();

        OnSettingsClosed?.Invoke(this, EventArgs.Empty);
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        CloseDropdown();
        ApplyActualModeText();
        OnSettingsClosed?.Invoke(this, EventArgs.Empty);
    }

    private void CloseDropdown()
    {
        ComboDropdownPanel.Visibility = Visibility.Collapsed;
        BtnScreenModeTrigger.IsChecked = false;
    }
    private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        AudioManager.SetVolume(VolumeSlider.Value);

        Properties.Settings.Default.MusicVolume = VolumeSlider.Value;
        Properties.Settings.Default.Save();
    }
}