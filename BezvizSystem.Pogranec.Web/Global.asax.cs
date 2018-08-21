using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject.Modules;
using BezvizSystem.BLL.DI;
using Ninject;
using Ninject.Web.Mvc;
using BezvizSystem.BLL.Interfaces;

namespace BezvizSystem.Pogranec.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            NinjectModule registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

           // GlobalFilters.Filters.Add(new TrackLoginFilter(kernel.Get<ILogger<UserActivityDTO>>()));
        }
    }
}
