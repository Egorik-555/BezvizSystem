using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL;
using BezvizSystem.Web.Mapper;
using BezvizSystem.Web.Models.Operator;
using BezvizSystem.Web.Views.Helpers.Pagging;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    [Authorize(Roles = "OBLSuperAdmin, OBLAdmin")]
    public class OperatorController : Controller
    {
        IUserService _userService;
        IMapper _mapper;

        public OperatorController(IUserService userService)
        {
            _userService = userService;
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromBLLToWebProfile())).CreateMapper();
        }

        //[HttpPost]
        public ActionResult Index(string id)
        {
            ViewBag.Id = id;
            return View((object)id);
        }

        public ActionResult DataOperators(string id, int page = 1)
        {
            ViewBag.Id = id;
            var usersDto = _userService.GetByRole("operator").OrderByDescending(m => m.ProfileUser.DateInSystem);
            var model = _mapper.Map<IEnumerable<UserDTO>, IEnumerable<ViewOperatorModel>>(usersDto);         
            if (!string.IsNullOrEmpty(id))
            {
                CleverSeach(id, ref model);
                //model = model.Where(m => m.ProfileUserTranscript.ToUpper().Contains(id.ToUpper()));
            }

            int pageSize = 10;
            var modelForPaging = model.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = model.Count()};
            IndexViewModel<ViewOperatorModel> ivm = new IndexViewModel<ViewOperatorModel> {PageInfo = pageInfo, Models = modelForPaging };

            return PartialView(ivm);
        }

        private void CleverSeach(string id, ref IEnumerable<ViewOperatorModel> model)
        {
            var temp = new List<ViewOperatorModel>(model);

            model = model.Where(m => m.ProfileUserTranscript.ToUpper().Contains(id.ToUpper()));

            if (model.Count() == 0)
            {
                model = temp.Where(m => m.ProfileUserUNP.ToUpper().Contains(id.ToUpper()));
            }

            if (model.Count() == 0)
            {
                model = temp.Where(m => m.ProfileUserOKPO.ToUpper().Contains(id.ToUpper()));
            }
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
                model.UserInSystem = User.Identity.Name;
                model.DateInSystem = DateTime.Now;
                var user = _mapper.Map<CreateOperatorModel, UserDTO>(model);
                var userProfile = _mapper.Map<CreateOperatorModel, ProfileUserDTO>(model);
                user.ProfileUser = userProfile;

                var result = await _userService.Create(user);
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
            var user = await _userService.GetByIdAsync(id);         
            if (user == null)
                return RedirectToAction("Index");
       
            var model = _mapper.Map<UserDTO, DeleteOperatorModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(DeleteOperatorModel model)
        {          
            var user = _mapper.Map<DeleteOperatorModel, UserDTO>(model);

            await _userService.Delete(user);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userService.GetByIdAsync(id);
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
                model.ProfileUserUserEdit = User.Identity.Name;
                var user = _mapper.Map<EditOperatorModel, UserDTO>(model);
                var userProfile = _mapper.Map<EditOperatorModel, ProfileUserDTO>(model);
                user.ProfileUser = userProfile;

                var result = await _userService.Update(user);
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