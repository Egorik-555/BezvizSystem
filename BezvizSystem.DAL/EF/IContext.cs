using BezvizSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.EF
{
    public interface IContext : IDisposable
    {
        DbSet<OperatorProfile> OperatorProfiles { get; set; }
        DbSet<Visitor> Visitors { get; set; }
        DbSet<GroupVisitor> GroupsVisitor { get; set; }
        DbSet<Status> Statuses { get; set; }
        DbSet<Nationality> Nationalities { get; set; }
        DbSet<CheckPoint> CheckPoints { get; set; }
    }
}
