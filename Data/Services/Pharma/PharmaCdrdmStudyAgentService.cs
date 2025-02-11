using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models.Pharma;
using TheradexPortal.Data.Services.Abstract.Pharma;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Services;


namespace TheradexPortal.Data.Services.Pharma
{
    public class PharmaCdrdmStudyAgentService : BaseService, IPharmaCdrdmStudyAgentService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public PharmaCdrdmStudyAgentService(
            IDatabaseConnectionService databaseConnectionService,
            IErrorLogService errorLogService,
            NavigationManager navigationManager)
            : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        /// <summary>
        /// Returns a distinct list of NSC codes.
        /// </summary>
        public async Task<IEnumerable<string>> GetDistinctNscCodesAsync()
        {
            return await context.Pharma_CdrdmStudyAgent.OrderBy(agent => agent.Nsc)
                .Select(agent => agent.Nsc)
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// Returns a distinct list of Document Numbers filtered by the provided NSC code.
        /// </summary>
        /// <param name="nscCode">The NSC code to filter by.</param>
        public async Task<IEnumerable<string>> GetDocumentNumbersByNscAsync(string nscCode)
        {
            if(string.IsNullOrEmpty(nscCode))
            {
                return await context.Pharma_CdrdmStudyAgent
                    .Select(agent => agent.DocumentNumber)
                    .Distinct()
                    .ToListAsync();
            }

            return await context.Pharma_CdrdmStudyAgent
                .Where(agent => agent.Nsc.ToLower() == nscCode.ToLower())
                .Select(agent => agent.DocumentNumber)
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// Returns the study agent record that matches the provided NSC code and Document Number.
        /// </summary>
        /// <param name="nscCode">The NSC code to filter by.</param>
        /// <param name="documentNumber">The Document Number to filter by.</param>
        public async Task<CdrdmStudyAgent> GetStudyAgentByNscAndDocumentNumberAsync(string nscCode, string documentNumber)
        {
            return await context.Pharma_CdrdmStudyAgent
                .FirstOrDefaultAsync(agent => agent.Nsc == nscCode && agent.DocumentNumber == documentNumber);
        }
    }
}
