using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
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
            return await context.PharmaNscTacs.ToListAsync();
        }

        // Get a single record by ID
        public async Task<PharmaNscTac> GetByIdAsync(string protocolNumber)
        {
            return await context.PharmaNscTacs
                .FirstOrDefaultAsync(e => e.ProtocolNumber == protocolNumber);
        }

        // Add a new record
        public async Task AddAsync(PharmaNscTac entity, int userId)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await context.PharmaNscTacs.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        // Update an existing record
        public async Task UpdateAsync(PharmaNscTac entity, int userId)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingEntity = await context.PharmaNscTacs
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

            context.PharmaNscTacs.Update(existingEntity);
            await context.SaveChangesAsync();
        }

        // Delete 
        public async Task DeleteAsync(string protocolNumber)
        {
            var entity = await context.PharmaNscTacs 
                .FirstOrDefaultAsync(e => e.ProtocolNumber == protocolNumber);

            if (entity == null)
            {
                throw new KeyNotFoundException("Record not found.");
            }

            context.PharmaNscTacs.Remove(entity);
            await context.SaveChangesAsync();
        }

        public Tuple<bool, string> DeleteAsync(string protocolNumber, int userId)
        {
            try
            {
                var primaryTable = context.Model.FindEntityType(typeof(PharmaNscTac)).ToString().Replace("EntityType: ", "");
                var pharmaNscTac = context.PharmaNscTacs.Where(g => g.ProtocolNumber == protocolNumber).First();
                context.Remove(pharmaNscTac);
                context.SaveChangesAsync(userId, primaryTable);
                return new Tuple<bool, string>(true, "Pharma Nsc Tac deleted successfully");
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return new Tuple<bool, string>(false, "Failed to delete Pharma Nsc Tac");
            }
        }

        public Tuple<bool, string> DeleteAsync(int id , int userId)
        {
            try
            {
                var primaryTable = context.Model.FindEntityType(typeof(PharmaNscTac)).ToString().Replace("EntityType: ", "");
                var pharmaNscTac = context.PharmaNscTacs.Where(g => g.Id== id).First();
                context.Remove(pharmaNscTac);
                context.SaveChangesAsync(userId, primaryTable);
                return new Tuple<bool, string>(true, "Pharma Nsc Tac deleted successfully");
            }
            catch (Exception ex)
            {
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

            return await context.PharmaNscTacs
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

            return await context.PharmaNscTacs
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

            return await context.PharmaNscTacs
                .Where(e => e.TrtAsgnmtCode == trtAsgnmtCode)
                .ToListAsync();
        }
    }
}
