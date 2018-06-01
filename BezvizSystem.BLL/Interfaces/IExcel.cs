using BezvizSystem.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IExcel
    {
        string InExcel<T>(IEnumerable<T> list);
        Task<string> InExcelAsync<T>(IEnumerable<T> list);
    }
}
