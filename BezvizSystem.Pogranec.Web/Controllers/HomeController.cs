﻿using BezvizSystem.BLL.DTO;
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

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                await SetInitDataAsync();

                UserDTO user = new UserDTO { UserName = model.Name, Password = model.Password };
                var claim = await _userService.Authenticate(user);
                if (claim != null)
                {
                    var findUser = await _userService.GetByNameAsync(user.UserName);

                    if (findUser.ProfileUser.Active)
                    {
                        Authentication.SignOut();
                        Authentication.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                        }, claim);

                        return Redirect
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь заблокирован");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }

            }
            return View();
        }

        [Authorize(Roles = "pogranecAdmin, pogranec") ]
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
                ProfileUser = new ProfileUserDTO { Role = "pogranecAdmin", Active = true, DateInSystem = DateTime.Now, UserInSystem = "Autoinitilize" }
            };

            var roles = new List<string> { "pogranecAdmin", "pogranec" };
            await _userService.SetInitialData(user, roles);
        }
    }
}