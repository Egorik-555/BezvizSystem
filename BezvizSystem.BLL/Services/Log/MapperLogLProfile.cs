using AutoMapper;
using AutoMapper.Configuration;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.DTO.XML;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Services.Log
{
    class MapperLogLProfile : Profile
    {
         
        public MapperLogLProfile()
        {
            CreateMap<BezvizSystem.DAL.Entities.Log, LogDTO>();
            CreateMap<LogDTO, BezvizSystem.DAL.Entities.Log>();
        }
    }
}
