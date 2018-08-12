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

        public async Task<IVisitorState> GetStateByIdRecordAsync(int id)
        {
            IVisitorState result;

            var dispatch = await Database.XMLDispatches.SingleOrDefaultAsync(d => d.IdVisitor == id);

            if (dispatch.Status == Status.Recd)
                result = new RecdVisitorState(dispatch.Status, dispatch.Operation);
            else if (dispatch.Status == Status.Send)
                result = new SendVisitorState(dispatch.Status, dispatch.Operation);
            else result = new NewVisitorState(dispatch.Status, dispatch.Operation);

            return result;
        }
    
    }
}
