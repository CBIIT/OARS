using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models.Pharma;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Services.Abstract.Pharma;

namespace TheradexPortal.Data.Services.Pharma
{
    public class PharmaProtocolTacService : BaseService, IPharmaProtocolTacService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public PharmaProtocolTacService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        // Get all records
        public async Task<IEnumerable<ProtocolTac>> GetAllAsync()
        {
            return await context.Pharma_ProtocolTacs.OrderBy(dl => dl.StudyId.ToLower()).ToListAsync();
        }
    }
}
