using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using Microsoft.AspNetCore.Components;

namespace TheradexPortal.Data.Services
{
    public class GroupService : BaseService, IGroupService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public GroupService(IDbContextFactory<WrDbContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<Group>> GetAllGroupsAsync()
        {
            return await context.Groups.Include(gp=>gp.GroupProtocols).OrderBy(g => g.GroupName).ToListAsync();
        }

        public async Task<Group?> GetGroupAsync(int groupId)
        {
            //return await context.Groups.FindAsync(groupId);
            //.Include(gp => gp.GroupProtocols)
            return await context.Groups.Include(gp => gp.GroupProtocols).Where(g=>g.WRGroupId==groupId).SingleOrDefaultAsync();
        }

        public bool CanDeleteGroup(int groupId)
        {
            return context.User_Groups.Where(ug=>ug.GroupId==groupId).Count() == 0;
        }

        public bool CheckGroupName(string groupName, int groupId)
        {
            Group foundGroup = context.Groups.FirstOrDefault(g => g.GroupName == groupName && g.WRGroupId != groupId);
            return foundGroup == null;
        }

        public bool SaveGroup(Group group, int userId)
        {
            DateTime curDateTime = DateTime.UtcNow;
            try
            {
                // if user.UserId is 0 or null, save new user, else edit user
                if (group.WRGroupId == 0)
                {
                    group.CreateDate = curDateTime;
                    context.Groups.Add(group);
                    context.SaveChanges();
                }
                else
                {
                    Group dbGroup = context.Groups.FirstOrDefault(g => g.WRGroupId == group.WRGroupId);
                    if (dbGroup != null)
                    {
                        dbGroup.GroupName = group.GroupName;
                        dbGroup.UpdateDate = curDateTime;

                        // iterate from site defined list - add or update as needed
                        foreach (GroupProtocol gp in group.GroupProtocols)
                        {
                            GroupProtocol foundGP = dbGroup.GroupProtocols.FirstOrDefault(og=>og.StudyId==gp.StudyId);
                            if (foundGP != null)
                            {
                                // Study already exists
                                foundGP.IsActive = gp.IsActive;
                                foundGP.UpdateDate = curDateTime;
                            }
                            else
                            {
                                // Study doesn't exist in db
                                GroupProtocol newGP = new GroupProtocol();
                                newGP.StudyId = gp.StudyId;
                                newGP.IsActive = gp.IsActive;
                                newGP.CreateDate = curDateTime;
                                dbGroup.GroupProtocols.Add(newGP);
                            }
                        }
                        // Remove protocols in db not in new list
                        List<GroupProtocol> gpToRemove = new List<GroupProtocol>();
                        foreach (GroupProtocol gp in dbGroup.GroupProtocols)
                        {
                            GroupProtocol foundGP = group.GroupProtocols.FirstOrDefault(og => og.StudyId == gp.StudyId);
                            if (foundGP == null)
                                gpToRemove.Add(gp);
                        }
                        foreach (GroupProtocol del in gpToRemove)
                            dbGroup.GroupProtocols.Remove(del);

                        context.SaveChanges();
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public Tuple<bool, string> DeleteGroup(int groupId, int userId)
        {
            try
            {
                var group = context.Groups.Where(g=>g.WRGroupId==groupId).Include(g => g.GroupProtocols).First();
                context.Remove(group);
                context.SaveChanges();
                return new Tuple<bool, string>(true,"Group deleted successfully");
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return new Tuple<bool, string>(false,"Failed to delete group");
            }
        }
    }
}
