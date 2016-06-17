using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTelechargement
{
    /// <summary>
    /// Summary description for Util.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Get input from the user
        /// </summary>
        public static SshConnectionInfo GetInput()
        {
            SshConnectionInfo info = new SshConnectionInfo();
            Console.Write("Enter Remote Host: ");
            info.Host = Console.ReadLine();
            Console.Write("Enter Username: ");
            info.User = Console.ReadLine();

            Console.Write("Use publickey authentication? [Yes|No] :");
            string resp = Console.ReadLine();
            if (resp.ToLower().StartsWith("y"))
            {
                Console.Write("Enter identity key filename: ");
                info.IdentityFile = Console.ReadLine();
            }
            else
            {
                Console.Write("Enter Password: ");
                info.Pass = Console.ReadLine();
            }
            Console.WriteLine();
            return info;
        }
    }

    public struct SshConnectionInfo
    {
        public string Host;
        public string User;
        public string Pass;
        public string IdentityFile;
    }
}
