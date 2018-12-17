using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Utils;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IDocumentGenerator
    {       
        XLWorkbook GenerateDocumentVisitor(string template, GroupVisitorDTO group);
        XLWorkbook GenerateDocumentGroup(string template, GroupVisitorDTO group);      
    }
}
