using AutoMapper;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Mapper;
using BezvizSystem.DAL;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Identity;
using BezvizSystem.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BezvizSystem.BLL.Services
{

    public class UserService : IUserService
    {
        public BezvizUserManager ManagerForChangePass { get; set; }
        IUnitOfWork Database { get; set; }

        IMapper mapper;

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FromDALToBLLProfile(uow));
            }).CreateMapper();
        }

        //tested
        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            if (userDto.ProfileUser.UNP == null && userDto.UserName == null)
                return new OperationDetails(true, "Имя пользователя не указано", "");
            if (userDto.ProfileUser == null)
                return new OperationDetails(true, "Профайл пользователя не заполнен", "");

            if (userDto.UserName == null)
                userDto.UserName = userDto.ProfileUser.UNP;

            BezvizUser user = await Database.UserManager.FindByNameAsync(userDto.UserName);
            if (user == null)
            {
                try
                {
                    user = mapper.Map<UserDTO, BezvizUser>(userDto);
                    user.OperatorProfile.DateInSystem = DateTime.Now;
                    var result = await Database.UserManager.CreateAsync(user, userDto.Password);

                    if (result.Errors.Count() > 0)
                        return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                    // добавляем роль
                    if (userDto.ProfileUser.Role != null)
                        await Database.UserManager.AddToRoleAsync(user.Id, userDto.ProfileUser.Role);
                    await Database.SaveAsync();
                    return new OperationDetails(true, "Пользователь успешно создан", "");
                }
                catch (Exception ex)
                {
                    return new OperationDetails(false, ex.Message, "");
                }
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким именем уже существует", "UserName");
            }
        }

        //tested
        public async Task<OperationDetails> Delete(UserDTO userDto)
        {
            if (userDto.UserName == null)
                userDto.UserName = userDto.ProfileUser.UNP;

            BezvizUser user = await Database.UserManager.FindByNameAsync(userDto.UserName);
            if (user != null)
            {
                try
                {
                    Database.OperatorManager.Delete(user.Id);
                    var result = await Database.UserManager.DeleteAsync(user);
                    if (result.Succeeded)
                        return new OperationDetails(true, "Пользователь успешно удален", "");
                    else return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }
                catch (Exception ex)
                {
                    return new OperationDetails(false, ex.Message, "");
                }
            }
            else
            {
                return new OperationDetails(false, "Пользователь не найден", "UserName");
            }
        }

        //tested
        public async Task<OperationDetails> Update(UserDTO userDto)
        {
            try
            {
                var user = await Database.UserManager.FindByIdAsync(userDto.Id);
                if (user == null)
                    return new OperationDetails(false, "Пользователь не найден", "");
            
                var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfileWithModelUser(user))).CreateMapper();
                var m = mapper.Map<UserDTO, BezvizUser>(userDto);

                //if password is edited
                if (!String.IsNullOrEmpty(userDto.Password))
                {
                    var validPass = await Database.UserManager.PasswordValidator.ValidateAsync(userDto.Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = Database.UserManager.PasswordHasher.HashPassword(userDto.Password);
                    }
                    else return new OperationDetails(false, validPass.Errors.FirstOrDefault(), "");
                }
                //////

                //Database.OperatorManager.Update(m.OperatorProfile);
                m.OperatorProfile.DateEdit = DateTime.Now;
                var result = await Database.UserManager.UpdateAsync(m);
                return new OperationDetails(result.Succeeded, result.Succeeded ? "Пользователь успешно изменен" : result.Errors.First(), "");
            }

            catch (Exception e)
            {
                return new OperationDetails(false, e.Message, "");
            }
        }


        public async Task<OperationDetails> Registrate(UserDTO userDto, string callback, IGeneratePass generator)
        {
            string pass = generator.Generate();
            string message = CreateMessage(userDto, callback, pass);

            try
            {
                var user = await Database.UserManager.FindByIdAsync(userDto.Id);
                if (user == null)
                    return new OperationDetails(false, "Туроператор не найден", "");

                var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new FromDALToBLLProfileWithModelUser(user))).CreateMapper();
                var m = mapper.Map<UserDTO, BezvizUser>(userDto);

                var result = await Database.UserManager.UpdateAsync(m);

                if (!result.Succeeded)
                    return new OperationDetails(result.Succeeded, "Произошла проблема при обновлении Email пользователя", "");

                //sending mail
                await Database.UserManager.SendEmailAsync(user.Id, "Подтверждение электронной почты", message);

                string code = ManagerForChangePass.GeneratePasswordResetToken(user.Id);
                result = await ManagerForChangePass.ResetPasswordAsync(user.Id, code, pass);

                return new OperationDetails(result.Succeeded, result.Errors.Count() == 0 ? "Туроператор зарегистрирован" : result.Errors.First(), "");
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        private string CreateMessage(UserDTO user, string callBackUrl, string pass)
        {
            string message = "Логин: " + user.ProfileUser.UNP + "<br/>";
            message += "Пароль: " + pass + "<br/>";

            message += "Для завершения регистрации перейдите по ссылке - <a href=\""
                                           + callBackUrl + "\">завершить регистрацию.</a><br/>";
            message += "<p>&copy; " + DateTime.Now.Year + " - Без виз</p>";
            return message;
        }

        //tested
        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // find user
            BezvizUser user = await Database.UserManager.FindAsync(userDto.UserName, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        //tested
        // initialize base with 2 roles and 1 user - admin
        public async Task<OperationDetails> SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new BezvizRole { Name = roleName };
                    var result = await Database.RoleManager.CreateAsync(role);
                    if (!result.Succeeded)
                        return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }
            }
            return await Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        //tested
        public async Task<UserDTO> GetByIdAsync(string id)
        {
            var user = await Database.UserManager.FindByIdAsync(id);
            var userDto = mapper.Map<BezvizUser, UserDTO>(user);
            return userDto;
        }

        //tested
        public async Task<UserDTO> GetByNameAsync(string name)
        {
            var user = await Database.UserManager.FindByNameAsync(name);
            var userDto = mapper.Map<BezvizUser, UserDTO>(user);
            return userDto;
        }

        //tested
        public IEnumerable<UserDTO> GetAll()
        {
            var users = Database.UserManager.Users.ToList();
            return mapper.Map<IEnumerable<BezvizUser>, IEnumerable<UserDTO>>(users);
        }

        //tested
        public IEnumerable<UserDTO> GetByRole(string roleName)
        {
            if (roleName != null)
            {
                var users = GetAll();
                var result = users.Where(u => u.ProfileUser != null ? u.ProfileUser.Role.ToUpper() == roleName.ToUpper() : false);
                return result;
            }
            return null;
        }
    }
}
