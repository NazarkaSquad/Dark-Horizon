using System.Windows;
using System.Windows.Controls;
using Dark_Horizon.Classes.Core;

namespace Dark_Horizon.Views
{
    public partial class Swamp : UserControl
    {
        public Swamp()
        {
            InitializeComponent();
            GameManager.Instance.LocationManager.SetCurrentLocation(LocationType.Swamp);
        }

        private void BtnToForest_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
                mw.StartTransition(new Forest());
        }
    }
}
