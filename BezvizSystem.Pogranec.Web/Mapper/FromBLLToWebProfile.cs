using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Report;
using BezvizSystem.BLL.Report.DTO;
using BezvizSystem.Pogranec.Web.Models.Admin;
using System.Collections.Generic;
using System.Linq;

namespace BezvizSystem.Pogranec.Web.Mapper
{
    public class FromBLLToWebProfile : Profile
    {
        public FromBLLToWebProfile()
        {
            CreateMap<ProfileUserDTO, DisplayUser>();
            CreateMap<UserDTO, DisplayUser>();
            
            CreateMap<CreateUser, UserDTO>();
            CreateMap<CreateUser, ProfileUserDTO>();
        }
    }
}