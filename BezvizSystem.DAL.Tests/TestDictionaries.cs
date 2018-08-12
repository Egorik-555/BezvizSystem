using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Data.Entity;

namespace BezvizSystem.DAL.Tests
{
    [TestClass]
    public class TestDictionaries
    {
        const string CONNECT = "BezvizConnection";

        GenderManager genderMng;
        CheckPointManager checkPointMng;
        NationalityManager nationalitiesMng;

        public TestDictionaries()
        {
            genderMng = new GenderManager(new BezvizContext(CONNECT));
            checkPointMng = new CheckPointManager(new BezvizContext(CONNECT));          
            nationalitiesMng = new NationalityManager(new BezvizContext(CONNECT));
        }

        [TestMethod]
        public async Task CheckPoint_Dictionary()
        {        
            CheckPoint checkPoint1 = checkPointMng.GetById(1);
            CheckPoint checkPoint2 = await checkPointMng.GetByIdAsync(1);
            var checkPoints = checkPointMng.GetAll();

            Assert.IsNotNull(checkPoint1);
            Assert.IsTrue(checkPoint1.Active);
            Assert.IsNotNull(checkPoint2);
            Assert.IsTrue(checkPoint2.Name.Contains("Брест"));
            Assert.IsTrue(checkPoints.Count() != 0);
            Assert.IsTrue(checkPoints.Contains(checkPoint1));
            Assert.IsTrue(checkPoints.Contains(checkPoint2));
        }

        [TestMethod]
        public async Task Nationality_Dictionary()
        {
            Nationality nat1 = nationalitiesMng.GetById(1);
            Nationality nat2 = await nationalitiesMng.GetByIdAsync(1);
            var nationalities = nationalitiesMng.GetAll();

            Assert.IsNotNull(nat1);
            Assert.IsTrue(nat1.Active);
            Assert.IsNotNull(nat2);
            Assert.IsTrue(nat2.Name.Contains("Польша"));
            Assert.IsTrue(nationalities.Count() != 0);
            Assert.IsTrue(nationalities.Contains(nat1));
            Assert.IsTrue(nationalities.Contains(nat2));
        }

        [TestMethod]
        public async Task Gender_Dictionary()
        {
            Gender gender1 = genderMng.GetById(1);
            Gender gender2 = await genderMng.GetByIdAsync(1);
            var genders = genderMng.GetAll();

            Assert.IsNotNull(gender1);
            Assert.IsTrue(gender2.Active);
            Assert.IsNotNull(gender2);
            Assert.IsTrue(gender1.Name.Contains("Муж"));
            Assert.IsTrue(genders.Count() != 0);
            Assert.IsTrue(genders.Contains(gender1));
            Assert.IsTrue(genders.Contains(gender2));
        }
    }
}
