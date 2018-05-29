using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
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
    public class AnketaController : Controller
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

        private IDictionaryService<CheckPointDTO> CheckPointService
        {
            get { return HttpContext.GetOwinContext().Get<IDictionaryService<CheckPointDTO>>(); }
        }

        private IDictionaryService<NationalityDTO> NationalityService
        {
            get { return HttpContext.GetOwinContext().Get<IDictionaryService<NationalityDTO>>(); }
        }

        public AnketaController()
        {
            IMapper visitorMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VisitorDTO, InfoVisitorModel>();
                cfg.CreateMap<InfoVisitorModel, VisitorDTO>();
            }
            ).CreateMapper();

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnketaDTO, ViewAnketaModel>().
                    ForMember(dest => dest.DateArrival, opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.Date : src.DateArrival));

                cfg.CreateMap<GroupVisitorDTO, CreateVisitorModel>().
                    ForMember(dest => dest.Info, opt => opt.MapFrom(src =>
                                        visitorMapper.Map<IEnumerable<VisitorDTO>, IEnumerable<InfoVisitorModel>>(src.Visitors).FirstOrDefault()));

                cfg.CreateMap<CreateVisitorModel, GroupVisitorDTO>().
                    ForMember(dest => dest.Visitors, opt => opt.MapFrom(src => 
                        new List<VisitorDTO> { visitorMapper.Map<InfoVisitorModel, VisitorDTO>(src.Info)}));

                cfg.CreateMap<GroupVisitorDTO, CreateGroupModel>().
                    ForMember(dest => dest.Infoes, opt => opt.MapFrom(src => src.Visitors));
                cfg.CreateMap<VisitorDTO, InfoVisitorModel>();

                cfg.CreateMap<CreateGroupModel, GroupVisitorDTO>().
                    ForMember(dest => dest.Visitors, opt => opt.MapFrom(src => src.Infoes));
                cfg.CreateMap<InfoVisitorModel, VisitorDTO>();

                cfg.CreateMap<GroupVisitorDTO, ViewAnketaModel>().
                    ForMember(dest => dest.CountMembers, opt => opt.MapFrom(src => src.Visitors.Count()));
                cfg.CreateMap<ViewAnketaModel, GroupVisitorDTO>();
            }).CreateMapper();
        }

        public ActionResult Index()
        {
            var anketas = AnketaService.GetAll();
            var model = mapper.Map<IEnumerable<AnketaDTO>, IEnumerable<ViewAnketaModel>>(anketas);
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var group = await GroupService.GetByIdAsync(id);
            if (group == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Genders = Gender();
            ViewBag.CheckPoints = CheckPoints();
            ViewBag.Nationalities = Nationalities();
            ViewBag.VisitorsList = group.Visitors;

            if (!group.Group)
            {
                var model = mapper.Map<GroupVisitorDTO, CreateVisitorModel>(group);
                return View("EditVisitor", model);
            }
            else
            {
                var model = mapper.Map<GroupVisitorDTO, CreateGroupModel>(group);             
                return View("EditGroup", model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditVisitor(CreateVisitorModel model)
        {
            if (ModelState.IsValid)
            {
                var visitor = mapper.Map<CreateVisitorModel, GroupVisitorDTO>(model);
                var result = await GroupService.Update(visitor);

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
        public async Task<ActionResult> EditGroup(CreateGroupModel model, ICollection<InfoVisitorModel> infoes)
        {
            if (ModelState.IsValid)
            {
                var visitor = mapper.Map<CreateGroupModel, GroupVisitorDTO>(model);
                var result = await GroupService.Update(visitor);

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
            var group = await GroupService.GetByIdAsync(id);
            if (group == null)
                return RedirectToAction("Index");

            var model = mapper.Map<GroupVisitorDTO, ViewAnketaModel>(group);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ViewAnketaModel model)
        {
            var group = mapper.Map<ViewAnketaModel, GroupVisitorDTO>(model);
            await GroupService.Delete(group.Id);
            return RedirectToAction("Index");
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