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
        private IRepository<XMLDispatch, int> xmlDispatchManager;

        private IRepository<CheckPoint, int> checkPoints;
        private IRepository<Nationality, int> nationalities;
        private IRepository<Gender, int> genders;


        public IdentityUnitOfWork(string connection)
        {
            context = new BezvizContext(connection);
            userManager = new BezvizUserManager(new UserStore<BezvizUser>(context));          
            roleManager = new BezvizRoleManager(new RoleStore<BezvizRole>(context));

            operatorManager = new OperatorManager(context);
            visitorManager = new VisitorManager(context);
            groupManager = new GroupManager(context);
            xmlDispatchManager = new XMLDispatchManager(context);

            checkPoints = new CheckPointManager(context);
            nationalities = new NationalityManager(context);
            genders = new GenderManager(context);
        }

        public BezvizUserManager UserManager => userManager;
        public BezvizRoleManager RoleManager => roleManager;

        public IRepository<OperatorProfile, string> OperatorManager => operatorManager;
        public IRepository<Visitor, int> VisitorManager => visitorManager;
        public IRepository<GroupVisitor, int> GroupManager => groupManager;
        public IRepository<XMLDispatch, int> XMLDispatchManager => xmlDispatchManager;

        public IRepository<Nationality, int> Nationalities => nationalities;
        public IRepository<CheckPoint, int> CheckPoints => checkPoints;
        public IRepository<Gender, int> Genders => genders;
        
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
                    xmlDispatchManager.Dispose();

                    checkPoints.Dispose();
                    nationalities.Dispose();
                    genders.Dispose();
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
