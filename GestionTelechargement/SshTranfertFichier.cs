using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch;

namespace GestionTelechargement
{
    public class SshTranfertFichier
    {

        /// <summary>
        /// test de la connexion
        /// </summary>
        /// <param name="host"></param>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        public static bool TestConnexionServeur(string host, string login, string pass)
        {
            try
            {
                SshTransferProtocolBase sftp = new Sftp(host, login, pass);
                sftp = new Sftp(host, login, pass);
                Console.Write("Connexion au serveur ...");
                sftp.Connect();
                Console.WriteLine(" OK");
                sftp.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        public static void TransfertTest(string host, string login, string pass)
        {
            try
            {
                Sftp sftp = new Sftp(host, login, pass);
                sftp.Connect();
                //sftp.Get("/env/omg/archives/OMG_INDIV_LIC1I307.20141119.211528.tar.gz", "c:\\tmp\\OMG_INDIV_LIC1I307.20141119.211528.tar.gz");
                //sftp.Get("/env/omg/archives/fichier_*inexistant.tar.gz", "c:\\tmp\\");
                //sftp.Get("/env/omg/archives/OMG_INDIV_LIC1I307.20141119.211528.tar.gz", "c:\\Temp\\telechargement\\");
                sftp.Get("/env/omg/archives/OMG_*.20141119.*.tar.gz", "c:\\Temp\\telechargement\\");
                sftp.Close();
                Console.WriteLine("OK");
            }
            catch (SftpException e)
            {
                Console.WriteLine("Fichier inexistant");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }




        public static void TransfertTestAttribut(string host, string login, string pass, string repertoireLocal, string repertoireRemote, string pattern, List<string> listeDatesFichiersRapatrier)
        {
            JSch jsch = new JSch();
            Session session = jsch.getSession(login, host);
           
            session.setPassword(pass);

            Hashtable config = new Hashtable();
            config["StrictHostKeyChecking"] = "no";
            session.setConfig(config);

            session.connect();
            Channel channel = session.openChannel("sftp");
            channel.connect();

            ChannelSftp sftpChannel = (ChannelSftp)channel;

            sftpChannel.lcd(repertoireLocal);
            sftpChannel.cd(repertoireRemote);

            //--
            ArrayList vv = sftpChannel.ls(repertoireRemote);


            if (vv != null)
            {
                for (int ii = 0; ii < vv.Count; ii++)
                {
                    object obj = vv[ii];

                    if (obj is ChannelSftp.LsEntry)
                    {
                        ChannelSftp.LsEntry entry = (ChannelSftp.LsEntry)obj;
                        //Console.WriteLine(vv[ii]);

                        if (entry.getAttrs().isDir()) continue;

                        if (entry.getFilename().ToString() != "." && entry.getFilename().ToString() != "..")
                        {
                            //Console.WriteLine("Name:" + entry.getFilename().ToString() + " date:" + entry.getAttrs().getMtimeString() + "size:" + entry.getAttrs().getSize());

                            if (!Regex.IsMatch(entry.getFilename(), pattern)) continue;

                            //-- pas de fichier de taille 0
                            if (entry.getAttrs().getSize() == 0) continue;

                            //-- le fichier dont la date correspond 

                            foreach(string dateStr in listeDatesFichiersRapatrier)
                            {
                                if (Regex.IsMatch(entry.getAttrs().getMtimeString(), "^"+dateStr))
                                {
                                    Console.WriteLine("=> " + entry.getFilename().ToString() + " [" + entry.getAttrs().getMtimeString() + "]");

                                    int mode = ChannelSftp.OVERWRITE;
                                    SftpProgressMonitor monitor = new MyProgressMonitor();
                                    sftpChannel.get(entry.getFilename().ToString(), entry.getFilename().ToString(), monitor, mode);

                                }
                            }

                            
                        }

                    }
                }
            }

            channel.disconnect();
            session.disconnect();

        }


       

  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <param name="rfile"></param>
        /// <param name="lpath"></param>
        public static void RapatriementFichier(string host, string login, string pass, string rfile, string lpath)
        {
            try
            {
                SshTransferProtocolBase sftp = new Sftp(host, login, pass);
                sftp = new Sftp(host, login, pass);

                sftp.OnTransferStart += new FileTransferEvent(sshCp_OnTransferStart);
                sftp.OnTransferProgress += new FileTransferEvent(sshCp_OnTransferProgress);
                sftp.OnTransferEnd += new FileTransferEvent(sshCp_OnTransferEnd);
                sftp.Connect();
                sftp.Get(rfile, lpath);
                sftp.Close();
            }
            catch (SftpException) { }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        public class MyProgressMonitor : SftpProgressMonitor
        {
            private ConsoleProgressBar bar;
            private long c = 0;
            private long max = 0;
            private long percent = -1;
            int elapsed = -1;

            System.Timers.Timer timer;

            public override void init(int op, String src, String dest, long max)
            {
                bar = new ConsoleProgressBar();
                this.max = max;
                elapsed = 0;
                timer = new System.Timers.Timer(1000);
                timer.Start();
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            }
            public override bool count(long c)
            {
                this.c += c;
                if (percent >= this.c * 100 / max) { return true; }
                percent = this.c * 100 / max;

                string note = ("Transfering... [Elapsed time: " + elapsed + "]");

                bar.Update((int)this.c, (int)max, note);
                return true;
            }
            public override void end()
            {
                timer.Stop();
                timer.Dispose();
                string note = ("Done in " + elapsed + " seconds!");
                bar.Update((int)this.c, (int)max, note);
                bar = null;
            }

            private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                this.elapsed++;
            }
        }



        /// <summary>
        /// telechargement interactif
        /// </summary>
        public static void TransfertInteractif()
        {
            try
            {
                SshConnectionInfo input = Util.GetInput();
                string proto = GetProtocol();
                SshTransferProtocolBase sshCp;

                if (proto.Equals("scp"))
                    sshCp = new Scp(input.Host, input.User);
                else
                    sshCp = new Sftp(input.Host, input.User);

                if (input.Pass != null) sshCp.Password = input.Pass;
                if (input.IdentityFile != null) sshCp.AddIdentityFile(input.IdentityFile);

                sshCp.OnTransferStart += new FileTransferEvent(sshCp_OnTransferStart);
                sshCp.OnTransferProgress += new FileTransferEvent(sshCp_OnTransferProgress);
                sshCp.OnTransferEnd += new FileTransferEvent(sshCp_OnTransferEnd);

                Console.Write("Connecting...");
                sshCp.Connect();
                Console.WriteLine("OK");

                while (true)
                {
                    string direction = GetTransferDirection();
                    if (direction.Equals("to"))
                    {
                        string lfile = GetArg("Enter local file ['Enter to cancel']");
                        if (lfile == "") break;
                        string rfile = GetArg("Enter remote file ['Enter to cancel']");
                        if (rfile == "") break;
                        sshCp.Put(lfile, rfile);
                    }
                    else
                    {
                        string rfile = GetArg("Enter remote file ['Enter to cancel']");
                        if (rfile == "") break;
                        string lpath = GetArg("Enter local path ['Enter to cancel']");
                        if (lpath == "") break;
                        sshCp.Get(rfile, lpath);
                    }
                }

                Console.Write("Disconnecting...");
                sshCp.Close();
                Console.WriteLine("OK");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Question sur le protocol a utiliser
        /// </summary>
        /// <returns></returns>
        public static string GetProtocol()
        {
            string proto = "";
            while (true)
            {
                Console.Write("Enter SSH transfer protocol [SCP|SFTP]: ");
                proto = Console.ReadLine();
                if (proto.ToLower().Equals("")) break;
                if (proto.ToLower().Equals("scp") || proto.ToLower().Equals("sftp"))
                    break;
                Console.Write("Bad input, ");
            }
            return proto;
        }


        /// <summary>
        /// Question sur la direction depuis ou vers
        /// </summary>
        /// <returns></returns>
        public static string GetTransferDirection()
        {
            string dir = "";
            while (true)
            {
                Console.Write("Enter transfer direction [To|From]: ");
                dir = Console.ReadLine();
                if (dir.ToLower().Equals("")) break;
                if (dir.ToLower().Equals("to") || dir.ToLower().Equals("from"))
                    break;
                Console.Write("Bad input, ");
            }
            return dir;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetArg(string msg)
        {
            Console.Write(msg + ": ");
            return Console.ReadLine();
        }


        static ConsoleProgressBar progressBar;

        private static void sshCp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            Console.WriteLine();
            progressBar = new ConsoleProgressBar();
            progressBar.Update(transferredBytes, totalBytes, message);
        }

        private static void sshCp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            if (progressBar != null)
            {
                progressBar.Update(transferredBytes, totalBytes, message);
            }
        }

        private static void sshCp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            if (progressBar != null)
            {
                progressBar.Update(transferredBytes, totalBytes, message);
                progressBar = null;
            }
        }



 


    }
}
