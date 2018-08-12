using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Identity;
using BezvizSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        BezvizUserManager UserManager { get; }
        BezvizRoleManager RoleManager { get; }

        IRepository<OperatorProfile, string> OperatorManager { get; }
        IRepository<Visitor, int> VisitorManager { get; }
        IRepository<GroupVisitor, int> GroupManager { get; }
        IRepositoryXMLDispatch<XMLDispatch, int> XMLDispatchManager { get; }

        IRepository<Nationality, int> Nationalities { get; }
        IRepository<CheckPoint, int> CheckPoints { get; }
        IRepository<Gender, int> Genders { get; }

        Task SaveAsync();
    }
}