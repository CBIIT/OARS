	using OARS.Data.Models;
	
	namespace OARS.Data.Services.Abstract
	{
	    public interface IProfileService
	    {
	        public Task<IList<Profile>> GetProfiles();
			public Task<Profile?> GetProfile(int profileId);
	        public Task<int?> SaveProfile(Profile profile);
	    }
}