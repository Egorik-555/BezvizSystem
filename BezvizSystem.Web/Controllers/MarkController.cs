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

        public MarkController()
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnketaDTO, ViewMarkModel>().
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival)).
                    ForMember(dest => dest.Arrived, opt => opt.MapFrom(src => CheckAllArrivals(src.Visitors)));


            }).CreateMapper();
        }

        private string CheckAllArrivals(IEnumerable<VisitorDTO> list)
        {
            int count = 0;
            foreach (var item in list)
            {
                if (item.Arrived)
                {
                    count++;
                }
            }

            if (count == list.Count())
                return "V";
            else if (count != 0)
                return "Частично";
            else return "X";
        }

        public async Task<ActionResult> Index()
        {
            var anketas = await AnketaService.GetForUserAsync(User.Identity.Name);

            var model = mapper.Map<IEnumerable<AnketaDTO>, IEnumerable<ViewMarkModel>>(anketas);

            return View(model);
        }
    }
}