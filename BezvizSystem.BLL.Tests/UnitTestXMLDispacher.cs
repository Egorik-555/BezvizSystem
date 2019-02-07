using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Tests.TestServises;
using BezvizSystem.DAL.Entities;
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

            Database.XMLDispatchManager.Create(new XMLDispatch { Id = 10, Status = Status.New, Operation = Operation.Add, DateInSystem = DateTime.Now });
            Database.XMLDispatchManager.Create(new XMLDispatch { Id = 11, Status = Status.Send, Operation = Operation.Done, DateInSystem = DateTime.Now });

            Service = new XMLDispatcher(Database);           
        }

        [TestMethod]
        public async Task New_XMLDispatchAsync()
        {
            var result = await Service.New(new Visitor { Id = 13 });
            var dispatches = Database.XMLDispatchManager.GetAll();

            //assert
            Assert.AreEqual(7, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatches.SingleOrDefault(x => x.Id == 13));
        }

        [TestMethod]
        public async Task New_XMLDispatchES_Async()
        {
            var result = await Service.New(new List<Visitor> { new Visitor { Id = 13 }, new Visitor { Id = 14 } });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var newDispatch = await Database.XMLDispatchManager.GetByIdAsync(14);


            //assert
            Assert.AreEqual(8, dispatches.Count());
            Assert.AreEqual(2, result.Count);

            Assert.IsNotNull(newDispatch);
            Assert.AreEqual(Status.New, newDispatch.Status);
            Assert.AreEqual(Operation.Add, newDispatch.Operation);
        }

        [TestMethod]
        public async Task Send_XMLDispatchAsync()
        {
            var result = await Service.Send(new Visitor { Id = 10 });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(10);

            //assert
            Assert.AreEqual(6, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Done, dispatch.Operation);
        }

        [TestMethod]
        public async Task Send_XMLDispatchES_Async()
        {
            var result = await Service.Send(new List<Visitor> { new Visitor { Id = 10 }, new Visitor { Id = 11 } });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch1 = await Database.XMLDispatchManager.GetByIdAsync(10);
            var dispatch2 = await Database.XMLDispatchManager.GetByIdAsync(11);

            //assert
            Assert.AreEqual(6, dispatches.Count());
            Assert.AreEqual(2, result.Count);
            Assert.IsNotNull(dispatch1);
            Assert.IsNotNull(dispatch2);

            Assert.AreEqual(Status.Send, dispatch1.Status);
            Assert.AreEqual(Operation.Done, dispatch1.Operation);
            Assert.AreEqual(Status.Send, dispatch2.Status);
            Assert.AreEqual(Operation.Done, dispatch2.Operation);
        }

        [TestMethod]
        public async Task Recd_XMLDispatch_Async()
        {
            var result = await Service.Recd(new Visitor { Id = 10 });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(10);

            //assert
            Assert.AreEqual(6, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.Recd, dispatch.Status);
            Assert.AreEqual(Operation.Done, dispatch.Operation);
        }

        [TestMethod]
        public async Task Recd_XMLDispatchES_Async()
        {
            var result = await Service.Recd(new List<Visitor> { new Visitor { Id = 10 }, new Visitor { Id = 11 } });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch1 = await Database.XMLDispatchManager.GetByIdAsync(10);
            var dispatch2 = await Database.XMLDispatchManager.GetByIdAsync(11);

            //assert
            Assert.AreEqual(6, dispatches.Count());
            Assert.AreEqual(2, result.Count);

            Assert.IsNotNull(dispatch1);
            Assert.IsNotNull(dispatch2);
            Assert.AreEqual(Status.Recd, dispatch1.Status);
            Assert.AreEqual(Operation.Done, dispatch1.Operation);
            Assert.AreEqual(Status.Recd, dispatch2.Status);
            Assert.AreEqual(Operation.Done, dispatch2.Operation);
        }

        [TestMethod]
        public async Task Edit_New_XMLDispatchAsync()
        {
            var result = await Service.Edit(new Visitor { Id = 10 });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(1);

            //assert
            Assert.AreEqual(6, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.New, dispatch.Status);
            Assert.AreEqual(Operation.Add, dispatch.Operation);
        }

        [TestMethod]
        public async Task Edit_Send_XMLDispatchAsync()
        {
            var result = await Service.Edit(new Visitor { Id = 11 });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(11);

            //assert
            Assert.AreEqual(6, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Edit, dispatch.Operation);
        }

        [TestMethod]
        public async Task Edit_Visitor1_New_Visitor2_Send_XMLDispatchES_Async()
        {
            var oldVisitors = new List<Visitor> { new Visitor { Id = 10 }, new Visitor { Id = 11 } };
            var newVisitors = new List<Visitor> { new Visitor { Id = 10, Name = "name1" }, new Visitor { Id = 11, Name = "name2" } };

            var result = await Service.Edit(oldVisitors, newVisitors);
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch1 = await Database.XMLDispatchManager.GetByIdAsync(10);
            var dispatch2 = await Database.XMLDispatchManager.GetByIdAsync(11);

            //assert
            Assert.AreEqual(6, dispatches.Count());
            Assert.AreEqual(2, result.Count);
            
            Assert.AreEqual(Status.New, dispatch1.Status);
            Assert.AreEqual(Operation.Add, dispatch1.Operation);
            Assert.AreEqual(Status.Send, dispatch2.Status);
            Assert.AreEqual(Operation.Edit, dispatch2.Operation);
        }

        [TestMethod]
        public async Task Edit_Add_Visitor3_XMLDispatchES_Async()
        {
            var oldVisitors = new List<Visitor> { new Visitor { Id = 1 }, new Visitor { Id = 2 } };
            var newVisitors = new List<Visitor> { new Visitor { Id = 1}, new Visitor { Id = 2}, new Visitor { Id = 3} };

            var result = await Service.Edit(oldVisitors, newVisitors);
            var dispatches = Database.XMLDispatchManager.GetAll();

            var dispatch1 = await Database.XMLDispatchManager.GetByIdAsync(1);
            var dispatch2 = await Database.XMLDispatchManager.GetByIdAsync(2);
            var dispatch3 = await Database.XMLDispatchManager.GetByIdAsync(3);

            //assert
            Assert.AreEqual(5, dispatches.Count());
            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(Status.New, dispatch1.Status);
            Assert.AreEqual(Operation.Add, dispatch1.Operation);
            Assert.AreEqual(Status.Send, dispatch2.Status);
            Assert.AreEqual(Operation.Edit, dispatch2.Operation);
            Assert.AreEqual(Status.New, dispatch3.Status);
            Assert.AreEqual(Operation.Add, dispatch3.Operation);
        }

        [TestMethod]
        public async Task Edit_Delete_All_Visitors_XMLDispatchES_Async()
        {
            var oldVisitors = new List<Visitor> { new Visitor { Id = 10 }, new Visitor { Id = 11 } };
            var newVisitors = new List<Visitor>();

            var result = await Service.Edit(oldVisitors, newVisitors);
            var dispatches = Database.XMLDispatchManager.GetAll();

            var dispatch1 = await Database.XMLDispatchManager.GetByIdAsync(10);
            var dispatch2 = await Database.XMLDispatchManager.GetByIdAsync(11);           

            //assert      
            Assert.AreEqual(5, dispatches.Count());
            Assert.AreEqual(2, result.Count);

            Assert.IsNull(dispatch1);
            Assert.AreEqual(Status.Send, dispatch2.Status);
            Assert.AreEqual(Operation.Remove, dispatch2.Operation);
        }

        [TestMethod]
        public async Task Edit_Visitor2_Send_Add_Visitor3_XMLDispatchES_Async()
        {
            var oldVisitors = new List<Visitor> { new Visitor { Id = 11 } };
            var newVisitors = new List<Visitor> { new Visitor { Id = 11, Name = "name2" }, new Visitor { Id = 12, Name = "name3"} };

            var result = await Service.Edit(oldVisitors, newVisitors);
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch1 = await Database.XMLDispatchManager.GetByIdAsync(11);
            var dispatch2 = await Database.XMLDispatchManager.GetByIdAsync(12);

            //assert
            Assert.AreEqual(7, dispatches.Count());
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(Status.Send, dispatch1.Status);
            Assert.AreEqual(Operation.Edit, dispatch1.Operation);
            Assert.AreEqual(Status.New, dispatch2.Status);
            Assert.AreEqual(Operation.Add, dispatch2.Operation);
        }

        [TestMethod]
        public async Task Remove_New_XMLDispatchAsync()
        {
            var result = await Service.Remove(new Visitor { Id = 10 });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(10);

            //assert
            Assert.AreEqual(5, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNull(dispatch);
        }

        [TestMethod]
        public async Task Remove_Send_XMLDispatchAsync()
        {
            var result = await Service.Remove(new Visitor { Id = 11 });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch = await Database.XMLDispatchManager.GetByIdAsync(11);

            //assert
            Assert.AreEqual(6, dispatches.Count());
            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(dispatch);
            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Remove, dispatch.Operation);
        }

        [TestMethod]
        public async Task Remove_XMLDispatchES_Async()
        {
            var result = await Service.Remove(new List<Visitor> { new Visitor { Id = 10 }, new Visitor { Id = 11 } });
            var dispatches = Database.XMLDispatchManager.GetAll();
            var dispatch1 = await Database.XMLDispatchManager.GetByIdAsync(10);
            var dispatch2 = await Database.XMLDispatchManager.GetByIdAsync(11);

            //assert
            Assert.AreEqual(5, dispatches.Count());
            Assert.AreEqual(2, result.Count);

            Assert.IsNull(dispatch1);
            Assert.AreEqual(Status.Send, dispatch2.Status);
            Assert.AreEqual(Operation.Remove, dispatch2.Operation);
        }
    }
}
