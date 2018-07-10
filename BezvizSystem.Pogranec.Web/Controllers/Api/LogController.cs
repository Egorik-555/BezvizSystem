using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "pogranecAdmin, pogranec")]
    public class LogController : ApiController
    {
        ILogger<UserActivityDTO> _loggerService;

        public LogController(ILogger<UserActivityDTO> service)
        {
            _loggerService = service;
        }

        public HttpResponseMessage Post([FromBody] UserActivityDTO item)
        {
            //Add item with _loggerService
            var result = _loggerService.Insert(item);

            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.Created, result);
            // var msg = Request.CreateResponse(HttpStatusCode.BadRequest);

            // Location заголовок стоит создавать, если новый элемент был создан
            //msg.Headers.Location = new Uri(Request.RequestUri + "/" + (_fruits.Count - 1));

            return msg;
        }


    }
}