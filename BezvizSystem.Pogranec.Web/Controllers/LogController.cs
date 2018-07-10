using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers
{
    public class LogController : Controller
    {
        ILogger<UserActivityDTO> _loggerService;

        public LogController(ILogger<UserActivityDTO> service)
        {
            _loggerService = service;
        }



    }
}