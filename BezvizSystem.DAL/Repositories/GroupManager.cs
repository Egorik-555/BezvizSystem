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
    public class GroupManager : IRepository<GroupVisitor, int>
    {
        public BezvizContext Database { get; set; }

        public GroupManager(BezvizContext db)
        {
            Database = db;
        }

        public IEnumerable<GroupVisitor> GetAll()
        {
            return Database.GroupsVisitors;
        }

        public GroupVisitor GetById(int id)
        {
            return Database.GroupsVisitors.Find(id);
        }

        public Task<GroupVisitor> GetByIdAsync(int id)
        {
            return Database.GroupsVisitors.FindAsync(id);
        }

        public GroupVisitor Create(GroupVisitor item)
        {
            var result = Database.GroupsVisitors.Add(item);
            Database.SaveChanges();
            return result;
        }

        public GroupVisitor Delete(int id)
        {
            var item = GetById(id);
            GroupVisitor result = null;
            if (item != null)
            {
                result = Database.GroupsVisitors.Remove(item);
                Database.SaveChanges();
            }
            return result;
        }

        public GroupVisitor Update(GroupVisitor item)
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
