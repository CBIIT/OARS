using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
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
            var converter = new BoolToStringConverter("n","Y");

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var table = entity.GetTableName();
                if (table != null)
                    entity.SetTableName(table.ToUpper());
                foreach (var property in entity.GetProperties())
                {
                    var colName = property.GetColumnName();
                    property.SetColumnName(colName.ToUpper());
                    if(property.ClrType.Name == "bool" || property.ClrType.Name == "Boolean")
                    {
                        property.SetValueConverter(converter);
                    }
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
        public DbSet<RoleVisual> Role_Visuals { get; set; }
        public DbSet<UserRole> User_Roles { get; set; }
        public DbSet<RoleDashboard> Role_Dashboards { get; set; } 
        public DbSet<RoleReport> Role_Reports { get; set; }
        public DbSet<UserProtocol> User_Protocols { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupProtocol> Group_Protocols { get; set; }
        public DbSet<UserGroup> User_Groups { get; set; }
        public DbSet<UserProtocolHistory> User_ProtocolHistory { get; set; }

    }

}
