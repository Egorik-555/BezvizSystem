﻿using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
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
    [Authorize(Roles = "admin, operator")]
    public class VisitorController : Controller
    {

        private IService<GroupVisitorDTO> GroupService
        {
            get { return HttpContext.GetOwinContext().Get<IService<GroupVisitorDTO>>(); }
        }

        private IDictionaryService<CheckPointDTO> CheckPointService
        {
            get { return HttpContext.GetOwinContext().Get<IDictionaryService<CheckPointDTO>>(); }
        }

        private IDictionaryService<NationalityDTO> NationalityService
        {
            get { return HttpContext.GetOwinContext().Get<IDictionaryService<NationalityDTO>>(); }
        }

        IMapper mapper;

        public VisitorController()
        {
            IMapper visitorMapper = new MapperConfiguration(cfg => cfg.CreateMap<InfoVisitorModel, VisitorDTO>()).CreateMapper();

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateVisitorModel, GroupVisitorDTO>().
                    ForMember(dest => dest.Visitors, opt => opt.MapFrom(
                        src => new List<VisitorDTO> { visitorMapper.Map<InfoVisitorModel, VisitorDTO>(src.Info) }));

                cfg.CreateMap<GroupVisitorDTO, CreateVisitorModel>();
                cfg.CreateMap<InfoVisitorModel, VisitorDTO>();
            }
            ).CreateMapper();
        }
    

        public ActionResult Create(string returnUrl)
        {
            ViewBag.Genders = Gender();
            ViewBag.CheckPoints = CheckPoints();
            ViewBag.Nationalities = Nationalities();
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string returnUrl, CreateVisitorModel model)
        {
            if (ModelState.IsValid)
            {
                var visitor = mapper.Map<CreateVisitorModel, GroupVisitorDTO>(model);
                var result = await GroupService.Create(visitor);

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
            ViewBag.Nationalities = Nationalities();
            return View(model);
        }

        private SelectList CheckPoints()
        {
            List<string> list = new List<string>(CheckPointService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return  new SelectList(list, "");
        }

        private SelectList Nationalities()
        {
            List<string> list = new List<string>(NationalityService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return new SelectList(list, "");
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
