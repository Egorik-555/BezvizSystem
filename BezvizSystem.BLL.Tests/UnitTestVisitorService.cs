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
            VisitorDTO visitor1 = new VisitorDTO {Id = 10, Name = "newTest", Nationality = "nat1", Gender = "Мужчина", UserInSystem = "Test1" };
            VisitorDTO visitor2 = new VisitorDTO { Id = 11, Name = "newTest", Nationality = "nat1", Gender = "Error", UserInSystem = "Test1" };

            var result1 = await Service.Create(visitor1);
            var result2 = await Service.Create(visitor2);
            var count = Service.GetAll().Count();

            var addedVisitor1 = await Service.GetByIdAsync(10);
            var addedVisitor2 = await Service.GetByIdAsync(11);

            Assert.IsTrue(result1.Succedeed);
            Assert.IsTrue(result2.Succedeed);
            Assert.IsTrue(count == 10);
            Assert.IsTrue(addedVisitor1.Gender == "Мужчина");
            Assert.IsNull(addedVisitor2.Gender);
            Assert.IsFalse(addedVisitor1.Arrived);
            Assert.IsTrue(addedVisitor1.DateInSystem.Value.Date == DateTime.Now.Date);

        }

        [TestMethod]
        public async Task Delete_Visitor()
        {
            var result = await Service.Delete(1);
            var visitor = await Service.GetByIdAsync(1);
            var count = Service.GetAll().Count();

            Assert.IsNull(visitor);
            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(count == 7);
        }

        [TestMethod]
        public async Task Update_Visitor()
        {
            VisitorDTO visitor = new VisitorDTO {Id = 2, Name = "new Name", Nationality = "nat2", Gender = "Мужчина",
                                                 DateInSystem = new DateTime(2018, 07, 01), UserInSystem = "user",  UserEdit = "new user"};

            var result = await Service.Update(visitor);
            var resVisitor = await Service.GetByIdAsync(2);
            var count = Service.GetAll().Count();

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(count == 8);
            Assert.IsTrue(resVisitor.Name == "new Name");
            Assert.IsTrue(resVisitor.UserInSystem == "user");
            Assert.IsTrue(resVisitor.DateInSystem == new DateTime(2018, 07, 01));
            Assert.IsTrue(resVisitor.UserEdit == "new user");
            Assert.IsTrue(resVisitor.DateEdit.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(resVisitor.Gender == "Мужчина");
            Assert.IsTrue(resVisitor.Nationality == "nat2");
        }

        [TestMethod]
        public void Get_By_Id_Visitor()
        {
            var visitor = Service.GetById(1);

            Assert.IsNotNull(visitor);
            Assert.AreEqual(visitor.Surname, "surname1");
            Assert.AreEqual(visitor.BithDate.Value.Date, DateTime.Now.Date);
        }

        [TestMethod]
        public void Get_All_Visitor()
        {
            var visitor = Service.GetAll();
            Assert.IsTrue(visitor.Count() == 8);
        }
    }
}
