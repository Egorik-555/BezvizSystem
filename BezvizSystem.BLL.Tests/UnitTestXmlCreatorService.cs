using System;
using System.Linq;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Services.XML;
using BezvizSystem.BLL.Tests.TestServises;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BezvizSystem.DAL.Helpers;
using System.Threading.Tasks;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Entities;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using System.Xml.Linq;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestXmlCreatorService
    {
        IUnitOfWork repo;
        IXmlCreator _service;
        IXMLDispatcher _xmlDispatcher;

        public UnitTestXmlCreatorService()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            repo = repoes.CreateIoWManager();

            _service = new XmlCreatorPogran(repo);
            _xmlDispatcher = new XMLDispatcher(repo);
        }

        [TestMethod]
        public void Test_ZipArchieve()
        {
            XElement form = new XElement("EXPORT", "test");
            var doc = new XDocument(form);

        
            doc.Save("file1.xml");


            ZipFile zipFile = new ZipFile("test.zip");
            zipFile.AddFile("file1.xml");
            zipFile.Save();

            File.Delete("file1.xml");
            

        }
        [TestMethod]
        public async Task Test_Save_New_with_one_argument()
        {        
            var visitors1 = _xmlDispatcher.GetAdded().ToList();
            var result1 = await _service.SaveNew("test.xml");
            var visitors2 = _xmlDispatcher.GetAdded().ToList();

            Assert.AreEqual(3, visitors1.Count());
            Assert.AreEqual(0, visitors2.Count());
            Assert.IsTrue(result1.Succedeed);
        }

        [TestMethod]
        public async Task Test_Save_Extra_with_one_argument()
        {
            Visitor visitor = new Visitor
            {
                Id = 10,
                Name = "testName",
                Surname = "testSurname",
                BithDate = new DateTime(1965, 1, 1)
            };

            GroupVisitor group = new GroupVisitor
            {
                ExtraSend = true,
                DaysOfStay = 1,
                DateArrival = new DateTime(2018, 9, 1),
                Visitors = new List<Visitor> { visitor },
                UserInSystem = "Test1"
            };

            visitor.Group = group;
            repo.VisitorManager.Create(visitor);
            repo.GroupManager.Create(group);
            repo.XMLDispatchManager.Create(new XMLDispatch { Id = 10, Operation = Operation.Add, Status = Status.New });

            var visitors1 = _xmlDispatcher.GetAdded().ToList();
            var result1 = await _service.SaveExtra("test.xml");
            var visitors2 = _xmlDispatcher.GetAdded().ToList();

            Assert.AreEqual(4, visitors1.Count());
            Assert.AreEqual(3, visitors2.Count());
            Assert.IsTrue(result1.Succedeed);
        }

        [TestMethod]
        public async Task Test_Unload_Edit_Item_with_one_argument()
        {
            Visitor visitor = new Visitor
            {
                Id = 3,
                Name = "testName",
                Surname = "testSurname",
                BithDate = new DateTime(1965, 1, 1)
            };

            GroupVisitor group = new GroupVisitor
            {
                ExtraSend = true,
                DaysOfStay = 1,
                DateArrival = new DateTime(2018, 9, 1),
                Visitors = new List<Visitor> { visitor },
                UserInSystem = "Test1"
            };

            visitor.Group = group;
            repo.VisitorManager.Create(visitor);
            repo.GroupManager.Create(group);
            repo.XMLDispatchManager.Create(new XMLDispatch { Id = 3, Operation = Operation.Edit, Status = Status.Send });

            var visitors1 = _xmlDispatcher.GetUpdated().ToList();
            var result1 = await _service.SaveNew("test.xml");
            var visitors2 = _xmlDispatcher.GetUpdated().ToList();

            Assert.AreEqual(1, visitors1.Count());
            Assert.AreEqual(0, visitors2.Count());
            Assert.IsTrue(result1.Succedeed);
        }

        [TestMethod]
        public async Task Test_Unload_Edit_Extra_Item_with_one_argument()
        {
            Visitor visitor = new Visitor
            {
                Id = 10,
                Name = "testName",
                Surname = "testSurname",
                BithDate = new DateTime(1965, 1, 1)
            };

            GroupVisitor group = new GroupVisitor
            {
                ExtraSend = true,
                DaysOfStay = 1,
                DateArrival = new DateTime(2018, 9, 1),
                Visitors = new List<Visitor> { visitor },
                UserInSystem = "Test1"
            };

            visitor.Group = group;
            repo.VisitorManager.Create(visitor);
            repo.GroupManager.Create(group);
            repo.XMLDispatchManager.Create(new XMLDispatch { Id = 10, Operation = Operation.Edit, Status = Status.Send });

            var visitors1 = _xmlDispatcher.GetUpdated().ToList();
            var result1 = await _service.SaveExtra("test.xml");
            var visitors2 = _xmlDispatcher.GetUpdated().ToList();

            Assert.AreEqual(1, visitors1.Count());
            Assert.AreEqual(0, visitors2.Count());
            Assert.IsTrue(result1.Succedeed);
        }

        [TestMethod]
        public async Task Test_Unload_Removed_Item_with_one_argument()
        {
            File.Delete("test.zip");

            repo.XMLDispatchManager.Create(new XMLDispatch { Id = 6, Operation = Operation.Remove, Status = Status.Send });
            repo.XMLDispatchManager.Create(new XMLDispatch { Id = 7, Operation = Operation.Remove, Status = Status.Send });

            var visitors1 = _xmlDispatcher.GetRemoved().ToList();
            var result1 = await _service.SaveNew("test.xml");
            var visitors2 = _xmlDispatcher.GetRemoved().ToList();

            Assert.AreEqual(2, visitors1.Count());
            Assert.AreEqual(0, visitors2.Count());
            Assert.IsTrue(result1.Succedeed);
        }

        [TestMethod]
        public async Task Test_Unload_Removed_Extra_Item_with_one_argument()
        {
            repo.XMLDispatchManager.Create(new XMLDispatch { Id = 3, Operation = Operation.Remove, Status = Status.Send });
            repo.XMLDispatchManager.Create(new XMLDispatch { Id = 4, Operation = Operation.Remove, Status = Status.Send });

            var visitors1 = _xmlDispatcher.GetRemoved().ToList();
            var result1 = await _service.SaveExtra("test.xml");
            var visitors2 = _xmlDispatcher.GetRemoved().ToList();

            Assert.AreEqual(2, visitors1.Count());
            Assert.AreEqual(2, visitors2.Count());
            Assert.IsFalse(result1.Succedeed);
        }
    }
}
