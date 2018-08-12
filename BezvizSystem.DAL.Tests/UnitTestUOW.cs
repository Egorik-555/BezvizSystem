using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Repositories;
using BezvizSystem.DAL.StateVisitor;
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
                UserName = "Egor",
                OperatorProfile = new OperatorProfile
                {
                    UNP = "123",
                    OKPO = "123",
                }
            };

            var result = uow.UserManager.FindByName(user.UserName);

            if (result != null)
            {
                var profileFind = uow.OperatorManager.GetById(result.Id);
                uow.OperatorManager.Delete(profileFind.Id);
                uow.UserManager.Delete(result);
            }

            uow.UserManager.Create(user);
            var resultUser = context.Users.Where(u => u.UserName == "Egor").FirstOrDefault();
            var resultProfile = context.OperatorProfiles.Where(p => p.UNP == "123").FirstOrDefault();

            Assert.IsNotNull(resultUser);
            Assert.IsNotNull(resultProfile);

            uow.OperatorManager.Delete(user.Id);
            uow.UserManager.Delete(user);
        }

        [TestMethod]
        public async Task Update_User_UnitOfWork()
        {
            BezvizUser user = new BezvizUser
            {
                UserName = "Egor",
                OperatorProfile = new OperatorProfile
                {
                    UNP = "123",
                    OKPO = "123",
                }
            };

            var result = uow.UserManager.FindByName(user.UserName);
            if (result != null)
            {
                uow.OperatorManager.Delete(result.Id);
                uow.UserManager.Delete(result);
            }

            var createUserResult = uow.UserManager.Create(user);

            if (createUserResult.Succeeded)
            {
                var testUser = await uow.UserManager.FindByIdAsync(user.Id);

                testUser.OperatorProfile.Transcript = "Transcript";
                testUser.Email = "test@test.ru";
                var updateResult = await uow.UserManager.UpdateAsync(testUser);

                Assert.IsTrue(updateResult.Succeeded);

                uow.OperatorManager.Delete(user.Id);
                uow.UserManager.Delete(user);
            }
        }

        [TestMethod]
        public void Add_Update_Remove_Visitor_UnitOfWork()
        {
            Visitor visitor = new Visitor
            {
                Id = 1,
                Name = "Alex",
                Surname = "Taylor",
                DateInSystem = DateTime.Now,
                XML = new XMLDispatch { Status = Status.New, Operation = Operation.Add }
            };

            //create
            var findVisitor = uow.VisitorManager.GetAll().Where(v => v.Name == visitor.Name && v.Surname == visitor.Surname).FirstOrDefault();
            Visitor resultVisitor = null;
            if (findVisitor == null)
            {
                resultVisitor = uow.VisitorManager.Create(visitor);
            }
            else
            {
                resultVisitor = findVisitor;
            }
            findVisitor = uow.VisitorManager.GetAll().Where(v => v.Name == visitor.Name && v.Surname == visitor.Surname).FirstOrDefault();

            Assert.IsNotNull(findVisitor);
            Assert.AreEqual(Status.New, findVisitor.XML.Status);
            ////

            //edit
            var newVisitor = findVisitor;
            newVisitor.Name = "New name";
            newVisitor.BithDate = new DateTime(2018, 1, 1);
            newVisitor.XML.Operation = Operation.Done;

            uow.VisitorManager.Update(findVisitor);
            findVisitor = uow.VisitorManager.GetAll().Where(v => v.Name == newVisitor.Name && v.Surname == newVisitor.Surname).FirstOrDefault();

            Assert.IsNotNull(findVisitor);
            Assert.AreEqual(Operation.Done, findVisitor.XML.Operation);
            ////

            uow.VisitorManager.Delete(findVisitor.Id);
            findVisitor = uow.VisitorManager.GetAll().Where(v => v.Name == visitor.Name && v.Surname == visitor.Surname).FirstOrDefault();
            Assert.IsNull(findVisitor);
        }

        [TestMethod]
        public async Task Get_And_State_Visitor_UnitOfWork()
        {
            Visitor visitor1 = new Visitor
            {
                Id = 1,
                Name = "Test1",
                Surname = "Taylor",
                DateInSystem = DateTime.Now,
                XML = new XMLDispatch { Status = Status.New, Operation = Operation.Add }
            };

            Visitor visitor2 = new Visitor
            {
                Id = 2,
                Name = "Test2",
                Surname = "Taylor",
                DateInSystem = DateTime.Now,
                XML = new XMLDispatch { Status = Status.Send, Operation = Operation.Add }
            };

            var findVisitor1 = uow.VisitorManager.GetAll().Where(v => v.Name == visitor1.Name && v.Surname == visitor1.Surname).FirstOrDefault();
            var findVisitor2 = uow.VisitorManager.GetAll().Where(v => v.Name == visitor2.Name && v.Surname == visitor2.Surname).FirstOrDefault();
            Visitor resultVisitor1 = null;
            Visitor resultVisitor2 = null;
            if (findVisitor1 == null) resultVisitor1 = uow.VisitorManager.Create(visitor1);
            else resultVisitor1 = findVisitor1;

            if (findVisitor2 == null) resultVisitor2 = uow.VisitorManager.Create(visitor2);
            else resultVisitor2 = findVisitor2;

            //getall
            findVisitor1 = uow.VisitorManager.GetAll().Where(v => v.Name == visitor1.Name && v.Surname == visitor1.Surname).FirstOrDefault();
            findVisitor2 = uow.VisitorManager.GetAll().Where(v => v.Name == visitor2.Name && v.Surname == visitor2.Surname).FirstOrDefault();

            Assert.IsNotNull(findVisitor1);
            Assert.IsInstanceOfType(findVisitor1.State, typeof(NewVisitorState));
            Assert.IsNotNull(findVisitor2);
            Assert.IsInstanceOfType(findVisitor2.State, typeof(SendVisitorState));
            ///

            //getbyid
            findVisitor1 = uow.VisitorManager.GetById(resultVisitor1.Id);
            findVisitor2 = uow.VisitorManager.GetById(resultVisitor2.Id);

            Assert.IsNotNull(findVisitor1);
            Assert.IsInstanceOfType(findVisitor1.State, typeof(NewVisitorState));
            Assert.IsNotNull(findVisitor2);
            Assert.IsInstanceOfType(findVisitor2.State, typeof(SendVisitorState));
            ///

            //getbyidasync
            findVisitor1 = await uow.VisitorManager.GetByIdAsync(resultVisitor1.Id);
            findVisitor2 = await uow.VisitorManager.GetByIdAsync(resultVisitor2.Id);

            Assert.IsNotNull(findVisitor1);
            Assert.IsInstanceOfType(findVisitor1.State, typeof(NewVisitorState));
            Assert.IsNotNull(findVisitor2);
            Assert.IsInstanceOfType(findVisitor2.State, typeof(SendVisitorState));
            ///

            uow.VisitorManager.Delete(findVisitor1.Id);
            uow.VisitorManager.Delete(findVisitor2.Id);
            findVisitor1 = uow.VisitorManager.GetById(findVisitor1.Id);
            findVisitor2 = uow.VisitorManager.GetById(findVisitor2.Id);
            Assert.IsNull(findVisitor1);
            Assert.IsNull(findVisitor2);
        }

        [TestMethod]
        public void Change_State_Visitor()
        {
            Visitor visitor = new Visitor
            {
                Id = 1,
                Name = "Alex",
                Surname = "Taylor",
                DateInSystem = DateTime.Now,
                XML = new XMLDispatch { Status = Status.New, Operation = Operation.Add}
            };

            Assert.IsInstanceOfType(visitor.State, typeof(NewVisitorState));

            //new
            visitor.Edit();
            Assert.IsInstanceOfType(visitor.State, typeof(NewVisitorState));
            Assert.AreEqual(Status.New, visitor.XML.Status);
            Assert.AreEqual(Operation.Add, visitor.XML.Operation);

            visitor.Remove();
            Assert.IsInstanceOfType(visitor.State, typeof(NewVisitorState));
            Assert.AreEqual(Status.New, visitor.XML.Status);
            Assert.AreEqual(Operation.Add, visitor.XML.Operation);

            visitor.Recd();
            Assert.IsInstanceOfType(visitor.State, typeof(NewVisitorState));
            Assert.AreEqual(Status.New, visitor.XML.Status);
            Assert.AreEqual(Operation.Add, visitor.XML.Operation);

            visitor.Send();
            Assert.IsInstanceOfType(visitor.State, typeof(SendVisitorState));
            Assert.AreEqual(Status.Send, visitor.XML.Status);
            Assert.AreEqual(Operation.Done, visitor.XML.Operation);

            //send
            visitor.Edit();
            Assert.IsInstanceOfType(visitor.State, typeof(SendVisitorState));
            Assert.AreEqual(Status.Send, visitor.XML.Status);
            Assert.AreEqual(Operation.Edit, visitor.XML.Operation);

            visitor.Remove();
            Assert.IsInstanceOfType(visitor.State, typeof(SendVisitorState));
            Assert.AreEqual(Status.Send, visitor.XML.Status);
            Assert.AreEqual(Operation.Remove, visitor.XML.Operation);

            visitor.Send();
            Assert.IsInstanceOfType(visitor.State, typeof(SendVisitorState));
            Assert.AreEqual(Status.Send, visitor.XML.Status);
            Assert.AreEqual(Operation.Remove, visitor.XML.Operation);

            visitor.Recd();
            Assert.IsInstanceOfType(visitor.State, typeof(RecdVisitorState));
            Assert.AreEqual(Status.Recd, visitor.XML.Status);
            Assert.AreEqual(Operation.Done, visitor.XML.Operation);          
        }

        [TestMethod]
        public void Add_Update_Remove_GroupVisitor_UnitOfWork()
        {
            //create
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

            GroupVisitor group = new GroupVisitor { PlaceOfRecidense = "test place" };
            group.Visitors = list;
            uow.GroupManager.Create(group);

            var findGroup = uow.GroupManager.GetAll().Where(g => g.Id == group.Id).FirstOrDefault();
            Assert.IsNotNull(findGroup);
            Assert.IsTrue(findGroup.Visitors.Count == 2);
            ////


            //edit
            List<Visitor> newList = new List<Visitor>();
            newList.Add(visitor1);
            group.PlaceOfRecidense = "new place";
            group.Visitors = newList;

            uow.GroupManager.Update(group);
            findGroup = uow.GroupManager.GetAll().Where(g => g.Id == group.Id).FirstOrDefault();
            Assert.AreEqual("new place", findGroup.PlaceOfRecidense);
            Assert.IsTrue(findGroup.Visitors.Count == 1);
            ///

            uow.VisitorManager.Delete(visitor1.Id);
            uow.VisitorManager.Delete(visitor2.Id);
            uow.GroupManager.Delete(findGroup.Id);
            findGroup = uow.GroupManager.GetAll().Where(g => g.Id == group.Id).FirstOrDefault();

            Assert.IsNull(findGroup);
        }

        [TestMethod]
        public void Add_Update_Remove_XMLDispatch_UnitOfWork()
        {
            //create
            XMLDispatch dispatch = new XMLDispatch { /*IdVisitor = 1,*/ Operation = Operation.Add, Status = Status.New };

            uow.XMLDispatchManager.Create(dispatch);

            var findDispatch = uow.XMLDispatchManager.GetAll().Where(d => d.Id == dispatch.Id).FirstOrDefault();
            Assert.AreEqual(Operation.Add, findDispatch.Operation);
            Assert.AreEqual(Status.New, findDispatch.Status);
            ////

            //edit
            var newDispatch = dispatch;
            newDispatch.Operation = Operation.Done;
            newDispatch.Status = Status.Send;

            uow.XMLDispatchManager.Update(newDispatch);
            findDispatch = uow.XMLDispatchManager.GetAll().Where(d => d.Id == newDispatch.Id).FirstOrDefault();
            Assert.AreEqual(Operation.Done, findDispatch.Operation);
            Assert.AreEqual(Status.Send, findDispatch.Status);
            ////

            uow.XMLDispatchManager.Delete(newDispatch.Id);
            findDispatch = uow.XMLDispatchManager.GetAll().Where(d => d.Id == newDispatch.Id).FirstOrDefault();

            Assert.IsNull(findDispatch);
        }

        [TestMethod]
        public void Get_Status_ById_XMLDispatch_UnitOfWork()
        {
            TestRepoXmlDispatch test = new TestRepoXmlDispatch();

            Status status1 = test.GetStatusByIdRecord(11);
            Status status2 = test.GetStatusByIdRecord(12);
            Status status3 = test.GetStatusByIdRecord(13);

            Assert.AreEqual(Status.New, status1);

        }
    }
}
