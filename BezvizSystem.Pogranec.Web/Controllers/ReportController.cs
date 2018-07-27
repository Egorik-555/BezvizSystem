using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Report.DTO;
using BezvizSystem.Pogranec.Web.Mapper;
using BezvizSystem.Pogranec.Web.Models.Report;
using Microsoft.AspNet.Identity.Owin;
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
        private IReport _report;
        IMapper _mapper;

        public ReportController(IReport report)
        {
            _report = report;
            _mapper = new MapperPogranecConfiguration().CreateMapper();
        }

        public ReportController()
        {
            _mapper = new MapperPogranecConfiguration().CreateMapper();
        }
      
        public ActionResult Index()
        {
            var model = _report.GetReport();
            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return View(modelInView);
        }

        [HttpPost]
        public ActionResult Index(DateTime dateFrom, DateTime dateTo)
        {
            var model = _report.GetReport(dateFrom, dateTo);
            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return View(modelInView);
        }
    }
}