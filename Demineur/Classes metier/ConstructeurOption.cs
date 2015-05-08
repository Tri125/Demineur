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
    /// <summary>
    /// La classe ConstructeurOption gère la génération des fichiers XML pour enregistrer les paramètres de jeu de l'utilisateur, pour charger
    /// les fichiers de configuration déjà présent, détecter les erreurs et effectuer les remplacements néccéssaires.
    /// </summary>
    public class ConstructeurOption
    {
        #region Attribut
        private bool mauvaisUtilisateur;
        private bool presentUtilisateur;
        private XmlTextReader lecteur = null;
        private XmlTextWriter ecriveur = null;
        private XmlSerializer serializer = null;
        #endregion
        //  Valeur interne des options de configuration pour la génération du fichier de départ
        //  ou la réparation. Le classe fallback automatiquement sur les variables internes si la réparation échoue
        //  plutôt que de continuer à essayer de lire des fichiers.
        #region Option Usine
        private ConfigJoueur optionUsine;
        private const string nomUtilisateur = "utilisateurConfig.xml";

        private const bool MINES_DE_COINS = true;
        private const int TAILLE_DES_CASES = 20;
        private const int HAUTEUR = 5;
        private const int LARGEUR = 5;
        private const int NOMBRE_DE_MINES = 4;
        #endregion

        public bool ChargementReussis { get; private set; }
        public ConfigJoueur OptionUtilisateur { get; private set; }

        /// <summary>
        /// Constructeur de base de ConstructeurOption.
        /// </summary>
        public ConstructeurOption()
        {
            OptionUtilisateur = new ConfigJoueur();
            optionUsine = new ConfigJoueur();
            serializer = new XmlSerializer(typeof(ConfigJoueur));
            //On charge l'objet Option d'usine qui contient les paramètres d'usine du jeu.
            OptionUsine();
        }

        /// <summary>
        /// Charge les paramètres d'usine dans un objet Option.
        /// Les paramètres d'usine sont utilisés lors de la réparation et de la création du fichiers XML des paramètres de jeu.
        /// </summary>
        private void OptionUsine()
        {
            this.optionUsine.MinesCoins = MINES_DE_COINS;
            this.optionUsine.TailleCases = TAILLE_DES_CASES;
            this.optionUsine.Hauteur = HAUTEUR;
            this.optionUsine.Largeur = LARGEUR;
            this.optionUsine.NombresMines = NOMBRE_DE_MINES;
        }


        /// <summary>
        /// Initialise le chargement et le teste des fichiers de configuration.
        /// Vérifie si les fichiers ont été marqués comme étant défectueux.
        /// Remplace les mauvais fichiers.
        /// </summary>
        public void Initialisation()
        {
            ChercheFichierConfig();
            TesterFichier();
            Console.WriteLine("Initialisation des fichiers de configuration");
            Console.WriteLine("Fichier utilisateur trouvé : " + presentUtilisateur);
            Console.WriteLine("Fichier utilisateur mal chargé : " + mauvaisUtilisateur);
            if (mauvaisUtilisateur)
            {
                Reparation();
                ChercheFichierConfig();
                TesterFichier();
                Console.WriteLine("RÉPARATION");
                Console.WriteLine("Fichier utilisateur trouvé : " + presentUtilisateur);
                Console.WriteLine("Fichier utilisateur mal chargé : " + mauvaisUtilisateur);
            }
            Console.WriteLine("Fin Initialisation");

            //Si, malgré la réparation, un des fichiers est défectueux, alors on charge l'objet Option avec les paramètres d'usine.
            if (mauvaisUtilisateur)
            {
                Console.WriteLine("Incapable de réparer : impossible d'écrire sur le disque");
                RetourUsine(OptionUtilisateur);

            }
        }

        /// <summary>
        /// ChercheFichierConfig recherche les fichiers de configuration de l'utilisateur
        /// d'après le nom du fichier. Un attribut bool signale si oui ou non le fichier est présent.
        /// </summary>
        private void ChercheFichierConfig()
        {
            List<string> fichiers = new List<string>();
            //  Fait une liste de tout les fichiers du dossier
            foreach (String path in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                fichiers.Add(System.IO.Path.GetFileName(path));
            }
            //  Vérifie qu'au moins un des fichiers est selui que l'on recherche
            foreach (string fichier in fichiers)
            {
                if (fichier == nomUtilisateur)
                {
                    presentUtilisateur = true;
                }
            }
        }


        /// <summary>
        /// TesterFichier teste les fichiers de configuration s'ils ont été signalés comme étant présent.
        /// Si la lecture provoque une erreur, le fichier est marqué.
        /// </summary>
        private void TesterFichier()
        {

            if (presentUtilisateur)
            {
                OptionUtilisateur = LectureOption(nomUtilisateur);
                if (ChargementReussis == false)
                {
                    mauvaisUtilisateur = true;
                }
            }
            else if (!presentUtilisateur)
            {
                mauvaisUtilisateur = true;
            }
        }

        /// <summary>
        /// Reparation remplace l'objet Option et le fichier XML approprié par les paramètres d'usine.
        /// Vérifie que la réparation c'est bien effectuée.
        /// </summary>
        private void Reparation()
        {
            OptionUtilisateur = new ConfigJoueur();
            EcritureOption(nomUtilisateur, optionUsine);
            mauvaisUtilisateur = false;
            presentUtilisateur = false;
        }


        /// <summary>
        /// Lecture d'un fichier XML.
        /// </summary>
        /// <param name="FichierEntre">Nom du fichier.</param>
        /// <param name="option">Objet Option où les paramètres seront enregistrés.</param>
        private ConfigJoueur LectureOption(string FichierEntre)
        {
            ConfigJoueur config;
            try
            {
                lecteur = new XmlTextReader(FichierEntre);
                config = (ConfigJoueur)serializer.Deserialize(lecteur);
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
            return config;
        }

        /// <summary>
        /// Écriture d'un fichier XML à partir d'un objet Option.
        /// </summary>
        /// <param name="FichierSortie">Nom du fichier de sortie.</param>
        /// <param name="option">Objet Option qui sera écrit transformé en fichier XML.</param>
        private void EcritureOption(string FichierSortie, ConfigJoueur option)
        {
            try
            {
                ecriveur = new XmlTextWriter(FichierSortie, null);
                ecriveur.WriteStartDocument();
                serializer.Serialize(ecriveur, option);
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
        /// Enregistre un nouveau fichier de configuration de l'utilisateur à partir d'un objet Option et lance son chargement.
        /// </summary>
        public void EnregistrementUtilisateur(ConfigJoueur option)
        {
            EcritureOption(nomUtilisateur, option);
            Initialisation();
        }

        /// <summary>
        /// Ré-écriture de l'objet d'option de l'utilisateur actuellement chargé
        /// Utile si les configurations sont changé lors de l'exécution et qu'il faudrait mettre à jour le fichier.
        /// </summary>
        public void EnregistreConfigCourante()
        {
            EnregistrementUtilisateur(OptionUtilisateur);
        }

        /// <summary>
        /// Change les options utilisés vers les paramètres de d'usine.
        /// </summary>
        private void RetourUsine(ConfigJoueur option)
        {
            option = optionUsine;
        }

    }
}
