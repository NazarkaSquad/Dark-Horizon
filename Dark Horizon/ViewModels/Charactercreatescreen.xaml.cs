using Dark_Horizon.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dark_Horizon.Classes.Core;
using Dark_Horizon.Classes.Entities;

namespace Dark_Horizon.Views
{
    public partial class Charactercreatescreen : UserControl
    {
        // Поточний вибір
        private string _selectedRace = "Human";
        private string _selectedGender = "Male";
        private string _selectedClass = "Warrior";

        // Всі кнопки груп для перемикання стилів
        private Button[] _raceBtns = null!;
        private Button[] _genderBtns = null!;
        private Button[] _classBtns = null!;

        // Описи та статистика класів
        private static readonly Dictionary<string, (string Desc, int HP, int Atk, int Def)> ClassData = new()
        {
            ["Warrior"] = ("Фронтовий боєць у важкій броні. Висока витривалість і захист. Ідеальний для новачків.", 150, 17, 6),
            ["Rogue"] = ("Тінь у темряві. Наносить критичні удари ззаду та ухиляється від ворожих атак.", 100, 22, 2),
            ["Mage"] = ("Майстер стихій. Слабке тіло, але магічні заклинання нищать ворогів здалеку.", 80, 28, 1),
            ["Paladin"] = ("Священний воїн. Може лікувати себе в бою. Золотий баланс атаки і захисту.", 130, 15, 8),
            ["Archer"] = ("Снайпер лісів. Атакує з відстані та має шанс двічі вдарити за хід.", 100, 20, 3),
            ["Necromancer"] = ("Некромант піднімає ворогів після смерті як союзників. Небезпечний і непередбачуваний.", 90, 24, 2),
        };

        private static readonly Dictionary<string, string> RaceDesc = new()
        {
            ["Human"] = "Збалансована раса без яскравих слабкостей. Отримує на 10% більше золота з мобів.",
            ["Orc"] = "Найміцніша раса. +30 HP. 10% шанс повністю ігнорувати удар. Слабкіша магія.",
            ["Elf"] = "Крихкий але магічно сильний. +15 до магії, +10 спритності. Бачить слабкі місця ворогів.",
        };

        // Кольори тіла персонажа залежно від раси
        private static readonly Dictionary<string, Color> RaceSkinColor = new()
        {
            ["Human"] = Color.FromRgb(200, 149, 108),
            ["Orc"] = Color.FromRgb(80, 130, 60),
            ["Elf"] = Color.FromRgb(220, 190, 160),
        };

        // Кольори тіла залежно від класу
        private static readonly Dictionary<string, Color> ClassBodyColor = new()
        {
            ["Warrior"] = Color.FromRgb(74, 56, 40),
            ["Rogue"] = Color.FromRgb(30, 30, 40),
            ["Mage"] = Color.FromRgb(60, 20, 80),
            ["Paladin"] = Color.FromRgb(120, 100, 30),
            ["Archer"] = Color.FromRgb(50, 80, 30),
            ["Necromancer"] = Color.FromRgb(20, 20, 20),
        };

        public Charactercreatescreen()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            UpdateCharacterSprite();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _raceBtns = new[] { BtnRaceHuman, BtnRaceOrc, BtnRaceElf };
            _genderBtns = new[] { BtnGenderMale, BtnGenderFemale };
            _classBtns = new[] { BtnClassWarrior, BtnClassRogue, BtnClassMage, BtnClassPaladin, BtnClassArcher, BtnClassNecro };

            RefreshClassInfo();
        }

        // ─── РАСА ───────────────────────────────────────────────────────────

        private void Race_Click(object sender, RoutedEventArgs e)
        {
            _selectedRace = (string)((Button)sender).Tag;
            HighlightActive(_raceBtns, (Button)sender);
            TxtRaceDesc.Text = RaceDesc[_selectedRace];
            UpdateCharacterSprite();
        }

        // ─── СТАТЬ ──────────────────────────────────────────────────────────

        private void Gender_Click(object sender, RoutedEventArgs e)
        {
            _selectedGender = (string)((Button)sender).Tag;
            HighlightActive(_genderBtns, (Button)sender);
            UpdateCharacterSprite();
        }

        // ─── КЛАС ───────────────────────────────────────────────────────────

        private void Class_Click(object sender, RoutedEventArgs e)
        {
            _selectedClass = (string)((Button)sender).Tag;
            HighlightActive(_classBtns, (Button)sender);
            RefreshClassInfo();
            UpdateCharacterSprite();
        }

        private void RefreshClassInfo()
        {
            var (desc, hp, atk, def) = ClassData[_selectedClass];
            TxtClassDesc.Text = desc;
            TxtHP.Text = hp.ToString();
            TxtAtk.Text = atk.ToString();
            TxtDef.Text = def.ToString();
        }

        // ─── ОНОВЛЕННЯ СПРАЙТУ ──────────────────────────────────────────────

        private void UpdateCharacterSprite()
        {
            // 1. Визначаємо першу літеру: M для чоловіка, W для жінки
            string genderPrefix = _selectedGender == "Male" ? "M" : "W";

            // 2. Формуємо назву файлу (наприклад, "MHuman.png" або "WElf.png")
            string fileName = $"{genderPrefix}{_selectedRace}.png";

            // 3. Вказуємо шлях до папки, куди ви поклали файли
            string uriString = $"pack://application:,,,/Assets/Images/Characters/{fileName}";

            // 4. Завантажуємо картинку в контрол (дивіться нижче, що додати в XAML)
            PlayerSprite.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(uriString));
        }

        // ─── ДОПОМІЖНИЙ МЕТОД: підсвітка активної кнопки ───────────────────

        private void HighlightActive(Button[] group, Button active)
        {
            var activeStyle = (Style)FindResource("SelectBtnActive");
            var normalStyle = (Style)FindResource("SelectBtn");

            foreach (var btn in group)
                btn.Style = btn == active ? activeStyle : normalStyle;
        }

        // ─── ПОЧАТИ ГРУ ─────────────────────────────────────────────────────

        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtPlayerName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name)) name = "Безіменний";

            // Конвертація рядка в enum
            var raceType = _selectedRace switch
            {
                "Orc" => RaceType.Orc,
                "Elf" => RaceType.Elf,
                _ => RaceType.Human,
            };

            // Тільки 3 класи зараз підтримуються у Player.cs
            // Нові класи треба додати у PlayerClass enum та Player.cs
            var playerClass = _selectedClass switch
            {
                "Rogue" => PlayerClass.Rogue,
                "Mage" => PlayerClass.Mage,
                "Paladin" => PlayerClass.Warrior,   // тимчасово, поки не додали
                "Archer" => PlayerClass.Rogue,     // тимчасово
                "Necromancer" => PlayerClass.Mage,      // тимчасово
                _ => PlayerClass.Warrior,
            };

            // Стартуємо гру
            GameManager.Instance.StartGame(name, playerClass, raceType);

            // Зберігаємо в обраний слот відразу
            GameState.SaveCurrentGame("Місто");

            // Переходимо в місто
            if (Application.Current.MainWindow is MainWindow mainWindow)
                mainWindow.StartTransition(new Town());
        }

        public event EventHandler? OnBackClosed;
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OnBackClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}