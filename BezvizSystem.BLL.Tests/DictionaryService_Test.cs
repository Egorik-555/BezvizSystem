using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Interfaces;
using System.Linq;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class DictionaryService_Test
    {
        IDictionaryService<StatusDTO> serviceStatus;
        IDictionaryService<NationalityDTO> serviceNat;
        IDictionaryService<CheckPointDTO> serviceCheck;

        public DictionaryService_Test()
        {
            IUnitOfWork database = new TestUnitOfWork();
         
            serviceStatus = new DictionaryService<StatusDTO>(database);
            serviceNat = new DictionaryService<NationalityDTO>(database);
            serviceCheck = new DictionaryService<CheckPointDTO>(database);
        }


        [TestMethod]
        public void Test_Init_Instance_Dictionary()
        {          
            var statuses = (IEnumerable<StatusDTO>)serviceStatus.Get();
            var nationalities = (IEnumerable<NationalityDTO>)serviceNat.Get();
            var checkPoints = (IEnumerable<CheckPointDTO>)serviceCheck.Get();
           
            Assert.IsNotNull(statuses);
            Assert.IsNotNull(nationalities);
            Assert.IsNotNull(checkPoints);
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
            Assert.IsTrue(checkPoints.Count() == 2);
            Assert.IsNotNull(checkPoints.Where(s => s.Name == "checkPoint1") != null);
        }

        [TestMethod]
        public void Test_Get_Nationality_Dictionary()
        {
            var nationalities = serviceNat.Get();

            Assert.IsNotNull(nationalities);
            Assert.IsTrue(nationalities.Count() == 3);
            Assert.IsNotNull(nationalities.Where(s => s.ShortName == "n1") != null);
            Assert.IsNotNull(nationalities.Where(s => s.ShortName == "n4") == null);
        }
    }
}
