using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demineur
{
    public static class ParcoursZone
    {
        public static List<Zone> ObtenirCaseVidePropager(Zone lz)
        {
            return DFS(lz);
        }


        // Algorithme de Depth first search sans cible
        private static List<Zone> DFS(Zone entrer)
        {
            //Dictionary<Zone, EtatVisite> etatVisite = new Dictionary<Zone, EtatVisite>();
            List<Zone> reponse = new List<Zone>();
            Stack<Zone> recherche = new Stack<Zone>();
            recherche.Push(entrer);
            reponse.Add(entrer);
            while (recherche.Count() != 0)
            {
                Zone courant = recherche.Pop();
                //etatVisite[courant] = EtatVisite.visited;
                if (courant.NbrMinesVoisins != 0 || courant.ContientMine == true)
                {
                    continue;

                }
                for (int i = 0; i < 8; i++)
                {
                    if (courant.LstVoisins[i] != null && !reponse.Contains(courant.LstVoisins[i]))
                    {
                        recherche.Push(courant.LstVoisins[i]);
                        reponse.Add(courant.LstVoisins[i]);
                    }
                }


            }
            return reponse;
        }
    }
}
