using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.StateVisitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Interfaces
{
    public interface IRepositoryXMLDispatch<T, TKey> : IDisposable where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(TKey id);
        Task<T> GetByIdAsync(TKey id);
        T Create(T item);
        T Update(T item);
        T Delete(TKey id);

        Task<IVisitorState> GetStateByIdRecordAsync(TKey id);
    }  
}
