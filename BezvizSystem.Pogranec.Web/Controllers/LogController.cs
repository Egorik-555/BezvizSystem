using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Interfaces.Log;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.Pogranec.Web.Mapper;
using BezvizSystem.Pogranec.Web.Models.Log;
using BezvizSystem.Pogranec.Web.Views.Helpers.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LogController(ILogger logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _mapper = new MapperPogranecConfiguration().CreateMapper();
        }

        [Authorize(Roles = "GPKSuperAdmin, GPKAdmin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "GPKSuperAdmin, GPKAdmin")]
        public ActionResult DataLogs(string id, int page = 1)
        {
            ViewBag.Id = id;
            var user = _userService.GetByName(User.Identity.Name);

            if (user == null) return new EmptyResult();
          
            string roleInString = _userService.GetByName(User.Identity.Name).ProfileUser.Role;
            var role = (UserLevel)Enum.Parse(typeof(UserLevel), roleInString);

            var logs = _logger.GetForRole(role);

            logs = logs.OrderByDescending(m => m.DateActivity);
            var model = _mapper.Map<IEnumerable<LogDTO>, IEnumerable<LogModel>>(logs);

            if (!String.IsNullOrEmpty(id))
                model = model.Where(m => m.UserName.ToUpperInvariant() == id.ToUpperInvariant());
            
            //if (!string.IsNullOrEmpty(id))
            //{
            //    CleverSeach(id, ref model);
            //}

            int pageSize = 3;
            var modelForPaging = model.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = model.Count() };
            IndexViewModel<LogModel> ivm = new IndexViewModel<LogModel> { PageInfo = pageInfo, Models = modelForPaging };

            return PartialView(ivm);
        }
    }
}