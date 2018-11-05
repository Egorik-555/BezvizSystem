using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.Pogranec.Web.Models.Anketa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers
{
    [Authorize(Roles = "GPKSuperAdmin, GPKAdmin, GPKMiddle, GPKUser")]
    public class AnketaController : Controller
    {
        IService<AnketaDTO> _anketaService;
        IXmlCreator _xmlService;

        public AnketaController(IService<AnketaDTO> anketaService, IXmlCreator xmlService)
        {
            _anketaService = anketaService;
            _xmlService = xmlService;
        }

        public ActionResult Index()
        {
            var date = DateTime.Now.Date;
            var list = _anketaService.GetAll();
            var group = list.Where(g => g.DateArrival.Value.Date == date).
                             GroupBy(a => a.CheckPoint, a => a.CountMembers).
                             Select(a => new ArrivedInfo { CheckPoint = a.Key, Count = a.Sum()}).ToList();

            var model = new ArrivedPerson { Infoes = group, Count = group.Sum(a => a.Count) };

            return View(model);
        }
     

        public async Task<ActionResult> GetAnketasDefault()
        {
            string file = HostingEnvironment.MapPath("~/App_Data/fileDefault.xml");
            var result = await _xmlService.SaveNew(file);

            string contentType = "application/xml";

            if (result.Succedeed)
                return File(file, contentType, Path.GetFileName(file));
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> GetAnketasExtra()
        {
            string file = HostingEnvironment.MapPath("~/App_Data/fileExtra.xml");
            var result = await _xmlService.SaveExtra(file);

            string contentType = "application/xml";

            if (result.Succedeed)
                return File(file, contentType, Path.GetFileName(file));
            else
            {
               return RedirectToAction("Index");
            }
        }
    }
}