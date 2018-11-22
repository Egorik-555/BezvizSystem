using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces.Log
{
    public interface ILogger
    {
        OperationDetails WriteLog(LogDTO log);
        LogDTO GetById(int id);
        Task<LogDTO> GetByIdAsync(int id);

        IEnumerable<LogDTO> GetByUserName(string name);
        IEnumerable<LogDTO> GetForUserAndRole(string user, UserLevel role);
        IEnumerable<LogDTO> GetAll();
    }
}
