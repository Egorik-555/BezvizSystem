
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
using System.Collections.Generic;

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
            app.CreatePerOwinContext<BezvizUserManager>(BezvizUserManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            app.Use(typeof(MyMiddlewareClass));
 
        }

        private BezvizContext CreateContext()
        {
            return serviceCreator.CreateContext(CONNECTION);
        }

    }

    public class MyMiddlewareClass : OwinMiddleware
    {
        DateTime DATE = new DateTime(2018, 12, 30);

        public MyMiddlewareClass(OwinMiddleware next)
            : base(next)
        {

        }

        public async override Task Invoke(IOwinContext context)
        {
            if (DateTime.Now > DATE)
                await context.Response.WriteAsync($"The term of the application expired ({DATE.ToShortDateString()})!!!");
            else await Next.Invoke(context);        
        }
    }

}
