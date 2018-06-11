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

    public class CheckPointManager : IRepository<CheckPoint, int>
    {
        private BezvizContext Database { get; set; }

        public CheckPointManager(BezvizContext db)
        {
            Database = db;
        }

        public CheckPoint Create(CheckPoint item)
        {
            throw new NotImplementedException();
        }

        public CheckPoint Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CheckPoint Update(CheckPoint item)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<CheckPoint> GetAll()
        {
            return Database.CheckPoints;
        }

        public CheckPoint GetById(int id)
        {
            return Database.CheckPoints.Find(id);
        }

        public Task<CheckPoint> GetByIdAsync(int id)
        {
            return Database.CheckPoints.FindAsync(id);
        }      
    }
}
