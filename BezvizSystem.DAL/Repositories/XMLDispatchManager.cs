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
    public class XMLDispatchManager : IRepositoryXMLDispatch<XMLDispatch, int>
    {
        private BezvizContext Database { get; set; }

        public XMLDispatchManager(BezvizContext db)
        {
            Database = db;
        }

        public XMLDispatch Create(XMLDispatch item)
        {
            var result = Database.XMLDispatches.Add(item);
            Database.SaveChanges();
            return result;
        }

        public XMLDispatch Delete(int id)
        {
            var item = GetById(id);
            XMLDispatch result = null;
            if (item != null)
            {
                result = Database.XMLDispatches.Remove(item);
                Database.SaveChanges();
            }
            return result;
        }

        public XMLDispatch Update(XMLDispatch item)
        {
            Database.Entry(item).State = EntityState.Modified;
            Database.SaveChanges();
            return item;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<XMLDispatch> GetAll()
        {
            return Database.XMLDispatches;
        }

        public XMLDispatch GetById(int id)
        {
            return Database.XMLDispatches.Find(id);
        }

        public Task<XMLDispatch> GetByIdAsync(int id)
        {
            return Database.XMLDispatches.FindAsync(id);
        }

        public Status GetStatusByIdRecord(int id)
        {
            //var allItems = Database.XMLDispatches.Where(i => i.IdVisitor == id);

            //// all items with status recd and all operations done
            //var itemsRecd = allItems.Where(i => i.Status == Status.Recd && i.Operation == Operation.Done);
            //// all items with status send and all operations done
            //var itemsSend = allItems.Where(i => i.Status == Status.Send && i.Operation == Operation.Done);

            //if (itemsRecd.Count() == allItems.Count())
            //    return Status.Recd;
            //else if (itemsSend.Count() == allItems.Count())
            //    return Status.Send;
            //else return Status.New;
            return Status.New;
        }
    }
}
