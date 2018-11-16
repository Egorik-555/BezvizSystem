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

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var context = filterContext.HttpContext;

            if (Type == LogType.CreateUser || Type == LogType.DeleteUser || Type == LogType.EditUser)
            {
                TextActivity += " - " + context.Items["user"];
            }
            else if(Type == LogType.LoadXML || Type == LogType.ExtraLoadXML)
            {
                TextActivity += " Файл - " + context.Items["file"];
            }


            LogDTO log = new LogDTO
            {
                Ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? context.Request.UserHostAddress,
                UserName = filterContext.HttpContext.User.Identity.Name,
                Type = Type,
                TextActivity = TextActivity,
                DateActivity = DateTime.Now
            };

            _logger.WriteLog(log);
        }
    }
}