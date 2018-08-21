using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IXMLDispatcher
    {
        Task<OperationDetails> New(Visitor visitor);
        Task<List<OperationDetails>> New(ICollection<Visitor> visitors);
        Task<OperationDetails> Send(Visitor visitor);
        Task<List<OperationDetails>> Send(ICollection<Visitor> visitors);
        Task<OperationDetails> Recd(Visitor visitor);
        Task<List<OperationDetails>> Recd(ICollection<Visitor> visitors);
        Task<OperationDetails> Remove(Visitor visitor);
        Task<List<OperationDetails>> Remove(ICollection<Visitor> visitors);
        Task<OperationDetails> Edit(Visitor visitor);
        Task<List<OperationDetails>> Edit(ICollection<Visitor> oldVisitors, ICollection<Visitor> newVisitors);
    }
}
