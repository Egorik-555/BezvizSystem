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
using ClosedXML.Excel;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    [Authorize(Roles = "OBLSuperAdmin, OBLAdmin, OBLUser")]
    public class AnketaController : Controller
    {
        IMapper mapper;
        private IService<AnketaDTO> _anketaService;
        private IService<GroupVisitorDTO> _groupService;
        private IDictionaryService<CheckPointDTO> _checkPointService;
        private IDictionaryService<NationalityDTO> _nationalityService;
        private IDictionaryService<GenderDTO> _genderService;
        IDocumentGenerator _document;

        public AnketaController(IService<AnketaDTO> anketaService, IService<GroupVisitorDTO> groupService,
                                IDictionaryService<CheckPointDTO> checkPointService, IDictionaryService<NationalityDTO> nationalityService,
                                IDictionaryService<GenderDTO> genderService,
                                IDocumentGenerator document)
        {
            _anketaService = anketaService;
            _groupService = groupService;
            _checkPointService = checkPointService;
            _nationalityService = nationalityService;
            _genderService = genderService;
            _document = document;

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
                  
            IExcel<string> print = new Excel();
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

        private MemoryStream GetMemoryDocumentVisitor(string name, GroupVisitorDTO visitor)
        {
            string template = Server.MapPath(name);
            XLWorkbook book = _document.GenerateDocumentVisitor(template, visitor);
            MemoryStream stream = new MemoryStream();
            book.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }

        private MemoryStream GetMemoryDocumentGroup(string name, GroupVisitorDTO visitor)
        {
            string template = Server.MapPath(name);
            XLWorkbook book = _document.GenerateDocumentGroup(template, visitor);
            MemoryStream stream = new MemoryStream();
            book.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }

        [HttpPost]
        public async Task<ActionResult> EditVisitor(EditVisitorModel model, string button)
        {

            if (ModelState.IsValid)
            {
                model.UserEdit = User.Identity.Name;
                var visitor = mapper.Map<EditVisitorModel, GroupVisitorDTO>(model);

                if (button == "Extra") model.ExtraSend = true;
                else model.ExtraSend = false;

                if (button == "Document")
                {
                    return new DocumentResult(GetMemoryDocumentVisitor("~/App_Data/templateVisitor.xlsx", visitor), "Документ на посещение.xlsx");
                }
                
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
                var group = mapper.Map<EditGroupModel, GroupVisitorDTO>(model);
                model.Group = true;

                if (button == "Document")
                {
                    return new DocumentResult(GetMemoryDocumentGroup("~/App_Data/templateGroup.xlsx", group), "Документ на посещение.xlsx");
                }

                if (button == "Extra") model.ExtraSend = true;
                else model.ExtraSend = false;

                          
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