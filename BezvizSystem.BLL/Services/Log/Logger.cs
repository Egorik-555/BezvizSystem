using AutoMapper;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces.Log;
using BezvizSystem.DAL.Helpers;
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
        private IMapper _mapper;

        public Logger(IUnitOfWork uow)
        {
            _base = uow;
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperLogLProfile()));
            _mapper = config.CreateMapper();
        }

        public IEnumerable<LogDTO> GetAll()
        {
            var logs = _base.LogManager.GetAll();
            var model = _mapper.Map<IEnumerable<DAL.Entities.Log>,IEnumerable<LogDTO>>(logs);
            return model;
        }

        public LogDTO GetById(int id)
        {
            var log = _base.LogManager.GetById(id);
            var model = _mapper.Map<BezvizSystem.DAL.Entities.Log, LogDTO>(log);
            return model;
        }

        public async Task<LogDTO> GetByIdAsync(int id)
        {
            var log = await _base.LogManager.GetByIdAsync(id);
            var model = _mapper.Map<BezvizSystem.DAL.Entities.Log, LogDTO>(log);
            return model;
        }

        public IEnumerable<LogDTO> GetByUserName(string name)
        {
            var logs = _base.LogManager.GetAll().Where(l => l.UserName.ToUpper() == name.ToUpper());
            var model = _mapper.Map<IEnumerable<DAL.Entities.Log>, IEnumerable<LogDTO>>(logs);
            return model;
        }

        public IEnumerable<LogDTO> GetForRole(UserLevel role)
        {
            var logs = _base.LogManager.GetAll().AsQueryable();

            logs = logs.Where(l => l.UserRole > role);

            return _mapper.Map<IEnumerable<DAL.Entities.Log>, IEnumerable<LogDTO>>(logs);
        }

        public OperationDetails WriteLog(LogDTO log)
        {
            try
            {
                var model = _mapper.Map<LogDTO, BezvizSystem.DAL.Entities.Log>(log);          
                var newLog = _base.LogManager.Create(model);
                return new OperationDetails(true, "Лог добавлен", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }
    }
}
