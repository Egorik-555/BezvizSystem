using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Utils
{
    public class DocumentGenerator : IDocumentGenerator
    {
        private XLWorkbook book;
        private IXLWorksheet sheet;

        private readonly Barcode _barcode;

        public DocumentGenerator(Barcode barcode)
        {
            _barcode = barcode;
        }

        public void AddBarcodePic(int code)
        {
            if (_barcode != null)
            {
                var img = _barcode.GetImage(code);
                Stream str = new MemoryStream();
                img.Save(str, ImageFormat.Png);

                IXLPicture pic = sheet.AddPicture(str, XLPictureFormat.Png)
                    .MoveTo(sheet.Cell("A2").Address)
                    .Scale(0.8, true);
            }
        }

        public XLWorkbook GenerateDocumentGroup(string template, GroupVisitorDTO group)
        {
            book = new XLWorkbook(template);
            sheet = book.Worksheet(1);

            var visitors = group.Visitors;
            var formen = visitors.FirstOrDefault();

            sheet.Cell("L14").Value = group.Name;

            var row = 16;
            if (group.OrganizeForm != null)
            {
                sheet.Cell("B" + row.ToString()).Value = group.OrganizeForm;
                row += 2;
            }

            if (group.NumberOfContract != null)
            {
                var contract = "договор - " + group.NumberOfContract;
                if (group.DateOfContract.HasValue) contract += " от " + group.DateOfContract.Value.ToShortDateString();
                sheet.Cell("B" + row.ToString()).Value = contract;
            }

            sheet.Cell("B26").Value = group.ProgramOfTravel;

            //Руководитель
            sheet.Cell("I29").SetValue(formen?.Surname.ToUpper() + " " + formen?.Name.ToUpper() + ", ");

            row = 31;
            if ((bool)formen?.BithDate.HasValue)
            {
                string dateBith = formen?.BithDate.Value.ToShortDateString() + " дата рождения, ";
                sheet.Cell("B" + row.ToString()).Value = dateBith;
                row += 2;
            }

            if (formen?.Nationality != null)
            {
                string nat = "гражданство - " + formen?.Nationality.ToUpper();               
                string pass = formen?.SerialAndNumber.ToUpper();
                string gender = formen?.Gender.ToUpper();

                var result = nat != null ? nat : null;
                result += pass != null ? ", " + pass : "";
                result += gender != null ? ", " + gender : "";

                sheet.Cell("B" + row.ToString()).Value = result;
            }
         
            //добавление туристов
            row = 36;
            foreach (var visitor in visitors)
            {
                sheet.Row(row).InsertRowsBelow(1);
                row++;
                sheet.Range("B" + row + ":E" + row).Merge();
                sheet.Range("F" + row + ":H" + row).Merge();
                sheet.Range("I" + row + ":L" + row).Merge();
                sheet.Range("M" + row + ":P" + row).Merge();

                sheet.Cell("B" + row.ToString()).Value = visitor.Name.ToUpper() + " " + visitor.Surname.ToUpper();
                sheet.Cell("F" + row.ToString()).Value = visitor.Nationality.ToUpper();
                sheet.Cell("I" + row.ToString()).Value = visitor.SerialAndNumber.ToUpper();
                sheet.Cell("M" + row.ToString()).Value = visitor.BithDate.Value.ToShortDateString();
                sheet.Cell("Q" + row.ToString()).Value = visitor.Gender.ToUpper();
            }

            sheet.Cell("I" + (row + 1).ToString()).Value = group.OtherInfo;
            sheet.Cell("B" + (row + 4).ToString()).Value = group.PlaceOfRecidense;

            AddBarcodePic(group.Id);

            return book;
        }

        public XLWorkbook GenerateDocumentVisitor(string template, GroupVisitorDTO group)
        {
            book = new XLWorkbook(template);
            sheet = book.Worksheet(1);

            var visitor = group.Visitors.FirstOrDefault();

            sheet.Cell("J14").SetValue(visitor?.Surname.ToUpper() + " " + visitor?.Name.ToUpper() + ", ");

            int row = 16;
            if ((bool)visitor?.BithDate.HasValue)
            {
                string dateBith = visitor?.BithDate.Value.ToShortDateString() + " дата рождения, ";
                sheet.Cell("B" + row.ToString()).Value = dateBith;
                row += 2;
            }

            if (visitor?.Nationality != null)
            {
                string nat = "гражданство - " + visitor.Nationality.ToUpper() + ", ";
                sheet.Cell("B" + row.ToString()).Value = nat;
                row += 2;
            }

            if (visitor?.SerialAndNumber != null)
            {
                string pass = visitor.SerialAndNumber.ToUpper() + ", ";
                string gender = visitor?.Gender.ToUpper();
                sheet.Cell("B" + row.ToString()).Value = pass + gender;
                row += 2;
            }

            if (group.DateArrival.HasValue)
            {
                sheet.Cell("F26").Value = group.DateArrival.Value.Day;
                sheet.Cell("H26").Value = group.DateArrival.Value.Month;
                sheet.Cell("J26").Value = group.DateArrival.Value.Year;
            }

            if (group.DateDeparture.HasValue)
            {
                sheet.Cell("L26").Value = group.DateDeparture.Value.Day;
                sheet.Cell("N26").Value = group.DateDeparture.Value.Month;
                sheet.Cell("P26").Value = group.DateDeparture.Value.Year;
            }

            sheet.Cell("B29").Value = group.PlaceOfRecidense;
            sheet.Cell("B32").Value = group.ProgramOfTravel;
            sheet.Cell("I35").Value = group.TimeOfWork;

            sheet.Cell("I36").Value = group.TranscriptUser;

            row = 38;
            string tel = null, site = null;
            if (group.TelNumber != null)
            {
                tel = "тел. " + group.TelNumber + " ";
            }

            if (group.SiteOfOperator != null)
            {
                tel = "сайт - " + group.SiteOfOperator;
            }

            sheet.Cell("B" + row.ToString()).Value = tel + site;
            if (tel != null || site != null) row += 2;

            sheet.Cell("B" + row.ToString()).Value = group.Email;

            AddBarcodePic(group.Id);

            return book;
        }
    }
}
