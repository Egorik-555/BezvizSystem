using BezvizSystem.DAL;
using BezvizSystem.DAL.Entities;
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
        BezvizUser user1 = new BezvizUser { Id = "aaa", UserName = "Test1", OperatorProfile = new OperatorProfile { UNP = "UnpTest1", OKPO = "OKPO1", Role = "Operator" } };
        BezvizUser user2 = new BezvizUser { Id = "bbb", UserName = "Test2", OperatorProfile = new OperatorProfile { UNP = "UnpTest2", OKPO = "OKPO2", Role = "Admin" } };
        BezvizUser user3 = new BezvizUser { Id = "ccc", UserName = "Test3", OperatorProfile = new OperatorProfile { UNP = "UnpTest3", OKPO = "OKPO3", Role = "Operator" } };
        BezvizUser user4 = new BezvizUser { Id = "ddd", UserName = "Admin", OperatorProfile = new OperatorProfile { UNP = "UnpAdmin", OKPO = "OKPO4", Role = "Operator" } };

        Nationality nat1 = new Nationality { Id = 1, Name = "nat1" };
        Nationality nat2 = new Nationality { Id = 2, Name = "nat2" };
        Nationality nat3 = new Nationality { Id = 3, Name = "nat3" };

        Visitor visitor1;
        Visitor visitor2;
        Visitor visitor3;
        Visitor visitor4;
        Visitor visitor5;
        Visitor visitor6;
        Visitor visitor7;

        GroupVisitor group1;
        GroupVisitor group2;
        GroupVisitor group3;
        GroupVisitor group4;
        GroupVisitor group5;

        Status status1 = new Status { Code = 1, Name = "status1" };
        Status status2 = new Status { Code = 2, Name = "status2" };
        Status status3 = new Status { Code = 3, Name = "status3" };

        CheckPoint check1 = new CheckPoint { Id = 1, Name = "check1"};
        CheckPoint check2 = new CheckPoint { Id = 2, Name = "check2" };
        CheckPoint check3 = new CheckPoint { Id = 3, Name = "check3" };
        CheckPoint check4 = new CheckPoint { Id = 4, Name = "check4" };

        public CreateTestRepositories()
        {
            visitor1 = new Visitor { Id = 1, Surname = "surname1", BithDate = DateTime.Now, Nationality = nat1};
            visitor2 = new Visitor { Id = 2, Surname = "surname2", Name = "Name2", UserInSystem = "user", Nationality = nat2 };
            visitor3 = new Visitor { Id = 3, Surname = "surname3", Nationality = nat3 };
            visitor4 = new Visitor { Id = 4, Surname = "surname4", Nationality = nat1 };
            visitor5 = new Visitor { Id = 5, Surname = "surname5", Nationality = nat3 };
            visitor6 = new Visitor { Id = 6, Surname = "surname6", Nationality = nat2 };
            visitor7 = new Visitor { Id = 7, Surname = "surname7", Nationality = nat3 };

            group1 = new GroupVisitor { Id = 1, CheckPoint = check1, PlaceOfRecidense = "place1", Visitors = new List<Visitor> { visitor1, visitor2 }, User = user4 };
            group2 = new GroupVisitor { Id = 2, CheckPoint = check2, PlaceOfRecidense = "place2", Visitors = new List<Visitor> { visitor2, visitor3 }, User = user1, UserInSystem = "Test1" };
            group3 = new GroupVisitor { Id = 3, CheckPoint = check3, PlaceOfRecidense = "place3", Visitors = new List<Visitor> { visitor4, visitor5, visitor6, visitor7 } };
            group4 = new GroupVisitor { Id = 4, CheckPoint = check4, PlaceOfRecidense = "place4", Visitors = new List<Visitor> { visitor2 } };
            group5 = new GroupVisitor { Id = 5, CheckPoint = check2, PlaceOfRecidense = "place5", Visitors = new List<Visitor> { visitor3 } };
        }


        private BezvizUserManager CreateUserManager()
        {
            List<BezvizUser> userList = new List<BezvizUser> { user1, user2, user3, user4 };
            Mock<IUserStore<BezvizUser>> userStore = new Mock<IUserStore<BezvizUser>>();
            userStore.Setup(m => m.FindByNameAsync(It.IsAny<string>())).Returns<string>(id =>
                                                                            Task<BezvizUser>.FromResult<BezvizUser>(
                                                                                userList.Where(u => u.UserName == id).FirstOrDefault()));
            return new BezvizUserManager(userStore.Object);
        }

        private IRepository<Status, int> CreateStatusManager()
        {
            List<Status> listStatuses = new List<Status> { status1, status2, status3};
            Mock<IRepository<Status, int>> mockStatuses = new Mock<IRepository<Status, int>>();
            mockStatuses.Setup(m => m.GetAll()).Returns(listStatuses);
            return mockStatuses.Object;
        }

        private IRepository<CheckPoint, int> CreateCheckPointManager()
        {
            List<CheckPoint> list = new List<CheckPoint> { check1, check2, check3, check4};
            Mock<IRepository<CheckPoint, int>> mock = new Mock<IRepository<CheckPoint, int>>();
            mock.Setup(m => m.GetAll()).Returns(list);
            return mock.Object;
        }


        private IRepository<Visitor, int> CreateVisitorManager()
        {
            List<Visitor> list = new List<Visitor> { visitor1, visitor2, visitor3, visitor4 };
            Mock<IRepository<Visitor, int>> mockVisitors = new Mock<IRepository<Visitor, int>>();
            mockVisitors.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => list.Where(v => v.Id == id).FirstOrDefault());
            mockVisitors.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                                        Task<Visitor>.FromResult<Visitor>(
                                                                                        list.Where(v => v.Id == id).FirstOrDefault()));

            mockVisitors.Setup(m => m.GetAll()).Returns(list);
            mockVisitors.Setup(m => m.Create(It.IsAny<Visitor>())).Returns<Visitor>(v => { list.Add(v); return v; });
            mockVisitors.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id =>
            {
                list.RemoveAt(id - 1);
                return null;
            });
            mockVisitors.Setup(m => m.Update(It.IsAny<Visitor>())).Returns<Visitor>(v =>
            {
                list.RemoveAt(v.Id - 1);
                list.Insert(v.Id - 1, v);
                return v;
            });

            return mockVisitors.Object;
        }

        private IRepository<GroupVisitor, int> CreateGroupVisitorManager()
        {
            List<GroupVisitor> list = new List<GroupVisitor> { group1, group2, group3, group4, group5 };
            Mock<IRepository<GroupVisitor, int>> mock = new Mock<IRepository<GroupVisitor, int>>();
            mock.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => list.Where(v => v.Id == id).FirstOrDefault());
            mock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                               Task<GroupVisitor>.FromResult<GroupVisitor>(
                                                                               list.Where(v => v.Id == id).FirstOrDefault()));

            mock.Setup(m => m.GetAll()).Returns(list);
            mock.Setup(m => m.Create(It.IsAny<GroupVisitor>())).Returns<GroupVisitor>(v => { list.Add(v); return v; });
            mock.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id =>
            {
                list.RemoveAt(id - 1);
                return null;
            });
            mock.Setup(m => m.Update(It.IsAny<GroupVisitor>())).Returns<GroupVisitor>(v =>
            {
                list.RemoveAt(v.Id - 1);
                list.Insert(v.Id - 1, v);
                return v;
            });

            return mock.Object;
        }

        private IRepository<Nationality, int> CreateNationalitiesManager()
        {
            List<Nationality> list = new List<Nationality> { nat1, nat2, nat3};
            Mock<IRepository<Nationality, int>> mockNat = new Mock<IRepository<Nationality, int>>();
            mockNat.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => list.Where(v => v.Id == id).FirstOrDefault());
            mockNat.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                                  Task<Nationality>.FromResult<Nationality>(
                                                                                  list.Where(v => v.Id == id).FirstOrDefault()));
            mockNat.Setup(m => m.GetAll()).Returns(list);
            return mockNat.Object;
        }

        public IUnitOfWork CreateIoWManager()
        {
            BezvizUserManager userManager = CreateUserManager();
            IRepository<Visitor, int> visitorManager = CreateVisitorManager();
            Mock<IUnitOfWork> mockDB = new Mock<IUnitOfWork>();

            mockDB.Setup(m => m.UserManager).Returns(userManager);
            mockDB.Setup(m => m.VisitorManager).Returns(visitorManager);
            mockDB.Setup(m => m.NationalityManager).Returns(CreateNationalitiesManager());
            mockDB.Setup(m => m.StatusManager).Returns(CreateStatusManager());
            mockDB.Setup(m => m.CheckPointManager).Returns(CreateCheckPointManager());
            mockDB.Setup(m => m.GroupManager).Returns(CreateGroupVisitorManager());

            return mockDB.Object;
        }
    }
}
