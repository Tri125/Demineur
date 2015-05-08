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

        public MemoirePartie MemoireJeu { get; set; }

        //  Nombre de cases sans mines restantes qui doivent être dévoilées afin de gagner la partie.
        private int NbrCasesRestantes { get; set; }

        public FenetreChampMines(int largeur, int hauteur, int nbMines, int tailleCase, bool minesCoins)
        {
            MemoireJeu = new MemoirePartie();
            MemoireJeu.CliqueDroit = new List<Point>();
            MemoireJeu.CliqueGauche = new List<Point>();
            MemoireJeu.Configuration = new ConfigJoueur(minesCoins, tailleCase, nbMines, hauteur, largeur);
            TAILLE_CASES = tailleCase;

            InitializeComponent();

            // Générer la structure du champ de mines.
            Jeu = new ChampMines(largeur, hauteur, nbMines, minesCoins);
            MemoireJeu.RandomSeed = Jeu.Seed;
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


        public FenetreChampMines(MemoirePartie memoire)
        {
            MemoireJeu = memoire;
            MemoireJeu.CliqueDroit = memoire.CliqueDroit;
            MemoireJeu.CliqueGauche = memoire.CliqueGauche;

            bool minesCoins = MemoireJeu.Configuration.MinesCoins;
            int largeur = MemoireJeu.Configuration.Largeur;
            int hauteur = MemoireJeu.Configuration.Hauteur;
            int nbMines = MemoireJeu.Configuration.NombresMines;

            TAILLE_CASES = MemoireJeu.Configuration.TailleCases;

            InitializeComponent();

            // Générer la structure du champ de mines.
            Jeu = new ChampMines(largeur, hauteur, nbMines, MemoireJeu.RandomSeed, minesCoins);
            MemoireJeu.RandomSeed = Jeu.Seed;
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

        public void RejouerParMemoire(List<Point> actionGauche, List<Point> actionDroite)
        {
            foreach  (Point p in actionGauche)
            {
                ActivateButton(buttonFromCoord((int)p.X, (int)p.Y));
            }

            foreach (Point p in actionDroite)
            {
                ToggleDrapeau(buttonFromCoord((int)p.X, (int)p.Y));
            }
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
        /// Retourne une image selon le nombre de mines adjacentes et contientMine.
        /// </summary>
        /// <param name="contientMine">Vrai si nous voulons l'image d'une case avec une mine</param>
        /// <param name="nbMines">Nombre de mines adjacentes</param>
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
                    //  Récupère une image selon le nombre de mines adjacentes et si cette Zone contient une mine
                    imgAffichage = assignerImage(colonne[j].ContientMine, colonne[j].NbrMinesVoisins);

                    //  Border transparent sur chaque case. Lorsque la case est dévoilé, le border sera mit noir.
                    border = new Border();
                    border.BorderBrush = Brushes.Transparent;
                    border.BorderThickness = new Thickness(1, 1, 1, 1);

                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, j);
                    // Les images "cachées" auront toutes un ZIndex = 1.
                    Grid.SetZIndex(border, 1);
                    border.Child = imgAffichage;
                    grdChampMine.Children.Add(border);

                    //  Pour les cases vides, imgAffichage a une source null, ce qui fait qu'elle ne sera pas rendu à l'écran.
                    //  Si elle n'est pas rendu à l'écran, elle ne pourra pas Handler des évênements, comme par example le double clique.
                    //  En rajoutant un Background au border, le contrôle sera rendu et les évênements seront traités.
                    border.Background = Brushes.Transparent;
                    //  Pour le double clique sur une case dévoilée.
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

        // Lorsqu'un bouton de la sourit est appuyé
        private void btnCouverture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!PartieTermine)
            {
                //  Si le bouton de gauche et de droite sont appuyés, alors c'est un double clique.
                if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Pressed)
                {
                    if (sender is Border)
                    {
                        Border b = sender as Border;
                        DoubleClique(b);
                    }
                }
            }
        }

        /// <summary>
        /// Si le nombre de drapeau sur les cases adjacentes du Border est égal au nombre de mines, alors les cases seront toutes dévoilées.
        /// </summary>
        /// <param name="sender"></param>
        private void DoubleClique(Border sender)
        {
            int column = Grid.GetColumn(sender);
            int row = Grid.GetRow(sender);
            int nbrDrapeau = 0;
            int nbrMines = 0;
            // Liste des boutons adjacents à la position du Border.
            List<Button> btnList = new List<Button>();
            for (int i = 0; i < 8; i++)
            {
                // Utilisation de l'index de ListeVoisin pour une itération bien rapide et sans écrire de longue lignes avec des noms d'attributs très spécifiques...
                Zone courant = Jeu.LstZones[column][row].LstVoisins[i];
                if (courant == null)
                    continue;
                if (courant.ContientMine)
                    nbrMines++;
                int tmpRow = 0;
                int tmpColumn = 0;
                //  Obtient les coordonnées de la grille de la Zone.
                ObtenirCoordGrille(courant, ref tmpRow, ref tmpColumn);
                //  Obtient un contrôle bouton à partir des coordonnées de la grille.
                Button btn = buttonFromCoord(tmpRow, tmpColumn);
                // Drapeau utilise StackPanel comme contenue.
                if (btn.Content is StackPanel)
                    nbrDrapeau++;
                btnList.Add(btn);
            }

            if (nbrDrapeau == nbrMines)
            {
                foreach (Button btn in btnList)
                {
                    ActivateButton(btn);
                    EnregistreActionCliqueGauche(btn);
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
                {
                    Button btn = sender as Button;
                    ActivateButton(btn);
                    EnregistreActionCliqueGauche(btn);
                }
            }
        }

        private void EnregistreActionCliqueGauche(Button btn)
        {
            // Puisqu'on utilise un StackPanel pour ajouter une image au bouton, 
            // la présence de ce type de "content" signifie qu'il y a une image.
            if ((btn.Content is StackPanel))
            {
                return;
            }
            MemoireJeu.CliqueGauche.Add(new Point(Grid.GetRow(btn), Grid.GetColumn(btn)));
        }


        /// <summary>
        /// Obtient un contrôle Button de la grille à partir du numéro de colonne et du numéro de ligne.
        /// </summary>
        /// <param name="row">X (Largeur)</param>
        /// <param name="column">Y (Hauteur)</param>
        /// <returns></returns>
        private Button buttonFromCoord(int row, int column)
        {
            // Lance une exception si aucun résultat
            Object bouton = grdChampMine.Children
            .Cast<UIElement>()
            .First(s => Grid.GetRow(s) == row && Grid.GetColumn(s) == column && Grid.GetZIndex(s) == 2);
            return (bouton as Button);
        }

        /// <summary>
        /// À partir d'un Button, active le comme il aurait été cliqué.
        /// </summary>
        /// <param name="btnSender">Le bouton à activé</param>
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
            //  Si caché, alors le bouton a déjà été dévoilée.
            if (btnSender.Visibility == System.Windows.Visibility.Hidden)
                return;

            NbrCasesRestantes--;
            //  Cache le bouton pour montrer la case en dessous.
            btnSender.Visibility = Visibility.Hidden;

            column = Grid.GetColumn(btnSender);
            row = Grid.GetRow(btnSender);

            //  Si la case ne contient pas de mine et qu'il n'y a pas de mines adjacentes, alors il faut propager l'activation du bouton sur les voisins.
            if ((Jeu.LstZones[column][row].NbrMinesVoisins == 0 && !Jeu.LstZones[column][row].ContientMine))
            {
                for (int i = 0; i < 8; i++)
                {
                    //  Utilise l'indexer de ListeVoisin pour vérifier les voisins bien rapidement.
                    //  Si null, alors l'origine est un coin et nous sommes à l'extérieur du jeu.
                    if (Jeu.LstZones[column][row].LstVoisins[i] != null)
                    {
                        int zRow = 0;
                        int zColumn = 0;
                        //  À partir de la Zone, récupère ses coordonnées dans la grille.
                        ObtenirCoordGrille(Jeu.LstZones[column][row].LstVoisins[i], ref zRow, ref zColumn);
                        //  Récupère le contrôle Button à ces même coordonnées.
                        Button btn = buttonFromCoord(zRow, zColumn);
                        // Si le Button est visible, alors il n'a pas déjà été dévoilé.
                        // Utilisation de VIsibility à la place de IsVisible, car IsVisible ne regarde pas la logique, mais bien le rendu à l'écran.
                        // L'activation des boutons dans les coins (avec l'option de configuration) est lancé bien avant que l'affichage soit complété, donc la
                        // condition était innappropriée.
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
                ToggleDrapeau(btnSender);
                EnregistreDrapeaux(btnSender);
            }

        }

        // À l'utiliser seulement lorsqu'on est pret en l'enregistrement de la partie puisqu'un drapeau peut être enlevé et rajouté n'importe quand et comment on est
        // seulement interessé au dernier état.
        // TODO: Uniquement prendre le dernier état.
        private void EnregistreDrapeaux(Button btn)
        {
            MemoireJeu.CliqueDroit.Add(new Point(Grid.GetRow(btn), Grid.GetColumn(btn)));
        }

        private void ToggleDrapeau(Button btnSender)
        {
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

        /// <summary>
        /// À partir des coordonnées dans la grille, révèle la bordure qui si trouve.
        /// Pour faire apparaitre la grille de jeu sur les cases dévoilées.
        /// </summary>
        /// <param name="row">X</param>
        /// <param name="column">Y</param>
        private void ReveleBordure(int row, int column)
        {
            //http://stackoverflow.com/questions/1511722/how-to-programmatically-access-control-in-wpf-grid-by-row-and-column-index
            //  Lance une exception si aucun résultat.
            Object border = grdChampMine.Children
                .Cast<UIElement>()
                .First(s => Grid.GetRow(s) == row && Grid.GetColumn(s) == column && Grid.GetZIndex(s) == 1);
            if (border is Border)
            {
                //  Change la Brush de la bordure en noir.
                (border as Border).BorderBrush = Brushes.Black;
            }
        }

        /// <summary>
        /// À partir d'une Zone, retrouve ses coordonnées dans la List de List de zone.
        /// Les coordonnées map directement l'emplacement sur la grille de jeu.
        /// </summary>
        /// <param name="z">La Zone à trouvé</param>
        /// <param name="row">X Le résultat sera mit dans la variable</param>
        /// <param name="column">Y Le résultat sera mit dans la variable</param>
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

        /// <summary>
        /// Lorsque le joueur perd la partie.
        /// </summary>
        private void Perdu()
        {
            //  La partie est terminé. Il sera impossible de cliquer.
            PartieTermine = true;
            //  Statut du joueur
            JoueurMort = true;
            // Quelqu'un inscrit à l'event Terminer? Si oui, on le lance.
            if (Terminer != null)
                // Lance l'évênement avec comme sender le statut du joueur
                // TODO: Utiliser sender et EventArgs comme il faut parce que je me sens vraiment mal d'écrire sa.
                Terminer(JoueurMort);
        }

        private void Gagnee()
        {
            //  La partie est terminé. Il sera impossible de cliquer.
            PartieTermine = true;
            //  Statut du joueur
            JoueurMort = false;
            // Quelqu'un d'inscrit à l'event Terminer? Si oui, on le lance.
            if (Terminer != null)
                // Lance l'évênement avec comme sender le statut du joueur
                // TODO: Utiliser sender et EventArgs comme il faut parce que je me sens vraiment mal d'écrire sa.
                Terminer(JoueurMort);
        }

        /// <summary>
        /// Lance un évênement signalant une modification des drapeaux
        /// </summary>
        /// <param name="rajout">Si un drapeau a été rajouté ou non</param>
        private void ChangementDrapeau(bool rajout)
        {
            //  Si quelqu'un est inscrit à l'évênement Drapeau.
            if (Drapeau != null)
            {
                // Lance l'évênement Drapeau avec DrapeauEventArgs selon si oui ou non un drapeau a été retiré ou rajouté.
                if (rajout)
                    Drapeau(this, DrapeauEventArgs.Rajout);
                else
                    Drapeau(this, DrapeauEventArgs.Retrait);

            }

        }

        /// <summary>
        /// Méthode pour activer les boutons dans les coins de la grille.
        /// </summary>
        private void ReveleCoins()
        {
            //  Utilisation des tuples à la place de Point pour aucune raison justifiable.
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
