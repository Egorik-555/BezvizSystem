using System;
using System.Linq;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Services.XML;
using BezvizSystem.BLL.Tests.TestServises;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestXmlCreatorService
    {
        IXmlCreator _service;
        IService<VisitorDTO> _serviceVisitor;

        public UnitTestXmlCreatorService()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            var repo = repoes.CreateIoWManager();

            _service = new XmlCreatorPogran(repo);
            _serviceVisitor = new VisitorService(repo);
        }


        [TestMethod]
        public void Test_Save_with_one_argument()
        {
            var visitors1 = _serviceVisitor.GetAll().Where(v => v.StatusName == "status1").ToList();
            var result1 = _service.Save("test.xml");
            var visitors2 = _serviceVisitor.GetAll().Where(v => v.StatusName == "status1").ToList();

            Assert.IsTrue(visitors1.Count() == 4);
            Assert.IsTrue(visitors2.Count() == 0);
            Assert.IsTrue(result1.Succedeed);
        }
    }
}
