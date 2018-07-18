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
        IService<VisitorDTO> ServiceVisitor;

        public UnitTestGroupService()
        {         
            CreateTestRepositories repoes = new CreateTestRepositories();
            var database = repoes.CreateIoWManager();

            Service = new GroupService(database);
            ServiceVisitor = new VisitorService(database);
        }


        [TestMethod]
        public async Task Create_Group()
        {
            GroupVisitorDTO group = new GroupVisitorDTO { Id = 6, PlaceOfRecidense = "testPlace", UserInSystem = "Admin" };
            List<VisitorDTO> visitors = new List<VisitorDTO> {
                new VisitorDTO{ Name="test1", Gender = "Мужчина", Nationality = "nat1", UserInSystem = "Admin"},
                new VisitorDTO{ Name="test2"},
                new VisitorDTO{ Name="test3"},
            };
            group.Visitors = visitors;
            var result = await Service.Create(group);
            var findGroup = Service.GetById(6);
            var visitor = findGroup.Visitors.ToList()[0];

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(Service.GetAll().Count() == 6);
            
            Assert.IsTrue(findGroup.PlaceOfRecidense == "testPlace");
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.DateInSystem.Value.Date == DateTime.Now.Date);     
            Assert.IsTrue(findGroup.UserOperatorProfileUNP == "UnpAdmin");
            Assert.IsTrue(findGroup.StatusName == "status1");
            Assert.IsTrue(findGroup.Visitors.Count() == 3);
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == 1).Count() == 3);
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusName == "status1").Count() == 3);

            Assert.IsTrue(visitor.Name == "test1");
            Assert.IsTrue(visitor.Gender == "Мужчина");
            Assert.IsTrue(visitor.Nationality == "nat1");
            Assert.IsTrue(visitor.DateInSystem.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(visitor.UserInSystem == "Admin");
            Assert.IsTrue(visitor.StatusOfOperation == 1);
        }

        [TestMethod]
        public async Task Delete_Group_Test()
        {
            // test delete item have status code 1 (new)
            var listOfVisitors1 = ServiceVisitor.GetAll();
            var result = await Service.Delete(2);
            var listOfVisitors2 = ServiceVisitor.GetAll();
            var list = Service.GetAll();
            var group = await Service.GetByIdAsync(2);
         
            Assert.IsTrue(listOfVisitors1.Count() == 7);
            Assert.IsTrue(listOfVisitors2.Count() == 5);
            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(result.Message == "Группа туристов удалена");
            Assert.IsTrue(list.Count() == 4);
            Assert.IsNull(group);

            // test delete item have status code 2, 3 (send to pogran)
            result = await Service.Delete(1);
            list = Service.GetAll();
            group = await Service.GetByIdAsync(1);
            var visitors = group.Visitors;

            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(group);
            Assert.IsTrue(group.StatusName == "status1");
            Assert.IsTrue(result.Message == "Группа туристов помечена к удалению");
            Assert.IsTrue(list.Count() == 4);

            Assert.IsTrue(visitors.Count() == 2);
            Assert.IsTrue(visitors.Where(v => v.StatusOfOperation == 3).Count() == 2);

            // test delete item have status code 2, 3 (send to pogran)
            result = await Service.Delete(3);
            list = Service.GetAll();
            group = await Service.GetByIdAsync(3);
            var allVisitors = ServiceVisitor.GetAll();

            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(group);
            Assert.IsTrue(group.StatusName == "status1");
            Assert.IsTrue(result.Message == "Группа туристов помечена к удалению");
            Assert.IsTrue(list.Count() == 4);

            Assert.IsTrue(allVisitors.Count() == 4);
            Assert.IsTrue(visitors.Where(v => v.StatusOfOperation == 3).Count() == 2);
        }

        [TestMethod]
        public async Task Update_Group_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 2,
                PlaceOfRecidense = "new Place",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO{Id = 1, Name="new name", Gender = "Мужчина", Nationality = "nat1", UserInSystem = "Admin", DateInSystem = new DateTime(2018, 07, 01)},
                    new VisitorDTO { Id = 2, Surname = "newSurname2", Name = "newName2" },
                    new VisitorDTO { Id = 3, Surname = "newSurname3", Name = "newName3" },
                },

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1",
                DateEdit = DateTime.Now
            };

            var result = await Service.Update(group);
            var findGroup = await Service.GetByIdAsync(2);
            var visitor = findGroup.Visitors.ToList()[0];
 
            Assert.IsTrue(findGroup.PlaceOfRecidense == "new Place");
            Assert.IsTrue(findGroup.UserUserName == "Admin");
            Assert.IsTrue(findGroup.UserOperatorProfileUNP == "UnpAdmin");
            Assert.IsTrue(findGroup.Visitors.Count() == 3);
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.DateInSystem == new DateTime(2018, 07, 01));
            Assert.IsTrue(findGroup.UserEdit == "Test1");
            Assert.IsTrue(findGroup.DateEdit.Value.Date == DateTime.Now.Date);

            Assert.IsTrue(visitor.Name == "new name");
            Assert.IsTrue(visitor.UserInSystem == "Admin");
            Assert.IsTrue(visitor.DateInSystem == new DateTime(2018, 07, 01));
            Assert.IsTrue(visitor.UserEdit == "Test1");
            Assert.IsTrue(visitor.DateEdit.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(visitor.Gender == "Мужчина");
            Assert.IsTrue(visitor.Nationality == "nat1");
        }

        [TestMethod]
        public void GetAll_Group_Test()
        {
            var groups = Service.GetAll();

            Assert.IsTrue(groups.Count() == 5);
            Assert.IsTrue(groups.Where(g => g.Id == 1).FirstOrDefault().UserOperatorProfileUNP == "UnpAdmin");
            Assert.IsTrue(groups.Where(g => g.Id == 1).FirstOrDefault().UserUserName == "Admin");
            Assert.IsTrue(groups.Where(g => g.Id == 1).FirstOrDefault().Visitors.Count() == 2);
            Assert.IsTrue(groups.Where(g => g.Id == 3).FirstOrDefault().Visitors.Count() == 3);
        }

        [TestMethod]
        public void GetId_Group_Test()
        {
            var group = Service.GetById(2);
            Assert.IsNotNull(group);
            Assert.IsTrue(group.PlaceOfRecidense == "place2");
            Assert.IsTrue(group.UserOperatorProfileUNP == "UnpAdmin");
            Assert.IsTrue(group.UserUserName == "Admin");
            Assert.IsTrue(group.Visitors.Count() == 2);
        }
    }
}
