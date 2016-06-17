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
            List<string> listeFichiersRapatrier = CreationListePatternFichierRapatrier();

            /*------------*/
   /*            
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
   */              /*
                            if (noOffreRecherche.Length < 3)
                            {
                                Console.WriteLine("Taille du numero d'offre trop petit (min 3 caractères) !");
                                continue;
                            }
                            */
                              /*              if (noOffreRecherche.Length > 11)
                                            { 
                                                Console.WriteLine("Taille du numero d'offre trop grand (max 11 caractères) !");
                                                continue;
                                            }
                               */             /*
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
                                            */
                                              /*
                                                             Console.WriteLine("Ajout du critère : " + noOffreRecherche);
                                                             listeNoOffreRecherche.Add(noOffreRecherche.ToUpper());

                                                         }
                                             */

List<string> listeNoOffreRecherche = new List<string>() 
            { "C0000027122",
"C0000036426",
"C0000036439",
"C0000036935",
"C0000037398",
"C0000039247",
"C0000044084",
"C0000044977",
"C0000045072",
"C0000045292",
"C0000045987",
"C0000047253",
"C0000047292",
"C0000047878",
"C0000047895",
"C0000047925",
"C0000048709",
"C0000048714",
"C0000050046",
"C0000050671",
"C0000050738",
"C0000050743",
"C0000050750",
"C0000050782",
"C0000050793",
"C0000050804",
"C0000050821",
"C0000051036",
"C0000051441",
"C0000051592",
"C0000052178",
"C0000052377",
"C0000052644",
"C0000052966",
"C0000053381",
"C0000053741",
"C0000053768",
"C0000053933",
"C0000055442",
"C0000055863",
"C0000055960",
"C0000056059",
"C0000056395",
"C0000056421",
"C0000056979",
"C0000057377",
"C0000057497",
"C0000058108",
"C0000058584",
"C0000059239",
"C0000059622",
"C0000060204",
"C0000060413",
"C0000060906",
"C0000061156",
"C0000061603",
"C0000061700",
"C0000061763",
"C0000061868",
"C0000062097",
"C0000062523",
"C0000063021",
"C0000063026",
"C0000063169",
"C0000063696",
"C0000064500",
"C0000064596",
"C0000064809",
"C0000064886",
"C0000064898",
"C0000064951",
"C0000065203",
"C0000065220",
"C0000065406",
"C0000065748",
"C0000065796",
"C0000066533",
"C0000066761",
"C0000066809",
"C0000066861",
"C0000067658",
"C0000068357",
"C0000069001",
"C0000069837",
"C0000069955",
"C0000070534",
"C0000071412",
"C0000071461",
"C0000072027",
"C0000072562",
"C0000072611",
"C0000073453",
"C0000073553",
"C0000073647",
"C0000073753",
"C0000073771",
"C0000073802",
"C0000074047",
"C0000074155",
"C0000074952",
"C0000075189",
"C0000075264",
"C0000075598",
"C0000075961",
"C0000075993",
"C0000076012",
"C0000076023",
"C0000076100",
"C0000076138",
"C0000076212",
"C0000076906",
"C0000076942",
"C0000077484",
"C0000077500",
"C0000077549",
"C0000077831",
"C0000077863",
"C0000077972",
"C0000078345",
"C0000078388",
"C0000078454",
"C0000078568",
"C0000078581",
"C0000078754",
"C0000078956",
"C0000079074",
"C0000079104",
"C0000079587",
"C0000079612",
"C0000079701",
"C0000079778",
"C0000079900",
"C0000079902",
"C0000079939",
"C0000080003",
"C0000080069",
"C0000080186",
"C0000080311",
"C0000080321",
"C0000080769",
"C0000080950",
"C0000081114",
"C0000081522",
"C0000081550",
"C0000081559",
"C0000081736",
"C0000081755",
"C0000082033",
"C0000082119",
"C0000082460",
"C0000082615",
"C0000082774",
"C0000082810",
"C0000082840",
"C0000082841",
"C0000082855",
"C0000082967",
"C0000083391",
"C0000083393",
"C0000083994",
"C0000084406",
"C0000084624",
"C0000085168",
"C0000085239",
"C0000085307",
"C0000085539",
"C0000086323",
"C0000086519",
"C0000086632",
"C0000087361",
"C0000087466",
"C0000087928",
"C0000088529",
"C0000088649",
"C0000088817",
"C0000089175",
"C0000089221",
"C0000089463",
"C0000089843",
"C0000090091",
"C0000090142",
"C0000090335",
"C0000090498",
"C0000092213",
"C0000092238",
"C0000093123",
"C0000093521",
"C0000093823",
"C0000046376",
"C0000054495",
"C0000080179",
"C0000081833",
"C0000083568",
"C0000084257",
"C0000088280",
"C0000089898",
"A0526200250",
"A0636900381",
"A0691900202",
"A0731200902",
"A0759401281",
"A0768500884",
"A0855800261",
"A0855800363",
"A0941501622",
"A0941501641",
"A0959400401",
"A0959400407",
"A0987401764",
"A0987401822",
"A1031900341",
"A1116500421",
"A1126301063",
"A1136400581",
"A1140900423",
"A1178000369",
"A1213200501",
"A1213201383",
"A1213201622",
"A1213201681",
"A1294000021",
"A1305200545",
"A1305200625",
"A1344200721",
"A1344701215",
"A1365100121",
"A1365100122",
"A1406100283",
"A1414300601",
"A1414800381",
"A1414800382",
"A1420800888",
"A1462800481",
"A1470200063",
"A1499400921",
"A1518800461",
"A1524200485",
"A1524200507",
"A1528700601",
"A1528700645",
"A1528700941",
"A1528701002",
"A1528701101",
"A1558200183",
"A1558200301",
"A1562101287",
"A1607800401",
"A1623900321",
"A1623900341",
"A1623900362",
"A1623900381",
"A1623900422",
"A1623900423",
"A1623900441",
"A1623900461",
"A1623900481",
"A1623900501",
"A1623900541",
"A1641500261",
"A1644000003",
"A1644000141",
"A1656600343",
"A1669400541",
"A1669800745",
"A1687100179",
"A1700200301",
"A1700200342",
"A1700200402",
"A1712900481",
"A1717600325",
"A1723800421",
"A1723800741",
"A1766000961",
"A1771100421",
"A1774601049",
"A1774700222",
"A1774700341",
"A1774700521",
"A1806200241",
"A1806200242",
"A1846500403",
"A1846500482",
"A1858100004",
"A1858100310",
"A1858100393",
"A1859001362",
"A1859001423",
"A1861100483",
"A1861100522",
"A1863201641",
"A1873600905",
"A1875800003",
"A1891300003",
"A1891300045",
"A1891300169",
"A1897300042",
"A1926901021",
"A1947200688",
"A1947200867",
"A1947200967",
"A1947201048",
"A1947201066",
"A1947201067",
"A1947201087",
"A1947201088",
"A1947201186",
"A1947201346",
"A1947601224",
"A1947601227",
"A1947601382",
"A1947601383",
"A1955900751",
"A1964100762",
"A1964100781",
"A1972600641",
"A1983800241",
"A1983800381",
"A1983800383",
"A1983800401",
"A1992300143",
"A1992300162",
"A1997600781",
"A1997600785",
"A2001300843",
"A2001301362",
"A2008000081",
"A2008000181",
"A2008000221",
"A2008300561",
"A2008800242",
"A2009200123",
"A2009301421",
"A2009301482",
"A2009301484",
"A2009301485",
"A2009301487",
"A2009301521",
"A2009301523",
"A2009301524",
"A2009301604",
"A2009301608",
"A2009301609",
"A2009301721",
"A2009301821",
"A2022900725",
"A2028400808",
"A2028400823",
"A2028400923",
"A2036000808",
"A2037400593",
"A2037501325",
"A2037502421",
"A2037502581",
"A2041300621",
"A2048600605",
"A2048600924",
"A2051401101",
"A2056500243",
"A2056500462",
"A2056500481",
"A2056500567",
"A2056700221",
"A2056700242",
"A2056700261",
"A2056700361",
"A2056700441",
"A2058700921",
"A2060600742",
"A2060600781",
"A2061701041",
"A2061701261",
"A2084600381",
"A2090500004",
"A2090500301",
"A2092700601",
"A2092700781",
"A2093200462",
"A2098400405",
"A2098400425",
"A2098400442",
"A2098400483",
"A2098400484",
"A2098400503",
"A2098400509",
"A2098400604",
"A2102900641",
"A2102900654",
"A2102900688",
"A2105700542",
"A2107300081",
"A2110201069",
"A2110201071",
"A2110201085",
"A2110201087",
"A2110201088",
"A2110201089",
"A2110201100",
"A2115400321",
"A2124700021",
"A2127700507",
"A2128701401",
"A2128701664",
"A2135100321",
"A2135100341",
"A2135100501",
"A2138400216",
"A2165901662",
"A2171400242",
"A2176800322",
"A2179100202",
"A2181200704",
"A2186200761",
"A2186300441",
"A2186900759",
"A2186901037",
"A2187400381",
"A2187500023",
"A2187500024",
"A2187500025",
"A2187500242",
"A2187500422",
"A2187500481",
"A2187500542",
"A2187500581",
"A2188700942",
"A2189600561",
"A2195800508",
"A2198600942",
"A2200300481",
"A2200300521",
"A2200700384",
"A2200700426",
"A2200700470",
"A2200700584",
"A2200700645",
"A2202100781",
"A2202500763",
"A2202501025",
"A2203100302",
"A2203100385",
"A2203100406",
"A2205300841",
"A2205300945",
"A2205800261",
"A2205800262",
"A2206000521",
"A2206100041",
"A2206800324",
"A2207700544",
"A2207700842",
"A2207800141",
"A2207800341",
"A2207800361",
"A2207800364",
"A2208400327",
"A2214600682",
"A2217400961",
"A2223401302",
"A2223401522",
"A2226000221",
"A2226000222",
"A2226000226",
"A2226000303",
"A2226000344",
"A2226000443",
"A2228300762",
"A2228300763",
"A2228700343",
"A2229102141",
"A2230000082",
"A2230000281",
"A2230000301",
"A2230100401",
"A2230100741",
"A2230801311",
"A2231100203",
"A2233101404",
"A2233102301",
"A2233102481",
"A2233400002",
"A2233400721",
"A2233400841",
"A2233500122",
"A2233500263",
"A2233500281",
"A2233500361",
"A2233500441",
"A2233500482",
"A2233500561",
"A2233500641",
"A2234200121",
"A2234200422",
"A2234200423",
"A2234200425",
"A2234200426",
"A2234200427",
"A2234200428",
"A2234200446",
"A2234500763",
"A2234900482",
"A2235200641",
"A2235200783",
"A2236700747",
"A2236700803",
"A2236700862",
"A2236700863",
"A2236700925",
"A2236900662",
"A2236901307",
"A2236901601",
"A2237200341",
"A2238101047",
"A2238400002",
"A2238400165",
"A2238400225",
"A2238700184",
"A2238700204",
"A2238700563",
"A2238700663",
"A2238700683",
"A2241001925",
"A2241300343",
"A2244200381",
"A2244200796",
"A2245600224",
"A2245600261",
"A2245600265",
"A2245600269",
"A2245600270",
"A2245600301",
"A2245600321",
"A2247101541",
"A2249400141",
"A2250400222",
"A2250400661",
"A2250500682",
"A2250500781",
"A2253401209",
"A2254500384",
"A2255900143",
"A2255900261",
"A2256400861",
"A2256500106",
"A2258500481",
"A2258501046",
"A2258501064",
"A2258600481",
"A2258600611",
"A2258600881",
"A2258600882",
"A2258600963",
"A2258601001",
"A2259200441",
"A2259200442",
"A2260000181",
"A2260000241",
"A2260000303",
"A2261301661",
"A2262201306",
"A2262201882",
"A2262202041",
"A2262202142",
"A2262202222",
"A2262202362",
"A2262202541",
"A2262202550",
"A2262300749",
"A2262300750",
"A2262300827",
"A2262300905",
"A2262300926",
"A2262301001",
"A2262301029",
"A2262401642",
"A2262401661",
"A2262401902",
"A2262402585",
"A2262402742",
"A2262402841",
"A2262600728",
"A2262600732",
"A2262600828",
"A2262600888",
"A2263900166",
"A2263900261",
"A2264700383",
"A2268300608",
"A2268902222",
"A2269200021",
"A2271200861",
"A2271500823",
"A2271500961",
"A2271900741",
"A2273400564",
"A2273400565",
"A2273400566",
"A2273900530",
"A2273900693",
"A2273900830",
"A2273900892",
"A2273900915",
"A2274100460",
"A2274400802",
"A2274600183",
"A2275301381",
"A2275500063",
"A2276100221",
"A2276300941",
"A2278700001",
"A2278700201",
"A2278700222",
"A2278700262",
"A2278700263",
"A2278700302",
"A2279700822",
"A2279701201",
"A2280101084",
"A2280101086",
"A2280101087",
"A2281600103",
"A2281600104",
"A2281600123",
"A2281600142",
"A2281700002",
"A2282000126",
"A2282800223",
"A2283200227",
"A2283200261",
"A2283200581",
"A2283200683",
"A2283200701",
"A2283200741",
"A2283200742",
"A2283200761",
"A2283200823",
"A2283200844",
"A2283200863",
"A2283201045",
"A2283300342",
"A2286800361",
"A2286800362",
"A2286800381",
"A2287100283",
"A2287100601",
"A2287300521",
"A2287400305",
"A2287400327",
"A2287400329",
"A2287400433",
"A2287400541",
"A2287900667",
"A2288400702",
"A2290700206",
"A2290700223",
"A2290700224",
"A2290700225",
"A2290700244",
"A2290800304",
"A2290900482",
"A2291900268",
"A2292900021",
"A2292900041",
"A2292900061",
"A2292900161",
"A2293700583",
"A2295000363",
"A2295000421",
"A2295000721",
"A2295800503",
"A2296300022",
"A2296300081",
"A2296300082",
"A2296300221",
"A2296300261",
"A2296300342",
"A2296300564",
"A2296300566",
"A2296400781",
"A2296400822",
"A2296400863",
"A2296500682",
"A2296500741",
"A2296500761",
"A2296500821",
"A2297700681",
"A2297900421",
"A2297900481",
"A2299500443",
"A2299500481",
"A2299500501",
"A2299500502",
"A2299500503",
"A2299500504",
"A2300000362",
"A2300200341",
"A2300200443",
"A2301700481",
"A2301900121",
"A2304801263",
"A2304801442",
"A2305801121",
"A2306100601",
"A2306100602",
"A2306500272",
"A2306500311",
"A2306500632",
"A2306500871",
"A2307100321",
"A2308200128",
"A2308200130",
"A2308200226",
"A2308800268",
"A2309400342",
"A2310000141",
"A2310000144",
"A2310200601",
"A2310200681",
"A2310200682",
"A2310200683",
"A2310200684",
"A2311900401",
"A2312100161",
"A2312300244",
"A2312300363",
"A2312300462",
"A2312500363",
"A2314000423",
"A2314100305",
"A2314400403",
"A2316300065",
"A2316600282",
"A2316700107",
"A2317200142",
"A2317300362",
"A2317300421",
"A2317400287",
"A2318400221",
"A2318500306",
"A2318900428",
"A2318900482",
"A2319000286",
"A2321400121",
"A2321400221",
"A2321500283",
"A2322500081",
"C0000025612",
"C0000044373",
"C0000047978",
"C0000049716",
"C0000050975",
"C0000052682",
"C0000060957",
"C0000065635",
"C0000068506",
"C0000071579",
"C0000073291",
"C0000073300",
"C0000075756",
"C0000079807",
"C0000080409",
"C0000084465",
"C0000084705",
"C0000084719",
"C0000085707",
"C0000086087",
"C0000086186",
"C0000086360",
"C0000086524",
"C0000087968",
"C0000088739",
"C0000090468",
"C0000054852",
"C0000072457",
"C0000074860",
"C0000081725",
"C0000082271",
"C0000082301",
"C0000084902",
"C0000085599",
"C0000086149",
"C0000086247",
"C0000087580",
"C0000087992",
"C0000087997",
"C0000089308",
"C0000089325",
"C0000090400",
"C0000090436",
"C0000091049",
"C0000091425",
"C0000093296",
"C0000093299",
"C0000093469",
"C0000093512",
"C0000093545",
"C0000093617",
"C0000093639",
"C0000093649",
"C0000093724",
"C0000082430",
"C0000085750",
"C0000088272",
"C0000089378",
"C0000093710",
"C0000093801",
"C0000093824",
"C0000022037",
"C0000033311",
"C0000034802",
"C0000053150",
"C0000053766",
"C0000056663",
"C0000060366",
"C0000061364",
"C0000061486",
"C0000063472",
"C0000064966",
"C0000068925",
"C0000069124",
"C0000069602",
"C0000071992",
"C0000075548",
"C0000075790",
"C0000076418",
"C0000077124",
"C0000077434",
"C0000079895",
"C0000081066",
"C0000084619",
"C0000085777",
"C0000085802",
"C0000085940",
"C0000085967",
"C0000085976",
"C0000085988",
"C0000086006",
"C0000086058",
"C0000086102",
"C0000086113",
"C0000087898",
"C0000091946",
"C0000092744",
"C0000093514",
"C0000093743",
"C0000093778",
"C0000093841",
"C0000043601",
"C0000069130",
"C0000070693",
"C0000076144",
"C0000077407",
"C0000079545",
"C0000084635",
"C0000090538",
"C0000090727"};



            //---- DEBUG -----
 /*
            Console.WriteLine("Recap des numero d'offres recherchés : ");
            foreach (string no in listeNoOffreRecherche)
            {
                Console.WriteLine("\t" + no);
            }
 */



            /*            while (true)
                        {
                            Console.Write("Entrez le numero d'offre recherche ? (11 car.) : ");
                            noOffreRecherche = Console.ReadLine();

                            if (noOffreRecherche.ToLower().Equals("")) break;
                            if (Regex.IsMatch(noOffreRecherche, "^[a-zA-Z]{1}\\d{10}$")) break;
                            Console.Write("Maivaise réponse, ");
                        }
            */



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


            if (listeNoOffreRecherche == null || listeNoOffreRecherche.Count > 0 )
            {
                int nbCorrespondance = 0;
                DirectoryInfo diWork = new DirectoryInfo(repertoireTravail);
                foreach (FileInfo file in diWork.GetFiles())
                {
                    //-- pour tous les fichiers XML 
                    if (Regex.IsMatch(file.Name,".*\\.xml$"))
                    { 
                        /*
                        bool aCoper = false;
                       
                        ArrayList idClient = SearchXPathNavigator("//codeAZ", file.FullName);
                        foreach (var val in idClient)
                        {
                            //Console.WriteLine("codeAZ : " + val);
                            if (noOffreRecherche.ToUpper().Equals(val))
                            {
                                aCoper = true;
                                nbCorrespondance++;
                                break;
                            }
                        }
                        */



                        if ( testOffreDansXml(listeNoOffreRecherche, file) )
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
        private static bool testOffreDansXml(List<string> listeNoOffreRecherche, FileInfo file)
        {
            ArrayList idClient = SearchXPathNavigator("//codeAZ", file.FullName);
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
                Console.WriteLine("Connexion au serveur : " + serveur.HostName );
                bool connexOk = GestionTelechargement.SshTranfertFichier.TestConnexionServeur(serveur.HostName, serveur.Login, serveur.PassWord);
                if (connexOk)
                {
                    foreach (string pattern in listeFichiersRapatrier) {
                       string remotefichier = repertoireRemote + pattern;
                       Console.WriteLine("Tantative de rapatriement de "+ remotefichier + " vers " + repertoireLocal);
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
        /// creation de la liste des pattern de fichiers à rapatrier selon la date d'entree, la profondeur 
        /// ainsi que le prefixe et suffixe en parametre
        /// </summary>
        /// <returns></returns>
        private static List<string> CreationListePatternFichierRapatrier()
        {
            List<DateTime> listeDates = GetCriteresFiltrage();

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
        private static List<DateTime> GetCriteresFiltrage()
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
            //--------------------------
            Hashtable sectionRepTraitements = (Hashtable)ConfigurationManager.GetSection("RepertoiresTraitements");
            string repRapatriement = sectionRepTraitements["REP_RAPATRIEMENT"].ToString();
            string repResultat     = sectionRepTraitements["REP_RESULTAT"].ToString();
            string repTravail      = sectionRepTraitements["REP_EXTRACTION"].ToString();


            //--------------------------
            if (!Directory.Exists(repTravail))
            {
                Console.WriteLine("Création du repertoire de travail : " + repTravail);
                Directory.CreateDirectory(repTravail);
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(repTravail);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                Console.WriteLine("Répertoire " + repTravail + " vidé.");
            }


            //--------------------------
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

            //--------------------------
            if ( ! Directory.Exists(repRapatriement))
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
                    Console.WriteLine("Répertoire "+repRapatriement+" vidé.");
                }
                
                Console.WriteLine();
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
