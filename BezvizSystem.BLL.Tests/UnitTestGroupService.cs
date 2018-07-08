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
            Service = new GroupService(repoes.CreateIoWManager());
        }


        [TestMethod]
        public async Task Create_Group()
        {
            GroupVisitorDTO group = new GroupVisitorDTO { Id = 6, PlaceOfRecidense = "testPlace", UserOperatorProfileUNP = "UNPTest", UserInSystem = "Admin" };
            List<VisitorDTO> visitors = new List<VisitorDTO> {
                new VisitorDTO{ Name="test1"},
                new VisitorDTO{ Name="test2"},
                new VisitorDTO{ Name="test3"},
            };
            group.Visitors = visitors;
            var result = await Service.Create(group);

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(Service.GetAll().Count() == 6);

            var findGroup = Service.GetById(6);

            Assert.IsTrue(findGroup.PlaceOfRecidense == "testPlace");
            Assert.IsTrue(findGroup.UserOperatorProfileUNP == "UnpAdmin");
            Assert.IsTrue(findGroup.Visitors.ToList()[0].Name == "test1");
            Assert.IsTrue(findGroup.Visitors.Count() == 3);
        }

        [TestMethod]
        public async Task Delete_Group_Test()
        {
            var result = await Service.Delete(2);
            var list = Service.GetAll();
            var group = await Service.GetByIdAsync(2);

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(list.Count() == 4);
            Assert.IsNull(group);
        }

        [TestMethod]
        public async Task Update_Group_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 2,
                PlaceOfRecidense = "new Place",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO { Id = 1, Surname = "newSurname1", Name = "newName1" },
                     new VisitorDTO { Id = 2, Surname = "newSurname2", Name = "newName2" },
                      new VisitorDTO { Id = 3, Surname = "newSurname3", Name = "newName3" },
                },

                UserInSystem = "Admin"
            };

            var oldGroup = await Service.GetByIdAsync(2);
            var result = await Service.Update(group);
            var findGroup = await Service.GetByIdAsync(2);

            Assert.IsTrue(oldGroup.UserOperatorProfileUNP == "UnpTest1");

            Assert.IsTrue(findGroup.PlaceOfRecidense == "new Place");
            Assert.IsTrue(findGroup.UserUserName == "Admin");
            Assert.IsTrue(findGroup.UserOperatorProfileUNP == "UnpAdmin");
            Assert.IsTrue(findGroup.Visitors.Count() == 3);
        }

        [TestMethod]
        public void GetAll_Group_Test()
        {
            var groups = Service.GetAll();

            Assert.IsTrue(groups.Count() == 5);
            Assert.IsTrue(groups.Where(g => g.Id == 1).FirstOrDefault().UserOperatorProfileUNP == "UnpAdmin");
            Assert.IsTrue(groups.Where(g => g.Id == 1).FirstOrDefault().UserUserName == "Admin");
            Assert.IsTrue(groups.Where(g => g.Id == 1).FirstOrDefault().Visitors.Count() == 2);
            Assert.IsTrue(groups.Where(g => g.Id == 3).FirstOrDefault().Visitors.Count() == 4);
        }

        [TestMethod]
        public void GetId_Group_Test()
        {
            var group = Service.GetById(2);
            Assert.IsNotNull(group);
            Assert.IsTrue(group.PlaceOfRecidense == "place2");
            Assert.IsTrue(group.UserOperatorProfileUNP == "UnpTest1");
            Assert.IsTrue(group.UserUserName == "Test1");
            Assert.IsTrue(group.Visitors.Count() == 2);
        }
    }
}
