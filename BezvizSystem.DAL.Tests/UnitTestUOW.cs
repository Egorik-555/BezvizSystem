using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BezvizSystem.DAL.Tests
{
    [TestClass]
    public class UnitTestUOW
    {
        IUnitOfWork uow = new IdentityUnitOfWork("BezvizConnection");
        BezvizContext context = new BezvizContext("BezvizConnection");

        [TestMethod]
        public void Add_User_UnitOfWork()
        {
            BezvizUser user = new BezvizUser
            {
                UserName = "Egor"
            };

            OperatorProfile profile = new OperatorProfile
            {
                UNP = "123",
                OKPO = "123",
            };

            var result = uow.UserManager.FindByName(user.UserName);

            if (result != null)
            {
                var profileFind = uow.OperatorManager.GetById(result.Id);
                uow.OperatorManager.Delete(profileFind.Id);
                uow.UserManager.Delete(result);
            }

            uow.UserManager.Create(user);
            profile.Id = user.Id;
            profile = uow.OperatorManager.Create(profile);

            var resultUser = context.Users.Where(u => u.UserName == "Egor").FirstOrDefault();
            var resultProfile = context.OperatorProfiles.Where(p => p.UNP == "123").FirstOrDefault();

            Assert.IsNotNull(resultUser);
            Assert.IsNotNull(resultProfile);

            uow.OperatorManager.Delete(profile.Id);
            uow.UserManager.Delete(user);
        }

        [TestMethod]
        public void Add_Remove_Visitor_UnitOfWork()
        {
            Visitor visitor = new Visitor
            {
                Name = "Alex",
                Surname = "Taylor",
                DateInSystem = DateTime.Now
            };

            var findVisitor = uow.VisitorManager.GetAll().Where(v => v.Name == visitor.Name && v.Surname == visitor.Surname).FirstOrDefault();
            Visitor resultVisitor = null;
            if (findVisitor == null)
            {
                resultVisitor = uow.VisitorManager.Create(visitor);
            }

            findVisitor = uow.VisitorManager.GetAll().Where(v => v.Name == visitor.Name && v.Surname == visitor.Surname).FirstOrDefault();

            Assert.AreEqual(visitor, resultVisitor);
            Assert.IsNotNull(findVisitor);

            uow.VisitorManager.Delete(findVisitor.Id);
            findVisitor = uow.VisitorManager.GetAll().Where(v => v.Name == visitor.Name && v.Surname == visitor.Surname).FirstOrDefault();
            Assert.IsNull(findVisitor);
        }

        [TestMethod]
        public void Add_Remove_GroupVisitor_UnitOfWork()
        {
            List<Visitor> list = new List<Visitor>();
            Visitor visitor1 = new Visitor
            {
                Name = "test1",
                Surname = "surname test1",
                DateInSystem = DateTime.Now,
            };
            list.Add(visitor1);

            Visitor visitor2 = new Visitor
            {
                Name = "test2",
                Surname = "surname test2",
                DateInSystem = DateTime.Now,
            };
            list.Add(visitor2);

            GroupVisitor group = new GroupVisitor { PlaceOfRecidense = "test place"};
            group.Visitors = list;
            uow.GroupManager.Create(group);

            var findGroup = uow.GroupManager.GetAll().Where(g => g.PlaceOfRecidense == "test place").FirstOrDefault();
            Assert.IsNotNull(findGroup);
            Assert.IsTrue(findGroup.Visitors.Count == 2);

            uow.VisitorManager.Delete(visitor1.Id);
            uow.VisitorManager.Delete(visitor2.Id);
            uow.GroupManager.Delete(findGroup.Id);
            findGroup = uow.GroupManager.GetAll().Where(g => g.PlaceOfRecidense == "test place").FirstOrDefault();

            Assert.IsNull(findGroup);
        }
    }
}
