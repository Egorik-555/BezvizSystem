using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject.Modules;
using BezvizSystem.BLL.DI;
using Ninject;
using Ninject.Web.Mvc;
using BezvizSystem.BLL.Interfaces;
using System;
using BezvizSystem.BLL.Interfaces.Log;
using BezvizSystem.BLL.DTO.Log;
using System.Diagnostics;

namespace BezvizSystem.Pogranec.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private NinjectModule registrations;
        private StandardKernel kernel;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            registrations = new NinjectRegistrations();
            kernel = new StandardKernel(registrations);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }

        //protected void Session_End(Object sender, EventArgs e)
        //{
        //    Debug.WriteLine("protected void Session_End();");
        //}
}
}
