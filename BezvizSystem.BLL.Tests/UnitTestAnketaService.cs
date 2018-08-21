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
            var group1 = anketas.Where(v => v.Id == 1).FirstOrDefault();
            var group2 = anketas.Where(v => v.Id == 2).FirstOrDefault();

            Assert.AreEqual(2, anketas.Count());

            Assert.AreEqual(new DateTime(2018, 6, 1), group1.DateArrival);
            Assert.AreEqual("check1", group1.CheckPoint);
            Assert.AreEqual("transcript User", group1.Operator);
            Assert.AreEqual(1, group1.CountMembers);
            Assert.AreEqual("Сохранено", group1.Status );
            Assert.AreEqual(1, group1.Visitors.Count());

            Assert.AreEqual(2, group2.CountMembers);
            Assert.AreEqual("Сохранено", group2.Status);
            Assert.AreEqual(2, group2.Visitors.Count());
        }

        [TestMethod]
        public void Get_ById_Anketa()
        {
            var anketa = _service.GetById(2);

            Assert.AreEqual(2, anketa.CountMembers);
            Assert.AreEqual("check2", anketa.CheckPoint);
            Assert.AreEqual("Сохранено", anketa.Status);
        }

        [TestMethod]
        public async Task Get_ById_AnketaAsync()
        {
            var anketa = await _service.GetByIdAsync(2);

            Assert.AreEqual(2, anketa.CountMembers);
            Assert.AreEqual("check2", anketa.CheckPoint);
            Assert.AreEqual("Сохранено", anketa.Status);
        }

        [TestMethod]
        public async Task Get_ForUser_AnketaAsync()
        {
            var anketas1 = await _service.GetForUserAsync("Test1");
            var anketas2 = await _service.GetForUserAsync("Test");
            var anketa1 = await _service.GetByIdAsync(1);
            var anketa2 = await _service.GetByIdAsync(2);

            Assert.IsNotNull(anketas1);
            Assert.IsNotNull(anketas2);
            Assert.AreEqual(2, anketas1.Count());

            Assert.AreEqual("V", anketa1.Arrived);
            Assert.AreEqual(1, anketa1.Visitors.Count);
            Assert.AreEqual("Частично", anketa2.Arrived);
            Assert.AreEqual("Сохранено", anketa1.Status);
            Assert.AreEqual("Сохранено", anketa2.Status);
            Assert.AreEqual(2, anketa2.Visitors.Count);
        }
    }
}
