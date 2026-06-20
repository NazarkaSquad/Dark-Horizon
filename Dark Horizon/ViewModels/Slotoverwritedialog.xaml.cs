using System.Windows;
using Dark_Horizon.Classes.Core.Database.Models;

namespace Dark_Horizon
{
    public partial class SlotOverwriteDialog : Window
    {
        public bool ChoiceIsNew { get; private set; } = false;

        public SlotOverwriteDialog(SaveData save)
        {
            InitializeComponent();
            TxtInfo.Text = $"{save.PlayerName}  ·  {save.PlayerRace}  ·  {save.PlayerClass}  ·  Рівень {save.Level}";
            TxtDate.Text = $"Збережено: {save.SavedAt:dd.MM.yyyy  HH:mm}";
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            ChoiceIsNew = false;
            DialogResult = true;
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            ChoiceIsNew = true;
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}