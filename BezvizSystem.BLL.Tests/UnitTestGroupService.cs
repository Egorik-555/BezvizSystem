using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Tests.TestServises;
using BezvizSystem.DAL;
using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Identity;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestGroupService
    {
        IService<GroupVisitorDTO> Service;

        public UnitTestGroupService()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            var database = repoes.CreateIoWManager();

            Service = new GroupService(database, new XMLDispatcher(database));
        }


        [TestMethod]
        public async Task Create_Group()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 33,
                PlaceOfRecidense = "new place",
                DateInSystem = DateTime.Now,
                UserInSystem = "Admin"
            };

            List<VisitorDTO> visitors = new List<VisitorDTO> {
                new VisitorDTO{Id = 33, Name="test1", Gender = "Мужчина", Nationality = "nat1"},
                new VisitorDTO{Id = 44, Name="test2", Nationality = "nat2"},
                new VisitorDTO{Id = 55, Name="test3", Nationality = "nat3"},
            };

            group.Visitors = visitors;
            var result = await Service.Create(group);
            var findGroup = Service.GetById(33);
            var visitor = findGroup.Visitors.SingleOrDefault(v => v.Id == 33);

            Assert.IsTrue(result.Succedeed);
            Assert.AreEqual(3, Service.GetAll().Count());

            Assert.AreEqual("new place", findGroup.PlaceOfRecidense);
            Assert.AreEqual("Admin", findGroup.UserInSystem);
            Assert.AreEqual("AdminTran", findGroup.TranscriptUser);
            Assert.AreEqual(DateTime.Now.Date, findGroup.DateInSystem.Value.Date);
            Assert.AreEqual(3, findGroup.Visitors.Count());

            Assert.AreEqual("test1", visitor.Name);
            Assert.AreEqual("Мужчина", visitor.Gender);
            Assert.AreEqual("nat1", visitor.Nationality);
            Assert.AreEqual(DateTime.Now.Date, visitor.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", visitor.UserInSystem);
        }

        [TestMethod]
        public async Task Delete_Group_Test()
        {           
            var result = await Service.Delete(2);
            var list = Service.GetAll();
            var group = await Service.GetByIdAsync(2);

            Assert.IsTrue(result.Succedeed);
            Assert.AreEqual("Группа туристов удалена", result.Message);
            Assert.AreEqual(1, list.Count());
            Assert.IsNull(group);         
        }

        [TestMethod]
        public async Task Update_Group_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 1,
                PlaceOfRecidense = "new Place",
                CheckPoint = "check3",

                Visitors = new List<VisitorDTO> {
                    new VisitorDTO { Id = 333, Surname = "surname3", Nationality = "nat3"},
                    new VisitorDTO { Id = 444, Surname = "surname4", Nationality = "nat1"}},

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1",
                DateEdit = DateTime.Now
            };

            var result = await Service.Update(group);
            var findGroup = await Service.GetByIdAsync(1);
            var visitor = findGroup.Visitors.SingleOrDefault(v => v.Id == 333);

            Assert.AreEqual(2, findGroup.Visitors.Count());
            Assert.AreEqual("Admin", findGroup.UserInSystem);
            Assert.AreEqual(DateTime.Now.Date, findGroup.DateEdit.Value.Date);

            Assert.AreEqual("surname3", visitor.Surname);
            Assert.AreEqual("nat3", visitor.Nationality);
        }

        [TestMethod]
        public async Task Update_Group_Delete_All_Visitors_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 1,
                PlaceOfRecidense = "new Place",
                CheckPoint = "check3",

                Visitors = new List<VisitorDTO>(),

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1",
                DateEdit = DateTime.Now
            };

            var result = await Service.Update(group);
            var findGroup = await Service.GetByIdAsync(1);
            
            Assert.AreEqual(0, findGroup.Visitors.Count());
            Assert.AreEqual("Admin", findGroup.UserInSystem);
            Assert.AreEqual(DateTime.Now.Date, findGroup.DateEdit.Value.Date);
        }

        [TestMethod]
        public void GetAll_Group_Test()
        {
            var groups = Service.GetAll();
            var group1 = groups.SingleOrDefault(g => g.Id == 1);
            var group2 = groups.SingleOrDefault(g => g.Id == 2);

            Assert.AreEqual(2, groups.Count());
            Assert.AreEqual("place 1", group1.PlaceOfRecidense);
            Assert.AreEqual("place 2", group2.PlaceOfRecidense);
            Assert.AreEqual(1, group1.Visitors.Count());
            Assert.AreEqual(2, group2.Visitors.Count());
        }

        [TestMethod]
        public void GetId_Group_Test()
        {
            var group = Service.GetById(2);
            Assert.IsNotNull(group);
            Assert.AreEqual("place 2", group.PlaceOfRecidense);
            Assert.AreEqual(2, group.Visitors.Count());
        }

        [TestMethod]
        public async Task GetId_Async_Group_Test()
        {
            var group = await Service.GetByIdAsync(2);
            Assert.IsNotNull(group);
            Assert.AreEqual("place 2", group.PlaceOfRecidense);
            Assert.AreEqual(2, group.Visitors.Count());
        }
    }
}
