using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Mapper;
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

        public IService<GroupVisitorDTO> GroupService
        {
            get { return HttpContext.GetOwinContext().Get<IService<GroupVisitorDTO>>(); }
            set { GroupService = value; }
        }

        public IDictionaryService<CheckPointDTO> CheckPointService
        {
            get { return HttpContext.GetOwinContext().Get<IDictionaryService<CheckPointDTO>>(); }
            set { CheckPointService = value; }
        }

        public IDictionaryService<NationalityDTO> NationalityService
        {
            get { return HttpContext.GetOwinContext().Get<IDictionaryService<NationalityDTO>>(); }
            set { NationalityService = value; }
        }

        public IDictionaryService<GenderDTO> GenderService
        {
            get { return HttpContext.GetOwinContext().Get<IDictionaryService<GenderDTO>>(); }
            set { GenderService = value; }
        }

        IMapper mapper;

        public VisitorController()
        {       
            mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromBLLToWebProfile())).CreateMapper();
        }

        public ActionResult Create()
        {
            ViewBag.Genders = Gender();
            ViewBag.CheckPoints = CheckPoints();
            ViewBag.Nationalities = Nationalities();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateVisitorModel model)
        {
            if (ModelState.IsValid)
            {
                var visitor = mapper.Map<CreateVisitorModel, GroupVisitorDTO>(model);
                var result = await GroupService.Create(visitor);

                if (result.Succedeed)
                {
                    return RedirectToAction("Index", "Home");
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
            return new SelectList(list, "");
        }

        private SelectList Nationalities()
        {
            List<string> list = new List<string>(NationalityService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return new SelectList(list, "");
        }

        private SelectList Gender()
        {
            List<string> list = new List<string>(GenderService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return new SelectList(list, "");
        }
    }
}
