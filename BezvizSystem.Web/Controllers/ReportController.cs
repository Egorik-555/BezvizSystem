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

        public ActionResult Index(DateTime? dateFrom, DateTime? dateTo)
        {
            ReportDTO model;
            model = GetModelByValidDates(dateFrom, dateTo);

            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return View(modelInView);
        }


        private ReportDTO GetModelByValidDates(DateTime? dateFrom, DateTime? dateTo)
        {
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                return _reportService.GetReport(dateFrom, dateTo);
            }
            else
            {
                if (!dateFrom.HasValue && dateTo.HasValue)
                {
                    dateFrom = DateTime.MinValue;
                    return _reportService.GetReport(dateFrom, dateTo);
                }

                if (dateFrom.HasValue && !dateTo.HasValue)
                {
                    dateTo = DateTime.MaxValue;
                    return _reportService.GetReport(dateFrom, dateTo);
                }

                return _reportService.GetReport();
            }
        }       
    }
}