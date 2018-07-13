using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Entities.Log;
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
        public DbSet<GroupVisitor> GroupsVisitor { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }

        // Dictionaries
        public DbSet<CheckPoint> CheckPoints { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<TypeOfOperation> TypeOfOperations { get; set; }

        
    }

    public class Initializer : CreateDatabaseIfNotExists<BezvizContext>
    {
        protected override void Seed(BezvizContext context)
        {
            List<Status> statuses = new List<Status>
            {
               new Status {Code = 1, Name = "Сохранено", Active = true },              
               new Status {Code = 2, Name = "Отправлено в пограничную службу", Active = true },
               new Status {Code = 3, Name = "Принято пограничной службой", Active = true }
            };           
            context.Statuses.AddRange(statuses);

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

            List<TypeOfOperation> operations = new List<TypeOfOperation>
            {
               new TypeOfOperation {Code = 1, Name = "Вход", Active = true},
               new TypeOfOperation {Code = 2, Name = "Выход", Active = true},
               new TypeOfOperation {Code = 3, Name = "Выгрузка данных", Active = true}            
            };
            context.TypeOfOperations.AddRange(operations);

            context.SaveChanges();
        }
    }
}
