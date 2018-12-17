using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.DTO.Report;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.DAL;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Mapper
{
    class FromDALToBLLProfile : Profile
    {
        IMapper mapperVisitor;
        IUnitOfWork _database;
        IXMLDispatcher _xmlDispatcher;

        public FromDALToBLLProfile(IUnitOfWork database)
        {
            _database = database;
            _xmlDispatcher = new XMLDispatcher(database);
            mapperVisitor = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfileWithModelVisitor(database))).CreateMapper();

            //anketa service
            CreateMap<GroupVisitor, AnketaDTO>().
                ForMember(dest => dest.CountMembers, opt => opt.MapFrom(src => src.Visitors.Count())).
                ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival)).
                ForMember(dest => dest.Status, opt => opt.MapFrom(src => CheckAllStatuses(src.Visitors))).
                ForMember(dest => dest.Operator, opt => opt.MapFrom(src => src.TranscriptUser)).
                ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => src.CheckPoint.Name)).
                ForMember(dest => dest.Arrived, opt => opt.MapFrom(src => CheckAllArrivals(src.Visitors)));

            //visitors
            CreateMap<Visitor, VisitorDTO>().
               ForMember(dest => dest.Group, opt => opt.Ignore()).
               //ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality)).
               ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.Name));

            CreateMap<VisitorDTO, Visitor>().
                //ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => database.Nationalities.GetAll().SingleOrDefault(n => n.Name == src.Nationality))).
                ForMember(dest => dest.Gender, opt => opt.MapFrom(src => database.Genders.GetAll().SingleOrDefault(n => n.Name == src.Gender)));
            /////

            //group service
            CreateMap<GroupVisitorDTO, GroupVisitor>().
                     ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => database.CheckPoints.GetAll().SingleOrDefault(n => n.Name == src.CheckPoint)));

            CreateMap<GroupVisitor, GroupVisitorDTO>().
                ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => src.CheckPoint.Name));

            //User
            CreateMap<ProfileUserDTO, OperatorProfile>();
            CreateMap<OperatorProfile, ProfileUserDTO>();

            CreateMap<UserDTO, BezvizUser>().
                ForMember(dest => dest.OperatorProfile, opt => opt.MapFrom(src => src.ProfileUser)).
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => new BezvizUser().Id));

            CreateMap<BezvizUser, UserDTO>().
                ForMember(dest => dest.ProfileUser, opt => opt.MapFrom(src => src.OperatorProfile));
            ////

            CreateMap<Nationality, NationalityDTO>();
            CreateMap<CheckPoint, CheckPointDTO>();
            CreateMap<Gender, GenderDTO>();         
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

        private string CheckAllStatuses(IEnumerable<Visitor> list)
        {
            int countNew = 0;
            int countSend = 0;
            int countRecieve = 0;

            foreach (var item in list)
            {
                var itemInDispatch = _database.XMLDispatchManager.GetById(item.Id);

                if (itemInDispatch != null)
                {
                    if (itemInDispatch.Status == Status.New) countNew++;
                    else if (itemInDispatch.Status == Status.Send) countSend++;
                    else if (itemInDispatch.Status == Status.Recd) countRecieve++;
                }
            }

            if (countSend == list.Count())
            {
                return "Передано в пограничную службу";
            }
            else if (countRecieve == list.Count())
            {
                return "Принято пограничной службой";
            }
            else return "Сохранено";
        }
    }

    class FromDALToBLLProfileWithModelVisitor : Profile
    {

        public FromDALToBLLProfileWithModelVisitor(IUnitOfWork _database)
        {
            CreateMap<VisitorDTO, Visitor>().
               ForMember(dest => dest.Group, opt => opt.MapFrom(src => _database.GroupManager.GetById(src.Group.Id))).
               //ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => _database.Nationalities.GetAll().Where(n => n.Name == src.Nationality).FirstOrDefault())).
               ForMember(dest => dest.Gender, opt => opt.MapFrom(src => _database.Genders.GetAll().Where(n => n.Name == src.Gender).FirstOrDefault()));

            CreateMap<Visitor, VisitorDTO>().
               ForMember(dest => dest.Group, opt => opt.Ignore());
               //ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name));
        }

        public FromDALToBLLProfileWithModelVisitor(IUnitOfWork _database, Visitor model)
        {
            CreateMap<VisitorDTO, Visitor>().ConstructUsing(v => model).
               ForMember(dest => dest.Group, opt => opt.Ignore()).
               //ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => _database.Nationalities.GetAll().Where(n => n.Name == src.Nationality).FirstOrDefault())).
               ForMember(dest => dest.Gender, opt => opt.MapFrom(src => _database.Genders.GetAll().Where(n => n.Name == src.Gender).FirstOrDefault()));

            CreateMap<Visitor, VisitorDTO>().
              ForMember(dest => dest.Group, opt => opt.Ignore());
              //ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name));
        }
    }

    class ProfileGroupDtoToDao : Profile
    {
        IMapper mapperVisitor;
        IUnitOfWork _database;

        public ProfileGroupDtoToDao(IUnitOfWork _database, GroupVisitor model)
        {
            this._database = _database;

            CreateMap<GroupVisitorDTO, GroupVisitor>().ConstructUsing(v => model).
                ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => _database.CheckPoints.GetAll().SingleOrDefault(n => n.Name == src.CheckPoint))).
                ForMember(d => d.Visitors, opt => opt.MapFrom(src => GetVisitors(model.Visitors, src.Visitors)));
        }

        private IEnumerable<Visitor> GetVisitors(IEnumerable<Visitor> oldModel, IEnumerable<VisitorDTO> newModel)
        {
            List<Visitor> result = new List<Visitor>();

            foreach (var newItem in newModel)
            {
                bool isOld = false;

                foreach (var oldItem in oldModel)
                {
                    //если в новом наборе туристов есть старые туристы
                    if (oldItem.Id == newItem.Id)
                    {
                        mapperVisitor = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfileWithModelVisitor(_database, oldItem))).CreateMapper();
                        result.Add(mapperVisitor.Map<VisitorDTO, Visitor>(newItem));
                        isOld = true;
                        break;
                    }
                }

                //если турист новый
                if (!isOld)
                {
                    mapperVisitor = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfileWithModelVisitor(_database))).CreateMapper();
                    result.Add(mapperVisitor.Map<VisitorDTO, Visitor>(newItem));
                }

            }

            return result;
        }
    }

    class VisitorComparer : IEqualityComparer<Visitor>
    {
        public bool Equals(Visitor x, Visitor y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Visitor obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class FromDALToBLLProfileWithModelUser : Profile
    {
        public FromDALToBLLProfileWithModelUser(BezvizUser model)
        {
            CreateMap<UserDTO, BezvizUser>().ConstructUsing(v => model).
                        ForMember(dest => dest.OperatorProfile, opt => opt.MapFrom(src => src.ProfileUser));

            CreateMap<ProfileUserDTO, OperatorProfile>().ConstructUsing(v => model.OperatorProfile);
        }
    }
}
