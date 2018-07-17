using System;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.BLL.Services.XML;
using BezvizSystem.BLL.Tests.TestServises;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestXmlCreatorService
    {
        IXmlCreator _service;

        public UnitTestXmlCreatorService()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            _service = new XmlCreatorPogran(repoes.CreateIoWManager());
        }


        [TestMethod]
        public void Test_Save_with_one_argument()
        {
            _service.Save("test.xml");
        }
    }
}
