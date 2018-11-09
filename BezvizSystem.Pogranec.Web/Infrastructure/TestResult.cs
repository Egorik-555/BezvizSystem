using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Infrustructure
{
    public class TestResult : ActionResult
    {
        public TestResult(/*string fileName, string report*/)
        {
            //this.Report = report;
            //this.FileName = fileName;
        }

        public string FileName { get; private set; }
        public string Report { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            XLWorkbook workbook = new XLWorkbook();
            workbook.Worksheets.Add("sample").Cell(1, 1).SetValue("Hello");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=\"HelloWorld.xlsx\"");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                memoryStream.Close();
            }


                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                //HttpContext.Current.Response.BufferOutput = true;
                //HttpContext.Current.Response.AddHeader("content-disposition",
                //    string.Format("attachment; filename = {0}", FileName));
                //HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                //HttpContext.Current.Response.Charset = "utf-8";
                //HttpContext.Current.Response.Write(Report);
            HttpContext.Current.Response.End();
        }
    }
}