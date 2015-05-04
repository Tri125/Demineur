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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demineur
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FenetreChampMines fenetreJeu;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnNouvellePartie_Click(object sender, RoutedEventArgs e)
        {
            FenetreNouvellePartie fenPartie = new FenetreNouvellePartie();
            //DialogResult
            if (fenPartie.ShowDialog().Value)
            {
                int largeur = fenPartie.Largeur;
                int hauteur = fenPartie.Hauteur;
                int nbrMines = fenPartie.NbrMines;
                NouvellePartie(largeur, hauteur, nbrMines);
            }
        }

        private void btnConfiguration_Click(object sender, RoutedEventArgs e)
        {
            FenetreConfiguration fenConfig = new FenetreConfiguration();
            fenConfig.ShowDialog();
        }

        private void NouvellePartie(int largeur, int hauteur, int nbrMines)
        {
            if (fenetreJeu != null)
            {
                fenetreJeu.Terminer -= new FenetreChampMines.PartieTermineEventHandler(ChangeLabelJeu);
                DefautLabelJeu();
            }
            gridPrincipale.Children.Remove(fenetreJeu);
            fenetreJeu = new FenetreChampMines(largeur, hauteur, nbrMines);
            fenetreJeu.Terminer += new FenetreChampMines.PartieTermineEventHandler(ChangeLabelJeu);
            gridPrincipale.Children.Add(fenetreJeu);
            indicateurMine.SetMineCount(nbrMines);
            indicateurMine.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnPartieRapide_Click(object sender, RoutedEventArgs e)
        {
            NouvellePartie(App.config.OptionUtilisateur.Largeur, App.config.OptionUtilisateur.Hauteur, App.config.OptionUtilisateur.NombresMines);
        }

        private void ChangeLabelJeu(object sender)
        {
            if (sender is bool)
            {
                bool joueurMort = (bool)sender;
                if (joueurMort == true)
                {
                    lblPartie.Foreground = Brushes.Red;
                    lblPartie.Content = "Partie Perdue";
                }
                else
                    if (joueurMort == false)
                    {
                        lblPartie.Foreground = Brushes.Green;
                        lblPartie.Content = "Partie Gagnée";
                    }
            }
        }

        private void DefautLabelJeu()
        {
            lblPartie.Foreground = Brushes.Black;
            lblPartie.Content = "";
        }

    }
}
