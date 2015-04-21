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
        public delegate void PartieTermineEventHandler(object sender);
        public event PartieTermineEventHandler Terminer;

        private ChampMines Jeu { get; set; }
        private bool JoueurMort { get; set; }
        private bool PartieTermine { get; set; }
        private int nbrCasesRestantes;

        public FenetreChampMines(int largeur, int hauteur, int nbMines)
        {
            InitializeComponent();

            // Générer la structure du champ de mines.
            Jeu = new ChampMines(largeur, hauteur, nbMines);

            nbrCasesRestantes = (largeur * hauteur) - nbMines;

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
        /// DADASDFAFASFGASGASGSGSDGDemande à la zone de vérifier son contenu et ses voisins et d'ajuster son image en conséquence.
        /// </summary>
        private Image assignerImage(bool contientMine, int nbMines = 0)
        {
            Image imageZone;
            // L'image de mine est tirée de http://doc.ubuntu-fr.org/gnomine.
            imageZone = new Image();
            BitmapImage bImg;
            if (contientMine)
            {
                bImg = new BitmapImage();
                bImg.BeginInit();
                bImg.UriSource = new Uri(@"Images\mine.png", UriKind.RelativeOrAbsolute);
                bImg.DecodePixelWidth = Zone.TAILLE_ZONE * 10;
                bImg.EndInit();

                imageZone.Source = bImg;
            }
            else
            {
                if (nbMines != 0)
                {
                    bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.DecodePixelWidth = Zone.TAILLE_ZONE * 10;  // Définir plus grand que requis?

                    switch (nbMines)
                    {
                        case 1:
                            bImg.UriSource = new Uri(@"Images\chiffre1.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 2:
                            bImg.UriSource = new Uri(@"Images\chiffre2.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 3:
                            bImg.UriSource = new Uri(@"Images\chiffre3.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 4:
                            bImg.UriSource = new Uri(@"Images\chiffre4.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 5:
                            bImg.UriSource = new Uri(@"Images\chiffre5.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 6:
                            bImg.UriSource = new Uri(@"Images\chiffre6.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 7:
                            bImg.UriSource = new Uri(@"Images\chiffre7.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 8:
                            bImg.UriSource = new Uri(@"Images\chiffre8.png", UriKind.RelativeOrAbsolute);
                            break;
                    }

                    bImg.EndInit();
                    imageZone.Source = bImg;
                }
                else
                {
                    // Zone vide sans mine avoisinante = pas d'image.
                    imageZone.Source = null;
                }

            }
            return imageZone;
        }


        /// <summary>
        /// Affiche les images associées à chaque Zone de la grille de jeu à l'écran.
        /// </summary>
        private void afficherZones()
        {
            List<Zone> colonne;
            Image imgAffichage;
            Border border;
            for (int i = 0; i <= Jeu.LstZones.Count - 1; i++)
            {
                colonne = Jeu.LstZones[i];

                for (int j = 0; j <= colonne.Count - 1; j++)
                {
                    imgAffichage = assignerImage(colonne[j].ContientMine, colonne[j].NbrMinesVoisins);

                    border = new Border();
                    border.BorderBrush = Brushes.Transparent;
                    border.BorderThickness = new Thickness(1, 1, 1, 1);

                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, j);
                    // Les images "cachées" auront toutes un ZIndex = 1.
                    Grid.SetZIndex(border, 1);
                    border.Child = imgAffichage;
                    grdChampMine.Children.Add(border);
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

            if (!PartieTermine)
            {
                int column;
                int row;
                btnSender = (Button)sender;

                // Puisqu'on utilise un StackPanel pour ajouter une image au bouton, 
                // la présence de ce type de "content" signifie qu'il y a une image.
                if ((btnSender.Content is StackPanel))
                {
                    return;
                }

                column = Grid.GetColumn(btnSender);
                row = Grid.GetRow(btnSender);

                foreach (Zone z in ParcoursZone.ObtenirCaseVidePropager(Jeu.LstZones[column][row]))
                {
                    nbrCasesRestantes--;
                    int columnPropager = 0;
                    int rowPropager = 0;
                    ObtenirCoordGrille(z, ref rowPropager, ref columnPropager);
                    Object obj = grdChampMine.Children
                    .Cast<UIElement>()
                    .First(s => Grid.GetRow(s) == rowPropager && Grid.GetColumn(s) == columnPropager && Grid.GetZIndex(s) == 2);
                    Button button = (obj as Button);
                    if ((button.Content is StackPanel))
                    {
                        continue;
                    }
                    button.Visibility = Visibility.Hidden;
                    //ReveleBoutonCoord(rowPropager, columnPropager);
                    ReveleBordure(rowPropager, columnPropager);
                    if (nbrCasesRestantes == 0)
                    {
                        Gagnee();
                    }
                }

                if (Jeu.LstZones[column][row].ContientMine)
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

                    Perdu();
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

            if (!PartieTermine)
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

        private void ReveleBordure(int row, int column)
        {
            //http://stackoverflow.com/questions/1511722/how-to-programmatically-access-control-in-wpf-grid-by-row-and-column-index
            Object border = grdChampMine.Children
                .Cast<UIElement>()
                .First(s => Grid.GetRow(s) == row && Grid.GetColumn(s) == column);
            if (border is Border)
            {
                (border as Border).BorderBrush = Brushes.Black;
            }
        }

        private void ObtenirCoordGrille(Zone z, ref int row, ref int column)
        {
            for (int i = 0; i < Jeu.LstZones.Count; i++)
            {
                for (int j = 0; j < Jeu.LstZones[0].Count; j++)
                {
                    if (Jeu.LstZones[i][j] == z)
                    {
                        column = i;
                        row = j;
                    }
                }
            }
        }

        //private void ReveleBoutonCoord(int row, int column)
        //{
        //    Object bouton = grdChampMine.Children
        //    .Cast<UIElement>()
        //    .First(s => Grid.GetRow(s) == row && Grid.GetColumn(s) == column && Grid.GetZIndex(s) == 2);
        //    (bouton as Button).Visibility = Visibility.Hidden;
        //}

        private void Perdu()
        {
            PartieTermine = true;
            JoueurMort = true;
            if (Terminer != null)
                Terminer(JoueurMort);
        }

        private void Gagnee()
        {
            PartieTermine = true;
            JoueurMort = false;
            if (Terminer != null)
                Terminer(JoueurMort);
        }

    }
}
