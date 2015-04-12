using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Demineur
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ConstructeurOption config = new ConstructeurOption();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            config.Initialisation();
        }

    }
}
