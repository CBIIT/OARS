using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.ComponentModel.Design;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolMappingService: BaseService, IProtocolMappingService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolMappingService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ProtocolMapping>> GetProtocolMappings(bool includeArchived)
        {
            IList<ProtocolMapping> protocolMappings = new List<ProtocolMapping>();
            if (includeArchived)
            {
                protocolMappings = await context.ProtocolMapping.Include(p => p.Protocol).Include(p => p.Status).ToListAsync();
            } else
            {
                protocolMappings = await context.ProtocolMapping.Include(p => p.Protocol).Include(p => p.Status).Where(p => p.Status.StatusName != "Archived").ToListAsync();
            }
            
            var protocols = await context.Protocols.ToListAsync();

            var pmStudyIds = new HashSet<string>(protocolMappings.Select(pm => pm.THORStudyId));

            foreach (var protocol in protocols)
            {
                if (protocol.StudyId is null)
                {
                    continue;
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

                    };
                    protocolMappings.Add(pm);
                }
            }

            return protocolMappings;
        }

        public async Task<IList<ProtocolMapping>> GetExistingProtocolMappings()
        {
            // Get only the existing protocol mappings
            // as opposed to the method above which also creates objects for protocols that don't have a mapping yet

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
    }
}
