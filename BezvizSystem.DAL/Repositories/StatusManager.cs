using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Repositories
{
    public class StatusManager : IRepository<Status, int>
    {
        public BezvizContext Database { get; set; }

        public StatusManager(BezvizContext db)
        {
            Database = db;
        }

        public Status Create(Status item)
        {
            throw new NotImplementedException();
        }

        public Status Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<Status> GetAll()
        {
            return Database.Statuses;
        }

        public Status GetById(int id)
        {
            return Database.Statuses.Find(id);
        }

        public Task<Status> GetByIdAsync(int id)
        {
            return Database.Statuses.FindAsync(id);
        }

        public Status Update(Status item)
        {
            throw new NotImplementedException();
        }
    }
}
