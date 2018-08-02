using AutoMapper;
using BezvizSystem.BLL.DTO;
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

            CreateMap<ReportDTO, ReportModel>();
            CreateMap<ReportModel, ReportDTO>();

            IMapper visitorMapper = new MapperConfiguration(cfg => cfg.AddProfile(new VisitorProfile())).CreateMapper();
            CreateMap<CreateVisitorModel, GroupVisitorDTO>().ForMember(dest => dest.Visitors, opt => opt.MapFrom(
                    src => new List<VisitorDTO> { visitorMapper.Map<InfoVisitorModel, VisitorDTO>(src.Info) }));
            CreateMap<GroupVisitorDTO, CreateVisitorModel>();
            CreateMap<InfoVisitorModel, VisitorDTO>();
            CreateMap<CreateGroupModel, GroupVisitorDTO>().
                    ForMember(dest => dest.Visitors, opt => opt.MapFrom(src => src.Infoes));

            CreateMap<VisitorDTO, InfoVisitorModel>();
            CreateMap<AnketaDTO, ViewAnketaModel>().
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival));
            CreateMap<GroupVisitorDTO, EditVisitorModel>().
                    ForMember(dest => dest.Info, opt => opt.MapFrom(src =>
                                        visitorMapper.Map<IEnumerable<VisitorDTO>, IEnumerable<InfoVisitorModel>>(src.Visitors).FirstOrDefault())).
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival));
            CreateMap<EditVisitorModel, GroupVisitorDTO>().
                    ForMember(dest => dest.Visitors, opt => opt.MapFrom(src =>
                        new List<VisitorDTO> { visitorMapper.Map<InfoVisitorModel, VisitorDTO>(src.Info) }));
            CreateMap<GroupVisitorDTO, EditGroupModel>().
                    ForMember(dest => dest.Infoes, opt => opt.MapFrom(src => src.Visitors));
            CreateMap<EditGroupModel, GroupVisitorDTO>().
                       ForMember(dest => dest.Visitors, opt => opt.MapFrom(src => src.Infoes));

            CreateMap<InfoVisitorModel, VisitorDTO>();
            CreateMap<GroupVisitorDTO, ViewAnketaModel>().
                    ForMember(dest => dest.CountMembers, opt => opt.MapFrom(src => src.Visitors.Count()));
            CreateMap<ViewAnketaModel, GroupVisitorDTO>();
            CreateMap<AnketaDTO, ViewAnketaExcel>();

            CreateMap<AnketaDTO, ViewMarkModel>().
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival));
            CreateMap<VisitorDTO, ViewVisitorModel>().
                    ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.Group.Id));
        }
    }

    public class VisitorProfile : Profile
    {
        public VisitorProfile()
        {
            CreateMap<InfoVisitorModel, VisitorDTO>();
            CreateMap<VisitorDTO, InfoVisitorModel>();
        }
    }

}