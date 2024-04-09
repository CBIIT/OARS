using Microsoft.AspNetCore.Authentication;
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
            var protocolMappings = await context.ProtocolMapping.Include(p => p.Protocol).ToListAsync();
            var protocols = await context.Protocols.ToListAsync();

            var pmStudyIds = new HashSet<string>(protocolMappings.Select(pm => pm.THORStudyId));

            foreach (var protocol in protocols)
            {
                if (protocol.StudyId is null)
                {
                    continue;
                }
                if(pmStudyIds.Contains(protocol.StudyId))
                {
                    List<ProtocolMapping> studyMappings = protocolMappings.Select(protocolMapping => protocolMapping).Where(protocolMapping => protocolMapping.THORStudyId == protocol.StudyId).ToList();
                    if (studyMappings.Count >= 1)
                    {
                        foreach (ProtocolMapping mapping in studyMappings)
                        {
                            // we can't edit a mapping that's published to prod, so remove it from the list
                            if (mapping.PublishStatus == ProtocolMappingPublishStatus.PublishedToProd)
                            {
                                protocolMappings.Remove(mapping);
                                studyMappings.Remove(mapping);
                                if(studyMappings.Count == 0)
                                {
                                    pmStudyIds.Remove(pmStudyIds.First(s => s == protocol.StudyId)); // remove this study id so a new one is created if there's no unpublished mappings
                                }
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }              
                }
                if (!pmStudyIds.Contains(protocol.StudyId))
                {
                    var pm = new ProtocolMapping
                    {
                        THORStudyId = protocol.StudyId,
                        Protocol = protocol,
                        ProtocolTitle = protocol.ProtocolTitle,
                        Sponsor = protocol.LeadOrganization,
                        ProtocolDataSystemId = 1,
                        DateFormat = "MM/dd/yyyy",
                        DataFileFolder = "C:\\DataFiles",
                        CreateDate = DateTime.Now,
                        Status = context.ProtocolMappingStatus.FirstOrDefault(s => s.ProtocolMappingStatusId == 1),
                        ProfileId = 1,
                        MappingVersion = 1,
                        PublishStatus = ProtocolMappingPublishStatus.NotPublished
                    };
                    protocolMappings.Add(pm);
                }
            }

            return protocolMappings;
        }

        public async Task<ProtocolMapping> GetProtocolMapping(int id)
        {
            var protocolMapping = await context.ProtocolMapping.Where(p => p.ProtocolMappingId == id).FirstOrDefaultAsync();
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
                    currentMapping.PublishStatus = mapping.PublishStatus;
                    currentMapping.ProtocolMappingStatusId = mapping.ProtocolMappingStatusId;
                    currentMapping.BillingCode = mapping.BillingCode;
                    currentMapping.ProtocolTitle = mapping.ProtocolTitle;
                    currentMapping.Sponsor = mapping.Sponsor;
                    currentMapping.ProtocolDataSystemId = mapping.ProtocolDataSystemId;
                    currentMapping.DateFormat = mapping.DateFormat;
                    currentMapping.DataFileFolder = mapping.DataFileFolder;
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

        public async Task<bool> PublishProtocolMapping(int id, string environment)
        {
			try
            {
				ProtocolMapping currentMapping = context.ProtocolMapping.Where(p => p.ProtocolMappingId == id).FirstOrDefault();
                
				if (currentMapping != null)
                {
                    currentMapping.PublishStatus = environment == "Production" ? ProtocolMappingPublishStatus.PublishedToProd : ProtocolMappingPublishStatus.PublishedToTest;
					context.Update(currentMapping);

					IList<ProtocolMapping> otherMappings = context.ProtocolMapping.Where(p => p.THORStudyId == currentMapping.THORStudyId && p.ProtocolMappingId != currentMapping.ProtocolMappingId).ToList();

					if (otherMappings != null && otherMappings.Count > 0)
                    {
                        if(environment == "Production")
                        {
							foreach (ProtocolMapping mapping in otherMappings)
                            {
								if(mapping.PublishStatus == ProtocolMappingPublishStatus.PublishedToProd)
                                {
                                    mapping.PublishStatus = ProtocolMappingPublishStatus.Archived;
                                    context.Update(mapping);
                                    break; // there should only ever be one other mapping that is published to prod
                                }
							}
						}
                        else if (environment == "Test")
                        { 
                            foreach (ProtocolMapping mapping in otherMappings)
                            {
                                if(mapping.PublishStatus == ProtocolMappingPublishStatus.PublishedToTest)
                                {
                                    mapping.PublishStatus = ProtocolMappingPublishStatus.NotPublished;
                                    context.Update(mapping);
                                    break; // there should only ever be one other mapping that is published to test
                                }
                            }
                        }
                    }
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
    }
}
