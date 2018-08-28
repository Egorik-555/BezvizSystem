using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Controllers;
using BezvizSystem.Web.Models.Group;
using BezvizSystem.Web.Models.Visitor;
using BezvizSystem.Web.Tests.TestServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BezvizSystem.Web.Tests
{
    [TestClass]
    public class UnitTestControllers
    {
        IServiceCreator serviceCreator = new ServiceCreatorTest();
        VisitorController visitorController;
        GroupController groupController;
        AnketaController anketaController;

        IService<VisitorDTO> visitorService;
        IService<GroupVisitorDTO> groupService;
        IService<AnketaDTO> anketaService;
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
            anketaController = new AnketaController(anketaService, groupService, checkPoint, nationalities, genders);

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
                this.anketaController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
                this.visitorController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
                this.groupController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        InfoVisitorModel visitor1;
        InfoVisitorModel visitor2;
        EditInfoVisitorModel editInfo1;
        EditInfoVisitorModel editInfo2;

        CreateVisitorModel createVisitor;
        CreateGroupModel createGroup;

        EditVisitorModel editVisitor;
        EditGroupModel editGroup;

        private void InitModel()
        {
            visitor1 = new InfoVisitorModel
            {
                Id = 111,
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
                Id = 222,
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
                Id = 111,
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
                Id = 222,
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

            editInfo1 = new EditInfoVisitorModel
            {
                Id = 1,
                Surname = "surname1 new",
                Name = "name1 new",
                SerialAndNumber = "test 1 new",
                Gender = "Мужчина",
                BithDate = new DateTime(1987, 5, 1),
                Nationality = "nat1",
                DateInSystem = DateTime.Now,
                UserInSystem = "Test",
                DateEdit = DateTime.Now,
                UserEdit = "Test edit"
            };

            editInfo2 = new EditInfoVisitorModel
            {
                Id = 2,
                Surname = "surname2 new",
                Name = "name2 new",
                SerialAndNumber = "test 2 new",
                Gender = "Мужчина",
                BithDate = new DateTime(2000, 5, 1),
                Nationality = "nat1",
                DateInSystem = DateTime.Now,
                UserInSystem = "Test",
                DateEdit = DateTime.Now,
                UserEdit = "Test 2 edit"
            };

            editVisitor = new EditVisitorModel
            {
                Id = 2,
                Info = editInfo1,
                DateArrival = DateTime.Now,
                DateDeparture = DateTime.Now,
                DaysOfStay = 1,
                CheckPoint = "Check1 new",
                PlaceOfRecidense = "place new",
                ProgramOfTravel = "program of travel",
                TimeOfWork = "time work",
                SiteOfOperator = "site",
                TelNumber = "tel",
                Email = "egorik-555@yandex.ru",
                DateInSystem = DateTime.Now,
                UserInSystem = "Test new",
                DateEdit = new DateTime(2018, 1, 1),
                UserEdit = "Test edit",
                TranscriptUser = "Test"
            };

            editGroup = new EditGroupModel
            {
                Id = 1,
                Infoes = new List<EditInfoVisitorModel> { editInfo1, editInfo2 },
                DateArrival = DateTime.Now,
                DateDeparture = DateTime.Now,
                DaysOfStay = 1,
                CheckPoint = "Check1 new",
                PlaceOfRecidense = "place new",
                ProgramOfTravel = "program of travel",
                OrganizeForm = "organize form",
                Name = "name new",
                NumberOfContract = "number of contract 123",
                DateOfContract = new DateTime(2018, 1, 3),
                TranscriptUser = "transcript user",
                DateInSystem = DateTime.Now,
                UserInSystem = "Test",
                DateEdit = new DateTime(2018, 1, 1),
                UserEdit = "test edit"
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
            var result = (ViewResult)visitorController.Create();
            var viewData = result.ViewData;
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
                Assert.AreEqual(4, groupService.GetAll().Count());
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
                Assert.AreEqual(4, groupService.GetAll().Count());
                Assert.AreEqual(2, visitorService.GetAll().Count());
            }
        }


        [TestMethod]
        public async Task Index_AnketaController()
        {
            //anketaController.User
            //var result = (await anketaController.Index()) as ViewResult;
            //Assert.IsNotNull(result);

            //var model = result.Model as IEnumerable<AnketaDTO>;
            //Assert.IsNotNull(model);

            //Assert.AreEqual(4, model.Count());
            //Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public async Task Edit_AnketaController()
        {
            var result1 = (await anketaController.Edit(1)) as ViewResult;
            Assert.IsNotNull(result1);
            var model1 = result1.Model as EditGroupModel;
            Assert.IsNotNull(model1);

            var result2 = (await anketaController.Edit(2)) as ViewResult;          
            Assert.IsNotNull(result2);         
            var model2 = result2.Model as EditVisitorModel;        
            Assert.IsNotNull(model2);

            Assert.AreEqual("EditGroup", result1.ViewName);
            Assert.AreEqual("EditVisitor", result2.ViewName);
        }

        [TestMethod]
        public async Task Edit_With_Model_Visitor_AnketaController()
        {
            InitModel();

            SimulateValidation(editInfo1);
            SimulateValidation(editVisitor);
            var result = (await anketaController.EditVisitor(editVisitor, null)) as RedirectToRouteResult;

            var newVisitor = await groupService.GetByIdAsync(2);

            Assert.AreEqual("Test edit", newVisitor.UserEdit);
            Assert.AreEqual("Test edit", newVisitor.Visitors.FirstOrDefault().UserEdit);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public async Task Edit_With_Model_Group_AnketaController()
        {
            InitModel();

            SimulateValidation(editInfo1);
            SimulateValidation(editInfo2);
            SimulateValidation(editGroup);
            var result = (await anketaController.EditGroup(editGroup, null)) as RedirectToRouteResult;

            var newVisitor = await groupService.GetByIdAsync(1);

            Assert.AreEqual("test edit", newVisitor.UserEdit);
            Assert.AreEqual("Test edit", newVisitor.Visitors.ToList()[0].UserEdit);
            Assert.AreEqual("Test 2 edit", newVisitor.Visitors.ToList()[1].UserEdit);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
