using System;
using System.Windows.Threading;
using Dark_Horizon.Classes.Core.Database;

namespace Dark_Horizon.Classes.Core
{
    public static class GameState
    {
        public static int SelectedSlot { get; set; } = 1;

        private static int _autoSaveMinutes = 5;
        public static int AutoSaveMinutes
        {
            get => _autoSaveMinutes;
            set { _autoSaveMinutes = Math.Clamp(value, 1, 10); RestartAutoSave(); }
        }

        private static DispatcherTimer? _autoSaveTimer;
        private static DispatcherTimer? _regenTimer;

        // ── Автозбереження ────────────────────────────────────────────────────
        public static void StartAutoSave()
        {
            RestartAutoSave();
            StartRegen();
        }

        public static void StopAutoSave()
        {
            _autoSaveTimer?.Stop();
            StopRegen();
        }

        private static void RestartAutoSave()
        {
            _autoSaveTimer?.Stop();
            _autoSaveTimer = new DispatcherTimer
                { Interval = TimeSpan.FromMinutes(_autoSaveMinutes) };
            _autoSaveTimer.Tick += (_, _) =>
            {
                if (GameManager.Instance.Player != null)
                    SaveCurrentGame(GameManager.Instance.CurrentLocation.Name);
            };
            _autoSaveTimer.Start();
        }

        public static void SaveCurrentGame(string locationName)
        {
            if (GameManager.Instance.Player == null) return;
            new SaveManager().Save(SelectedSlot, GameManager.Instance.Player, locationName);
        }

        // ── Пасивна регенерація: +1 HP / хвилину поза боєм ──────────────────
        public static void StartRegen()
        {
            _regenTimer?.Stop();
            _regenTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1) };
            _regenTimer.Tick += (_, _) =>
            {
                var gm = GameManager.Instance;
                if (gm.Player == null || gm.IsInBattle) return;
                gm.Player.Heal(1);
            };
            _regenTimer.Start();
        }

        public static void StopRegen() => _regenTimer?.Stop();
    }
}
