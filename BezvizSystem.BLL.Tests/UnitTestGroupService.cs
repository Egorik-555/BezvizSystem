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
                new VisitorDTO{Id = 33, Name="test1", Gender = "Мужчина", Nationality = "nat1"},
                new VisitorDTO{Id = 44, Name="test2"},
                new VisitorDTO{Id = 55, Name="test3"},
            };
            group.Visitors = visitors;
            var result = await Service.Create(group);
            var findGroup = Service.GetById(6);
            var visitor = await ServiceVisitor.GetByIdAsync(33);

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(Service.GetAll().Count() == 6);

            Assert.IsTrue(findGroup.PlaceOfRecidense == "testPlace");
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.DateInSystem.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(findGroup.UserOperatorProfileUNP == "UnpAdmin");
            Assert.IsTrue(findGroup.Visitors.Count() == 3);
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Add).Count() == 3);
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfRecord == StatusOfRecord.Save).Count() == 3);

            Assert.IsTrue(visitor.Name == "test1");
            Assert.IsTrue(visitor.Gender == "Мужчина");
            Assert.IsTrue(visitor.Nationality == "nat1");
            Assert.IsTrue(visitor.DateInSystem.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(visitor.UserInSystem == "Admin");
            Assert.IsTrue(visitor.StatusOfOperation == StatusOfOperation.Add);
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

            Assert.IsTrue(listOfVisitors1.Count() == 8);
            Assert.IsTrue(listOfVisitors2.Count() == 6);
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
            Assert.IsTrue(result.Message == "Группа туристов удалена");
            Assert.IsTrue(list.Count() == 4);

            Assert.IsTrue(visitors.Count() == 2);
            Assert.IsTrue(visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Remove).Count() == 2);

            // test delete item have status code 2, 3 (send to pogran)
            result = await Service.Delete(3);
            list = Service.GetAll();
            group = await Service.GetByIdAsync(3);
            var allVisitors = ServiceVisitor.GetAll();

            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(group);
            Assert.IsTrue(result.Message == "Группа туристов удалена");
            Assert.IsTrue(list.Count() == 4);

            Assert.IsTrue(allVisitors.Count() == 5);
            Assert.IsTrue(visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Remove).Count() == 2);
        }

        [TestMethod]
        public async Task Update_Group_OnlyUpdateVisitors_ForStatusOne_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 2,
                PlaceOfRecidense = "new Place",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO{Id = 3, Name="new name", Gender = "Мужчина", Nationality = "nat1", UserInSystem = "Admin", DateInSystem = new DateTime(2018, 07, 01)},
                    new VisitorDTO { Id = 4, Surname = "newSurname2", Name = "newName2" },
                },

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1",
                DateEdit = DateTime.Now
            };

            var result = await Service.Update(group);
            var findGroup = await Service.GetByIdAsync(2);
            var visitor = await ServiceVisitor.GetByIdAsync(3);

            Assert.IsTrue(findGroup.PlaceOfRecidense == "new Place");
            Assert.IsTrue(findGroup.UserUserName == "Admin");
            Assert.IsTrue(findGroup.UserOperatorProfileUNP == "UnpAdmin");
            Assert.IsTrue(findGroup.Visitors.Count() == 2);
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.DateInSystem == new DateTime(2018, 07, 01));
            Assert.IsTrue(findGroup.UserEdit == "Test1");
            Assert.IsTrue(findGroup.DateEdit.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Add).Count() == 2);

            Assert.IsTrue(visitor.Name == "new name");
            Assert.IsTrue(visitor.UserInSystem == "Admin");
            Assert.IsTrue(visitor.DateInSystem == new DateTime(2018, 07, 01));
            Assert.IsTrue(visitor.UserEdit == "Test1");
            Assert.IsTrue(visitor.DateEdit.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(visitor.Gender == "Мужчина");
            Assert.IsTrue(visitor.Nationality == "nat1");
            Assert.AreEqual(StatusOfRecord.Save, visitor.StatusOfRecord);
        }

        [TestMethod]
        public async Task Update_Group_AddOneVisitor_For_Status_One_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 2,
                PlaceOfRecidense = "new Place", CheckPoint = "check3",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO{Id = 3, Name="new name", Gender = "Мужчина", Nationality = "nat1", UserInSystem = "Admin", DateInSystem = new DateTime(2018, 07, 01)},
                    new VisitorDTO { Id = 4, Surname = "newSurname2", Name = "newName2" },
                    new VisitorDTO{Id = 0, Name="name added", Gender = "Женщина", Nationality = "nat1"},
                },

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1",
                DateEdit = DateTime.Now
            };

            var result = await Service.Update(group);
            var findGroup = await Service.GetByIdAsync(2);
            var visitor = findGroup.Visitors.ToList()[2];

            Assert.AreEqual("check3", findGroup.CheckPoint);
            Assert.IsTrue(findGroup.Visitors.Count() == 3);
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Add).Count() == 3);

            Assert.IsTrue(visitor.Name == "name added");
            Assert.IsTrue(visitor.UserInSystem == "Admin");
            Assert.IsTrue(visitor.DateInSystem.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(visitor.Gender == "Женщина");
            Assert.IsTrue(visitor.Nationality == "nat1");
        }

        [TestMethod]
        public async Task Update_Group_DeleteOneVisitor_ForStatusOne_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 2,
                PlaceOfRecidense = "new Place",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO{Id = 3, Name="new name", Gender = "Мужчина", Nationality = "nat1", UserInSystem = "Admin", DateInSystem = new DateTime(2018, 07, 01)},
                },

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1",
                DateEdit = DateTime.Now
            };

            var result = await Service.Update(group);
            var findGroup = await Service.GetByIdAsync(2);
            var visitor = findGroup.Visitors.ToList()[0];

            Assert.IsTrue(findGroup.Visitors.Count() == 1);
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Add).Count() == 1);

            Assert.IsTrue(visitor.Name == "new name");
            Assert.IsTrue(visitor.UserInSystem == "Admin");
            Assert.IsTrue(visitor.DateInSystem == new DateTime(2018, 07, 01));
            Assert.IsTrue(visitor.UserEdit == "Test1");
            Assert.IsTrue(visitor.DateEdit.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(visitor.Gender == "Мужчина");
            Assert.IsTrue(visitor.Nationality == "nat1");
            Assert.IsTrue(visitor.StatusOfRecord == StatusOfRecord.Save);
            Assert.IsTrue(visitor.StatusOfOperation == StatusOfOperation.Add);
        }

        [TestMethod]
        public async Task Update_OnlyGroup_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 2,
                PlaceOfRecidense = "new Place",
                CheckPoint = "check3",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO { Id = 3, Surname = "surname3", Nationality = "nat3"},
                    new VisitorDTO { Id = 4, Surname = "surname4", Nationality = "nat1"}},

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1"
            };

            var result = await Service.Update(group);
            var findGroup = await Service.GetByIdAsync(2);
            var visitor = findGroup.Visitors.ToList()[0];

            Assert.IsTrue(findGroup.Visitors.Count() == 2);
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Add).Count() == 2);

            Assert.IsTrue(visitor.Surname == "surname3");
            Assert.IsTrue(visitor.Nationality == "nat3");
            Assert.IsTrue(visitor.StatusOfRecord == StatusOfRecord.Save);
            Assert.IsTrue(visitor.StatusOfOperation == StatusOfOperation.Add);
        }

        [TestMethod]
        public async Task Update_Group_EditVisitor_With_Status_Two_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 5,
                PlaceOfRecidense = "new Place",
                CheckPoint = "check3",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO { Id = 7, Surname = "new surname", Nationality = "nat1"}},

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1"
            };

            var result = await Service.Update(group);

            var findGroup = await Service.GetByIdAsync(5);
            var visitor = await ServiceVisitor.GetByIdAsync(7);

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(findGroup.Visitors.Count() == 1);
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Edit).Count() == 1);

            Assert.IsTrue(visitor.Surname == "new surname");
            Assert.IsTrue(visitor.Nationality == "nat1");
            Assert.IsTrue(visitor.StatusOfRecord == StatusOfRecord.Save);
            Assert.IsTrue(visitor.StatusOfOperation == StatusOfOperation.Edit);
        }

        [TestMethod]
        public async Task Update_Group_Edit_Visitor_With_Status_Two_And_Add_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 5,
                PlaceOfRecidense = "new Place",
                CheckPoint = "check3",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO { Id = 7, Surname = "new surname", Nationality = "nat1"},
                    new VisitorDTO { Id = 0, Surname = "add new Visitor", Nationality = "nat3", Gender = "Мужчина"},
                },

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1",
                DateEdit = DateTime.Now
            };

            var result = await Service.Update(group);

            var findGroup = await Service.GetByIdAsync(5);
            var visitor = findGroup.Visitors.ToList()[1];

            Assert.IsTrue(result.Succedeed);
            Assert.AreEqual(DateTime.Now.Date, findGroup.DateEdit.Value.Date);
            Assert.IsTrue(findGroup.Visitors.Count() == 2);
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Edit).Count() == 1);
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Add).Count() == 1);

            Assert.IsTrue(visitor.Surname == "add new Visitor");
            Assert.IsTrue(visitor.Nationality == "nat3");
            Assert.IsTrue(visitor.Gender == "Мужчина");
            Assert.IsTrue(visitor.StatusOfRecord == StatusOfRecord.Save);
            Assert.IsTrue(visitor.StatusOfOperation == StatusOfOperation.Add);
        }

        [TestMethod]
        public async Task Update_Group_No_Edit_Visitor_With_Status_Two_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 5,
                PlaceOfRecidense = "new Place",
                CheckPoint = "check3",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO { Id = 7, Surname = "surname7", StatusOfRecord = StatusOfRecord.Send, Nationality = "nat3", Gender = "Мужчина"  }
                    },

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1",
                DateEdit = DateTime.Now
            };

            var result = await Service.Update(group);

            var findGroup = await Service.GetByIdAsync(5);
            var visitor = findGroup.Visitors.ToList()[0];

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(findGroup.Visitors.Count() == 1);
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
        
            Assert.IsTrue(visitor.Surname == "surname7");
            Assert.IsTrue(visitor.StatusOfRecord == StatusOfRecord.Send);
            Assert.IsTrue(visitor.StatusOfOperation == 0);
        }

        [TestMethod]
        public async Task Update_Group_Remove_Visitor_With_Status_Two_And_Add_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Id = 5,
                PlaceOfRecidense = "new Place",
                CheckPoint = "check3",
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO { Id = 0, Surname = "add new Visitor", Nationality = "nat3", Gender = "Мужчина"},
                },

                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                UserEdit = "Test1",
                DateEdit = DateTime.Now
            };

            var result = await Service.Update(group);

            var findGroup = await Service.GetByIdAsync(5);
            var removeVisitor = findGroup.Visitors.ToList()[0];
            var visitor = findGroup.Visitors.ToList()[1];

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(findGroup.Visitors.Count() == 2);
            Assert.IsTrue(findGroup.UserInSystem == "Admin");
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Remove).Count() == 1);
            Assert.IsTrue(findGroup.Visitors.Where(v => v.StatusOfOperation == StatusOfOperation.Add).Count() == 1);

            Assert.IsTrue(visitor.Surname == "add new Visitor");
            Assert.IsTrue(visitor.StatusOfRecord == StatusOfRecord.Save);
            Assert.IsTrue(visitor.StatusOfOperation == StatusOfOperation.Add);

            Assert.AreEqual(7, removeVisitor.Id);
            Assert.AreEqual(StatusOfRecord.Save, removeVisitor.StatusOfRecord);
            Assert.AreEqual(StatusOfOperation.Remove, removeVisitor.StatusOfOperation);
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
