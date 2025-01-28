using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;
using TheradexPortal.Data.Services.Abstract;
using Oracle.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;

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
        public async Task<List<AuditTrailDTO>> GetFullAuditTrailAsync(int userId, int reviewId, int reviewHistoryId, List<int> reviewHistoryItemIds, List<int> reviewHistoryNoteIds)
        {
            List<AuditTrailDTO> auditTrail = new List<AuditTrailDTO>();
            Audit reviewAuditTrail = await GetReviewAuditTrailAsync(userId, reviewId);
            var currentUser = await _userService.GetUserAsync(userId);
            var userName = currentUser.FirstName + " " + currentUser.LastName;

            if (reviewAuditTrail != null)
            {
                ProcessAuditEntry(reviewAuditTrail, userName, userId, auditTrail);
            }

            if (reviewHistoryItemIds.Count > 0)
            {
                IList<Audit> reviewItemAuditTrail = await GetReviewHistoryItemAuditTrailAsync(userId, reviewHistoryId, reviewHistoryItemIds);
                if (reviewItemAuditTrail != null)
                {
                    foreach (var riAudit in reviewItemAuditTrail)
                    {
                        ProcessAuditEntry(riAudit, userName, userId, auditTrail);
                    }
                }
            }
            if (reviewHistoryItemIds.Count > 0)
            {
                IList<Audit> reviewNoteAuditTrail = await GetReviewNoteAuditTrailAsync(userId, reviewHistoryId, reviewHistoryNoteIds);
                if (reviewNoteAuditTrail != null)
                {
                    foreach (var riAudit in reviewNoteAuditTrail)
                    {
                        ProcessAuditEntry(riAudit, userName, userId, auditTrail);
                    }
                }
            }
            return auditTrail;
        }

        private void ProcessAuditEntry(Audit auditEntry, string userName, int userId, List<AuditTrailDTO> auditTrail)
        {
            if (auditEntry != null)
            {
                if (auditEntry.AuditType == "Update")
                {
                    var fieldList = ParseStrings(auditEntry.AffectedColumns);
                    JObject oldValueJsonObjects = JObject.Parse(auditEntry.OldValues);
                    JObject newValueJsonObjects = JObject.Parse(auditEntry.NewValues);
                    foreach (var key in fieldList)
                    {
                        auditTrail.Add(
                            new AuditTrailDTO
                            {
                                userName = userName,
                                userId = userId,
                                dateOfChange = auditEntry.CreateDate,
                                typeOfChange = auditEntry.AuditType,
                                changeField = key,
                                previousValue = oldValueJsonObjects[key] != null ? oldValueJsonObjects[key].ToString() : "null",
                                newValue = newValueJsonObjects[key] != null ? newValueJsonObjects[key].ToString() : "null"
                            });
                    }
                }
                else if (auditEntry.AuditType == "Create")
                {
                    JObject newValueJsonObjects = JObject.Parse(auditEntry.NewValues);
                    auditTrail.Add(
                        new AuditTrailDTO
                        {
                            userName = userName,
                            userId = userId,
                            dateOfChange = auditEntry.CreateDate,
                            typeOfChange = auditEntry.AuditType,
                            changeField = auditEntry.TableName,
                            previousValue = "",
                            newValue = string.Join("\n", newValueJsonObjects.Properties().Select(p => $"{p.Name}: {p.Value}"))
                        });
                }
            }
        }

        private List<string> ParseStrings(string input)
        {
            var result = JsonConvert.DeserializeObject<List<string>>(input);
            return result;
        }

        private async Task<IList<Audit>> GetReviewNoteAuditTrailAsync(int userId, int reviewHistoryId, List<int> reviewHistoryNoteIds)
        {
            string numberList = string.Join(",", reviewHistoryNoteIds);
            string sqlInts = "(" + string.Join(",", reviewHistoryNoteIds) + ")";
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0} AND TABLENAME = 'ReviewHistoryNote' AND JSON_VALUE(PRIMARYKEY, '$.ReviewHistoryNoteId') IN " + sqlInts;

            var ret = await context.Audits
                    .FromSqlRaw(sqlQuery, userId)
                    .ToListAsync();

            return ret;
        }

        private async Task<IList<Audit>> GetReviewHistoryItemAuditTrailAsync(int userId, int reviewHistoryId, List<int> reviewHistoryItemIds)
        {
            string numberList = string.Join(",", reviewHistoryItemIds);
            string sqlInts = "(" + string.Join(",", reviewHistoryItemIds) + ")";
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0} AND TABLENAME = 'ReviewHistoryItem' AND JSON_VALUE(PRIMARYKEY, '$.ReviewHistoryItemId') IN "+sqlInts;

            var ret = await context.Audits
                    .FromSqlRaw(sqlQuery, userId)
                    .ToListAsync();

            return ret;
        }

        private async Task<IList<Audit>> GetReviewHistoryAuditTrailAsync(int userId, int reviewHistoryId)
        {
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0} AND JSON_VALUE(PRIMARYKEY, '$.ReviewId') = {1}";

            var ret = await context.Audits
                    .FromSqlRaw(sqlQuery, userId)
                    .ToListAsync();

            return ret;
        }

        /* There should only ever be 1 "review" Audit per audit history we pull, the most recent. */
        private async Task<Audit> GetReviewAuditTrailAsync(int userId, int reviewId)
        {
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0} AND JSON_VALUE(PRIMARYKEY, '$.ReviewId') = {1} ORDER BY CREATEDATE DESC FETCH FIRST 1 ROWS ONLY";

            var ret = await context.Audits
                    .FromSqlRaw (sqlQuery, userId, reviewId)
                    .FirstOrDefaultAsync();

            return ret;
        }
    }
}
