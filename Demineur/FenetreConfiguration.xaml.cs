using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Demineur
{
    /// <summary>
    /// Interaction logic for FenetreConfiguration.xaml
    /// </summary>
    public partial class FenetreConfiguration : Window
    {
        public FenetreConfiguration()
        {
            InitializeComponent();
            sTaille.Value = App.config.OptionUtilisateur.TailleCases;
            Console.WriteLine("Me " + App.config.OptionUtilisateur.TailleCases);
            chkMinesCoins.IsChecked = App.config.OptionUtilisateur.MinesCoins;
        }

        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            ConfigJoueur config = new ConfigJoueur(App.config.OptionUtilisateur.MinesCoins, App.config.OptionUtilisateur.TailleCases);
            App.config.EnregistrementUtilisateur(ref config);
        }

        private void sTaille_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.OldValue == 0)
                return;
            App.config.OptionUtilisateur.TailleCases = (int)e.NewValue;
            Console.WriteLine(App.config.OptionUtilisateur.TailleCases);
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsChecked == true)
                {
                    App.config.OptionUtilisateur.MinesCoins = true;
                }
                else
                {
                    App.config.OptionUtilisateur.MinesCoins = false;
                }
            }
        }

        private void btnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
