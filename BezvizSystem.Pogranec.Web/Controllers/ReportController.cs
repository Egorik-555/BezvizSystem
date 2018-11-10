using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Report;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Report.DTO;
using BezvizSystem.BLL.Utils;
using BezvizSystem.Pogranec.Web.Infrastructure;
using BezvizSystem.Pogranec.Web.Infrustructure;
using BezvizSystem.Pogranec.Web.Mapper;
using BezvizSystem.Pogranec.Web.Models.Report;
using ClosedXML.Excel;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers.Api
{
    [Authorize(Roles = "GPKSuperAdmin, GPKAdmin, GPKMiddle, GPKUser")]
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
            var model = GetModelByValidDates(dateFrom, dateTo);

            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return View(modelInView);
        }

        private ReportDTO GetModelByValidDates(DateTime? dateFrom, DateTime? dateTo)
        {       
            ViewBag.dateFrom = dateFrom ?? DateTime.Now;
            ViewBag.dateTo = dateTo ?? DateTime.Now;

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
        public async Task<ActionResult> InExcel1(string id)
        {      
            var serialize = new System.Web.Script.Serialization.JavaScriptSerializer();
            var list = serialize.Deserialize<IEnumerable<NatAndAgeModel>>(id).ToList();

            //добавить итог
            list.Add(new NatAndAgeModel { Natiolaty = "Итого", ManLess18 = list.Sum(s => s.ManLess18), ManMore18 = list.Sum(s => s.ManMore18),
                                                               WomanLess18 = list.Sum(s => s.WomanLess18), WomanMore18 = list.Sum(s => s.WomanMore18), All = list.Sum(s => s.All)});

            var modelForExcel = _mapper.Map<IEnumerable<NatAndAgeModel>, IEnumerable<NatAndAgeExcel>>(list);    
            
            IExcel<XLWorkbook> print = new CloseXmlExcel();
            XLWorkbook book = await print.InExcelAsync<NatAndAgeExcel>(modelForExcel);

            return new ExcelResult("Половозрастной признак.xlsx", book);
        }

        [HttpPost]
        public async Task<ActionResult> InExcel2(string id)
        {
            var serialize = new System.Web.Script.Serialization.JavaScriptSerializer();
            var list = serialize.Deserialize<IEnumerable<CountByDateModel>>(id).ToList();

            ////добавить итог
            list.Add(new CountByDateModel
            {
                DateArrival = "Итого",
                Count = list.Sum(s => s.Count)
            });

            var modelForExcel = _mapper.Map<IEnumerable<CountByDateModel>, IEnumerable<CountByDateModelExcel>>(list);
          
            IExcel<XLWorkbook> print = new CloseXmlExcel();
            XLWorkbook book = await print.InExcelAsync<CountByDateModelExcel>(modelForExcel);

            return new ExcelResult("Половозрастной признак.xlsx", book);
        }
    }
}