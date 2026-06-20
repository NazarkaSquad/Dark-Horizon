using System;
using Dark_Horizon.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using Dark_Horizon.Classes.Core;
using Dark_Horizon.Classes.Entities;

namespace Dark_Horizon;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Завантажуємо екран головного меню
        MainContentPresenter.Content = new MainMenu();

        // 🎵 ЗАПУСКАЄМО МУЗИКУ ПРИ СТАРТІ ГРИ
        AudioManager.PlayBGM("Anxious_Heart.mp3");
        InGameSettingsView.OnSettingsClosed += InGameSettingsView_OnSettingsClosed;
    }

    private void BtnMenu_Click(object sender, RoutedEventArgs e)
    {
        PauseMenuOverlay.Visibility = Visibility.Visible;
        BtnMenu.Visibility = Visibility.Collapsed;
    }

    private void BtnResume_Click(object sender, RoutedEventArgs e)
    {
        PauseMenuOverlay.Visibility = Visibility.Collapsed;
        BtnMenu.Visibility = Visibility.Visible;
    }

    private void BtnInGameSettings_Click(object sender, RoutedEventArgs e)
    {
        PauseMenuOverlay.Visibility = Visibility.Collapsed;
        InGameSettingsContainer.Visibility = Visibility.Visible;
    }

    private async void BtnSaveGame_Click(object sender, RoutedEventArgs e)
    {
        if (GameManager.Instance.Player == null) return;

        GameState.SaveCurrentGame(GameManager.Instance.CurrentLocation.Name);

        TxtSaveFeedback.Visibility = Visibility.Visible;
        await Task.Delay(1200);
        TxtSaveFeedback.Visibility = Visibility.Collapsed;
    }

    private void InGameSettingsView_OnSettingsClosed(object? sender, EventArgs e)
    {
        InGameSettingsContainer.Visibility = Visibility.Collapsed;
        PauseMenuOverlay.Visibility = Visibility.Visible;
    }

    // ── Інвентар (відкривається з HudPanel поверх поточного екрану) ────────────
    public void OpenInventory()
    {
        InventoryOverlayView.Refresh();
        InventoryOverlayContainer.Visibility = Visibility.Visible;
    }

    public void CloseInventory()
    {
        InventoryOverlayContainer.Visibility = Visibility.Collapsed;
    }

    private void BtnInGameExit_Click(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow is MainWindow mainWindow)
        {
            mainWindow.StartTransition(new MainMenu());
            PauseMenuOverlay.Visibility = Visibility.Collapsed;
        }
    }

    public void StartTransition(UserControl nextScreen)
    {
        CloseInventory();

        if (nextScreen is MainMenu)
        {
            BtnMenu.Visibility = Visibility.Collapsed;
        }
        else
        {
            BtnMenu.Visibility = Visibility.Visible;
        }

        double halfWidth = 960;
        double halfHeight = 540;
        TimeSpan duration = TimeSpan.FromSeconds(1);

        TransitionOverlay.IsHitTestVisible = true;

        DoubleAnimation topAnim = new(0, halfHeight, duration);
        DoubleAnimation bottomAnim = new(0, halfHeight, duration);
        DoubleAnimation leftAnim = new(0, halfWidth, duration);
        DoubleAnimation rightAnim = new(0, halfWidth, duration);

        Storyboard closingStoryboard = new();
        closingStoryboard.Children.Add(topAnim);
        closingStoryboard.Children.Add(bottomAnim);
        closingStoryboard.Children.Add(leftAnim);
        closingStoryboard.Children.Add(rightAnim);

        Storyboard.SetTarget(topAnim, TopCurtain);
        Storyboard.SetTargetProperty(topAnim, new(Rectangle.HeightProperty));
        Storyboard.SetTarget(bottomAnim, BottomCurtain);
        Storyboard.SetTargetProperty(bottomAnim, new(Rectangle.HeightProperty));
        Storyboard.SetTarget(leftAnim, LeftCurtain);
        Storyboard.SetTargetProperty(leftAnim, new(Rectangle.WidthProperty));
        Storyboard.SetTarget(rightAnim, RightCurtain);
        Storyboard.SetTargetProperty(rightAnim, new(Rectangle.WidthProperty));

        closingStoryboard.Completed += (_, _) =>
        {
            MainContentPresenter.Content = nextScreen;

            DoubleAnimation topOpen = new(halfHeight, 0, duration);
            DoubleAnimation bottomOpen = new(halfHeight, 0, duration);
            DoubleAnimation leftOpen = new(halfWidth, 0, duration);
            DoubleAnimation rightOpen = new(halfWidth, 0, duration);

            Storyboard openingStoryboard = new();
            openingStoryboard.Children.Add(topOpen);
            openingStoryboard.Children.Add(bottomOpen);
            openingStoryboard.Children.Add(leftOpen);
            openingStoryboard.Children.Add(rightOpen);

            Storyboard.SetTarget(topOpen, TopCurtain);
            Storyboard.SetTargetProperty(topOpen, new(Rectangle.HeightProperty));
            Storyboard.SetTarget(bottomOpen, BottomCurtain);
            Storyboard.SetTargetProperty(bottomOpen, new(Rectangle.HeightProperty));
            Storyboard.SetTarget(leftOpen, LeftCurtain);
            Storyboard.SetTargetProperty(leftOpen, new(Rectangle.WidthProperty));
            Storyboard.SetTarget(rightOpen, RightCurtain);
            Storyboard.SetTargetProperty(rightOpen, new(Rectangle.WidthProperty));

            openingStoryboard.Completed += (_, _) =>
            {
                TransitionOverlay.IsHitTestVisible = false;
            };

            openingStoryboard.Begin();
        };

        closingStoryboard.Begin();
    }
}