﻿using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Identity;
using Moq;

namespace BezvizSystem.BLL.Tests
{
    class TestUnitOfWork : IUnitOfWork
    {     
        Mock<IRepository<Nationality, int>> nationalityManager;
        Mock<IRepository<CheckPoint, int>> checkPointManager;
        Mock<IRepository<GroupVisitor, int>> groupManger;

        List<Nationality> nationalities = new List<Nationality>
        {
            new Nationality{ Code = 1, Name = "nat1", ShortName = "n1", Active = true},
            new Nationality{ Code = 2, Name = "nat2", ShortName = "n2", Active = true},
            new Nationality{ Code = 3, Name = "nat3", ShortName = "n3", Active = true},
            new Nationality{ Code = 3, Name = "nat4", ShortName = "n4" }
        };

        List<CheckPoint> checkPoints = new List<CheckPoint>
        {
            new CheckPoint{ Name = "checkPoint1", Active = true},
            new CheckPoint{ Name = "checkPoint2", Active = true},
            new CheckPoint{ Name = "checkPoint3"},
            new CheckPoint{ Name = "checkPoint4"}
        };

        List<GroupVisitor> groups = new List<GroupVisitor>
        {
            new GroupVisitor{ Visitors = new List<Visitor>{ new Visitor { Surname = "surname1_group1", Arrived = true}, new Visitor { Surname = "surname2_group1" } }, Group = true,  DateArrival = DateTime.Parse("25.06.2018") },
            new GroupVisitor{ Visitors = new List<Visitor>{ new Visitor { Surname = "surname1_visitor1", Arrived = true} },  DateArrival = DateTime.Parse("26.06.2018") },
            new GroupVisitor{ Visitors = new List<Visitor>{ new Visitor { Surname = "surname2_visitor2"} },  DateArrival = DateTime.Parse("30.06.2018") },
            new GroupVisitor{ Visitors = new List<Visitor>{ new Visitor { Surname = "surname1_group2"}, new Visitor { Surname = "surname2_group2"} },  Group = true,  DateArrival = DateTime.Parse("01.07.2018") }
        };

        public TestUnitOfWork()
        {         
            nationalityManager = new Mock<IRepository<Nationality, int>>();
            nationalityManager.Setup(m => m.GetAll()).Returns(nationalities);

            checkPointManager = new Mock<IRepository<CheckPoint, int>>();
            checkPointManager.Setup(m => m.GetAll()).Returns(checkPoints);

            groupManger = new Mock<IRepository<GroupVisitor, int>>();
            groupManger.Setup(m => m.GetAll()).Returns(groups);
        }

        public BezvizUserManager UserManager => throw new NotImplementedException();
        public BezvizRoleManager RoleManager => throw new NotImplementedException();

        public IRepository<OperatorProfile, string> OperatorManager => throw new NotImplementedException();
        public IRepository<Visitor, int> VisitorManager => throw new NotImplementedException();
        public IRepository<GroupVisitor, int> GroupManager => groupManger.Object;
        public IRepository<XMLDispatch, int> XMLDispatchManager => throw new NotImplementedException();

        //Dictionaries
        public IRepository<Nationality, int> Nationalities => nationalityManager.Object;
        public IRepository<CheckPoint, int> CheckPoints => checkPointManager.Object;
        public IRepository<Gender, int> Genders => throw new NotImplementedException();
     
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
