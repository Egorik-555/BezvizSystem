using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Entities.Loging;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Repositories
{
    public class UserActivityManager : IRepository<UserActivity, int>
    {
        public BezvizContext Database { get; set; }

        public UserActivityManager(BezvizContext db)
        {
            Database = db;
        }

        public IEnumerable<UserActivity> GetAll()
        {
            return Database.UserActivities;
        }

        public UserActivity GetById(int id)
        {
            return Database.UserActivities.Find(id);
        }

        public Task<UserActivity> GetByIdAsync(int id)
        {
            return Database.UserActivities.FindAsync(id);
        }

        public UserActivity Create(UserActivity item)
        {
            var result = Database.UserActivities.Add(item);
            Database.SaveChanges();
            return result;
        }

        public UserActivity Delete(int id)
        {
            var item = GetById(id);
            UserActivity result = null;
            if (item != null)
            {
                result = Database.UserActivities.Remove(item);
                Database.SaveChanges();
            }
            return result;
        }

        public UserActivity Update(UserActivity item)
        {
            Database.Entry(item).State = EntityState.Modified;
            Database.SaveChanges();
            return item;
        }

        public void Dispose()
        {
            Database.Dispose();
        }    
    }
}
