using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Controllers;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Services;
using BezvizSystem.Web.Models.Visitor;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Repositories;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;

namespace BezvizSystem.Web.Tests
{
    [TestClass]
    public class IntegratedTest
    {
        const string CONNECT = "BezvizConnection";

        IServiceCreator serviceCreator = new ServiceCreator();
        IUnitOfWork database;

        IRepository<XMLDispatch, int> xmlDispatcher;

        IUserService userService;
        IService<VisitorDTO> visitorService;
        IService<GroupVisitorDTO> groupService;
        IService<AnketaDTO> anketaService;
        IDictionaryService<CheckPointDTO> checkPoint;
        IDictionaryService<NationalityDTO> nationalities;
        IDictionaryService<GenderDTO> genders;
        IXMLDispatcher xmlDispatcherService;

        VisitorController visitorController;
        GroupController groupController;
        AnketaController anketaController;
        AccountController accountController;

        public IntegratedTest()
        {
            database = new IdentityUnitOfWork(CONNECT);
            xmlDispatcher = database.XMLDispatchManager;

            xmlDispatcherService = new XMLDispatcher(database);
            visitorService = serviceCreator.CreateVisitorService(CONNECT);
            //groupService = serviceCreator.CreateGroupService(CONNECT);
            groupService = new GroupService(database);
            checkPoint = serviceCreator.CreateCheckPointService(CONNECT);
            nationalities = serviceCreator.CreateNationalityService(CONNECT);
            genders = serviceCreator.CreateGenderService(CONNECT);
            userService = serviceCreator.CreateUserService(CONNECT);

            accountController = new AccountController(userService);
            visitorController = new VisitorController(groupService, checkPoint, nationalities, genders);
            groupController = new GroupController(groupService, checkPoint, nationalities, genders);
            anketaController = new AnketaController(anketaService, groupService, checkPoint, nationalities, genders);
        }

        InfoVisitorModel visitor1 = new InfoVisitorModel
        {
            Surname = "surname test",
            Name = "name test",
            SerialAndNumber = "test test",
            Gender = "Мужчина",
            BithDate = new DateTime(1987, 5, 1),
            Nationality = "Польша",
            DateInSystem = DateTime.Now,
            UserInSystem = "Admin"
        };

        CreateVisitorModel createVisitor = new CreateVisitorModel
        {
            DateArrival = DateTime.Now,
            DateDeparture = DateTime.Now,
            DaysOfStay = 1,
            CheckPoint = "Брест (Тересполь)",
            PlaceOfRecidense = "place TEST",
            ProgramOfTravel = "program of travel",
            TimeOfWork = "time work",
            SiteOfOperator = "site",
            TelNumber = "tel",
            Email = "egorik-555@yandex.ru",
            DateInSystem = DateTime.Now,
            UserInSystem = "Admin"
        };

        EditInfoVisitorModel visitorNew = new EditInfoVisitorModel
        {
            Surname = "surname test new",
            Name = "name test new",
            SerialAndNumber = "test test new",
            Gender = "Мужчина",
            BithDate = new DateTime(1988, 5, 1),
            Nationality = "Польша",        
            Arrived = true,
            DateInSystem = DateTime.Now,
            UserInSystem = "Admin",
            UserEdit = "Admin",
            DateEdit = new DateTime(2018, 01, 01)
        };

        EditVisitorModel groupVisitorNew = new EditVisitorModel
        {
            DateArrival = DateTime.Now,
            DateDeparture = DateTime.Now,
            DaysOfStay = 2,
            CheckPoint = "Брест (Тересполь)",
            PlaceOfRecidense = "place TEST new",
            ProgramOfTravel = "program of travel new",
            TimeOfWork = "time work new",
            SiteOfOperator = "site new",
            TelNumber = "tel new",
            Email = "egorik-555@yandex.ru",
            TranscriptUser = "Брестский облисполком",
            DateInSystem = DateTime.Now,
            UserInSystem = "Admin",
            UserEdit = "Admin",
            DateEdit = new DateTime(2018, 01, 01)
        };


