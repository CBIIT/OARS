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
            var protocolMappings = await context.ProtocolMapping.ToListAsync();
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
                        IsPublished = false
                    };
                    protocolMappings.Add(pm);
                }
            }

            return protocolMappings;
        }

        public async Task<bool> SaveProtocolMapping(ProtocolMapping mapping)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                ProtocolMapping currentMapping = context.ProtocolMapping.Where(p => p.ProtocolMappingId == mapping.ProtocolMappingId).FirstOrDefault();

                if (currentMapping == null || currentMapping.CreateDate == null)
                {
                    currentMapping.CreateDate = currentDateTime;
                    context.Add(currentMapping);
                }
                else
                {
                    //Can we change the ThorIDfield?
                    currentMapping.ProfileId = mapping.ProfileId;
                    currentMapping.THORStudyId = mapping.THORStudyId;
                    currentMapping.Protocol = mapping.Protocol;
                    currentMapping.MappingVersion = mapping.MappingVersion;
                    currentMapping.SourceProtocolMappingId = mapping.SourceProtocolMappingId;
                    currentMapping.IsPublished = mapping.IsPublished;
                    currentMapping.ProtocolMappingStatusId = mapping.ProtocolMappingStatusId;
                    currentMapping.BillingCode = mapping.BillingCode;
                    currentMapping.ProtocolTitle = mapping.ProtocolTitle;
                    currentMapping.Sponsor = mapping.Sponsor;
                    currentMapping.ProtocolDataSystemId = mapping.ProtocolDataSystemId;
                    currentMapping.DateFormat = mapping.DateFormat;
                    currentMapping.DataFileFolder = mapping.DataFileFolder;
                    context.Update(currentMapping);
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
