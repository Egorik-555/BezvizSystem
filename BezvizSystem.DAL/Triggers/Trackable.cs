using EntityFramework.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Triggers
{
    public abstract class Trackable
    {
        public DateTime Inserted { get; private set; }
        public DateTime Updated { get; private set; }

        static Trackable()
        {
            Triggers<Trackable>.Inserting += entry => entry.Entity.Inserted = entry.Entity.Updated = DateTime.UtcNow;
            Triggers<Trackable>.Updating += entry => entry.Entity.Updated = DateTime.UtcNow;
        }
    }
}
