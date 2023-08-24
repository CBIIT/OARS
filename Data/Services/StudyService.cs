namespace TheradexPortal.Data.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Security.Permissions;
    using TheradexPortal.Data.Models;
    using TheradexPortal.Data.Services.Abstract;
    using TheradexPortal.Data.ViewModels;

    public class StudyService : BaseService, IStudyService
    {
        public StudyService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }
        
        public async Task<IList<Protocol>> GetAllProtocolsAsync()
        {
            return await context.Protocols.ToListAsync();
        }

        public IList<Protocol> GetProtocolsForUserAsync(int userId, bool isAdmin)
        {
            List<Protocol> protocols = new List<Protocol>();

            // Use tables WRUSER_PROTOCOL, WR_USER_GROUP and WR_GROUPPROTOCOL to get list of studies for the user
            if (!isAdmin)
            { 
            protocols = (from up in context.User_Protocols
                             join p in context.Protocols on up.StudyId equals p.StudyId
                             where up.UserId == userId && (up.ExpirationDate == null || up.ExpirationDate.Value.Date >= DateTime.Today)
                             select p)
                             .Union(from ug in context.User_Groups
                                    join ugp in context.Group_Protocols on ug.GroupId equals ugp.GroupId where ugp.IsActive
                                    join p in context.Protocols on ugp.StudyId equals p.StudyId
                                    where ug.UserId == userId && (ug.ExpirationDate == null || ug.ExpirationDate.Value.Date >= DateTime.Today)
                                    select p)
                             .OrderBy(p1 => p1.StudyId).ToList();
            }
            else
                protocols = (from p in context.Protocols
                            select p).ToList();

            return protocols;
        }

        public IList<Protocol> GetCurrentStudiesForUser(int userId)
        {

            var protocolList = (from u in context.Users
                                where u.UserId == userId
                                select u.CurrentStudy).SingleOrDefault();

            if (protocolList != null)
            {
                string[] protocolArray = protocolList.Split(',');
                var protocols = (from p in context.Protocols
                                 where protocolArray.Contains(p.StudyId)
                                 select p).ToList();

                return protocols;
            }
            else
                return new List<Protocol>();
        }

        public string GetSelectedStudyIdsForUser(int userId)
        {
            var studyList = (from u in context.Users
                                where u.UserId == userId
                                select u.SelectedStudies).SingleOrDefault();

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
    }
}