using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Services
{
    public class AnketaService : IService<AnketaDTO>
    {
        IUnitOfWork Database;
        IMapper mapper;

        public AnketaService(IUnitOfWork db)
        {
            Database = db;

            mapper = new MapperConfiguration(cfg =>
            {            
                cfg.CreateMap<GroupVisitor, AnketaDTO>().
                    ForMember(dest => dest.CountMembers, opt => opt.MapFrom(src => src.Visitors.Count())).                  
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival)).
                    ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name)).
                    ForMember(dest => dest.Operator, opt => opt.MapFrom(src => src.User.OperatorProfile.Transcript)).
                    ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => src.CheckPoint.Name));
            }
            ).CreateMapper();
        }

        public IEnumerable<AnketaDTO> GetAll()
        {        
            var groups = Database.GroupManager.GetAll().ToList();         
            var anketaGroup = mapper.Map<IEnumerable<GroupVisitor>, IEnumerable<AnketaDTO>>(groups);
            List<AnketaDTO> result = new List<AnketaDTO>(anketaGroup);
            return result;
        }

        public AnketaDTO GetById(int id)
        {
            var group = Database.GroupManager.GetById(id);
            var anketa = mapper.Map<GroupVisitor, AnketaDTO>(group);
            return anketa;
        }

        public async Task<AnketaDTO> GetByIdAsync(int id)
        {
            var group = await Database.GroupManager.GetByIdAsync(id);
            var anketa = mapper.Map<GroupVisitor, AnketaDTO>(group);
            return anketa;
        }


        public void Dispose()
        {
            Database.Dispose();
        }

        public Task<OperationDetails> Update(AnketaDTO visitor)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> Create(AnketaDTO visitor)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
