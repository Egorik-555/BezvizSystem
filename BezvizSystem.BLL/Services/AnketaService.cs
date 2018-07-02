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
                    ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => src.CheckPoint.Name)).
                    ForMember(dest => dest.Arrived, opt => opt.MapFrom(src => CheckAllArrivals(src.Visitors)));              

                 cfg.CreateMap<Visitor, VisitorDTO>().
                    ForMember(dest => dest.Group, opt => opt.Ignore()).
                    ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name));
            }
            ).CreateMapper();
        }

        private string CheckAllArrivals(IEnumerable<Visitor> list)
        {
            int count = 0;
            foreach (var item in list)
            {
                if (item.Arrived)
                {
                    count++;
                }
            }

            if (count == list.Count())
                return "V";
            else if (count != 0)
                return "Частично";
            else return "X";
        }

        public IEnumerable<AnketaDTO> GetAll()
        {        
            var groups = Database.GroupManager.GetAll().ToList();         
            var anketaGroup = mapper.Map<IEnumerable<GroupVisitor>, IEnumerable<AnketaDTO>>(groups);
            List<AnketaDTO> result = new List<AnketaDTO>(anketaGroup);
            return result;
        }

        public async Task<IEnumerable<AnketaDTO>> GetForUserAsync(string username)
        {                   
            var user = await Database.UserManager.FindByNameAsync(username);
            if (user != null)
            {             
                var groups = Database.GroupManager.GetAll().ToList();

                if (user.OperatorProfile.Role.ToUpper() != "ADMIN")
                    groups = groups.Where(g => g.User.OperatorProfile.UNP == user.OperatorProfile.UNP).ToList();              

                var anketaGroup = mapper.Map<IEnumerable<GroupVisitor>, IEnumerable<AnketaDTO>>(groups);
                return anketaGroup;
            }
            else return null;
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
