using BezvizSystem.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BezvizSystem.DAL.EF
{
    public class BezvizContext : IdentityDbContext<BezvizUser>
    {
        static BezvizContext()
        {
            Database.SetInitializer(new Initializer());
        }

        public BezvizContext(string connection)
            : base(connection)
        {

        }

        public DbSet<OperatorProfile> OperatorProfiles { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<GroupVisitor> GroupsVisitor { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
    }

    public class Initializer : CreateDatabaseIfNotExists<BezvizContext>
    {
        protected override void Seed(BezvizContext context)
        {
            List<Status> list = new List<Status>
            {
               new Status {Code = 1, Name = "Сохранено" },              
               new Status {Code = 2, Name = "Отправлено в пограничную службу" },
               new Status {Code = 3, Name = "Принято пограничной службой" }
            };
            
            context.Statuses.AddRange(list);

            List<Nationality> nationalities = new List<Nationality>
            {
               new Nationality {Code = 1, Name = "Беларусь", ShortName = "BLR" },
               new Nationality {Code = 2, Name = "Польша", ShortName = "PL" },
               new Nationality {Code = 3, Name = "Латвия", ShortName = "LTV" }
            };

            context.SaveChanges();
        }
    }
}
