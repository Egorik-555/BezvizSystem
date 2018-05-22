using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
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
        Mock<IUnitOfWork> mockDB = new Mock<IUnitOfWork>();
        BezvizUserManager UserManager;

        List<Visitor> list;

        public UnitTestVisitorService()
        {
            BezvizUser user1 = new BezvizUser { Id = "aaa", UserName = "Admin", OperatorProfile = new OperatorProfile { UNP = "UNPAdmin" } };
            BezvizUser user2 = new BezvizUser { Id = "bbb", UserName = "Test", OperatorProfile = new OperatorProfile { UNP = "UNPTest" } };
            List<BezvizUser> userList = new List<BezvizUser> { user1, user2 };

            Mock<IUserStore<BezvizUser>> userStore = new Mock<IUserStore<BezvizUser>>();
            userStore.Setup(m => m.FindByNameAsync(It.IsAny<string>())).Returns<string>(id =>
                                                                            Task<BezvizUser>.FromResult<BezvizUser>(
                                                                                userList.Where(u => u.UserName == id).FirstOrDefault()));

            UserManager = new BezvizUserManager(userStore.Object);

            list = new List<Visitor> {new Visitor {Id = 1, Surname = "surname1", BithDate = DateTime.Now },
                                      new Visitor { Id = 2, Surname = "surname2", Name = "Name2" },
                                      new Visitor { Id = 3, Surname = "surname3" },
                                      new Visitor { Id = 4, Surname = "surname4"}};


            Mock<IRepository<Visitor, int>> mockVisitors = new Mock<IRepository<Visitor, int>>();
            mockVisitors.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => list.Where(v => v.Id == id).FirstOrDefault());
            mockVisitors.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                                        Task<Visitor>.FromResult<Visitor>(
                                                                                        list.Where(v => v.Id == id).FirstOrDefault()));

            mockVisitors.Setup(m => m.GetAll()).Returns(list);
            mockVisitors.Setup(m => m.Create(It.IsAny<Visitor>())).Returns<Visitor>(v => { list.Add(v); return v; });
            mockVisitors.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id =>
            {
                list.RemoveAt(id - 1);
                return null;
            });
            mockVisitors.Setup(m => m.Update(It.IsAny<Visitor>())).Returns<Visitor>(v =>
            {
                list.RemoveAt(v.Id - 1);
                list.Insert(v.Id-1, v);
                return v;
            });

            mockDB.Setup(m => m.VisitorManager).Returns(mockVisitors.Object);
            mockDB.Setup(m => m.UserManager).Returns(UserManager);

            Service = new VisitorService(mockDB.Object);
        }

        [TestMethod]
        public async Task Create_Visitor()
        {
            VisitorDTO visitor = new VisitorDTO { Name = "test", UserInSystem = "Admin" };

            var result = await Service.Create(visitor);

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(list.Count == 5);
        }

        [TestMethod]
        public async Task Delete_Visitor()
        {
            var result = await Service.Delete(1);
            var visitor = await Service.GetByIdAsync(1);

            Assert.IsNull(visitor);
            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(list.Count == 3);
        }

        [TestMethod]
        public async Task Update_Visitor()
        {
            VisitorDTO visitor = new VisitorDTO {Id = 2, Name = "new Name", UserInSystem = "Admin" };

            var result = await Service.Update(visitor);
            var resVisitor = await Service.GetByIdAsync(2);
     
            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(list.Count == 4);
            Assert.IsTrue(resVisitor.Name == "new Name");
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
