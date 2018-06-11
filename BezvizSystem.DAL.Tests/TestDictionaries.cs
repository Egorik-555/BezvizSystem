using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace BezvizSystem.DAL.Tests
{
    [TestClass]
    public class TestDictionaries
    {

        //IContext context = new BezvizContext("BezvizConnection");
        Mock<IContext> context;
        CheckPointManager checkPointmng;

        public TestDictionaries()
        {
            Mock<IContext> context = new Mock<IContext>();

            List<CheckPoint> checkPoints = new List<CheckPoint>()
                {
                    new CheckPoint{ Id = 1, Name = "check1", Active = true, UserInSystem = "test1"},
                    new CheckPoint{ Id = 2, Name = "check2", Active = true, UserInSystem = "test1"},
                    new CheckPoint{ Id = 3, Name = "check3", Active = false, UserInSystem = "test2"},
                    new CheckPoint{ Id = 4, Name = "check4", Active = true, UserInSystem = "test1"}
                };


            context.Setup(m => m.CheckPoints).Returns(checkPoints.AsQueryable());

        }



        [TestMethod]
        public async Task CheckPoint()
        {
            checkPointmng = new CheckPointManager(context);
            CheckPoint checkPoint1 = checkPointmng.GetById(1);
          
            CheckPoint checkPoint2 = await checkPointmng.GetByIdAsync(1);

            Assert.IsNotNull(checkPoint1);
            Assert.IsTrue(checkPoint1.Active);
            Assert.IsNotNull(checkPoint2);
            Assert.IsTrue(checkPoint2.Name.Contains("Брест"));
        }
    }
}
