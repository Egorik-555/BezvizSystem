using System;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.DAL;
using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using BezvizSystem.BLL.DTO.Dictionary;

[assembly: OwinStartup(typeof(BezvizSystem.Web.App_Start.Startup))]

namespace BezvizSystem.Web.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();
        string CONNECTION = "BezvizContext";


        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<BezvizContext>(CreateContext);
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.CreatePerOwinContext<BezvizUserManager>(BezvizUserManager.Create);

            app.CreatePerOwinContext<IService<VisitorDTO>>(CreateVisitorService);
            app.CreatePerOwinContext<IService<GroupVisitorDTO>>(CreateGroupService);
            app.CreatePerOwinContext<IService<AnketaDTO>>(CreateAnketaService);
            app.CreatePerOwinContext<IDictionaryService<DictionaryDTO>>(CreateDictionaryService);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }

        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService(CONNECTION);
        }

        private BezvizContext CreateContext()
        {
            return serviceCreator.CreateContext(CONNECTION);
        }

        private IService<VisitorDTO> CreateVisitorService()
        {
            return serviceCreator.CreateVisitorService(CONNECTION);
        }

        private IService<GroupVisitorDTO> CreateGroupService()
        {
            return serviceCreator.CreateGroupService(CONNECTION);
        }

        private IService<AnketaDTO> CreateAnketaService()
        {
            return serviceCreator.CreateAnketaService(CONNECTION);
        }

        private IDictionaryService<DictionaryDTO> CreateDictionaryService()
        {
            return serviceCreator.CreateDictionaryService(CONNECTION);
        }
    }
}
