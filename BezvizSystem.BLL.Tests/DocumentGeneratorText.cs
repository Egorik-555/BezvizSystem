using System;
using System.Collections.Generic;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Utils;
using ClosedXML.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class DocumentGeneratorText
    {
        [TestMethod]
        public void Generate_Visitor_Document_Test()
        {
            IDocumentGenerator writer = new DocumentGenerator();

            //string file = HostingEnvironment.MapPath("~/App_Data/XMLs/" + fileName);
            GroupVisitorDTO visitor = new GroupVisitorDTO
            {
                Visitors = new List<VisitorDTO> { new VisitorDTO {
                    Surname = "Surname", Name = "Name", BithDate = DateTime.Now, Nationality = "POLAND", Gender = "Мужчина", SerialAndNumber = "AB 123456"} },
                DateArrival = new DateTime(2018, 05, 06),
                DateDeparture = new DateTime(2018, 05, 10),
                PlaceOfRecidense = "dfsdf sf sofsdfps doifnsdp fsjdf sdfns diofmsjdngsd msdjnfl skd",
                ProgramOfTravel = "aslfkasl aksflaks famf mweowef wepfoweofweof w,e[few pe ",
                TimeOfWork = "11.00 - 21.00",
                TranscriptUser = "OKO POKO",
                TelNumber = "1111111",
                Email = "heruvima@papapa.ru"
            };

            XLWorkbook book = writer.GenerateDocumentVisitor("D:\\Develop\\BezvizSystem_2_0\\BezvizSystem.BLL.Tests\\bin\\Debug\\templateVisitor.xlsx", visitor);

            book.SaveAs("Visitor.xlsx");
        }

        [TestMethod]
        public void Generate_Group_Document_Test()
        {
            IDocumentGenerator writer = new DocumentGenerator();

            GroupVisitorDTO group = new GroupVisitorDTO
            {
                Visitors = new List<VisitorDTO> {
                    new VisitorDTO { Surname = "Surname1", Name = "Name1", BithDate = DateTime.Now, Nationality = "POLAND1", Gender = "Мужчина1", SerialAndNumber = "AB 111"},
                    new VisitorDTO { Surname = "Surname2", Name = "Name2", BithDate = DateTime.Now, Nationality = "POLAND2", Gender = "Мужчина2", SerialAndNumber = "AB 222"},
                    new VisitorDTO { Surname = "Surname3", Name = "Name3", BithDate = DateTime.Now, Nationality = "POLAND3", Gender = "Мужчина3", SerialAndNumber = "AB 333"},
                },

                DateArrival = new DateTime(2018, 05, 06),
                DateDeparture = new DateTime(2018, 05, 10),
                PlaceOfRecidense = "dfsdf sf sofsdfps doifnsdp fsjdf sdfns diofmsjdngsd msdjnfl skd",
                ProgramOfTravel = "aslfkasl aksflaks famf mweowef wepfoweofweof w,e[few pe ",
                OrganizeForm = "Organize form",
                Name = "name",
                NumberOfContract = "123 contract",
                DateOfContract = DateTime.Now,
                OtherInfo = "over info asfasfsdf",
                TranscriptUser = "OKO POKO"
            };

            XLWorkbook book = writer.GenerateDocumentGroup("D:\\Develop\\BezvizSystem_2_0\\BezvizSystem.BLL.Tests\\bin\\Debug\\templateGroup.xlsx", group);

            book.SaveAs("Group.xlsx");
        }
    }
}
