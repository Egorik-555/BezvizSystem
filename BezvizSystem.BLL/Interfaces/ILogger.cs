using BezvizSystem.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface ILogger<T> : IDisposable where T : class
    {
        OperationDetails Insert(T item);

        IEnumerable<T> GetByLoginAsync(string login);
        IEnumerable<T> GetAll();
    }
}
