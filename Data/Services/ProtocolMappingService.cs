using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolMappingService: BaseService, IProtocolMappingService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolMappingService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ProtocolMapping>> GetProtocolMappings()
        {
            return await context.ProtocolMapping.ToListAsync();
        }

        public async Task<ProtocolMapping> SaveProtocolMapping(ProtocolMapping mapping)
        {
            context.Add(mapping);
            await context.SaveChangesAsync();
            return mapping;
        }
    }
}
