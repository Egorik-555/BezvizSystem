using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Entities.Log;
using BezvizSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Repositories
{

    public class TypeOfOperationManager : IRepository<TypeOfOperation, int>
    {
        private BezvizContext Database { get; set; }

        public TypeOfOperationManager(BezvizContext db)
        {
            Database = db;
        }

        public TypeOfOperation Create(TypeOfOperation item)
        {
            throw new NotImplementedException();
        }

        public TypeOfOperation Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TypeOfOperation Update(TypeOfOperation item)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<TypeOfOperation> GetAll()
        {
            return Database.TypeOfOperations;
        }

        public TypeOfOperation GetById(int id)
        {
            return Database.TypeOfOperations.Find(id);
        }

        public Task<TypeOfOperation> GetByIdAsync(int id)
        {
            return Database.TypeOfOperations.FindAsync(id);
        }      
    }
}
