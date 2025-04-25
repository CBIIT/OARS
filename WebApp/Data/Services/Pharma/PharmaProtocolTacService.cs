using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OARS.Data.Models.Pharma;
using OARS.Data.Services.Abstract;
using OARS.Data.Services.Abstract.Pharma;

namespace OARS.Data.Services.Pharma
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
