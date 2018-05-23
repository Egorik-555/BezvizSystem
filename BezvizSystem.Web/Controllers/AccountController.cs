﻿using BezvizSystem.BLL.DTO;
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
        //private IUserService


        private IUserService Service
        {
            get { return HttpContext.GetOwinContext().Get<IUserService>(); }
        }

        private IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private BezvizUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<BezvizUserManager>(); }
        }



        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
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
                var user = await Service.GetByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "Оператор с указанным УНП не найден");
                    return View(model);
                }
                // отправка мыла
                if (user.Email == null || !user.EmailConfirmed)
                {                  
                    user.Email = model.Email;                   
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { token = user.Id, email = user.Email },
                                                  protocol: Request.Url.Scheme);
                    Service.ManagerForChangePass = UserManager;
                    var result = await Service.Registrate(user, callbackUrl, new SimpleGeneratePass());

                    if (result.Succedeed)
                        return View("ConfirmEmail");
                    else ModelState.AddModelError("", result.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Оператор с таким УНП зарегистрирован");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string token, string email)
        {
            if (token == null || email == null)
                return RedirectToAction("Index", "Home");
            var user = await Service.GetByIdAsync(token);

            if (user != null)
            {
                if (user.Email == email)
                {
                    user.EmailConfirmed = true;
                    await Service.Update(user);
                }
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string returnUrl, LoginModel model)
        {
            if (ModelState.IsValid)
            {
                await SetInitDataAsync();

                UserDTO user = new UserDTO { UserName = model.Name, Password = model.Password };
                var claim = await Service.Authenticate(user);
                if (claim != null)
                {
                    var findUser = await Service.GetByNameAsync(user.UserName);

                    if ((findUser.ProfileUser.Role == "admin") ||
                            (findUser.ProfileUser.Role == "operator" && findUser.EmailConfirmed))
                    {
                        Authentication.SignOut();
                        Authentication.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true,
                        }, claim);
                        if (returnUrl != null)
                            return Redirect(returnUrl);
                        else return RedirectToAction("Index", "Home");
                    }
                    else if (findUser.ProfileUser.Role == "operator")
                    {
                        ModelState.AddModelError("", "Email не подтвержден");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Logout()
        {
            Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private async Task SetInitDataAsync()
        {
            var user = new UserDTO
            {
                UserName = "Admin",
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { Role = "admin" }
            };

            var roles = new List<string> { "admin", "operator" };
            await Service.SetInitialData(user, roles);
        }
    }
}