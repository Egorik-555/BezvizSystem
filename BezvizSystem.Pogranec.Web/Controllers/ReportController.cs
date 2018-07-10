using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers.Api
{
    [Authorize(Roles = "pogranecAdmin, pogranec")]
    public class ReportController : Controller
    {      
        public ActionResult Index(string id)
        {



            return View();
        }
    }
}