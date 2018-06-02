using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL;
using BezvizSystem.BLL.Services;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestAnketaService
    {
        IService<AnketaDTO> anketaService;
        Mock<IUnitOfWork> databaseMock = new Mock<IUnitOfWork>();
        Mock<IRepository<GroupVisitor, int>> groupMangerMock = new Mock<IRepository<GroupVisitor, int>>();
        IEnumerable<GroupVisitor> listGroups;

        public UnitTestAnketaService()
        {
            BezvizUser user1 = new BezvizUser { Id = "aaa", UserName = "Admin", OperatorProfile = new OperatorProfile { UNP = "UNPAdmin", Transcript = "Oper1" } };
            BezvizUser user2 = new BezvizUser { Id = "bbb", UserName = "Test", OperatorProfile = new OperatorProfile { UNP = "UNPTest", Transcript = "Oper2" } };

            CheckPoint point1 = new CheckPoint { Name = "point1" };
            CheckPoint point2 = new CheckPoint { Name = "point2" };

            listGroups = new List<GroupVisitor>()
            {
                new GroupVisitor { Id = 1,
                    CheckPoint = point1,
                    DateArrival = DateTime.Now,
                    User = user1,
                    Visitors = new List<Visitor>{ new Visitor {Id = 1,  Name = "testVisitor1" } } },

                new GroupVisitor { Id = 2,
                    CheckPoint = point2,
                    DateArrival = DateTime.Now,
                    User = user2,
                    Visitors = new List<Visitor>{ new Visitor {Id = 2, Name = "testVisitor21"} ,
                                                  new Visitor {Id = 3, Name = "testVisitor22"}}}
            };
       
            groupMangerMock.Setup(m => m.GetAll()).Returns(listGroups);
            groupMangerMock.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => listGroups.Where(g => g.Id == id).FirstOrDefault());

            databaseMock.Setup(m => m.GroupManager).Returns(groupMangerMock.Object);
            anketaService = new AnketaService(databaseMock.Object);
        }

        [TestMethod]
        public void Get_All_Anketa()
        {
            var anketas = anketaService.GetAll();
            var group = anketas.Where(v => v.Id == 2).First();

            Assert.IsTrue(anketas.Count() == 2);
            Assert.IsTrue(group.CheckPoint == "point2");
            Assert.IsTrue(group.Operator == "Oper2");
        }

        [TestMethod]
        public void Get_ById_Anketa()
        {
            var anketa = anketaService.GetById(2);

            Assert.IsTrue(anketa.CountMembers == 2);
            Assert.IsTrue(anketa.CheckPoint == "point2");
            Assert.IsTrue(anketa.Operator == "Oper2");
        }
    }
}
