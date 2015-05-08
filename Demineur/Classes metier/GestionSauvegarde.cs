using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Demineur
{
    public class GestionSauvegarde
    {
        private XmlTextReader lecteur = null;
        private XmlTextWriter ecriveur = null;
        private XmlSerializer serializer = null;
        public bool ChargementReussis { get; private set; }

        public MemoirePartie Memoire { get; private set; }

        public GestionSauvegarde()
        {
            serializer = new XmlSerializer(typeof(MemoirePartie));
        }

        public void EnregistreMemoire(string nom, MemoirePartie mem)
        {
            EcritureOption(nom, mem);
        }

        public void LectureMemoire(string nom)
        {
            Memoire = LectureFichierMemoire(nom);
        }

        /// <summary>
        /// Écriture d'un fichier XML à partir d'un objet MemoirePartie.
        /// </summary>
        /// <param name="FichierSortie">Nom du fichier de sortie.</param>
        /// <param name="mem">Objet qui sera écrit transformé en fichier XML.</param>
        private void EcritureOption(string FichierSortie, MemoirePartie mem)
        {
            try
            {
                ecriveur = new XmlTextWriter(FichierSortie, null);
                ecriveur.WriteStartDocument();
                serializer.Serialize(ecriveur, mem);
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
                    ecriveur.Dispose();
                }
            }

        }



        /// <summary>
        /// Lecture d'un fichier XML.
        /// </summary>
        /// <param name="FichierEntre">Nom du fichier.</param>
        /// <param name="mem">Objet qui sera chargé.</param>
        private MemoirePartie LectureFichierMemoire(string FichierEntre)
        {
            MemoirePartie mem;
            try
            {
                lecteur = new XmlTextReader(FichierEntre);
                mem = (MemoirePartie)serializer.Deserialize(lecteur);
                ChargementReussis = true;
            }


            catch (Exception ex)
            {
                ChargementReussis = false;
                Console.WriteLine(ex.Message);
                return null;
            }

            finally
            {
                if (lecteur != null)
                {
                    lecteur.Close();
                    lecteur.Dispose();
                }
            }
            return mem;
        }
    }
}
