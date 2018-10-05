using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Utils;
using BezvizSystem.Web.Infrustructure;
using BezvizSystem.Web.Mapper;
using BezvizSystem.Web.Models.Anketa;
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
    public class AnketaController : Controller
    {
        IMapper mapper;
        private IService<AnketaDTO> _anketaService;
        private IService<GroupVisitorDTO> _groupService;
        private IDictionaryService<CheckPointDTO> _checkPointService;
        private IDictionaryService<NationalityDTO> _nationalityService;
        private IDictionaryService<GenderDTO> _genderService;

        public AnketaController(IService<AnketaDTO> anketaService, IService<GroupVisitorDTO> groupService,
                                IDictionaryService<CheckPointDTO> checkPointService, IDictionaryService<NationalityDTO> nationalityService,
                                IDictionaryService<GenderDTO> genderService)
        {
            _anketaService = anketaService;
            _groupService = groupService;
            _checkPointService = checkPointService;
            _nationalityService = nationalityService;
            _genderService = genderService;

            mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromBLLToWebProfile())).CreateMapper();
        }
    
        public async Task<ActionResult> Index()
        {
            var anketas = await _anketaService.GetForUserAsync(User != null ? User.Identity.Name : null);
            var model = mapper.Map<IEnumerable<AnketaDTO>, IEnumerable<ViewAnketaModel>>(anketas.OrderBy(m => m.DateArrival));
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> InExcel()
        {         
            var anketas = await _anketaService.GetForUserAsync(User.Identity.Name);

            //список всех туристов
            var visitors = new List<ViewAnketaExcel>();
            foreach (var item in anketas)
            {
                foreach (var visitor in item.Visitors)
                {
                    var v = mapper.Map<AnketaDTO, ViewAnketaExcel>(item);
                    v.Surname = visitor.Surname;
                    v.Name = visitor.Name;
                    v.SerialAndNumber = visitor.SerialAndNumber;
                    v.Nationality = visitor.Nationality;
                    v.Gender = visitor.Gender;
                    v.BithDate = visitor.BithDate;
                    visitors.Add(v);
                }
            }
                  
            IExcel print = new Excel();
            string workString = await print.InExcelAsync<ViewAnketaExcel>(visitors);

            return new ExcelResult("Зарегистрированные анкеты.xls", workString);
        }


        public async Task<ActionResult> Edit(int id)
        {
            var group = await _groupService.GetByIdAsync(id);
            if (group == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Genders = Gender();
            ViewBag.CheckPoints = CheckPoints();
            ViewBag.Nationalities = Nationalities();

            if (!group.Group)
            {
                var model = mapper.Map<GroupVisitorDTO, EditVisitorModel>(group);
                return View("EditVisitor", model);
            }
            else
            {
                var model = mapper.Map<GroupVisitorDTO, EditGroupModel>(group);             
                return View("EditGroup", model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditVisitor(EditVisitorModel model, string button)
        {

            if (ModelState.IsValid)
            {
                model.UserEdit = User.Identity.Name;
                if (button == "Extra") model.ExtraSend = true;
                else model.ExtraSend = false;

                var visitor = mapper.Map<EditVisitorModel, GroupVisitorDTO>(model);
                var result = await _groupService.Update(visitor);

                if (result.Succedeed)
                {
                    return RedirectToAction("Index");
                }
                else ModelState.AddModelError("", result.Message);
            }

            ViewBag.Genders = Gender();
            ViewBag.CheckPoints = CheckPoints();
            ViewBag.Nationalities = Nationalities();           
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditGroup(EditGroupModel model, string button)
        {
            if (ModelState.IsValid)
            {
                model.UserEdit = User.Identity.Name;
                if (button == "Extra") model.ExtraSend = true;
                else model.ExtraSend = false;

                var group = mapper.Map<EditGroupModel, GroupVisitorDTO>(model);
                var result = await _groupService.Update(group);

                if (result.Succedeed)
                {
                    return RedirectToAction("Index");
                }
                else ModelState.AddModelError("", result.Message);
            }

            ViewBag.Genders = Gender();
            ViewBag.CheckPoints = CheckPoints();
            ViewBag.Nationalities = Nationalities();
            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var group = await _groupService.GetByIdAsync(id);
            if (group == null)
                return RedirectToAction("Index");

            var model = mapper.Map<GroupVisitorDTO, ViewAnketaModel>(group);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ViewAnketaModel model)
        {
            var group = mapper.Map<ViewAnketaModel, GroupVisitorDTO>(model);
            await _groupService.Delete(group.Id);
            return RedirectToAction("Index");
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