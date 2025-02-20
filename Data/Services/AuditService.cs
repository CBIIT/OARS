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
using RestSharp.Serialization.Json;

namespace TheradexPortal.Data.Services
{
    public class AuditService : BaseService, IAuditService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        private readonly IReviewService _reviewService;
        private readonly IReviewHistoryItemService _reviewHistoryItemService;
        private readonly IUserService _userService;
        public AuditService(IDatabaseConnectionService databaseConnectionService,
                                    IErrorLogService errorLogService,
                                    NavigationManager navigationManager,
                                    IReviewService reviewService,
                                    IReviewHistoryItemService reviewHistoryItemService,
                                    IUserService userService) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _reviewService = reviewService;
            _reviewHistoryItemService = reviewHistoryItemService;
            _userService = userService;
        }
        public async Task<List<AuditTrailDTO>> GetFullAuditTrailAsync(int userId, int reviewId, int reviewHistoryId, List<int> reviewHistoryItemIds,
            List<int> reviewHistoryNoteIds, List<int> reviewHistoryEmailIds)
        {
            List<AuditTrailDTO> auditTrail = new List<AuditTrailDTO>();
            // Audit reviewAuditTrail = await GetReviewAuditTrailAsync(userId, reviewId);
            var currentUser = await _userService.GetUserAsync(userId);
            var userName = currentUser.FirstName + " " + currentUser.LastName;
            var userEmail = currentUser.EmailAddress;

            //if (reviewAuditTrail != null)
            //{
            //    ProcessAuditEntry(reviewAuditTrail, userName, userEmail, auditTrail);
            //}

            if (reviewHistoryId != 0)
            {
                IList<Audit> reviewHistoryAuditTrail = await GetReviewHistoryAuditTrailAsync(userId, reviewHistoryId);
                if (reviewHistoryAuditTrail != null)
                {
                    foreach (var riAudit in reviewHistoryAuditTrail)
                    {
                        ProcessAuditEntry(riAudit, userName, userEmail, auditTrail);
                    }
                }
            }

            if (reviewHistoryItemIds.Count > 0)
            {
                IList<Audit> reviewItemAuditTrail = await GetReviewHistoryItemAuditTrailAsync(userId, reviewHistoryId, reviewHistoryItemIds);
                if (reviewItemAuditTrail != null)
                {
                    foreach (var riAudit in reviewItemAuditTrail)
                    {
                        ProcessAuditEntry(riAudit, userName, userEmail, auditTrail);
                    }
                }
            }
            if (reviewHistoryNoteIds.Count > 0)
            {
                IList<Audit> reviewNoteAuditTrail = await GetReviewNoteAuditTrailAsync(userId, reviewHistoryId, reviewHistoryNoteIds);
                if (reviewNoteAuditTrail != null)
                {
                    foreach (var riAudit in reviewNoteAuditTrail)
                    {
                        ProcessAuditEntry(riAudit, userName, userEmail, auditTrail);
                    }
                }
            }
            if (reviewHistoryEmailIds.Count > 0)
            {
                IList<Audit> reviewEmailAuditTrail = await GetReviewEmailAuditTrailAsync(userId, reviewHistoryId, reviewHistoryEmailIds);
                if (reviewEmailAuditTrail != null)
                {
                    foreach (var riAudit in reviewEmailAuditTrail)
                    {
                        ProcessAuditEntry(riAudit, userName, userEmail, auditTrail);
                    }
                }
            }
            return auditTrail;
        }

        private void ProcessAuditEntry(Audit auditEntry, string userName, string userEmail, List<AuditTrailDTO> auditTrail)
        {
            if (auditEntry != null)
            {
                if (auditEntry.AuditType == "Update")
                {
                    auditTrail.Add(
                        new AuditTrailDTO
                        {
                            userName = userName,
                            userEmail = userEmail,
                            dateOfChange = auditEntry.CreateDate,
                            typeOfChange = auditEntry.AuditType,
                            changeField = auditEntry.AffectedColumns,
                            previousValue = auditEntry.OldValues,
                            newValue = auditEntry.NewValues
                        });
                }
                else if (auditEntry.AuditType == "Create")
                {
                    auditTrail.Add(
                        new AuditTrailDTO
                        {
                            userName = userName,
                            userEmail = userEmail,
                            dateOfChange = auditEntry.CreateDate,
                            typeOfChange = auditEntry.AuditType,
                            changeField = auditEntry.TableName,
                            previousValue = "",
                            newValue = auditEntry.NewValues
                        });
                }
            }
        }

        private async Task<IList<Audit>> GetReviewEmailAuditTrailAsync(int userId, int reviewHistoryId, List<int> reviewHistoryEmailIds)
        {
            IList<Audit> localAuditCopy = null;
            string numberList = string.Join(",", reviewHistoryEmailIds);
            string sqlInts = "(" + string.Join(",", reviewHistoryEmailIds) + ")";
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0} AND TABLENAME = 'ReviewHistoryEmail' AND JSON_VALUE(PRIMARYKEY, '$.ReviewHistoryEmailId') IN " + sqlInts;

            var ret = await context.Audits
                    .FromSqlRaw(sqlQuery, userId)
                    .ToListAsync();

            if (ret != null)
            {
                localAuditCopy = ret.Select(audit => new Audit
                {
                    AuditId = audit.AuditId,
                    UserId = audit.UserId,
                    CreateDate = audit.CreateDate,
                    AuditType = audit.AuditType,
                    TableName = audit.TableName,
                    AffectedColumns = audit.AffectedColumns,
                    OldValues = audit.OldValues,
                    NewValues = audit.NewValues,
                    PrimaryKey = audit.PrimaryKey,
                    IsPrimaryTable = audit.IsPrimaryTable
                }).ToList();

                foreach (var item in localAuditCopy)
                {
                    var newValuesDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.NewValues);
                    string emailTo = newValuesDict["EmailToAddress"] ?? "Missing recipiant";
                    string emailText = newValuesDict["EmailText"] ?? "Missing Body";
                    item.TableName = "Email";
                    item.NewValues = "Email sent to: " + emailTo + "\nEmail Contents: " + emailText;
                }
            }
            return localAuditCopy;
        }

        private async Task<IList<Audit>> GetReviewNoteAuditTrailAsync(int userId, int reviewHistoryId, List<int> reviewHistoryNoteIds)
        {
            IList<Audit> localAuditCopy = null;
            string numberList = string.Join(",", reviewHistoryNoteIds);
            string sqlInts = "(" + string.Join(",", reviewHistoryNoteIds) + ")";
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0} AND TABLENAME = 'ReviewHistoryNote' AND JSON_VALUE(PRIMARYKEY, '$.ReviewHistoryNoteId') IN " + sqlInts;

            var ret = await context.Audits
                    .FromSqlRaw(sqlQuery, userId)
                    .ToListAsync();

            if (ret != null)
            {
                localAuditCopy = ret.Select(audit => new Audit
                {
                    AuditId = audit.AuditId,
                    UserId = audit.UserId,
                    CreateDate = audit.CreateDate,
                    AuditType = audit.AuditType,
                    TableName = audit.TableName,
                    AffectedColumns = audit.AffectedColumns,
                    OldValues = audit.OldValues,
                    NewValues = audit.NewValues,
                    PrimaryKey = audit.PrimaryKey,
                    IsPrimaryTable = audit.IsPrimaryTable
                }).ToList();

                foreach (var item in localAuditCopy)
                {
                    var newValuesDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.NewValues);
                    string noteText = newValuesDict["NoteText"] ?? "Missing Text";
                    item.TableName = "Note";
                    item.NewValues = "Added Note\nContents: " + noteText;
                }
            }
            return localAuditCopy;
        }

        private async Task<IList<Audit>> GetReviewHistoryItemAuditTrailAsync(int userId, int reviewHistoryId, List<int> reviewHistoryItemIds)
        {
            IList<Audit> localAuditCopy = null;
            string numberList = string.Join(",", reviewHistoryItemIds);
            string sqlInts = "(" + string.Join(",", reviewHistoryItemIds) + ")";
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0} AND TABLENAME = 'ReviewHistoryItem' AND JSON_VALUE(PRIMARYKEY, '$.ReviewHistoryItemId') IN " + sqlInts;

            var ret = await context.Audits
                    .FromSqlRaw(sqlQuery, userId)
                    .ToListAsync();
            string itemName;

            if (ret != null)
            {
                localAuditCopy = ret.Select(audit => new Audit
                {
                    AuditId = audit.AuditId,
                    UserId = audit.UserId,
                    CreateDate = audit.CreateDate,
                    AuditType = audit.AuditType,
                    TableName = audit.TableName,
                    AffectedColumns = audit.AffectedColumns,
                    OldValues = audit.OldValues,
                    NewValues = audit.NewValues,
                    PrimaryKey = audit.PrimaryKey,
                    IsPrimaryTable = audit.IsPrimaryTable
                }).ToList();


                foreach (var item in localAuditCopy)
                {
                    itemName = await _reviewHistoryItemService.GetReviewHistoryItemNameAsync(JsonConvert.DeserializeObject<Dictionary<string, int>>(item.PrimaryKey)["ReviewHistoryItemId"]);
                    var oldValuesDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.NewValues);
                    string checkedStatus = oldValuesDict["IsCompleted"] == "T" ? "Checked" : "Unchecked";

                    item.AffectedColumns = "Reivew Item";
                    item.TableName = "Review Item";
                    if (item.AuditType == "Update")
                    {
                        item.OldValues = "Review Item: " + itemName + "\nPrevious Value: " + checkedStatus;
                        item.NewValues = "Review Item: " + itemName + "\nPrevious Value: " + (checkedStatus == "Checked" ? "Unchecked" : "Checked");
                    }
                    else
                    {
                        item.NewValues = "Review Item: " + itemName + "\nSet to: " + checkedStatus;
                    }
                }
            }

            return localAuditCopy;
        }

        private async Task<IList<Audit>> GetReviewHistoryAuditTrailAsync(int userId, int reviewHistoryId)
        {
            IList<Audit> localAuditCopy = null;
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0} AND JSON_VALUE(PRIMARYKEY, '$.ReviewHistoryId') = {1}";

            var ret = await context.Audits
                    .FromSqlRaw(sqlQuery, userId, reviewHistoryId)
                    .ToListAsync();

            if (ret != null)
            {
                localAuditCopy = ret.Select(audit => new Audit
                {
                    AuditId = audit.AuditId,
                    UserId = audit.UserId,
                    CreateDate = audit.CreateDate,
                    AuditType = audit.AuditType,
                    TableName = audit.TableName,
                    AffectedColumns = audit.AffectedColumns,
                    OldValues = audit.OldValues,
                    NewValues = audit.NewValues,
                    PrimaryKey = audit.PrimaryKey,
                    IsPrimaryTable = audit.IsPrimaryTable
                }).ToList();


                foreach (var item in localAuditCopy)
                {
                    var oldValuesDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.NewValues);
                    string dueDate = oldValuesDict["DueDate"];
                    string reviewPeriodName = oldValuesDict["ReviewPeriodName"];

                    item.AffectedColumns = "Reivew Status";
                    item.TableName = "Review Status";
                    if (item.AuditType == "Update")
                    {
                        item.OldValues = "";
                        item.NewValues = "Review Completed.";
                    }
                    else
                    {
                        item.OldValues = "";
                        item.NewValues = "New Review Started\nDue Date: " + dueDate + "\nReview Period Name: " + reviewPeriodName;
                    }
                }
            }
            return localAuditCopy;
        }

        /* There should only ever be 1 "review" Audit per audit history we pull, the most recent. */
        private async Task<Audit> GetReviewAuditTrailAsync(int userId, int reviewId)
        {
            Audit localAuditCopy = null;
            string sqlQuery = "SELECT * FROM \"AUDIT\" WHERE USERID = {0} AND AFFECTEDCOLUMNS LIKE '%\"NextDueDate\"%' AND JSON_VALUE(PRIMARYKEY, '$.ReviewId') = {1} ORDER BY CREATEDATE DESC FETCH FIRST 1 ROWS ONLY";

            var ret = await context.Audits
                    .FromSqlRaw(sqlQuery, userId, reviewId)
                    .FirstOrDefaultAsync();

            if (ret != null)
            {
                localAuditCopy = new Audit();
                localAuditCopy.AuditId = ret.AuditId;
                localAuditCopy.UserId = ret.UserId;
                localAuditCopy.CreateDate = ret.CreateDate;
                localAuditCopy.AuditType = ret.AuditType;
                localAuditCopy.TableName = ret.TableName;
                localAuditCopy.OldValues = ret.OldValues;
                localAuditCopy.NewValues = ret.NewValues;
                localAuditCopy.PrimaryKey = ret.PrimaryKey;
                localAuditCopy.IsPrimaryTable = ret.IsPrimaryTable;

                // If one of the affected columns is "NextDueDate" then we have a review closed event
                // If it's ReviewPeriodUpcoming, then it's a change to the period
                var oldValuesDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(localAuditCopy.OldValues);
                var newValuesDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(localAuditCopy.NewValues);
                string oldDueDate = oldValuesDict["NextDueDate"] ?? "";
                string newDueDate = newValuesDict["NextDueDate"] ?? "";

                string oldPeriodName = oldValuesDict["ReviewPeriodName"] ?? "";
                string newPeriodName = newValuesDict["ReviewPeriodName"] ?? "";

                localAuditCopy.AffectedColumns = "Reivew Transition";
                localAuditCopy.OldValues = "Previous Reivew Due Date: " + oldDueDate + "\nPrevious Review Name: " + oldPeriodName;
                localAuditCopy.NewValues = "Next Review Due Date: " + newDueDate + "\nNext Review Name: " + newPeriodName;
            }

            return localAuditCopy;
        }
    }
}
