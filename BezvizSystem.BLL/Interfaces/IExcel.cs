using BezvizSystem.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IExcel<TOutput>
    {
        TOutput InExcel<T>(IEnumerable<T> list);
        Task<TOutput> InExcelAsync<T>(IEnumerable<T> list);
    }
}
