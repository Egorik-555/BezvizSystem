using System;
using BezvizSystem.DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.DAL.Tests
{
    [TestClass]
    public class UnitTestEqualsVisitors
    {
        Gender gender1 = new Gender { Name = "gender1"};
        Gender gender2 = new Gender { Name = "gender2"};

        Visitor visitor1;
        Visitor visitor2;
        Visitor visitor3;      

        public UnitTestEqualsVisitors()
        {
            visitor1 = new Visitor {Id = 1, Name = "name", Gender = gender1 };
            visitor2 = new Visitor {Id = 1, Name = "name", Gender = gender1 };
            visitor3 = new Visitor {Id = 3, Name = "name", Gender = gender2 };
        }

        [TestMethod]
        public void Test_Equals_Visitors()
        {
            Assert.IsTrue(visitor1.Equals(visitor2));          
            Assert.IsFalse(visitor1.Equals(visitor3));

        }
    }
}
