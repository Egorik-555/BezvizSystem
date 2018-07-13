using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BezvizSystem.Pogranec.Web.Infrastructure.Log
{ 
    public class TrackLoginFilter : ActionFilterAttribute
    {

        private ILogger<UserActivityDTO> _logger;

        public TrackLoginFilter(ILogger<UserActivityDTO> logger)
        {
            _logger = logger;
        }


        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var identity = HttpContext.Current.User.Identity;

            if (identity.IsAuthenticated)
            {
                _logger.Insert(new UserActivityDTO
                {
                    Login = identity.Name,
                    Ip = HttpContext.Current.Request.UserHostAddress,
                    Operation = "Вход",
                    TimeActivity = DateTime.Now
                });
            }
        }
    }
}