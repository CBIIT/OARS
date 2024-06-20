using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using YamlDotNet.Core.Events;

namespace TheradexPortal.Data.Services
{
    public class ProtocolFieldService : BaseService, IProtocolFieldService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolFieldService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ProtocolField>> GetProtocolDateFieldsByMappingId(int protocolMappingId)
        {
            
            var protocolFields = await context.ProtocolField.Where(pf => pf.ProtocolMappingId == protocolMappingId && pf.ThorField.FieldType!.FieldTypeName == "Date" && pf.IsEnabled).ToListAsync();
            var thorFields = await context.THORField.Where(tf => tf.FieldType!.FieldTypeName == "Date").ToListAsync();

            var pfThorFieldIds = new HashSet<string>(protocolFields.Select(pf => pf.ThorFieldId));

            foreach (var thorField in thorFields)
            {
                if (!pfThorFieldIds.Contains(thorField.ThorFieldId))
                {
                    var pf = new ProtocolField
                    {
                        ProtocolMappingId = protocolMappingId,
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

        public async Task<IList<ProtocolField>> GetAllProtocolFieldsByMappingId(int protocolMappingId)
        {
            var mapping = await context.ProtocolMapping.Where(p => p.ProtocolMappingId == protocolMappingId).Include(p => p.Protocol).Include(p => p.Profile).FirstOrDefaultAsync();
            if (mapping == null)
                return new List<ProtocolField>();

            var profileFields = await context.ProfileFields.Include(x => x.ThorField).Where(pf => pf.ProfileId == mapping.ProfileId).ToListAsync();
            var pfIds = profileFields.Select(pf => pf.THORFieldId).ToList();
            var protocolFields = await context.ProtocolField
                .Include(x => x.ThorField)
                .ThenInclude(y => y.Category)
                .Where(pf => pf.ProtocolMappingId == protocolMappingId)
                .ToListAsync();

            protocolFields = protocolFields.Where(p => pfIds.Contains(p.ThorFieldId)).ToList();

            foreach (var protocolField in protocolFields)
            {
                protocolField.ThorDataCategoryId = protocolField.ThorField.Category!.ThorDataCategoryId;
            }

            return protocolFields;
        }
        
        public async Task<IList<ProtocolField>> GetAllProtocolFieldsByMappingIdForMapping(int protocolMappingId, string dataCategoryId)
        {
            return await context.ProtocolField
                .Include(pf => pf.ThorField)
                .Where(pf =>
                    pf.ProtocolMappingId == protocolMappingId &&
                    pf.ThorField.ThorDataCategoryId == dataCategoryId &&
                    pf.IsEnabled
                )
                .ToListAsync();
        }

        public async Task<IList<ProtocolField>> GetAllProtocolDisabledFieldsByMappingIdForMapping(int protocolMappingId, string dataCategoryId)
        {
            return await context.ProtocolField
                .Include(pf => pf.ThorField)
                .Where(pf =>
                    pf.ProtocolMappingId == protocolMappingId &&
                    pf.ThorField.ThorDataCategoryId == dataCategoryId &&
                    pf.IsEnabled == false
                )
                .ToListAsync();
        }
        public async Task<bool> SaveProtocolField(int protocolMappingId, ProtocolField protocolField)
        {
            try
            {
                int? statusId = context.ProtocolMapping.Where(x => x.ProtocolMappingId == protocolMappingId).Select(x => x.ProtocolMappingStatusId).FirstOrDefault();
                if (statusId != null)
                {
                    string? statusText = context.ProtocolMappingStatus.Where(x => x.ProtocolMappingStatusId == statusId).Select(x => x.StatusName).FirstOrDefault();
                    if (statusText != "Active")
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                string protocolMappingFormat = "MM/dd/yyyy";
                var protocolMapping = await context.ProtocolMapping.Where(p => p.ProtocolMappingId == protocolMappingId).Include(p => p.Protocol).Include(p => p.Profile).FirstOrDefaultAsync();
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

        public async Task<bool> CreateProtocolFieldsFromProfile(int? profileId, int mappingId)
        {
            try
            {
                if (profileId == null) return false;

                var profileFields = await context.ProfileFields
                    .Include(x => x.ThorField)
                    .Include(x => x.ThorField.FieldType)
                    .Where(pf => pf.ProfileId == profileId && pf.ThorField.IsActive)
                    .ToListAsync();

                foreach (var profileField in profileFields)
                {
                    ProtocolField protocolField = new ProtocolField
                    {
                        ThorDataCategoryId = profileField.ThorField.ThorDataCategoryId,
                        ThorFieldId = profileField.THORFieldId,
                        Format = "",
                        IsRequired = false,
                        IsEnabled = false,
                        CanBeDictionary = profileField.ThorField.FieldType?.FieldTypeName == "Dropdown",
                        IsMultiForm = profileField.ThorField.IsMultiForm,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                    };

                    bool result = await SaveProtocolField(mappingId, protocolField);
                    if (!result) return false;
                }

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
