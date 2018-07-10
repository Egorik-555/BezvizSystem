using System;
using System.Linq;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Tests.TestServises;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestLoggerTest
    {
        ILogger<UserActivityDTO> Service;

        public UnitTestLoggerTest()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            Service = new ActivityLoggerService(repoes.CreateIoWManager());
        }

        [TestMethod]
        public void Logger_Insert_Item()
        {
            UserActivityDTO activity1 = new UserActivityDTO { Id = 5, Login = "login1", Ip = "ip", Operation = "Enter" };
            UserActivityDTO activity2 = new UserActivityDTO { Id = 5, Login = "login5", Ip = "ip", Operation = "ErrorEnter" };

            var result1 = Service.Insert(activity1);
            var result2 = Service.Insert(activity2);

            Assert.IsTrue(result1.Succedeed);
            Assert.IsTrue(result2.Succedeed);

            var list = Service.GetByLogin("login5").FirstOrDefault();
            Assert.IsNull(list.Operation);
        }

        [TestMethod]
        public void Logger_Get_All()
        {
            var list = Service.GetAll();

            Assert.IsTrue(list.Count() == 4);
        }

        [TestMethod]
        public void Logger_Get_By_Login()
        {
            var list = Service.GetByLogin("login1");

            Assert.IsTrue(list.Count() == 2);
            Assert.IsNotNull(list.Where(t => t.Id == 1).FirstOrDefault());
        }
    }
}
