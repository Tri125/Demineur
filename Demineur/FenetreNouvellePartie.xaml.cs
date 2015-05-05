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
    /// Interaction logic for FenetreNouvellePartie.xaml
    /// </summary>
    public partial class FenetreNouvellePartie : Window
    {
        public int Largeur { get; set; }
        public int Hauteur { get; set; }
        public int NbrMines { get; set; }

        public FenetreNouvellePartie()
        {
       
            InitializeComponent();
            DataObject.AddPastingHandler(txtHauteur, new DataObjectPastingEventHandler(OnPaste));
            DataObject.AddPastingHandler(txtLargeur, new DataObjectPastingEventHandler(OnPaste));
            DataObject.AddPastingHandler(txtMines, new DataObjectPastingEventHandler(OnPaste));

            txtHauteur.TextChanged += new TextChangedEventHandler(txtNumeric_TextChanged);
            txtLargeur.TextChanged += new TextChangedEventHandler(txtNumeric_TextChanged);
            txtMines.TextChanged += new TextChangedEventHandler(txtNumeric_TextChanged);

            //Présent dans le code et non dans la fenêtre sinon s'inscris à l'évènement avant l'instantiation des contrôles et les fonctions modifies un contrôle.
            rdDebutant.Checked += new RoutedEventHandler(rdDebutant_Checked);
            rdAvance.Checked += new RoutedEventHandler(rdAvance_Checked);
            rdPerso.Checked += new RoutedEventHandler(rdPerso_Checked);

            txtHauteur.Text = App.config.OptionUtilisateur.Hauteur.ToString();
            txtLargeur.Text = App.config.OptionUtilisateur.Largeur.ToString();
            txtMines.Text = App.config.OptionUtilisateur.NombresMines.ToString();
        }

        // http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
        private void txtNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidNumericChars(e.Text);
        }

        // http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
        private bool AreAllValidNumericChars(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsNumber(c)) return false;
            }

            return true;
        }

        private void txtNumeric_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        // http://stackoverflow.com/questions/3061475/paste-event-in-a-wpf-textbox
        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            bool isText = e.SourceDataObject.GetDataPresent(System.Windows.DataFormats.Text, true);
            if (!isText) return;

            string text = e.SourceDataObject.GetData(DataFormats.Text) as string;
            if (!AreAllValidNumericChars(text))
            {
                e.CancelCommand();
            }
        }

        private void btnJouer_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (rdDebutant.IsChecked == true)
            {
                Largeur = 5;
                Hauteur = 5;
                NbrMines = 5;
            }
            else
                if (rdAvance.IsChecked == true)
                {
                    Largeur = 15;
                    Hauteur = 15;
                    NbrMines = 60;
                }
                else
                    if (rdPerso.IsChecked == true)
                    {
                        Largeur = int.Parse(txtLargeur.Text);
                        Hauteur = int.Parse(txtHauteur.Text);
                        NbrMines = int.Parse(txtMines.Text);
                        ConfigJoueur config = new ConfigJoueur(App.config.OptionUtilisateur.MinesCoins, App.config.OptionUtilisateur.TailleCases,
                        NbrMines, Hauteur, Largeur);
                        App.config.EnregistrementUtilisateur(ref config);
                    }
            this.Close();
        }

        private bool IsValidParametrePerso()
        {
            int largeur;
            if (!int.TryParse(txtLargeur.Text, out largeur))
            {
                largeur = 0;
            }

            int hauteur;
            if (!int.TryParse(txtHauteur.Text, out hauteur))
            {
                hauteur = 0;
            }

            int nbrMines;
            if (!int.TryParse(txtMines.Text, out nbrMines))
            {
                nbrMines = 0;
            }

            if (largeur <= 0 || hauteur <= 0 || nbrMines < 0 || (largeur * hauteur) < 4 || largeur > 50 || hauteur > 30 || (nbrMines > (largeur * hauteur)))
            {
                return false;
            }
            return true;
        }

        private void txtNumeric_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsValidParametrePerso())
            {
                btnJouer.IsEnabled = false;
            }
            else
            {
                btnJouer.IsEnabled = true;
            }
        }

        private void rdDebutant_Checked(object sender, RoutedEventArgs e)
        {
            btnJouer.IsEnabled = true;
        }

        private void rdAvance_Checked(object sender, RoutedEventArgs e)
        {
            btnJouer.IsEnabled = true;
        }

        private void rdPerso_Checked(object sender, RoutedEventArgs e)
        {
            btnJouer.IsEnabled = IsValidParametrePerso();
        }


    }
}
