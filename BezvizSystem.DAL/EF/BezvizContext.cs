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
        public DbSet<CheckPoint> CheckPoints { get; set; }
    }

    public class Initializer : CreateDatabaseIfNotExists<BezvizContext>
    {
        protected override void Seed(BezvizContext context)
        {
            List<Status> statuses = new List<Status>
            {
               new Status {Code = 1, Name = "Сохранено" },              
               new Status {Code = 2, Name = "Отправлено в пограничную службу" },
               new Status {Code = 3, Name = "Принято пограничной службой" }
            };           
            context.Statuses.AddRange(statuses);

            List<Nationality> nationalities = new List<Nationality>
            {
               new Nationality {Code = 1, Name = "Польша", ShortName = "PL" },
               new Nationality {Code = 2, Name = "Германия", ShortName = "GER" },
               new Nationality {Code = 3, Name = "Латвия", ShortName = "LTV" }
            };
            context.Nationalities.AddRange(nationalities);

            List<CheckPoint> checkPoints = new List<CheckPoint>
            {
               new CheckPoint { Name = "Брест (Тересполь)"},
               new CheckPoint { Name = "Домачево (Словатичи)"},
               new CheckPoint { Name = "Песчатка (Половцы)"},
               new CheckPoint { Name = "Переров (Беловежа)"},
               new CheckPoint { Name = "Аэропорт Брест"}
            };
            context.CheckPoints.AddRange(checkPoints);

            context.SaveChanges();
        }
    }
}
