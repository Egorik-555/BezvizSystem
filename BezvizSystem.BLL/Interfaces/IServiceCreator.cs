using BezvizSystem.BLL.DTO;
using BezvizSystem.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IServiceCreator
    {
        BezvizContext CreateContext(string connection);
        IUserService CreateUserService(string connection);
        IService<VisitorDTO> CreateVisitorService(string connection);
        IService<GroupVisitorDTO> CreateGroupService(string connection);
        IService<AnketaDTO> CreateAnketaService(string connection);
    }
}
