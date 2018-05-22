using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
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
        IService<GroupVisitorDTO> groupService;
        Mock<IUnitOfWork> databaseMock = new Mock<IUnitOfWork>();
        Mock<IRepository<GroupVisitor, int>> groupMangerMock = new Mock<IRepository<GroupVisitor, int>>();
        Mock<IRepository<Visitor, int>> mockVisitors = new Mock<IRepository<Visitor, int>>();
        List<GroupVisitor> listGroups;
        List<Visitor> listVisitors;

        public UnitTestGroupService()
        {
            BezvizUser user1 = new BezvizUser { Id = "aaa", UserName = "Admin", OperatorProfile = new OperatorProfile { UNP = "UNPAdmin", Transcript = "Tr1" } };
            BezvizUser user2 = new BezvizUser { Id = "bbb", UserName = "Test", OperatorProfile = new OperatorProfile { UNP = "UNPTest", Transcript = "Tr2" } };
            List<BezvizUser> userList = new List<BezvizUser> { user1, user2 };
            Mock<IUserStore<BezvizUser>> userStore = new Mock<IUserStore<BezvizUser>>();
            userStore.Setup(m => m.FindByNameAsync(It.IsAny<string>())).Returns<string>(name =>
                                                                            Task<BezvizUser>.FromResult<BezvizUser>(
                                                                                userList.Where(u => u.UserName == name).FirstOrDefault()));

            var UserManager = new BezvizUserManager(userStore.Object);



            Status status1 = new Status { Code = 1, Name = "status1" };
            Status status2 = new Status { Code = 2, Name = "status2" };
            List<Status> listStatuses = new List<Status> { status1, status2 };
            Mock<IRepository<Status, int>> mockStatuses = new Mock<IRepository<Status, int>>();
            mockStatuses.Setup(m => m.GetAll()).Returns(listStatuses);
            

            listVisitors = new List<Visitor>
            {
                new Visitor {Id = 1,  Name = "testVisitor1"},
                new Visitor {Id = 2, Name = "testVisitor21"},
                new Visitor {Id = 3, Name = "testVisitor22"}
            };

            listGroups = new List<GroupVisitor>()
            {
                new GroupVisitor { Id = 1, PlaceOfRecidense = "test1",
                                    Visitors = new List<Visitor>{ listVisitors[0] }, UserInSystem = "Test", User = user1},
                new GroupVisitor { Id = 2, PlaceOfRecidense = "test2",
                                    Visitors = new List<Visitor>{ listVisitors[1], listVisitors[2] }, UserInSystem = "Test", User = user2 }
            };

            mockVisitors.Setup(m => m.Create(It.IsAny<Visitor>())).Returns<Visitor>(v => { listVisitors.Add(v); return v; });
            mockVisitors.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id =>
            {
                for (int i = listVisitors.Count - 1; i >= 0; i--)
                {
                    if (listVisitors[i].Id == id)
                        listVisitors.RemoveAt(i);
                }

                return null;
            });
            mockVisitors.Setup(m => m.Update(It.IsAny<Visitor>())).Returns<Visitor>(v =>
            {
                for (int i = 0; i < listVisitors.Count; i++)
                {
                    if (listVisitors[i].Id == v.Id)
                    {
                        listVisitors.Remove(listVisitors[i]);
                        listVisitors.Insert(i, v);
                    }
                }
                return v;
            });

            groupMangerMock.Setup(m => m.Create(It.IsAny<GroupVisitor>())).Returns<GroupVisitor>(g => { listGroups.Add(g); return g; });
            groupMangerMock.Setup(m => m.GetAll()).Returns(listGroups);
            groupMangerMock.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => listGroups.Where(g => g.Id == id).FirstOrDefault());

            groupMangerMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>

                Task<GroupVisitor>.FromResult<GroupVisitor>(listGroups.Where(g => g.Id == id).FirstOrDefault())
            );
            groupMangerMock.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id =>
            {
                var group = listGroups.Where(g => g.Id == id).FirstOrDefault();
                if (group != null) listGroups.Remove(group);
                return group;
            });

            groupMangerMock.Setup(m => m.Update(It.IsAny<GroupVisitor>())).Returns<GroupVisitor>(g =>
            {
                foreach (var item in g.Visitors)
                    listVisitors.Add(item);

                var group = listGroups.Where(gr => gr.Id == g.Id).FirstOrDefault();
                int ind = listGroups.FindIndex(gr => gr.Id == g.Id);
                listGroups.Remove(group);
                listGroups.Insert(ind, g);
                return g;
            });

            databaseMock.Setup(m => m.StatusManager).Returns(mockStatuses.Object);
            databaseMock.Setup(m => m.VisitorManager).Returns(mockVisitors.Object);
            databaseMock.Setup(m => m.GroupManager).Returns(groupMangerMock.Object);
            databaseMock.Setup(m => m.UserManager).Returns(UserManager);

            groupService = new GroupService(databaseMock.Object);
        }


        [TestMethod]
        public async Task Create_Group_Test()
        {
            GroupVisitorDTO group = new GroupVisitorDTO { Id = 3, PlaceOfRecidense = "testPlace", UserInSystem = "Test" };
            List<VisitorDTO> visitors = new List<VisitorDTO> {
                new VisitorDTO{ Name="test1"},
                new VisitorDTO{ Name="test2"},
                new VisitorDTO{ Name="test3"},
            };
            group.Visitors = visitors;
            var result = await groupService.Create(group);

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(listGroups.Count == 3);

            var findGroup = groupService.GetById(3);

            Assert.IsTrue(findGroup.PlaceOfRecidense == "testPlace");
            Assert.IsTrue(findGroup.UserOperatorProfileUNP == "UNPTest");
            Assert.IsTrue(findGroup.Visitors.ToList()[0].Name == "test1");
        }

        [TestMethod]
        public async Task Delete_Group_Test()
        {
            var result = await groupService.Delete(2);
            var list = groupService.GetAll();
            var group = await groupService.GetByIdAsync(2);

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(list.Count() == 1);
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

            var result = await groupService.Update(group);

            var findGroup = await groupService.GetByIdAsync(2);


            Assert.IsTrue(findGroup.PlaceOfRecidense == "new Place");
            Assert.IsTrue(findGroup.UserUserName == "Admin");
            Assert.IsTrue(findGroup.UserOperatorProfileUNP == "UNPAdmin");
            Assert.IsTrue(findGroup.Visitors.Count() == 3);
            Assert.IsTrue(listVisitors.Count() == 4);
        }

        [TestMethod]
        public void GetAll_Group_Test()
        {
            List<GroupVisitorDTO> groups = groupService.GetAll().ToList();

            Assert.IsTrue(groups.Count() == 2);
            Assert.IsTrue(groups[0].UserOperatorProfileUNP == "UNPAdmin");
            Assert.IsTrue(groups[0].UserUserName == "Admin");
            Assert.IsTrue(groups[0].Visitors.Count() == 1);
            Assert.IsTrue(groups[1].Visitors.Count() == 2);
        }

        [TestMethod]
        public void GetId_Group_Test()
        {
            var group = groupService.GetById(2);
            Assert.IsNotNull(group);
            Assert.IsTrue(group.PlaceOfRecidense == "test2");
            Assert.IsTrue(group.UserOperatorProfileUNP == "UNPTest");
            Assert.IsTrue(group.UserUserName == "Test");
            Assert.IsTrue(group.Visitors.Count() == 2);
        }
    }
}
