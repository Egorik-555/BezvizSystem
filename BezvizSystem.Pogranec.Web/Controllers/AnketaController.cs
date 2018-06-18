using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers
{
    public class AnketaController : Controller
    {

        IService<AnketaDTO> _anketaService;

        public AnketaController(IService<AnketaDTO> service)
        {
            _anketaService = service;
        }

        public ActionResult Index()
        {


            return View();
        }
    }
}