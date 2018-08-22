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

namespace BezvizSystem.Web.Tests
{
    [TestClass]
    public class IntegratedTest
    {
        const string CONNECT = "BezvizConnection";

        IServiceCreator serviceCreator = new ServiceCreator();

        IService<VisitorDTO> visitorService;
        IService<GroupVisitorDTO> groupService;
        IService<AnketaDTO> anketaService;
        IDictionaryService<CheckPointDTO> checkPoint;
        IDictionaryService<NationalityDTO> nationalities;
        IDictionaryService<GenderDTO> genders;

        VisitorController visitorController;
        GroupController groupController;
        AnketaController anketaController;

        public IntegratedTest()
        {
            visitorService = serviceCreator.CreateVisitorService(CONNECT);
            groupService = serviceCreator.CreateGroupService(CONNECT);
            checkPoint = serviceCreator.CreateCheckPointService(CONNECT);
            nationalities = serviceCreator.CreateNationalityService(CONNECT);
            genders = serviceCreator.CreateGenderService(CONNECT);

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
            PlaceOfRecidense = "place",
            ProgramOfTravel = "program of travel",
            TimeOfWork = "time work",
            SiteOfOperator = "site",
            TelNumber = "tel",
            Email = "egorik-555@yandex.ru",
            DateInSystem = DateTime.Now,
            UserInSystem = "Admin"
        };


        [TestMethod]
        public async Task Create_Group_Of_One_Visitor()
        {
            createVisitor.Info = visitor1;
            var result = (await visitorController.Create(createVisitor)) as RedirectToRouteResult;

            Assert.IsNotNull(result);

            var group = groupService.GetAll().SingleOrDefault(g => g.CheckPoint == "Брест (Тересполь)");

            //group
            Assert.IsNotNull(group);
            Assert.AreEqual(DateTime.Now.Date, group.DateArrival.Value.Date);
            Assert.AreEqual(DateTime.Now.Date, group.DateDeparture.Value.Date);
            Assert.AreEqual(1, group.DaysOfStay);
            Assert.AreEqual("Брест (Тересполь)", group.CheckPoint);
            Assert.AreEqual("place", group.PlaceOfRecidense);
            Assert.AreEqual("program of travel", group.ProgramOfTravel);
            Assert.AreEqual("time work", group.TimeOfWork);
            Assert.AreEqual("site", group.SiteOfOperator);
            Assert.AreEqual("tel", group.TelNumber);
            Assert.AreEqual("egorik-555@yandex.ru", group.Email);
            Assert.AreEqual(DateTime.Now.Date, group.DateInSystem.Value.Date);
            Assert.AreEqual("Admin", group.UserInSystem);

            Assert.AreEqual(1, visitorService.GetAll().Count());

        }
    }
}
