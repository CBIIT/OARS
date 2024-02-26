using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolFieldService: BaseService, IProtocolFieldService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolFieldService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ProtocolField>> GetProtocolFieldsByMappingId(int mappingId)
        {
            return await context.ProtocolField.Where(pf => pf.ProtocolMappingId == mappingId && pf.ThorField.FieldType.FieldTypeName == "Date").ToListAsync(); //todo verify this logic
        }

        public async Task<bool> SaveProtocolField(ProtocolField protocolField)
        {
            if (protocolField.ProtocolFieldId == 0)
            {
                context.ProtocolField.Add(protocolField);
            }
            else
            {
                context.ProtocolField.Update(protocolField);
            }

            return await context.SaveChangesAsync() > 0;
        }
    }
}
