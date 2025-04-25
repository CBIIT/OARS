using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public class ProtocolMappingService: BaseService, IProtocolMappingService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        private readonly IProtocolFieldService _protocolFieldService;
        private readonly IProtocolDataCategoryService _protocolDataCategoryService;
        public ProtocolMappingService(
            IDatabaseConnectionService databaseConnectionService, 
            IErrorLogService errorLogService, 
            NavigationManager navigationManager,
            IProtocolFieldService protocolFieldService,
            IProtocolDataCategoryService protocolDataCategoryService) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _protocolFieldService = protocolFieldService;
            _protocolDataCategoryService = protocolDataCategoryService;
        }

        public async Task<IList<ProtocolMapping>> GetProtocolMappings(bool includeArchived)
        {
            IList<ProtocolMapping> protocolMappings = new List<ProtocolMapping>();
            if (includeArchived)
            {
                protocolMappings = await context.ProtocolMapping.Include(p => p.Status).ToListAsync();
            } else
            {
                protocolMappings = await context.ProtocolMapping.Include(p => p.Status).Where(p => p.Status.StatusName != "Archived").ToListAsync();
            }

            return protocolMappings;
        }

        public async Task<IList<ProtocolMapping>> GetProtocolMappings(List<string> studyIds, bool includeArchived)
        {
            var mappings = await GetProtocolMappings(includeArchived);
            return mappings.Where(p => studyIds.Contains(p.THORStudyId)).ToList();
        }

        public async Task<IList<ProtocolMapping>> GetExistingProtocolMappings()
        {
            var mappings = await context.ProtocolMapping.Include(p => p.Protocol).Include(p => p.Status).Where(p => p.Status.StatusName != "Archived").ToListAsync();
            return mappings;
        }
        public async Task<ProtocolMapping> GetProtocolMapping(int id)
        {
            var protocolMapping = await context.ProtocolMapping.Where(p => p.ProtocolMappingId == id)
                .Include(p => p.Protocol)
                .Include(p => p.Profile)
                .Include(p => p.Status)
                .FirstOrDefaultAsync();
            return protocolMapping;
        }

        public async Task<bool> SaveProtocolMapping(ProtocolMapping mapping, IList<ProtocolPhase> phasesSet)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                ProtocolMapping currentMapping = context.ProtocolMapping.Where(p => p.ProtocolMappingId == mapping.ProtocolMappingId).FirstOrDefault();

                if (currentMapping == null || currentMapping.CreateDate == null)
                {
                    mapping.CreateDate = currentDateTime;
                    context.Add(mapping);
                    await context.SaveChangesAsync();

                    int protocolMappingId = mapping.ProtocolMappingId;

                    if (phasesSet != null && phasesSet.Count > 0)
                    {
                        foreach( ProtocolPhase phase in phasesSet)
                        {
                            phase.ProtocolMappingId = protocolMappingId;
                            context.Add(phase);
                        }
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    //Can we change the ThorIDfield?
                    currentMapping.ProfileId = mapping.ProfileId;
                    currentMapping.THORStudyId = mapping.THORStudyId;
                    currentMapping.Protocol = mapping.Protocol;
                    currentMapping.MappingVersion = mapping.MappingVersion;
                    currentMapping.SourceProtocolMappingId = mapping.SourceProtocolMappingId;
                    currentMapping.ProtocolMappingStatusId = mapping.ProtocolMappingStatusId;
                    currentMapping.BillingCode = mapping.BillingCode;
                    currentMapping.ProtocolTitle = mapping.ProtocolTitle;
                    currentMapping.Sponsor = mapping.Sponsor;
                    currentMapping.ProtocolDataSystemId = mapping.ProtocolDataSystemId;
                    currentMapping.DateFormat = mapping.DateFormat;
                    currentMapping.DataFileFolder = mapping.DataFileFolder;
                    currentMapping.ProtocolCrossoverOptionId = mapping.ProtocolCrossoverOptionId;
                    context.Update(currentMapping);

                    if (phasesSet != null && phasesSet.Count > 0)
                    {
                        foreach (ProtocolPhase phase in phasesSet)
                        {
                            ProtocolPhase currentPhase = context.ProtocolPhases.Where(p => p.ProtocolPhaseId == phase.ProtocolPhaseId && p.ProtocolMappingId == mapping.ProtocolMappingId).FirstOrDefault();
                            if (currentPhase == null || currentPhase.CreateDate == null)
                            {
                                phase.CreateDate = currentDateTime;
                                phase.ProtocolMappingId = currentMapping.ProtocolMappingId;
                                phase.IsRandomized = phase.IsRandomized;
                                phase.IsEnabled = phase.IsEnabled;
                                phase.UpdateDate = currentDateTime;
                                context.Add(phase);
                            }
                            else
                            {
                                currentPhase.IsRandomized = phase.IsRandomized;
                                currentPhase.IsEnabled = phase.IsEnabled;
                                currentPhase.UpdateDate = currentDateTime;
                                context.Update(currentPhase);
                            }
                        }
                        await context.SaveChangesAsync();
                    }

                    await context.SaveChangesAsync();
                }


                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> PublishProtocolMapping(int id)
        {
			try
            {
				ProtocolMapping currentMapping = context.ProtocolMapping.Where(p => p.ProtocolMappingId == id).FirstOrDefault();
                IList<ProtocolMappingStatus> currStatuses = await context.ProtocolMappingStatus.ToListAsync();
                if (currStatuses.Count < 3)
                {
                    // if there aren't the three statuses we're expecting, we can't publish
                    return false;
                }

                ProtocolMappingStatus publishedToProd = currStatuses.Where(s => s.StatusName == "Published To Prod").FirstOrDefault();     
                ProtocolMappingStatus archived = currStatuses.Where(s => s.StatusName == "Archived").FirstOrDefault();
                ProtocolMappingStatus active = currStatuses.Where(s => s.StatusName == "Active").FirstOrDefault();

                if(publishedToProd == null || archived == null || active == null)
                {
                    // if any of the statuses are missing then something has changed in the DB, or the statuses don't exist
                    return false;
                }

				if (currentMapping != null)
                {
                    currentMapping.ProtocolMappingStatusId = publishedToProd.ProtocolMappingStatusId;
                    currentMapping.IsPublished = true;
					context.Update(currentMapping);

					IList<ProtocolMapping> otherMappings = context.ProtocolMapping.Where(p => p.THORStudyId == currentMapping.THORStudyId && p.ProtocolMappingId != currentMapping.ProtocolMappingId).ToList();

					if (otherMappings != null && otherMappings.Count > 0)
                    {
						foreach (ProtocolMapping mapping in otherMappings)
                        {
							if(mapping.ProtocolMappingStatusId == publishedToProd.ProtocolMappingStatusId)
                            {
                                mapping.ProtocolMappingStatusId = archived.ProtocolMappingStatusId;
                                mapping.IsPublished = false;
                                context.Update(mapping);
                                break; // there should only ever be one other mapping that is published to prod
                            }
						}
                    }

                    // Create a new active mapping from the old one
                    ProtocolMapping newMapping = new ProtocolMapping
                    {
                        ProtocolMappingId = 0,
                        ProtocolMappingStatusId = active.ProtocolMappingStatusId,
                        MappingVersion = currentMapping.MappingVersion + 1,
                        ProfileId = currentMapping.ProfileId,
                        THORStudyId = currentMapping.THORStudyId,
                        Protocol = currentMapping.Protocol,
                        Profile = currentMapping.Profile,
                        SourceProtocolMappingId = currentMapping.SourceProtocolMappingId,
                        BillingCode = currentMapping.BillingCode,
                        ProtocolTitle = currentMapping.ProtocolTitle,
                        Sponsor = currentMapping.Sponsor,
                        ProtocolDataSystemId = currentMapping.ProtocolDataSystemId,
                        DateFormat = currentMapping.DateFormat,
                        DataFileFolder = currentMapping.DataFileFolder,
                        CreateDate = DateTime.Now,
                        IsPublished = false
                    };
                    context.Add(newMapping);

					await context.SaveChangesAsync();
					return true;
				}
				return false;
			}
			catch (Exception ex)
            {
				await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
				return false;
			}
		}

        public async Task<IList<ProtocolMapping>> GetAllProtocolMappingsFromProfileType(int profileType)
        {
            //PENDING: This needs to reviewed, where are we storing the profiletype. The comparison needs to be fixed.
            var protocolMappings = await context.ProtocolMapping.Where(p=> p.ProtocolMappingId == profileType).ToListAsync();

            return protocolMappings;
        }

        private struct FieldMapping
        {
            public int ProtocolEDCFieldId;
            public string ThorFieldId;
        }

        public async Task<bool> CopyMapping(int sourceId, int targetId)
        {
            var sourceProtocolCategories = await context.ProtocolDataCategories
                .Include(p => p.THORDataCategory)
                .Where(p => p.ProtocolMappingId == sourceId)
                .ToListAsync();

            var targetProtocolDataCategories = new Dictionary<string, ProtocolDataCategory>();

            foreach (var sourceCategory in sourceProtocolCategories)
            {
                if (sourceCategory.THORDataCategoryId == null)
                {
                    continue;
                }

                var targetCategory = await _protocolDataCategoryService.GetOrBuildProtocolDataCategory(targetId, sourceCategory.THORDataCategoryId);
                targetCategory.IsMultiForm = sourceCategory.IsMultiForm;
                var saveSuccess = await _protocolDataCategoryService.SaveCategory(targetCategory, targetId);
                targetProtocolDataCategories.Add(sourceCategory.THORDataCategoryId, targetCategory);
            }

            var sourceFormMappings = await context.ProtocolFormMappings
                .Include(p => p.ProtocolCategory)
                .Include(p => p.ProtocolCategory.THORDataCategory)
                .Where(p => p.ProtocolEDCForm.ProtocolMappingId == sourceId)
                .GroupBy(p => p.ProtocolEDCForm.EDCFormIdentifier)
                .ToDictionaryAsync(p => p.Key!, p => p.ToList());

            Dictionary<string, ProtocolEDCForm> targetForms = new Dictionary<string, ProtocolEDCForm>();
            if(sourceFormMappings.Count != 0)
            {
                targetForms = await context.ProtocolEDCForms
                    .Where(p => p.ProtocolMappingId == targetId)
                    .GroupBy(p => p.EDCFormIdentifier)
                    .ToDictionaryAsync(p => p.Key!, p => p.ToList().First()!);

            }

            DataTable formMappings = new DataTable();
            formMappings.Columns.Add("PROTOCOL_FORM_MAPPING_ID", typeof(int));
            formMappings.Columns.Add("PROTOCOL_EDC_FORM_ID", typeof(int));
            formMappings.Columns.Add("IS_PRIMARY_FORM", typeof(char));
            formMappings.Columns.Add("CREATE_DATE", typeof(DateTime));
            formMappings.Columns.Add("UPDATE_DATE", typeof(DateTime));
            formMappings.Columns.Add("PROTOCOL_CATEGORY_ID", typeof(int));


            foreach (var sourceForm in sourceFormMappings)
            {
                // If the target protocol does not contain the form, skip it
                if (targetForms.ContainsKey(sourceForm.Key) == false)
                {
                    continue;
                }

                var targetForm = targetForms[sourceForm.Key];
                if (targetForm.ProtocolMappingId == null)
                {
                    continue;
                }
                var sourceFormMappingList = sourceForm.Value;
                foreach (var srcFormMapping in sourceFormMappingList)
                {
                    if (srcFormMapping == null)
                    {
                        continue;
                    }
                    if (srcFormMapping.ProtocolCategory == null)
                    {
                        continue;
                    }
                    if (srcFormMapping.ProtocolCategory.THORDataCategoryId == null)
                    {
                        continue;
                    }

                    var targetProtocolDataCategory = targetProtocolDataCategories[srcFormMapping.ProtocolCategory.THORDataCategoryId];

                    DataRow targetFormMapping = formMappings.NewRow();
                    targetFormMapping["PROTOCOL_CATEGORY_ID"] = targetProtocolDataCategory.ProtocolCategoryId;
                    targetFormMapping["PROTOCOL_EDC_FORM_ID"] = targetForm.ProtocolEDCFormId;
                    targetFormMapping["IS_PRIMARY_FORM"] = srcFormMapping.IsPrimaryForm ? 'Y' : 'N';
                    targetFormMapping["CREATE_DATE"] = DateTime.Now;
                    targetFormMapping["UPDATE_DATE"] = DateTime.Now;
                    formMappings.Rows.Add(targetFormMapping);
                }
            }

            var sourceFieldMappings = await context.ProtocolFieldMappings
                .Include(p => p.ProtocolEDCField)
                .Include(p => p.ProtocolEDCField.ProtocolEDCForm)
                .Where(p => p.ProtocolEDCField.ProtocolEDCForm.ProtocolMappingId == sourceId)
                .GroupBy(p => p.ProtocolEDCField.ProtocolEDCForm.EDCFormIdentifier + "|" + p.ProtocolEDCField.EDCFieldIdentifier)
                .ToDictionaryAsync(p => p.Key, p => p.ToList());

            Dictionary<string, ProtocolEDCField> targetFields = new Dictionary<string, ProtocolEDCField>();
            if (sourceFieldMappings.Count != 0)
            {
                targetFields = await context.ProtocolEDCField
                    .Include(p => p.ProtocolEDCForm)
                    .Where(p => p.ProtocolEDCForm.ProtocolMappingId == targetId)
                    .GroupBy(p => p.ProtocolEDCForm.EDCFormIdentifier + "|" + p.EDCFieldIdentifier)
                    .ToDictionaryAsync(Dictionary => Dictionary.Key, Dictionary => Dictionary.ToList().First()!);
            }

            DataTable fieldMappings = new DataTable();
            fieldMappings.Columns.Add("PROTOCOL_FIELD_MAPPING_ID", typeof(int));
            fieldMappings.Columns.Add("THOR_FIELD_ID", typeof(string));
            fieldMappings.Columns.Add("PROTOCOL_EDC_FIELD_ID", typeof(int));
            fieldMappings.Columns.Add("CREATE_DATE", typeof(DateTime));
            fieldMappings.Columns.Add("UPDATE_DATE", typeof(DateTime));

            foreach(var sourceFieldMapping in sourceFieldMappings)
            {
                if (targetFields.ContainsKey(sourceFieldMapping.Key) == false)
                {
                    continue;
                }

                var targetField = targetFields[sourceFieldMapping.Key];
                var sourceFieldMappingList = sourceFieldMapping.Value;
                foreach (var srcFieldMapping in sourceFieldMappingList)
                {
                    if (srcFieldMapping == null)
                    {
                        continue;
                    }
                    if (srcFieldMapping.ThorFieldId == null)
                    {
                        continue;
                    }

                    DataRow targetFieldMapping = fieldMappings.NewRow();
                    targetFieldMapping["THOR_FIELD_ID"] = srcFieldMapping.ThorFieldId;
                    targetFieldMapping["PROTOCOL_EDC_FIELD_ID"] = targetField.ProtocolEDCFieldId;
                    targetFieldMapping["CREATE_DATE"] = DateTime.Now;
                    targetFieldMapping["UPDATE_DATE"] = DateTime.Now;
                    fieldMappings.Rows.Add(targetFieldMapping);

                }
            }

            try
            {
                using (var bulkCopy = new OracleBulkCopy(oracleConnection, OracleBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationSchemaName = "DMU";
                    bulkCopy.DestinationTableName = "\"PROTOCOLFORMMAPPING\"";
                    bulkCopy.BatchSize = formMappings.Rows.Count;
                    bulkCopy.WriteToServer(formMappings);

                    bulkCopy.DestinationTableName = "\"PROTOCOLFIELDMAPPING\"";
                    bulkCopy.BatchSize = fieldMappings.Rows.Count;
                    bulkCopy.WriteToServer(fieldMappings);
                }

                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }

            // Get all of the dictionaries for the source
            var sourceDictionaryMappings = await context.ProtocolDictionaryMapping
                .Include(p => p.ProtocolEDCDictionary)
                .Include(p => p.ProtocolFieldMapping)
                .Include(p => p.ProtocolFieldMapping!.ProtocolEDCField)
                .Where(p => p.ProtocolFieldMapping!.ProtocolEDCField.ProtocolEDCForm.ProtocolMappingId == sourceId)
                .GroupBy(p => p.ProtocolEDCDictionary!.EDCDictionaryName + "|" + p.ProtocolEDCDictionary.EDCItemId)
                .ToDictionaryAsync(p => p.Key, p => p.ToList());

            Dictionary<string, ProtocolFieldMapping> targetFieldMappings = new Dictionary<string, ProtocolFieldMapping>();
            Dictionary<string, ProtocolEDCDictionary> targetDictionaries = new Dictionary<string, ProtocolEDCDictionary>();
            if (sourceDictionaryMappings.Count != 0)
            {
                // Get the newly created target field mappings that can have dictionaries associeated, for the target field mapping IDs
                targetFieldMappings = await context.ProtocolFieldMappings
                .Include(p => p.ProtocolEDCField)
                .Include(p => p.ProtocolEDCField.ProtocolEDCForm)
                .Where(p =>
                    p.ProtocolEDCField.ProtocolEDCForm.ProtocolMappingId == targetId &&
                    p.ThorField.ThorDictionaryId > 0
                )
                .GroupBy(p => p.ProtocolEDCField.ProtocolEDCForm.EDCFormIdentifier + "|" + p.ProtocolEDCField.EDCFieldIdentifier + "|" + p.ThorFieldId)
                .ToDictionaryAsync(p => p.Key, p => p.ToList().First());

                targetDictionaries = await context.ProtocolEDCDictionary
                    .Where(p => p.ProtocolMappingId == targetId)
                    .GroupBy(p => p.EDCDictionaryName + "|" + p.EDCItemId)
                    .ToDictionaryAsync(p => p.Key!, p => p.ToList().First()!);
            }

            DataTable dictionaryMappings = new DataTable();
            dictionaryMappings.Columns.Add("PROTOCOL_DICTIONARY_MAPPING_ID", typeof(int));
            dictionaryMappings.Columns.Add("PROTOCOL_FIELD_MAPPING_ID", typeof(int));
            dictionaryMappings.Columns.Add("PROTOCOL_EDC_DICTIONARY_ID", typeof(int));
            dictionaryMappings.Columns.Add("THOR_DICTIONARY_ID", typeof(int));
            dictionaryMappings.Columns.Add("CREATE_DATE", typeof(DateTime));
            dictionaryMappings.Columns.Add("UPDATE_DATE", typeof(DateTime));

            foreach (var dictMapDictItem in sourceDictionaryMappings)
            {
                var dictCompositeKey = dictMapDictItem.Key;
                if (targetDictionaries.ContainsKey(dictCompositeKey) == false)
                {
                    continue;
                }

                var targetDictionary = targetDictionaries[dictCompositeKey];

                var sourceDictionaryMappingList = dictMapDictItem.Value;
                foreach (var sourceDictionaryMapping in sourceDictionaryMappingList)
                {
                    var protocolFieldMapping = sourceDictionaryMapping.ProtocolFieldMapping;
                    var sourceFieldMappingKey = $"{protocolFieldMapping!.ProtocolEDCField.ProtocolEDCForm.EDCFormIdentifier}|{protocolFieldMapping.ProtocolEDCField.EDCFieldIdentifier + "|" + protocolFieldMapping.ThorFieldId}";
                    if (targetFieldMappings.ContainsKey(sourceFieldMappingKey) == false)
                    {
                        continue;
                    }

                    var targetFieldMapping = targetFieldMappings[sourceFieldMappingKey];

                    DataRow targetDictionaryMapping = dictionaryMappings.NewRow();
                    targetDictionaryMapping["PROTOCOL_FIELD_MAPPING_ID"] = targetFieldMapping.ProtocolFieldMappingId;
                    targetDictionaryMapping["PROTOCOL_EDC_DICTIONARY_ID"] = targetDictionary.ProtocolEDCDictionaryId;
                    targetDictionaryMapping["THOR_DICTIONARY_ID"] = sourceDictionaryMapping.THORDictionaryId;
                    targetDictionaryMapping["CREATE_DATE"] = DateTime.Now;
                    targetDictionaryMapping["UPDATE_DATE"] = DateTime.Now;
                    dictionaryMappings.Rows.Add(targetDictionaryMapping);
                }

            }

            try
            {
                using (var bulkCopy = new OracleBulkCopy(oracleConnection, OracleBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationSchemaName = "DMU";
                    bulkCopy.DestinationTableName = "\"PROTOCOLDICTIONARYMAPPING\"";
                    bulkCopy.BatchSize = dictionaryMappings.Rows.Count;
                    bulkCopy.WriteToServer(dictionaryMappings);
                }

                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }

            var target = await context.ProtocolMapping.Where(p => p.ProtocolMappingId == targetId).FirstOrDefaultAsync();
            target.SourceProtocolMappingId = sourceId;
            context.Update(target);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<IList<CrossoverOption>> GetCrossoverOptions()
        {
            return context.CrossoverOptions.ToList();
        }
    }
}
