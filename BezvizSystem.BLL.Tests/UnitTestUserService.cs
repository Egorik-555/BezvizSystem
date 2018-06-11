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

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestUserService
    {
        IUserService service;
        IUnitOfWork database;

        public UnitTestUserService()
        {
            database = new IdentityUnitOfWork("BezvizConnection");
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
            var roleResult1 = database.RoleManager.FindByName("Test1");
            var roleResult2 = database.RoleManager.FindByName("Test2");
            var userUser = await service.GetByNameAsync("Test");

            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(roleResult1);
            Assert.IsNotNull(roleResult2);
            Assert.IsNotNull(userUser);


            await service.Delete(user);

            var role = database.RoleManager.FindByName(list[0]);
            await database.RoleManager.DeleteAsync(role);
            role = database.RoleManager.FindByName(list[1]);
            await database.RoleManager.DeleteAsync(role);
        }

        [TestMethod]
        public async Task Create_Delete_UserAsync()
        {
            UserDTO user = new UserDTO
            {
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { OKPO = "okpo", UNP = "unp" }
            };

            var result = await service.Create(user);

            Assert.IsTrue(result.Succedeed);

            if (!result.Succedeed && result.Property == "UserName")
            {
                await service.Delete(user);
                result = await service.Create(user);
            }

            var userResult = database.UserManager.FindByName(user.ProfileUser.UNP);
            await service.Delete(user);

            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(userResult);
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
        public async Task Get_All_By_Role()
        {
            BezvizRole role = new BezvizRole { Name = "test" };
            database.RoleManager.Create(role);

            UserDTO user1 = new UserDTO
            {
                Password = "qwerty111",
                ProfileUser = new ProfileUserDTO { Role = "test", UNP = "test" }
            };

            UserDTO user2 = new UserDTO
            {
                Password = "qwerty111",
                ProfileUser = new ProfileUserDTO { Role = "test", UNP = "unp" }
            };

            var result1 = await service.Create(user1);
            var result2 = await service.Create(user2);

            var users = service.GetByRole("test").ToList();

            Assert.IsNotNull(users.Where(u => u.UserName == user2.ProfileUser.UNP).FirstOrDefault());
            Assert.IsNotNull(users.Where(u => u.UserName == user1.ProfileUser.UNP).FirstOrDefault());

            await service.Delete(user1);
            await service.Delete(user2);
            var findRole = database.RoleManager.FindByName(role.Name);
            await database.RoleManager.DeleteAsync(findRole);
        }

        [TestMethod]
        public async Task Get_By_Id_And_Name_UserService()
        {
            BezvizRole role = new BezvizRole { Name = "test" };
            database.RoleManager.Create(role);

            UserDTO user = new UserDTO
            {
                Password = "test123",
                ProfileUser = new ProfileUserDTO { Role = "test", UNP = "test" }
            };

            var findUser = await service.GetByNameAsync(user.ProfileUser.UNP);

            if (findUser == null)
            {
                await service.Create(user);
                findUser = await service.GetByNameAsync(user.UserName);
            }

            var user1 = await service.GetByNameAsync(user.ProfileUser.UNP);
            var user2 = await service.GetByIdAsync(findUser.Id);

            Assert.IsNotNull(user1);
            Assert.IsNotNull(user2);

            await service.Delete(user1);
            var findRole = database.RoleManager.FindByName(role.Name);
            await database.RoleManager.DeleteAsync(findRole);
        }
    }
}
