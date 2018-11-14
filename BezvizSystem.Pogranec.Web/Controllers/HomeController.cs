using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Pogranec.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BezvizSystem.Pogranec.Web.Infrastructure;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.Pogranec.Web.Filters;

namespace BezvizSystem.Pogranec.Web.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private IUserService _userService;


        public HomeController(IUserService service)
        {
            _userService = service;
        }


        public ActionResult Login()
        {
            return View();
        }

        [LoginException]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            string errorMsg = null;
                
            if (ModelState.IsValid)
            {
                await SetInitDataAsync();

                UserDTO user = new UserDTO { UserName = model.Name, Password = model.Password };
                var claim = await _userService.Authenticate(user);
                if (claim == null)
                {
                    errorMsg = "Неверный логин или пароль";
                    TempData["errorMsg"] = errorMsg;
                    TempData["userName"] = model.Name;
                    ModelState.AddModelError("", errorMsg);
                    return View(model);
                }

                var findUser = await _userService.GetByNameAsync(user.UserName);

                if (!findUser.ProfileUser.Active && (!findUser.ProfileUser.NotActiveToDate.HasValue || findUser.ProfileUser.NotActiveToDate > DateTime.Now))
                {
                    errorMsg = "Пользователь заблокирован";
                    TempData["errorMsg"] = errorMsg;
                    TempData["userName"] = model.Name;
                    ModelState.AddModelError("", errorMsg);
                    return View(model);
                }

                Authentication.SignOut();
                Authentication.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true,
                }, claim);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public ActionResult Logout()
        {          
            Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "GPKSuperAdmin, GPKAdmin, GPKMiddle, GPKUser")]
        [ActionLogger(Type = DAL.Helpers.LogType.Enter, TextActivity = "Вход в систему")]
        public ActionResult Index()
        {
            return View();
        }

        private async Task SetInitDataAsync()
        {
            var user = new UserDTO
            {
                UserName = "Pogranec",
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { Role = UserLevel.GPKSuperAdmin.ToString(), Active = true, DateInSystem = DateTime.Now, UserInSystem = "Autoinitilize" }
            };

            var roles = new List<string> { "GPKSuperAdmin", "GPKAdmin", "GPKMiddle", "GPKUser" };
            await _userService.SetInitialData(user, roles);
        }
    }
}