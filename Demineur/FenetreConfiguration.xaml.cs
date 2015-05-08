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
    /// Fenêtre de configuration
    /// </summary>
    public partial class FenetreConfiguration : Window
    {

        public int TailleCases { get; set; }
        public bool MinesCoins { get; set; }

        public FenetreConfiguration()
        {
            InitializeComponent();
            //  Initialise la valeur du slider de taille de cases selon les configurations de l'utilisateur.
            TailleCases = App.config.OptionUtilisateur.TailleCases;
            sTaille.Value = TailleCases;
            //  Évênement enregistré dans le code pour éviter que la méthode soit exécuté lors de l'initialisation.
            sTaille.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sTaille_ValueChanged);

            // Initialise la valeur de la checkbox des mines de coins selon les configurations de l'utilisateur.
            MinesCoins = App.config.OptionUtilisateur.MinesCoins;
            chkMinesCoins.IsChecked = MinesCoins;
        }

        /// <summary>
        /// Enregistre un nouveau fichier de configuration selon les valeurs des contrôles de la fenêtre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            //  Met à jour le fichier de configuration.
            App.config.OptionUtilisateur.MinesCoins = MinesCoins;
            App.config.OptionUtilisateur.TailleCases = TailleCases;
            App.config.EnregistreConfigCourante();
            this.Close();
        }

        /// <summary>
        /// Évênement lorsque la valeur du Slider de la taille des cases change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sTaille_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TailleCases = (int)e.NewValue;
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsChecked == true)
                {
                    MinesCoins = true;
                }
                else
                {
                    MinesCoins = false;
                }
            }
        }
    }
}
