using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Demineur
{
    /// <summary>
    /// Classe serializable des options de l'utilisateur
    /// </summary>
    [Serializable]
    public class ConfigJoueur
    {
        [XmlElement("Mines_de_coins")]
        public bool MinesCoins { get; set; }

        [XmlElement("Taille_des_cases")]
        public int TailleCases { get; set; }

        [XmlElement("Nombre_de_mines")]
        public int NombresMines { get; set; }

        [XmlElement("Hauteur_du_jeu")]
        public int Hauteur { get; set; }

        [XmlElement("Largeur_du_jeu")]
        public int Largeur { get; set; }

        public ConfigJoueur() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minesCoins">Faux pour ne pas générer des mines dans les coins</param>
        /// <param name="tailleCases">Taille visuel des cases de la grille du jeu</param>
        /// <param name="nbrMines">Nombre de mines</param>
        /// <param name="hauteur">Hauteur de la grille du jeu</param>
        /// <param name="largeur">Largeur de la grille du jeu</param>
        public ConfigJoueur(bool minesCoins, int tailleCases, int nbrMines, int hauteur, int largeur)
        {
            MinesCoins = minesCoins;
            TailleCases = tailleCases;
            NombresMines = nbrMines;
            Hauteur = hauteur;
            Largeur = largeur;
        }
    }
}
