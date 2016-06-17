using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OutilAcquittementConsole
{
    class OutilAcquittement
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Outil de rapatriement des acquittements en erreur ---");

            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Voullez vous quitter l'application ? [Q] :");
                string resp = Console.ReadLine();
                if (resp.ToLower().StartsWith("q")) break;
                Traitement();
            }

            Environment.Exit(0);
        }

        /// <summary>
        /// traitement en boucle jusqu'a ce que l'utilisateur quitte la console 
        /// </summary>
        static void Traitement()
        {
            //-- 
            RepertoireTraitementLocal();

            //-- construction de la liste de pattern
            List<string> listeDatesFichiersRapatrier = CreationListePatternDatesFichierRapatrier();

            //-- telechargement des fichier
            RappatriementFichiers(listeDatesFichiersRapatrier);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeFichiersRapatrier"></param>
        private static void RappatriementFichiers(List<string> listeDatesFichiersRapatrier)
        {
            Console.Write("Séléctionnez le domaine de recherche (Indiv., coll, les deux) ? [i|c] :");
            string resp = Console.ReadLine();

            if (resp.ToLower().StartsWith("i"))
                RappatriementFichiers(listeDatesFichiersRapatrier, "REP_INDIVIDUEL");
            else if (resp.ToLower().StartsWith("c"))
                RappatriementFichiers(listeDatesFichiersRapatrier, "REP_COLLECTIF");
            else
            {
                RappatriementFichiers(listeDatesFichiersRapatrier, "REP_INDIVIDUEL");
                RappatriementFichiers(listeDatesFichiersRapatrier, "REP_COLLECTIF");
            }
            Console.WriteLine("--- Fin du rapatriement ---");
        }

        /// <summary>
        /// Rapatriement des fichiers
        /// </summary>
        /// <param name="listeFichiersRapatrier"></param>
        private static void RappatriementFichiers(List<string> listeDatesFichiersRapatrier, string domaine)
        {
            //-- config repertoire distant
            Hashtable sectionRepertoires = (Hashtable)ConfigurationManager.GetSection("RepertoiresDistants");
            string repertoireRemote = sectionRepertoires[domaine].ToString();


            //-- config repertoire rapatriement
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireLocal = sectionRepTraitements["REP_ACQUITTEMENT"].ToString();


            Hashtable sectionFiltrage = (Hashtable)ConfigurationManager.GetSection("CriteresFiltrageFichier");
            string patternFichier = sectionFiltrage["FICHIER_PATTERN_REGEX"].ToString();

            //-- config serveurs SFTP            
            ListeServeurFtpSection section = ConfigurationManager.GetSection("ListeServeurFtpSection") as ListeServeurFtpSection;
            //-- Pour chacun des serveur nous rapatrions les fichiers correspondants
            foreach (ServeurFtpElement serveur in section.ServeursFtp)
            {
                Console.WriteLine("Connexion au serveur : " + serveur.HostName);
                GestionTelechargement.SshTranfertFichier.TransfertTestAttribut(
                    serveur.HostName, 
                    serveur.Login, 
                    serveur.PassWord, 
                    repertoireLocal, 
                    repertoireRemote, 
                    patternFichier,
                    listeDatesFichiersRapatrier);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private static void RepertoireTraitementLocal()
        {
            //--------------------------
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repAcquittement = sectionRepTraitements["REP_ACQUITTEMENT"].ToString();
 
            if (!Directory.Exists(repAcquittement))
            {
                Console.WriteLine("Création du repertoire de recup. des acquitements en erreur : " + repAcquittement);
                Directory.CreateDirectory(repAcquittement);
            }
            else
            {
                Console.Write("Voulez vous vider le répertoire ? [Oui|Non]: ");
                string resp = Console.ReadLine();
                if (resp.ToLower().Equals("") || resp.ToLower().StartsWith("o"))
                {
                    DirectoryInfo di = new DirectoryInfo(repAcquittement);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    Console.WriteLine("Répertoire " + repAcquittement + " vidé.");
                }

                Console.WriteLine();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<string> CreationListePatternDatesFichierRapatrier()
        {
            List<DateTime> listeDates = GetCriteresFiltrage();
            List<string> listeDateStr = new List<string>();
           
            foreach (DateTime dat in listeDates)
            {
                int jour = dat.Day;
                int mois = dat.Month;
                int annee = dat.Year;

                //string patternFichiers = prefix + String.Format("{0:0000}", annee) + String.Format("{0:00}", mois) + String.Format("{0:00}", jour) + suffix;
                string patternDates = String.Format("{0:00}", jour) + "/" + String.Format("{0:00}", mois) + "/" + String.Format("{0:0000}", annee);

                //Console.WriteLine("Rapatriement des fichier : " + patternDates);
                listeDateStr.Add(patternDates);
            }

            return listeDateStr;
        }



        /// <summary>
        /// criteres de filtrage 
        /// date de début et profondeur
        /// </summary>
        /// <returns></returns>
        private static List<DateTime> GetCriteresFiltrage()
        {

            Hashtable sectionFiltrage = (Hashtable)ConfigurationManager.GetSection("CriteresFiltrageFichier");
            int nbJourMax = Convert.ToInt32(sectionFiltrage["NB_JOUR_MAX"]);
            List<DateTime> listeDates = new List<DateTime>();

            string dateStr = "";
            string sPattern = "^(?<jour>\\d{2})/(?<mois>\\d{2})/(?<annee>\\d{4})$";

            while (true)
            {
                Console.Write("Date de début de recherche ? ['jj/mm/aaaa'] : ");
                dateStr = Console.ReadLine();

                if (dateStr.ToLower().Equals("")) break;
                if (Regex.IsMatch(dateStr, sPattern)) break;
                Console.Write("Maivaise réponse, ");
            }

            //DateTime dateDebut = DateTime.Now;
            DateTime dateDebut = DateTime.Now.AddDays(-3);

            if (!dateStr.Equals(""))
            {
                Regex expression = new Regex(sPattern);
                var results = expression.Matches(dateStr);
                string jour = null, mois = null, annee = null;
                foreach (Match match in results)
                {
                    jour = match.Groups["jour"].Value;
                    mois = match.Groups["mois"].Value;
                    annee = match.Groups["annee"].Value;
                    //Console.WriteLine(jour +"-"+ mois +"-"+ annee);
                }
                dateDebut = new DateTime(Convert.ToInt32(annee), Convert.ToInt32(mois), Convert.ToInt32(jour));
            }
            else
            {
                int jour = dateDebut.Day;
                int mois = dateDebut.Month;
                int annee = dateDebut.Year;
                Console.WriteLine("Utilisation de la date : " + jour + "/" + mois + "/" + annee);
            }

            //-------------------
            string nbJourStr = "";
            int nbJour = 0;
            while (true)
            {
                Console.Write("Nombre jour de recherche depuis la date de début ? [0-" + nbJourMax + "] : ");
                nbJourStr = Console.ReadLine();
                if (Regex.IsMatch(nbJourStr, "^\\d{1,5}$"))
                {
                    nbJour = Convert.ToInt32(nbJourStr);
                    if (nbJour >= 0 && nbJour <= nbJourMax)
                        break;// doit etre pisitif et ne depasse pas le max 
                }
                Console.Write("Maivaise réponse, ");
            }


            listeDates.Add(dateDebut);
            if (nbJour != 0)
            {
                for (int i = 1; i < nbJour; i++)
                {
                    var dueDatePlus1Jour = dateDebut.AddDays(i);
                    listeDates.Add(dueDatePlus1Jour);
                }
            }

            return listeDates;
        }

    }


}
