using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.BusinessLogic;
using WpfApp.Model;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KoltsegKezelo _koltsegKezelo;

        public MainWindow()
        {
            InitializeComponent();

            _koltsegKezelo = new KoltsegKezelo("chef_koltsegek_2025.csv");

            KoltsegGrid.ItemsSource = _koltsegKezelo.Koltsegek;
        }

        private void Hozzaadas_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(OsszegBox.Text, out var osszeg) || osszeg <= 0)
            {
                MessageBox.Show("Érvénytelen összeg!");
                return;
            }
            var kategoria = ((ComboBoxItem)KategoriaCombo.SelectedItem!).Content.ToString()!;
            var datum = DatumPicker.SelectedDate ?? DateTime.Today;
            var ujId = _koltsegKezelo.Koltsegek.Any() ? _koltsegKezelo.Koltsegek.Max(k => k.Id) + 1 : 1;
            _koltsegKezelo.Koltsegek.Add(new Koltseg
            {
                Id = ujId,
                ChefName = ChefNameBox.Text.Trim(),
                Datum = DateOnly.FromDateTime(datum),
                Kategoria = kategoria,
                Osszeg = osszeg,
                Megjegyzes = MegjegyzesBox.Text.Trim()
            });

            _koltsegKezelo.WriteToFile();

            KoltsegGrid.Items.Refresh();
            ChefNameBox.Clear();
            OsszegBox.Clear();
            MegjegyzesBox.Clear();
            DatumPicker.SelectedDate = null;
        }
    }
}