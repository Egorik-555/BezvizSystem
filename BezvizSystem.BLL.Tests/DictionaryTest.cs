using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.DAL.Interfaces;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class DictionaryTest
    {
        [TestMethod]
        public void Test_Get_Dictionary()
        {
            IUnitOfWork database = new TestUnitOfWork();
            IDictionaryService<StatusDTO> serviceStatus = new DictionaryService<StatusDTO>(database);
            IDictionaryService<NationalityDTO> serviceNat = new DictionaryService<NationalityDTO>(database);
            IDictionaryService<CheckPointDTO> serviceCheck = new DictionaryService<CheckPointDTO>(database);

            IEnumerable<StatusDTO> statuses = (IEnumerable<StatusDTO>)serviceStatus.Get();
            IEnumerable<NationalityDTO> nationalities = (IEnumerable<NationalityDTO>)serviceNat.Get();
            IEnumerable<CheckPointDTO> checkPoints = (IEnumerable<CheckPointDTO>)serviceCheck.Get();
           
            Assert.IsNotNull(statuses);
            Assert.IsNotNull(nationalities);
            Assert.IsNotNull(checkPoints);
        }
    }
}
