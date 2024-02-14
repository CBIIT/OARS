	using Microsoft.AspNetCore.Components;
	using Microsoft.EntityFrameworkCore;
	using TheradexPortal.Data.Models;
	using TheradexPortal.Data.Services.Abstract;
	namespace TheradexPortal.Data.Services
	{
	    public class ProfileService : BaseService, IProfileService
	    {
	        private readonly IErrorLogService _errorLogService;
	        private readonly NavigationManager _navManager;
	        public ProfileService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
	        {
	            _errorLogService = errorLogService;
	            _navManager = navigationManager;
	        }
	        public async Task<IList<Profile>> GetProfiles()
	        {
	            return await context.Profiles.OrderBy(c => c.ProfileName).ToListAsync();
	        }
	        
	        public async Task<bool> SaveProfile(Profile profile)
	        {
	            DateTime currentDateTime = DateTime.UtcNow;
	            try
	            {
	                profile.UpdateDate = currentDateTime;
	                context.Add(profile);
	                await context.SaveChangesAsync();
	                return true;
	            }
	            catch (Exception ex)
	            {
	                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
	                return false;
	            }
	        }
    }
	}
