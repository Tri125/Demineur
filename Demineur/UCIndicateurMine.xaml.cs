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
        public UCIndicateurMine(int nbrMines)
        {
            InitializeComponent();
            lblNbrMines.Content = nbrMines;
        }

        public void DecrementeMine()
        {
            int tmp;
            bool result;
            result = int.TryParse(lblNbrMines.Content.ToString(), out tmp);
            if (result && tmp > 0)
            {
                lblNbrMines.Content = --tmp;
            }
        }

        public void IncrementeMine()
        {
            int tmp;
            bool result;
            result = int.TryParse(lblNbrMines.Content.ToString(), out tmp);
            if (result)
            {
                lblNbrMines.Content = ++tmp;
            }
        }
    }
}
