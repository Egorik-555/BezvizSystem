using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Report.DTO;
using BezvizSystem.BLL.Utils;
using BezvizSystem.Pogranec.Web.Mapper;
using BezvizSystem.Pogranec.Web.Models.Report;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers.Api
{
    [Authorize(Roles = "GPKSuperAdmin, GPKAdmin, GPKMiddle")]
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
      
        public ActionResult Index(DateTime? dateFrom, DateTime? dateTo)
        {
            ReportDTO model;
            ViewBag.dateFrom = dateFrom ?? DateTime.Now;
            ViewBag.dateTo = dateTo ?? DateTime.Now;

            model = GetModelByValidDates(dateFrom, dateTo);

            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return View(modelInView);
        }

        private ReportDTO GetModelByValidDates(DateTime? dateFrom, DateTime? dateTo)
        {
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                return _report.GetReport(dateFrom, dateTo);
            }
            else
            {
                if (!dateFrom.HasValue && dateTo.HasValue)
                {
                    dateFrom = DateTime.MinValue;
                    return _report.GetReport(dateFrom, dateTo);
                }

                if (dateFrom.HasValue && !dateTo.HasValue)
                {
                    dateTo = DateTime.MaxValue;
                    return _report.GetReport(dateFrom, dateTo);
                }

                return _report.GetReport();
            }
        }

        [HttpPost]
        public ActionResult InExcel()
        {
           //список всех туристовIEnumerable<
           // var modelForExcel = _mapper.Map<IEnumerable<NatAndAge>, IEnumerable<ViewTable1InExcel>>(id);    
            IExcel print = new Excel();
           // string workString = await print.InExcelAsync<ViewTable1InExcel>(modelForExcel);

            return new EmptyResult();//ExcelResult("Зарегистрированные анкеты.xls", workString);
        }
    }
}