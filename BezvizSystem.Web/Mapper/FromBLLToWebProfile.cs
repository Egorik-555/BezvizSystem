﻿using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Report;
using BezvizSystem.BLL.Report.DTO;
using BezvizSystem.Web.Models.Anketa;
using BezvizSystem.Web.Models.Group;
using BezvizSystem.Web.Models.Mark;
using BezvizSystem.Web.Models.Operator;
using BezvizSystem.Web.Models.Report;
using BezvizSystem.Web.Models.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Mapper
{
    public class FromBLLToWebProfile : Profile
    {
        public FromBLLToWebProfile()
        {
            RecognizePrefixes("ProfileUser");
            CreateMap<CreateOperatorModel, ProfileUserDTO>();
            CreateMap<EditOperatorModel, ProfileUserDTO>();
            CreateMap<DeleteOperatorModel, ProfileUserDTO>();
            

            CreateMap<UserDTO, ViewOperatorModel>();
            CreateMap<UserDTO, EditOperatorModel>();
            CreateMap<UserDTO, DeleteOperatorModel>();


            CreateMap<CreateOperatorModel, UserDTO>();
            CreateMap<EditOperatorModel, UserDTO>();
            CreateMap<DeleteOperatorModel, UserDTO>();

            CreateMap<BLL.DTO.Report.NatAndAge, Web.Models.Report.NatAndAge>();
            CreateMap<BLL.DTO.Report.CountByOperator, Web.Models.Report.CountByOperator>();
            CreateMap<ReportDTO, ReportModel>();
            CreateMap<ReportModel, ReportDTO>();

            IMapper visitorMapper = new MapperConfiguration(cfg => cfg.AddProfile(new VisitorProfile())).CreateMapper();

            CreateMap<CreateVisitorModel, GroupVisitorDTO>().ForMember(dest => dest.Visitors, opt => opt.MapFrom(
                    src => new List<VisitorDTO> { visitorMapper.Map<InfoVisitorModel, VisitorDTO>(src.Info) }));
            CreateMap<GroupVisitorDTO, CreateVisitorModel>();
            CreateMap<InfoVisitorModel, VisitorDTO>();
            CreateMap<EditInfoVisitorModel, VisitorDTO>();
            CreateMap<CreateGroupModel, GroupVisitorDTO>().
                    ForMember(dest => dest.Visitors, opt => opt.MapFrom(src => src.Infoes));

            CreateMap<VisitorDTO, InfoVisitorModel>();
            CreateMap<VisitorDTO, EditInfoVisitorModel>();
            CreateMap<AnketaDTO, ViewAnketaModel>().
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival));
            CreateMap<GroupVisitorDTO, EditVisitorModel>().
                    ForMember(dest => dest.Info, opt => opt.MapFrom(src =>
                                        visitorMapper.Map<VisitorDTO, EditInfoVisitorModel>(src.Visitors.FirstOrDefault()))).
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival));
            CreateMap<EditVisitorModel, GroupVisitorDTO>().
                    ForMember(dest => dest.Visitors, opt => opt.MapFrom(src =>
                        new List<VisitorDTO> { visitorMapper.Map<EditInfoVisitorModel, VisitorDTO>(src.Info) }));

            CreateMap<GroupVisitorDTO, EditGroupModel>().
                    ForMember(dest => dest.Infoes, opt => opt.MapFrom(src => src.Visitors));
            CreateMap<EditGroupModel, GroupVisitorDTO>().
                       ForMember(dest => dest.Visitors, opt => opt.MapFrom(src => src.Infoes));

            CreateMap<GroupVisitorDTO, ViewAnketaModel>().
                    ForMember(dest => dest.CountMembers, opt => opt.MapFrom(src => src.Visitors.Count()));
            CreateMap<ViewAnketaModel, GroupVisitorDTO>();
            CreateMap<AnketaDTO, ViewAnketaExcel>();

            CreateMap<AnketaDTO, ViewMarkModel>().
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival));
            CreateMap<VisitorDTO, ViewVisitorModel>().
                    ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.Group.Id));

            CreateMap<CountByDate, ObjectForDiagram>().
                    ForMember(dest => dest.Value1, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date.ToString("dd.MM.yyyy") : null)).
                    ForMember(dest => dest.Value2, opt => opt.MapFrom(src => src.Count));
        }
    }

    public class VisitorProfile : Profile
    {
        public VisitorProfile()
        {
            CreateMap<InfoVisitorModel, VisitorDTO>();
            CreateMap<VisitorDTO, InfoVisitorModel>();

            CreateMap<EditInfoVisitorModel, VisitorDTO>();
            CreateMap<VisitorDTO, EditInfoVisitorModel>();

        }
    }

}