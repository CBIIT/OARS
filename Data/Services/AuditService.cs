using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;
using TheradexPortal.Data.Services.Abstract;
using Oracle.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace TheradexPortal.Data.Services
{
    public class AuditService: BaseService, IAuditService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;
        public AuditService(IDatabaseConnectionService databaseConnectionService,
                                    IErrorLogService errorLogService,
                                    NavigationManager navigationManager,
                                    IReviewService reviewService,
                                    IUserService userService) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _reviewService = reviewService;
            _userService = userService;
        }

        public async Task<IList<Audit>> GetReviewAuditTrailAsync(int userId, int reviewId)
        {
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0}";

            var ret = await context.Audits
                    .FromSqlRaw (sqlQuery, userId)
                    .ToListAsync();

            return ret;
        }
    }
}
