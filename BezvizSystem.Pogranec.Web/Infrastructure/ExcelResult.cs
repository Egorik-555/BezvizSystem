using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Infrustructure
{
    public class ExcelResult : ActionResult
    {
        public ExcelResult(string fileName, XLWorkbook book)
        {
            this.Book = book;
            this.FileName = fileName;
        }

        public string FileName { get; private set; }
        public XLWorkbook Book { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=\"" + FileName + "\"");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Book.SaveAs(memoryStream);
                memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                memoryStream.Close();
            }          
            HttpContext.Current.Response.End();
        }
    }
}