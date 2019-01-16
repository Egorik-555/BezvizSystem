using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.Pogranec.Web.Filters;
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
        private IUserService _userService;
        private IDictionaryService<CheckPointDTO> _checkPointService;
        IXmlCreator _xmlService;

        public AnketaController(IService<AnketaDTO> anketaService, IDictionaryService<CheckPointDTO> checkPointService, IXmlCreator xmlService, IUserService userService)
        {
            _checkPointService = checkPointService;
            _userService = userService;
            _anketaService = anketaService;
            _xmlService = xmlService;
        }

        public ActionResult Index(DateTime? dateFrom, DateTime? dateTo, string checkPoint)
        {
            ViewBag.CheckPoints = CheckPoints();
            return View();
        }

        public ActionResult DataAnketa(DateTime? dateFrom, DateTime? dateTo, string checkPoint)
        {
            var list = GetModelByValidDates(ref dateFrom, ref dateTo, checkPoint);
            var group = list.GroupBy(a => a.CheckPoint, a => a.CountMembers).
                             Select(a => new ArrivedInfo { CheckPoint = a.Key, Count = a.Sum() }).ToList();

            string info = "Ожидается к прибытию ";
            if (dateFrom == dateTo)
            {
                info += "на " + dateFrom.Value.ToShortDateString();
            }
            else if (dateFrom == DateTime.MinValue && dateTo != DateTime.MaxValue)
            {
                info += "по " + dateTo.Value.ToShortDateString();
            }
            else if (dateFrom != DateTime.MinValue && dateTo == DateTime.MaxValue)
            {
                info += "с " + dateFrom.Value.ToShortDateString();
            }
            else if (dateFrom != DateTime.MinValue && dateTo != DateTime.MaxValue)
            {
                info += "с " + dateFrom.Value.ToShortDateString() + " по " + dateTo.Value.ToShortDateString();
            }

            if (!string.IsNullOrEmpty(checkPoint))
            {
                info += " в пункте пропуска - " + checkPoint;
            }

            var model = new ArrivedPerson
            {
                Infoes = group,
                Info = info,
                Count = group.Sum(a => a.Count),
                ArriveFrom = dateFrom,
                ArriveTo = dateTo,
                Xmls = _xmlService.Count(),
                ExtraXmls = _xmlService.ExtraCount()
            };
            return PartialView(model);
        }

        private IEnumerable<AnketaDTO> GetModelByValidDates(ref DateTime? dateFrom, ref DateTime? dateTo, string checkPoint)
        {
            var list = _anketaService.GetAll();

            var date1 = dateFrom;
            var date2 = dateTo;
            if (!dateFrom.HasValue && !dateTo.HasValue)
            {
                date1 = DateTime.Now.Date;
                date2 = DateTime.Now.Date;
            }
            else if (!dateFrom.HasValue && dateTo.HasValue)
            {
                date1 = DateTime.MinValue;
            }
            else if (dateFrom.HasValue && !dateTo.HasValue)
            {
                date2 = DateTime.MaxValue;
            }

            if (!String.IsNullOrEmpty(checkPoint))
                list = list.Where(a => a.CheckPoint == checkPoint);

            dateFrom = date1;
            dateTo = date2;
            return list.Where(a => a.DateArrival.Value <= date2 && a.DateArrival >= date1);
        }

        [ActionLogger(Type = DAL.Helpers.LogType.LoadXML, TextActivity = "Выгрузка анкет.")]
        public async Task<JsonResult> GetXMLFileName()
        {
            string date = DateTime.Now.ToFileTime().ToString();
            string fileName = "DefaultXml_" + date + ".xml";
            string file = HostingEnvironment.MapPath("~/App_Data/XMLs/" + fileName);
            var result = await _xmlService.SaveNew(file);

            if (result.Succedeed)
                return Json(new OperationDetails(true, fileName), JsonRequestBehavior.AllowGet);
            else
                return Json(result, JsonRequestBehavior.AllowGet);
        }
       
        [ActionLogger(Type = DAL.Helpers.LogType.ExtraLoadXML, TextActivity = "Экстренная выгрузка анкет.")]
        public async Task<JsonResult> GetExtraXMLFileName()
        {
            string date = DateTime.Now.ToFileTime().ToString();
            string fileName = "ExtraXml_" + date + ".xml";
            string file = HostingEnvironment.MapPath("~/App_Data/XMLs/" + fileName);
            var result = await _xmlService.SaveExtra(file);

            if (result.Succedeed)
                return Json(new OperationDetails(true, fileName), JsonRequestBehavior.AllowGet);
            else
                return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAnketas(string fileName)
        {
            string contentType = "application/xml";
            string file = HostingEnvironment.MapPath("~/App_Data/XMLs/" + fileName);

            return File(file, contentType, Path.GetFileName(file));
        }

        private SelectList CheckPoints()
        {
            List<string> list = new List<string>(_checkPointService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return new SelectList(list, "");
        }
    }
}