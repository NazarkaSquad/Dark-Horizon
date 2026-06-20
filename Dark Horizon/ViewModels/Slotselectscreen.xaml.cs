using Dark_Horizon.Views;
using Dark_Horizon.Classes.Core.Database; // Щоб бачити SaveManager
using Dark_Horizon.Classes.Core;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Dark_Horizon.Views
{
    public partial class Slotselectscreen : UserControl
    {
        private readonly SaveManager _saveManager = new();
        public event EventHandler? OnBackClosed;
        public event EventHandler? OnCharacterCreateRequested;

        public Slotselectscreen()
        {
            InitializeComponent();
            LoadSlots();

            Slot1.SlotSelected += OnSlotSelected;
            Slot2.SlotSelected += OnSlotSelected;
            Slot3.SlotSelected += OnSlotSelected;
        }

        private void LoadSlots()
        {
            var saves = _saveManager.GetAllSaves();

            Slot1.Setup(1, saves.FirstOrDefault(s => s.Id == 1));
            Slot2.Setup(2, saves.FirstOrDefault(s => s.Id == 2));
            Slot3.Setup(3, saves.FirstOrDefault(s => s.Id == 3));
        }

        private void OnSlotSelected(object? sender, int slotNumber)
        {
            if (Application.Current.MainWindow is not MainWindow mainWindow) return;

            var existingSave = _saveManager.Load(slotNumber);

            // Зберігаємо вибраний слот у GameState
            GameState.SelectedSlot = slotNumber;

            if (existingSave == null)
            {
                // Слот порожній → йдемо на створення персонажа
                OnCharacterCreateRequested?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // Слот зайнятий → запитуємо: Нова гра чи Завантажити?
                var dialog = new SlotOverwriteDialog(existingSave);
                dialog.Owner = mainWindow;
                var result = dialog.ShowDialog();

                if (result == true && dialog.ChoiceIsNew)
                {
                    // Гравець хоче нову гру поверх старого сейву
                    _saveManager.Delete(slotNumber);
                    OnCharacterCreateRequested?.Invoke(this, EventArgs.Empty);
                }
                else if (result == true && !dialog.ChoiceIsNew)
                {
                    // Завантажити стару гру
                    Classes.Core.GameManager.Instance.LoadFromSave(existingSave);
                    mainWindow.StartTransition(new Town());
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
                OnBackClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}