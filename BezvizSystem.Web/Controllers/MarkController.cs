using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Mapper;
using BezvizSystem.Web.Models.Mark;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    [Authorize(Roles = "admin, operator")]
    public class MarkController : Controller
    {
        IMapper mapper;
        private IService<AnketaDTO> _anketaService;
        private IService<GroupVisitorDTO> _groupService;
        private IService<VisitorDTO> _visitorService;

        public MarkController(IService<AnketaDTO> anketaService, IService<GroupVisitorDTO> groupService, IService<VisitorDTO> visitorService)
        {
            _anketaService = anketaService;
            _groupService = groupService;
            _visitorService = visitorService;
            mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromBLLToWebProfile())).CreateMapper();
        }   
        
        public async Task<ActionResult> Index()
        {
            var anketas = await _anketaService.GetForUserAsync(User.Identity.Name);
            var model = mapper.Map<IEnumerable<AnketaDTO>, IEnumerable<ViewMarkModel>>(anketas);
            return View(model);
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
    }
}