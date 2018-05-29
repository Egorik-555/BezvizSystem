using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Utils
{
    class Excel
    {
        Workbook wb = new Workbook();
        Worksheet ws;

        public XmlPrint()
        {
            wb.ExcelWorkbook.ActiveSheet = 1;
            wb.ExcelWorkbook.DisplayInkNotes = false;
            wb.ExcelWorkbook.FirstVisibleSheet = 1;
            wb.ExcelWorkbook.ProtectStructure = false;

            ws = new Worksheet("Данные");
            wb.AddWorksheet(ws);
        }

        private void MakeHead<T>(T item)
        {
            Type t = typeof(T);
            int i = 0;

            foreach (var info in t.GetProperties())
            {
                DisplayAttribute dispAttr = (DisplayAttribute)Attribute.GetCustomAttribute(info, typeof(DisplayAttribute));
                UIHintAttribute uiAttr = (UIHintAttribute)Attribute.GetCustomAttribute(info, typeof(UIHintAttribute));

                if (uiAttr != null && uiAttr.UIHint == "HiddenInput") continue;

                if (dispAttr != null)
                    ws.AddCell(0, i, dispAttr.Name);
                else
                    ws.AddCell(0, i, info.Name);
                i++;
            }
        }

        public string PrintListInFile<T>(IEnumerable<T> list) where T : class
        {
            Type t = typeof(T);
            MakeHead<T>(list.FirstOrDefault());

            int r = 1;
            foreach (var item in list)
            {
                int c = 0;
                foreach (var info in t.GetProperties())
                {
                    UIHintAttribute uiAttr = (UIHintAttribute)Attribute.GetCustomAttribute(info, typeof(UIHintAttribute));
                    if (uiAttr != null && uiAttr.UIHint == "HiddenInput") continue;
                    ws.AddCell(r, c, info.GetValue(item) != null ? info.GetValue(item).ToString() : "");
                    c++;
                }
                r++;
            }
            return wb.ExportToXML();
        }
    }
}
