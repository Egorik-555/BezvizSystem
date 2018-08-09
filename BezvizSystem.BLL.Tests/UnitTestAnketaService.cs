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

            Assert.AreEqual(7, anketas.Count());
            Assert.IsTrue(group.DateArrival == new DateTime(2018, 6, 1));
            Assert.IsTrue(group.CheckPoint == "check1");
            Assert.IsTrue(group.Operator == "AdminTran");
            Assert.IsTrue(group.CountMembers == 2);
            Assert.IsTrue(group.Status == "Передано");

            Assert.IsTrue(group3.CountMembers == 3);
            Assert.IsTrue(group3.Status == "Сохранено");
        }

        [TestMethod]
        public void Get_ById_Anketa()
        {
            var anketa = _service.GetById(2);

            Assert.IsTrue(anketa.CountMembers == 2);
            Assert.IsTrue(anketa.CheckPoint == "check2");
            Assert.IsTrue(anketa.Status == "Сохранено");
        }

        [TestMethod]
        public async Task Get_ById_AnketaAsync()
        {
            var anketa = await _service.GetByIdAsync(2);

            Assert.IsTrue(anketa.CountMembers == 2);
            Assert.IsTrue(anketa.CheckPoint == "check2");
            Assert.IsTrue(anketa.Status == "Сохранено");
        }

        [TestMethod]
        public async Task Get_ForUser_AnketaAsync()
        {
            var anketa = await _service.GetForUserAsync("Test1");

            var anketa1 = await _service.GetByIdAsync(3);
            var anketa2 = await _service.GetByIdAsync(7);

            Assert.AreEqual(2, anketa.Count());
            Assert.AreEqual("Частично", anketa1.Arrived);
            Assert.AreEqual("X", anketa2.Arrived);

            Assert.AreEqual("Save", anketa1.Status);
            Assert.AreEqual("Save", anketa2.Status);
        }
    }
}
