using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.Pogranec.Web.Models.Anketa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers
{
    [Authorize(Roles = "GPKSuperAdmin, GPKAdmin, GPKMiddle, GPKUser")]
    public class AnketaController : Controller
    {
        IService<AnketaDTO> _anketaService;
        private IDictionaryService<CheckPointDTO> _checkPointService;
        IXmlCreator _xmlService;

        public AnketaController(IService<AnketaDTO> anketaService, IDictionaryService<CheckPointDTO> checkPointService, IXmlCreator xmlService)
        {
            _checkPointService = checkPointService;
            _anketaService = anketaService;
            _xmlService = xmlService;
        }

        public ActionResult Index(DateTime? dateFrom, DateTime? dateTo, string checkPoint)
        {
            ViewBag.dateFrom = dateFrom;
            ViewBag.dateTo = dateTo;
            ViewBag.checkPoint = checkPoint;
            ViewBag.CheckPoints = CheckPoints();
            return View();
        }

        public ActionResult DataAnketa(DateTime? dateFrom, DateTime? dateTo, string checkPoint)
        {
            var list = GetModelByValidDates(dateFrom, dateTo, checkPoint);
            var group = list.GroupBy(a => a.CheckPoint, a => a.CountMembers).
                             Select(a => new ArrivedInfo { CheckPoint = a.Key, Count = a.Sum() }).ToList();

            
            var model = new ArrivedPerson { Infoes = group, Count = group.Sum(a => a.Count), ArriveFrom = dateFrom, ArriveTo = dateTo };
            return PartialView(model);
        }

        private IEnumerable<AnketaDTO> GetModelByValidDates(DateTime? dateFrom, DateTime? dateTo, string checkPoint)
        {
            var list = _anketaService.GetAll();

            if (!dateFrom.HasValue)
                dateFrom = DateTime.Now.Date;

            if (!dateTo.HasValue)
                dateTo = DateTime.Now.Date;

            if (!String.IsNullOrEmpty(checkPoint))
                list = list.Where(a => a.CheckPoint == checkPoint);

            return list.Where(a => a.DateArrival.Value <= dateTo && a.DateArrival >= dateFrom);
        }

        public async Task<ActionResult> GetAnketasDefault()
        {
            string file = HostingEnvironment.MapPath("~/App_Data/fileDefault.xml");
            var result = await _xmlService.SaveNew(file);

            string contentType = "application/xml";

            if (result.Succedeed)
                return File(file, contentType, Path.GetFileName(file));
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> GetAnketasExtra()
        {
            string file = HostingEnvironment.MapPath("~/App_Data/fileExtra.xml");
            var result = await _xmlService.SaveExtra(file);

            string contentType = "application/xml";

            if (result.Succedeed)
                return File(file, contentType, Path.GetFileName(file));
            else
            {
                return RedirectToAction("Index");
            }
        }

        private SelectList CheckPoints()
        {
            List<string> list = new List<string>(_checkPointService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return new SelectList(list, "");
        }
    }
}