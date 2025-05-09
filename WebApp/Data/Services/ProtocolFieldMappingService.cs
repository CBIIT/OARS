﻿using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public class ProtocolFieldMappingService : BaseService, IProtocolFieldMappingService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolFieldMappingService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ProtocolFieldMapping>> GetProtocolFieldMappings()
        {
            return await context.ProtocolFieldMappings.ToListAsync();
        }

        public async Task<bool> HasProtocolFieldMappingsForProtocolMapping(int protocolMappingId)
        {
            return await context.ProtocolFieldMappings.Where(x => x.ProtocolEDCField.ProtocolEDCForm.ProtocolMappingId == protocolMappingId).AnyAsync();
        }

        public async Task<IList<ProtocolFieldMapping>> GetProtocolFieldMappings(string fieldId)
        {
            return await context.ProtocolFieldMappings.Where(x => x.ThorFieldId == fieldId).Include(x => x.ProtocolEDCField).Include(p => p.ProtocolEDCField.ProtocolEDCForm).ToListAsync();
        }

        public async Task<IList<ProtocolFieldMapping>> GetProtocolFieldMappingsForCategory(int protocolMappingId, string categoryId)
        {
            var mappings = await context.ProtocolFieldMappings.
                Include(x => x.ProtocolEDCField).
                Include(p => p.ProtocolEDCField.ProtocolEDCForm).
                Include(p => p.ThorField).
                Where(p => 
                    p.ProtocolEDCField.ProtocolEDCForm.ProtocolMappingId == protocolMappingId &&
                    p.ThorFieldId != null &&
                    p.ThorField!.ThorDataCategoryId == categoryId
                )
                .ToListAsync();

            foreach(var mapping in mappings)
            {
                mapping.ProtocolEDCFormId = mapping.ProtocolEDCField.ProtocolEDCFormId;
            }
            return mappings;
        }

        public async Task<ProtocolFieldMapping> GetProtocolFieldMapping(int protocolFieldMappingId)
        {
            var fieldMapping = await context.ProtocolFieldMappings
                .Include(x=>x.ThorField)
                .Include(x=>x.ThorField.Category)
                .Include(x=>x.ProtocolEDCField)
                .Include(x=>x.ProtocolEDCField.ProtocolEDCForm)
                .Include(x=>x.ProtocolEDCField.ProtocolEDCForm.ProtocolMapping)
                .FirstOrDefaultAsync(x => x.ProtocolFieldMappingId == protocolFieldMappingId);
            return fieldMapping;
        }

        public async Task<bool> SaveProtocolFieldMapping(ProtocolFieldMapping protocolFieldMapping)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                ProtocolFieldMapping currentMapping = context.ProtocolFieldMappings.FirstOrDefault(x => x.ProtocolFieldMappingId == protocolFieldMapping.ProtocolFieldMappingId);
                if (currentMapping == null)
                {
                    protocolFieldMapping.ThorField = null;
                    protocolFieldMapping.CreateDate = currentDateTime;
                    protocolFieldMapping.UpdateDate = currentDateTime;
                    context.Add(protocolFieldMapping);
                }
                else
                {
                    currentMapping.ThorFieldId = protocolFieldMapping.ThorFieldId;
                    currentMapping.ProtocolEDCFieldId = protocolFieldMapping.ProtocolEDCFieldId;
                    currentMapping.UpdateDate = currentDateTime;
                    context.Update(currentMapping);
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> DeleteProtocolFieldMapping(ProtocolFieldMapping mapping)
        {
            try
            {
                await context.ProtocolDictionaryMapping
                    .Where(ProtocolDictionaryMapping => ProtocolDictionaryMapping.ProtocolFieldMappingId == mapping.ProtocolFieldMappingId)
                    .ExecuteDeleteAsync();
                context.ProtocolFieldMappings.Remove(mapping);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}