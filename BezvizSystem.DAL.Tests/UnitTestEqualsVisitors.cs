using System;
using BezvizSystem.DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.DAL.Tests
{
    [TestClass]
    public class UnitTestEqualsVisitors
    {
        Gender gender1 = new Gender { Name = "gender1"};
        Gender gender2 = new Gender { Name = "gender2" };

        Nationality nat1 = new Nationality { Name = "nat1" };

        Visitor visitor1;
        Visitor visitor2;
        Visitor visitor3;
        Visitor visitor4;
        Visitor visitor5;
        Visitor visitor6;

        public UnitTestEqualsVisitors()
        {
            visitor1 = new Visitor { Name = "name", Gender = gender1 };
            visitor2 = new Visitor { Name = "name", Gender = gender1 };
            visitor3 = new Visitor { Name = "name", Gender = gender2 };

            visitor4 = new Visitor { Nationality = nat1, BithDate = DateTime.Now.Date};
            visitor5 = new Visitor { Nationality = nat1, BithDate = DateTime.Now.Date };
            visitor6 = new Visitor { };
        }

        [TestMethod]
        public void Test_Equals_Visitors()
        {
            Assert.IsTrue(visitor1.Equals(visitor2));
            Assert.IsTrue(visitor4.Equals(visitor5));
            Assert.IsTrue(visitor6.Equals(visitor6));
            Assert.IsFalse(visitor1.Equals(visitor3));

        }
    }
}
