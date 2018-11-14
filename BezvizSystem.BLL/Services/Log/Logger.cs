using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Interfaces.Log;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Services.Log
{
    public class Logger : ILogger
    {
        private IUnitOfWork _base;

        public Logger(IUnitOfWork uow)
        {
            _base = uow;
              


        }

        public LogDTO Read()
        {
            throw new NotImplementedException();
        }

        public void WriteLog(LogDTO log)
        {
            throw new NotImplementedException();
        }
    }
}
