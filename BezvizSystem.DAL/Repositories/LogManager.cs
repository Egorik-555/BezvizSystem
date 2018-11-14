using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Repositories
{
    public class LogManager : IRepository<Log, int>
    {
        public BezvizContext Database { get; set; }

        public LogManager(BezvizContext db)
        {
            Database = db;
        }

        public IEnumerable<Log> GetAll()
        {          
            return Database.Logs;
        }

        public Log GetById(int id)
        {
            return Database.Logs.Find(id);
        }

        public Task<Log> GetByIdAsync(int id)
        {
            return Database.Logs.FindAsync(id);
        }

        public Log Create(Log item)
        {
            var result = Database.Logs.Add(item);
            Database.SaveChanges();
            return result;
        }

        public Log Delete(int id)
        {
            var item = GetById(id);
            Log result = null;
            if (item != null)
            {
                result = Database.Logs.Remove(item);
                Database.SaveChanges();
            }
            return result;
        }

        public Log Update(Log item)
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
