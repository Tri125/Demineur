using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demineur
{
    /// <summary>
    /// Représente un espace du jeu qui peut contenir ou pas une mine.
    /// </summary>
    public class Zone
    {
        // La taille, en pixel, d'une zone lors de l'affichage.
        public static int TAILLE_ZONE;
        private int nbrMinesVoisins;

        #region Attributs


        public bool ContientMine { get; set; }
        public int NbrMinesVoisins
        {
            get
            {
                return nbrMinesVoisins;
            }
            private set
            {
                if (value >= 0 && value <= 8)
                    nbrMinesVoisins = value;
                else
                    throw new InvalidOperationException("La valeur de nbrMinesVoisins doit être entre 0 et 8");
            }
        }
        public ListeVoisin LstVoisins { get; private set; }

        #endregion


        /// <summary>
        /// Constructeur de base de la classe Zone.
        /// </summary>
        public Zone()
        {
            TAILLE_ZONE = App.config.OptionUtilisateur.TailleCases;
        }

        #region Méthodes

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
        public void assignerVoisins(Zone voisinNO, Zone voisinN, Zone voisinNE, Zone voisinO, Zone voisinE, Zone voisinSO, Zone voisinS, Zone voisinSE)
        {
            LstVoisins = new ListeVoisin(voisinNO, voisinN, voisinNE, voisinO, voisinE, voisinSO, voisinS, voisinSE);
        }

        public void assignerCompteur()
        {
            NbrMinesVoisins = compterMineVoisines();
        }

        #endregion

    }
}
