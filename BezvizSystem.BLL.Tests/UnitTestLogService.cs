using System;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Services.Log;
using System.Linq;
using BezvizSystem.DAL.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestLogService
    {
        [TestMethod]
        public void Test_GetById()
        {
            var service = new Logger(new UnitTestOfWork());

            var log = service.GetById(1);

            Assert.IsNotNull(log);
            Assert.AreEqual(LogType.Enter, log.Type);
            Assert.AreEqual("test", log.Ip);
        }

        [TestMethod]
        public void Test_Create()
        {
            var service = new Logger(new UnitTestOfWork());

            var log = new LogDTO
            {
                Id = 3,
                Type = LogType.Exit,
                UserName = "newAdded"
            };

            var result = service.WriteLog(log);

            Assert.IsTrue(result.Succedeed);        
        }

        [TestMethod]
        public void Test_GetAll()
        {
            var service = new Logger(new UnitTestOfWork());
          
            var result = service.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Test_GetByName()
        {
            var service = new Logger(new UnitTestOfWork());

            var result = service.GetByUserName("test");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            result = service.GetByUserName("test1");

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
