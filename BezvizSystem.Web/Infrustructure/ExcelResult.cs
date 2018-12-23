using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Infrustructure
{
    //public class ExcelResult : ActionResult
    //{
    //    public ExcelResult(string fileName, string report)
    //    {
    //        this.Report = report;
    //        this.FileName = fileName;
    //    }

    //    public string FileName { get; private set; }
    //    public string Report { get; private set; }

    //    public override void ExecuteResult(ControllerContext context)
    //    {
    //        HttpContext.Current.Response.Clear();
    //        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
    //        HttpContext.Current.Response.BufferOutput = true;
    //        HttpContext.Current.Response.AddHeader("content-disposition",
    //            string.Format("attachment; filename = {0}", FileName));
    //        HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
    //        HttpContext.Current.Response.Charset = "utf-8";
    //        HttpContext.Current.Response.Write(Report);
    //        HttpContext.Current.Response.Flush();
    //        HttpContext.Current.Response.End();
    //    }
    //}

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