using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data
{
    public class WrDbContext : DbContext
    {
        public WrDbContext(DbContextOptions<WrDbContext> options) : base(options)
        {
            Debug.WriteLine($"{ContextId} context created.");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var table = entity.GetTableName();
                if (table != null)
                    entity.SetTableName(table.ToUpper());
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToUpper());
                }
            }
        }

        //Register Models
        public DbSet<User> Users { get; set; }
        public DbSet<Protocol> Protocols { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Report> Reports { get; set; } 
        public DbSet<Visual> Visuals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Role_Visual> Role_Visuals { get; set; }
        public DbSet<User_Role> User_Roles { get; set; }

    }

}
