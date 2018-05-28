using BezvizSystem.BLL.DTO.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IDictionaryService<T> : IDisposable where T: DictionaryDTO
    {
        IEnumerable<T> Get();
    }
}
