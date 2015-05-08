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

namespace Demineur
{
    /// <summary>
    /// Logique d'interaction pour FenetreChampMine.xaml
    /// </summary>
    public partial class FenetreChampMines : UserControl
    {
        //  Event et delegate lorsqu'une partie de jeu est terminé.
        public delegate void PartieTermineEventHandler(object sender);
        public event PartieTermineEventHandler Terminer;

        // Event et delegate lorsqu'un drapeau est rajouté/retiré du jeu.
        public delegate void DrapeauEventHandler(object sender, DrapeauEventArgs e);
        public event DrapeauEventHandler Drapeau;

        private ChampMines Jeu { get; set; }
        private bool JoueurMort { get; set; }
        private bool PartieTermine { get; set; }

        public readonly int TAILLE_CASES;

        //  Nombre de cases sans mines restantes qui doivent être dévoilées afin de gagner la partie.
        private int NbrCasesRestantes { get; set; }

        public FenetreChampMines(int largeur, int hauteur, int nbMines, int tailleCase, bool minesCoins)
        {
            TAILLE_CASES = tailleCase;

            InitializeComponent();

            // Générer la structure du champ de mines.
            Jeu = new ChampMines(largeur, hauteur, nbMines, minesCoins);

            NbrCasesRestantes = (largeur * hauteur) - nbMines;

            // Modifie la Grid pour correspondre au champ de mine du jeu.
            genererGrilleJeu();

            // Affiche le premier niveau - les mines et les chiffres.
            afficherZones();

            // Couvre le premier niveau d'un second niveau - les éléments qui cachent le jeu.
            afficherCouverture();

            JoueurMort = false;

            //  Si dans les configurations l'utilisateur ne veut pas de mines dans les coins
            if (!minesCoins)
                //  Dévoile les coins comme si le joueur aurait cliqué sur les boutons.
                ReveleCoins();

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
                colDefinition.Width = new GridLength(TAILLE_CASES);

                grdChampMine.ColumnDefinitions.Add(colDefinition);
            }
            for (int i = 0; i < Jeu.HauteurChampMine; i++)
            {
                rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(TAILLE_CASES);

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
                bImg.DecodePixelWidth = TAILLE_CASES * 10;
                bImg.EndInit();

                imageZone.Source = bImg;
            }
            else
            {
                if (nbMines != 0)
                {
                    bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.DecodePixelWidth = TAILLE_CASES * 10;  // Définir plus grand que requis?

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

                    border.Background = Brushes.Transparent;
                    border.MouseDown += new MouseButtonEventHandler(btnCouverture_MouseDown);
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

                    btnCouverture.Height = TAILLE_CASES;
                    btnCouverture.Width = TAILLE_CASES;
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

        private void btnCouverture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!PartieTermine)
            {
                if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Pressed)
                {
                    if (sender is Border)
                    {
                        Border b = sender as Border;
                        int column = Grid.GetColumn(b);
                        int row = Grid.GetRow(b);
                        int nbrDrapeau = 0;
                        int nbrMines = 0;
                        List<Button> btnList = new List<Button>();
                        for (int i = 0; i < 8; i++)
                        {
                            Zone courant = Jeu.LstZones[column][row].LstVoisins[i];
                            if (courant == null)
                                continue;
                            if (courant.ContientMine)
                                nbrMines++;
                            int tmpRow = 0;
                            int tmpColumn = 0;
                            ObtenirCoordGrille(courant, ref tmpRow, ref tmpColumn);
                            Button btn = buttonFromCoord(tmpRow, tmpColumn);
                            if (btn.Content is StackPanel)
                                nbrDrapeau++;
                            btnList.Add(btn);
                        }

                        if (nbrDrapeau == nbrMines)
                        {
                            foreach (Button btn in btnList)
                            {
                                ActivateButton(btn);
                            }
                        }
                    }
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
            if (!PartieTermine)
            {
                if (sender is Button)
                    ActivateButton(sender as Button);
            }
        }

        private Button buttonFromCoord(int row, int column)
        {
            Object bouton = grdChampMine.Children
            .Cast<UIElement>()
            .First(s => Grid.GetRow(s) == row && Grid.GetColumn(s) == column && Grid.GetZIndex(s) == 2);
            return (bouton as Button);
        }

        private void ActivateButton(Button btnSender)
        {
            int column;
            int row;

            // Puisqu'on utilise un StackPanel pour ajouter une image au bouton, 
            // la présence de ce type de "content" signifie qu'il y a une image.
            if ((btnSender.Content is StackPanel))
            {
                return;
            }
            if (btnSender.Visibility == System.Windows.Visibility.Hidden)
                return;
            NbrCasesRestantes--;
            btnSender.Visibility = Visibility.Hidden;

            column = Grid.GetColumn(btnSender);
            row = Grid.GetRow(btnSender);

            if ((Jeu.LstZones[column][row].NbrMinesVoisins == 0 && !Jeu.LstZones[column][row].ContientMine))
            {
                for (int i = 0; i < 8; i++)
                {
                    if (Jeu.LstZones[column][row].LstVoisins[i] != null)
                    {
                        int zRow = 0;
                        int zColumn = 0;
                        ObtenirCoordGrille(Jeu.LstZones[column][row].LstVoisins[i], ref zRow, ref zColumn);
                        Button btn = buttonFromCoord(zRow, zColumn);
                        if (btn.Visibility == System.Windows.Visibility.Visible)
                            ActivateButton(btn);
                    }
                }
            }
            ReveleBordure(row, column);
            if (NbrCasesRestantes == 0)
            {
                Gagnee();
            }


            if (Jeu.LstZones[column][row].ContientMine)
            {
                Image imgBombe = new Image();
                BitmapImage bImg = new BitmapImage();
                bImg.BeginInit();
                bImg.UriSource = new Uri(@"Images\mine_explosion.png", UriKind.RelativeOrAbsolute);
                bImg.DecodePixelWidth = TAILLE_CASES;
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
                    ChangementDrapeau(false);
                }
                else
                {
                    ImageBrush ib = new ImageBrush();
                    Image img = new Image();
                    BitmapImage bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.UriSource = new Uri(@"Images\drapeau.png", UriKind.RelativeOrAbsolute);
                    bImg.DecodePixelWidth = TAILLE_CASES;
                    bImg.EndInit();
                    img.Source = bImg;

                    StackPanel sp = new StackPanel();
                    //sp.Orientation = Orientation.Horizontal;
                    sp.Children.Add(img);

                    btnSender.Content = sp;
                    ChangementDrapeau(true);
                }
            }

        }

