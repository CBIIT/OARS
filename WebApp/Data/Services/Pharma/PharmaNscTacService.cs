using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OARS.Data.Models;
using OARS.Data.Models.Pharma;
using OARS.Data.Services.Abstract;
using OARS.Data.Services.Abstract.Pharma;

namespace OARS.Data.Services.Pharma
{
    public class PharmaNscTacService : BaseService, IPharmaNscTacService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public PharmaNscTacService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        // Get all records
        public async Task<IEnumerable<PharmaNscTac>> GetAllAsync()
        {
            return await context.Pharma_PharmaNscTacs.ToListAsync();
        }

        // Get a single record by ID
        public async Task<PharmaNscTac> GetByIdAsync(int id)
        {
            return await context.Pharma_PharmaNscTacs.FirstOrDefaultAsync(e => e.Id == id);
        }
        // Get a single record by Protocol Number
        public async Task<PharmaNscTac> GetByProtocolNumberAsync(string protocolNumber)
        {
            return await context.Pharma_PharmaNscTacs.FirstOrDefaultAsync(e => e.ProtocolNumber == protocolNumber);
        }
        // Add a new record
        public async Task<bool> AddAsync(PharmaNscTac entity, int userId)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            try
            {

                entity.Created = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                context.Pharma_PharmaNscTacs.Add(entity);
                var primaryTable = context.Model.FindEntityType(typeof(PharmaNscTac)).ToString().Replace("EntityType: ", "");
                var status = await context.SaveChangesAsync(userId, primaryTable);
                return true;

            }
            catch (Exception ex)
            {
                // Log the error or handle it as per your logging mechanism
                Console.WriteLine($"Error inserting record: {ex.Message}");
                return false;
            }
        }



        // Update an existing record
        public async Task UpdateAsync(PharmaNscTac entity, int userId)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingEntity = await context.Pharma_PharmaNscTacs
                .FirstOrDefaultAsync(e => e.ProtocolNumber == entity.ProtocolNumber);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException("Record not found.");
            }

            // Update properties
            existingEntity.TrtAsgnmtCode = entity.TrtAsgnmtCode;
            existingEntity.TrtAsgnmtDescription = entity.TrtAsgnmtDescription;
            existingEntity.Nsc = entity.Nsc;
            existingEntity.AgreementNumber = entity.AgreementNumber;

            context.Pharma_PharmaNscTacs.Update(existingEntity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> CanDelete(int id)
        {
            return context.Pharma_PharmaNscTacs.Where(ug => ug.Id == id).Count() == 1;
        }

        // Delete 
        public async Task<Tuple<bool, string>> DeleteAsync(int id, int userId, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                try
                {
                    var primaryTable = context.Model.FindEntityType(typeof(PharmaNscTac)).ToString().Replace("EntityType: ", "");
                    var pharmaNscTac = context.Pharma_PharmaNscTacs.Where(g => g.Id == id).First();
                    
                    if (pharmaNscTac == null)
                        return new Tuple<bool, string>(false, "Record not found"); // Record not found

                    context.Remove(pharmaNscTac);

                    await context.SaveChangesAsync(userId, primaryTable);

                    return new Tuple<bool, string>(true, "Pharma Nsc Tac deleted successfully");
                }
                catch (Exception ex)
                {
                    _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                    return new Tuple<bool, string>(false, "Failed to delete Pharma Nsc Tac");
                }
            }

            try
            {
                var record = await context.Pharma_PharmaNscTacs.FindAsync(id);

                if (record == null)
                    return new Tuple<bool, string>(false, "Record not found"); // Record not found

                record.IsDeleted = true;
                record.IsActive = false; // Mark inactive if applicable
                record.Deleted = DateTime.UtcNow; // Set deletion timestamp

                context.Pharma_PharmaNscTacs.Update(record);
                var primaryTable = context.Model.FindEntityType(typeof(PharmaNscTac)).ToString().Replace("EntityType: ", "");
                await context.SaveChangesAsync(userId, primaryTable);
                return new Tuple<bool, string>(true, "Pharma Nsc Tac deleted successfully");
            }
            catch (Exception ex)
            {
                // Log or handle the error
                Console.WriteLine($"Error deleting record: {ex.Message}");
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return new Tuple<bool, string>(false, "Failed to delete Pharma Nsc Tac");
            }
        }

        // Get records by AgreementNumber
        public async Task<IEnumerable<PharmaNscTac>> GetByAgreementNumberAsync(string agreementNumber)
        {
            if (string.IsNullOrWhiteSpace(agreementNumber))
            {
                throw new ArgumentException("AgreementNumber cannot be null or empty.", nameof(agreementNumber));
            }

            return await context.Pharma_PharmaNscTacs
                .Where(e => e.AgreementNumber == agreementNumber)
                .ToListAsync();
        }

        // Get records by NSC
        public async Task<IEnumerable<PharmaNscTac>> GetByNscAsync(string nsc)
        {
            if (string.IsNullOrWhiteSpace(nsc))
            {
                throw new ArgumentException("NSC cannot be null or empty.", nameof(nsc));
            }

            return await context.Pharma_PharmaNscTacs
                .Where(e => e.Nsc == nsc)
                .ToListAsync();
        }

        // Get records by Treatment Assignment Code
        public async Task<IEnumerable<PharmaNscTac>> GetByTrtAsgnmtCodeAsync(string trtAsgnmtCode)
        {
            if (string.IsNullOrWhiteSpace(trtAsgnmtCode))
            {
                throw new ArgumentException("Treatment Assignment Code cannot be null or empty.", nameof(trtAsgnmtCode));
            }

            return await context.Pharma_PharmaNscTacs
                .Where(e => e.TrtAsgnmtCode == trtAsgnmtCode)
                .ToListAsync();
        }

        public async Task<bool> IsUniqueCombinationAsync(string agreementNumber, string nsc, string protocolNumber, string trtAsgnmtCode)
        {
            return !await context.Pharma_PharmaNscTacs.AnyAsync(tac =>
                tac.AgreementNumber == agreementNumber &&
                tac.Nsc == nsc &&
                tac.ProtocolNumber == protocolNumber &&
                tac.TrtAsgnmtCode == trtAsgnmtCode);
        }
    }
}
