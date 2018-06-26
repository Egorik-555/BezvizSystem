using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.BLL.Services;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class ReportTest
    {
        [TestMethod]
        public void GetReportTest()
        {
            IUnitOfWork testBase = new TestUnitOfWork();
            IReport report = new ReportService(testBase);

            var r = report.GetReport(DateTime.Parse("25.06.2018"), DateTime.Parse("30.06.2018"));

            Assert.IsTrue(r.AllRegistrated == "4");
            Assert.IsTrue(r.AllArrived == "2");
            Assert.IsTrue(r.WaitArrived == "1");
            Assert.IsTrue(r.NotArriverd == "1");
            Assert.IsTrue(r.AllTourist == "2");
            Assert.IsTrue(r.AllGroup == "1 (2 чел.)");

        }
    }
}
