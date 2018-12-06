using BezvizSystem.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IService<T> : IDisposable where T : class
    {
        Task<OperationDetails> Create(T visitor);
        Task<OperationDetails> Delete(int id);
        Task<OperationDetails> Update(T visitor);

        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetForUserAsync(string username);
        IEnumerable<T> GetForUser(string username);
    }
}
