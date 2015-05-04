﻿using System;
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

        public UCIndicateurMine()
        {
            InitializeComponent();
        }

        public UCIndicateurMine(int nbrMines)
            : this()
        {
            lblNbrMines.Content = nbrMines;
        }

        public void SetMineCount(int i)
        {
            lblNbrMines.Content = i;
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
            else if (!result)
                lblNbrMines.Content = 0;
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
            else
                lblNbrMines.Content = 0;
        }
    }
}
