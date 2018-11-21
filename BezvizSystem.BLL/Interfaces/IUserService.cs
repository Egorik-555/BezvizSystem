using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.DAL;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        BezvizUserManager ManagerForChangePass { get; set; }

        Task<OperationDetails> Create(UserDTO userDto);
        Task<OperationDetails> Delete(UserDTO userDto);
        Task<OperationDetails> Update(UserDTO userDto);
        Task<OperationDetails> Registrate(UserDTO user, string callback, IGeneratePass generator);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);

        Task<OperationDetails> SetInitialData(UserDTO adminDto, List<string> roles);

        Task<UserDTO> GetByIdAsync(string id);
        Task<UserDTO> GetByNameAsync(string name);
        UserDTO GetByName(string name);
        IEnumerable<UserDTO> GetByRole(string roleName);
        IEnumerable<UserDTO> GetAll();
        UserLevel GetRoleByUser(string name);
    }
}
