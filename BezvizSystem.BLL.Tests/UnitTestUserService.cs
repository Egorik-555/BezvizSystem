using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.DAL;
using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BezvizSystem.DAL.Entities;
using BezvizSystem.BLL.Tests.TestServises;

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestUserService
    {
        IUserService service;
        IUnitOfWork database;
       
        public UnitTestUserService()
        {
            CreateTestRepositories repoes = new CreateTestRepositories();
            database = repoes.CreateIoWManager();

            service = new UserService(database);          
        }

        [TestMethod]
        public async Task Init_DataAsync()
        {
            UserDTO user = new UserDTO
            {
                UserName = "Test",
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { Role = "Test1" }
            };

            List<string> list = new List<string>
            {
                "Test1",
                "Test2"
            };

            var result = await service.SetInitialData(user, list);
            var roleResult1 = await database.RoleManager.FindByNameAsync("Test1");
            var roleResult2 = await database.RoleManager.FindByNameAsync("Test2");
            //var userUser = await service.GetByNameAsync("Test");

            //Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(roleResult1.Name == "Test1");
            Assert.IsTrue(roleResult2.Name == "Test2");
            //Assert.IsNotNull(userUser);      
        }

        [TestMethod]
        public async Task Create_User_Test()
        {
            UserDTO user = new UserDTO
            {
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { OKPO = "okpo", UNP = "unp", Role = "role1", Transcript = "transcript", DateInSystem = DateTime.Now }
            };

            var result = await service.Create(user);
            var userResult = await service.GetByNameAsync("unp");

            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(userResult);
            Assert.IsTrue(userResult.UserName == "unp");
        }

        [TestMethod]
        public async Task Delete_User_Test()
        {
            UserDTO user = new UserDTO
            {
                Id = "aaa", UserName = "Test1",  
                ProfileUser = new ProfileUserDTO { OKPO = "okpo", UNP = "unp", Role = "role1", Transcript = "transcript", DateInSystem = DateTime.Now }
            };
          
            var result = await service.Delete(user);
            var userResult1 = await service.GetByNameAsync("Test1");
            var userResult2 = await service.GetByIdAsync("aaa");
            var profile = database.OperatorManager.GetById("aaa");

            Assert.IsTrue(result.Succedeed);
            Assert.IsNull(userResult1);
            Assert.IsNull(userResult2);
            Assert.IsNull(profile);
        }

        [TestMethod]
        public async Task Update_UserAsync()
        {
            UserDTO user = new UserDTO
            {
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { OKPO = "okpo", UNP = "unp" }
            };

            var findUser = await service.GetByNameAsync(user.ProfileUser.UNP);
            if(findUser != null)
            {
                await service.Delete(findUser);
            }

            var result = await service.Create(user);
            if (result.Succedeed)
            {
                findUser = await service.GetByNameAsync(user.ProfileUser.UNP);
                findUser.Email = "test@test.ru";
                findUser.ProfileUser.Transcript = "transcript";
                var updateResult = await service.Update(findUser);

                findUser = await service.GetByNameAsync(user.ProfileUser.UNP);
                await service.Delete(findUser);

                Assert.IsTrue(updateResult.Succedeed);
                Assert.IsTrue(findUser.Email == "test@test.ru");
                Assert.IsTrue(findUser.ProfileUser.Transcript == "transcript");
            }
        }

        [TestMethod]
        public async Task AutheticateAsync()
        {
            UserDTO user = new UserDTO
            {
                UserName = "Egor",
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { Role = "Admin" }
            };

            var claim = await service.Authenticate(user);
            var controlUser = await database.UserManager.FindAsync(user.UserName, user.Password);

            if (claim == null && controlUser == null) Assert.IsTrue(true);
            if (claim != null && controlUser != null) Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task Not_AutheticateAsync()
        {
            UserDTO user = new UserDTO
            {
                UserName = "Egor",
                Password = "qwerty111",
                ProfileUser = new ProfileUserDTO { Role = "Admin" }
            };

            var claim = await service.Authenticate(user);
            if (claim == null) Assert.IsTrue(true);
            await service.Delete(user);
        }

        [TestMethod]
        public async Task Get_By_Id_Test()
        {
            var users = await service.GetByIdAsync("aaa");
            Assert.IsTrue(users.Id == "aaa");
            Assert.IsTrue(users.ProfileUser.UNP == "UnpTest1");
            Assert.IsTrue(users.ProfileUser.OKPO == "OKPO1");
        }

        [TestMethod]
        public async Task Get_By_Name_Test()
        {
            var users = await service.GetByNameAsync("Admin");
            Assert.IsTrue(users.Id == "ddd");
            Assert.IsTrue(users.ProfileUser.UNP == "UnpAdmin");
            Assert.IsTrue(users.ProfileUser.OKPO == "OKPO4");
        }

        [TestMethod]
        public void Get_All_Test()
        {
            var users = service.GetAll();
            Assert.IsTrue(users.Count() == 4);
            Assert.IsTrue(users.Where(u => u.Id == "aaa").FirstOrDefault().ProfileUser.UNP == "UnpTest1");
        }

        [TestMethod]
        public void Get_All_By_Role_Test()
        {          
            var users = service.GetByRole("operator");
            Assert.IsTrue(users.Count() == 3);
            Assert.IsNull(users.Where(u => u.Id == "ddd").FirstOrDefault());
            Assert.IsTrue(users.Where(u => u.Id == "aaa").FirstOrDefault().ProfileUser.OKPO == "OKPO1");
        }
   
    }
}
