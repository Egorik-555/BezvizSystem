﻿using BezvizSystem.BLL.DI;
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
    public class LoginException11Attribute : ActionFilterAttribute
    {
        private ILogger _logger;

        public LoginException11Attribute()
        {
            NinjectModule registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            _logger = kernel.Get<ILogger>();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var controller = filterContext.Controller;
            var request = filterContext.HttpContext.Request;
            var msg = controller.TempData["errorMsg"];

            if (msg != null)
            {
                UserLevel role;
                if (controller.TempData["role"] == null)
                    role = UserLevel.GPKUser;
                else role = (UserLevel)controller.TempData["role"];

                LogDTO log = new LogDTO
                {
                    Ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                    UserName = (string)controller.TempData["userName"],
                    Type = LogType.Enter,
                    UserRole = role,
                    TextActivity = "Попытка входа в систему. " + (string)msg,
                    DateActivity = DateTime.Now
                };

                _logger.WriteLog(log);
            }

            base.OnResultExecuted(filterContext);
        }
    }
}