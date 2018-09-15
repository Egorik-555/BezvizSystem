using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Utils;
using BezvizSystem.DAL.Identity;
using BezvizSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _service;

        private IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private BezvizUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<BezvizUserManager>(); }
        }

        public AccountController(IUserService service)
        {
            _service = service;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _service.GetByNameAsync(model.UNP);
                if (user == null)
                {
                    ModelState.AddModelError("", "Туроператор с УНП - " + model.UNP + " не найден");
                    return View(model);
                }

                if (user.ProfileUser.OKPO != model.OKPO)
                {
                    ModelState.AddModelError("", "Туроператор с ОКПО - " + model.OKPO + " не найден");
                    return View(model);
                }

                if (!user.ProfileUser.Active)
                {
                    ModelState.AddModelError("", "Туроператор с УНП - " + user.ProfileUser.UNP + " заблокирован");
                    return View(model);
                }

                if (user.Email != null && user.EmailConfirmed)
                {
                    ModelState.AddModelError("", "Туроператор с УНП - " + user.ProfileUser.UNP + " уже зарегистрирован");
                    return View(model);
                }

                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { token = user.Id, email = model.Email },
                                              protocol: Request.Url.Scheme);

                _service.ManagerForChangePass = UserManager;
                var result = await _service.Registrate(user, callbackUrl, new SimpleGeneratePass());

                if (result.Succedeed)
                    return View("ConfirmEmail");
                else ModelState.AddModelError("", result.Message);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string token, string email)
        {
            if (token == null || email == null)
            {
                var result = View("Register");
                result.ViewData.ModelState.AddModelError("", "Неверные данные пользователя");
                return result;
            }

            var user = await _service.GetByIdAsync(token);
            if (user == null)
            {
                var result = View("Register");
                result.ViewData.ModelState.AddModelError("", "Неверные данные пользователя");
                return result;
            }

            user.Email = email;
            user.EmailConfirmed = true;
            var resultUpdate = await _service.Update(user);

            if (!resultUpdate.Succedeed)
            {
                var result = View("Register");
                result.ViewData.ModelState.AddModelError("", resultUpdate.Message);
                return result;
            }

            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                await SetInitDataAsync();

                UserDTO user = new UserDTO { UserName = model.Name, Password = model.Password };
                var claim = await _service.Authenticate(user);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                    return View(model);
                }

                var findUser = await _service.GetByNameAsync(user.UserName);

                if (!findUser.ProfileUser.Active)
                {
                    ModelState.AddModelError("", "Пользователь заблокирован");
                    return View(model);
                }

                if (findUser.ProfileUser.Role == "operator" && !findUser.EmailConfirmed)
                {
                    ModelState.AddModelError("", "Email не подтвержден");
                    return View(model);
                }

                Authentication.SignOut();
                Authentication.SignIn(new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
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

        [NonAction]
        public async Task SetInitDataAsync()
        {
            var user = new UserDTO
            {
                UserName = "Admin",
                Password = "rgg777",
                ProfileUser = new ProfileUserDTO { Role = "admin", Active = true, Transcript = "Брестский облисполком", DateInSystem = DateTime.Now, UserInSystem = "Autoinitilize" }
            };

            var roles = new List<string> { "admin", "operator" };
            await _service.SetInitialData(user, roles);
        }
    }
}