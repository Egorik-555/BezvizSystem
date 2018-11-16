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

        int Count();
        int ExtraCount();

        Task<OperationDetails> SaveNew(string name, SaveOptions options);
        Task<OperationDetails> SaveNew(string name);

        Task<OperationDetails> SaveExtra(string name, SaveOptions options);
        Task<OperationDetails> SaveExtra(string name);
    }
}
