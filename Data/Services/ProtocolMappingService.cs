using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Polly;
using System.ComponentModel.Design;
using System.Data;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolMappingService: BaseService, IProtocolMappingService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        private readonly IProtocolFieldService _protocolFieldService;
        public ProtocolMappingService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager, IProtocolFieldService protocolFieldService) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _protocolFieldService = protocolFieldService;
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
            var protocolMapping = await context.ProtocolMapping.Where(p => p.ProtocolMappingId == id).Include(p => p.Protocol).Include(p => p.Profile).FirstOrDefaultAsync();
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

                if (mapping.ProfileId != null && mapping.ProfileId != 0)
                {
                    var protocolFields = await _protocolFieldService.GetAllProtocolFieldsByMappingId(mapping.ProtocolMappingId);
                    if (protocolFields.Count == 0)
                    {
                        await _protocolFieldService.CreateProtocolFieldsFromProfile(mapping.ProfileId, mapping.ProtocolMappingId);
                    }
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

        public async Task<bool> CopyMapping(int sourceId, int targetId)
        {
            ProtocolMapping target = await context.ProtocolMapping.Where(p => p.ProtocolMappingId == targetId).FirstOrDefaultAsync();
            target.SourceProtocolMappingId = sourceId;
            context.Update(target);

            // Get all of the forms and form mappings for the source
            var sourceForms = await context.ProtocolEDCForms.Where(p => p.ProtocolMappingId == sourceId).ToListAsync();
            var sourceFormIds = sourceForms.Select(p => p.ProtocolEDCFormId).ToList();
            var sourceFormMappings = await context.ProtocolFormMappings.Where(p => sourceFormIds.Contains((int)p.ProtocolEDCFormId)).ToListAsync();

            List<ProtocolEDCForm> targetForms = new List<ProtocolEDCForm>();
            if(sourceFormMappings.Count != 0)
            {
                targetForms = await context.ProtocolEDCForms.Where(p => p.ProtocolMappingId == targetId).ToListAsync();
            }

            DataTable formMappings = new DataTable();
            formMappings.Columns.Add("Protocol_Form_Mapping_Id", typeof(int));
            formMappings.Columns.Add("Protocol_EDC_Form_Id", typeof(int));
            formMappings.Columns.Add("Is_Primary_Form", typeof(bool));
            formMappings.Columns.Add("Create_Date", typeof(DateTime));
            formMappings.Columns.Add("Update_Date", typeof(DateTime));
            formMappings.Columns.Add("Protocol_Category_Id", typeof(int));

            foreach (var targetForm in targetForms)
            {
                var matchingSourceForm = sourceForms.FirstOrDefault(sf => sf.EDCFormIdentifier == targetForm.EDCFormIdentifier);
                if (matchingSourceForm != null)
                {
                    var toCopy = sourceFormMappings.Where(sf => sf.ProtocolEDCFormId == matchingSourceForm.ProtocolEDCFormId).FirstOrDefault();
                    if (toCopy != null)
                    {
                        DataRow targetFormMapping = formMappings.NewRow();
                        targetFormMapping["Protocol_EDC_Form_Id"] = targetForm.ProtocolEDCFormId;
                        targetFormMapping["Is_Primary_Form"] = toCopy.IsPrimaryForm;
                        targetFormMapping["Create_Date"] = DateTime.Now;
                        targetFormMapping["Update_Date"] = DateTime.Now;
                        targetFormMapping["Protocol_Category_Id"] = toCopy.ProtocolCategoryId;
                        formMappings.Rows.Add(targetFormMapping);
                    }
                }
            }

            // Get all of the fields and field mappings for the source
            var sourceFields = await context.ProtocolEDCField.Where(p => sourceFormIds.Contains(p.ProtocolEDCFormId)).ToListAsync();
            var sourceFieldIds = sourceFields.Select(p => p.ProtocolEDCFieldId).ToList();
            var sourceFieldMappings = await context.ProtocolFieldMappings.Where(p => sourceFieldIds.Contains(p.ProtocolEDCFieldId)).ToListAsync();

            List<ProtocolEDCField> targetFields = new List<ProtocolEDCField>();
            if (sourceFieldMappings.Count != 0)
            {
                targetFields = await context.ProtocolEDCField.Where(p => p.ProtocolEDCFormId == targetId).ToListAsync();
            }

            Dictionary<int, int> sourceTargetMatches = new Dictionary<int, int>();

            DataTable fieldMappings = new DataTable();
            fieldMappings.Columns.Add("Protocol_Field_Mapping_Id", typeof(int));
            fieldMappings.Columns.Add("THOR_Field_Id", typeof(string));
            fieldMappings.Columns.Add("Protocol_EDC_Field_Id", typeof(int));
            fieldMappings.Columns.Add("Create_Date", typeof(DateTime));
            fieldMappings.Columns.Add("Update_Date", typeof(DateTime));

            foreach(var targetField in targetFields)
            {
                var matchingSourceField = sourceFields.FirstOrDefault(sf => sf.EDCFieldIdentifier == targetField.EDCFieldIdentifier);
                if(matchingSourceField != null)
                {
                    sourceTargetMatches.Add(matchingSourceField.ProtocolEDCFieldId, targetField.ProtocolEDCFieldId);
                    var toCopy = sourceFieldMappings.Where(sf => sf.ProtocolEDCFieldId == matchingSourceField.ProtocolEDCFieldId).FirstOrDefault();
                    if (toCopy != null)
                    {
                        DataRow targetFieldMapping = fieldMappings.NewRow();
                        targetFieldMapping["THOR_Field_Id"] = toCopy.ThorFieldId;
                        targetFieldMapping["Protocol_EDC_Field_Id"] = targetField.ProtocolEDCFieldId;
                        targetFieldMapping["Create_Date"] = DateTime.Now;
                        targetFieldMapping["Update_Date"] = DateTime.Now;
                        fieldMappings.Rows.Add(targetFieldMapping);
                    }
                }
            }

            // Get all of the dictionaries for the source
            var sourceDictionaries = await context.ProtocolEDCDictionary.Where(p => p.ProtocolMappingId == sourceId).ToListAsync();
            var sourceDictionaryIds = sourceDictionaries.Select(p => p.ProtocolEDCDictionaryId).ToList();
            var sourceDictionaryMappings = await context.ProtocolDictionaryMapping.Where(p => sourceDictionaryIds.Contains(p.ProtocolEDCDictionaryId)).ToListAsync();

            List<ProtocolEDCDictionary> targetDictionaries = new List<ProtocolEDCDictionary>();
            if(sourceDictionaryMappings.Count != 0)
            {
                targetDictionaries = await context.ProtocolEDCDictionary.Where(p => p.ProtocolMappingId == targetId).ToListAsync();
            }

            DataTable dictionaryMappings = new DataTable();
            dictionaryMappings.Columns.Add("Protocol_Dictionary_Mapping_Id", typeof(int));
            dictionaryMappings.Columns.Add("Protocol_Field_Mapping_Id", typeof(int));
            dictionaryMappings.Columns.Add("Protocol_EDC_Dictionary_Id", typeof(int));
            dictionaryMappings.Columns.Add("THOR_Dictionary_Id", typeof(int));
            dictionaryMappings.Columns.Add("Create_Date", typeof(DateTime));
            dictionaryMappings.Columns.Add("Update_Date", typeof(DateTime));

            foreach(var targetDictionary in targetDictionaries)
            {
                var matchingSourceDictionary = sourceDictionaries.FirstOrDefault(sd => sd.EDCDictionaryName == targetDictionary.EDCDictionaryName);
                if(matchingSourceDictionary != null)
                {
                    var toCopy = sourceDictionaryMappings.Where(sd => sd.ProtocolEDCDictionaryId == matchingSourceDictionary.ProtocolEDCDictionaryId).FirstOrDefault();
                    if (toCopy != null)
                    {
                        DataRow targetDictionaryMapping = dictionaryMappings.NewRow();
                        targetDictionaryMapping["Protocol_EDC_Dictionary_Id"] = targetDictionary.ProtocolEDCDictionaryId;
                        targetDictionaryMapping["Protocol_Field_Mapping_Id"] = sourceTargetMatches[toCopy.ProtocolFieldMappingId];
                        targetDictionaryMapping["THOR_Dictionary_Id"] = toCopy.THORDictionaryId;
                        targetDictionaryMapping["Create_Date"] = DateTime.Now;
                        targetDictionaryMapping["Update_Date"] = DateTime.Now;

                        dictionaryMappings.Rows.Add(targetDictionaryMapping);
                    }
                }
            }

            try
            {
                using (var bulkCopy = new OracleBulkCopy(oracleConnection, OracleBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationSchemaName = "DMU";
                    bulkCopy.DestinationTableName = "\"ProtocolFormMapping\"";
                    bulkCopy.BatchSize = formMappings.Rows.Count;
                    bulkCopy.WriteToServer(formMappings);

                    bulkCopy.DestinationTableName = "\"ProtocolFieldMapping\"";
                    bulkCopy.BatchSize = fieldMappings.Rows.Count;
                    bulkCopy.WriteToServer(fieldMappings);

                    bulkCopy.DestinationTableName = "\"ProtocolDictionaryMapping\"";
                    bulkCopy.BatchSize = dictionaryMappings.Rows.Count;
                    bulkCopy.WriteToServer(dictionaryMappings);
                }

                await context.SaveChangesAsync();
                return true;

            } catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IList<CrossoverOption>> GetCrossoverOptions()
        {
            return context.CrossoverOptions.ToList();
        }
    }
}
