using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Infrustructure
{
    public class DocumentResult : ActionResult
    {
        private MemoryStream stream;
        private string fileName;

        public DocumentResult(MemoryStream stream, string fileName)
        {
            this.stream = stream;
            this.fileName = fileName;
        }

        
        public override void ExecuteResult(ControllerContext context)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(stream.ToArray());       
            HttpContext.Current.Response.End();
        }
    }
}