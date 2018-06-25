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

            report.GetReport(DateTime.Now, DateTime.Now);

        }
    }
}
