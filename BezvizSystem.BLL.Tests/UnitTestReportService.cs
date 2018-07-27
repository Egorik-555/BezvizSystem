using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Tests.TestServises;
using System.Linq;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestReportService
    {
        IReport report;

        public UnitTestReportService()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            var database = repoes.CreateIoWManager();
            report = new ReportService(database);
        }

        [TestMethod]
        public void GetReportTest()
        {
            var r = report.GetReport(DateTime.Parse("01.06.2016"), DateTime.Parse("30.07.2018"), DateTime.Parse("21.07.2018"));
           
            Assert.IsTrue(r.AllRegistrated == 9);
            Assert.IsTrue(r.AllArrived == 5);
            Assert.IsTrue(r.WaitArrived == 3);
            Assert.IsTrue(r.NotArriverd == 1);
            Assert.IsTrue(r.AllTourist == 2);
            Assert.IsTrue(r.AllGroup == 3);
            Assert.IsTrue(r.AllTouristInGroup == 7);
          
        }

        [TestMethod]
        public void Get_By_Nat_And_Age_Test()
        {
            var r = report.GetReport(DateTime.Parse("01.06.2016"), DateTime.Parse("30.07.2018"), DateTime.Parse("21.07.2018"));
            var list = r.AllByNatAndAge;

            Assert.IsTrue(list.Count() == 3);
            Assert.IsTrue(list.Where(l => l.Natiolaty == "n1").FirstOrDefault().All == 2);
            Assert.IsTrue(list.Where(l => l.Natiolaty == "n1").FirstOrDefault().ManMore18 == 1);
            Assert.IsTrue(list.Where(l => l.Natiolaty == "n1").FirstOrDefault().WomanMore18 == 1);
            Assert.IsTrue(list.Where(l => l.Natiolaty == "n2").FirstOrDefault().All == 2);
        }

        [TestMethod]
        public void Get_By_Date_Count_Test()
        {
            var r = report.GetReport(DateTime.Parse("01.06.2018"), DateTime.Parse("30.07.2018"), DateTime.Parse("21.07.2018"));
            var list = r.AllByDateArrivalCount;

            Assert.IsTrue(list.Count() == 2);
            Assert.IsTrue(list.Where(l => l.DateArrival.Value == new DateTime(2018, 7, 1)).FirstOrDefault().Count == 4);
            Assert.IsTrue(list.Where(l => l.DateArrival.Value == new DateTime(2018, 7, 2)).FirstOrDefault().Count == 2);
        }

        [TestMethod]
        public void Get_By_CheckPoint_Count_Test()
        {
            var r = report.GetReport(DateTime.Parse("01.06.2018"), DateTime.Parse("30.07.2018"), DateTime.Parse("21.07.2018"));
            var list = r.AllByCheckPointCount;

            Assert.IsTrue(list.Count() == 2);
            Assert.IsTrue(list.Where(l => l.CheckPoint == "check1").FirstOrDefault().Count == 4);
            Assert.IsTrue(list.Where(l => l.CheckPoint == "check2").FirstOrDefault().Count == 2);
        }
    }
}
