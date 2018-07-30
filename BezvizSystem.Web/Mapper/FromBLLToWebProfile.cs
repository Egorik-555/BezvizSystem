using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.Web.Models.Operator;
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
        }
    }

}