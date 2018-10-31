using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Pogranec.Web.Mapper;
using BezvizSystem.Pogranec.Web.Models.Admin;
using BezvizSystem.Pogranec.Web.Views.Helpers.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers
{

    [Authorize(Roles = "pogranecAdmin")]
    public class AdminController : Controller
    { 
        IUserService _userService;
        IMapper _mapper;

        public AdminController(IUserService userService)
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

        public ActionResult DataUsers(string id, int page = 1)
        {
            ViewBag.Id = id;
            var usersDto = _userService.GetByRole("pogranec").OrderByDescending(m => m.ProfileUser.Transcript);
            var model = _mapper.Map<IEnumerable<UserDTO>, IEnumerable<DisplayUser>>(usersDto);
            if (!string.IsNullOrEmpty(id))
            {
                model = model.Where(m => m.ProfileUserTranscript.ToUpper().Contains(id.ToUpper()));
            }

            int pageSize = 3;
            var modelForPaging = model.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = model.Count() };
            IndexViewModel<DisplayUser> ivm = new IndexViewModel<DisplayUser> { PageInfo = pageInfo, Models = modelForPaging };

            return PartialView(ivm);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                model.UserInSystem = User.Identity.Name;
                model.DateInSystem = DateTime.Now;
                var user = _mapper.Map<CreateUser, UserDTO>(model);
                var userProfile = _mapper.Map<CreateUser, ProfileUserDTO>(model);
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

        //public async Task<ActionResult> Delete(string id)
        //{
        //    var user = await _userService.GetByIdAsync(id);
        //    if (user == null)
        //        return RedirectToAction("Index");

        //    var model = _mapper.Map<UserDTO, DeleteOperatorModel>(user);
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Delete(DeleteOperatorModel model)
        //{
        //    var user = _mapper.Map<DeleteOperatorModel, UserDTO>(model);

        //    await _userService.Delete(user);
        //    return RedirectToAction("Index");
        //}

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