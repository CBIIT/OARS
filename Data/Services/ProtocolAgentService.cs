using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolAgentService: BaseService, IProtocolAgentService
    {
        private readonly IErrorLogService _errorLogService;
        public ProtocolAgentService(IDbContextFactory<ThorDBContext> context, IErrorLogService errorLogService) : base(context)
        {
            _errorLogService = errorLogService;
        }

        public async Task<List<ProtocolAgent>> GetAgentsByMappingId(int mappingId)
        {
            return await context.ProtocolAgents.Where(x => x.ProtocolMappingId == mappingId).ToListAsync();
        }

        public async Task<bool> SaveAgent(ProtocolAgent agent, int mappingId)
        {
            try
            {
                ProtocolAgent currAgent = context.ProtocolAgents.FirstOrDefault(x => x.ProtocolAgentId == agent.ProtocolAgentId);
                int? statusId = context.ProtocolMapping.Where(x => x.ProtocolMappingId == mappingId).Select(x => x.ProtocolMappingStatusId).FirstOrDefault();
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

                if (currAgent == null || agent.CreateDate == null)
                {
                    agent.ProtocolMappingId = mappingId;
                    agent.CreateDate = DateTime.Now;
                    agent.UpdateDate = DateTime.Now;
                    context.ProtocolAgents.Add(agent);
                }
                else
                {
                    currAgent.UpdateDate = DateTime.Now;
                    currAgent.ProtocolMappingId = mappingId;
                    currAgent.NscNumber = agent.NscNumber;
                    currAgent.AgentName = agent.AgentName;

                    context.ProtocolAgents.Update(agent);
                }
                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }

        }

        public async Task<bool> DeleteAgent(ProtocolAgent agent)
        {
            try
            {
                context.ProtocolAgents.Remove(agent);
                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
