using BezvizSystem.BLL.Interfaces;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Utils
{
    public class CloseXmlExcel : IExcel<XLWorkbook>
    {
        private XLWorkbook book;
        private IXLWorksheet sheet;

        public CloseXmlExcel()
        {
            book = new XLWorkbook();
            sheet = book.Worksheets.Add("Данные");
        }

        private void MakeHead<T>()
        {
            Type t = typeof(T);
            int i = 1;

            foreach (var info in t.GetProperties())
            {
                DisplayAttribute dispAttr = (DisplayAttribute)Attribute.GetCustomAttribute(info, typeof(DisplayAttribute));
                UIHintAttribute uiAttr = (UIHintAttribute)Attribute.GetCustomAttribute(info, typeof(UIHintAttribute));

                if (uiAttr != null && uiAttr.UIHint == "HiddenInput") continue;

                if (dispAttr != null)
                    sheet.Cell(1, i).SetValue(dispAttr.Name);
                else
                    sheet.Cell(1, i).SetValue(info.Name);
                i++;
            }
        }


        public XLWorkbook InExcel<T>(IEnumerable<T> list)
        {
            if (list == null) return null;

            Type t = typeof(T);
            MakeHead<T>();

            if (list.Count() == 0) return book;

            int r = 2;
            foreach (var item in list)
            {
                int c = 1;
                foreach (var info in t.GetProperties())
                {
                    UIHintAttribute uiAttr = (UIHintAttribute)Attribute.GetCustomAttribute(info, typeof(UIHintAttribute));
                    if (uiAttr != null && uiAttr.UIHint == "HiddenInput") continue;

                    sheet.Cell(r, c).SetValue(info.GetValue(item) != null ? info.GetValue(item).ToString() : "");
                    c++;
                }
                r++;
            }

            return book;
        }

        public Task<XLWorkbook> InExcelAsync<T>(IEnumerable<T> list)
        {
            return Task.Run(() =>
            {
                return InExcel(list);
            });
        }
    }
}
