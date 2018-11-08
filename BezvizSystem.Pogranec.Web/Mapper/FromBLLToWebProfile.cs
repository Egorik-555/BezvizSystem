using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Report;
using BezvizSystem.Pogranec.Web.Models.Admin;
using BezvizSystem.Pogranec.Web.Models.Report;


namespace BezvizSystem.Pogranec.Web.Mapper
{
    public class FromBLLToWebProfile : Profile
    {
        public FromBLLToWebProfile()
        {
            RecognizePrefixes("ProfileUser");

            CreateMap<UserDTO, DisplayUser>();
            CreateMap<UserDTO, EditUser>();
            CreateMap<UserDTO, DeleteUser>();

            CreateMap<CreateUser, UserDTO>();
            CreateMap<CreateUser, ProfileUserDTO>();

            CreateMap<EditUser, UserDTO>();
            CreateMap<EditUser, ProfileUserDTO>();

            CreateMap<DeleteUser, UserDTO>();
            CreateMap<DeleteUser, ProfileUserDTO>();
        }
    }
}