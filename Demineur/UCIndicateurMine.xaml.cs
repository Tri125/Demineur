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
    /// User Control pour indiquer le nombre de mines restants - le nombre de drapeaux.
    /// </summary>
    public partial class UCIndicateurMine : UserControl
    {

        private int Compteur { get; set; }

        public UCIndicateurMine()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nbrMines">Le nombre de mines dans le jeu</param>
        public UCIndicateurMine(int nbrMines)
            : this()
        {
            Compteur = nbrMines;
            lblNbrMines.Content = Compteur;
        }

        //  Idéalement, ce code serait dans un constructeur, mais seulement le constructeur par défaut 
        //  est utilisé lorsque le contrôle est rajouté par le xaml.
        /// <summary>
        /// Initialize le nombre de mine et les indicateurs.
        /// </summary>
        /// <param name="i"></param>
        public void SetMineCount(int i)
        {
            Compteur = i;
            lblNbrMines.Content = Compteur;
            elliCentre.Fill = Brushes.Transparent;
        }

        //  Appelé lorsqu'un drapeau est rajouté.
        /// <summary>
        /// Réduit le compteur de 1.
        /// </summary>
        public void DecrementeMine()
        {
            //  Pour ne pas afficher dans le négatif
            if (Compteur > 0)
                lblNbrMines.Content = --Compteur;
            else
            {
                // Si Compteur est négatif, alors le nombre de drapeau dépasse le nombre de mines
                // et le centre devient rouge.
                elliCentre.Fill = Brushes.Red;
                Compteur--;
            }
        }

        // Appelé lorsqu'un drapeau est retiré
        /// <summary>
        /// Augmente le compteur de 1
        /// </summary>
        public void IncrementeMine()
        {
            // Alors le Compteur est sur le point de retourner positif, il y aura autant de drapeaux que de mines.
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
