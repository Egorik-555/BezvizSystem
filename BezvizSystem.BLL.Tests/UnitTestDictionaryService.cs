using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Interfaces;
using System.Linq;
using BezvizSystem.BLL.Tests.TestServises;
using BezvizSystem.BLL.DTO.Log;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestDictionaryService
    {
        IDictionaryService<StatusDTO> serviceStatus;
        IDictionaryService<NationalityDTO> serviceNat;
        IDictionaryService<CheckPointDTO> serviceCheck;
        IDictionaryService<TypeOfOperationDTO> serviceOperation;
        IDictionaryService<GenderDTO> serviceGender;

        public UnitTestDictionaryService()
        {
            CreateTestRepositories creator = new CreateTestRepositories();
            IUnitOfWork database = creator.CreateIoWManager();

            serviceStatus = new DictionaryService<StatusDTO>(database);
            serviceNat = new DictionaryService<NationalityDTO>(database);
            serviceCheck = new DictionaryService<CheckPointDTO>(database);
            serviceOperation = new DictionaryService<TypeOfOperationDTO>(database);
            serviceGender = new DictionaryService<GenderDTO>(database);
        }


        [TestMethod]
        public void Test_Init_Instance_Dictionary()
        {          
            var statuses = (IEnumerable<StatusDTO>)serviceStatus.Get();
            var nationalities = (IEnumerable<NationalityDTO>)serviceNat.Get();
            var checkPoints = (IEnumerable<CheckPointDTO>)serviceCheck.Get();
            var operations = (IEnumerable<TypeOfOperationDTO>)serviceOperation.Get();
            var genders = (IEnumerable<GenderDTO>)serviceGender.Get();

            Assert.IsNotNull(statuses);
            Assert.IsNotNull(nationalities);
            Assert.IsNotNull(checkPoints);
            Assert.IsNotNull(operations);
            Assert.IsNotNull(genders);
        }

        [TestMethod]
        public void Test_Get_Status_Dictionary()
        {
            var statuses = serviceStatus.Get();
            
            Assert.IsNotNull(statuses);
            Assert.IsTrue(statuses.Count() == 2);
            Assert.IsNotNull(statuses.Where(s => s.Code == 1).FirstOrDefault().Name == "Status1");
        }

        [TestMethod]
        public void Test_Get_checkPoint_Dictionary()
        {
            var checkPoints = serviceCheck.Get();

            Assert.IsNotNull(checkPoints);
            Assert.IsTrue(checkPoints.Count() == 3);
            Assert.IsNotNull(checkPoints.Where(s => s.Name == "check3").FirstOrDefault().Id == 1);
        }

        [TestMethod]
        public void Test_Get_Nationality_Dictionary()
        {
            var nationalities = serviceNat.Get();

            Assert.IsNotNull(nationalities);
            Assert.IsTrue(nationalities.Count() == 3);
            Assert.IsNotNull(nationalities.Where(s => s.ShortName == "n1") != null);
            Assert.IsNotNull(nationalities.Where(s => s.Name == "n2") == null);
        }

        [TestMethod]
        public void Test_Get_TypeOfOperation_Dictionary()
        {
            var operations = serviceOperation.Get();

            Assert.IsNotNull(operations);
            Assert.IsTrue(operations.Count() == 2);
            Assert.IsNotNull(operations.Where(s => s.Code == 3).Count() == 0);
            Assert.IsTrue(operations.Where(s => s.Name == "Enter").FirstOrDefault().Code == 1 );
           
        }

        [TestMethod]
        public void Test_Get_Gender_Dictionary()
        {
            var genders = serviceGender.Get();

            Assert.IsNotNull(genders);
            Assert.IsTrue(genders.Count() == 2);        
            Assert.IsTrue(genders.Where(s => s.Code == 1).FirstOrDefault().Name == "Мужчина");

        }
    }
}
