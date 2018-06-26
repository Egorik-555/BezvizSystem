using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Models.Report;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    public class ReportController : Controller
    {
        private IReport _reportService
        {
            get { return HttpContext.GetOwinContext().Get<IReport>(); }
        }

        IMapper _mapper;

        public ReportController()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReportDTO, ReportModel>();
                cfg.CreateMap<ReportModel, ReportDTO>();

            }).CreateMapper();

        }

        public ActionResult Index()
        {
            var model = _reportService.GetReport();
            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return View(modelInView);
        }

        [HttpPost]
        public ActionResult Index(DateTime dateFrom, DateTime dateTo)
        {
            var model = _reportService.GetReport(dateFrom, dateTo);
            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return View(modelInView);
        }
    }
}