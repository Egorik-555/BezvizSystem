using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Services
{
    public class VisitorService : IService<VisitorDTO>
    {
        IUnitOfWork Database;
        IMapper mapper;


        public VisitorService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VisitorDTO, Visitor>().
                    ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => Database.NationalityManager.GetAll().Where(n => n.Name == src.Nationality).FirstOrDefault()));
                cfg.CreateMap<Visitor, VisitorDTO>().ForMember(dest => dest.Group, opt => opt.Ignore()).
                    ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name));


            }).CreateMapper();
        }

        public async Task<OperationDetails> Create(VisitorDTO visitor)
        {
            try
            {              
                var model = mapper.Map<VisitorDTO, Visitor>(visitor);               
                Database.VisitorManager.Create(model);
                return new OperationDetails(true, "Турист создан", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public async Task<OperationDetails> Delete(int id)
        {
            try
            {
                var visitor = await Database.VisitorManager.GetByIdAsync(id);
                if (visitor != null)
                {
                    Database.VisitorManager.Delete(visitor.Id);
                    return new OperationDetails(true, "Турист удален", "");
                }
                else return new OperationDetails(false, "Турист не найден", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public async Task<OperationDetails> Update(VisitorDTO visitor)
        {
            try
            {
                var model = await Database.VisitorManager.GetByIdAsync(visitor.Id);
                if (model != null)
                {                  
                    var mapper = new MapperConfiguration(cfg => 
                    {
                        cfg.CreateMap<VisitorDTO, Visitor>().ConstructUsing(v => model).
                            ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => Database.NationalityManager.GetAll().Where(n => n.Name == src.Nationality).FirstOrDefault()));

                    }).CreateMapper();

                    var m = mapper.Map<VisitorDTO, Visitor>(visitor);
                    Database.VisitorManager.Update(m);
                    return new OperationDetails(true, "Турист изменен", "");
                }
                else return new OperationDetails(false, "Турист не найден", "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<VisitorDTO> GetAll()
        {
            var visitors = Database.VisitorManager.GetAll().AsQueryable().Include(v => v.Group).ToList();
            var visitorsDto = mapper.Map<IEnumerable<Visitor>, IEnumerable<VisitorDTO>>(visitors);
            return visitorsDto;
        }

        public async Task<VisitorDTO> GetByIdAsync(int id)
        {
            var visitor = await Database.VisitorManager.GetByIdAsync(id);           
            var visitorDto = mapper.Map<Visitor, VisitorDTO>(visitor);
            return visitorDto;
        }

        public VisitorDTO GetById(int id)
        {
            var visitor = Database.VisitorManager.GetById(id);
            var visitorDto = mapper.Map<Visitor, VisitorDTO>(visitor);
            return visitorDto;
        }

        public Task<IEnumerable<VisitorDTO>> GetForUserAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}
