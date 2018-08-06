﻿using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.DAL;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Entities.Log;
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
        IUnitOfWork _database;

        public FromDALToBLLProfile(IUnitOfWork database)
        {
            _database = database;

            //anketa service
            CreateMap<GroupVisitor, AnketaDTO>().
               ForMember(dest => dest.CountMembers, opt => opt.MapFrom(src => src.Visitors.Count())).
               ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival)).
               ForMember(dest => dest.Status, opt => opt.MapFrom(src => CheckAllStatuses(src.Visitors))).
               ForMember(dest => dest.Operator, opt => opt.MapFrom(src => src.User.OperatorProfile.Transcript)).
               ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => src.CheckPoint.Name)).
               ForMember(dest => dest.Arrived, opt => opt.MapFrom(src => CheckAllArrivals(src.Visitors)));

            CreateMap<Visitor, VisitorDTO>().
               ForMember(dest => dest.Group, opt => opt.Ignore()).
               ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality.Name));
            /////

            //group service
            CreateMap<GroupVisitorDTO, GroupVisitor>().
                     ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => database.CheckPointManager.GetAll().Where(n => n.Name == src.CheckPoint).FirstOrDefault()));
            CreateMap<VisitorDTO, Visitor>().
                ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => database.NationalityManager.GetAll().Where(n => n.Name == src.Nationality).FirstOrDefault())).
                ForMember(dest => dest.Gender, opt => opt.MapFrom(src => database.Genders.GetAll().Where(n => n.Name == src.Gender).FirstOrDefault())).
                ForMember(dest => dest.Status, opt => opt.MapFrom(src => database.StatusManager.GetAll().Where(n => n.Name == src.StatusName).FirstOrDefault()));

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

            CreateMap<Status, StatusDTO>();
            CreateMap<Nationality, NationalityDTO>();
            CreateMap<CheckPoint, CheckPointDTO>();
            CreateMap<TypeOfOperation, TypeOfOperationDTO>();
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
            var statuses = _database.StatusManager.GetAll().ToList();
            var save = statuses.Where(s => s.Code == 1).FirstOrDefault();
            var send = statuses.Where(s => s.Code == 2).FirstOrDefault();
            var recieve = statuses.Where(s => s.Code == 3).FirstOrDefault();

            int countSave = 0;
            int countSend = 0;
            int countRecieve = 0;

            foreach (var item in list)
            {
                if (item.Status.Code == 1)
                {
                    countSave++;
                }
                else if (item.Status.Code == 2)
                {
                    countSend++;
                }
                else if (item.Status.Code == 3)
                {
                    countRecieve++;
                }
            }

            if (countSave > 0 && countSave <= list.Count())
            {
                return save.Name;
            }
            else if (countSend > 0 && countSend <= list.Count())
            {
                return send.Name;
            }
            else return recieve.Name;
        }
    }

    class FromDALToBLLProfileWithModelVisitor : Profile
    {
        public FromDALToBLLProfileWithModelVisitor(IUnitOfWork _database, Visitor model)
        {
            CreateMap<VisitorDTO, Visitor>().ConstructUsing(v => model).
                    ForMember(dest => dest.Group, opt => opt.Ignore()).
                    ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => _database.NationalityManager.GetAll().Where(n => n.Name == src.Nationality).FirstOrDefault())).
                    ForMember(dest => dest.Gender, opt => opt.MapFrom(src => _database.Genders.GetAll().Where(n => n.Name == src.Gender).FirstOrDefault()));
        }
    }

    class FromDALToBLLProfileWithModelGroup : Profile
    {
        IMapper mapperVisitor;

        public FromDALToBLLProfileWithModelGroup(IUnitOfWork _database, GroupVisitor model)
        {
            mapperVisitor = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfile(_database))).CreateMapper();

            CreateMap<GroupVisitorDTO, GroupVisitor>().ConstructUsing(v => model).
                ForMember(dest => dest.CheckPoint, opt => opt.MapFrom(src => _database.CheckPointManager.GetAll().Where(n => n.Name == src.CheckPoint).FirstOrDefault())).
                ForMember(d => d.Visitors, opt => opt.Ignore()).
                AfterMap((d, e) => AddOrUpdateVisitors(d, e));
        }

        private void AddOrUpdateVisitors(GroupVisitorDTO dto, GroupVisitor group)
        {

            foreach(var visitor in group.Visitors)
            {
                if(visitor.Status.Code != 1)
                {
                    if(dto.Visitors.SingleOrDefault(v => v.Id == visitor.Id) == null)
                    {

                    }

                }
            }

            foreach (var visitorDTO in dto.Visitors)
            {
                visitorDTO.Group = dto;

                if (visitorDTO.Id == 0)
                {
                    visitorDTO.StatusOfOperation = StatusOfOperation.Add;
                    visitorDTO.StatusName = "Сохранено";
                    visitorDTO.DateInSystem = DateTime.Now;
                    visitorDTO.UserInSystem = dto.UserInSystem;
                    group.Visitors.Add(mapperVisitor.Map<Visitor>(visitorDTO));
                }
                else
                {
                    var oldVisitor = group.Visitors.SingleOrDefault(v => v.Id == visitorDTO.Id);
                    var newVisitor = mapperVisitor.Map<VisitorDTO, Visitor>(visitorDTO);
                                                                        
                    if (!oldVisitor.Equals(newVisitor))
                    {
                        if (oldVisitor.Status.Code != 1)
                            visitorDTO.StatusOfOperation = StatusOfOperation.Edit;
                        else
                            visitorDTO.StatusOfOperation = StatusOfOperation.Add;

                        visitorDTO.StatusName = "Сохранено";
                        visitorDTO.DateEdit = dto.DateEdit;
                        visitorDTO.UserEdit = dto.UserEdit;
                    }
                    
                    mapperVisitor.Map(visitorDTO, oldVisitor);
                }
            }
        }
    }

    class FromDALToBLLProfileWithModelUser : Profile
    {
        public FromDALToBLLProfileWithModelUser(BezvizUser model)
        {
            CreateMap<UserDTO, BezvizUser>().ConstructUsing(v => model).
                        ForMember(dest => dest.OperatorProfile, opt => opt.MapFrom(src => src.ProfileUser));

            CreateMap<ProfileUserDTO, OperatorProfile>().ConstructUsing(v => model.OperatorProfile);
        }
    }
}
