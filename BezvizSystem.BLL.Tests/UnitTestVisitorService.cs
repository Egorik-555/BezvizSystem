using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Tests.TestServises;
using BezvizSystem.DAL;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Identity;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestVisitorService
    {
        IService<VisitorDTO> Service;

        public UnitTestVisitorService()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            Service = new VisitorService(repoes.CreateIoWManager());
        }

        [TestMethod]
        public async Task Create_Visitor()
        {
            VisitorDTO visitor1 = new VisitorDTO { Id = 111, Name = "newTest", Nationality = "nat1", Gender = "Мужчина", UserInSystem = "Test1", DateInSystem = DateTime.Now };
            VisitorDTO visitor2 = new VisitorDTO { Id = 222, Name = "newTest", Nationality = "nat1", Gender = "Error", UserInSystem = "Test1", DateInSystem = DateTime.Now };

            var result1 = await Service.Create(visitor1);
            var result2 = await Service.Create(visitor2);
            var count = Service.GetAll().Count();

            var addedVisitor1 = await Service.GetByIdAsync(111);
            var addedVisitor2 = await Service.GetByIdAsync(222);

            Assert.IsTrue(result1.Succedeed);
            Assert.IsTrue(result2.Succedeed);
            Assert.AreEqual(4, count);
            Assert.AreEqual("Мужчина", addedVisitor1.Gender);
            Assert.IsNull(addedVisitor2.Gender);
            Assert.IsFalse(addedVisitor1.Arrived);
            Assert.AreEqual(DateTime.Now.Date, addedVisitor1.DateInSystem.Value.Date);

        }

        [TestMethod]
        public async Task Delete_Visitor()
        {
            var result = await Service.Delete(1);
            var visitor = await Service.GetByIdAsync(1);
            var count = Service.GetAll().Count();

            Assert.IsNull(visitor);
            Assert.IsTrue(result.Succedeed);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task Update_Visitor()
        {
            VisitorDTO visitor = new VisitorDTO
            {
                Id = 2,
                Name = "new Name",
                Nationality = "nat1",
                Gender = "Мужчина",
                DateInSystem = new DateTime(2018, 07, 02),
                UserInSystem = "Test",
                DateEdit = DateTime.Now,
                UserEdit = "new user"
            };

            var result = await Service.Update(visitor);
            var resVisitor = await Service.GetByIdAsync(2);
            var count = Service.GetAll().Count();

            Assert.IsTrue(result.Succedeed);
            Assert.AreEqual(2, count);
            Assert.AreEqual("new Name", resVisitor.Name);
            Assert.AreEqual("Test", resVisitor.UserInSystem);
            Assert.AreEqual(new DateTime(2018, 07, 02).Date, resVisitor.DateInSystem.Value.Date);
            Assert.AreEqual("new user", resVisitor.UserEdit);
            Assert.AreEqual(DateTime.Now.Date, resVisitor.DateEdit.Value.Date);
            Assert.AreEqual("Мужчина", resVisitor.Gender);
            Assert.AreEqual("nat1", resVisitor.Nationality);
        }

        [TestMethod]
        public void Get_By_Id_Visitor()
        {
            var visitor = Service.GetById(1);

            Assert.IsNotNull(visitor);
            Assert.AreEqual("surname1", visitor.Surname);
            Assert.AreEqual(new DateTime(1987, 07, 01).Date, visitor.BithDate.Value.Date);
        }

        [TestMethod]
        public async Task Get_By_Id_Async_Visitor()
        {
            var visitor = await Service.GetByIdAsync(1);

            Assert.IsNotNull(visitor);
            Assert.AreEqual("surname1", visitor.Surname);
            Assert.AreEqual(new DateTime(1987, 07, 01).Date, visitor.BithDate.Value.Date);
        }

        [TestMethod]
        public void Get_All_Visitor()
        {
            var visitor = Service.GetAll();
            Assert.AreEqual(2, visitor.Count());         
        }
    }
}
