using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Infrustructure;
using BezvizSystem.Web.Mapper;
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
    public class GroupController : Controller
    {
        private IService<GroupVisitorDTO> _groupService;
        private IDictionaryService<CheckPointDTO> _checkPointService;
        private IDictionaryService<NationalityDTO> _nationalityService;
        private IDictionaryService<GenderDTO> _genderService;
        IDocumentGenerator _document;

        IMapper mapper;

        public GroupController(IService<GroupVisitorDTO> groupService, IDictionaryService<CheckPointDTO> checkPointService,
                               IDictionaryService<NationalityDTO> nationalityService, IDictionaryService<GenderDTO> genderService,
                               IDocumentGenerator document)
        {
            _groupService = groupService;
            _checkPointService = checkPointService;
            _nationalityService = nationalityService;
            _genderService = genderService;
            _document = document;
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

        private MemoryStream GetMemoryDocument(string name, GroupVisitorDTO visitor)
        {
            string template = Server.MapPath(name);
            XLWorkbook book = _document.GenerateDocumentGroup(template, visitor);
            MemoryStream stream = new MemoryStream();
            book.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateGroupModel model, string button)
        {
            if (ModelState.IsValid)
            {
                model.UserInSystem = User.Identity.Name;
                model.Group = true;
                var group = mapper.Map<CreateGroupModel, GroupVisitorDTO>(model);

                //if click button for load document
                if (button == "Document")
                {
                    return new DocumentResult(GetMemoryDocument("~/App_Data/templateGroup.xlsx", group), "Документ на посещение.xlsx");
                }

                var result = await _groupService.Create(group);
                if (result.Succedeed)
                {
                    return RedirectToAction("Index", "Anketa");
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
