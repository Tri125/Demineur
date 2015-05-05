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
    /// Interaction logic for UCIndicateurMine.xaml
    /// </summary>
    public partial class UCIndicateurMine : UserControl
    {

        private int Compteur { get; set; }

        public UCIndicateurMine()
        {
            InitializeComponent();
        }

        public UCIndicateurMine(int nbrMines)
            : this()
        {
            Compteur = nbrMines;
            lblNbrMines.Content = Compteur;
        }

        public void SetMineCount(int i)
        {
            Compteur = i;
            lblNbrMines.Content = Compteur;
            elliCentre.Fill = Brushes.Transparent;
        }

        public void DecrementeMine()
        {
            if (Compteur > 0)
                lblNbrMines.Content = --Compteur;
            else
            {
                elliCentre.Fill = Brushes.Red;
                Compteur--;
            }
        }

        public void IncrementeMine()
        {
            if (Compteur == -1)
            {
                elliCentre.Fill = Brushes.Transparent;
            }
            if (Compteur >= 0)
                lblNbrMines.Content = ++Compteur;
            else
                Compteur++;
        }
    }
}
