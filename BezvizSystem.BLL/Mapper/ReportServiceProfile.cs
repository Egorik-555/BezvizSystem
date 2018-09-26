using AutoMapper;
using BezvizSystem.BLL.DTO.Report;

namespace BezvizSystem.BLL.Mapper
{
    class ReportServiceProfile : Profile
    {
        public ReportServiceProfile()
        {
            CreateMap<CountByDate, ObjectForDiagram>().
                    ForMember(dest => dest.Value1, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date.ToString("dd.MM.yyyy") : null)).
                    ForMember(dest => dest.Value2, opt => opt.MapFrom(src => src.Count));
        }
    }
}
