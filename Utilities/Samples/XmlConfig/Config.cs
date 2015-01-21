using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Samples.XmlConfig
{
    public class Config
    {
        public Processus Processus;
        public List<Mail> Mails;
    
    }

    public class Mail
    {
        [XmlAttribute("address")]
        public string Address;

        [XmlAttribute("enabled")]
        public bool Enabled;

        [XmlAttribute("debugOnly")]
        public bool DebugOnly;
    }

    public class Processus
    {
        [XmlAttribute("name")]
        public string Name;

        public string Purpose;

        public List<DemoItem> DemoItems;
    }

    public class DemoItem
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlText]
        public string Content;
    }
}
