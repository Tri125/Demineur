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
        private int largeur;
        private int hauteur;
        private int nbrMines;

        public FenetreNouvellePartie()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(txtHauteur, new DataObjectPastingEventHandler(OnPaste));
            DataObject.AddPastingHandler(txtLargeur, new DataObjectPastingEventHandler(OnPaste));
            DataObject.AddPastingHandler(txtMines, new DataObjectPastingEventHandler(OnPaste));
            txtHauteur.Text = App.config.OptionUtilisateur.Hauteur.ToString();
            txtLargeur.Text = App.config.OptionUtilisateur.Largeur.ToString();
            txtMines.Text = App.config.OptionUtilisateur.NombresMines.ToString();
        }

        public int Largeur
        {
            get
            {
                return largeur;
            }
            set
            {
                if (value <= 0 || value > 50)
                {
                    value = 15;
                }
                largeur = value;
            }
        }
        public int Hauteur
        {
            get
            {
                return hauteur;
            }
            set
            {
                if (value <= 0 || value > 30)
                {
                    value = 15;
                }
                hauteur = value;
            }
        }

        public int NbrMines
        {
            get
            {
                return nbrMines;
            }
            set
            {
                if (value <= 0)
                    value = 60;
                nbrMines = value;
            }
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
            bool valid = true;
            if (rdDebutant.IsChecked == true)
            {
                Largeur = 5;
                Hauteur = 5;
                NbrMines = 5;
                DialogResult = true;
            }
            else
                if (rdAvance.IsChecked == true)
                {
                    Largeur = 15;
                    Hauteur = 15;
                    NbrMines = 60;
                    DialogResult = true;
                }
                else
                    if (rdPerso.IsChecked == true)
                    {
                        valid = IsValidParametrePerso();
                    }
            if (valid)
                this.Close();
        }

        private bool IsValidParametrePerso()
        {
            if (txtHauteur != null && txtLargeur != null && txtMines != null)
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

                if (largeur <= 0 || hauteur <= 0 || nbrMines < 0 || (largeur * hauteur) < 2 || largeur > 50 || hauteur > 30 || (nbrMines > (largeur * hauteur)))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private void txtNumeric_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (btnJouer != null)
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
        }

    }
}
