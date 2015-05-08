using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Windows;

namespace Demineur
{
    /// <summary>
    /// Classe serializable des actions d'un joueur au cours d'une partie.
    /// </summary>
    [Serializable]
    public class MemoirePartie
    {
        [XmlElement("ActionClique")]
        public List<Point> CliqueGauche { get; set; }

        [XmlElement("ActionDrapeau")]
        public List<Point> CliqueDroit { get; set; }

        [XmlElement("ConfigurationPartie")]
        public ConfigJoueur Configuration { get; set; }

        [XmlElement("Seed")]
        public int RandomSeed { get; set; }

        public MemoirePartie() { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cliqueGauche">Les cliques gauche/double clique ayant dévoilé une case</param>
        /// <param name="cliqueDroit">La localisation des drapeaux</param>
        public MemoirePartie(List<Point> cliqueGauche, List<Point> cliqueDroit, ConfigJoueur config, int seed)
        {
            this.CliqueGauche = cliqueGauche;
            this.CliqueDroit = cliqueDroit;
            this.Configuration = config;
            this.RandomSeed = seed;
        }
    }
}
