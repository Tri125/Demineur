using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Demineur
{
    /// <summary>
    /// Représente un espace du jeu qui peut contenir ou pas une mine.
    /// </summary>
    public class Zone
    {
        // La taille, en pixel, d'une zone lors de l'affichage.
        public static int TAILLE_ZONE = 20;

        #region Attributs


        public bool ContientMine { get; set; }

        public ListeVoisin LstVoisins { get; private set; }

        public Image ImageZone { get; private set; }

        #endregion

        /// <summary>
        /// Constructeur de base de la classe Zone.
        /// </summary>
        public Zone()
        {
        }

        #region Méthodes

        /// <summary>
        /// Demande à la zone de vérifier son contenu et ses voisins et d'ajuster son image en conséquence.
        /// </summary>
        public void assignerImage()
        {
            // L'image de mine est tirée de http://doc.ubuntu-fr.org/gnomine.
            ImageZone = new Image();
            BitmapImage bImg;
            int nbMines;

            if (ContientMine)
            {
                bImg = new BitmapImage();
                bImg.BeginInit();
                bImg.UriSource = new Uri(@"Images\mine.png", UriKind.RelativeOrAbsolute);
                bImg.DecodePixelWidth = TAILLE_ZONE * 10;
                bImg.EndInit();

                ImageZone.Source = bImg;
            }
            else
            { 
                nbMines = compterMineVoisines(); 
                if (nbMines != 0)
                {
                    bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.DecodePixelWidth = TAILLE_ZONE * 10;  // Définir plus grand que requis?

                    switch (nbMines)
                    {
                        case 1 : 
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
                    ImageZone.Source = bImg;
                }
                else
                { 
                    // Zone vide sans mine avoisinante = pas d'image.
                    ImageZone.Source = null;
                }
            }
        }

        /// <summary>
        /// Permet de compter le nombre de mines chez les voisins de la zone.
        /// </summary>
        /// <returns>Une valeur entre 0 et n, n étant égal au nombre de voisins de la case.</returns>
        public int compterMineVoisines()
        {
            int nbMines = 0;

            if (LstVoisins.VoisinNO != null && LstVoisins.VoisinNO.ContientMine) { nbMines++; }
            if (LstVoisins.VoisinN != null && LstVoisins.VoisinN.ContientMine) { nbMines++; }
            if (LstVoisins.VoisinNE != null && LstVoisins.VoisinNE.ContientMine) { nbMines++; }
            if (LstVoisins.VoisinO != null && LstVoisins.VoisinO.ContientMine) { nbMines++; }
            if (LstVoisins.VoisinE != null && LstVoisins.VoisinE.ContientMine) { nbMines++; }
            if (LstVoisins.VoisinSO != null && LstVoisins.VoisinSO.ContientMine) { nbMines++; }
            if (LstVoisins.VoisinS != null && LstVoisins.VoisinS.ContientMine) { nbMines++; }
            if (LstVoisins.VoisinSE != null && LstVoisins.VoisinSE.ContientMine) { nbMines++; }

            return nbMines;
        }

        /// <summary>
        /// Permet d'indiquer quels sont les voisins de la zone.
        /// Écrase les valeurs présentes s'il y en a.
        /// </summary>
        /// <param name="voisinNO">Le voisin en haut à gauche (nord ouest).</param>
        /// <param name="voisinN">Le voisin en haut (nord).</param>
        /// <param name="voisinNE">Le voisin en haut à droite (nord est).</param>
        /// <param name="voisinO">Le voisin à gauche (ouest).</param>
        /// <param name="voisinE">Le voisin à droite (est).</param>
        /// <param name="voisinSO">Le voisin en base à gauche (sud ouest).</param>
        /// <param name="voisinS">Le voisin en bas (sud).</param>
        /// <param name="voisinSE">Le voisin en bas à droite (sud est).</param>
        public void assignerVoisins (Zone voisinNO, Zone voisinN, Zone voisinNE, Zone voisinO, Zone voisinE, Zone voisinSO, Zone voisinS, Zone voisinSE)
        {
            LstVoisins = new ListeVoisin(voisinNO, voisinN, voisinNE, voisinO, voisinE, voisinSO, voisinS, voisinSE);
        }



        #endregion

    }
}