        private void ReveleBordure(int row, int column)
        {
            //http://stackoverflow.com/questions/1511722/how-to-programmatically-access-control-in-wpf-grid-by-row-and-column-index
            Object border = grdChampMine.Children
                .Cast<UIElement>()
                .First(s => Grid.GetRow(s) == row && Grid.GetColumn(s) == column && Grid.GetZIndex(s) == 1);
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

        private void ChangementDrapeau(bool rajout)
        {
            if (Drapeau != null)
            {
                if (rajout)
                    Drapeau(this, DrapeauEventArgs.Rajout);
                else
                    Drapeau(this, DrapeauEventArgs.Retrait);

            }

        }


        private void ReveleCoins()
        {
            List<Tuple<int, int>> listCoord = new List<Tuple<int, int>>();
            //  Coin en haut à gauche
            listCoord.Add(Tuple.Create(0, 0));
            //  Coin en haut à droite
            listCoord.Add(Tuple.Create(0, Jeu.LargeurChampMine - 1));
            //  Coin en bas à gauche
            listCoord.Add(Tuple.Create(Jeu.HauteurChampMine - 1, 0));
            // Coin en bas à droite
            listCoord.Add(Tuple.Create(Jeu.HauteurChampMine - 1, Jeu.LargeurChampMine - 1));

            foreach (Tuple<int, int> coord in listCoord)
            {
                ActivateButton(buttonFromCoord(coord.Item1, coord.Item2));
            }

        }
    }
}
