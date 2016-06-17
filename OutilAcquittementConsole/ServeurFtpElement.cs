using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutilAcquittementConsole
{
    public class ServeurFtpElement : ConfigurationElement
    {

        public ServeurFtpElement() { }

        public ServeurFtpElement(string hostName, string login, string passWord)
        {
            HostName = hostName;
            Login = login;
            PassWord = passWord;
        }

        [ConfigurationProperty("HostName", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string HostName
        {
            get { return (string)this["HostName"]; }
            set { this["HostName"] = value; }
        }

        [ConfigurationProperty("Login", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Login
        {
            get { return (string)this["Login"]; }
            set { this["Login"] = value; }
        }


        [ConfigurationProperty("PassWord", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string PassWord
        {
            get { return (string)this["PassWord"]; }
            set { this["PassWord"] = value; }
        }

    }


    public class ServeurFtpCollection : ConfigurationElementCollection
    {
        public ServeurFtpCollection()
        {
           
        }

        public ServeurFtpElement this[int index]
        {
            get { return (ServeurFtpElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(ServeurFtpElement serveurFtpElement)
        {
            BaseAdd(serveurFtpElement);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServeurFtpElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServeurFtpElement)element).HostName;
        }

        public void Remove(ServeurFtpElement serveurFtpElement)
        {
            BaseRemove(serveurFtpElement.HostName);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }


    public class ListeServeurFtpSection : ConfigurationSection
    {
        [ConfigurationProperty("ServeursFtp", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ServeurFtpCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ServeurFtpCollection ServeursFtp
        {
            get
            {
                return (ServeurFtpCollection)base["ServeursFtp"];
            }
        }
    }
}
