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
    /// Logique d'interaction pour FenetreChampMine.xaml
    /// </summary>
    public partial class FenetreChampMines : UserControl
    {
        private ChampMines Jeu { get; set; }
        private bool JoueurMort { get; set; }

        public FenetreChampMines(int largeur, int hauteur, int nbMines)
        {
            InitializeComponent();

            // Générer la structure du champ de mines.
            Jeu = new ChampMines(largeur, hauteur, nbMines);

            // Modifie la Grid pour correspondre au champ de mine du jeu.
            genererGrilleJeu();

            // Affiche le premier niveau - les mines et les chiffres.
            afficherZones();
                        
            // Couvre le premier niveau d'un second niveau - les éléments qui cachent le jeu.
            afficherCouverture();

            JoueurMort = false;
        }

        /// <summary>
        /// Modifie la "Grid" du jeu pour qu'elle ait le bon nombre de colonnes et de rangées.
        /// Se base sur le ChampMines (qui doit donc avoir été généré).
        /// </summary>
        private void genererGrilleJeu()
        {
            ColumnDefinition colDefinition;
            RowDefinition rowDefinition;
            
            // Définir les colonnes et rangées de la Grid.
            for (int i = 0; i < Jeu.LargeurChampMine; i++)
            {
                colDefinition = new ColumnDefinition();
                colDefinition.Width = new GridLength(Zone.TAILLE_ZONE);

                grdChampMine.ColumnDefinitions.Add(colDefinition);
            }
            for (int i = 0; i < Jeu.HauteurChampMine; i++)
            {
                rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(Zone.TAILLE_ZONE);

                grdChampMine.RowDefinitions.Add(rowDefinition);
            }
        }

        /// <summary>
        /// Affiche les images associées à chaque Zone de la grille de jeu à l'écran.
        /// </summary>
        private void afficherZones()
        {
            List<Zone> colonne;
            Image imgAffichage;

            for (int i = 0; i <= Jeu.LstZones.Count - 1; i++)
            {
                colonne = Jeu.LstZones[i];

                for (int j = 0; j <= colonne.Count - 1; j++)
                {
                    imgAffichage = colonne[j].ImageZone;
                    
                    Grid.SetColumn(imgAffichage, i);
                    Grid.SetRow(imgAffichage, j);
                    // Les images "cachées" auront toutes un ZIndex = 1.
                    Grid.SetZIndex(imgAffichage,1);

                    grdChampMine.Children.Add(imgAffichage);
                }
            }

        }

        /// <summary>
        /// Créé la couverture du champ de mine. Sert à cacher les informations des cases.
        /// </summary>
        /// <remarks>
        /// Utilise des boutons pour faire la couverture.
        /// </remarks>
        private void afficherCouverture()
        {
            Button btnCouverture;

            for (int i = 0; i < Jeu.LstZones.Count; i++)
            {
                for (int j = 0; j < Jeu.LstZones[0].Count; j++)
                {
                    btnCouverture = new Button();

                    btnCouverture.Height = Zone.TAILLE_ZONE;
                    btnCouverture.Width = Zone.TAILLE_ZONE;
                    btnCouverture.Focusable = false;                    
                    // On précise les gestionnaires d'évènements pour le bouton.
                    btnCouverture.Click += new RoutedEventHandler(btnCouverture_Click);
                    btnCouverture.MouseRightButtonUp += new MouseButtonEventHandler(btnCouverture_MouseRightButtonUp);

                    Grid.SetColumn(btnCouverture, i);
                    Grid.SetRow(btnCouverture, j);
                    // Les boutons aurons tous une ZIndez de 2, plus haut que les éléments cachés qui sont à 1.
                    Grid.SetZIndex(btnCouverture, 2);

                    grdChampMine.Children.Add(btnCouverture);
                }
            }
        }

        /// <summary>
        /// Gestion des cliques gauches sur les boutons de couverture.
        /// Fonctionne quand le boutton est relaché (button up) pour permettre au joueur de changer d'idée (lacher le bouton ailleurs).
        /// </summary>
        /// <param name="sender">Le bouton qui doit être considéré comme la source de l'évènement</param>
        /// <param name="e"></param>
        private void btnCouverture_Click(object sender, RoutedEventArgs e)
        {
            Button btnSender;

            if (!JoueurMort)
            {
                btnSender = (Button)sender;

				// Puisqu'on utilise un StackPanel pour ajouter une image au bouton, 
				// la présence de ce type de "content" signifie qu'il y a une image.
				if ((btnSender.Content is StackPanel))
				{
					return;
				}
                // Faire disparaitre le bouton, quoiqu'il arrive.
                btnSender.Visibility = Visibility.Hidden;

                if (Jeu.LstZones[Grid.GetColumn(btnSender)][Grid.GetRow(btnSender)].ContientMine)
                {
                    Image imgBombe = new Image();
                    BitmapImage bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.UriSource = new Uri(@"Images\mine_explosion.png", UriKind.RelativeOrAbsolute);
                    bImg.DecodePixelWidth = Zone.TAILLE_ZONE;
                    bImg.EndInit();
                    imgBombe.Source = bImg;

                    Grid.SetColumn(imgBombe, Grid.GetColumn(btnSender));
                    Grid.SetRow(imgBombe, Grid.GetRow(btnSender));
                    // Afficher la bombe par dessus l'autre.
                    Grid.SetZIndex(imgBombe, 2);

                    grdChampMine.Children.Add(imgBombe);

                    JoueurMort = true;
                }
            }
        }

        /// <summary>
        /// Gestion des cliques de droites sur les boutons de couverture.
        /// Fonctionne quand le boutton est relaché (button up) pour permettre au joueur de changer d'idée (lacher le bouton ailleurs).
        /// </summary>
        /// <param name="sender">Le bouton qui doit être considéré comme la source de l'évènement</param>
        /// <param name="e"></param>
        private void btnCouverture_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button btnSender;

            if (!JoueurMort)
            {
                btnSender = (Button)sender;

                // Puisqu'on utilise un StackPanel pour ajouter une image au bouton, 
                // la présence de ce type de "content" signifie qu'il y a une image.
                if ((btnSender.Content is StackPanel))
                { 
                    btnSender.Content = null;
                }
                else
                {
                    ImageBrush ib = new ImageBrush();
                    Image img = new Image();
                    BitmapImage bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.UriSource = new Uri(@"Images\drapeau.png", UriKind.RelativeOrAbsolute);
                    bImg.DecodePixelWidth = Zone.TAILLE_ZONE;
                    bImg.EndInit();
                    img.Source = bImg;

                    StackPanel sp = new StackPanel();
                    //sp.Orientation = Orientation.Horizontal;
                    sp.Children.Add(img);

                    btnSender.Content = sp;
                }
            }
            
        }
    }
}
