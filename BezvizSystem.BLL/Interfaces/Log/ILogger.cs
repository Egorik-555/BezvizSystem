using BezvizSystem.BLL.DTO.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces.Log
{
    interface ILogger
    {
        void WriteLog(LogDTO log);
        LogDTO Read();
    }
}
