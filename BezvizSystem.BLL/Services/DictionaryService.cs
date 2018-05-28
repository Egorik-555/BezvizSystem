using AutoMapper;
using BezvizSystem.BLL.DTO.Dictionary;
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
    public class DictionaryService<T> : IDictionaryService<T> where T : DictionaryDTO
    {
        IUnitOfWork Database;
        IMapper mapper;

        public DictionaryService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Status, StatusDTO>();
                cfg.CreateMap<Nationality, NationalityDTO>();
                cfg.CreateMap<CheckPoint, CheckPointDTO>();

            }).CreateMapper();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<T> Get()
        {
            if (typeof(T).Name == "StatusDTO")
            {
                return (IEnumerable<T>)mapper.Map<IEnumerable<Status>, IEnumerable<StatusDTO>>(Database.StatusManager.GetAll());
            }
            else if (typeof(T).Name == "NationalityDTO")
            {
                return (IEnumerable<T>)mapper.Map<IEnumerable<Nationality>, IEnumerable<NationalityDTO>>(Database.NationalityManager.GetAll());
            }
            else if (typeof(T).Name == "CheckPointDTO")
            {
                return (IEnumerable<T>)mapper.Map<IEnumerable<CheckPoint>, IEnumerable<CheckPointDTO>>(Database.CheckPointManager.GetAll());
            }
            else return null;
        }      
    }
}
