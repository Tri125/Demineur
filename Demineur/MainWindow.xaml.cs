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

        //  Lorsque le bouton de nouvelle partie est cliqué.
        private void btnNouvellePartie_Click(object sender, RoutedEventArgs e)
        {
            //  Ouverture de la fenêtre de nouvelle partie en mode ShowDialog pour qu'elle garde le focus et empêcher de jouer dans la fenêtre principale.
            FenetreNouvellePartie fenPartie = new FenetreNouvellePartie();
            //  ShowDialog retourne uniquement lorsque la fenêtre est fermé
            //  Value est le DialogResult de la fenêtre. Utilisé pour voir si le fenêtre à été annulé ou non.
            if (fenPartie.ShowDialog().Value)
            {
                //  Récupère les propriétés de la fenêtre pour créer une nouvelle partie.
                //  Selon l'exécution les paramètres sont déjà dans le fichier de configuration, mais il est préférable prendre les données de la fenêtre
                //  si le fichier ne peut être écrit, la partie sera lancé avec les paramètres d'usines.
                int largeur = fenPartie.Largeur;
                int hauteur = fenPartie.Hauteur;
                int nbrMines = fenPartie.NbrMines;
                NouvellePartie(largeur, hauteur, nbrMines);
            }
        }

        // Bouton de configuration
        private void btnConfiguration_Click(object sender, RoutedEventArgs e)
        {
            FenetreConfiguration fenConfig = new FenetreConfiguration();
            // Pour que la fenêtre garde le focus et empêcher de jouer dans la fenêtre principale.
            fenConfig.ShowDialog();
        }

        /// <summary>
        /// Crée une nouvelle fenêtre FenetreChampMines et la rajoute dans la grille de jeu.
        /// </summary>
        /// <param name="largeur">Largeur du jeu</param>
        /// <param name="hauteur">Hauteur du jeu</param>
        /// <param name="nbrMines">Nombre de mines du jeu</param>
        private void NouvellePartie(int largeur, int hauteur, int nbrMines)
        {
            // S'il y a déjà une partie en cours
            if (fenetreJeu != null)
            {
                // On désinscrit des évênements et on remet par défaut le label du statut de la partie (faire disparaître).
                fenetreJeu.Terminer -= new FenetreChampMines.PartieTermineEventHandler(ChangeLabelJeu);
                fenetreJeu.Drapeau -= new FenetreChampMines.DrapeauEventHandler(OnChangementDrapeau);
                DefautLabelJeu();
            }
            // Pas besoin de vérifier s'il était déjà présent ou non.
            gridPrincipale.Children.Remove(fenetreJeu);
            fenetreJeu = new FenetreChampMines(largeur, hauteur, nbrMines);

            // Inscription aux évênements de FenetreChampMines pour savoir lorsque la partie est terminé et lorsqu'un drapeau est enlevé/placé.
            fenetreJeu.Terminer += new FenetreChampMines.PartieTermineEventHandler(ChangeLabelJeu);
            fenetreJeu.Drapeau += new FenetreChampMines.DrapeauEventHandler(OnChangementDrapeau);

            gridPrincipale.Children.Add(fenetreJeu);

            // Initialise le compteur de mines.
            indicateurMine.SetMineCount(nbrMines);
            // On rend visible le compteur.
            indicateurMine.Visibility = System.Windows.Visibility.Visible;
        }

        // Lorsque le bouton de partie rapide est appuyé.
        private void btnPartieRapide_Click(object sender, RoutedEventArgs e)
        {
            // Crée une nouvelle partie selon les configurations personnalisées de l'utilisateur.
            NouvellePartie(App.config.OptionUtilisateur.Largeur, App.config.OptionUtilisateur.Hauteur, App.config.OptionUtilisateur.NombresMines);

            //  TODO Créer une nouvelle partie selon les derniers paramètres utilisés.
        }

        //  Change le label du statut de la partie selon si la partie est gagnée ou perdue.
        private void ChangeLabelJeu(object sender)
        {
            // C'est juste mauvais parce que le sender de l'event est un bool à savoir si le joueur est mort ou non...
            // TODO Faire un EventHandler qui respecte le principe de sender et d'EventArgs.
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

        // Lors d'une nouvelle partie, remet à neuf le label de statut de la partie.
        private void DefautLabelJeu()
        {
            lblPartie.Foreground = Brushes.Black;
            lblPartie.Content = "";
        }

        //  Appel la méthode à l'indicateur de mines pour incrémenter son compteur
        private void RetraitDrapeau()
        {
            indicateurMine.IncrementeMine();
        }

        //  Appel la méthode à l'indicateur de mines pour décrémenter son compteur
        private void RajoutDrapeau()
        {
            indicateurMine.DecrementeMine();
        }

        // Selon DrapeauEventArgs qui indique si un drapeau a été rajouté ou retiré,
        // appel les méthodes appropriées.
        private void OnChangementDrapeau(object sender, DrapeauEventArgs e)
        {
            if (e == DrapeauEventArgs.Rajout)
            {
                RajoutDrapeau();
            }
            else if (e == DrapeauEventArgs.Retrait)
            {
                RetraitDrapeau();
            }
        }

    }
}
