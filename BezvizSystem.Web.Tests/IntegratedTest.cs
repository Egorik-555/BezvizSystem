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
using BezvizSystem.Web.Models.Operator;
using BezvizSystem.Web.Views.Helpers.Pagging;
using BezvizSystem.Web.Models;

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
        IReport reportService;

        VisitorController visitorController;
        GroupController groupController;
        AnketaController anketaController;
        AccountController accountController;
        OperatorController operatorController;
        ReportController reporterController;

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
            reportService = serviceCreator.CreateReport(CONNECT);

            accountController = new AccountController(userService);          
            visitorController = new VisitorController(groupService, checkPoint, nationalities, genders);
            groupController = new GroupController(groupService, checkPoint, nationalities, genders);
            anketaController = new AnketaController(anketaService, groupService, checkPoint, nationalities, genders);
            operatorController = new OperatorController(userService);
            reporterController = new ReportController(reportService);
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
            Group = true,
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

        EditInfoVisitorModel visitorNew3 = new EditInfoVisitorModel
        {
            Surname = "surname3 test",
            Name = "name3 test",
            SerialAndNumber = "test3 test",
            Gender = "Женщина",
            BithDate = new DateTime(1950, 5, 1),
            Nationality = "Германия",
            DateInSystem = DateTime.Now,
            UserInSystem = "Admin"
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

        EditGroupModel editGroupNew = new EditGroupModel
        {
            DateArrival = new DateTime(2018, 08, 31),
            DateDeparture = new DateTime(2018, 09, 01),
            DaysOfStay = 3,
            CheckPoint = "Брест (Тересполь)",
            PlaceOfRecidense = "place TEST new",
            ProgramOfTravel = "program of travel new",
            OrganizeForm = "orginize form",
            Name = "name",
            NumberOfContract = "number of contract",
            DateOfContract = new DateTime(2018, 6, 1),
            OtherInfo = "Other info",
            TranscriptUser = "Брестский облисполком",
            Group = true,
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

        private async Task<GroupVisitorDTO> EditGroup(EditGroupModel group, params EditInfoVisitorModel[] visitors)
        {
            var findGroup = groupService.GetAll().LastOrDefault();
            var oldVisitors = findGroup.Visitors.OrderBy(v => v.Id).ToList();

            int i = 0;
            foreach (var visitor in visitors)
            {
                if (i < oldVisitors.Count)
                    visitor.Id = oldVisitors[i++].Id;
                else break;
            }
            group.Infoes = visitors;
            group.Id = findGroup.Id;

            var result = (await anketaController.EditGroup(group, "Extra")) as RedirectToRouteResult;

            if (result == null) return null;
            var groupResult = groupService.GetById(group.Id);

            return groupResult;
        }

        private async Task<RedirectToRouteResult> DeleteGroup(int id)
        {
            ViewAnketaModel model = new ViewAnketaModel { Id = id };
            var result = (await anketaController.Delete(model)) as RedirectToRouteResult;
            return result;
        }

        private async Task SendVisitor(VisitorDTO visitor)
        {
            Visitor v = new Visitor { Id = visitor.Id };
            await xmlDispatcherService.Send(v);
        }

        private async Task SendVisitor(ICollection<VisitorDTO> visitors)
        {
            foreach (var visitor in visitors)
            {
                await SendVisitor(visitor);
            }
        }

        private async Task RecdVisitor(VisitorDTO visitor)
        {
            Visitor v = new Visitor { Id = visitor.Id };
            await xmlDispatcherService.Recd(v);
        }

        private async Task RecdVisitor(ICollection<VisitorDTO> visitors)
        {
            foreach (var visitor in visitors)
            {
                await RecdVisitor(visitor);
            }
        }

        CreateOperatorModel operatorModel = new CreateOperatorModel
        {
            Transcript = "transcript test",
            UNP = "123456789",
            OKPO = "12345",
            Password = "123456",
            Active = true,
            DateInSystem = DateTime.Now,
            UserInSystem = "Admin"
        };

        private async Task<UserDTO> CreateUser(CreateOperatorModel model)
        {
            var user = await userService.GetByNameAsync(model.UNP);
            if (user != null)
                await userService.Delete(user);

            var result = (await operatorController.Create(model)) as RedirectToRouteResult;

            if (result == null) return null;

            user = await userService.GetByNameAsync(model.UNP);
            return user;
        }

        private async Task<RedirectToRouteResult> DeleteUser(string id)
        {
            var user = await userService.GetByIdAsync(id);

            var result = (await operatorController.Delete(user.Id)) as ViewResult;

            if (result == null)
                return null;
            else
                return (await operatorController.Delete((DeleteOperatorModel)result.Model)) as RedirectToRouteResult;
        }

        private async Task<RedirectToRouteResult> EditUser(string id)
        {
            var user = await userService.GetByIdAsync(id);

            var result = (await operatorController.Edit(user.Id)) as ViewResult;

            if (result == null) return null;

            var model = result.Model as EditOperatorModel;
            model.Email = "email";
            model.EmailConfirmed = true;
            model.ProfileUserTranscript = "new Transcript";

            return (await operatorController.Edit(model)) as RedirectToRouteResult;
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

            var findVisitor1 = group.Visitors.FirstOrDefault(v => v.Surname == "surname test");
            var findVisitor2 = group.Visitors.FirstOrDefault(v => v.Surname == "surname2 test");

            //visitors
            Assert.AreEqual(2, group.Visitors.Count);
            Assert.AreEqual("surname test", findVisitor1.Surname);
            Assert.AreEqual("name test", findVisitor1.Name);
            Assert.AreEqual("test test", findVisitor1.SerialAndNumber);
            Assert.AreEqual("Мужчина", findVisitor1.Gender);
            Assert.AreEqual(new DateTime(1987, 5, 1).Date, findVisitor1.BithDate);
            Assert.AreEqual("Польша", findVisitor1.Nationality);
            Assert.AreEqual(false, findVisitor1.Arrived);
            Assert.AreEqual(DateTime.Now.Date, findVisitor1.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", findVisitor1.UserInSystem);

            Assert.AreEqual("surname2 test", findVisitor2.Surname);
            Assert.AreEqual("name2 test", findVisitor2.Name);
            Assert.AreEqual("test2 test", findVisitor2.SerialAndNumber);
            Assert.AreEqual("Женщина", findVisitor2.Gender);
            Assert.AreEqual(new DateTime(1950, 5, 1).Date, findVisitor2.BithDate);
            Assert.AreEqual("Германия", findVisitor2.Nationality);
            Assert.AreEqual(false, findVisitor2.Arrived);
            Assert.AreEqual(DateTime.Now.Date, findVisitor2.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", findVisitor2.UserInSystem);


            //XMLDispatcher
            var dispatch1 = database.XMLDispatchManager.GetById(findVisitor1.Id);
            var dispatch2 = database.XMLDispatchManager.GetById(findVisitor2.Id);

            Assert.AreEqual(Status.New, dispatch1.Status);
            Assert.AreEqual(Operation.Add, dispatch1.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch1.DateInSystem.Value.Date);

            Assert.AreEqual(Status.New, dispatch2.Status);
            Assert.AreEqual(Operation.Add, dispatch2.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch2.DateInSystem.Value.Date);
        }

        [TestMethod]
        public async Task Edit_Group_Delete_One_Visitor_Of_Many_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);
            var oldVisitor1 = group.Visitors.FirstOrDefault(v => v.Surname == visitor1.Surname);
            var oldVisitor2 = group.Visitors.FirstOrDefault(v => v.Surname == visitor2.Surname);
            Assert.IsNotNull(group);
            group = await EditGroup(editGroupNew, visitorNew);
            Assert.IsNotNull(group);

            //group
            Assert.IsNotNull(group);
            Assert.AreEqual(new DateTime(2018, 08, 31).Date, group.DateArrival.Value.Date);
            Assert.AreEqual(new DateTime(2018, 09, 01).Date, group.DateDeparture.Value.Date);
            Assert.AreEqual(3, group.DaysOfStay);
            Assert.AreEqual("Брест (Тересполь)", group.CheckPoint);
            Assert.AreEqual("place TEST new", group.PlaceOfRecidense);
            Assert.AreEqual("program of travel new", group.ProgramOfTravel);
            Assert.AreEqual("orginize form", group.OrganizeForm);
            Assert.AreEqual("name", group.Name);
            Assert.AreEqual("number of contract", group.NumberOfContract);
            Assert.AreEqual(new DateTime(2018, 6, 1).Date, group.DateOfContract.Value.Date);
            Assert.AreEqual("Other info", group.OtherInfo);

            Assert.AreEqual("Брестский облисполком", group.TranscriptUser);
            Assert.AreEqual(true, group.ExtraSend);
            Assert.AreEqual(true, group.Group);
            Assert.AreEqual(DateTime.Now.Date, group.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", group.UserInSystem);
            Assert.AreEqual(new DateTime(2018, 1, 1).Date, group.DateEdit.Value.Date);
            Assert.AreEqual("Admin", group.UserInSystem);

            var visitor = group.Visitors.FirstOrDefault();

            //visitors
            Assert.AreEqual(1, group.Visitors.Count);
            Assert.AreEqual("surname test new", visitor.Surname);
            Assert.AreEqual("name test new", visitor.Name);
            Assert.AreEqual("test test new", visitor.SerialAndNumber);
            Assert.AreEqual("Мужчина", visitor.Gender);
            Assert.AreEqual(new DateTime(1988, 5, 1).Date, visitor.BithDate);
            Assert.AreEqual("Польша", visitor.Nationality);
            Assert.AreEqual(true, visitor.Arrived);
            Assert.AreEqual(DateTime.Now.Date, visitor.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", visitor.UserInSystem);
            Assert.AreEqual(new DateTime(2018, 01, 01).Date, visitor.DateEdit.Value.Date);
            Assert.AreEqual("Admin", visitor.UserInSystem);

            //XMLDispatcher
            var dispatch1 = database.XMLDispatchManager.GetById(visitor.Id);
            var dispatch2 = database.XMLDispatchManager.GetById(oldVisitor2.Id);

            Assert.AreEqual(Status.New, dispatch1.Status);
            Assert.AreEqual(Operation.Add, dispatch1.Operation);
            Assert.AreEqual(DateTime.Now.Date, dispatch1.DateInSystem.Value.Date);
            Assert.IsNull(dispatch2);
        }

        [TestMethod]
        public async Task Edit_Group_Add_One_Visitor_Of_Many_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);

            Assert.IsNotNull(group);
            group = await EditGroup(editGroupNew, visitorNew, visitorNew2, visitorNew3);

            Assert.IsNotNull(group);

            foreach (var item in group.Visitors)
            {
                XMLDispatch dispatch = database.XMLDispatchManager.GetById(item.Id);
                Assert.AreEqual(Status.New, dispatch.Status);
                Assert.AreEqual(Operation.Add, dispatch.Operation);
            }
        }

        [TestMethod]
        public async Task Edit_Group_Delete_All_Visitors_Of_Many_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);
            var oldVisitor1 = group.Visitors.FirstOrDefault(v => v.Surname == visitor1.Surname);
            var oldVisitor2 = group.Visitors.FirstOrDefault(v => v.Surname == visitor2.Surname);
            Assert.IsNotNull(group);
            group = await EditGroup(editGroupNew);

            Assert.IsNotNull(group);

            XMLDispatch dispatch1 = database.XMLDispatchManager.GetById(oldVisitor1.Id);
            XMLDispatch dispatch2 = database.XMLDispatchManager.GetById(oldVisitor2.Id);

            Assert.IsNull(dispatch1);
            Assert.IsNull(dispatch2);
        }

        [TestMethod]
        public async Task Edit_Group_Update_Visitors_Of_Many_Send_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);
            await SendVisitor(group.Visitors);
            Assert.IsNotNull(group);

            group = await EditGroup(editGroupNew, visitorNew, visitorNew2);
            Assert.IsNotNull(group);

            foreach (var item in group.Visitors)
            {
                XMLDispatch dispatch = database.XMLDispatchManager.GetById(item.Id);
                Assert.AreEqual(Status.Send, dispatch.Status);
                Assert.AreEqual(Operation.Edit, dispatch.Operation);
            }
        }

        [TestMethod]
        public async Task Edit_Group_Update_Visitors_One_Visitor_Send_Of_Many_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);
            await SendVisitor(group.Visitors.FirstOrDefault());
            Assert.IsNotNull(group);

            group = await EditGroup(editGroupNew, visitorNew, visitorNew2);
            Assert.IsNotNull(group);

            var resultVisitor1 = group.Visitors.LastOrDefault();

            XMLDispatch dispatch = database.XMLDispatchManager.GetById(resultVisitor1.Id);
            Assert.AreEqual(Status.New, dispatch.Status);
            Assert.AreEqual(Operation.Add, dispatch.Operation);

        }

        [TestMethod]
        public async Task Delete_Group_Of_Many_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);
            Assert.IsNotNull(group);

            var result = await DeleteGroup(group.Id);
            Assert.IsNotNull(result);

            foreach (var item in group.Visitors)
            {
                XMLDispatch dispatch = database.XMLDispatchManager.GetById(item.Id);
                Assert.IsNull(dispatch);
            }
        }

        [TestMethod]
        public async Task Delete_Group_Of_Many_Send_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);
            Assert.IsNotNull(group);

            await SendVisitor(group.Visitors);

            var result = await DeleteGroup(group.Id);
            Assert.IsNotNull(result);

            foreach (var item in group.Visitors)
            {
                XMLDispatch dispatch = database.XMLDispatchManager.GetById(item.Id);
                Assert.AreEqual(Status.Send, dispatch.Status);
                Assert.AreEqual(Operation.Remove, dispatch.Operation);
            }
        }

        [TestMethod]
        public async Task Delete_Group_Of_Many_Recd_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);
            Assert.IsNotNull(group);

            await RecdVisitor(group.Visitors);

            var result = await DeleteGroup(group.Id);
            Assert.IsNotNull(result);

            foreach (var item in group.Visitors)
            {
                XMLDispatch dispatch = database.XMLDispatchManager.GetById(item.Id);
                Assert.AreEqual(Status.Recd, dispatch.Status);
                Assert.AreEqual(Operation.Remove, dispatch.Operation);
            }

        }

        [TestMethod]
        public async Task Delete_Group_One_Visitor_Send_Of_Many_Visitors()
        {
            var group = await CreateGroup(createGroup, visitor1, visitor2);
            Assert.IsNotNull(group);

            await SendVisitor(group.Visitors.FirstOrDefault());

            var result = await DeleteGroup(group.Id);
            Assert.IsNotNull(result);


            var resultVisitor1 = group.Visitors.FirstOrDefault();
            var resultVisitor2 = group.Visitors.LastOrDefault();

            XMLDispatch dispatch1 = database.XMLDispatchManager.GetById(resultVisitor1.Id);
            XMLDispatch dispatch2 = database.XMLDispatchManager.GetById(resultVisitor2.Id);

            Assert.AreEqual(Status.Send, dispatch1.Status);
            Assert.AreEqual(Operation.Remove, dispatch1.Operation);
            Assert.IsNull(dispatch2);
        }


        [TestMethod]
        public async Task Create_Operator()
        {
            var operatorResult = await CreateUser(operatorModel);

            Assert.IsNotNull(operatorResult);
            Assert.AreEqual("transcript test", operatorResult.ProfileUser.Transcript);
            Assert.AreEqual("123456789", operatorResult.UserName);
            Assert.AreEqual("123456789", operatorResult.ProfileUser.UNP);
            Assert.AreEqual("12345", operatorResult.ProfileUser.OKPO);
            Assert.AreEqual(true, operatorResult.ProfileUser.Active);
            Assert.AreEqual("operator", operatorResult.ProfileUser.Role);
            Assert.IsNull(operatorResult.Email);
            Assert.IsFalse(operatorResult.EmailConfirmed);
            Assert.AreEqual(DateTime.Now.Date, operatorResult.ProfileUser.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", operatorResult.ProfileUser.UserInSystem);
        }

        [TestMethod]
        public async Task Delete_Operator()
        {
            var operatorResult = await CreateUser(operatorModel);

            var result = await DeleteUser(operatorResult.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public async Task Edit_Operator()
        {
            var operatorResult = await CreateUser(operatorModel);

            var result = await EditUser(operatorResult.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);

            var resultModel = await userService.GetByIdAsync(operatorResult.Id);
            Assert.AreEqual("email", resultModel.Email);
            Assert.IsTrue(resultModel.EmailConfirmed);
            Assert.AreEqual("new Transcript", resultModel.ProfileUser.Transcript);
        }

        [TestMethod]
        public async Task DataOperators_Operator()
        {
            var operatorResult = await CreateUser(operatorModel);

            var result = operatorController.DataOperators("") as PartialViewResult;

            Assert.IsNotNull(result);

            var model = (IndexViewModel<ViewOperatorModel>)result.Model;
            Assert.AreEqual(1, model.PageInfo.TotalItems);
            Assert.AreEqual(1, model.PageInfo.PageNumber);

            var opeartorInView = model.Models.FirstOrDefault();
            Assert.AreEqual("123456789", opeartorInView.ProfileUserUNP);
        }

        [TestMethod]
        public async Task Register_WarnUnp_Account()
        {
            var operatorResult = await CreateUser(operatorModel);

            RegisterModel warnUnpModel = new RegisterModel { UNP = "1111" };

            var result = await accountController.Register(warnUnpModel) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Туроператор с УНП - 1111 не найден", result.ViewData.ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }

        [TestMethod]
        public async Task Register_WarnOKPO_Account()
        {
            var operatorResult = await CreateUser(operatorModel);

            RegisterModel warnUnpModel = new RegisterModel { UNP = "123456789", OKPO = "1111" };

            var result = await accountController.Register(warnUnpModel) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Туроператор с ОКПО - 1111 не найден", result.ViewData.ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }

        [TestMethod]
        public async Task Register_NotActive_Account()
        {
            operatorModel.Active = false;
            var operatorResult = await CreateUser(operatorModel);

            RegisterModel warnUnpModel = new RegisterModel { UNP = "123456789", OKPO = "12345" };
            var result = await accountController.Register(warnUnpModel) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Туроператор с УНП - 123456789 заблокирован", result.ViewData.ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }

        [TestMethod]
        public async Task Confirm_Email_With_Null_Parameters_Account()
        {          
            var result = await accountController.ConfirmEmail(null, null) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Register", result.ViewName);
            Assert.AreEqual("Неверные данные пользователя", result.ViewData.ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }

        [TestMethod]
        public async Task Confirm_WrangToken_Account()
        {
            var operatorResult = await CreateUser(operatorModel);
            var result = await accountController.ConfirmEmail("000", "egorik-555@yandex.ru") as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Register", result.ViewName);
            Assert.AreEqual("Неверные данные пользователя", result.ViewData.ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }

        [TestMethod]
        public async Task Confirm_Account()
        {
            var operatorResult = await CreateUser(operatorModel);
            var result = await accountController.ConfirmEmail(operatorResult.Id, "egorik-555@yandex.ru") as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.RouteValues["action"]);       
        }

        [TestMethod]
        public async Task Login_Wrang_Login_Pass_Account()
        {         
            var result = await accountController.Login(new LoginModel { Name = "1", Password = "2"}) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual("Неверный логин или пароль", result.ViewData.ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }

        [TestMethod]
        public async Task Login_Wrang_Locked_Account()
        {
            operatorModel.Active = false;
            var operatorResult = await CreateUser(operatorModel);
            var result = await accountController.Login(new LoginModel { Name = "123456789", Password = "123456" }) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual("Пользователь заблокирован", result.ViewData.ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }

        [TestMethod]
        public async Task Login_Email_Not_Confirmed_Account()
        {
            var operatorResult = await CreateUser(operatorModel);
            var result = await accountController.Login(new LoginModel { Name = "123456789", Password = "123456" }) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual("Email не подтвержден", result.ViewData.ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }

        //ReportController
        [TestMethod]
        public void Test_Report_Controller_GetDataByDateCount()
        {
            var result = reporterController.GetDataByDateCount(DateTime.Parse("01.06.2016"), DateTime.Parse("30.07.2018");

            Assert.IsNotNull(result);
        
        }
    }
}
