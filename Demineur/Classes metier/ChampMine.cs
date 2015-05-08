using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demineur
{
    public class ChampMines
    {
        #region Attributs

        public List<List<Zone>> LstZones { get; private set; }
        public int LargeurChampMine { get; private set; }
        public int HauteurChampMine { get; private set; }
        private Random Hasard { get; set; }
        public int Seed { get; private set; }

        #endregion

        //  Valeur par défaut de minesCoins pour garder l'interface du constructeur stable.
        public ChampMines(int largeur, int hauteur, int nbMines, bool minesCoins = true)
        {
            if (largeur < 2 || hauteur < 2)
            {
                throw new Exception("Un champ de mine ne peut avoir une dimension plus petite que 2 en largeur ou en hauteur.");
            }
            Seed = DateTime.Now.Millisecond;
            Hasard = new Random(Seed);
            LargeurChampMine = largeur;
            HauteurChampMine = hauteur;

            initialiserChampMines(nbMines, minesCoins);
        }

        //  Valeur par défaut de minesCoins pour garder l'interface du constructeur stable.
        public ChampMines(int largeur, int hauteur, int nbMines, int seed, bool minesCoins = true)
        {
            if (largeur < 2 || hauteur < 2)
            {
                throw new Exception("Un champ de mine ne peut avoir une dimension plus petite que 2 en largeur ou en hauteur.");
            }
            Seed = seed;
            Hasard = new Random(Seed);
            LargeurChampMine = largeur;
            HauteurChampMine = hauteur;

            initialiserChampMines(nbMines, minesCoins);
        }

        #region Méthodes

        /// <summary>
        /// Cette méthode ordonne les étapes de création d'un champ de mine.
        /// </summary>
        /// <param name="nbMines">Le nombre de mines qui doivent se retrouver dans le champ de mines.</param>
        /// <param name="minesCoins">Si oui ou non nous voulons générer des mines dans les coins.</param>
        private void initialiserChampMines(int nbMines, bool minesCoins)
        {
            genererChampMines(LargeurChampMine, HauteurChampMine);
            //  Vérifie l'option utilisateur de mines dans les coins
            if (minesCoins)
                assignerMines(nbMines);
            else
                // Si l'utilisateur ne veut pas de mines dans les coins, une autre méthode est exécuté
                assignerMinesSansCoins(nbMines);

            lierZones();

            assignerCompteurs();
        }


        /// <summary>
        /// Génère la structure à deux dimensions qui va contenir les zones.
        /// </summary>
        /// <param name="largeur">La largeur du champ de mine (axe X).</param>
        /// <param name="hauteur">Le hauteur du champ de mine (axe Y).</param>
        private void genererChampMines(int largeur, int hauteur)
        {
            List<List<Zone>> liste2D = new List<List<Zone>>();

            for (int i = 0; i < largeur; i++)
            {
                liste2D.Add(new List<Zone>());
            }

            foreach (List<Zone> lz in liste2D)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    lz.Add(new Zone());
                }
            }

            LstZones = liste2D;
        }

        /// <summary>
        /// À partir d'une liste de zones initialisée, assigne au hasard le nombre de mines demandées.
        /// </summary>
        private void assignerMines(int nbMines)
        {
            int x, y;
            Zone zoneCible;
            int minesGeneree = 0;

            while (minesGeneree < nbMines)
            {
                x = Hasard.Next(LstZones.Count);
                y = Hasard.Next(LstZones[0].Count);

                zoneCible = LstZones[x][y];

                if (!zoneCible.ContientMine)
                {
                    zoneCible.ContientMine = true;
                    minesGeneree++;
                }
            }

        }

        /// <summary>
        /// À partir d'une liste de zones initialisée, assigne au hasard le nombre de mines demandées sauf dans les coins
        /// </summary>
        /// <param name="nbMines"></param>
        private void assignerMinesSansCoins(int nbMines)
        {
            // Honnêtement, le code ce répète, mais j'ai préféré faire sa à la place de modifier le code existant.
            int x, y;
            Zone zoneCible;
            int minesGeneree = 0;

            while (minesGeneree < nbMines)
            {
                do
                {
                    x = Hasard.Next(LstZones.Count);
                    y = Hasard.Next(LstZones[0].Count);
                } while ( (x == 0 && y == 0) || (x == LstZones.Count -1 && y == 0) || (x == 0 && y == LstZones[0].Count - 1) || ( x == LstZones.Count() -1 && y == LstZones[0].Count() -1) );
                zoneCible = LstZones[x][y];


                if (!zoneCible.ContientMine)
                {
                    zoneCible.ContientMine = true;
                    minesGeneree++;
                }
            }
        }

        /// <summary>
        /// Lie les zones voisines du champs de mines entres elles.
        /// </summary>
        private void lierZones()
        {
            // Faire les coins.
            LstZones[0][0].assignerVoisins(null, null, null, null
                                          , LstZones[1][0]
                                          , null
                                          , LstZones[0][1]
                                          , LstZones[1][1]
                                          );
            LstZones[LargeurChampMine - 1][0].assignerVoisins(null, null, null
                                                           , LstZones[LargeurChampMine - 2][0]
                                                           , null
                                                           , LstZones[LargeurChampMine - 2][1]
                                                           , LstZones[LargeurChampMine - 1][1]
                                                           , null
                                                           );
            LstZones[0][HauteurChampMine - 1].assignerVoisins(null
                                                           , LstZones[0][HauteurChampMine - 2]
                                                           , LstZones[1][HauteurChampMine - 2]
                                                           , null
                                                           , LstZones[1][HauteurChampMine - 1]
                                                           , null, null, null
                                                           );
            LstZones[LargeurChampMine - 1][HauteurChampMine - 1].assignerVoisins(LstZones[LargeurChampMine - 2][HauteurChampMine - 2]
                                                                            , LstZones[LargeurChampMine - 1][HauteurChampMine - 2]
                                                                            , null
                                                                            , LstZones[LargeurChampMine - 2][HauteurChampMine - 1]
                                                                            , null, null, null, null
                                                                            );

            // Faire la rangée du haut (moins les coins).
            for (int i = 1; i < LargeurChampMine - 1; i++)
            {
                LstZones[i][0].assignerVoisins(null
                                              , null
                                              , null
                                              , LstZones[i - 1][0]
                                              , LstZones[i + 1][0]
                                              , LstZones[i - 1][1]
                                              , LstZones[i][1]
                                              , LstZones[i + 1][1]
                                              );
            }

            // Faire la rangée du bas (moins les coins).
            for (int i = 1; i < LargeurChampMine - 1; i++)
            {
                LstZones[i][HauteurChampMine - 1].assignerVoisins(LstZones[i - 1][HauteurChampMine - 2]
                                                                , LstZones[i][HauteurChampMine - 2]
                                                                , LstZones[i + 1][HauteurChampMine - 2]
                                                                , LstZones[i - 1][HauteurChampMine - 1]
                                                                , LstZones[i + 1][HauteurChampMine - 1]
                                                                , null
                                                                , null
                                                                , null
                                                                );
            }

            // Faire la rangée de gauche (moins les coins).
            for (int j = 1; j < HauteurChampMine - 1; j++)
            {
                LstZones[0][j].assignerVoisins(null
                                              , LstZones[0][j - 1]
                                              , LstZones[1][j - 1]
                                              , null
                                              , LstZones[1][j]
                                              , null
                                              , LstZones[0][j + 1]
                                              , LstZones[1][j + 1]
                                              );
            }

            // Faire la rangée de droite (moins les coins).
            for (int j = 1; j < HauteurChampMine - 1; j++)
            {
                LstZones[LargeurChampMine - 1][j].assignerVoisins(LstZones[LargeurChampMine - 2][j - 1]
                                                               , LstZones[LargeurChampMine - 1][j - 1]
                                                               , null
                                                               , LstZones[LargeurChampMine - 2][j]
                                                               , null
                                                               , LstZones[LargeurChampMine - 2][j + 1]
                                                               , LstZones[LargeurChampMine - 1][j + 1]
                                                               , null
                                                               );
            }

            // Faire les cases du "milieu", s'il y en a.
            for (int i = 1; i < LargeurChampMine - 1; i++)
            {
                for (int j = 1; j < HauteurChampMine - 1; j++)
                {
                    LstZones[i][j].assignerVoisins(LstZones[i - 1][j - 1]
                                                  , LstZones[i][j - 1]
                                                  , LstZones[i + 1][j - 1]
                                                  , LstZones[i - 1][j]
                                                  , LstZones[i + 1][j]
                                                  , LstZones[i - 1][j + 1]
                                                  , LstZones[i][j + 1]
                                                  , LstZones[i + 1][j + 1]
                                                  );
                }
            }
        }

        /// <summary>
        /// Demande à chaque zone de mettre à jour son compteur indiquant le nombre de mines dans les zones voisines.
        /// </summary>
        private void assignerCompteurs()
        {
            foreach (List<Zone> lz in LstZones)
            {
                foreach (Zone z in lz)
                {
                    z.assignerCompteur();
                }   
            }
        }

        #endregion
    }
}
