using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OutilInfosBanquaireConsole
{
    class OutilAcqRecherche
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Voullez vous quitter l'application ? [Q] :");
                string resp = Console.ReadLine();
                if (resp.ToLower().StartsWith("q")) break;

                string cvsdir = @"C:\Applications\AG2R\A0601 - OMG\Archives\4_acquittement";
                DirectoryInfo diWork = new DirectoryInfo(cvsdir);
                foreach (FileInfo file in diWork.GetFiles())
                {
                    //-- pour tous les fichiers XML 
                    //if (Regex.IsMatch(file.Name, ".*\\.xml$"))
                    if (Regex.IsMatch(file.Name, ".*\\.csv$"))
                    {

                        /*
                        //string val = GetNumeroContrat(file.FullName, "0HL7196");
                        string val = GetNumeroOffre(file.FullName, "C0000050793");
                        if (val == null)
                        {

                        }
                        else
                        {
                            Console.WriteLine("==> " + val + " trouvé dans " + file.Name);
                            break;
                        }
                        */
                        


                        /*------*/

                        
                        
                        List<string> listeRecherche = new List<string>()
                        {
"C0000044977",
"C0000050793",
"C0000083994",
"C0000095013",
"C0000095125",
"C0000095264",
"C0000095721",
"C0000095741",
"C0000096109",
"C0000096235",
"C0000096259",
"C0000096321",
"C0000096942",
"C0000097273",
"C0000097340",
"C0000097531",
"C0000097734",
"C0000097781",
"C0000098237",
"A1530700201",
"A1530700204",
"A1668600922",
"A1873600925",
"A2008301241",
"A2035100741",
"A2048601266",
"A2087700562",
"A2102900825",
"A2187200682",
"A2187601221",
"A2215200922",
"A2227800744",
"A2229000121",
"A2237200341",
"A2249400181",
"A2255300463",
"A2262203043",
"A2262403143",
"A2268902321",
"A2270500705",
"A2278700501",
"A2293700583",
"A2294800886",
"A2308200128",
"A2314400421",
"A2318800301",
"A2322400261"
                        };

                        foreach (string no in listeRecherche)
                        { 
                            string val = GetNumeroOffre(file.FullName, no);

                            if (val == null)
                            {
                            }
                            else
                            {
                                Console.WriteLine("==> " + val + " trouvé dans " + file.Name);
                                break;
                            }
                        }
                        
                    

                    }
                }

                /*
                string cvsfile = @"C:\tmp\acq\pb premiere ligne manquante\OMG_CCN_COLL_CONTRATS_LPC1I351.20160407.223732.csv";
                string val = GetAddress(cvsfile, "A2307700422");

                if (val == null)
                {

                } 
                else
                { 
                    Console.WriteLine("==> " + val + " trouvé dans " + cvsfile);
                }
                */

            /*****************/

                //-- traitement des archives
                //Traitement();

                //-- traitement des numero de contrats Absents
                //TraitementNoContratAbs();

                //-- analyse stat dossier
                //TraitementStatDossier();
                //TraitementAnalyse();
            }

            Environment.Exit(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cvsfile"></param>
        /// <param name="searchName"></param>
        /// <returns></returns>
        static string GetNumeroOffre(string cvsfile, string searchName)
        {            
            var strLines = File.ReadLines(cvsfile);
            foreach (var line in strLines)
            {
                if (line.Split(';')[0].Equals(searchName))
                    return line.Split(';')[2];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cvsfile"></param>
        /// <param name="searchNoContrat"></param>
        /// <returns></returns>
        static string GetNumeroContrat(string cvsfile, string searchNoContrat)
        {
            var strLines = File.ReadLines(cvsfile);
            foreach (var line in strLines)
            {
                if (line.Split(';')[2].Equals(searchNoContrat))
                    return line.Split(';')[0];
            }

            return null;
        }

    }
}
