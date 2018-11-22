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

            var role = _userService.GetRoleByUser(User.Identity.Name);
            var logs = _logger.GetForUserAndRole(User.Identity.Name, role);

            logs = logs.OrderByDescending(m => m.DateActivity);
            var model = _mapper.Map<IEnumerable<LogDTO>, IEnumerable<LogModel>>(logs);           
            
            if (!string.IsNullOrEmpty(id))
            {
                CleverSeach(id, ref model);
            }

            int pageSize = 3;
            var modelForPaging = model.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = model.Count() };
            IndexViewModel<LogModel> ivm = new IndexViewModel<LogModel> { PageInfo = pageInfo, Models = modelForPaging };

            return PartialView(ivm);
        }

        private void CleverSeach(string id, ref IEnumerable<LogModel> model)
        {
            var temp = new List<LogModel>(model);

            model = model.Where(m => m.UserName.ToUpper().Contains(id.ToUpper()));

            if (model.Count() == 0)
            {
                model = temp.Where(m => m.Type.ToUpper().Contains(id.ToUpper()));
            }

            if (model.Count() == 0)
            {
                model = temp.Where(m => m.Ip.ToUpper().Contains(id.ToUpper()));
            }

            if (model.Count() == 0)
            {
                model = temp.Where(m => m.TextActivity.ToUpper().Contains(id.ToUpper()));
            }
        }
    }
}