using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolFieldService : BaseService, IProtocolFieldService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        private readonly IProtocolMappingService _protocolMappingService;
        public ProtocolFieldService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager, IProtocolMappingService protocolMappingService) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _protocolMappingService = protocolMappingService;
        }

        public async Task<IList<ProtocolField>> GetProtocolFieldsByMappingId(int mappingId)
        {
            
            var protocolFields = await context.ProtocolField.Where(pf => pf.ProtocolMappingId == mappingId && pf.ThorField.FieldType.FieldTypeName == "Date" && pf.IsEnabled == 'Y').ToListAsync();
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
                        IsRequired = 'F',
                        IsEnabled = 'F',
                        CanBeDictionary = 'F',
                        IsMultiForm = 'F',
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    protocolFields.Add(pf);
                }
            }
            return protocolFields;
        }

        public async Task<IList<ProtocolField>> GetAllProtocolFieldsByMappingId(int mappingId)
        {

            var protocolFields = await context.ProtocolField.Include(x => x.ThorField).ThenInclude(y => y.Category).Where(pf => pf.ProtocolMappingId == mappingId).ToListAsync();

            return protocolFields;
        }

        public async Task<bool> SaveProtocolField(int protocolMappingId, ProtocolField protocolField)
        {
            try
            {
                string protocolMappingFormat = "MM/dd/yyyy";
                var protocolMapping = _protocolMappingService.GetProtocolMapping(protocolMappingId).Result;
                if (protocolMapping != null && protocolMapping.DateFormat != null) protocolMappingFormat = protocolMapping.DateFormat;

                DateTime currentDateTime = DateTime.UtcNow;

                ProtocolField currentField = context.ProtocolField.Where(p => p.ProtocolFieldId == protocolField.ProtocolFieldId).FirstOrDefault();

                if (currentField == null || protocolField.CreateDate == null)
                {
                    protocolField.CreateDate = currentDateTime;
                    protocolField.UpdateDate = currentDateTime;
                    protocolField.ProtocolMappingId = protocolMappingId;
                    protocolField.Format = protocolMappingFormat;
                    context.Add(protocolField);
                }
                else
                {
                    currentField.ProtocolFieldId = protocolField.ProtocolFieldId;
                    currentField.ProtocolMappingId = protocolField.ProtocolMappingId;
                    currentField.ThorFieldId = protocolField.ThorFieldId;
                    currentField.Format = protocolMappingFormat;
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

        public async Task<bool> DeleteAllFieldsForMappingId(int mappingId)
        {
            try
            {
                context.RemoveRange(context.ProtocolField.Where(f => f.ProtocolMappingId == mappingId));
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> DeleteField(int fieldId)
        {
            try
            {
                context.Remove(context.ProtocolField.Where(f => f.ProtocolFieldId == fieldId).FirstOrDefault());
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
