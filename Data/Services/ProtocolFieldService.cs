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
            var protocolFields = await context.ProtocolField.Where(pf => pf.ProtocolMappingId == mappingId && pf.ThorField.FieldType.FieldTypeName == "Date" && pf.IsEnabled).ToListAsync();
            var thorFields = await context.THORField.Where(tf => tf.FieldType.FieldTypeName == "Date").ToListAsync();

            var pfThorFieldIds = new HashSet<string>(protocolFields.Select(pf => pf.ThorFieldId));

            foreach (var thorField in thorFields)
            {
                if (!pfThorFieldIds.Contains(thorField.ThorFieldId))
                {
                    var pf = new ProtocolField
                    {
                        ProtocolMappingId = mappingId,
                        ThorFieldId = thorField.ThorFieldId,
                        ThorField = thorField,
                        Format = "MM/dd/yyyy",
                        IsRequired = false,
                        IsEnabled = false,
                        CanBeDictionary = false,
                        IsMultiForm = false,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    protocolFields.Add(pf);
                }
            }
            return protocolFields;
        }

        public async Task<bool> SaveProtocolField(int protocolMappingId, ProtocolField protocolField)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                ProtocolField currentField = context.ProtocolField.Where(p => p.ProtocolFieldId == protocolField.ProtocolFieldId).FirstOrDefault();

                if (currentField == null || protocolField.CreateDate == null)
                {
                    protocolField.CreateDate = currentDateTime;
                    protocolField.UpdateDate = currentDateTime;
                    protocolField.ProtocolMappingId = protocolMappingId;
                    context.Add(protocolField);
                }
                else
                {
                    currentField.ProtocolFieldId = protocolField.ProtocolFieldId;
                    currentField.ProtocolMappingId = protocolField.ProtocolMappingId;
                    currentField.ThorFieldId = protocolField.ThorFieldId;
                    currentField.Format = protocolField.Format;
                    currentField.IsRequired = protocolField.IsRequired;
                    currentField.IsEnabled = protocolField.IsEnabled;
                    currentField.CanBeDictionary = protocolField.CanBeDictionary;
                    currentField.IsMultiForm = protocolField.IsMultiForm;
                    currentField.CreateDate = protocolField.CreateDate;
                    currentField.UpdateDate = currentDateTime;
                    context.Update(currentField);
                }

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
