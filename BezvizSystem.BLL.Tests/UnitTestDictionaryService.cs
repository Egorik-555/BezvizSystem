using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Interfaces;
using System.Linq;
using BezvizSystem.BLL.Tests.TestServises;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestDictionaryService
    {     
        IDictionaryService<NationalityDTO> serviceNat;
        IDictionaryService<CheckPointDTO> serviceCheck;
        IDictionaryService<GenderDTO> serviceGender;

        public UnitTestDictionaryService()
        {
            CreateTestRepositories creator = new CreateTestRepositories();
            IUnitOfWork database = creator.CreateIoWManager();
        
            serviceNat = new DictionaryService<NationalityDTO>(database);
            serviceCheck = new DictionaryService<CheckPointDTO>(database);
            serviceGender = new DictionaryService<GenderDTO>(database);
        }


        [TestMethod]
        public void Test_Init_Instance_Dictionary()
        {                    
            var nationalities = (IEnumerable<NationalityDTO>)serviceNat.Get();
            var checkPoints = (IEnumerable<CheckPointDTO>)serviceCheck.Get();
            var genders = (IEnumerable<GenderDTO>)serviceGender.Get();
        
            Assert.IsNotNull(nationalities);
            Assert.IsNotNull(checkPoints);
            Assert.IsNotNull(genders);
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
        public void Test_Get_Gender_Dictionary()
        {
            var genders = serviceGender.Get();

            Assert.IsNotNull(genders);
            Assert.IsTrue(genders.Count() == 2);        
            Assert.IsTrue(genders.Where(s => s.Code == 1).FirstOrDefault().Name == "Мужчина");

        }
    }
}
