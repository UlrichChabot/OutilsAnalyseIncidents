using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace OutilRapatriementConsole
{
    class OutilRapatriement
    {

        static void Main(string[] args)
        {
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;

                List<string> listeFichiersRapatrier = null;
                List<string> listeNoOffreRecherche = null;
                List<string> listeContratCcnRecherche = null;

                Console.WriteLine();
                Console.WriteLine("Options dinponibles : ");
                Console.WriteLine("[a] : Rapatriement des fichiers archives, ");
                Console.WriteLine("[b] : Desarchivage des fichiers, ");
                Console.WriteLine("[c] : Analyse par référence dossier (no offre), ");
                Console.WriteLine("[d] : Analyse par numéro de contrat CCN, ");
                Console.WriteLine("[e] : Recherche de dossier sans BIC/IBAN, ");
                Console.WriteLine("[f] : Recherche de dossier sans SIREN, ");
                Console.WriteLine("[g] : Recherche de dossier avec no contrat CCN à blanc, ");

                Console.WriteLine("[abc] : Rapatriement + unzip + analyse par référence dossier, ");
                Console.WriteLine("[abd] : Rapatriement + unzip + analyse par numéro de contrat CCN, ");
                Console.WriteLine("[abe] : Rapatriement + unzip + recherche de dossier sans BIC/IBAN, ");
                Console.WriteLine("[abf] : Rapatriement + unzip + recherche de dossier sans SIREN, ");

                Console.WriteLine("[Q] : Quitter l'application.");
                Console.WriteLine();
                Console.Write("Que voulez vous lancer ? :");


                /*-----------------------*/
                /*--      QUESTIONS    --*/
                /*-----------------------*/

                string resp = Console.ReadLine();
                if (resp.ToLower().StartsWith("q")) break;

                if (resp.ToLower().Contains("a"))
                {
                    listeFichiersRapatrier = CreationListePatternFichierRapatrier();
                    RepertoireRapatriementLocal();
                }
                if (resp.ToLower().Contains("b"))
                {
                    RepertoireExtractionLocal();
                }
                if (resp.ToLower().Contains("c"))
                {
                    RepertoireResultatLocal();
                    listeNoOffreRecherche = CreationListeNoOffreRecherche();
                }
                if (resp.ToLower().Contains("d"))
                {
                    RepertoireResultatLocal();
                    listeContratCcnRecherche = CreationListeContratCcnRecherche();
                }
                if (resp.ToLower().Contains("e"))
                {
                    RepertoireResultatLocal();
                }
                if (resp.ToLower().Contains("f"))
                {
                    RepertoireResultatLocal();
                }
                if (resp.ToLower().Contains("g"))
                {
                    RepertoireResultatLocal();
                }


                if (resp.ToLower().Contains("z"))
                {
                    RepertoireResultatLocal();
                }

                /* ***************************************** */

                if (resp.ToLower().Contains("a"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("**** Rapatriement des fichiers archives ****");
                    //-- telechargement des fichier
                    RappatriementFichiers(listeFichiersRapatrier);
                }
                if (resp.ToLower().Contains("b"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("**** Desarchivage des fichiers ****");
                    ExtractionFichers();
                }
                if (resp.ToLower().Contains("c"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("**** Analyse par référence dossier ****");
                    AnalyseXmlSelonNoOffre(listeNoOffreRecherche);
                }
                if (resp.ToLower().Contains("d"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("**** Analyse par numéro de contrat CCN ****");
                    AnalyseXmlSelonContratCcn(listeContratCcnRecherche);
                }
                if (resp.ToLower().Contains("e"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("**** Recherche de dossier sans BIC/IBAN ****");
                    AnalyseXmlSelonIban();
                }
                if (resp.ToLower().Contains("f"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("**** Recherche de dossier sans SIREN ****");
                    AnalyseXmlSelonSiren();
                }
                if (resp.ToLower().Contains("g"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("**** Recherche de dossier avec no contrat CCN à blanc ****");
                    AnalyseXmlSelonContratCcnVide();
                }

/*
                if (resp.ToLower().Contains("x"))
                {
                    List<string> liste = new List<string>() { "corinne.compagnon@mbamutuelle.com" } ;
                    AnalyseXmlSelonEmail(liste);
                }
*/
                if (resp.ToLower().Contains("z"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("**** Analyse de masse ****");
                    TraitementDossiersMasse();
                    //TraitementAnalyseNomsFichiers();
                }

                if (resp.ToLower().Contains("@"))
                {
                    //TraitementAnalyseNomsFichiers();
                    TraitementAnalyseNumContratCCN();
                }


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
            List<string> listeFichiersRapatrier = CreationListePatternFichierRapatrier();

            /*------------*/

            List<string> listeNoOffreRecherche = new List<string>();
            while (true)
            {
                string noOffreRecherche = "";
                Console.WriteLine("Entrez le numero d'offre recherché ? (3 car. min) : ");
                noOffreRecherche = Console.ReadLine();

                if (noOffreRecherche.Equals(""))
                {
                    break;
                }
                if (noOffreRecherche.Length < 3)
                {
                    Console.WriteLine("Taille du numero d'offre trop petit (min 3 caractères) !");
                    continue;
                }
                if (noOffreRecherche.Length > 11)
                {
                    Console.WriteLine("Taille du numero d'offre trop grand (max 11 caractères) !");
                    continue;
                }
                if (noOffreRecherche.Length == 11)
                {
                    if (!Regex.IsMatch(noOffreRecherche, "^[a-zA-Z]{1}\\d{10}$"))
                    {
                        Console.WriteLine("Le numero ne respecte pas le format (Z9999999999) !");
                        continue;
                    }
                }
                if (!Regex.IsMatch(noOffreRecherche, "^[a-zA-Z]{0,1}\\d{0,10}$"))
                {
                    Console.WriteLine("Maivaise réponse");
                    continue;
                }

                Console.WriteLine("Ajout du critère : " + noOffreRecherche);
                listeNoOffreRecherche.Add(noOffreRecherche.ToUpper());

            }


            //---- DEBUG -----

            Console.WriteLine("Recap des numero d'offres recherchés : ");
            foreach (string no in listeNoOffreRecherche)
            {
                Console.WriteLine("\t" + no);
            }


            /*------------*/

            //-- telechargement des fichier
            RappatriementFichiers(listeFichiersRapatrier);

            /*------------*/

            //-- config repertoire rapatriement
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireLocal = sectionRepTraitements["REP_RAPATRIEMENT"].ToString();
            string repertoireTravail = sectionRepTraitements["REP_EXTRACTION"].ToString();
            string repertoireResultat = sectionRepTraitements["REP_RESULTAT"].ToString();

            DirectoryInfo di = new DirectoryInfo(repertoireLocal);
            foreach (FileInfo file in di.GetFiles())
            {
                //-- Desarchivage des fichiers du bac a sable 
                GestionArchive.ExtractTGZ extractTGZ = new GestionArchive.ExtractTGZ();
                extractTGZ.Extract(file.FullName, repertoireTravail);
            }

            /*------------*/


            if (listeNoOffreRecherche == null || listeNoOffreRecherche.Count > 0)
            {
                int nbCorrespondance = 0;
                DirectoryInfo diWork = new DirectoryInfo(repertoireTravail);
                foreach (FileInfo file in diWork.GetFiles())
                {
                    //-- pour tous les fichiers XML 
                    if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                    {

                        if (testOffreDansXml(listeNoOffreRecherche, file, "//codeAZ"))
                        {
                            nbCorrespondance++;

                            string sourceXmlFile = file.FullName;
                            string destXmlFile = repertoireResultat + file.Name;
                            File.Copy(sourceXmlFile, destXmlFile, true);

                            Regex expression = new Regex("^(?<nom>.*)\\.xml$");
                            string nom = null;
                            var results = expression.Matches(file.Name);
                            foreach (Match match in results)
                            {
                                nom = match.Groups["nom"].Value;
                                //Console.WriteLine("");
                            }
                            string sourcePdfFile = repertoireTravail + nom + ".pdf";
                            string destPdfFile = repertoireResultat + nom + ".pdf";

                            if (File.Exists(sourcePdfFile))
                            {
                                File.Copy(sourcePdfFile, destPdfFile, true);
                            }

                        }
                    }

                }

                if (nbCorrespondance == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("--- Acune correspondance ---");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--- Il y à " + nbCorrespondance + " fichier(s) correspondant(s) aux critères ---");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }

            Console.WriteLine("<<< Fin >>>");

        }


        

        /// <summary>
        /// recherche des dossier sans SIREN pour les CCN et fichiers collectifs
        /// </summary>
        /// <param name="listeNoOffreRecherche"></param>
        static void AnalyseXmlSelonSiren()
        {
            //AnalyseXmlSelonAttribut(listeNoOffreRecherche, "//identifiantContratCCN");

            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireTravail = sectionRepTraitements["REP_EXTRACTION"].ToString();
            string repertoireResultat = sectionRepTraitements["REP_RESULTAT"].ToString();

            int nbCorrespondance = 0;
            //            using (System.IO.StreamWriter fileRes = new System.IO.StreamWriter(repertoireResultat + @"\stdout.log", true))
            //            {
            DirectoryInfo diWork = new DirectoryInfo(repertoireTravail);
            foreach (FileInfo file in diWork.GetFiles())
            {
                //-- pour tous les fichiers XML 
                //if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                if (Regex.IsMatch(file.Name, ".*\\.xml$") && Regex.IsMatch(file.Name, "^(C_)|^(CCN)") )
                {

                    ArrayList idClient = SearchXPathNavigator("//codeSIREN", file.FullName);
                    bool errone = false;
                    if (idClient.Count == 0)
                    {
                        errone = true;
                    }
                    else
                    {
                        foreach (string val in idClient)
                        {
                            if (val.Length != 9)
                            {
                                errone = true;
                                break;
                            }
                        }
                    }


                    if (errone)
                    {
                        nbCorrespondance++;

                        string sourceXmlFile = file.FullName;
                        string destXmlFile = repertoireResultat + file.Name;
                        File.Copy(sourceXmlFile, destXmlFile, true);

                        Regex expression = new Regex("^(?<nom>.*)\\.xml$");
                        string nom = null;
                        var results = expression.Matches(file.Name);
                        foreach (Match match in results)
                        {
                            nom = match.Groups["nom"].Value;
                            //Console.WriteLine("");
                        }
                        string sourcePdfFile = repertoireTravail + nom + ".pdf";
                        string destPdfFile = repertoireResultat + nom + ".pdf";

                        //-- trace 
                        //fileRes.WriteLine(file.Name + ";" + nom);

                        if (File.Exists(sourcePdfFile))
                        {
                            File.Copy(sourcePdfFile, destPdfFile, true);
                        }
                    }



                }

            }
            //}

            if (nbCorrespondance == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("--- Acune correspondance ---");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("--- Il y à " + nbCorrespondance + " fichier(s) correspondant(s) aux critères ---");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

        }

        /// <summary>
        /// recherche des dossier CCN sans contrat CCN 
        /// </summary>
        /// <param name="listeNoOffreRecherche"></param>
        static void AnalyseXmlSelonContratCcnVide()
        {

            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireTravail = sectionRepTraitements["REP_EXTRACTION"].ToString();
            string repertoireResultat = sectionRepTraitements["REP_RESULTAT"].ToString();

            int nbCorrespondance = 0;
            //            using (System.IO.StreamWriter fileRes = new System.IO.StreamWriter(repertoireResultat + @"\stdout.log", true))
            //            {
            DirectoryInfo diWork = new DirectoryInfo(repertoireTravail);
            foreach (FileInfo file in diWork.GetFiles())
            {
                //-- pour tous les fichiers XML 
                //if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                if (Regex.IsMatch(file.Name, ".*\\.xml$") && Regex.IsMatch(file.Name, "^CCN"))
                {

                    ArrayList idClient = SearchXPathNavigator("//identifiantContratCCN", file.FullName);
                    bool errone = false;
                    if (idClient.Count == 0)
                    {
                        errone = true;
                    }
                    else
                    {
                        foreach (string val in idClient)
                        {
                            if (val.Length != 7)
                            {
                                errone = true;
                                break;
                            }

                            if (!Regex.IsMatch(val, "^[0-9A-Z]{7}$"))
                            {
                                errone = true;
                                break;
                            }
                        }
                    }


                    if (errone)
                    {
                        nbCorrespondance++;

                        string sourceXmlFile = file.FullName;
                        string destXmlFile = repertoireResultat + file.Name;
                        File.Copy(sourceXmlFile, destXmlFile, true);

                        Regex expression = new Regex("^(?<nom>.*)\\.xml$");
                        string nom = null;
                        var results = expression.Matches(file.Name);
                        foreach (Match match in results)
                        {
                            nom = match.Groups["nom"].Value;
                            //Console.WriteLine("");
                        }
                        string sourcePdfFile = repertoireTravail + nom + ".pdf";
                        string destPdfFile = repertoireResultat + nom + ".pdf";

                        //-- trace 
                        //fileRes.WriteLine(file.Name + ";" + nom);

                        if (File.Exists(sourcePdfFile))
                        {
                            File.Copy(sourcePdfFile, destPdfFile, true);
                        }
                    }
                }
            }
            //}

            if (nbCorrespondance == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("--- Acune correspondance ---");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("--- Il y à " + nbCorrespondance + " fichier(s) correspondant(s) aux critères ---");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

        }


        /// <summary>
        /// recherche des dossier sans iban sauf pour les dossier CCN (sans iban)
        /// seulement en signature electronique :
        ///  <codeNatureSig>Signature électronique</codeNatureSig>
        /// seuelemnt pour VAD (*_VAD_*) :
        ///     <IdVendeur>
        ///         <idSilo>? VAD eCommerce ?</idSilo>
        ///         <codeAppli>A0171</codeAppli>
        ///     </IdVendeur>
        /// </summary>
        /// <param name="listeNoOffreRecherche"></param>
        static void AnalyseXmlSelonIban()
        {
            //AnalyseXmlSelonAttribut(listeNoOffreRecherche, "//identifiantContratCCN");

            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireTravail = sectionRepTraitements["REP_EXTRACTION"].ToString();
            string repertoireResultat = sectionRepTraitements["REP_RESULTAT"].ToString();

            int nbCorrespondance = 0;
//            using (System.IO.StreamWriter fileRes = new System.IO.StreamWriter(repertoireResultat + @"\stdout.log", true))
//            {
                DirectoryInfo diWork = new DirectoryInfo(repertoireTravail);
                foreach (FileInfo file in diWork.GetFiles())
                {
                    //-- pour tous les fichiers XML 
                    //if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                    if (Regex.IsMatch(file.Name, ".*\\.xml$") && !Regex.IsMatch(file.Name, "^CCN") && Regex.IsMatch(file.Name, "_VAD_") )
                    {
                        ArrayList idClient = SearchXPathNavigator("//codeIBAN", file.FullName);

                        bool errone = false;
                        if (idClient.Count == 0)
                        {
                            errone = true;
                        }
                        else
                        {
                            foreach (string val in idClient)
                            {
                                if (val.Length < 4)
                                {
                                    errone = true;
                                    break;
                                }
                                /*
                                if (!Regex.IsMatch(val, "^[0-9A-Z]{7}$"))
                                {
                                    errone = true;
                                    break;
                                }*/

                            }
                        }


                        if (errone)
                        {
                            ArrayList codeNatureSig = SearchXPathNavigator("//codeNatureSig", file.FullName);


                            foreach (string val in codeNatureSig)
                            {
                                /*if (val.Length < 4)
                                {
                                    errone = true;
                                    break;
                                }*/
                                
                                if (!Regex.IsMatch(val, "Signature électronique"))
                                {
                                    errone = false;
                                    break;
                                }

                            }

                        }


                        if (errone)
                        {
                            nbCorrespondance++;

                            string sourceXmlFile = file.FullName;
                            string destXmlFile = repertoireResultat + file.Name;
                            File.Copy(sourceXmlFile, destXmlFile, true);

                            Regex expression = new Regex("^(?<nom>.*)\\.xml$");
                            string nom = null;
                            var results = expression.Matches(file.Name);
                            foreach (Match match in results)
                            {
                                nom = match.Groups["nom"].Value;
                                //Console.WriteLine("");
                            }
                            string sourcePdfFile = repertoireTravail + nom + ".pdf";
                            string destPdfFile = repertoireResultat + nom + ".pdf";

                            //-- trace 
                            //fileRes.WriteLine(file.Name + ";" + nom);

                            if (File.Exists(sourcePdfFile))
                            {
                                File.Copy(sourcePdfFile, destPdfFile, true);
                            }
                        }

                    }

                }
            //}

            if (nbCorrespondance == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("--- Acune correspondance ---");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("--- Il y à " + nbCorrespondance + " fichier(s) correspondant(s) aux critères ---");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeNoOffreRecherche"></param>
        static void AnalyseXmlSelonContratCcn(List<string> listeNoOffreRecherche)
        {
            AnalyseXmlSelonAttribut(listeNoOffreRecherche, "//identifiantContratCCN");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeNoOffreRecherche"></param>
        static void AnalyseXmlSelonNoOffre(List<string> listeNoOffreRecherche)
        {
            AnalyseXmlSelonAttribut(listeNoOffreRecherche, "//codeAZ");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeNoOffreRecherche"></param>
        static void AnalyseXmlSelonEmail(List<string> listeNoOffreRecherche)
        {
            AnalyseXmlSelonAttribut(listeNoOffreRecherche, "//libEmail");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeNoOffreRecherche"></param>
        static void AnalyseXmlSelonAttribut(List<string> listeChaineRecherche, string attribut)
        {
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireTravail = sectionRepTraitements["REP_EXTRACTION"].ToString();
            string repertoireResultat = sectionRepTraitements["REP_RESULTAT"].ToString();

            if (listeChaineRecherche == null || listeChaineRecherche.Count > 0)
            {
                int nbCorrespondance = 0;
                DirectoryInfo diWork = new DirectoryInfo(repertoireTravail);
                foreach (FileInfo file in diWork.GetFiles())
                {
                    //-- pour tous les fichiers XML 
                    if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                    {

                        if (testOffreDansXml(listeChaineRecherche, file, attribut))
                        {
                            nbCorrespondance++;

                            string sourceXmlFile = file.FullName;
                            string destXmlFile = repertoireResultat + file.Name;
                            File.Copy(sourceXmlFile, destXmlFile, true);

                            Regex expression = new Regex("^(?<nom>.*)\\.xml$");
                            string nom = null;
                            var results = expression.Matches(file.Name);
                            foreach (Match match in results)
                            {
                                nom = match.Groups["nom"].Value;
                                //Console.WriteLine("");
                            }
                            string sourcePdfFile = repertoireTravail + nom + ".pdf";
                            string destPdfFile = repertoireResultat + nom + ".pdf";

                            if (File.Exists(sourcePdfFile))
                            {
                                File.Copy(sourcePdfFile, destPdfFile, true);
                            }

                        }
                    }

                }

                if (nbCorrespondance == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("--- Acune correspondance ---");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--- Il y à " + nbCorrespondance + " fichier(s) correspondant(s) aux critères ---");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }
        }


        /// <summary>
        /// traitement en boucle jusqu'a ce que l'utilisateur quitte la console 
        /// </summary>
        static void TraitementAnalyseNomsFichiers()
        {
            //-- config repertoire rapatriement
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireResultat = sectionRepTraitements["REP_RESULTAT"].ToString();

            using (System.IO.StreamWriter fileRes = new System.IO.StreamWriter(@"C:\Temp\res.txt", true))
            {
                DirectoryInfo diWork = new DirectoryInfo(repertoireResultat);
                foreach (FileInfo file in diWork.GetFiles())
                {
                    //-- pour tous les fichiers XML 
                    if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                    {                        
                        fileRes.WriteLine(file.Name);
                    }
                }
            }

            Console.WriteLine("<<< Fin >>>");

        }

        /// <summary>
        /// traitement en boucle jusqu'a ce que l'utilisateur quitte la console 
        /// </summary>
        static void TraitementAnalyseNumContratCCN()
        {
            //-- config repertoire rapatriement
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireResultat = sectionRepTraitements["REP_RESULTAT"].ToString();

            using (System.IO.StreamWriter fileRes = new System.IO.StreamWriter(@"C:\Temp\res_CCN.txt", true))
            {
                DirectoryInfo diWork = new DirectoryInfo(repertoireResultat);
                foreach (FileInfo file in diWork.GetFiles())
                {
                    //-- pour tous les fichiers XML 
                    if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                    {                        
                        ArrayList idClient = SearchXPathNavigator("//identifiantContratCCN", file.FullName);
                        //ArrayList idClient = SearchXPathNavigator("//codeIBAN", file.FullName);
                        foreach (string val in idClient)
                        {
                            fileRes.WriteLine(file.Name+";"+val);
                        }
                    }
                }
            }

            Console.WriteLine("<<< Fin >>>");

        }

        /// <summary>
        /// traitement en boucle jusqu'a ce que l'utilisateur quitte la console 
        /// </summary>
        static void TraitementNoContratAbs()
        {
            //-- 
            RepertoireTraitementLocal();

            //-- construction de la liste de pattern
            List<string> listeFichiersRapatrier = CreationListePatternFichierRapatrier();

            /*------------*/

            List<string> listeNoOffreRecherche = new List<string>();
            while (true)
            {
                string noOffreRecherche = "";
                Console.WriteLine("Entrez le numero contrat recherché ? : ");
                noOffreRecherche = Console.ReadLine();

                if (noOffreRecherche.Equals(""))
                {
                    break;
                }
                /*
                if (noOffreRecherche.Length > 11)
                { 
                    Console.WriteLine("Taille du numero d'offre trop grand (max 11 caractères) !");
                    continue;
                }
                */

                Console.WriteLine("Ajout du critère : " + noOffreRecherche);
                listeNoOffreRecherche.Add(noOffreRecherche.ToUpper());

            }

            //---- DEBUG -----
            Console.WriteLine("Recap des numero d'offres recherchés : ");
            foreach (string no in listeNoOffreRecherche)
            {
                Console.WriteLine("\t" + no);
            }

            //-- telechargement des fichier
            RappatriementFichiers(listeFichiersRapatrier);

            //-- config repertoire rapatriement
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireLocal = sectionRepTraitements["REP_RAPATRIEMENT"].ToString();
            string repertoireTravail = sectionRepTraitements["REP_EXTRACTION"].ToString();
            string repertoireResultat = sectionRepTraitements["REP_RESULTAT"].ToString();

            DirectoryInfo di = new DirectoryInfo(repertoireLocal);
            foreach (FileInfo file in di.GetFiles())
            {
                //-- Desarchivage des fichiers du bac a sable 
                GestionArchive.ExtractTGZ extractTGZ = new GestionArchive.ExtractTGZ();
                extractTGZ.Extract(file.FullName, repertoireTravail);
            }


            if (listeNoOffreRecherche == null || listeNoOffreRecherche.Count > 0)
            {
                int nbCorrespondance = 0;
                DirectoryInfo diWork = new DirectoryInfo(repertoireTravail);
                foreach (FileInfo file in diWork.GetFiles())
                {
                    //-- pour tous les fichiers XML 
                    if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                    {

                        if (testOffreDansXml(listeNoOffreRecherche, file, "//identifiantContratCCN"))
                        {
                            nbCorrespondance++;

                            string sourceXmlFile = file.FullName;
                            string destXmlFile = repertoireResultat + file.Name;
                            File.Copy(sourceXmlFile, destXmlFile, true);

                            Regex expression = new Regex("^(?<nom>.*)\\.xml$");
                            string nom = null;
                            var results = expression.Matches(file.Name);
                            foreach (Match match in results)
                            {
                                nom = match.Groups["nom"].Value;
                                //Console.WriteLine("");
                            }
                            string sourcePdfFile = repertoireTravail + nom + ".pdf";
                            string destPdfFile = repertoireResultat + nom + ".pdf";

                            if (File.Exists(sourcePdfFile))
                            {
                                File.Copy(sourcePdfFile, destPdfFile, true);
                            }

                        }
                    }

                }

                if (nbCorrespondance == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("--- Acune correspondance ---");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--- Il y à " + nbCorrespondance + " fichier(s) correspondant(s) aux critères ---");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }

            Console.WriteLine("<<< Fin >>>");

        }



        

        /// <summary>
        /// traitement en boucle jusqu'a ce que l'utilisateur quitte la console 
        /// </summary>
        static void TraitementDossiersMasse()
        {
                        
            List<string> listeNoOffreRecherche = new List<string>() 
            {
"A0855800261",
"A1530700201",
"A1530700204",
"A1607800401",
"A1668600922",
"A1717600325",
"A1723800501",
"A1873600925",
"A1910900424",
"A1992300143",
"A2008301241",
"A2028400823",
"A2035100741",
"A2037400593",
"A2037501325",
"A2048600605",
"A2048601266",
"A2056700221",
"A2061701041",
"A2087700562",
"A2186900759",
"A2187200682",
"A2187601221",
"A2215200922",
"A2223401302",
"A2227800744",
"A2229000121",
"A2234200121",
"A2234200422",
"A2236901307",
"A2237200341",
"A2238400002",
"A2238400165",
"A2238400225",
"A2249400181",
"A2254500384",
"A2255300463",
"A2262203043",
"A2262403143",
"A2268300608",
"A2268902321",
"A2269200181",
"A2270500705",
"A2271500961",
"A2274400802",
"A2274600183",
"A2278700501",
"A2290601022",
"A2293600222",
"A2293700583",
"A2294800886",
"A2295800503",
"A2300200341",
"A2307100321",
"A2308200128",
"A2314400421",
"A2318800301",
"A2318900482",
"A2318900901",
"A2320900712",
"A2321600402",
"A2322400261",
"C0000027122",
"C0000037398",
"C0000039247",
"C0000043601",
"C0000044977",
"C0000050793",
"C0000077484",
"C0000077500",
"C0000082855",
"C0000083994",
"C0000095013",
"C0000095125",
"C0000095264",
"C0000095440",
"C0000095721",
"C0000095741",
"C0000096109",
"C0000096235",
"C0000096259",
"C0000096321",
"C0000096427",
"C0000096942",
"C0000097273",
"C0000097340",
"C0000097531",
"C0000097734",
"C0000097781",
"C0000098236",
"C0000098237"
            };
            


            //-- config repertoire rapatriement
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireTravail = sectionRepTraitements["REP_EXTRACTION"].ToString();
            string repertoireResultat = sectionRepTraitements["REP_RESULTAT"].ToString();


            if (listeNoOffreRecherche == null || listeNoOffreRecherche.Count > 0)
            {
                int nbCorrespondance = 0;
                DirectoryInfo diWork = new DirectoryInfo(repertoireTravail);
                foreach (FileInfo file in diWork.GetFiles())
                {
                    //-- pour tous les fichiers XML 
                    if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                    {
                        if (testOffreDansXml(listeNoOffreRecherche, file, "//codeAZ"))
                        {
                            nbCorrespondance++;

                            string sourceXmlFile = file.FullName;
                            string destXmlFile = repertoireResultat + file.Name;
                            File.Copy(sourceXmlFile, destXmlFile, true);

                            Regex expression = new Regex("^(?<nom>.*)\\.xml$");
                            string nom = null;
                            var results = expression.Matches(file.Name);
                            foreach (Match match in results)
                            {
                                nom = match.Groups["nom"].Value;
                                //Console.WriteLine("");
                            }
                            string sourcePdfFile = repertoireTravail + nom + ".pdf";
                            string destPdfFile = repertoireResultat + nom + ".pdf";

                            if (File.Exists(sourcePdfFile))
                            {
                                File.Copy(sourcePdfFile, destPdfFile, true);
                            }

                        }
                    }

                }

                if (nbCorrespondance == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("--- Acune correspondance ---");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--- Il y à " + nbCorrespondance + " fichier(s) correspondant(s) aux critères ---");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }

            Console.WriteLine("<<< Fin >>>");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeNoOffreRecherche"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool testOffreDansXml(List<string> listeNoOffreRecherche, FileInfo file, string chaine)
        {
            ArrayList idClient = SearchXPathNavigator(chaine, file.FullName);
            //ArrayList idClient = SearchXPathNavigator("//codeAZ", file.FullName);
            //ArrayList idClient = SearchXPathNavigator("//identifiantContratCCN", file.FullName);
            //ArrayList idClient = SearchXPathNavigator("//instantCreat", file.FullName);

            foreach (string val in idClient)
                foreach (string no in listeNoOffreRecherche)
                    if (val.ToUpper().Contains(no.ToUpper()))
                        return true;

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeFichiersRapatrier"></param>
        private static void RappatriementFichiers(List<string> listeFichiersRapatrier)
        {
            Console.Write("Séléctionnez le domaine de recherche (Indiv., coll, les deux) ? [i|c] :");
            string resp = Console.ReadLine();

            if (resp.ToLower().StartsWith("i"))
                RappatriementFichiers(listeFichiersRapatrier, "REP_INDIVIDUEL");
            else if (resp.ToLower().StartsWith("c"))
                RappatriementFichiers(listeFichiersRapatrier, "REP_COLLECTIF");
            else
            {
                RappatriementFichiers(listeFichiersRapatrier, "REP_INDIVIDUEL");
                RappatriementFichiers(listeFichiersRapatrier, "REP_COLLECTIF");
            }
            Console.WriteLine("--- Fin du rapatriement ---");
        }

        /// <summary>
        /// Dezippage et désachivage des fichiers archives tgz
        /// </summary>
        private static void ExtractionFichers()
        {
            //-- config repertoire rapatriement
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireLocal = sectionRepTraitements["REP_RAPATRIEMENT"].ToString();
            string repertoireTravail = sectionRepTraitements["REP_EXTRACTION"].ToString();

            DirectoryInfo di = new DirectoryInfo(repertoireLocal);
            foreach (FileInfo file in di.GetFiles())
            {
                //-- Desarchivage des fichiers du bac a sable 
                GestionArchive.ExtractTGZ extractTGZ = new GestionArchive.ExtractTGZ();
                extractTGZ.Extract(file.FullName, repertoireTravail);
            }
        }


        /// <summary>
        /// Rapatriement des fichiers
        /// </summary>
        /// <param name="listeFichiersRapatrier"></param>
        private static void RappatriementFichiers(List<string> listeFichiersRapatrier, string domaine)
        {
            //-- config repertoire distant
            Hashtable sectionRepertoires = (Hashtable)ConfigurationManager.GetSection("RepertoiresDistants");
            string repertoireRemote = sectionRepertoires[domaine].ToString();


            //-- config repertoire rapatriement
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repertoireLocal = sectionRepTraitements["REP_RAPATRIEMENT"].ToString();

            //-- config serveurs SFTP            
            ListeServeurFtpSection section = ConfigurationManager.GetSection("ListeServeurFtpSection") as ListeServeurFtpSection;
            //-- Pour chacun des serveur nous rapatrions les fichiers correspondants
            foreach (ServeurFtpElement serveur in section.ServeursFtp)
            {
                Console.WriteLine("Connexion au serveur : " + serveur.HostName);
                bool connexOk = GestionTelechargement.SshTranfertFichier.TestConnexionServeur(serveur.HostName, serveur.Login, serveur.PassWord);
                if (connexOk)
                {
                    foreach (string pattern in listeFichiersRapatrier) {
                        string remotefichier = repertoireRemote + pattern;
                        Console.WriteLine("Tantative de rapatriement de " + remotefichier + " vers " + repertoireLocal);
                        GestionTelechargement.SshTranfertFichier.RapatriementFichier(
                             serveur.HostName,
                             serveur.Login,
                             serveur.PassWord,
                             remotefichier,
                             repertoireLocal
                         );
                    }
                }
            }


        }

        /// <summary>
        /// creation de la liste des numero de contrat a rechercher dans le fichiers archive
        /// </summary>
        /// <returns></returns>
        private static List<string> CreationListeContratCcnRecherche()
        {
            List<string> listeContratRecherche = new List<string>();
            while (true)
            {
                string noOffreRecherche = "";
                Console.WriteLine("Entrez le numero contrat recherché ? : ");
                noOffreRecherche = Console.ReadLine();

                if (noOffreRecherche.Equals(""))
                {
                    break;
                }
                /*
                if (noOffreRecherche.Length > 11)
                { 
                    Console.WriteLine("Taille du numero d'offre trop grand (max 11 caractères) !");
                    continue;
                }
                */

                Console.WriteLine("Ajout du critère : " + noOffreRecherche);
                listeContratRecherche.Add(noOffreRecherche.ToUpper());

            }

            //---- DEBUG -----
            Console.WriteLine("Recap des numero de contrat recherchés : ");
            foreach (string no in listeContratRecherche)
            {
                Console.WriteLine("\t" + no);
            }

            return listeContratRecherche;
        }

        /// <summary>
        /// creation de la liste des numero d'offre a rechercher dans le fichiers archive
        /// </summary>
        /// <returns></returns>
        private static List<string> CreationListeNoOffreRecherche()
        {
            List<string> listeNoOffreRecherche = new List<string>();
            while (true)
            {
                string noOffreRecherche = "";
                Console.WriteLine("Entrez le numero d'offre recherché ? (3 car. min) : ");
                noOffreRecherche = Console.ReadLine();

                if (noOffreRecherche.Equals(""))
                {
                    break;
                }
                if (noOffreRecherche.Length < 3)
                {
                    Console.WriteLine("Taille du numero d'offre trop petit (min 3 caractères) !");
                    continue;
                }
                if (noOffreRecherche.Length > 11)
                {
                    Console.WriteLine("Taille du numero d'offre trop grand (max 11 caractères) !");
                    continue;
                }
                if (noOffreRecherche.Length == 11)
                {
                    if (!Regex.IsMatch(noOffreRecherche, "^[a-zA-Z]{1}\\d{10}$"))
                    {
                        Console.WriteLine("Le numero ne respecte pas le format (Z9999999999) !");
                        continue;
                    }
                }
                if (!Regex.IsMatch(noOffreRecherche, "^[a-zA-Z]{0,1}\\d{0,10}$"))
                {
                    Console.WriteLine("Maivaise réponse");
                    continue;
                }

                Console.WriteLine("Ajout du critère : " + noOffreRecherche);
                listeNoOffreRecherche.Add(noOffreRecherche.ToUpper());

            }

            //---- DEBUG -----
            Console.WriteLine("Recap des numero d'offres recherchés : ");
            foreach (string no in listeNoOffreRecherche)
            {
                Console.WriteLine("\t" + no);
            }

            return listeNoOffreRecherche;
        }


        /// <summary>
        /// creation de la liste des pattern de fichiers à rapatrier selon la date d'entree, la profondeur 
        /// ainsi que le prefixe et suffixe en parametre
        /// </summary>
        /// <returns></returns>
        private static List<string> CreationListePatternFichierRapatrier()
        {
            List<DateTime> listeDates = CreationListeDatesFichiersArchives();

            Hashtable sectionFiltrage = (Hashtable)ConfigurationManager.GetSection("CriteresFiltrageFichier");
            string prefix = sectionFiltrage["FICHIER_PREFIXE"].ToString();
            string suffix = sectionFiltrage["FICHIER_SUFFIXE"].ToString();

            List<string> listeFichiersRapatrier = new List<string>();
            foreach (DateTime dat in listeDates)
            {
                int jour = dat.Day;
                int mois = dat.Month;
                int annee = dat.Year;

                string patternFichiers = prefix + String.Format("{0:0000}", annee) + String.Format("{0:00}", mois) + String.Format("{0:00}", jour) + suffix;

                //Console.WriteLine("Rapatriement des fichier : " + patternFichiers);
                listeFichiersRapatrier.Add(patternFichiers);
            }

            return listeFichiersRapatrier;
        }

        /// <summary>
        /// criteres de filtrage 
        /// date de début et profondeur
        /// </summary>
        /// <returns></returns>
        private static List<DateTime> CreationListeDatesFichiersArchives()
        {

            Hashtable sectionFiltrage = (Hashtable)ConfigurationManager.GetSection("CriteresFiltrageFichier");
            int nbJourMax = Convert.ToInt32(sectionFiltrage["NB_JOUR_MAX"]);
            List < DateTime > listeDates = new List<DateTime>();

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

            DateTime dateDebut = DateTime.Now;
            if (!dateStr.Equals(""))
            {
                Regex expression = new Regex(sPattern);
                var results = expression.Matches(dateStr);
                string jour=null, mois=null, annee=null;
                foreach (Match match in results)
                {
                    jour  = match.Groups["jour"].Value;
                    mois  = match.Groups["mois"].Value;
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
                Console.WriteLine("Utilisation de la date du jour : " + jour + "/" + mois + "/" + annee);
            }

            //-------------------
            string nbJourStr = "";
            int nbJour = 0;
            while (true)
            {
                Console.Write("Nombre jour de recherche depuis la date de début ? [0-"+ nbJourMax + "] : ");
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
                for (int i=1; i<nbJour; i++)
                {
                    var dueDatePlus1Jour = dateDebut.AddDays(i);
                    listeDates.Add(dueDatePlus1Jour);
                }
            }

            return listeDates;
        }

        /// <summary>
        /// 
        /// </summary>
        private static void RepertoireTraitementLocal()
        {
            RepertoireRapatriementLocal();
            RepertoireExtractionLocal();            
            RepertoireResultatLocal();
        }


        /// <summary>
        /// 
        /// </summary>
        private static void RepertoireRapatriementLocal()
        {
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repRapatriement = sectionRepTraitements["REP_RAPATRIEMENT"].ToString();
         
            if (!Directory.Exists(repRapatriement))
            {
                Console.WriteLine("Création du repertoire de rapatriement : " + repRapatriement);
                Directory.CreateDirectory(repRapatriement);
            }
            else
            {
                Console.Write("Voulez vous vider le répertoire de rapatirement? [Oui|Non]: ");
                string resp = Console.ReadLine();
                if (resp.ToLower().Equals("") || resp.ToLower().StartsWith("o"))
                {
                    DirectoryInfo di = new DirectoryInfo(repRapatriement);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    Console.WriteLine("Répertoire " + repRapatriement + " vidé.");
                }

                Console.WriteLine();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private static void RepertoireExtractionLocal()
        {
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repTravail = sectionRepTraitements["REP_EXTRACTION"].ToString();

            if (!Directory.Exists(repTravail))
            {
                Console.WriteLine("Création du repertoire d'extraction : " + repTravail);
                Directory.CreateDirectory(repTravail);
            }
            else
            {
                Console.Write("Voulez vous vider le répertoire d'extraction? [Oui|Non]: ");
                string resp = Console.ReadLine();
                if (resp.ToLower().Equals("") || resp.ToLower().StartsWith("o"))
                {
                    DirectoryInfo di = new DirectoryInfo(repTravail);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    Console.WriteLine("Répertoire " + repTravail + " vidé.");
                }

                Console.WriteLine();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private static void RepertoireResultatLocal()
        {
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repResultat = sectionRepTraitements["REP_RESULTAT"].ToString();

            if (!Directory.Exists(repResultat))
            {
                Console.WriteLine("Création du repertoire de resultat : " + repResultat);
                Directory.CreateDirectory(repResultat);
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(repResultat);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                Console.WriteLine("Répertoire " + repResultat + " vidé.");

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xPathString"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static ArrayList SearchXPathNavigator(string xPathString, string xmlFile)
        {
            // Initilisation des variables
            XPathDocument xpathDoc;
            XPathNavigator xpathNavigator;
            XPathNodeIterator xpathNodeIterator;
            XPathExpression expr;
            ArrayList listOfAttributes = new ArrayList();

            // Parcours du fichier XML
            try
            {
                string XMLFilePath = xmlFile; //@"c:\tmp\I_VIAS_VADT__N_00000000_P_C_C0000030313.xml";
                xpathDoc = new XPathDocument(XMLFilePath);
                xpathNavigator = xpathDoc.CreateNavigator();

                expr = xpathNavigator.Compile(xPathString);
                xpathNodeIterator = xpathNavigator.Select(expr);

                while (xpathNodeIterator.MoveNext())
                {
                    // On récupère l'attribut
                    //listOfAttributes.Add(xpathNodeIterator.Current.GetAttribute(attribute, ""));                    

                    XPathNavigator nav = xpathNodeIterator.Current.Clone();
                    listOfAttributes.Add(nav.Value);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return listOfAttributes;
        }
    }
}
