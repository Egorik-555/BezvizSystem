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
using BezvizSystem.Web.Models.Anketa;
using BezvizSystem.Web.Models.Group;
using System.Collections.Generic;

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

        InfoVisitorModel visitor2 = new InfoVisitorModel
        {
            Surname = "surname2 test",
            Name = "name2 test",
            SerialAndNumber = "test2 test",
            Gender = "Женщина",
            BithDate = new DateTime(1950, 5, 1),
            Nationality = "Германия",
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

        CreateGroupModel createGroup = new CreateGroupModel
        {
            DateArrival = DateTime.Now,
            DateDeparture = DateTime.Now,
            DaysOfStay = 5,
            CheckPoint = "Аэропорт Брест",
            PlaceOfRecidense = "place TEST",
            ProgramOfTravel = "program of travel",
            OrganizeForm = "orginize form",
            Name = "name",
            NumberOfContract = "number of contract",
            DateOfContract = new DateTime(2018, 6, 1),
            OtherInfo = "Other info",
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

        EditInfoVisitorModel visitorNew2 = new EditInfoVisitorModel
        {
            Surname = "surname2 test new",
            Name = "name2 test new",
            SerialAndNumber = "test2 test new",
            Gender = "Мужчина",
            BithDate = new DateTime(1951, 5, 1),
            Nationality = "Польша",
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



        private async Task<GroupVisitorDTO> CreateGroup(CreateVisitorModel group, InfoVisitorModel visitor)
        {
            await accountController.SetInitDataAsync();
            group.Info = visitor;
            var result = (await visitorController.Create(group)) as RedirectToRouteResult;

            if (result == null) return null;

            var groupResult = groupService.GetAll().LastOrDefault();
            return groupResult;
        }

        private async Task<GroupVisitorDTO> CreateGroup(CreateGroupModel group, params InfoVisitorModel[] visitor)
        {
            await accountController.SetInitDataAsync();
            group.Infoes = visitor;
            var result = (await groupController.Create(group)) as RedirectToRouteResult;

            if (result == null) return null;

            var groupResult = groupService.GetAll().LastOrDefault();
            return groupResult;
        }

        private async Task<GroupVisitorDTO> EditGroup(EditVisitorModel group, EditInfoVisitorModel visitor)
        {
            var findGroup = groupService.GetAll().LastOrDefault();

            visitor.Id = findGroup.Visitors.FirstOrDefault().Id;
            group.Info = visitor;
            group.Id = findGroup.Id;

            var result = (await anketaController.EditVisitor(group, "Extra")) as RedirectToRouteResult;

            if (result == null) return null;
            var groupResult = groupService.GetById(group.Id);

            return groupResult;
        }

        private async Task<RedirectToRouteResult> DeleteGroup(int id)
        {
            ViewAnketaModel model = new ViewAnketaModel{ Id = id};
            var result = (await anketaController.Delete(model)) as RedirectToRouteResult;
            return result;
        }


        private async Task SendVisitor(VisitorDTO visitor)
        {
            Visitor v = new Visitor { Id = visitor.Id };
            await xmlDispatcherService.Send(v);
        }

        private async Task RecdVisitor(VisitorDTO visitor)
        {
            Visitor v = new Visitor { Id = visitor.Id };
            await xmlDispatcherService.Recd(v);
        }


        [TestMethod]
        public async Task Create_Group_Of_One_Visitor()
        {
            var group = await CreateGroup(createVisitor, visitor1);
           
            Assert.IsNotNull(group);

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
            var group = await CreateGroup(createVisitor, visitor1);
            Assert.IsNotNull(group);
            group = await EditGroup(groupVisitorNew, visitorNew);
            Assert.IsNotNull(group);

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
            Assert.AreEqual(false, group.Group);
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

            Assert.AreEqual(Status.New, dispatch.Status);
            Assert.AreEqual(Operation.Add, dispatch.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateInSystem.Value.Date);
        }
     
        [TestMethod]
        public async Task Edit_Group_Of_One_Send_Visitor()
        {                    
            var group = await CreateGroup(createVisitor, visitor1);
            Assert.IsNotNull(group);
            await SendVisitor(group.Visitors.FirstOrDefault());
            group = await EditGroup(groupVisitorNew, visitorNew);
            Assert.IsNotNull(group);
                  
            //XMLDispatcher
            var dispatch = database.XMLDispatchManager.GetById(group.Visitors.FirstOrDefault().Id);

            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Edit, dispatch.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateInSystem.Value.Date);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateEdit.Value.Date);
        }

        [TestMethod]
        public async Task Edit_Group_Of_One_Recd_Visitor()
        {
            var group = await CreateGroup(createVisitor, visitor1);
            Assert.IsNotNull(group);
            await RecdVisitor(group.Visitors.FirstOrDefault());
            group = await EditGroup(groupVisitorNew, visitorNew);
            Assert.IsNotNull(group);

            //XMLDispatcher
            var dispatch = database.XMLDispatchManager.GetById(group.Visitors.FirstOrDefault().Id);

            Assert.AreEqual(Status.Recd, dispatch.Status);
            Assert.AreEqual(Operation.Edit, dispatch.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateInSystem.Value.Date);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateEdit.Value.Date);
        }

        [TestMethod]
        public async Task Delete_Group_Of_One_New_Visitor()
        {
            var group = await CreateGroup(createVisitor, visitor1);
            Assert.IsNotNull(group);         
            var result = await DeleteGroup(group.Id);
            Assert.IsNotNull(result);

            var findGroup = await groupService.GetByIdAsync(group.Id);
            //group
            Assert.IsNull(findGroup);

            var visitor = await visitorService.GetByIdAsync(group.Visitors.FirstOrDefault().Id);
            //visitors
            Assert.IsNull(visitor);

            //XMLDispatcher
            var dispatch = database.XMLDispatchManager.GetById(group.Visitors.FirstOrDefault().Id);

            Assert.IsNull(dispatch);       
        }

        [TestMethod]
        public async Task Delete_Group_Of_One_Send_Visitor()
        {
            var group = await CreateGroup(createVisitor, visitor1);
            Assert.IsNotNull(group);
            await SendVisitor(group.Visitors.FirstOrDefault());
            var result = await DeleteGroup(group.Id);
            Assert.IsNotNull(result);

            //XMLDispatcher
            var dispatch = database.XMLDispatchManager.GetById(group.Visitors.FirstOrDefault().Id);

            Assert.AreEqual(Status.Send, dispatch.Status);
            Assert.AreEqual(Operation.Remove, dispatch.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateInSystem.Value.Date);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateEdit.Value.Date);
        }

        [TestMethod]
        public async Task Delete_Group_Of_One_Recd_Visitor()
        {
            var group = await CreateGroup(createVisitor, visitor1);
            Assert.IsNotNull(group);
            await RecdVisitor(group.Visitors.FirstOrDefault());
            var result = await DeleteGroup(group.Id);
            Assert.IsNotNull(result);

            //XMLDispatcher
            var dispatch = database.XMLDispatchManager.GetById(group.Visitors.FirstOrDefault().Id);

            Assert.AreEqual(Status.Recd, dispatch.Status);
            Assert.AreEqual(Operation.Remove, dispatch.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateInSystem.Value.Date);
            Assert.AreEqual(DateTime.Now.Date, dispatch.DateEdit.Value.Date);
        }





        [TestMethod]
        public async Task Create_Group_Of_Many_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);
            
            Assert.IsNotNull(group);

            //group
            Assert.IsNotNull(group);
            Assert.AreEqual(DateTime.Now.Date, group.DateArrival.Value.Date);
            Assert.AreEqual(DateTime.Now.Date, group.DateDeparture.Value.Date);
            Assert.AreEqual(5, group.DaysOfStay);
            Assert.AreEqual("Аэропорт Брест", group.CheckPoint);
            Assert.AreEqual("place TEST", group.PlaceOfRecidense);
            Assert.AreEqual("program of travel", group.ProgramOfTravel);
            Assert.AreEqual("orginize form", group.OrganizeForm);
            Assert.AreEqual("name", group.Name);
            Assert.AreEqual("number of contract", group.NumberOfContract);
            Assert.AreEqual(false, group.ExtraSend);
            Assert.AreEqual(true, group.Group);
            Assert.AreEqual(new DateTime(2018, 6, 1), group.DateOfContract);
            Assert.AreEqual("Other info", group.OtherInfo);
            Assert.AreEqual(DateTime.Now.Date, group.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", group.UserInSystem);
            Assert.AreEqual("Брестский облисполком", group.TranscriptUser);

            var visitor = group.Visitors.FirstOrDefault(v => v.Surname == "surname test");

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
    }
}
