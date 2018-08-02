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
            var userUser = await database.UserManager.FindByNameAsync("Test");

            Assert.IsTrue(result.Succedeed);
            Assert.IsTrue(roleResult1.Name == "Test1");
            Assert.IsTrue(roleResult2.Name == "Test2");
            Assert.IsNotNull(userUser.OperatorProfile.Role == "Test1");
            Assert.IsNotNull(userUser.UserName == "Test");
        }

        [TestMethod]
        public async Task Create_User_Test()
        {
            UserDTO user = new UserDTO
            {
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { OKPO = "okpo", UNP = "unp", Role = "role1", Transcript = "transcript" }
            };

            var result = await service.Create(user);
            var userResult = await database.UserManager.FindByNameAsync(user.ProfileUser.UNP);

            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(userResult);
            Assert.IsNotNull(userResult.OperatorProfile);
            Assert.IsTrue(userResult.UserName == "unp");
            Assert.IsNotNull(userResult.OperatorProfile.DateInSystem.Value.Date == DateTime.Now.Date);
        }

        [TestMethod]
        public async Task Delete_User_Test()
        {
            UserDTO user = new UserDTO
            {
                Id = "aaa",
                UserName = "Test1",
                ProfileUser = new ProfileUserDTO { OKPO = "okpo", UNP = "unp", Role = "role1", Transcript = "transcript", DateInSystem = DateTime.Now }
            };

            var result = await service.Delete(user);
            var userResult1 = await database.UserManager.FindByNameAsync("Test1");
            var userResult2 = await database.UserManager.FindByIdAsync("aaa");
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
                Id = "aaa",
                UserName = "Test1",
                Email = "newMail@mail.ru",
                ProfileUser = new ProfileUserDTO {Transcript = "newTranscript", OKPO = "okpoNew",
                                                    UNP = "unpNew", DateInSystem = new DateTime(2018, 1, 1),
                                                    UserInSystem = "User", UserEdit = "UserEdit"}
            };

            var updateResult = await service.Update(user);
            var findUser = await database.UserManager.FindByNameAsync(user.UserName);
          
            Assert.IsTrue(updateResult.Succedeed);
            Assert.IsTrue(findUser.Email == "newMail@mail.ru");
            Assert.IsTrue(findUser.OperatorProfile.Transcript == "newTranscript");
            Assert.IsTrue(findUser.OperatorProfile.DateInSystem.Value.Date == new DateTime(2018, 1, 1).Date);
            Assert.IsTrue(findUser.OperatorProfile.DateEdit.Value.Date == DateTime.Now.Date);
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

            var result = await service.Create(user);

            var claim = await service.Authenticate(user);
            var controlUser = await database.UserManager.FindAsync(user.UserName, user.Password);
          
            Assert.IsNotNull(claim);
            Assert.IsNotNull(controlUser);
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

            Assert.IsNull(claim);
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