        [TestMethod]
        public async Task Create_Group_Of_One_Visitor()
        {
            await accountController.SetInitDataAsync();
            createVisitor.Info = visitor1;
            var result = (await visitorController.Create(createVisitor)) as RedirectToRouteResult;

            Assert.IsNotNull(result);

            var group = groupService.GetAll().LastOrDefault(g => g.PlaceOfRecidense.Contains("TEST"));

            //group
            Assert.IsNotNull(group);
            Assert.AreEqual(DateTime.Now.Date, group.DateArrival.Value.Date);
            Assert.AreEqual(DateTime.Now.Date, group.DateDeparture.Value.Date);
            Assert.AreEqual(1, group.DaysOfStay);
            Assert.AreEqual("Брест (Тересполь)", group.CheckPoint);
            Assert.AreEqual("place TEST", group.PlaceOfRecidense);
            Assert.AreEqual("program of travel", group.ProgramOfTravel);
            Assert.AreEqual("time work", group.TimeOfWork);
            Assert.AreEqual("site", group.SiteOfOperator);
            Assert.AreEqual("tel", group.TelNumber);
            Assert.AreEqual("egorik-555@yandex.ru", group.Email);
            Assert.AreEqual(false, group.ExtraSend);
            Assert.AreEqual(DateTime.Now.Date, group.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", group.UserInSystem);
            Assert.AreEqual("Брестский облисполком", group.TranscriptUser);

            var visitor = group.Visitors.FirstOrDefault();

            //visitors
            Assert.AreEqual(1, group.Visitors.Count);
            Assert.AreEqual("surname test", visitor.Surname);
            Assert.AreEqual("name test", visitor.Name);
            Assert.AreEqual("test test", visitor.SerialAndNumber);
            Assert.AreEqual("Мужчина", visitor.Gender);
            Assert.AreEqual(new DateTime(1987, 5, 1).Date, visitor.BithDate);
            Assert.AreEqual("Польша", visitor.Nationality);
            Assert.AreEqual(false, visitor.Arrived);
            Assert.AreEqual(DateTime.Now.Date, visitor.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", visitor.UserInSystem);

            //XMLDispatcher
            var dispatch = database.XMLDispatchManager.GetById(visitor.Id);

            Assert.AreEqual(Status.New, dispatch.Status);
            Assert.AreEqual(Operation.Add, dispatch.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateInSystem.Value.Date);
        }

        [TestMethod]
        public async Task Edit_Group_Of_One_Visitor()
        {
            var group = groupService.GetAll().LastOrDefault();
            groupVisitorNew.Id = group.Id;
            visitorNew.Id = group.Visitors.LastOrDefault().Id;
            groupVisitorNew.Info = visitorNew;
            
            if (group == null) return;

            var result = (await anketaController.EditVisitor(groupVisitorNew, "Extra")) as RedirectToRouteResult;

            group = groupService.GetById(group.Id);

            Assert.IsNotNull(result);
            //group
            Assert.IsNotNull(group);
            Assert.AreEqual(DateTime.Now.Date, group.DateArrival.Value.Date);
            Assert.AreEqual(DateTime.Now.Date, group.DateDeparture.Value.Date);
            Assert.AreEqual(2, group.DaysOfStay);
            Assert.AreEqual("Брест (Тересполь)", group.CheckPoint);
            Assert.AreEqual("place TEST new", group.PlaceOfRecidense);
            Assert.AreEqual("program of travel new", group.ProgramOfTravel);
            Assert.AreEqual("time work new", group.TimeOfWork);
            Assert.AreEqual("site new", group.SiteOfOperator);
            Assert.AreEqual("tel new", group.TelNumber);
            Assert.AreEqual("egorik-555@yandex.ru", group.Email);
            Assert.AreEqual(true, group.ExtraSend);
            Assert.AreEqual(DateTime.Now.Date, group.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", group.UserInSystem);
            Assert.AreEqual("Брестский облисполком", group.TranscriptUser);
            Assert.AreEqual(new DateTime(2018, 1, 1).Date, group.DateEdit.Value.Date);
            Assert.AreEqual("Admin", group.UserInSystem);

            var visitor = group.Visitors.FirstOrDefault();

            //visitors
            Assert.AreEqual(1, group.Visitors.Count);
            Assert.AreEqual("surname test new", visitor.Surname);
            Assert.AreEqual("name test new", visitor.Name);
            Assert.AreEqual("test test new", visitor.SerialAndNumber);
            Assert.AreEqual("Мужчина", visitor.Gender);
            Assert.AreEqual(new DateTime(1988, 5, 1).Date, visitor.BithDate.Value.Date);
            Assert.AreEqual("Польша", visitor.Nationality);
            Assert.AreEqual(true, visitor.Arrived);
            Assert.AreEqual(DateTime.Now.Date, visitor.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", visitor.UserInSystem);
            Assert.AreEqual(new DateTime(2018, 1, 1).Date, visitor.DateEdit.Value.Date);
            Assert.AreEqual("Admin", visitor.UserInSystem);

            //XMLDispatcher
            var dispatch = database.XMLDispatchManager.GetById(visitor.Id);

            Assert.AreEqual(DAL.Helpers.Status.New, dispatch.Status);
            Assert.AreEqual(DAL.Helpers.Operation.Add, dispatch.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateInSystem.Value.Date);
        }


        private async Task<GroupVisitorDTO> CreateGroup()
        {
            await accountController.SetInitDataAsync();
            createVisitor.Info = visitor1;
            var result = (await visitorController.Create(createVisitor)) as RedirectToRouteResult;

            if (result == null) return null;

            var group = groupService.GetAll().LastOrDefault();
                   
            return group;
        }

        private async Task SendVisitor(VisitorDTO visitor)
        {
            Visitor v = new Visitor { Id = visitor.Id};
            await xmlDispatcherService.Send(v);           
        }

        private async Task<GroupVisitorDTO> EditGroup(GroupVisitorDTO group)
        {
            visitorNew.Id = group.Visitors.FirstOrDefault().Id;
            groupVisitorNew.Info = visitorNew;
            groupVisitorNew.Id = group.Id;
            var result = (await anketaController.EditVisitor(groupVisitorNew, "")) as RedirectToRouteResult;

            if (result == null) return null;
            var groupResult = groupService.GetById(group.Id);

            return groupResult;
        }

        [TestMethod]
        public async Task Edit_Group_Of_One_Send_Visitor()
        {
            var group = await CreateGroup();
            await SendVisitor(group.Visitors.FirstOrDefault());
            group = await EditGroup(group);

            Assert.IsNotNull(group);
                  
            //XMLDispatcher
            var dispatch = database.XMLDispatchManager.GetById(group.Visitors.FirstOrDefault().Id);

            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Edit, dispatch.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateInSystem.Value.Date);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateEdit.Value.Date);
        }
    }
}
