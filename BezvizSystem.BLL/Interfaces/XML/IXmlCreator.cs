using BezvizSystem.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BezvizSystem.BLL.Interfaces.XML
{
    public interface IXmlCreator
    {
        OperationDetails SaveNew(string name, SaveOptions options);
        OperationDetails SaveNew(string name);

        OperationDetails SaveExtra(string name, SaveOptions options);
        OperationDetails SaveExtra(string name);
    }
}
