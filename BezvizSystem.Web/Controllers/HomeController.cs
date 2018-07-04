using BezvizSystem.BLL.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    [Authorize(Roles = "admin, operator")]
    public class HomeController : Controller
    {
        private IUserService Service
        {
            get { return HttpContext.GetOwinContext().Get<IUserService>(); }
        }

        public ActionResult Index()
        {          
            return View();
        }   
    }
}