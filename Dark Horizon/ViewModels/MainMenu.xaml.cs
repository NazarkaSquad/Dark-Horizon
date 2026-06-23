using Dark_Horizon.Classes.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Dark_Horizon.Views;

public partial class MainMenu : UserControl
{
    private bool isSettingsOpen = false;
    private bool isSlotSelectOpen = false;
    private bool isCharacterCreateOpen = false;

    public MainMenu()
    {
        InitializeComponent();
        SettingsPanel.OnSettingsClosed += SettingsPanel_OnSettingsClosed;
        SlotSelectPanel.OnBackClosed += SlotSelectPanel_OnBackClosed;
        SlotSelectPanel.OnCharacterCreateRequested += SlotSelectPanel_OnCharacterCreateRequested;
        CharacterCreatePanel.OnBackClosed += CharacterCreatePanel_OnBackClosed;

        GameState.StopAutoSave();
    }

    private void SettingsPanel_OnSettingsClosed(object? sender, EventArgs e)
    {
        if (isSettingsOpen)
            ToggleSettings();
    }

    private void BtnPlay_Click(object sender, RoutedEventArgs e)
    {
        ToggleSlotSelect(true);
    }

    private void SlotSelectPanel_OnBackClosed(object? sender, EventArgs e)
    {
        ToggleSlotSelect(false);
    }

    private void ToggleSlotSelect(bool open)
    {
        if (isSlotSelectOpen == open) return;
        isSlotSelectOpen = open;

        TimeSpan duration = TimeSpan.FromSeconds(0.4);
        IEasingFunction ease = new CubicEase { EasingMode = EasingMode.EaseOut };

        DoubleAnimation menuAnim = new DoubleAnimation { Duration = duration, EasingFunction = ease };
        DoubleAnimation slotAnim = new DoubleAnimation { Duration = duration, EasingFunction = ease };
        DoubleAnimation settingsAnim = new DoubleAnimation { Duration = duration, EasingFunction = ease };

        if (open)
        {
            menuAnim.From = Canvas.GetLeft(MainMenuContent);
            menuAnim.To = -1920; 

            slotAnim.From = 1920;
            slotAnim.To = 0;

            if (isSettingsOpen)
            {
                settingsAnim.From = Canvas.GetLeft(SettingsContainer);
                settingsAnim.To = 700 - 1920;
            }
        }
        else
        {
            menuAnim.From = Canvas.GetLeft(MainMenuContent);
            menuAnim.To = 0;

            slotAnim.From = Canvas.GetLeft(SlotSelectContainer);
            slotAnim.To = 1920;

            if (isSettingsOpen)
            {
                settingsAnim.From = Canvas.GetLeft(SettingsContainer);
                settingsAnim.To = 700;
            }
        }

        MainMenuContent.BeginAnimation(Canvas.LeftProperty, menuAnim);
        SlotSelectContainer.BeginAnimation(Canvas.LeftProperty, slotAnim);

        if (isSettingsOpen)
        {
            SettingsContainer.BeginAnimation(Canvas.LeftProperty, settingsAnim);
        }
    }

    private void SlotSelectPanel_OnCharacterCreateRequested(object? sender, EventArgs e)
    {
        ToggleCharCreate(true);
    }

    private void CharacterCreatePanel_OnBackClosed(object? sender, EventArgs e)
    {
        ToggleCharCreate(false);
    }

    private void ToggleCharCreate(bool open)
    {
        if (isCharacterCreateOpen == open) return;
        isCharacterCreateOpen = open;

        TimeSpan duration = TimeSpan.FromSeconds(0.4);
        IEasingFunction ease = new CubicEase { EasingMode = EasingMode.EaseOut };

        DoubleAnimation slotAnim = new DoubleAnimation { Duration = duration, EasingFunction = ease };
        DoubleAnimation charAnim = new DoubleAnimation { Duration = duration, EasingFunction = ease };

        if (open)
        {
            slotAnim.From = Canvas.GetLeft(SlotSelectContainer);
            slotAnim.To = -1920;

            charAnim.From = 1920;
            charAnim.To = 0;
        }
        else
        {
            slotAnim.From = Canvas.GetLeft(SlotSelectContainer);
            slotAnim.To = 0;

            charAnim.From = Canvas.GetLeft(CharacterCreateContainer);
            charAnim.To = 1920;
        }

        SlotSelectContainer.BeginAnimation(Canvas.LeftProperty, slotAnim);
        CharacterCreateContainer.BeginAnimation(Canvas.LeftProperty, charAnim);
    }

    private void BtnLoad_Click(object sender, RoutedEventArgs e)
    {
        ToggleSlotSelect(true);
    }


    private void BtnSettings_Click(object sender, RoutedEventArgs e)
    {
        ToggleSettings();
    }

    private void ToggleSettings()
    {
        DoubleAnimation slideAnim = new DoubleAnimation();
        slideAnim.Duration = TimeSpan.FromSeconds(0.2);

        if (!isSettingsOpen)
        {
            SettingsContainer.Visibility = Visibility.Visible;
            slideAnim.From = 1920;
            slideAnim.To = 700;
            isSettingsOpen = true;
        }
        else
        {
            slideAnim.From = 700;
            slideAnim.To = 1920;
            isSettingsOpen = false;
            slideAnim.Completed += (s, ev) =>
            {
                SettingsContainer.Visibility = Visibility.Collapsed;
            };
        }

        SettingsContainer.BeginAnimation(Canvas.LeftProperty, slideAnim);
    }

    private void BtnExit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}