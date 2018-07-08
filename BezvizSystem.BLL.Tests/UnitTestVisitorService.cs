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
            VisitorDTO visitor = new VisitorDTO { Name = "newTest", Nationality = "nat1", UserInSystem = "Test1" };

            var result = await Service.Create(visitor);
            var count = Service.GetAll().Count();

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(count == 5);
        }

        [TestMethod]
        public async Task Delete_Visitor()
        {
            var result = await Service.Delete(1);
            var visitor = await Service.GetByIdAsync(1);
            var count = Service.GetAll().Count();

            Assert.IsNull(visitor);
            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(count == 3);
        }

        [TestMethod]
        public async Task Update_Visitor()
        {
            VisitorDTO visitor = new VisitorDTO {Id = 2, Name = "new Name", UserInSystem = "user", UserEdit = "new user", DateEdit = DateTime.Now };

            var result = await Service.Update(visitor);
            var resVisitor = await Service.GetByIdAsync(2);
            var count = Service.GetAll().Count();

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(count == 4);
            Assert.IsTrue(resVisitor.Name == "new Name");
            Assert.IsTrue(resVisitor.UserInSystem == "user");
            Assert.IsTrue(resVisitor.UserEdit == "new user");
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
            Assert.IsTrue(visitor.Count() == 4);
        }
    }
}
