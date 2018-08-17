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

        private VisitorDTO visitor1 = new VisitorDTO
        {
            Id = 1,
            Surname = "surname new",      
        };

        VisitorDTO visitor2 = new VisitorDTO
        {
            Id = 2,
            Surname = "surname2",
            Name = "Name2",
            Gender = "Женщина",
            BithDate = new DateTime(2005, 1, 1),
            Nationality = "nat2",
            DateInSystem = new DateTime(2018, 07, 02),
            UserInSystem = "Test1",
            Arrived = true
        };

        VisitorDTO visitor3 = new VisitorDTO
        {
            Id = 3,
            Surname = "surname3",
            Name = "Name3",
            Gender = "Мужчина",
            BithDate = new DateTime(2000, 1, 1),
            Nationality = "nat2",
            DateInSystem = new DateTime(2017, 01, 01),
            UserInSystem = "Test2"
        };

        VisitorDTO visitor4 = new VisitorDTO
        {
            Id = 4,
            Surname = "surname3"
        };

        [TestMethod]
        public async Task Update_Group_Add_new_Visitor()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 2,
                PlaceOfRecidense = "new place",
                CheckPoint = "check1",
                Visitors = new List<VisitorDTO> { visitor2, visitor3, visitor4 }
            };

            var result = await Service.Update(group);
            var dispatches = database.XMLDispatchManager.GetAll();
            var dispatch2 = await database.XMLDispatchManager.GetByIdAsync(2);
            var dispatch4 = await database.XMLDispatchManager.GetByIdAsync(4);

            Assert.IsTrue(result.Succedeed);
            Assert.AreEqual(3, dispatches.Count());

            Assert.AreEqual(Status.Send, dispatch2.Status);
            Assert.AreEqual(Operation.Edit, dispatch2.Operation);
            Assert.AreEqual(Status.New, dispatch4.Status);
            Assert.AreEqual(Operation.Add, dispatch4.Operation);
        }

        [TestMethod]
        public async Task Update_Group_Delete_Visitors()
        {
            GroupVisitorDTO group1 = new GroupVisitorDTO
            {
                Id = 1,
                PlaceOfRecidense = "new place",
                CheckPoint = "check1",
                Visitors = new List<VisitorDTO>()
            };

            GroupVisitorDTO group2 = new GroupVisitorDTO
            {
                Id = 2,
                PlaceOfRecidense = "new place",
                CheckPoint = "check1",
                Visitors = new List<VisitorDTO> ()
            };

            var result1 = await Service.Update(group1);
            var result2 = await Service.Update(group2);
            var dispatches = database.XMLDispatchManager.GetAll();
            var dispatch1 = await database.XMLDispatchManager.GetByIdAsync(1);
            var dispatch2 = await database.XMLDispatchManager.GetByIdAsync(2);

            Assert.IsTrue(result1.Succedeed);
            Assert.IsTrue(result2.Succedeed);
            Assert.AreEqual(1, dispatches.Count());
       
            Assert.AreEqual(Status.Send, dispatch2.Status);
            Assert.AreEqual(Operation.Remove, dispatch2.Operation);
        }

        [TestMethod]
        public async Task Update_Group_Edit_new_Visitor()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 1,
                PlaceOfRecidense = "new place",
                CheckPoint = "check1",
                Visitors = new List<VisitorDTO> { visitor1 }
            };

            var result = await Service.Update(group);
            var dispatches = database.XMLDispatchManager.GetAll();
            var dispatch = await database.XMLDispatchManager.GetByIdAsync(1);

            Assert.IsTrue(result.Succedeed);
            Assert.AreEqual(2, dispatches.Count());

            Assert.AreEqual(Status.New, dispatch.Status);
            Assert.AreEqual(Operation.Add, dispatch.Operation);
        }

        [TestMethod]
        public async Task Update_Group_Delete_One_Add_One_Visitor()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 1,
                PlaceOfRecidense = "new place",
                CheckPoint = "check1",
                Visitors = new List<VisitorDTO> { visitor4 }
            };

            var result = await Service.Update(group);
            var dispatches = database.XMLDispatchManager.GetAll();
            var dispatch1 = await database.XMLDispatchManager.GetByIdAsync(1);
            var dispatch4 = await database.XMLDispatchManager.GetByIdAsync(4);

            Assert.IsTrue(result.Succedeed);
            Assert.AreEqual(2, dispatches.Count());

            Assert.IsNull(dispatch1);
            Assert.AreEqual(Status.New, dispatch4.Status);
            Assert.AreEqual(Operation.Add, dispatch4.Operation);
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
