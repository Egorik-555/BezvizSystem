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
        Task<OperationDetails> New(int IdVisitor);
        Task<OperationDetails> Send(int IdVisitor);
        Task<OperationDetails> Recd(int IdVisitor);
        Task<OperationDetails> Remove(int IdVisitor);
        Task<OperationDetails> Edit(int IdVisitor);      
    }
}
