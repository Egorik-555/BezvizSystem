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

    public class NationalityManager : IRepository<Nationality, int>
    {
        public BezvizContext Database { get; set; }

        public NationalityManager(BezvizContext db)
        {
            Database = db;
        }

        public Nationality Create(Nationality item)
        {
            throw new NotImplementedException();
        }

        public Nationality Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<Nationality> GetAll()
        {
            return Database.Nationalities;
        }

        public Nationality GetById(int id)
        {
            return Database.Nationalities.Find(id);
        }

        public Task<Nationality> GetByIdAsync(int id)
        {
            return Database.Nationalities.FindAsync(id);
        }

        public Nationality Update(Nationality item)
        {
            throw new NotImplementedException();
        }
    }
}
