﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;
using OARS.Data.Models;
using OARS.Data.Models.Pharma;
using OARS.Data.Static;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OARS.Data
{
    public class ThorDBContext : DbContext
    {
        private readonly ILogger<ThorDBContext> logger; // Add logger field
        public ThorDBContext(ILogger<ThorDBContext> logger, DbContextOptions<ThorDBContext> options) : base(options)
        {
            this.logger = logger; // Initialize logger
            Debug.WriteLine($"{ContextId} context created.");
            logger.LogInformation($"{ContextId} context created.");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Start a stopwatch to measure the time taken
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var converter = new BoolToStringConverter("N", "Y");

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var schema = entity.GetSchema();
                if (schema != null && schema == "DMU")
                {
                    foreach (var property in entity.GetProperties())
                    {
                        if (property.ClrType.Name == "bool" || property.ClrType.Name == "Boolean")
                        {
                            property.SetValueConverter(converter);
                        }
                    }
                    // This will make sure that the table name is in uppercase and the column names are in uppercase

                    // Convert table names to uppercase to ignore case sensitivity
                    entity.SetTableName(entity.GetTableName().ToUpper());

                    // Convert column names to uppercase to ignore case sensitivity
                    foreach (var property in entity.GetProperties())
                    {
                        property.SetColumnName(property.GetColumnName().ToUpper());
                    }
                    continue;
                }

                var table = entity.GetTableName();
                if (table != null)
                    entity.SetTableName(table.ToUpper());
                foreach (var property in entity.GetProperties())
                {
                    var colName = property.GetColumnName();
                    property.SetColumnName(colName.ToUpper());
                    if (property.ClrType.Name == "bool" || property.ClrType.Name == "Boolean")
                    {
                        property.SetValueConverter(converter);
                    }
                }
                // Configure entity as keyless if no primary key is detected
                if (!entity.GetKeys().Any())
                {
                    modelBuilder.Entity(entity.ClrType).HasNoKey();
                }
            }
            //modelBuilder.Entity<Models.Pharma.CdrdmStudyAgent>().HasKey(d => new { d.DocumentNumber, d.Nsc});
            modelBuilder.Entity<Models.Pharma.ProtocolTac>().HasKey(d => new { d.StudyId, d.TrtAsgnmtCode});

            // Stop the stopwatch and log the time taken
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            logger.LogInformation("OnModelCreating executed in {ElapsedMilliseconds} ms.", elapsedMilliseconds);
        }

        public virtual async Task<int> SaveChangesAsync(int userId, string primaryTable)
        {
            // Start a stopwatch to measure the time taken
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            DateTime auditDateTime = DateTime.UtcNow;
            var auditEntries = OnBeforeSaveChanges(userId, auditDateTime, primaryTable);
            var result = await base.SaveChangesAsync();
            await OnAfterSaveChanges(auditEntries);

            // Stop the stopwatch and log the time taken
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            logger.LogInformation("SaveChangesAsync executed in {ElapsedMilliseconds} ms.", elapsedMilliseconds);

            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges(int userId, DateTime auditDateTime, string primaryTable)
        {
            // Start a stopwatch to measure the time taken
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntry.CreateDate = auditDateTime;
                auditEntry.IsPrimaryTable = (auditEntry.TableName == primaryTable);
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;

                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            // Save audit entities that have all the modifications
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                Audit.Add(auditEntry.ToAudit());
            }

            // Stop the stopwatch and log the time taken
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            logger.LogInformation("OnBeforeSaveChanges executed in {ElapsedMilliseconds} ms.", elapsedMilliseconds);

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }
        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            // Start a stopwatch to measure the time taken
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry
                Audit.Add(auditEntry.ToAudit());
            }

            // Stop the stopwatch and log the time taken
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            logger.LogInformation("OnAfterSaveChanges executed in {ElapsedMilliseconds} ms.", elapsedMilliseconds);

            return SaveChangesAsync();
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
        public DbSet<ThorAlert> Alerts { get; set; }
        public DbSet<RoleDashboard> Role_Dashboards { get; set; }
        public DbSet<RoleReport> Role_Reports { get; set; }
        public DbSet<UserProtocol> User_Protocols { get; set; }
        public DbSet<UserSelectedProtocols> User_Selected_Protocols { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupProtocol> Group_Protocols { get; set; }
        public DbSet<UserGroup> User_Groups { get; set; }
        public DbSet<UserProtocolHistory> User_ProtocolHistory { get; set; }
        public DbSet<UserActivityLog> User_ActivityLog { get; set; }
        public DbSet<ContactUsCategory> ContactUsCategory { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<Audit> Audit { get; set; }
        public DbSet<UserFavorite> User_Favorite { get; set; }
        public DbSet<ThorCategory> THORDataCategory { get; set; }
        public DbSet<ThorFieldType> THORFieldType { get; set; }
        public DbSet<ThorField> THORField { get; set; }
        public DbSet<ThorDictionary> THORDictionary { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProtocolMapping> ProtocolMapping { get; set; }
        public DbSet<ProtocolMappingStatus> ProtocolMappingStatus { get; set; }
        public DbSet<ProfileDataCategory> ProfileDataCategory { get; set; }
        public DbSet<ProtocolDataSystem> ProtocolDataSystem { get; set; }
        public DbSet<ProtocolField> ProtocolField { get; set; }
        public DbSet<ProfileField> ProfileFields { get; set; }
        public DbSet<ProtocolEDCField> ProtocolEDCField { get; set; }
        public DbSet<ProtocolEDCForm> ProtocolEDCForm { get; set; }
        public DbSet<ProtocolEDCDictionary> ProtocolEDCDictionary { get; set; }
        public DbSet<ProtocolPhase> ProtocolPhases { get; set; }
        public DbSet<ProtocolEDCForm> ProtocolEDCForms { get; set; }
        public DbSet<ProtocolAgent> ProtocolAgents { get; set; }
        public DbSet<ProtocolSubGroup> ProtocolSubGroups { get; set; }
        public DbSet<Models.ProtocolTac> ProtocolTacs { get; set; }
        public DbSet<ProtocolDisease> ProtocolDiseases { get; set; }
        public DbSet<ProtocolDataCategory> ProtocolDataCategories { get; set; }
        public DbSet<ProtocolCategoryStatus> ProtocolCategoryStatus { get; set; }
        public DbSet<ProtocolDictionaryMapping> ProtocolDictionaryMapping { get; set; }
        public DbSet<ProtocolFieldMapping> ProtocolFieldMappings { get; set; }

        public DbSet<ProtocolFormMapping> ProtocolFormMappings { get; set; }
        public DbSet<CrossoverOption> CrossoverOptions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewHistory> ReviewHistories { get; set; }
        public DbSet<ReviewHistoryEmail> ReviewHistoryEmails { get; set; }
        public DbSet<ReviewHistoryItem> ReviewHistoryItems { get; set; }
        public DbSet<ReviewHistoryNote> ReviewHistoryNotes { get; set; }
        public DbSet<ReviewItem> ReviewItems { get; set; }
        public DbSet<ReportFilter> ReportFilters { get; set; }
        public DbSet<ReportFilterItem> ReportFilterItems { get; set; }

        public DbSet<Audit> Audits { get; set; }
        public DbSet<Models.Pharma.PharmaNscTac> Pharma_PharmaNscTacs { get; set; }
        public DbSet<Models.Pharma.CdrdmStudyAgent> Pharma_CdrdmStudyAgent{ get; set; }
        public DbSet<Models.Pharma.ProtocolTac> Pharma_ProtocolTacs { get; set; }

    }
}
