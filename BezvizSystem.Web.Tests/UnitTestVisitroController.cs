using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.Web.Controllers;
using BezvizSystem.Web.Tests.TestServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BezvizSystem.Web.Models.Visitor;

namespace BezvizSystem.Web.Tests
{
    [TestClass]
    public class UnitTestVisitroController
    {
        IServiceCreator serviceCreator = new ServiceCreatorTest();
        VisitorController controller;

        public UnitTestVisitroController()
        {
            var groupService = serviceCreator.CreateGroupService(null);
            var checkPoint = serviceCreator.CreateCheckPointService(null);
            var nationalities = serviceCreator.CreateNationalityService(null);
            var genders = serviceCreator.CreateGenderService(null);

            controller = new VisitorController(groupService, checkPoint, nationalities, genders);         
        }


        [TestMethod]
        public void Create_Visitor()
        {
            var result = (ViewResult) controller.Create();
            var viewData =  result.ViewData;
            var viewBag = result.ViewBag;
        
            Assert.AreEqual(3, viewData.Values.Count);
            Assert.AreEqual(3, ((SelectList)viewBag.Genders).Count());
            Assert.AreEqual(4, ((SelectList)viewBag.CheckPoints).Count());
            Assert.AreEqual(5, ((SelectList)viewBag.Nationalities).Count());
        }

        [TestMethod]
        public void Create_Visitor_With_Model()
        {
            var visitor = new InfoVisitorModel
            {

            };


            var model = new CreateVisitorModel
            {
                Id = 1,
                Info = visitor,
                DateArrival = DateTime.Now,
                DateDeparture = DateTime.Now,
                DaysOfStay = 1,
                CheckPoint = "Check1",
                PlaceOfRecidense = "place",
                ProgramOfTravel = "program of travel",
                TimeOfWork = "time work",
                SiteOfOperator = "site",
                TelNumber = "tel",
                Email = "egorik-555@yandex.ru"
            };
        }

    }
}
