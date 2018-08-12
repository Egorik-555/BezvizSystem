using BezvizSystem.DAL;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Identity;
using BezvizSystem.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Tests.TestServises
{
    public class CreateTestRepositories
    {
        OperatorProfile profile1 = new OperatorProfile
        {
            Id = "aaa",
            Transcript = "operator1",
            UNP = "UnpTest1",
            OKPO = "OKPO1",
            Role = "Operator",
            DateInSystem = new DateTime(2018, 1, 1)
        };

        OperatorProfile profile2 = new OperatorProfile { Id = "bbb", Transcript = "operator2", UNP = "UnpTest2", OKPO = "OKPO2", Role = "Operator" };
        OperatorProfile profile3 = new OperatorProfile { Id = "ccc", Transcript = "operator3", UNP = "UnpTest3", OKPO = "OKPO3", Role = "Operator" };
        OperatorProfile profile4 = new OperatorProfile { Id = "ddd", Transcript = "AdminTran", UNP = "UnpAdmin", OKPO = "OKPO4", Role = "Admin" };

        BezvizUser user1 = new BezvizUser { Id = "aaa", UserName = "Test1" };
        BezvizUser user2 = new BezvizUser { Id = "bbb", UserName = "Test2" };
        BezvizUser user3 = new BezvizUser { Id = "ccc", UserName = "Test3" };
        BezvizUser user4 = new BezvizUser { Id = "ddd", UserName = "Admin" };

        Nationality nat1 = new Nationality { Id = 1, Code = 1, Name = "nat1", ShortName = "n1", Active = true };
        Nationality nat2 = new Nationality { Id = 2, Code = 2, Name = "nat2", ShortName = "n2", Active = true };
        Nationality nat3 = new Nationality { Id = 3, Code = 3, Name = "nat3", ShortName = "n3", Active = true };

        Gender gender1 = new Gender { Id = 1, Code = 1, Name = "Мужчина", Active = true };
        Gender gender2 = new Gender { Id = 2, Code = 2, Name = "Женщина", Active = true };

        Visitor visitor1;
        Visitor visitor2;
        Visitor visitor3;

        GroupVisitor group1;
        GroupVisitor group2;

        CheckPoint check1 = new CheckPoint { Id = 1, Name = "check1", Active = true };
        CheckPoint check2 = new CheckPoint { Id = 2, Name = "check2" };
        CheckPoint check3 = new CheckPoint { Id = 3, Name = "check3", Active = true };
        CheckPoint check4 = new CheckPoint { Id = 4, Name = "check4", Active = true };

        List<Visitor> listOfVisitorsForVisitorService;

        List<GroupVisitor> listOfGroupsForGroupService;
        List<Visitor> listOfVisitorsForGroupService;

        private void ListForVisitorService()
        {
            visitor1 = new Visitor
            {
                Id = 1,
                Surname = "surname1",
                BithDate = new DateTime(1987, 07, 01),
                Nationality = nat1,
                Gender = gender1,
                DateInSystem = new DateTime(2018, 07, 01),
                UserInSystem = "Test"
            };

            visitor2 = new Visitor
            {
                Id = 2,
                Surname = "surname2",
                Name = "Name2",
                Gender = gender2,
                BithDate = new DateTime(2005, 1, 1),
                Nationality = nat2,
                DateInSystem = new DateTime(2018, 07, 02),
                UserInSystem = "Test"
            };

            group1 = new GroupVisitor
            {
                Id = 1,
                Group = false,
                PlaceOfRecidense = "place 1",
                CheckPoint = check1,
                DaysOfStay = 4,
                DateArrival = new DateTime(2018, 6, 1),
                Visitors = new List<Visitor> { visitor1 },

                DateInSystem = new DateTime(2018, 07, 02),
                TranscriptUser = "transcript User",
                UserInSystem = "Test1"
            };

            visitor1.Group = group1;
            visitor2.Group = group1;

            listOfVisitorsForVisitorService = new List<Visitor> { visitor1, visitor2 };
        }

        private void ListForGroupVisitorService()
        {           
            visitor1 = new Visitor
            {
                Id = 1,
                Group = group1,
                Surname = "surname1",
                BithDate = new DateTime(1987, 07, 01),           
                Nationality = nat1,
                Gender = gender1,
                DateInSystem = new DateTime(2018, 07, 01),
                UserInSystem = "Test1",
                Arrived = true
            };

            visitor2 = new Visitor
            {
                Id = 2,
                Group = group2,
                Surname = "surname2",
                Name = "Name2",
                Gender = gender2,
                BithDate = new DateTime(2005, 1, 1),
                Nationality = nat2,
                DateInSystem = new DateTime(2018, 07, 02),
                UserInSystem = "Test1",
                Arrived = true
            };

            visitor3 = new Visitor
            {
                Id = 3,
                Group = group2,
                Surname = "surname3",
                Name = "Name3",
                Gender = gender1,
                BithDate = new DateTime(2000, 1, 1),
                Nationality = nat2,
                DateInSystem = new DateTime(2017, 01, 01),
                UserInSystem = "Test2"
            };

            group1 = new GroupVisitor
            {
                Id = 1,
                Group = false,
                PlaceOfRecidense = "place 1",
                CheckPoint = check1,
                DateArrival = new DateTime(2018, 6, 1),
                Visitors = new List<Visitor> { visitor1 },

                DateInSystem = new DateTime(2018, 07, 02),
                TranscriptUser = "transcript User",
                UserInSystem = "Test1"
            };

            group2 = new GroupVisitor
            {
                Id = 2,
                Group = true,
                PlaceOfRecidense = "place 2",
                CheckPoint = check2,
                DateArrival = new DateTime(2018, 7, 30),
                Visitors = new List<Visitor> { visitor2, visitor3 },

                DateInSystem = new DateTime(2018, 05, 02),
                TranscriptUser = "transcript test",
                UserInSystem = "Test1"
            };

            visitor1.Group = group1;
            visitor2.Group = group2;
            visitor3.Group = group2;

            listOfVisitorsForGroupService = new List<Visitor> { visitor1, visitor2, visitor3 };
            listOfGroupsForGroupService = new List<GroupVisitor> { group1, group2 };
        }

        public CreateTestRepositories()
        {
            user1.OperatorProfile = profile1;
            user2.OperatorProfile = profile2;
            user3.OperatorProfile = profile3;
            user4.OperatorProfile = profile4;
        }

        private IRepository<OperatorProfile, string> CreateOperatorManager()
        {
            List<OperatorProfile> list = new List<OperatorProfile> { profile1, profile2, profile3, profile4 };
            Mock<IRepository<OperatorProfile, string>> operatorMng = new Mock<IRepository<OperatorProfile, string>>();

            operatorMng.Setup(m => m.Delete(It.IsAny<string>())).Returns<string>(id =>
            {
                var profile = list.Where(p => p.Id == id).FirstOrDefault();
                list.Remove(profile); return profile;
            });

            operatorMng.Setup(m => m.GetById(It.IsAny<string>())).Returns<string>(id => { return list.Where(p => p.Id == id).FirstOrDefault(); });
            operatorMng.Setup(m => m.GetAll()).Returns(() => list);

            return operatorMng.Object;
        }

        private BezvizUserManager CreateUserManager()
        {
            List<BezvizUser> userList = new List<BezvizUser> { user1, user2, user3, user4 };
            List<OperatorProfile> profiles = new List<OperatorProfile> { profile1, profile2, profile3, profile4 };

            Mock<IUserStore<BezvizUser>> userStore = new Mock<IUserStore<BezvizUser>>();
            return new UserManagerTest(userList, profiles, userStore.Object);
        }

        private BezvizRoleManager CreateRoleManager()
        {
            List<BezvizRole> list = new List<BezvizRole> { };
            Mock<IRoleStore<BezvizRole, string>> roleStore = new Mock<IRoleStore<BezvizRole, string>>();

            roleStore.Setup(m => m.FindByNameAsync(It.IsAny<string>())).Returns<string>(id =>
                                                                            Task<BezvizRole>.FromResult<BezvizRole>(
                                                                            list.Where(u => u.Name == id).FirstOrDefault()));

            roleStore.Setup(m => m.CreateAsync(It.IsAny<BezvizRole>())).Returns<BezvizRole>(role => Task.FromResult(Task.Run(() => list.Add(role))));

            return new BezvizRoleManager(roleStore.Object);
        }


        private IRepository<CheckPoint, int> CreateCheckPointManager()
        {
            List<CheckPoint> list = new List<CheckPoint> { check1, check2, check3, check4 };
            Mock<IRepository<CheckPoint, int>> mock = new Mock<IRepository<CheckPoint, int>>();
            mock.Setup(m => m.GetAll()).Returns(list);
            return mock.Object;
        }

        private IRepository<Visitor, int> CreateVisitorManager()
        {
            ListForVisitorService();

            Mock<IRepository<Visitor, int>> mockVisitors = new Mock<IRepository<Visitor, int>>();
            mockVisitors.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => listOfVisitorsForVisitorService.Where(v => v.Id == id).FirstOrDefault());
            mockVisitors.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                                        Task<Visitor>.FromResult<Visitor>(
                                                                                        listOfVisitorsForVisitorService.Where(v => v.Id == id).FirstOrDefault()));

            mockVisitors.Setup(m => m.GetAll()).Returns(listOfVisitorsForVisitorService);
            mockVisitors.Setup(m => m.Create(It.IsAny<Visitor>())).Returns<Visitor>(v => { listOfVisitorsForVisitorService.Add(v); return v; });
            mockVisitors.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id =>
            {
                var item = listOfVisitorsForVisitorService.Where(i => i.Id == id).FirstOrDefault();
                var result = listOfVisitorsForVisitorService.Remove(item);
                return null;
            });
            mockVisitors.Setup(m => m.Update(It.IsAny<Visitor>())).Returns<Visitor>(v =>
            {
                var item = listOfVisitorsForVisitorService.Where(i => i.Id == v.Id).FirstOrDefault();
                listOfVisitorsForVisitorService.Remove(item);
                listOfVisitorsForVisitorService.Add(v);
                return v;
            });

            return mockVisitors.Object;
        }

        private IRepository<GroupVisitor, int> CreateGroupVisitorManager()
        {
            ListForGroupVisitorService();

            Mock<IRepository<GroupVisitor, int>> mock = new Mock<IRepository<GroupVisitor, int>>();
            mock.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => listOfGroupsForGroupService.Where(v => v.Id == id).FirstOrDefault());
            mock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                               Task<GroupVisitor>.FromResult<GroupVisitor>(
                                                                               listOfGroupsForGroupService.Where(v => v.Id == id).FirstOrDefault()));

            mock.Setup(m => m.GetAll()).Returns(listOfGroupsForGroupService);
            mock.Setup(m => m.Create(It.IsAny<GroupVisitor>())).Returns<GroupVisitor>(v => { listOfGroupsForGroupService.Add(v); listOfVisitorsForGroupService.AddRange(v.Visitors); return v; });
            mock.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id =>
            {
                var deleteGroup = listOfGroupsForGroupService.SingleOrDefault(g => g.Id == id);
                foreach (var item in deleteGroup.Visitors)
                    listOfVisitorsForGroupService.Remove(item);
                listOfGroupsForGroupService.Remove(deleteGroup);
                return deleteGroup;
            });
            mock.Setup(m => m.Update(It.IsAny<GroupVisitor>())).Returns<GroupVisitor>(v =>
            {
                var deleteGroup = listOfGroupsForGroupService.SingleOrDefault(g => g.Id == v.Id);
                foreach (var item in deleteGroup.Visitors)
                    listOfVisitorsForGroupService.Remove(item);
                listOfGroupsForGroupService.Remove(deleteGroup);

                listOfGroupsForGroupService.Add(v);
                foreach (var item in v.Visitors)
                    listOfVisitorsForGroupService.Add(item);
                return v;
            });

            return mock.Object;
        }

        private IRepository<Nationality, int> CreateNationalitiesManager()
        {
            List<Nationality> list = new List<Nationality> { nat1, nat2, nat3 };
            Mock<IRepository<Nationality, int>> mockNat = new Mock<IRepository<Nationality, int>>();
            mockNat.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => list.Where(v => v.Id == id).FirstOrDefault());
            mockNat.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                                  Task<Nationality>.FromResult<Nationality>(
                                                                                  list.Where(v => v.Id == id).FirstOrDefault()));
            mockNat.Setup(m => m.GetAll()).Returns(list);
            return mockNat.Object;
        }

        private IRepository<Gender, int> CreateGenderManager()
        {
            List<Gender> list = new List<Gender> { gender1, gender2 };
            Mock<IRepository<Gender, int>> mockNat = new Mock<IRepository<Gender, int>>();
            mockNat.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => list.Where(v => v.Id == id).FirstOrDefault());
            mockNat.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                                  Task<Gender>.FromResult<Gender>(
                                                                                  list.Where(v => v.Id == id).FirstOrDefault()));
            mockNat.Setup(m => m.GetAll()).Returns(list);
            return mockNat.Object;
        }

        public IUnitOfWork CreateIoWManager()
        {
            Mock<IUnitOfWork> mockDB = new Mock<IUnitOfWork>();

            mockDB.Setup(m => m.UserManager).Returns(CreateUserManager());
            mockDB.Setup(m => m.OperatorManager).Returns(CreateOperatorManager());
            mockDB.Setup(m => m.RoleManager).Returns(CreateRoleManager());
            mockDB.Setup(m => m.VisitorManager).Returns(CreateVisitorManager());
            mockDB.Setup(m => m.GroupManager).Returns(CreateGroupVisitorManager());

            mockDB.Setup(m => m.Nationalities).Returns(CreateNationalitiesManager());
            mockDB.Setup(m => m.CheckPoints).Returns(CreateCheckPointManager());
            mockDB.Setup(m => m.Genders).Returns(CreateGenderManager());

            return mockDB.Object;
        }
    }
}
