using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL;
using BezvizSystem.Web.Models.Operator;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class OperatorController : Controller
    {     
        IUserService UserService
        {
            get { return HttpContext.GetOwinContext().Get<IUserService>(); }
        }

        IMapper mapper;

        public OperatorController()
        {
            IMapper mapperProfile = new MapperConfiguration(cfg => {

                cfg.CreateMap<CreateOperatorModel, ProfileUserDTO>();
                cfg.CreateMap<EditOperatorModel, ProfileUserDTO>();
                cfg.CreateMap<DeleteOperatorModel, ProfileUserDTO>();
                cfg.RecognizePrefixes("ProfileUser");

                //cfg.CreateMap<ProfileUserDTO, EditOperatorModel>();            
            }).CreateMapper();

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, ViewOperatorModel>();
                
                cfg.CreateMap<CreateOperatorModel, UserDTO>().
                   ForMember(dest => dest.ProfileUser, opt => opt.MapFrom(src => mapperProfile.Map<CreateOperatorModel, ProfileUserDTO>(src)));

                cfg.CreateMap<UserDTO, EditOperatorModel>();

                cfg.CreateMap<EditOperatorModel, UserDTO>().
                   ForMember(dest => dest.ProfileUser, opt => opt.MapFrom(src => mapperProfile.Map<EditOperatorModel, ProfileUserDTO>(src)));

                cfg.CreateMap<UserDTO, DeleteOperatorModel>();
                cfg.CreateMap<DeleteOperatorModel, UserDTO>().
                    ForMember(dest => dest.ProfileUser, opt => opt.MapFrom(src => mapperProfile.Map<DeleteOperatorModel, ProfileUserDTO>(src)));

            }).CreateMapper();
        }

        public ActionResult Index()
        {
            var usersDto = UserService.GetByRole("operator");           
            var model = mapper.Map<IEnumerable<UserDTO>, IEnumerable<ViewOperatorModel>>(usersDto);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateOperatorModel model)
        {
            if (ModelState.IsValid)
            {          
                var user = mapper.Map<CreateOperatorModel, UserDTO>(model);
                     
                var result = await UserService.Create(user);
                if (result.Succedeed)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserService.GetByIdAsync(id);         
            if (user == null)
                return RedirectToAction("Index");
       
            var model = mapper.Map<UserDTO, DeleteOperatorModel>(user);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(DeleteOperatorModel model)
        {          
            var user = mapper.Map<DeleteOperatorModel, UserDTO>(model);

            await UserService.Delete(user);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserService.GetByIdAsync(id);
            if (user == null)
                return RedirectToAction("Index");

            var model = mapper.Map<UserDTO, EditOperatorModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditOperatorModel model)
        {
            if (ModelState.IsValid)
            {
                var user = mapper.Map<EditOperatorModel, UserDTO>(model);

                var result = await UserService.Update(user);
                if (result.Succedeed)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View(model);
        }
    }
}