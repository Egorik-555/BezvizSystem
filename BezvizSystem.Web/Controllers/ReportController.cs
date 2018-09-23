using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Report;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Report.DTO;
using BezvizSystem.Web.Mapper;
using BezvizSystem.Web.Models.Report;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    [Authorize(Roles = "admin, operator")]
    public class ReportController : Controller
    {
        private IReport _reportService;

        IMapper _mapper;

        public ReportController(IReport reportService)
        {
            _reportService = reportService;
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromBLLToWebProfile())).CreateMapper();
        }

        public ActionResult Index()
        {
            var model = _reportService.GetReport();
            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return View(modelInView);
        }

        public ActionResult DataReport(DateTime? dateFrom, DateTime? dateTo)
        {
            ReportDTO model;
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                model = _reportService.GetReport(dateFrom, dateTo);
            }
            else
            {
                model = _reportService.GetReport();
            }

            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return PartialView(modelInView);
        }

        private string GetString(string label1, string label2, IEnumerable<ObjectForDiagram> list)
        {
            string result = "{\"cols\" : [";
            result += "{\"id\":\"\",\"label\":\"" + label1 + "\",\"pattern\":\"\",\"type\":\"string\"},";
            result += "{\"id\":\"\",\"label\":\"" + label2 + "\",\"pattern\":\"\",\"type\":\"number\"}";
            result += "],";
            result += "\"rows\": [";

            foreach (var item in list)
            {
                result += "{ \"c\":[{\"v\":\"" + item.Value1 + "\",\"f\":null},{\"v\":" + item.Value2 + ",\"f\":null}]},";
            }

            result += "] }";

            return result;
        }

        public JsonResult GetDataByDateCount(DateTime? dateFrom, DateTime? dateTo)
        {
            ReportDTO model;
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                model = _reportService.GetReport(dateFrom, dateTo);
            }
            else
            {
                model = _reportService.GetReport();
            }

            var list = _mapper.Map<IEnumerable<CountByDate>, IEnumerable<ObjectForDiagram>>(model.AllByDateArrivalCount);
            string result = GetString("Дата прибытия", "Количество", list);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}