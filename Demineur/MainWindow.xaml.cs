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
		}

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FenetreConfiguration fenConfig = new FenetreConfiguration();
            fenConfig.ShowDialog();
        }
	}
}
