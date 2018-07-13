using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BezvizSystem.BLL.Interfaces.XML
{
    interface IXmlCreator
    {
        void Save(string name, SaveOptions options);
    }
}
