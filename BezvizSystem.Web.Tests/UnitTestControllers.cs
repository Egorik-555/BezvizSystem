using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Controllers;
using BezvizSystem.Web.Tests.TestServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BezvizSystem.Web.Models.Visitor;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using BezvizSystem.Web.Models.Group;

namespace BezvizSystem.Web.Tests
{
    [TestClass]
    public class UnitTestControllers
    {
        IServiceCreator serviceCreator = new ServiceCreatorTest();
        VisitorController visitorController;
        GroupController groupController;
        IService<VisitorDTO> visitorService;
        IService<GroupVisitorDTO> groupService;
        IDictionaryService<CheckPointDTO> checkPoint;
        IDictionaryService<NationalityDTO> nationalities;
        IDictionaryService<GenderDTO> genders;

        public UnitTestControllers()
        {
            visitorService = serviceCreator.CreateVisitorService(null);
            groupService = serviceCreator.CreateGroupService(null);
            checkPoint = serviceCreator.CreateCheckPointService(null);
            nationalities = serviceCreator.CreateNationalityService(null);
            genders = serviceCreator.CreateGenderService(null);

            visitorController = new VisitorController(groupService, checkPoint, nationalities, genders);
            groupController = new GroupController(groupService, checkPoint, nationalities, genders);

            InitModel();
        }

        private void SimulateValidation(object model)
        {
            // mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                this.visitorController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
                this.groupController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        InfoVisitorModel visitor1;
        InfoVisitorModel visitor2;

        CreateVisitorModel createVisitor;
        CreateGroupModel createGroup;

        private void InitModel()
        {
            visitor1 = new InfoVisitorModel
            {
                Id = 1,               
                Surname = "surname1",
                Name = "name1",
                SerialAndNumber = "test 1",
                Gender = "Мужчина",
                BithDate = new DateTime(1987, 5, 1),
                Nationality = "nat1",
                DateInSystem = DateTime.Now,
                UserInSystem = "Test"
            };

            visitor2 = new InfoVisitorModel
            {
                Id = 2,
                Surname = "surname2",
                Name = "name2",
                SerialAndNumber = "test 2",
                Gender = "Мужчина",
                BithDate = new DateTime(1990, 6, 5),
                Nationality = "nat2",
                DateInSystem = DateTime.Now,
                UserInSystem = "Test"
            };

            createVisitor = new CreateVisitorModel
            {
                Id = 1,
                Info = visitor1,
                DateArrival = DateTime.Now,
                DateDeparture = DateTime.Now,
                DaysOfStay = 1,
                CheckPoint = "Check1",
                PlaceOfRecidense = "place",
                ProgramOfTravel = "program of travel",
                TimeOfWork = "time work",
                SiteOfOperator = "site",
                TelNumber = "tel",
                Email = "egorik-555@yandex.ru",
                DateInSystem = DateTime.Now,
                UserInSystem = "Test",             
            };

            createGroup = new CreateGroupModel
            {
                Id = 2,
                Infoes = new List<InfoVisitorModel> { visitor1, visitor2 },
                DateArrival = DateTime.Now,
                DateDeparture = DateTime.Now,
                DaysOfStay = 1,
                CheckPoint = "Check1",
                PlaceOfRecidense = "place",
                ProgramOfTravel = "program of travel",
                OrganizeForm = "organize form",
                Name = "name",
                NumberOfContract = "number of contract 123",
                DateOfContract = new DateTime(2018, 1, 3),
                DateInSystem = DateTime.Now,
                UserInSystem = "Test"
            };
        }

        [TestMethod]
        public async Task Validation_Fail_Model_Visitor_VisitorController()
        {
            var model = new CreateVisitorModel();

            SimulateValidation(model);
            var result = (ViewResult)await visitorController.Create(model);

            Assert.IsNotNull(result);

            var viewData = result.ViewData;
            var viewBag = result.ViewBag;

            Assert.AreEqual(3, viewData.Values.Count);
            Assert.AreEqual(3, ((SelectList)viewBag.Genders).Count());
            Assert.AreEqual(4, ((SelectList)viewBag.CheckPoints).Count());
            Assert.AreEqual(5, ((SelectList)viewBag.Nationalities).Count());
        }

        [TestMethod]
        public void Create_Visitor_VisitorController()
        {
            var result = (ViewResult) visitorController.Create();
            var viewData =  result.ViewData;
            var viewBag = result.ViewBag;
        
            Assert.AreEqual(3, viewData.Values.Count);
            Assert.AreEqual(3, ((SelectList)viewBag.Genders).Count());
            Assert.AreEqual(4, ((SelectList)viewBag.CheckPoints).Count());
            Assert.AreEqual(5, ((SelectList)viewBag.Nationalities).Count());
        }

      
        [TestMethod]
        public async Task Create_Visitor_With_Model_VisitorController()
        {        
            SimulateValidation(visitor1);
            SimulateValidation(createVisitor);
            var result = (await visitorController.Create(createVisitor)) as RedirectToRouteResult;

            Assert.IsNotNull(result);

            if (result != null)
            {               
                Assert.AreEqual(1, groupService.GetAll().Count());
                Assert.AreEqual(1, visitorService.GetAll().Count());
            }
        }

        [TestMethod]
        public async Task Validation_Fail_Model_GroupVisitor_GroupVisitorController()
        {
            var model = new CreateGroupModel();

            SimulateValidation(model);
            var result = (ViewResult)await groupController.Create(model);

            Assert.IsNotNull(result);

            var viewData = result.ViewData;
            var viewBag = result.ViewBag;

            Assert.AreEqual(3, viewData.Values.Count);
            Assert.AreEqual(3, ((SelectList)viewBag.Genders).Count());
            Assert.AreEqual(4, ((SelectList)viewBag.CheckPoints).Count());
            Assert.AreEqual(5, ((SelectList)viewBag.Nationalities).Count());
        }

        [TestMethod]
        public void Create_GroupVisitor_GroupVisitorController()
        {
            var result = (ViewResult)groupController.Create();
            var viewData = result.ViewData;
            var viewBag = result.ViewBag;


            Assert.IsNotNull(result.Model);
            Assert.AreEqual(1, ((CreateGroupModel)result.Model).Infoes.Count);

            Assert.AreEqual(3, viewData.Values.Count);
            Assert.AreEqual(3, ((SelectList)viewBag.Genders).Count());
            Assert.AreEqual(4, ((SelectList)viewBag.CheckPoints).Count());
            Assert.AreEqual(5, ((SelectList)viewBag.Nationalities).Count());
        }

        [TestMethod]
        public async Task Create_GroupVisitor_With_Model_GroupVisitorController()
        {
            SimulateValidation(visitor1);
            SimulateValidation(visitor2);
            SimulateValidation(createGroup);
            var result = (await groupController.Create(createGroup)) as RedirectToRouteResult;

            Assert.IsNotNull(result);

            if (result != null)
            {
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual("Index", result.RouteValues["action"]);
                Assert.AreEqual(1, groupService.GetAll().Count());
                Assert.AreEqual(2, visitorService.GetAll().Count());
            }
        }
    }
}
