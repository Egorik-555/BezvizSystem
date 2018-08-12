using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.DAL;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using System;
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

        public FromDALToBLLProfile(IUnitOfWork database)
        {
            _database = database;
            mapperVisitor = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfileWithModelVisitor(database, null))).CreateMapper();

            //anketa service
            CreateMap<GroupVisitor, AnketaDTO>().
                ForMember(dest => dest.CountMembers, opt => opt.MapFrom(src => src.Visitors.Count())).
                ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival)).
                ForMember(dest => dest.Status, opt => opt.MapFrom(src => CheckAllStatuses(src.Visitors))).
                ForMember(dest => dest.Operator, opt => opt.MapFrom(src => src.TranscriptUser)).
                ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => src.CheckPoint.Name)).
                ForMember(dest => dest.Arrived, opt => opt.MapFrom(src => CheckAllArrivals(src.Visitors)));//.
               // AfterMap((d, e) => RemovedOrNotVisitors(d, e));

            CreateMap<Visitor, VisitorDTO>().
               ForMember(dest => dest.Group, opt => opt.Ignore()).
               ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name));
            /////

            //group service
            CreateMap<GroupVisitorDTO, GroupVisitor>().
                     ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => database.CheckPoints.GetAll().SingleOrDefault(n => n.Name == src.CheckPoint)));

            CreateMap<VisitorDTO, Visitor>().
                ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => database.Nationalities.GetAll().SingleOrDefault(n => n.Name == src.Nationality))).
                ForMember(dest => dest.Gender, opt => opt.MapFrom(src => database.Genders.GetAll().SingleOrDefault(n => n.Name == src.Gender)));

            CreateMap<GroupVisitor, GroupVisitorDTO>().
                ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => src.CheckPoint.Name));
            CreateMap<Visitor, VisitorDTO>().
                ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name)).
                ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.Name));
            ///

            //User service
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

            //foreach (var item in list)
            //{
            //    var itemInDispatches = _database.XMLDispatchManager.GetAll().SingleOrDefault(i => i.IdVisitor == item.Id && i.Status == Status.New);
            //}

            //if (countSend == list.Count())
            //{
            //    return StatusOfRecord.Send.ToString();
            //}
            //else if (countRecieve == list.Count())
            //{
            //    return StatusOfRecord.Recd.ToString();
            //}
            //else return StatusOfRecord.New.ToString();

            return "Сохранено";
        }


        private void RemovedOrNotVisitors(GroupVisitor groupVisitor, AnketaDTO anketa)
        {
            ICollection<Visitor> result = new List<Visitor>();
            foreach (var visitor in groupVisitor.Visitors)
            {
                //if (!visitor.IsRemove())
                //{
                //    result.Add(visitor);
                //}
            }

            //mapperVisitor.Map(result, anketa.Visitors);
        }
    }

    class FromDALToBLLProfileWithModelVisitor : Profile
    {
        public FromDALToBLLProfileWithModelVisitor(IUnitOfWork _database, Visitor model)
        {
            CreateMap<VisitorDTO, Visitor>().ConstructUsing(v => model).
                    ForMember(dest => dest.Group, opt => opt.MapFrom(src => _database.GroupManager.GetById(src.Group.Id))).
                    ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => _database.Nationalities.GetAll().Where(n => n.Name == src.Nationality).FirstOrDefault())).
                    ForMember(dest => dest.Gender, opt => opt.MapFrom(src => _database.Genders.GetAll().Where(n => n.Name == src.Gender).FirstOrDefault()));

            CreateMap<Visitor, VisitorDTO>().
               ForMember(dest => dest.Group, opt => opt.Ignore()).
               ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name));
        }
    }

    class FromDALToBLLProfileWithModelGroup : Profile
    {
        IMapper mapperVisitor;
        IUnitOfWork _database;

        public FromDALToBLLProfileWithModelGroup(IUnitOfWork _database, GroupVisitor model)
        {
            this._database = _database;
            mapperVisitor = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfile(_database))).CreateMapper();

            CreateMap<VisitorDTO, Visitor>();

            CreateMap<GroupVisitorDTO, GroupVisitor>().ConstructUsing(v => model).
                ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => _database.CheckPoints.GetAll().SingleOrDefault(n => n.Name == src.CheckPoint))).
                ForMember(d => d.Visitors, opt => opt.MapFrom(src => mapperVisitor.Map<IEnumerable<VisitorDTO>, IEnumerable<Visitor>>(src.Visitors)));
            // AfterMap((d, e) => AddOrUpdateVisitors(d, e));        
        }

        private void AddOrUpdateVisitors(GroupVisitorDTO newGroupDto, GroupVisitor oldGroup)
        {

            var oldVisitors = oldGroup.Visitors.ToList();
            foreach (var visitor in oldVisitors)
            {
                if (newGroupDto.Visitors.SingleOrDefault(v => v.Id == visitor.Id) == null)
                {
                    //    if (visitor.StatusOfRecord != StatusOfRecord.New)
                    //    {
                    //        visitor.StatusOfRecord = StatusOfRecord.Remove;
                    //    }
                    //    else
                    //    {
                    //        oldGroup.Visitors.Remove(visitor);
                    //    }
                    //}
                }

                foreach (var visitorDTO in newGroupDto.Visitors)
                {
                    //visitorDTO.Group = dto;

                    if (visitorDTO.Id == 0)
                    {
                        // visitorDTO.StatusOfRecord = StatusOfRecord.New;
                        visitorDTO.DateInSystem = DateTime.Now;
                        visitorDTO.UserInSystem = newGroupDto.UserInSystem;
                        //oldGroup.Visitors.Add(mapperVisitor.Map<Visitor>(visitorDTO));
                    }
                    else
                    {
                        var oldVisitor = oldGroup.Visitors.SingleOrDefault(v => v.Id == visitorDTO.Id);
                        //var newVisitor = mapperVisitor.Map<VisitorDTO, Visitor>(visitorDTO);
                        //if (visitorDTO.StatusOfRecord == 0)
                        //    visitorDTO.StatusOfRecord = StatusOfRecord.Save;
                        //if (visitorDTO.StatusOfOperation == 0)
                        //    visitorDTO.StatusOfOperation = StatusOfOperation.Add;

                        //if (!oldVisitor.Equals(newVisitor) ||
                        //        (newGroupDto.DateArrival.HasValue && oldGroup.DateArrival.HasValue && newGroupDto.DateArrival.Value != oldGroup.DateArrival.Value))
                        //{
                            //if (oldVisitor.StatusOfRecord != StatusOfRecord.New)
                            //    visitorDTO.StatusOfRecord = StatusOfRecord.Edit;

                            //visitorDTO.DateEdit = newGroupDto.DateEdit;
                            //visitorDTO.UserEdit = newGroupDto.UserEdit;
                       // }

                        //mapperVisitor.Map(visitorDTO, oldVisitor);
                    }
                }
            }
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
