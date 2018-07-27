using BezvizSystem.DAL;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Entities.Log;
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
        BezvizUser user1 = new BezvizUser { Id = "aaa", UserName = "Test1", OperatorProfile = new OperatorProfile { UNP = "UnpTest1", OKPO = "OKPO1", Role = "Operator" } };
        BezvizUser user2 = new BezvizUser { Id = "bbb", UserName = "Test2", OperatorProfile = new OperatorProfile { UNP = "UnpTest2", OKPO = "OKPO2", Role = "Admin" } };
        BezvizUser user3 = new BezvizUser { Id = "ccc", UserName = "Test3", OperatorProfile = new OperatorProfile { UNP = "UnpTest3", OKPO = "OKPO3", Role = "Operator" } };
        BezvizUser user4 = new BezvizUser { Id = "ddd", UserName = "Admin", OperatorProfile = new OperatorProfile {Transcript = "AdminTran", UNP = "UnpAdmin", OKPO = "OKPO4", Role = "Operator" } };

        Nationality nat1 = new Nationality { Id = 1, Name = "nat1", ShortName = "n1", Active = true };
        Nationality nat2 = new Nationality { Id = 2, Name = "nat2", ShortName = "n2", Active = true };
        Nationality nat3 = new Nationality { Id = 3, Name = "nat3", ShortName = "n3", Active = true };

        Gender gender1 = new Gender { Id = 1, Code = 1, Name = "Мужчина", Active = true };
        Gender gender2 = new Gender { Id = 2, Code = 2, Name = "Женщина", Active = true };  

        Visitor visitor1;
        Visitor visitor2;
        Visitor visitor3;
        Visitor visitor4;
        Visitor visitor5;
        Visitor visitor6;
        Visitor visitor7;
        Visitor visitor8;

        GroupVisitor group1;
        GroupVisitor group2;
        GroupVisitor group3;
        GroupVisitor group4;
        GroupVisitor group5;
        GroupVisitor groupForVisitor;

        UserActivity activity1;
        UserActivity activity2;
        UserActivity activity3;
        UserActivity activity4;

        TypeOfOperation operation1 = new TypeOfOperation { Code = 1, Name = "Enter", Active = true };
        TypeOfOperation operation2 = new TypeOfOperation { Code = 2, Name = "Exit", Active = true };
        TypeOfOperation operation3 = new TypeOfOperation { Code = 3, Name = "ErrorEnter" };

        Status status1 = new Status { Code = 1, Name = "status1", Active = true };
        Status status2 = new Status { Code = 2, Name = "status2", Active = true };
        Status status3 = new Status { Code = 3, Name = "status3", Active = false };

        CheckPoint check1 = new CheckPoint { Id = 1, Name = "check1", Active = true};
        CheckPoint check2 = new CheckPoint { Id = 2, Name = "check2" };
        CheckPoint check3 = new CheckPoint { Id = 3, Name = "check3", Active = true };
        CheckPoint check4 = new CheckPoint { Id = 4, Name = "check4", Active = true };

        public CreateTestRepositories()
        {
            groupForVisitor = new GroupVisitor {DateArrival = new DateTime(2018,7,1), DaysOfStay = 3, CheckPoint = check1};

            GroupVisitor groupForVisitor1 = new GroupVisitor { DateArrival = new DateTime(2018, 7, 2), DaysOfStay = 3, CheckPoint = check2 };
            GroupVisitor groupForVisitor2 = new GroupVisitor { DateArrival = new DateTime(2018, 5, 1), DaysOfStay = 3, CheckPoint = check3 };

            visitor1 = new Visitor { Id = 1, Surname = "surname1", BithDate = new DateTime(1987, 07, 01),
                                     Nationality = nat1, Group = groupForVisitor, Status = status3, Gender = gender1 };
            visitor2 = new Visitor { Id = 2, Surname = "surname2", Gender = gender2, Name = "Name2", UserInSystem = "user", 
                                     BithDate = new DateTime(2005, 1, 1),
                                     Nationality = nat2, DateInSystem = new DateTime(2018, 07, 01),
                                     Group = groupForVisitor, Status = status2, Arrived = true };

            visitor3 = new Visitor { Id = 3, Surname = "surname3", Nationality = nat3, Group = groupForVisitor1, Status = status1, Arrived = true, Gender = gender1 };
            visitor4 = new Visitor { Id = 4, Surname = "surname4", Nationality = nat1, BithDate = new DateTime(1960, 2, 6),
                                     Group = groupForVisitor1, Status = status1, Arrived = true, Gender = gender2 };
            visitor5 = new Visitor { Id = 5, Surname = "surname5", Nationality = nat3, Group = groupForVisitor2, Status = status1, Gender = gender1 };
            visitor6 = new Visitor { Id = 6, Surname = "surname6", Nationality = nat2, Group = groupForVisitor2, Status = status2, Arrived = true, Gender = gender2};
            visitor7 = new Visitor { Id = 7, Surname = "surname7", Nationality = nat3, Group = groupForVisitor, Status = status2, Gender = gender1 };

            visitor8 = new Visitor { Id = 9,
                                     StatusOfOperation = StatusOfOperation.Add,
                                     Name = "Name", Surname = "surname",
                                     Nationality = nat3, Group = groupForVisitor,
                                     BithDate = new DateTime(1987,07,26),
                                     Gender = gender1,
                                     SerialAndNumber = "AB344",
                                     DocValid = null,                             
                                     Status = status1 };

            group1 = new GroupVisitor { Id = 1, CheckPoint = check1, PlaceOfRecidense = "place1", DateArrival = new DateTime(2018, 6, 1), Group = true,
                                        Visitors = new List<Visitor> { visitor1, visitor2 }, User = user4 };

            group2 = new GroupVisitor { Id = 2, CheckPoint = check2, PlaceOfRecidense = "place2", Group = true,
                                        Visitors = new List<Visitor> { visitor3, visitor4 }, DateArrival = new DateTime(2018, 07, 21),
                                        User = user4, UserInSystem = "Admin", DateInSystem = new DateTime(2018, 07, 01) };

            group3 = new GroupVisitor { Id = 3, CheckPoint = check3, PlaceOfRecidense = "place3", DateArrival = new DateTime(2018, 07, 26),
                                        Visitors = new List<Visitor> { visitor5, visitor6, visitor7 }, Group = true
            };
            group4 = new GroupVisitor { Id = 4, CheckPoint = check4, PlaceOfRecidense = "place4",
                                        Visitors = new List<Visitor> { visitor6 }, DateArrival = new DateTime(2018, 5, 1) };

            group5 = new GroupVisitor {
                Id = 5, CheckPoint = check2,
                PlaceOfRecidense = "place5",
                Visitors = new List<Visitor> { visitor7 },
                User = user4,
                UserInSystem = "Admin",
                DateInSystem = new DateTime(2018, 07, 01),
                DateArrival = new DateTime(2018, 07, 30)
            };

            activity1 = new UserActivity { Id = 1, Login = "login1", Ip = "Ip1", TimeActivity = DateTime.Now, Operation = operation1 };
            activity2 = new UserActivity { Id = 2, Login = "login2", Ip = "Ip2", TimeActivity = DateTime.Now, Operation = operation2 };
            activity3 = new UserActivity { Id = 3, Login = "login1", Ip = "Ip3", TimeActivity = DateTime.Now, Operation = operation3 };
            activity4 = new UserActivity { Id = 4, Login = "login3", Ip = "Ip4", TimeActivity = DateTime.Now, Operation = operation1 };
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
            List<Visitor> list = new List<Visitor> { visitor1, visitor2, visitor3, visitor4, visitor5, visitor6, visitor7, visitor8 };
            Mock<IRepository<Visitor, int>> mockVisitors = new Mock<IRepository<Visitor, int>>();
            mockVisitors.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => list.Where(v => v.Id == id).FirstOrDefault());
            mockVisitors.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                                        Task<Visitor>.FromResult<Visitor>(
                                                                                        list.Where(v => v.Id == id).FirstOrDefault()));

            mockVisitors.Setup(m => m.GetAll()).Returns(list);
            mockVisitors.Setup(m => m.Create(It.IsAny<Visitor>())).Returns<Visitor>(v => { list.Add(v); return v; });
            mockVisitors.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>(id =>
            {
                var item = list.Where(i => i.Id == id).FirstOrDefault();
                var result = list.Remove(item);
                return null;
            });
            mockVisitors.Setup(m => m.Update(It.IsAny<Visitor>())).Returns<Visitor>(v =>
            {
                var item = list.Where(i => i.Id == v.Id).FirstOrDefault();
                list.Remove(item);
                list.Add(v);
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

        private IRepository<TypeOfOperation, int> CreateTypeOfOperationManager()
        {
            List<TypeOfOperation> list = new List<TypeOfOperation> { operation1, operation2, operation3};
            Mock<IRepository<TypeOfOperation, int>> mockNat = new Mock<IRepository<TypeOfOperation, int>>();
            mockNat.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => list.Where(v => v.Id == id).FirstOrDefault());
            mockNat.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                                  Task<TypeOfOperation>.FromResult<TypeOfOperation>(
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

        private IRepository<UserActivity, int> CreateUserActivityManager()
        {
            List<UserActivity> list = new List<UserActivity> { activity1, activity2, activity3, activity4};
            Mock<IRepository<UserActivity, int>> mock = new Mock<IRepository<UserActivity, int>>();

            mock.Setup(m => m.GetById(It.IsAny<int>())).Returns<int>(id => list.Where(v => v.Id == id).FirstOrDefault());
            mock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns<int>(id =>
                                                                               Task<UserActivity>.FromResult<UserActivity>(
                                                                               list.Where(v => v.Id == id).FirstOrDefault()));

            mock.Setup(m => m.GetAll()).Returns(list);
            mock.Setup(m => m.Create(It.IsAny<UserActivity>())).Returns<UserActivity>(v => { list.Add(v); return v; });    

            return mock.Object;
        }

        public IUnitOfWork CreateIoWManager()
        {
            Mock<IUnitOfWork> mockDB = new Mock<IUnitOfWork>();

            mockDB.Setup(m => m.UserManager).Returns(CreateUserManager());
            mockDB.Setup(m => m.VisitorManager).Returns(CreateVisitorManager());
            mockDB.Setup(m => m.NationalityManager).Returns(CreateNationalitiesManager());
            mockDB.Setup(m => m.StatusManager).Returns(CreateStatusManager());
            mockDB.Setup(m => m.CheckPointManager).Returns(CreateCheckPointManager());
            mockDB.Setup(m => m.GroupManager).Returns(CreateGroupVisitorManager());
            mockDB.Setup(m => m.TypeOfOperations).Returns(CreateTypeOfOperationManager());
            mockDB.Setup(m => m.UserActivities).Returns(CreateUserActivityManager());
            mockDB.Setup(m => m.Genders).Returns(CreateGenderManager());

            return mockDB.Object;
        }
    }
}
