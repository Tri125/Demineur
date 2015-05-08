using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demineur
{
    //  EventArgs utilisé pour envoyer l'information à un évênement si un drapeau a été rajouté ou retiré.
    public class DrapeauEventArgs : EventArgs
    {
        private DrapeauEventArgs() { }

        public static readonly DrapeauEventArgs Rajout = new DrapeauEventArgs();
        public static readonly DrapeauEventArgs Retrait = new DrapeauEventArgs();
    }
}
