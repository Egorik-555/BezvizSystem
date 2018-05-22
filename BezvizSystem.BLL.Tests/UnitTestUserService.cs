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

namespace BezvizSystem.BLL.Tests
{
    [TestClass]
    public class UnitTestUserService
    {
        IUserService service;
        IUnitOfWork db;

        public UnitTestUserService()
        {
            IServiceCreator creator = new ServiceCreator();
            service = creator.CreateUserService("BezvizDB");

            db = new IdentityUnitOfWork("BezvizDB");
        }

        [TestMethod]
        public async Task Init_DataAsync()
        {
            UserDTO user = new UserDTO
            {
                UserName = "Admin",
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { Role = "Admin" }
            };

            List<string> list = new List<string>
            {
                "Admin",
                "Operator"
            };

            await service.SetInitialData(user, list);
            var roleResult1 = db.RoleManager.FindByName("Admin");
            var roleResult2 = db.RoleManager.FindByName("Operator");
            var userUser = db.UserManager.FindByName("Admin");

            Assert.IsNotNull(roleResult1);
            Assert.IsNotNull(roleResult2);
            Assert.IsNotNull(userUser);

            await service.Delete(user);
        }

        [TestMethod]
        public async Task Create_Delete_UserAsync()
        {
            UserDTO user = new UserDTO
            {
                //UserName = "Egor",
                Password = "qwerty",
                ProfileUser = new ProfileUserDTO { Role = "Admin", UNP = "test" }
            };

            var result = await service.Create(user);

            if (!result.Succedeed && result.Property == "UserName")
            {
                await service.Delete(user);
                result = await service.Create(user);
            }

            var userResult = db.UserManager.FindByName(user.ProfileUser.UNP);
            await service.Delete(user);

            Assert.IsTrue(result.Succedeed);
            Assert.IsNotNull(userResult);
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
            var controlUser = await db.UserManager.FindAsync(user.UserName, user.Password);

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
            UserDTO user1 = new UserDTO
            {      
                Password = "qwerty111",
                ProfileUser = new ProfileUserDTO { Role = "operator", UNP = "test" }
            };

            UserDTO user2 = new UserDTO
            {              
                Password = "qwerty111",               
                ProfileUser = new ProfileUserDTO { Role = "operator", UNP = "unp" }
            };

            var result1 = await service.Create(user1);
            var result2 = await service.Create(user2);

            var users = service.GetByRole("Operator").ToList();

            Assert.IsNotNull(users.Where(u => u.UserName == user2.ProfileUser.UNP).FirstOrDefault());
            Assert.IsNotNull(users.Where(u => u.UserName == user1.ProfileUser.UNP).FirstOrDefault());

            await service.Delete(user1);
            await service.Delete(user2);
        }

        [TestMethod]
        public async Task Get_By_Id_And_Name_UserService()
        {
            UserDTO user = new UserDTO
            {
                UserName = "test",
                Password = "test123",
                ProfileUser = new ProfileUserDTO { Role = "operator", UNP = "test" }
            };

            var findUser = await service.GetByNameAsync(user.UserName);

            if (findUser == null)
            {
                await service.Create(user);
            }
                    
            var user1 = await service.GetByNameAsync(user.UserName);

            Assert.IsNotNull(user1);

            await service.Delete(user1);
            user1 = await service.GetByNameAsync(user1.UserName);
            Assert.IsNull(user1);
        }
    }
}
