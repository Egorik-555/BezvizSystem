using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Models.Group;
using BezvizSystem.Web.Models.Visitor;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    public class GroupController : Controller
    {
        private IService<GroupVisitorDTO> GroupService
        {
            get { return HttpContext.GetOwinContext().Get<IService<GroupVisitorDTO>>(); }
        }

        IMapper mapper;

        public GroupController()
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateGroupModel, GroupVisitorDTO>().
                    ForMember(dest => dest.Visitors, opt => opt.MapFrom(src => src.Infoes));
                cfg.CreateMap<InfoVisitorModel, VisitorDTO>();
            }).CreateMapper();
        }

        public ActionResult Create(string returnUrl)
        {
            ViewBag.Genders = Gender();
            ViewBag.CheckPoints = CheckPoints();
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string returnUrl, CreateGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var group = mapper.Map<CreateGroupModel, GroupVisitorDTO>(model);
              
                var result = await GroupService.Create(group);
                if (result.Succedeed)
                {
                    if (returnUrl == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else return Redirect(returnUrl);
                }
                else ModelState.AddModelError("", result.Message);
            }
            ViewBag.Genders = Gender();
            ViewBag.CheckPoints = CheckPoints();
            ViewBag.returnUrl = returnUrl;
            return View();
        }     

        private SelectList CheckPoints()
        {
            string[] list = new string[]
            {
                "",
                "Брест (Тересполь)",
                "Домачево (Словатичи)",
                "Песчатка (Половцы)",
                "Переров (Беловежа)",
                "Аэропорт Брест"
            };

            return new SelectList(list);
        }

        private SelectList Gender()
        {
            string[] list = new string[]
            {
                "",
                "Мужчина",
                "Женщина",
            };

            return new SelectList(list);
        }
    }
}
