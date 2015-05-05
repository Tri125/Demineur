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
