namespace TheradexPortal.Data.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Security.Permissions;
    using TheradexPortal.Data.Models;
    using TheradexPortal.Data.Services.Abstract;
    using TheradexPortal.Data.ViewModels;

    public class StudyService : BaseService, IStudyService
    {
        private readonly ILogger<StudyService> logger; // Add logger field
        public StudyService(ILogger<StudyService> logger, IDatabaseConnectionService databaseConnectionService, IHttpContextAccessor httpContextAccessor) : base(databaseConnectionService) {
            this.logger = logger; // Initialize logger
        }
        
        public async Task<IList<Protocol>> GetAllProtocolsAsync()
        {
            return await context.Protocols.ToListAsync();
        }

        public IList<Protocol> GetProtocolsForUserAsync(int userId, bool allStudies)
        {
            List<Protocol> protocols = new List<Protocol>();

            // Use tables USER_PROTOCOL, USER_GROUP and GROUPPROTOCOL to get list of studies for the user
            if (!allStudies)
            { 
            protocols = (from up in context.User_Protocols
                             join p in context.Protocols on up.StudyId equals p.StudyId
                             where up.UserId == userId && (up.ExpirationDate == null || up.ExpirationDate.Value.Date >= DateTime.UtcNow.Date) && p.HideStudy.ToUpper()=="NO"
                             select p)
                             .Union(from ug in context.User_Groups
                                    join ugp in context.Group_Protocols on ug.GroupId equals ugp.GroupId where ugp.IsActive
                                    join p in context.Protocols on ugp.StudyId equals p.StudyId
                                    where ug.UserId == userId && (ug.ExpirationDate == null || ug.ExpirationDate.Value.Date >= DateTime.UtcNow.Date && p.HideStudy.ToUpper() == "NO")
                                    select p)
                             .OrderBy(p1 => p1.StudyId).ToList();
            }
            else
                protocols = (from p in context.Protocols
                             where p.HideStudy.ToUpper() == "NO"
                            select p).ToList();

            return protocols;
        }

        public string GetCurrentStudiesForUser(int userId)
        {

            var protocolList = (from u in context.User_Selected_Protocols
                                where u.UserId == userId
                                select u.Current_Protocols).SingleOrDefault();

            return protocolList;
        }

        public string GetSelectedStudyIdsForUser(int userId)
        {
            var studyList = (from u in context.User_Selected_Protocols
                                where u.UserId == userId
                                select u.Selected_Protocols).SingleOrDefault();

            return studyList;
        }

        public List<Protocol> GetSelectedStudiesForUser(int userId)
        {
            string[] studyListArray;
            string studyList = GetSelectedStudyIdsForUser(userId);

            if (studyList != null && studyList != "")
                studyListArray = studyList.Split(',');
            else
                studyListArray = Array.Empty<string>();

            List<Protocol> protocols = (from p in context.Protocols
                            where studyListArray.Contains(p.StudyId)
                            select p).ToList();

            return protocols;
        }

        public async Task<String> GetStudyTitleAsync(string protocolID)
        {
            var studyTitle = await context.Protocols.Where(s => s.StudyId == protocolID.ToString())
                .Select(pt => pt.ProtocolTitle ?? "").FirstOrDefaultAsync();

            return studyTitle;
        }
    }
}