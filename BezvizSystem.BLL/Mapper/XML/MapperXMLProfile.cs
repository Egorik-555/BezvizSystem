using AutoMapper;
using AutoMapper.Configuration;
using BezvizSystem.BLL.DTO.XML;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Mapper.XML
{
    class MapperXMLProfile : Profile
    {
         
        public MapperXMLProfile(IUnitOfWork _database)
        {
            CreateMap<Visitor, ModelForXmlToPogran>().
                ForMember(dest => dest.Organization, opt => opt.MapFrom(src => 1)).
                //ForMember(dest => dest.TypeOperation, opt => opt.MapFrom(src => src.StatusOfRecord)).
                ForMember(dest => dest.DayBith, opt => opt.MapFrom(src => src.BithDate.HasValue ? src.BithDate.Value.Day.ToString() : null)).
                ForMember(dest => dest.MonthBith, opt => opt.MapFrom(src => src.BithDate.HasValue ? src.BithDate.Value.Month.ToString() : null)).
                ForMember(dest => dest.YearBith, opt => opt.MapFrom(src => src.BithDate.HasValue ? src.BithDate.Value.Year.ToString() : null)).
                ForMember(dest => dest.TextSex, opt => opt.MapFrom(src => src.Gender.Name)).
                ForMember(dest => dest.CodeSex, opt => opt.MapFrom(src => src.Gender.Code)).

                ForMember(dest => dest.DayValid, opt => opt.MapFrom(src => src.DocValid.HasValue ? src.DocValid.Value.Day.ToString() : null)).
                ForMember(dest => dest.MonthValid, opt => opt.MapFrom(src => src.DocValid.HasValue ? src.DocValid.Value.Month.ToString() : null)).
                ForMember(dest => dest.YearValid, opt => opt.MapFrom(src => src.DocValid.HasValue ? src.DocValid.Value.Year.ToString() : null)).

                ForMember(dest => dest.DayOfStay, opt => opt.MapFrom(src => src.Group.DaysOfStay)).
                ForMember(dest => dest.DayArrival, opt => opt.MapFrom(src => src.Group.DateArrival.HasValue ? src.Group.DateArrival.Value.Day.ToString() : null)).
                ForMember(dest => dest.MonthArrival, opt => opt.MapFrom(src => src.Group.DateArrival.HasValue ? src.Group.DateArrival.Value.Month.ToString() : null)).
                ForMember(dest => dest.YearArrival, opt => opt.MapFrom(src => src.Group.DateArrival.HasValue ? src.Group.DateArrival.Value.Year.ToString() : null));
        }
    }
}
