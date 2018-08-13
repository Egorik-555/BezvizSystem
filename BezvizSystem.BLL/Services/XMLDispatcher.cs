using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Services
{
    public class XMLDispatcher : IXMLDispatcher
    {
        IUnitOfWork _database;

        public XMLDispatcher(IUnitOfWork uow)
        {
            _database = uow;
        }

        public Task<OperationDetails> New(VisitorDTO visitor, XMLDispatchDTO dispatch)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> Remove(VisitorDTO visitor, XMLDispatchDTO dispatch)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> Update(VisitorDTO visitor, XMLDispatchDTO dispatch)
        {
            throw new NotImplementedException();
        }
    }
}
