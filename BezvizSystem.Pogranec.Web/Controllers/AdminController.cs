using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Pogranec.Web.Infrastructure;
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

    [Authorize(Roles = "GPKAdmin")]
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
            var usersDto = _userService.GetAll().Where(u => u.UserName.ToUpper() != User.Identity.Name.ToUpper()).Where(u => u.UserName.ToUpper() != "Pogranec".ToUpper());
            usersDto = usersDto.OrderByDescending(m => m.ProfileUser.Transcript);
            var model = _mapper.Map<IEnumerable<UserDTO>, IEnumerable<DisplayUser>>(usersDto);
            if (!string.IsNullOrEmpty(id))
            {
                CleverSeach(id, ref model);
            }

            int pageSize = 10;
            var modelForPaging = model.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = model.Count() };
            IndexViewModel<DisplayUser> ivm = new IndexViewModel<DisplayUser> { PageInfo = pageInfo, Models = modelForPaging };

            return PartialView(ivm);
        }

        private void CleverSeach(string id, ref IEnumerable<DisplayUser> model)
        {
            var temp = new List<DisplayUser>(model);

            model = model.Where(m => m.UserName.ToUpper().Contains(id.ToUpper()));

            if (model.Count() == 0)
            {
                model = temp.Where(m => m.ProfileUserTranscript.ToUpper().Contains(id.ToUpper()));
            }

            if (model.Count() == 0)
            {
                model = temp.Where(m => m.ProfileUserIp.ToUpper().Contains(id.ToUpper()));
            }
        }

        private SelectList GetLevels()
        {
            List<string> list = new List<string>(Enum.GetNames(typeof(UserLevel)));
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (var item in list)
            {
                if (item == UserLevel.GPKAdmin.ToString())
                    result.Add(new SelectListItem {Text = "Админ", Value = item});
                else if (item == UserLevel.GPKMiddle.ToString())
                    result.Add(new SelectListItem { Text = "Средний уровень", Value = item });       
                else if (item == UserLevel.GPKUser.ToString())
                    result.Add(new SelectListItem { Text = "Пользователь", Value = item });
                else result.Add(new SelectListItem { Text = item, Value = item });
            }

            return new SelectList(result, "Value", "Text", UserLevel.GPKUser);
        }

        public ActionResult Create()
        {
            ViewBag.Levels = GetLevels();
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
            ViewBag.Levels = GetLevels();
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return RedirectToAction("Index");

            var model = _mapper.Map<UserDTO, DeleteUser>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(DeleteUser model)
        {
            var user = _mapper.Map<DeleteUser, UserDTO>(model);

            await _userService.Delete(user);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return RedirectToAction("Index");

            ViewBag.Levels = GetLevels();
            var model = _mapper.Map<UserDTO, EditUser>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUser model)
        {
            if (ModelState.IsValid)
            {
                model.ProfileUserUserEdit = User.Identity.Name;
                var user = _mapper.Map<EditUser, UserDTO>(model);
                var userProfile = _mapper.Map<EditUser, ProfileUserDTO>(model);
                user.ProfileUser = userProfile;

                var result = await _userService.Update(user);
                if (result.Succedeed)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            ViewBag.Levels = GetLevels();
            return View(model);
        }

    }
}