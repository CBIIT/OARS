using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IGroupService
    {
        public Task<IList<Group>> GetAllGroupsAsync();
        public Task<Group?> GetGroupAsync(int groupId);
        public bool CanDeleteGroup(int groupId);
        public bool CheckGroupName(string groupName, int groupId);
        public bool SaveGroup(Group group);
        public Tuple<bool, string> DeleteGroup(int groupId);

    }
}
