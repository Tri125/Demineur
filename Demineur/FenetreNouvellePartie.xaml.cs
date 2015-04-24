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
using System.Windows.Shapes;

namespace Demineur
{
    /// <summary>
    /// Interaction logic for FenetreNouvellePartie.xaml
    /// </summary>
    public partial class FenetreNouvellePartie : Window
    {
        public FenetreNouvellePartie()
        {
            InitializeComponent();
        }

        // http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
        private void txtNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidNumericChars(e.Text);
        }

        // http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
        private bool AreAllValidNumericChars(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsNumber(c)) return false;
            }

            return true;
        }
    }
}
