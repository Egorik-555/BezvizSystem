using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Tests.TestServises;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestGroupServiceWithXmlDispatcher
    {
        IService<GroupVisitorDTO> Service;
        IUnitOfWork database;

        public UnitTestGroupServiceWithXmlDispatcher()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            database = repoes.CreateIoWManager();
            Service = new GroupService(database, new XMLDispatcher(database));
        }

        [TestMethod]
        public async Task Create_Group()
        {
            VisitorDTO visitor1 = new VisitorDTO { Id = 111, Name = "visitor1", Nationality = "nat1", Gender = "Мужчина", UserInSystem = "Test1" };
            VisitorDTO visitor2 = new VisitorDTO { Id = 222, Name = "visitor2", Nationality = "nat2", Gender = "Мужчина", UserInSystem = "Test1" };
            VisitorDTO visitor3 = new VisitorDTO { Id = 333, Name = "visitor3", Nationality = "nat3", Gender = "Женщина", UserInSystem = "Test1" };

            GroupVisitorDTO group1 = new GroupVisitorDTO
            {
                Id = 111,
                CheckPoint = "check1",
                DateArrival = new DateTime(2018, 1, 1),
                Visitors = new List<VisitorDTO> { visitor1, visitor2},
                DateInSystem = DateTime.Now,
                UserInSystem = "Test1"
            };

            GroupVisitorDTO group2 = new GroupVisitorDTO
            {
                Id = 222,
                CheckPoint = "check2",
                DateArrival = new DateTime(2018, 1, 2),
                Visitors = new List<VisitorDTO> { visitor3 },
                DateInSystem = DateTime.Now,
                UserInSystem = "Test1"
            };


            var result1 = await Service.Create(group1);
            var result2 = await Service.Create(group2);
            var count = Service.GetAll().Count();

            var addedGroup1 = await Service.GetByIdAsync(111);
            var addedGroup2 = await Service.GetByIdAsync(222);

            var dispatch1 = await database.XMLDispatchManager.GetByIdAsync(111);
            var dispatch2 = await database.XMLDispatchManager.GetByIdAsync(333);

            Assert.IsTrue(result1.Succedeed);
            Assert.IsTrue(result2.Succedeed);
            Assert.AreEqual(4, count);

            Assert.AreEqual(Status.New, dispatch1.Status);
            Assert.AreEqual(Operation.Add, dispatch1.Operation);
            Assert.AreEqual(Status.New, dispatch2.Status);
            Assert.AreEqual(Operation.Add, dispatch2.Operation);
        }

        [TestMethod]
        public async Task Update_Group()
        {

            VisitorDTO visitor = new VisitorDTO
            {
                Id = 2,
                Name = "new Name",
                Nationality = "nat2"
            };

            //var result = await Service.Update(visitor);
            //var dispatch = await database.XMLDispatchManager.GetByIdAsync(2);

            //Assert.AreEqual(Status.Send, dispatch.Status);
            //Assert.AreEqual(Operation.Edit, dispatch.Operation);
        }

        [TestMethod]
        public async Task Delete_New_Visitors()
        {
            var result = await Service.Delete(1);

            var group = await Service.GetByIdAsync(1);
            var dispatch = database.XMLDispatchManager.GetById(1);

            Assert.IsNull(group);
            Assert.IsTrue(result.Succedeed);

            Assert.IsNull(dispatch);
        }

        [TestMethod]
        public async Task Delete_Send_Visitors()
        {
            var result = await Service.Delete(2);

            var group = await Service.GetByIdAsync(2);
            var dispatch = database.XMLDispatchManager.GetById(2);

            Assert.IsNull(group);
            Assert.IsTrue(result.Succedeed);

            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Remove, dispatch.Operation);
        }

    }
}
