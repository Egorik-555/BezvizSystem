using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL;
using BezvizSystem.Web.Mapper;
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

        IMapper _mapper;

        public OperatorController()
        {        
            _mapper = new MapperConfiguration(cfg => 
                cfg.AddProfile(new FromBLLToWebProfile())).
                    CreateMapper();
        }

        public ActionResult Index()
        {     
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id)
        {
            return View((object)id);
        }

        public ActionResult DataOperators(string id)
        {
            var usersDto = UserService.GetByRole("operator").OrderByDescending(m => m.ProfileUser.DateInSystem);
            var model = _mapper.Map<IEnumerable<UserDTO>, IEnumerable<ViewOperatorModel>>(usersDto);         
            if (!string.IsNullOrEmpty(id))
            {
                model = model.Where(m => m.ProfileUserTranscript.ToUpper().Contains(id.ToUpper()));
            }
            return PartialView(model);
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
                var user = _mapper.Map<CreateOperatorModel, UserDTO>(model);
                var userProfile = _mapper.Map<CreateOperatorModel, ProfileUserDTO>(model);
                user.ProfileUser = userProfile;

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
       
            var model = _mapper.Map<UserDTO, DeleteOperatorModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(DeleteOperatorModel model)
        {          
            var user = _mapper.Map<DeleteOperatorModel, UserDTO>(model);

            await UserService.Delete(user);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserService.GetByIdAsync(id);
            if (user == null)
                return RedirectToAction("Index");
            var model = _mapper.Map<UserDTO, EditOperatorModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditOperatorModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<EditOperatorModel, UserDTO>(model);
                var userProfile = _mapper.Map<EditOperatorModel, ProfileUserDTO>(model);
                user.ProfileUser = userProfile;

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