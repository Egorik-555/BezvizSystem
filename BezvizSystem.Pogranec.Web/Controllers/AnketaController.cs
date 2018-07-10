using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Pogranec.Web.Models.Anketa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers
{
    [Authorize(Roles = "pogranecAdmin, pogranec")]
    public class AnketaController : Controller
    {
        IService<AnketaDTO> _anketaService;

        public AnketaController(IService<AnketaDTO> service)
        {
            _anketaService = service;         
        }

        public ActionResult Index()
        {
            var date = DateTime.Now.Date;
            var list = _anketaService.GetAll();
            var group = list.Where(g => g.DateArrival.Value.Date == date).
                             GroupBy(a => a.CheckPoint, a => a.CountMembers).
                             Select(a => new ArrivedInfo { CheckPoint = a.Key, Count = a.Sum()}).ToList();

            var model = new ArrivedPerson { Infoes = group, Count = group.Sum(a => a.Count) };

            return View(model);
        }
    }
}