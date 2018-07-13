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
    public class GenderManager : IRepository<Gender, int>
    {
        public BezvizContext Database { get; set; }

        public GenderManager(BezvizContext db)
        {
            Database = db;
        }

        public Gender Create(Gender item)
        {
            throw new NotImplementedException();
        }

        public Gender Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<Gender> GetAll()
        {
            return Database.Genders;
        }

        public Gender GetById(int id)
        {
            return Database.Genders.Find(id);
        }

        public Task<Gender> GetByIdAsync(int id)
        {
            return Database.Genders.FindAsync(id);
        }

        public Gender Update(Gender item)
        {
            throw new NotImplementedException();
        }
    }
}
