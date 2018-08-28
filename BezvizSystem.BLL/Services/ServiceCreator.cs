using BezvizSystem.BLL.DTO;
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
            IdentityUnitOfWork uow = new IdentityUnitOfWork(connection);
            return new VisitorService(uow);
        }

        public IService<GroupVisitorDTO> CreateGroupService(string connection)
        {
            IdentityUnitOfWork uow = new IdentityUnitOfWork(connection);
            return new GroupService(uow);
        }

        public IService<AnketaDTO> CreateAnketaService(string connection)
        {
            return new AnketaService(new IdentityUnitOfWork(connection));
        }  

        public IDictionaryService<NationalityDTO> CreateNationalityService(string connection)
        {
            return new DictionaryService<NationalityDTO>(new IdentityUnitOfWork(connection));
        }

        public IDictionaryService<CheckPointDTO> CreateCheckPointService(string connection)
        {
            return new DictionaryService<CheckPointDTO>(new IdentityUnitOfWork(connection));
        }

        public IDictionaryService<GenderDTO> CreateGenderService(string connection)
        {
            return new DictionaryService<GenderDTO>(new IdentityUnitOfWork(connection));
        }

        public IReport CreateReport(string connection)
        {
            return new ReportService(new IdentityUnitOfWork(connection));
        }
    }
}
