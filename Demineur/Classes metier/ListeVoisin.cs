using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demineur
{
    /// <summary>
    /// Une liste des zones qui sont voisine à une zone en particulier.
    /// Sert à gérer la structure d'un tableau de jeu.
    /// </summary>
    public class ListeVoisin
    {
        #region Attributs

        public Zone VoisinNO { get; private set; }
        public Zone VoisinN { get; private set; }
        public Zone VoisinNE { get; private set; }
        public Zone VoisinO { get; private set; }
        public Zone VoisinE { get; private set; }
        public Zone VoisinSO { get; private set; }
        public Zone VoisinS { get; private set; }
        public Zone VoisinSE { get; private set; }

        #endregion

        #region Indexer
        // Représentation en 2D de la carte de l'indexer:
        /*
         * [0]  [1] [2]
         * 
         * [3]  [x] [4]
         * 
         * [5]  [6] [7]
         */
        /// <summary>
        /// Permet d'accéder aux Zones selon une index.
        /// D'une grille 2D, [0] étant VoisinNO, le centre étant l'instance actuel (aucune index pour y accéder)
        /// et [7] étant VoisinSE
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Zone this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return VoisinNO;
                    case 1:
                        return VoisinN;
                    case 2:
                        return VoisinNE;
                    case 3:
                        return VoisinO;
                    case 4:
                        return VoisinE;
                    case 5:
                        return VoisinSO;
                    case 6:
                        return VoisinS;
                    case 7:
                        return VoisinSE;
                    default:
                        throw new IndexOutOfRangeException();

                }
            }
        }
        #endregion

        /// <summary>
        /// Constructeur qui permet d'indiquer les voisins de la liste.
        /// </summary>
        /// <param name="voisinNO">Le voisin en haut à gauche (nord ouest).</param>
        /// <param name="voisinN">Le voisin en haut (nord).</param>
        /// <param name="voisinNE">Le voisin en haut à droite (nord est).</param>
        /// <param name="voisinO">Le voisin à gauche (ouest).</param>
        /// <param name="voisinE">Le voisin à droite (est).</param>
        /// <param name="voisinSO">Le voisin en base à gauche (sud ouest).</param>
        /// <param name="voisinS">Le voisin en bas (sud).</param>
        /// <param name="voisinSE">Le voisin en bas à droite (sud est).</param>
        public ListeVoisin(Zone voisinNO, Zone voisinN, Zone voisinNE, Zone voisinO, Zone voisinE, Zone voisinSO, Zone voisinS, Zone voisinSE)
        {
            assignerVoisins(voisinNO, voisinN, voisinNE, voisinO, voisinE, voisinSO, voisinS, voisinSE);
        }

        #region Méthodes

        /// <summary>
        /// Permet d'indiquer les voisins de la liste.
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
            VoisinNO = voisinNO;
            VoisinN = voisinN;
            VoisinNE = voisinNE;
            VoisinO = voisinO;
            VoisinE = voisinE;
            VoisinSO = voisinSO;
            VoisinS = voisinS;
            VoisinSE = voisinSE;
        }

        #endregion

    }
}
