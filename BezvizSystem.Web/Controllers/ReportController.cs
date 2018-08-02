using AutoMapper;
using BezvizSystem.BLL.DTO;
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
        private IReport _reportService
        {
            get { return HttpContext.GetOwinContext().Get<IReport>(); }
        }

        IMapper _mapper;

        public ReportController()
        {
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
    }
}