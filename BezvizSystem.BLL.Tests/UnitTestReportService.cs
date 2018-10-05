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
           
            Assert.AreEqual(3, r.AllRegistrated);
            Assert.AreEqual(2, r.AllArrived);
            Assert.AreEqual(1, r.WaitArrived);
            Assert.AreEqual(0, r.NotArriverd);
            Assert.AreEqual(1, r.AllTourist);
            Assert.AreEqual(1, r.AllGroup);
            Assert.AreEqual(2, r.AllTouristInGroup);
          
        }

        [TestMethod]
        public void Get_By_Nat_And_Age_Test()
        {
            var r = report.GetReport(DateTime.Parse("01.06.2016"), DateTime.Parse("30.07.2018"), DateTime.Parse("21.07.2018"));
            var list = r.AllByNatAndAge;

            Assert.AreEqual(2, list.Count());
            Assert.AreEqual(1, list.SingleOrDefault(l => l.Natiolaty == "n1").All);
            Assert.AreEqual(1, list.SingleOrDefault(l => l.Natiolaty == "n1").ManMore18);
            Assert.AreEqual(0, list.SingleOrDefault(l => l.Natiolaty == "n1").WomanMore18);
            Assert.AreEqual(1, list.SingleOrDefault(l => l.Natiolaty == "n2").All);
        }

        [TestMethod]
        public void Get_By_Date_Count_Test()
        {
            var r = report.GetReport(DateTime.Parse("01.06.2018"), DateTime.Parse("30.07.2018"), DateTime.Parse("21.07.2018"));
            var list = r.StringDateByArrivalCount;

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void Get_By_CheckPoint_Count_Test()
        {
            var r = report.GetReport(DateTime.Parse("01.06.2018"), DateTime.Parse("30.07.2018"), DateTime.Parse("21.07.2018"));
            var list = r.StringCheckPointCount;

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void Get_By_Days_Count_Test()
        {
            var r = report.GetReport(DateTime.Parse("01.06.2018"), DateTime.Parse("30.07.2018"), DateTime.Parse("21.07.2018"));
            var list = r.StringDaysByCount;

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void Get_By_Operator_Count_Test()
        {
            var r = report.GetReport(DateTime.Parse("01.06.2018"), DateTime.Parse("30.07.2018"), DateTime.Parse("21.07.2018"));
            var list = r.AllByOperatorCount;

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(2, list.SingleOrDefault(l => l.Operator == "transcript User").Count);
            Assert.IsNull(list.SingleOrDefault(l => l.Operator == "operator2"));
        }
    }
}
