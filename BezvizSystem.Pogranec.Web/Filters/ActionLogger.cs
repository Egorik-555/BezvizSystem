using BezvizSystem.BLL.DI;
using BezvizSystem.BLL.DTO.Log;
using System.Linq;
using BezvizSystem.BLL.Interfaces.Log;
using BezvizSystem.DAL.Helpers;
using Ninject;
using Ninject.Modules;
using System;
using System.Web.Mvc;
using BezvizSystem.Pogranec.Web.Infrastructure;

namespace BezvizSystem.Pogranec.Web.Filters
{
    public class ActionLoggerAttribute : ActionFilterAttribute
    {
        public LogType Type { get; set; }
        public string TextActivity { get; set; }

        private ILogger _logger;
        private ActionExecutedContext filterContext;

        public ActionLoggerAttribute()
        {
            NinjectModule registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            _logger = kernel.Get<ILogger>();
        }

        private LogDTO Login()
        {
            Controller controller = filterContext.Controller as Controller;
            var msg = controller?.ModelState.Values.Where(x => x.Errors.Count() != 0).FirstOrDefault()?.Errors.FirstOrDefault();

            string userName;

            string textActivity;
            UserLevel userRole;

            if (LogType.Validation == Type && msg != null)
            {
                userName = controller.ModelState["Name"]?.Value?.AttemptedValue;
                textActivity = "Попытка входа в систему. " + msg.ErrorMessage;
                userRole = UserLevel.GPKUser;
            }
            else if (LogType.Enter == Type)
            {
                userName = filterContext.HttpContext.User.Identity.Name;
                textActivity = TextActivity;
                userRole = filterContext.HttpContext.User.GetRole();
            }
            else return null;

            return new LogDTO
            {
                Ip = filterContext.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? filterContext.HttpContext.Request.UserHostAddress,
                UserName = userName,
                Type = Type,
                UserRole = userRole,
                TextActivity = textActivity,
                DateActivity = DateTime.Now
            };
        }

        private LogDTO Logout()
        {
            return new LogDTO
            {
                Ip = filterContext.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? filterContext.HttpContext.Request.UserHostAddress,
                UserName = filterContext.HttpContext.User.Identity.Name,
                Type = Type,
                UserRole = filterContext.HttpContext.User.GetRole(),
                TextActivity = TextActivity,
                DateActivity = DateTime.Now
            };
        }

        private LogDTO Administration()
        {
            Controller controller = filterContext.Controller as Controller;
            var userName = controller?.ModelState["UserName"]?.Value?.AttemptedValue;
            string textActivity = TextActivity + " - " + userName;

            return new LogDTO
            {
                Ip = filterContext.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? filterContext.HttpContext.Request.UserHostAddress,
                UserName = filterContext.HttpContext.User.Identity.Name,
                Type = Type,
                UserRole = filterContext.HttpContext.User.GetRole(),
                TextActivity = textActivity,
                DateActivity = DateTime.Now
            };
        }

        private LogDTO LoadXML()
        {
            Controller controller = filterContext.Controller as Controller;
            var file = (filterContext.Result as ContentResult)?.Content;
         
            string textActivity = TextActivity + ((!String.IsNullOrEmpty(file)) ? " Файл - " + file : " Файл пуст");
            
            return new LogDTO
            {
                Ip = filterContext.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? filterContext.HttpContext.Request.UserHostAddress,
                UserName = filterContext.HttpContext.User.Identity.Name,
                Type = Type,
                UserRole = filterContext.HttpContext.User.GetRole(),
                TextActivity = textActivity,
                DateActivity = DateTime.Now
            };
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.filterContext = filterContext;

            LogDTO log = null;

            if (LogType.Validation == Type || LogType.Enter == Type)
            {
                log = Login();
            }
            else if (LogType.Exit == Type)
            {
                log = Logout();
            }
            else if (Type == LogType.CreateUser || Type == LogType.DeleteUser || Type == LogType.EditUser)
            {
                log = Administration();
            }
            else if (Type == LogType.LoadXML || Type == LogType.ExtraLoadXML)
            {
                log = LoadXML();
            }    

            if (log != null)
                _logger.WriteLog(log);
        }
    }
}