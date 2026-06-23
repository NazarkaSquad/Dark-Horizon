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
        private string _selectedRace = "Human";
        private string _selectedGender = "Male";
        private string _selectedClass = "Warrior";

        private Button[] _raceBtns = null!;
        private Button[] _genderBtns = null!;
        private Button[] _classBtns = null!;

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


        private void Race_Click(object sender, RoutedEventArgs e)
        {
            _selectedRace = (string)((Button)sender).Tag;
            HighlightActive(_raceBtns, (Button)sender);
            TxtRaceDesc.Text = RaceDesc[_selectedRace];
            UpdateCharacterSprite();
        }


        private void Gender_Click(object sender, RoutedEventArgs e)
        {
            _selectedGender = (string)((Button)sender).Tag;
            HighlightActive(_genderBtns, (Button)sender);
            UpdateCharacterSprite();
        }


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


        private void UpdateCharacterSprite()
        {
            string genderPrefix = _selectedGender == "Male" ? "M" : "W";

            string fileName = $"{genderPrefix}{_selectedRace}.png";

            string uriString = $"pack://application:,,,/Assets/Images/Characters/{fileName}";

            PlayerSprite.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(uriString));
        }


        private void HighlightActive(Button[] group, Button active)
        {
            var activeStyle = (Style)FindResource("SelectBtnActive");
            var normalStyle = (Style)FindResource("SelectBtn");

            foreach (var btn in group)
                btn.Style = btn == active ? activeStyle : normalStyle;
        }


        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtPlayerName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name)) name = "Безіменний";

            var raceType = _selectedRace switch
            {
                "Orc" => RaceType.Orc,
                "Elf" => RaceType.Elf,
                _ => RaceType.Human,
            };

            var playerClass = _selectedClass switch
            {
                "Rogue" => PlayerClass.Rogue,
                "Mage" => PlayerClass.Mage,
                "Paladin" => PlayerClass.Warrior,
                "Archer" => PlayerClass.Rogue,
                "Necromancer" => PlayerClass.Mage,
                _ => PlayerClass.Warrior,
            };

            GameManager.Instance.StartGame(name, playerClass, raceType);

            GameState.SaveCurrentGame("Місто");

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