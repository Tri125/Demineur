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

		public ConfigJoueur() { }

		public ConfigJoueur(bool minesCoins, int tailleCases)
		{
			MinesCoins = minesCoins;
			TailleCases = tailleCases;
		}
	}
}
