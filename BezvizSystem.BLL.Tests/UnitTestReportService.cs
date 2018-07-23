using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Tests.TestServises;

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

            Assert.IsTrue(r.AllRegistrated == "9");
            Assert.IsTrue(r.AllArrived == "5");
            Assert.IsTrue(r.WaitArrived == "3");
            Assert.IsTrue(r.NotArriverd == "1");
            Assert.IsTrue(r.AllTourist == "2");
            Assert.IsTrue(r.AllGroup == "3");
            Assert.IsTrue(r.AllTouristInGroup == "7");
        }
    }
}
