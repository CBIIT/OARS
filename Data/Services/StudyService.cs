namespace TheradexPortal.Data.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Security.Permissions;
    using TheradexPortal.Data.Models;
    using TheradexPortal.Data.ViewModels;

    public class StudyService : BaseService
    {
        public StudyService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }
        
        public async Task<IList<Protocol>> GetAllProtocolsAsync()
        {
            return await context.Protocols.ToListAsync();
        }
    }
}