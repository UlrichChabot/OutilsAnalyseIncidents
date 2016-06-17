using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.IO;



// example: Extract(@"c:\temp\test.tar.gz", @"C:\DestinationFolder")

namespace GestionArchive
{
    public class ExtractTGZ
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gzArchiveName"></param>
        /// <param name="destFolder"></param>
        public void Extract(String gzArchiveName, String destFolder)
        {
            ExtractVerbos(gzArchiveName, destFolder, true);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gzArchiveName"></param>
        /// <param name="destFolder"></param>
        /// <param name="verbos"></param>
        public void ExtractVerbos(String gzArchiveName, String destFolder, bool verbos)
        {

            Stream inStream = File.OpenRead(gzArchiveName);
            Stream gzipStream = new GZipInputStream(inStream);

            System.IO.StreamWriter fileRes = null;

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            if (verbos)
            {
                fileRes = new System.IO.StreamWriter(destFolder + @"\stdout.csv", true);
                tarArchive.ProgressMessageEvent += delegate (TarArchive tar, TarEntry tarEntry, string msg) { fileRes.WriteLine(gzArchiveName+ ";" +tarEntry.Name); };
            }

            tarArchive.ExtractContents(destFolder);
            //tarArchive.ListContents();

            if (verbos)
            {
                fileRes.Flush();
                fileRes.Close();
            }

            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
        }

    }

}
