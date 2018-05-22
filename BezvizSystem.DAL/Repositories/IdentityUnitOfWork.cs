using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Identity;
using BezvizSystem.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private BezvizContext context;
        private BezvizUserManager userManager;
        private BezvizRoleManager roleManager;
        private IRepository<OperatorProfile, string> operatorManager;
        private IRepository<Visitor, int> visitorManager;
        private IRepository<GroupVisitor, int> groupManager;
        private IRepository<Status, int> statusManager;

        public IdentityUnitOfWork(string connection)
        {
            context = new BezvizContext(connection);
            userManager = new BezvizUserManager(new UserStore<BezvizUser>(context));          
            roleManager = new BezvizRoleManager(new RoleStore<BezvizRole>(context));
            operatorManager = new OperatorManager(context);
            visitorManager = new VisitorManager(context);
            groupManager = new GroupManager(context);
            statusManager = new StatusManager(context);
        }

        public BezvizUserManager UserManager => userManager;
        public BezvizRoleManager RoleManager => roleManager;
        public IRepository<OperatorProfile, string> OperatorManager => operatorManager;
        public IRepository<Visitor, int> VisitorManager => visitorManager;
        public IRepository<GroupVisitor, int> GroupManager => groupManager;
        public IRepository<Status, int> StatusManager => statusManager;

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    operatorManager.Dispose();
                    userManager.Dispose();
                    roleManager.Dispose();
                    visitorManager.Dispose();
                    groupManager.Dispose();
                    statusManager.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
