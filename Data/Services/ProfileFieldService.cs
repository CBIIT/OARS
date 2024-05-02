using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProfileFieldService : BaseService, IProfileFieldService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public ProfileFieldService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ProfileField>> GetProfileFields(int profileId)
        {
            return await context.ProfileFields.Include(x=>x.ThorField).ThenInclude(y=>y.Category).Where(c => c.ProfileId == profileId).ToListAsync();
        }

        public async Task<bool> SaveProfileField(int profileId, ProfileField profileField)
        {
            try
            {
                if (profileField.THORFieldId == null) return false;
                
                DateTime currentDateTime = DateTime.UtcNow;
                ProfileField currentProfileField = context.ProfileFields.Where(p => p.ProfileFieldId == profileField.ProfileFieldId).FirstOrDefault();

                if (currentProfileField == null || profileField.CreateDate == null)
                {
                    profileField.ProfileId = profileId;
                    profileField.THORFieldId = profileField.THORFieldId;
                    profileField.CreateDate = currentDateTime;
                    context.Add(profileField);
                }
                else
                {
                    currentProfileField.THORFieldId = profileField.THORFieldId;
                    context.Update(currentProfileField);
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

        public async Task<IList<ThorField>> GetProfileFieldsFromDataCategory(string thorDataCategoryId)
        {
            return await context.THORField.Where(c => c.ThorDataCategoryId.Equals(thorDataCategoryId)).ToListAsync();
        }
    }
}
