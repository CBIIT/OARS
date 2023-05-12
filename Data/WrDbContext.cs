using Microsoft.EntityFrameworkCore;
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

    }

}
