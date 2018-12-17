using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Mapper;
using BezvizSystem.Web.Models;
using BezvizSystem.Web.Models.Mark;
using BezvizSystem.Web.Views.Helpers.Pagging;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    [Authorize(Roles = "OBLSuperAdmin, OBLAdmin, OBLUser")]
    public class MarkController : Controller
    {
        IMapper mapper;
        private IService<AnketaDTO> _anketaService;
        private IService<GroupVisitorDTO> _groupService;
        private IService<VisitorDTO> _visitorService;
        private IDictionaryService<CheckPointDTO> _checkPointService;

        public MarkController(IService<AnketaDTO> anketaService, IService<GroupVisitorDTO> groupService, 
                              IService<VisitorDTO> visitorService,
                              IDictionaryService<CheckPointDTO> checkPointService)
        {
            _anketaService = anketaService;
            _groupService = groupService;
            _visitorService = visitorService;
            _checkPointService = checkPointService;
            mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromBLLToWebProfile())).CreateMapper();
        }   
        
        public ActionResult Index()
        {
            //var anketas = await _anketaService.GetForUserAsync(User.Identity.Name);
            //var model = mapper.Map<IEnumerable<AnketaDTO>, IEnumerable<ViewMarkModel>>(anketas);
            ViewBag.CheckPoints = CheckPoints();
            return View();
        }

        private bool IsNullProperties(SearchModel obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value != null) return false;
            }
            return true;
        }

        public ActionResult GroupData(SearchModel model, string search, int page = 1)
        {
            if (!IsNullProperties(model))
            {
                ViewBag.SearchModel = model;
            }
            else if (search != null && search != "null")
            {
                model = JsonConvert.DeserializeObject<SearchModel>(search);
                model.DateFrom = model.DateFrom.HasValue ? model.DateFrom.Value.ToLocalTime() : default(DateTime);
                model.DateTo = model.DateTo.HasValue ? model.DateTo.Value.ToLocalTime() : default(DateTime);
                ViewBag.SearchModel = model;
            }

            var anketas = _anketaService.GetForUser(User?.Identity.Name);

            if (model != null)
            {
                if (!String.IsNullOrEmpty(model.Name))
                    anketas = anketas.Where(a => a.Visitors.Count(v => v.Surname.ToUpper().Contains(model.Name.ToUpper())) != 0);

                DateTime dateFrom, dateTo;
                if (!model.DateFrom.HasValue)
                    dateFrom = DateTime.MinValue;
                else dateFrom = model.DateFrom.Value;

                if (!model.DateTo.HasValue)
                    dateTo = DateTime.MaxValue;
                else dateTo = model.DateTo.Value;

                anketas = anketas.Where(a => a.DateArrival >= dateFrom && a.DateArrival <= dateTo);

                if (!String.IsNullOrEmpty(model.CheckPoint))
                    anketas = anketas.Where(a => a.CheckPoint.ToUpper() == model.CheckPoint.ToUpper());
            }

            var result = mapper.Map<IEnumerable<AnketaDTO>, IEnumerable<ViewMarkModel>>(anketas.OrderBy(a => a.DateArrival));

            int pageSize = 10;
            var modelForPaging = result.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = result.Count() };
            IndexViewModel<ViewMarkModel> ivm = new IndexViewModel<ViewMarkModel> { PageInfo = pageInfo, Models = modelForPaging };

            return PartialView(ivm);
        }


        public async Task<ActionResult> ShowVisitors(int? id)
        {
            if (id != null)
            {
                var group = await _groupService.GetByIdAsync(id.Value);
                if (group == null)
                {
                    return RedirectToAction("Index");
                }

                var visitors = group.Visitors;
                var model = mapper.Map<IEnumerable<VisitorDTO>, IEnumerable<ViewVisitorModel>>(visitors);
                return View("VisitorData", model);

            }
            else return new EmptyResult();
        }

        public async Task Edit(int? id, bool arrived)
        {
            if (id.HasValue)
            {
                var visitor = await _visitorService.GetByIdAsync(id.Value);
                if (visitor != null)
                {
                    visitor.Arrived = arrived;
                    var result = await _visitorService.Update(visitor);
                }
          
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