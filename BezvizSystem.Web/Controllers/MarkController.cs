using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
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
    public class MarkController : Controller
    {
        IMapper mapper;
        private IService<AnketaDTO> AnketaService
        {
            get { return HttpContext.GetOwinContext().Get<IService<AnketaDTO>>(); }
        }

        private IService<GroupVisitorDTO> GroupService
        {
            get { return HttpContext.GetOwinContext().Get<IService<GroupVisitorDTO>>(); }
        }

        private IService<VisitorDTO> VisitorService
        {
            get { return HttpContext.GetOwinContext().Get<IService<VisitorDTO>>(); }
        }

        public MarkController()
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnketaDTO, ViewMarkModel>().
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival));
                   
                cfg.CreateMap<VisitorDTO, ViewVisitorModel>().
                    ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.Group.Id));

            }).CreateMapper();
        }   
        
        public async Task<ActionResult> Index()
        {
            var anketas = await AnketaService.GetForUserAsync(User.Identity.Name);
            var model = mapper.Map<IEnumerable<AnketaDTO>, IEnumerable<ViewMarkModel>>(anketas);
            return View(model);
        }

        public async Task<ActionResult> ShowVisitors(int? id)
        {
            if (id != null)
            {

                var group = await GroupService.GetByIdAsync(id.Value);
                if (group == null)
                {
                    return RedirectToAction("Index");
                }

                var visitors = group.Visitors;
                var model = mapper.Map<IEnumerable<VisitorDTO>, IEnumerable<ViewVisitorModel>>(visitors);
                return PartialView("VisitorData", model);

            }
            else return new EmptyResult();
        }

        public async Task Edit(int? id, bool arrived)
        {
            if (id.HasValue)
            {
                var visitor = await VisitorService.GetByIdAsync(id.Value);
                if (visitor != null)
                {
                    visitor.Arrived = arrived;
                    var result = await VisitorService.Update(visitor);
                }
          
            }
                   
        }
    }
}