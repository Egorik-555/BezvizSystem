using BezvizSystem.DAL;
using BezvizSystem.DAL.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Tests.TestServises
{
    public class UserManagerTest : BezvizUserManager
    {
        IEnumerable<BezvizUser> list;

        public UserManagerTest(IEnumerable<BezvizUser> list, IUserStore<BezvizUser> store)
            : base(store)
        {
            this.EmailService = new EmailService();
            this.list = list;
        }

        public override IQueryable<BezvizUser> Users => list.AsQueryable();

        public override Task<IdentityResult> CreateAsync(BezvizUser user, string password)
        {          
            return Task.Run(() => { list = list.Concat(new List<BezvizUser> { user }); return IdentityResult.Success; } );            
        }

        public override Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            return Task.Run(() => new IdentityResult());
        }

        public override Task<BezvizUser> FindByIdAsync(string userId)
        {
            return Task.Run(() => list.Where(u => u.Id == userId).FirstOrDefault());
        }

        public override Task<BezvizUser> FindByNameAsync(string userName)
        {
            return Task.Run(() => list.Where(u => u.UserName == userName).FirstOrDefault());
        }

        public override Task<IdentityResult> DeleteAsync(BezvizUser user)
        {
            var listNew = list.ToList();
            listNew.Remove(user);
            return Task.Run(() => {  list = listNew.AsEnumerable(); return IdentityResult.Success; });
        }
    }
}
