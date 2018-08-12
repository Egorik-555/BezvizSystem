using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.StateVisitor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Repositories
{
    public class VisitorManager : IRepository<Visitor, int>
    {
        public BezvizContext Database { get; set; }

        public VisitorManager(BezvizContext db)
        {
            Database = db;
        }

        public IEnumerable<Visitor> GetAll()
        {          
            return Database.Visitors;
        }

        public Visitor GetById(int id)
        {
            return Database.Visitors.Find(id); ;
        }

        public Task<Visitor> GetByIdAsync(int id)
        {
            return Database.Visitors.FindAsync(id);
        }

        public Visitor Create(Visitor item)
        {
            var result = Database.Visitors.Add(item);
            Database.SaveChanges();
            return result;
        }

        public Visitor Delete(int id)
        {
            var item = GetById(id);
            Visitor result = null;
            if (item != null)
            {
                result = Database.Visitors.Remove(item);
                Database.SaveChanges();
            }
            return result;
        }

        public Visitor Update(Visitor item)
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
