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
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace Demineur
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			FenetreChampMines fenetreJeu = new FenetreChampMines(5, 5, 4);
			gridPrincipale.Children.Add(fenetreJeu);

			XmlTextWriter ecriveur = null;
			XmlSerializer serializer = new XmlSerializer(typeof(ConfigJoueur));

			ConfigJoueur c = new ConfigJoueur(true, 15);

			try
			{
				ecriveur = new XmlTextWriter("yolo", null);
				ecriveur.WriteStartDocument();
				serializer.Serialize(ecriveur, c);
				ecriveur.WriteEndDocument();
			}

			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}

			finally
			{
				if (ecriveur != null)
				{
					ecriveur.Close();
				}
			}



		}
	}
}
