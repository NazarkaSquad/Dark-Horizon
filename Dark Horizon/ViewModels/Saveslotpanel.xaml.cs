using System.Windows;
using System.Windows.Controls;
using Dark_Horizon.Classes.Core.Database.Models;

namespace Dark_Horizon.Views
{
    public partial class Saveslotpanel : UserControl
    {
        public int SlotNumber { get; private set; }
        public SaveData? SaveData { get; private set; }

        public event EventHandler<int>? SlotSelected;

        public Saveslotpanel()
        {
            InitializeComponent();
        }

        public void Setup(int slotNumber, SaveData? saveData)
        {
            SlotNumber = slotNumber;
            SaveData = saveData;

            TxtSlotTitle.Text = $"СЛОТ {slotNumber}";

            if (saveData != null)
            {
                TxtCharInfo.Text = $"{saveData.PlayerName}  ·   Рівень {saveData.Level}";
                TxtCharInfo.Foreground = System.Windows.Media.Brushes.White;
                TxtSaveDate.Text = $"Збережено: {saveData.SavedAt:dd.MM.yyyy  HH:mm}";
            }
            else
            {
                TxtCharInfo.Text = "— Порожньо —";
                TxtSaveDate.Text = "";
            }
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            SlotSelected?.Invoke(this, SlotNumber);
        }
    }
}