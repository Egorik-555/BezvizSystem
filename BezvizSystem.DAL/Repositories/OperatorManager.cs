using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Repositories
{
    public class OperatorManager : IRepository<OperatorProfile, string>
    {
        public BezvizContext Database { get; set; }

        public OperatorManager(BezvizContext db)
        {
            Database = db;
        }

        public IEnumerable<OperatorProfile> GetAll()
        {
            return Database.OperatorProfiles;
        }

        public OperatorProfile GetById(string id)
        {
            return Database.OperatorProfiles.Find(id);
        }

        public Task<OperatorProfile> GetByIdAsync(string id)
        {
            return Database.OperatorProfiles.FindAsync(id);
        }

        public OperatorProfile Create(OperatorProfile item)
        {
            var result = Database.OperatorProfiles.Add(item);
            Database.SaveChanges();
            return result;
        }

        public OperatorProfile Delete(string id)
        {
            var item = GetById(id);
            OperatorProfile result = null;
            if (item != null)
            {
                result = Database.OperatorProfiles.Remove(item);
                Database.SaveChanges();
            }
            return result;        
        }

        public OperatorProfile Update(OperatorProfile item)
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
