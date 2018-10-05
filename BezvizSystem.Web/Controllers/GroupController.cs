using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Mapper;
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
    [Authorize(Roles = "admin, operator")]
    public class GroupController : Controller
    {
        private IService<GroupVisitorDTO> _groupService;
        private IDictionaryService<CheckPointDTO> _checkPointService;
        private IDictionaryService<NationalityDTO> _nationalityService;
        private IDictionaryService<GenderDTO> _genderService;

        IMapper mapper;

        public GroupController(IService<GroupVisitorDTO> groupService, IDictionaryService<CheckPointDTO> checkPointService,
                               IDictionaryService<NationalityDTO> nationalityService, IDictionaryService<GenderDTO> genderService)
        {
            _groupService = groupService;
            _checkPointService = checkPointService;
            _nationalityService = nationalityService;
            _genderService = genderService;
            mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromBLLToWebProfile())).CreateMapper();
        }

        public ActionResult Create()
        {
            ViewBag.Genders = Gender();
            ViewBag.CheckPoints = CheckPoints();
            ViewBag.Nationalities = Nationalities();

            var model = new CreateGroupModel();
            model.Infoes.Add(new InfoVisitorModel());
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateGroupModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserInSystem = User.Identity.Name;
                model.Group = true;
                var group = mapper.Map<CreateGroupModel, GroupVisitorDTO>(model);
                
                var result = await _groupService.Create(group);
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
            List<string> list = new List<string>(_checkPointService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return new SelectList(list, "");
        }

        private SelectList Nationalities()
        {
            List<string> list = new List<string>(_nationalityService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return new SelectList(list, "");
        }

        private SelectList Gender()
        {
            List<string> list = new List<string>(_genderService.Get().Select(c => c.Name));
            list.Insert(0, "");
            return new SelectList(list, "");
        }
    }
}
