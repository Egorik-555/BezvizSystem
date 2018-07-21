using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Tests.TestServises;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestAnketaService
    {
        IService<AnketaDTO> _service;

        public UnitTestAnketaService()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            var database = repoes.CreateIoWManager();
            _service = new AnketaService(database);        
        }

        [TestMethod]
        public void Get_All_Anketa()
        {
            var anketas = _service.GetAll();
            var group = anketas.Where(v => v.Id == 1).FirstOrDefault();
            var group3 = anketas.Where(v => v.Id == 3).FirstOrDefault();

            Assert.IsTrue(anketas.Count() == 5);
            Assert.IsTrue(group.DateArrival == new DateTime(2018, 6, 1));
            Assert.IsTrue(group.CheckPoint == "check1");
            Assert.IsTrue(group.Operator == "AdminTran");
            Assert.IsTrue(group.CountMembers == 2);
            Assert.IsTrue(group.Status == "status2");

            Assert.IsTrue(group3.CountMembers == 3);
            Assert.IsTrue(group3.Status == "status1");
        }

        [TestMethod]
        public void Get_ById_Anketa()
        {
            var anketa = _service.GetById(2);

            Assert.IsTrue(anketa.CountMembers == 2);
            Assert.IsTrue(anketa.CheckPoint == "check2");
            Assert.IsTrue(anketa.Status == "status1");
        }
    }
}
