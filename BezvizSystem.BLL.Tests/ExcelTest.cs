using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Utils;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class ExcelTest
    {
        [TestMethod]
        public void List_To_Excel()
        {
            IEnumerable<AnketaDTO> list = new List<AnketaDTO>
            {
                new AnketaDTO{ Id = 1, CheckPoint = "Check1", Operator = "Operator1" },
                new AnketaDTO{ Id = 2, CheckPoint = "Check2", Operator = "Operator2" },
                new AnketaDTO{ Id = 3, CheckPoint = "Check3", Operator = "Operator3" },
                new AnketaDTO{ Id = 4, CheckPoint = "Check4", Operator = "Operator4" },
                new AnketaDTO{ Id = 5, CheckPoint = "Check5", Operator = "Operator5" },
                new AnketaDTO{ Id = 6, CheckPoint = "Check6", Operator = "Operator6" },
            };

            //IExcel excel = new Excel();
            //string r = excel.InExcel(list);

            //Assert.IsNotNull(r);
        }
    }
}
