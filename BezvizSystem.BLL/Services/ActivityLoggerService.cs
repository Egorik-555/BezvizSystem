﻿using AutoMapper;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Entities.Log;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Services
{
    class ActivityLoggerService : ILogger<UserActivityDTO>
    {
        IUnitOfWork Database;
        IMapper mapper;

        public ActivityLoggerService(IUnitOfWork db)
        {
            Database = db;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserActivity, UserActivityDTO>();
                cfg.CreateMap<UserActivityDTO, UserActivity>();

            }).CreateMapper();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<UserActivityDTO> GetAll()
        {
            return mapper.Map<IEnumerable<UserActivity>, IEnumerable<UserActivityDTO>>(Database.UserActivities.GetAll());
        }

        public IEnumerable<UserActivityDTO> GetByLoginAsync(string login)
        {
            var source = Database.UserActivities.GetAll().Where(s => s.Login.ToUpper() == login.ToUpper());
            return mapper.Map<IEnumerable<UserActivity>, IEnumerable<UserActivityDTO>>(source);
        }

        public OperationDetails Insert(UserActivityDTO item)
        {
            try
            {
                var model = mapper.Map<UserActivityDTO, UserActivity>(item);
                var result = Database.UserActivities.Create(model);
                return new OperationDetails(true, "Запись добавлена", "");
            }
            catch(Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }
    }
}
