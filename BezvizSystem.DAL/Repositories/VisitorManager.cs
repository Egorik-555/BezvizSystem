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
            List<Visitor> list = Database.Visitors.ToList();
            foreach(var item in list)
            {              
                if (item.XML == null)
                {
                    item.State = new NewVisitorState();
                    continue;
                }

                if (item.XML.Status == Status.Recd)
                {
                    item.State = new RecdVisitorState();
                }
                else if (item.XML.Status == Status.Send)
                {
                    item.State = new SendVisitorState();
                }
                else
                {
                    item.State = new NewVisitorState();
                }
            }

            return list;
        }

        public Visitor GetById(int id)
        {
            Visitor visitor = Database.Visitors.Find(id);
            if (visitor != null)
            {
                if (visitor.XML == null)
                {
                    visitor.State = new NewVisitorState();
                    return visitor;
                }

                if (visitor.XML.Status == Status.Recd)
                {
                    visitor.State = new RecdVisitorState();
                }
            }
            return visitor;
        }

        public Task<Visitor> GetByIdAsync(int id)
        {
            Task<Visitor> task = Task.Run(async () => 
            {
                Visitor visitor = await Database.Visitors.FindAsync(id);

                if (visitor != null)
                {
                    if (visitor.XML == null)
                    {
                        visitor.State = new NewVisitorState();
                        return visitor;
                    }

                    if (visitor.XML.Status == Status.Recd)
                    {
                        visitor.State = new RecdVisitorState();
                    }
                    else if (visitor.XML.Status == Status.Send)
                    {
                        visitor.State = new SendVisitorState();
                    }
                    else
                    {
                        visitor.State = new NewVisitorState();
                    }
                }

                return visitor;
            });
                    
            return task;
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
