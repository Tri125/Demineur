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
        // Pour les niveaux de difficultés.
        public const int DEBUTANT_LARGEUR = 5;
        public const int DEBUTANT_HAUTEUR = 5;
        public const int DEBUTANT_NBR_MINES = 5;

        public const int AVANCE_LARGEUR = 15;
        public const int AVANCE_HAUTEUR = 15;
        public const int AVANCE_NBR_MINES = 60;

        public const int MINIMUM_CASE = 4;
        public const int MAXIMUM_HAUTEUR = 30;
        public const int MAXIMUM_LARGEUR = 50;

        // Propriétées des valeurs pour la partie personnalisée.
        public int Largeur { get; set; }
        public int Hauteur { get; set; }
        public int NbrMines { get; set; }

        public FenetreNouvellePartie()
        {
       
            InitializeComponent();
            //  Rajoute un Handler aux textBox pour gérer lorsque l'utilisateur colle du texte.
            DataObject.AddPastingHandler(txtHauteur, new DataObjectPastingEventHandler(OnPaste));
            DataObject.AddPastingHandler(txtLargeur, new DataObjectPastingEventHandler(OnPaste));
            DataObject.AddPastingHandler(txtMines, new DataObjectPastingEventHandler(OnPaste));

            //  Présent dans le code et non dans la fenêtre sinon s'inscris à l'évènement avant l'instantiation des contrôles et les fonctions modifies un contrôle
            //  donc il faudrait plus de vérifications inutiles.
            //  Utilisé à des fins de vérification.
            rdDebutant.Checked += new RoutedEventHandler(rdNiveau_Checked);
            rdAvance.Checked += new RoutedEventHandler(rdNiveau_Checked);
            rdPerso.Checked += new RoutedEventHandler(rdPerso_Checked);

            //  Initialisation des textBox avec les paramètres de l'utilisateur.
            txtHauteur.Text = App.config.OptionUtilisateur.Hauteur.ToString();
            txtLargeur.Text = App.config.OptionUtilisateur.Largeur.ToString();
            txtMines.Text = App.config.OptionUtilisateur.NombresMines.ToString();

            //  Inscription à l'évênement TextChanged à chaque textBox. Sert à vérifier si les paramètres sont valides.
            //  Fait dans le code et non le xaml puisqu'à l'initialisation l'évênement est lancé et pas tout les contrôles sont créés.
            txtHauteur.TextChanged += new TextChangedEventHandler(txtNumeric_TextChanged);
            txtLargeur.TextChanged += new TextChangedEventHandler(txtNumeric_TextChanged);
            txtMines.TextChanged += new TextChangedEventHandler(txtNumeric_TextChanged);
        }

        // http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
        //  Lancé lorsque l'utilisateur essaie d'écrire dans les txtBox
        private void txtNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //  Si le texte n'est pas des caractères numériques, alors l'évênement est Handled.
            //  Arrête la chaîne de l'évênement et le texte ne sera jamais mit dans le contrôle.
            e.Handled = !AreAllValidNumericChars(e.Text);
        }

        // http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
        //  Vérifie que chaque caractère est un numéro.
        private bool AreAllValidNumericChars(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsNumber(c)) return false;
            }

            return true;
        }

        // Évênement spécifique pour la gestion des espaces, car PreviewTextInput ne gère pas les espaces (évênement même pas lancé).
        // Prend la clée du clavier avant qu'elle soit entrée dans le contrôle.
        private void txtNumeric_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Si l'utilisateur a appuyé sur la touche d'espace, nous ne voulons pas l'écrire dans la txtBox.
            if (e.Key == Key.Space)
            {
                //  Évênement Handled, donc arrête la chaîne.
                e.Handled = true;
            }
        }

        // http://stackoverflow.com/questions/3061475/paste-event-in-a-wpf-textbox
        // Évênement spécifique pour vérifier les caractères lorsque l'utilisateur colle du texte dans les txtBox.
        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            // Si ce n'est pas du texte, le contrôle ne le prend pas de toute manière.
            bool isText = e.SourceDataObject.GetDataPresent(System.Windows.DataFormats.Text, true);
            if (!isText) return;

            string text = e.SourceDataObject.GetData(DataFormats.Text) as string;
            if (!AreAllValidNumericChars(text))
            {
                // Paste est une commande. On peut annuler la commande sans briser la chaîne de l'évênement avec Handled.
                e.CancelCommand();
            }
        }

        private void btnJouer_Click(object sender, RoutedEventArgs e)
        {
            //  Le bouton pour créer une partie a été cliqué, donc DialogResult est à true (la fenêtre mère en fera la vérification) .
            DialogResult = true;
            //  Selon le radioButton, modifie les propriétés avec les valeurs adéquates.
            if (rdDebutant.IsChecked == true)
            {
                Largeur = DEBUTANT_LARGEUR;
                Hauteur = DEBUTANT_HAUTEUR;
                NbrMines = DEBUTANT_NBR_MINES;
            }
            else
                if (rdAvance.IsChecked == true)
                {
                    Largeur = AVANCE_LARGEUR;
                    Hauteur = AVANCE_HAUTEUR;
                    NbrMines = AVANCE_NBR_MINES;
                }
                else
                    //  Si une partie personnalisé est désirée.
                    if (rdPerso.IsChecked == true)
                    {
                        // Prend les valeurs des txtBox.
                        Largeur = int.Parse(txtLargeur.Text);
                        Hauteur = int.Parse(txtHauteur.Text);
                        NbrMines = int.Parse(txtMines.Text);

                        // Modifie la configuration courante et l'enregistre.
                        App.config.OptionUtilisateur.Largeur = Largeur;
                        App.config.OptionUtilisateur.Hauteur = Hauteur;
                        App.config.OptionUtilisateur.NombresMines = NbrMines;
                        App.config.EnregistreConfigCourante();
                    }
            this.Close();
        }

        // Fait la vérification des paramètres personnalisés et retourne le résultat.
        private bool IsValidParametrePerso()
        {
            //  Conversion en int et vérification.
            //  Honêtement, une conversion forcé réduirait le code et avec les vérifications des txtBox nous sommes assuré d'avoir une conversion valide.
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

            //  Les chiffres magiques
            if (largeur <= 0 || hauteur <= 0 || nbrMines < 0 || (largeur * hauteur) < MINIMUM_CASE || largeur > MAXIMUM_LARGEUR || hauteur > MAXIMUM_HAUTEUR || (nbrMines > (largeur * hauteur)))
            {
                return false;
            }
            return true;
        }

        // Lorsque le texte des txtBox changent, vérifie les paramètres à savoir s'ils sont valident ou non.
        // Désactive/Active le bouton de création de nouvelle partie selon le résultat.
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

        // Peut importe les paramètres personnalisés, réactive le bouton de nouvelle partie si niveau débutant ou avancé est choisi.
        private void rdNiveau_Checked(object sender, RoutedEventArgs e)
        {
            btnJouer.IsEnabled = true;
        }

        // Lorsque le niveau personnalisé est choisi, vérifie la validité de la configuration et active/désactive le bouton de nouvelle partie selon le cas.
        private void rdPerso_Checked(object sender, RoutedEventArgs e)
        {
            btnJouer.IsEnabled = IsValidParametrePerso();
        }


    }
}
