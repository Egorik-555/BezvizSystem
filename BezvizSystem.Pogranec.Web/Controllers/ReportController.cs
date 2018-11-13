using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
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
        private IDictionaryService<CheckPointDTO> _checkPointService;
        IMapper _mapper;

        public ReportController(IReport report, IDictionaryService<CheckPointDTO> checkPointService)
        {
            _report = report;
            _checkPointService = checkPointService;
            _mapper = new MapperPogranecConfiguration().CreateMapper();
        }

        public ReportController()
        {
            _mapper = new MapperPogranecConfiguration().CreateMapper();
        }
      
        public ActionResult Index(DateTime? dateFrom, DateTime? dateTo, string checkPoint)
        {
            var model = GetModelByValidDates(dateFrom, dateTo, checkPoint);                 

            ViewBag.dateFrom = dateFrom;
            ViewBag.dateTo = dateTo;
            ViewBag.checkPoint = checkPoint;

            ViewBag.CheckPoints = CheckPoints();

            var modelInView = _mapper.Map<ReportDTO, ReportModel>(model);
            return View(modelInView);
        }

        private ReportDTO GetModelByValidDates(DateTime? dateFrom, DateTime? dateTo, string checkPoint)
        {       
            if (!dateFrom.HasValue && !dateTo.HasValue)
            {
                return _report.GetReport(checkPoint);             
            }

            return _report.GetReport(dateFrom, dateTo, checkPoint);
        }

        private SelectList CheckPoints()
        {
            List<string> list = new List<string>(_checkPointService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return new SelectList(list, "");
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

            return new ExcelResult("По половозрастному признаку.xlsx", book);
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

            return new ExcelResult("По дате прибытия.xlsx", book);
        }

        [HttpPost]
        public async Task<ActionResult> InExcel3(string id)
        {
            var serialize = new System.Web.Script.Serialization.JavaScriptSerializer();
            var list = serialize.Deserialize<IEnumerable<CountByCheckPointModel>>(id).ToList();

            ////добавить итог
            list.Add(new CountByCheckPointModel
            {
                CheckPoint = "Итого",
                Count = list.Sum(s => s.Count)
            });

            var modelForExcel = _mapper.Map<IEnumerable<CountByCheckPointModel>, IEnumerable<CountByCheckPointModelExcel>>(list);

            IExcel<XLWorkbook> print = new CloseXmlExcel();
            XLWorkbook book = await print.InExcelAsync<CountByCheckPointModelExcel>(modelForExcel);

            return new ExcelResult("По пунктам пропуска.xlsx", book);
        }

        [HttpPost]
        public async Task<ActionResult> InExcel4(string id)
        {
            var serialize = new System.Web.Script.Serialization.JavaScriptSerializer();
            var list = serialize.Deserialize<IEnumerable<CountByDaysModel>>(id).ToList();

            ////добавить итог
            list.Add(new CountByDaysModel
            {
                Days = "Итого",
                Count = list.Sum(s => s.Count)
            });

            var modelForExcel = _mapper.Map<IEnumerable<CountByDaysModel>, IEnumerable<CountByDaysModelExcel>>(list);

            IExcel<XLWorkbook> print = new CloseXmlExcel();
            XLWorkbook book = await print.InExcelAsync<CountByDaysModelExcel>(modelForExcel);

            return new ExcelResult("По дням пребывания.xlsx", book);
        }

        [HttpPost]
        public async Task<ActionResult> InExcel5(string id)
        {
            var serialize = new System.Web.Script.Serialization.JavaScriptSerializer();
            var list = serialize.Deserialize<IEnumerable<CountByOperatorModel>>(id).ToList();

            ////добавить итог
            list.Add(new CountByOperatorModel
            {
                Operator = "Итого",
                Count = list.Sum(s => s.Count)
            });

            var modelForExcel = _mapper.Map<IEnumerable<CountByOperatorModel>, IEnumerable<CountByOperatorModelExcel>>(list);

            IExcel<XLWorkbook> print = new CloseXmlExcel();
            XLWorkbook book = await print.InExcelAsync<CountByOperatorModelExcel>(modelForExcel);

            return new ExcelResult("По туроператорам.xlsx", book);
        }
    }
}