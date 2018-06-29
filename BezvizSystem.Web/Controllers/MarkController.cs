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

        public async Task<ActionResult> Edit(int? id)
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
                //return View(model);

                return PartialView("VisitorData", model);
            }
            else return new EmptyResult();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ICollection<ViewVisitorModel> visitors)
        {
            if (visitors.Count != 0)
            {
                var group = await GroupService.GetByIdAsync(visitors.First().GroupId);
                if (group != null)
                {
                    foreach (var visitor in group.Visitors)
                    {
                        foreach (var item in visitors)
                        {
                            if (visitor.Id == item.Id)
                                visitor.Arrived = item.Arrived;
                        }
                    }
                    await GroupService.Update(group);
                }
            }

            var anketas = await AnketaService.GetForUserAsync(User.Identity.Name);
            var model = mapper.Map<IEnumerable<AnketaDTO>, IEnumerable<ViewMarkModel>>(anketas);
            return View("Index", model);          
        }
    }
}