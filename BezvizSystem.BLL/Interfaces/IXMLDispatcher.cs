using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IXMLDispatcher
    {

        Task<OperationDetails> New(VisitorDTO visitor, XMLDispatchDTO dispatch);
        Task<OperationDetails> Update(VisitorDTO visitor, XMLDispatchDTO dispatch);
        Task<OperationDetails> Remove(VisitorDTO visitor, XMLDispatchDTO dispatch);      

    }
}
