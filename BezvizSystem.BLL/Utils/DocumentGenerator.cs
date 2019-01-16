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

            var visitors = group.Visitors.ToList();
            var formen = visitors.FirstOrDefault();

            sheet.Cell("L8").Value = (group.OrganizeForm != null ? group.OrganizeForm + " " : "") + group.Name;

            var row = 10;
            
            if (group.NumberOfContract != null)
            {
                var contract = "договор " + group.NumberOfContract;
                if (group.DateOfContract.HasValue) contract += " от " + group.DateOfContract.Value.ToShortDateString();
                sheet.Cell("B" + row.ToString()).Value = contract;
            }

            sheet.Cell("B20").Value = group.ProgramOfTravel;

            //Руководитель
            sheet.Cell("I23").SetValue(formen?.Surname.ToUpper() + " " + formen?.Name.ToUpper() + ", ");

            String result = "";
            if ((bool)formen?.BithDate.HasValue)
                result += formen?.BithDate.Value.ToShortDateString();

            if (formen?.Nationality != null)
                result += ", " + formen.Nationality.ToUpper();

            if (formen?.SerialAndNumber != null)
                result += ", " + formen.SerialAndNumber.ToUpper();

            if (formen?.Gender != null)
                result += ", " + formen?.Gender.ToUpper();

            sheet.Cell("B25").Value = result;        

            //добавление туристов
            row = 30;
            for (int i = 1; i < visitors.Count; i++)
            {
                sheet.Row(row).InsertRowsBelow(1);
                row++;
                sheet.Range("B" + row + ":E" + row).Merge();
                sheet.Range("F" + row + ":H" + row).Merge();
                sheet.Range("I" + row + ":L" + row).Merge();
                sheet.Range("M" + row + ":P" + row).Merge();

                sheet.Cell("B" + row.ToString()).Value = visitors[i].Surname.ToUpper() + " " + visitors[i].Name.ToUpper();
                sheet.Cell("F" + row.ToString()).Value = visitors[i].Nationality.ToUpper();
                sheet.Cell("I" + row.ToString()).Value = visitors[i].SerialAndNumber.ToUpper();
                sheet.Cell("M" + row.ToString()).Value = visitors[i].BithDate.Value.ToShortDateString();
                sheet.Cell("Q" + row.ToString()).Value = visitors[i].Gender.ToUpper();
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

            sheet.Cell("J8").SetValue(visitor?.Surname.ToUpper() + " " + visitor?.Name.ToUpper() + ", ");

            String result = "";
            if ((bool)visitor?.BithDate.HasValue)
                result += visitor?.BithDate.Value.ToShortDateString();

            if (visitor?.Nationality != null)
                result += ", " + visitor.Nationality.ToUpper();

            if (visitor?.SerialAndNumber != null)
                result += ", " + visitor.SerialAndNumber.ToUpper();

            if (visitor?.Gender != null)
                result += ", " + visitor?.Gender.ToUpper();

            sheet.Cell("B10").Value = result;

            if (group.DateArrival.HasValue)
            {
                sheet.Cell("F20").Value = group.DateArrival.Value.Day;
                sheet.Cell("H20").Value = group.DateArrival.Value.Month;
                sheet.Cell("J20").Value = group.DateArrival.Value.Year;
            }

            if (group.DateDeparture.HasValue)
            {
                sheet.Cell("L20").Value = group.DateDeparture.Value.Day;
                sheet.Cell("N20").Value = group.DateDeparture.Value.Month;
                sheet.Cell("P20").Value = group.DateDeparture.Value.Year;
            }

            sheet.Cell("B23").Value = group.PlaceOfRecidense;
            sheet.Cell("B26").Value = group.ProgramOfTravel;
            sheet.Cell("I29").Value = group.TimeOfWork;

            sheet.Cell("I30").Value = group.TranscriptUser;

            int row = 32;
            result = "";

            if (group.AddressUser != null)
            {
                result = group.AddressUser;
            }
            if (group.TelNumber != null)
            {
                result += ", " + group.TelNumber;
            }        
            if (group.SiteOfOperator != null)
            {
                result += ", " + group.SiteOfOperator;
            }
            if (!String.IsNullOrEmpty(result))
            {
                sheet.Cell("B" + row.ToString()).Value = result;
                row += 2;
            }

            if (group.Email != null)
            {
                sheet.Cell("B" + row.ToString()).Value = group.Email;
            }

            AddBarcodePic(group.Id);

            return book;
        }
    }
}
