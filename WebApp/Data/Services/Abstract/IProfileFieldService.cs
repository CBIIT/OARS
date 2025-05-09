﻿using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProfileFieldService
    {
        public Task<IList<ProfileField>> GetProfileFields(int profileId);
        public Task<IList<ThorField>> GetProfileFieldsFromDataCategory(string thorDataCategoryId);
        public Task<bool> SaveProfileField(int profileId, ProfileField profileField);
        public Task<bool> DeleteProfileField(ProfileField field);
    }
}
