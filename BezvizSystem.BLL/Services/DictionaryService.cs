using AutoMapper;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Mapper;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Entities.Log;
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
        IMapper _mapper;

        public DictionaryService(IUnitOfWork uow)
        {
            Database = uow;

            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FromDALToBLLProfile(Database));
            });
            _mapper = config.CreateMapper();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<T> Get()
        {
            if (typeof(T).Name == "StatusDTO")
            {
                return (IEnumerable<T>)_mapper.Map<IEnumerable<Status>, IEnumerable<StatusDTO>>(Database.StatusManager.GetAll().Where(s => s.Active));
            }
            else if (typeof(T).Name == "NationalityDTO")
            {
                return (IEnumerable<T>)_mapper.Map<IEnumerable<Nationality>, IEnumerable<NationalityDTO>>(Database.NationalityManager.GetAll().Where(n => n.Active));
            }
            else if (typeof(T).Name == "CheckPointDTO")
            {
                return (IEnumerable<T>)_mapper.Map<IEnumerable<CheckPoint>, IEnumerable<CheckPointDTO>>(Database.CheckPointManager.GetAll().Where(c => c.Active));
            }
            else if (typeof(T).Name == "TypeOfOperationDTO")
            {
                return (IEnumerable<T>)_mapper.Map<IEnumerable<TypeOfOperation>, IEnumerable<TypeOfOperationDTO>>(Database.TypeOfOperations.GetAll().Where(c => c.Active));
            }
            else if (typeof(T).Name == "GenderDTO")
            {
                return (IEnumerable<T>)_mapper.Map<IEnumerable<Gender>, IEnumerable<GenderDTO>>(Database.Genders.GetAll().Where(c => c.Active));
            }
            else return null;
        }      
    }
}
