using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Interfaces;
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
        public DbSet<GroupVisitor> GroupsVisitors { get; set; }
        public DbSet<XMLDispatch> XMLDispatches { get; set; }

        // Dictionaries
        public DbSet<CheckPoint> CheckPoints { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Gender> Genders { get; set; }

    }

    public class Initializer : CreateDatabaseIfNotExists<BezvizContext>
    {
        protected override void Seed(BezvizContext context)
        {        
            List<Nationality> nationalities = new List<Nationality>
            {
               new Nationality {Code = 1, Name = "Польша", ShortName = "POL", Active = true },
               new Nationality {Code = 2, Name = "Германия", ShortName = "GER", Active = true },
               new Nationality {Code = 3, Name = "Латвия", ShortName = "LTV", Active = true }
            };
            context.Nationalities.AddRange(nationalities);

            List<CheckPoint> checkPoints = new List<CheckPoint>
            {
               new CheckPoint { Name = "Брест (Тересполь)", Active = true},
               new CheckPoint { Name = "Домачево (Словатичи)", Active = true},
               new CheckPoint { Name = "Песчатка (Половцы)", Active = true},
               new CheckPoint { Name = "Переров (Беловежа)", Active = true},
               new CheckPoint { Name = "Аэропорт Брест", Active = true}
            };
            context.CheckPoints.AddRange(checkPoints);

           List<Gender> genders = new List<Gender>
            {
               new Gender {Code = 1, Name = "Мужчина", Active = true},
               new Gender {Code = 2, Name = "Женщина", Active = true}
            };
            context.Genders.AddRange(genders);

            context.SaveChanges();
        }
    }
}
