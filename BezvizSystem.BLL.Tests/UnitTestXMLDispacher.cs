using System;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Tests.TestServises;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestXMLDispacher
    {
        IXMLDispatcher Service;
        IUnitOfWork Database;

        public UnitTestXMLDispacher()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            Database = repoes.CreateIoWManager();
            Service = new XMLDispatcher(Database);
        }

        [TestMethod]
        public async Task New_XMLDispatchAsync()
        {
            var result = await Service.New(3);
            var dispatches = Database.XMLDispatchManager.GetAll();

            //assert
            Assert.AreEqual(3, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatches.SingleOrDefault(x => x.Id == 2));
        }

        [TestMethod]
        public async Task Send_XMLDispatchAsync()
        {
            var result = await Service.Send(1);
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(1);

            //assert
            Assert.AreEqual(2, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Done, dispatch.Operation);
        }

        [TestMethod]
        public async Task Recd_XMLDispatchAsync()
        {
            var result = await Service.Recd(1);
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(1);

            //assert
            Assert.AreEqual(2, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.Recd, dispatch.Status);
            Assert.AreEqual(Operation.Done, dispatch.Operation);
        }

        [TestMethod]
        public async Task Edit_New_XMLDispatchAsync()
        {
            var result = await Service.Edit(1);
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(1);

            //assert
            Assert.AreEqual(2, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.New, dispatch.Status);
            Assert.AreEqual(Operation.Add, dispatch.Operation);
        }

        [TestMethod]
        public async Task Edit_Send_XMLDispatchAsync()
        {
            var result = await Service.Edit(2);
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(2);

            //assert
            Assert.AreEqual(2, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Edit, dispatch.Operation);
        }

        [TestMethod]
        public async Task Remove_New_XMLDispatchAsync()
        {
            var result = await Service.Remove(1);
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(1);

            //assert
            Assert.AreEqual(2, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.New, dispatch.Status);
            Assert.AreEqual(Operation.Add, dispatch.Operation);
        }

        [TestMethod]
        public async Task Remove_Send_XMLDispatchAsync()
        {
            var result = await Service.Remove(2);
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(2);

            //assert
            Assert.AreEqual(2, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Remove, dispatch.Operation);
        }
    }
}
