using BezvizSystem.BLL.DI;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Interfaces.Log;
using BezvizSystem.DAL.Helpers;
using Ninject;
using Ninject.Modules;
using System;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Filters
{
    public class ActionLoggerAttribute : ActionFilterAttribute
    {
        public LogType Type { get; set; }
        public string TextActivity { get; set; }

        private ILogger _logger;

        public ActionLoggerAttribute()
        {
            NinjectModule registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            _logger = kernel.Get<ILogger>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            LogDTO log = new LogDTO
            {
                Ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                UserName = filterContext.HttpContext.User.Identity.Name,
                Type = Type,
                TextActivity = TextActivity,
                DateActivity = DateTime.Now
            };

            _logger.WriteLog(log);
        }
    }
}