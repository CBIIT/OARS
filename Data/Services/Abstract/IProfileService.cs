	using TheradexPortal.Data.Models;
	
	namespace TheradexPortal.Data.Services.Abstract
	{
	    public interface IProfileService
	    {
	        public Task<IList<Profile>> GetProfiles();
	        public Task<bool> SaveProfile(Profile profile);
	    }
}