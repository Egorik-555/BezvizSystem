using System;
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
    public class UnitTestVisitorServiceWithXmlDispatcher
    {
        IService<VisitorDTO> Service;
        IUnitOfWork database;

        public UnitTestVisitorServiceWithXmlDispatcher()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            database = repoes.CreateIoWManager();
            Service = new VisitorService(database);
        }

        [TestMethod]
        public async Task Create_Visitor()
        {
            VisitorDTO visitor1 = new VisitorDTO { Id = 111, Name = "newTest", Nationality = "nat1", Gender = "Мужчина", UserInSystem = "Test1", DateInSystem = DateTime.Now };
            VisitorDTO visitor2 = new VisitorDTO { Id = 222, Name = "newTest", Nationality = "nat1", Gender = "Error", UserInSystem = "Test1", DateInSystem = DateTime.Now };

            var result1 = await Service.Create(visitor1);
            var result2 = await Service.Create(visitor2);
            var count = Service.GetAll().Count();

            var addedVisitor1 = await Service.GetByIdAsync(111);
            var addedVisitor2 = await Service.GetByIdAsync(222);

            var dispatch1 = await database.XMLDispatchManager.GetByIdAsync(111);
            var dispatch2 = await database.XMLDispatchManager.GetByIdAsync(222);

            Assert.IsTrue(result1.Succedeed);
            Assert.IsTrue(result2.Succedeed);
            Assert.AreEqual(4, count);

            Assert.AreEqual(Status.New, dispatch1.Status);
            Assert.AreEqual(Operation.Add, dispatch1.Operation);
        }

        [TestMethod]
        public async Task Update_Visitor()
        {

            VisitorDTO visitor = new VisitorDTO
            {
                Id = 2,
                Name = "new Name",
                Nationality = "nat2"
            };

            var result = await Service.Update(visitor);
            var dispatch = await database.XMLDispatchManager.GetByIdAsync(2);

            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Edit, dispatch.Operation);
        }

        [TestMethod]
        public async Task Delete_Send_Visitor()
        {
            var result = await Service.Delete(2);
            var visitor = await Service.GetByIdAsync(2);
            var dispatch = database.XMLDispatchManager.GetById(2);

            Assert.IsNull(visitor);
            Assert.IsTrue(result.Succedeed);

            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Remove, dispatch.Operation);
        }

        [TestMethod]
        public async Task Delete_New_Visitor()
        {
            var result = await Service.Delete(1);
            var visitor = await Service.GetByIdAsync(1);
            var dispatch = database.XMLDispatchManager.GetById(1);

            Assert.IsNull(visitor);
            Assert.IsTrue(result.Succedeed);

            Assert.IsNull(dispatch);
        }
    }
}
