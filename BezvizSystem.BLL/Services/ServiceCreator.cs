﻿using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO.Dictionary;

namespace BezvizSystem.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public BezvizContext CreateContext(string connection)
        {
            return new BezvizContext(connection);
        }

        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }

        public IService<VisitorDTO> CreateVisitorService(string connection)
        {
            return new VisitorService(new IdentityUnitOfWork(connection));
        }

        public IService<GroupVisitorDTO> CreateGroupService(string connection)
        {
            return new GroupService(new IdentityUnitOfWork(connection));
        }

        public IService<AnketaDTO> CreateAnketaService(string connection)
        {
            return new AnketaService(new IdentityUnitOfWork(connection));
        }

        public IDictionaryService<StatusDTO> CreateStatusService(string connection)
        {
            return new DictionaryService<StatusDTO>(new IdentityUnitOfWork(connection));
        }

        public IDictionaryService<NationalityDTO> CreateNationalityService(string connection)
        {
            return new DictionaryService<NationalityDTO>(new IdentityUnitOfWork(connection));
        }

        public IDictionaryService<CheckPointDTO> CreateCheckPointService(string connection)
        {
            return new DictionaryService<CheckPointDTO>(new IdentityUnitOfWork(connection));
        }
    }
}
