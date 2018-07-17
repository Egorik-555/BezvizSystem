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
               
                
                //TODO add formember         
                ForMember(dest => dest.TextSex, opt => opt.MapFrom(src => _database.Genders.GetAll().Where(g => g.Name == src.Gender.Name).FirstOrDefault()));
        }
    }
}
