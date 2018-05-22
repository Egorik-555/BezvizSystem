using BezvizSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Interfaces
{
    public interface IOperatorManager : IDisposable
    {
        IEnumerable<OperatorProfile> GetAll();
        OperatorProfile GetById(string id);
        OperatorProfile Create(OperatorProfile item);
        OperatorProfile Delete(OperatorProfile item);
        OperatorProfile Update(OperatorProfile item);
    }
}
