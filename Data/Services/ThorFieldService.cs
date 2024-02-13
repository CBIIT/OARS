using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ThorFieldService : BaseService, IThorFieldService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ThorFieldService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ThorFieldType>> GetFieldTypes()
        {
            return await context.THORFieldType.OrderBy(c => c.FieldTypeName).ToListAsync();
        }

        public async Task<IList<ThorField>> GetFields()
        {
            return await context.THORField.OrderBy(c => c.SortOrder).ToListAsync();
        }
        
        public async Task<bool> SaveField(ThorField field)
        {
            DateTime curDateTime = DateTime.UtcNow;
            try
            {
                field.UpdateDate = curDateTime;
                context.Add(field);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
