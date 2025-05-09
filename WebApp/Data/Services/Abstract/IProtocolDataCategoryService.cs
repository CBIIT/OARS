﻿using Oracle.ManagedDataAccess.Types;
using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolDataCategoryService
    {
        public Task<List<ProtocolDataCategory>> GetCategoriesByMappingId(int mappingId);
        public Task<List<ProtocolDataCategory>> GetCategoriesByMappingProfile(int mappingId);
        public Task<ProtocolDataCategory> BuildDefaultProtocolDataCategory(ThorCategory category, int mappingId);
        public Task<ProtocolDataCategory> GetOrBuildProtocolDataCategory(int mappingId, string thorDataCategoryId);
        public Task<bool> SaveCategory(ProtocolDataCategory category, int mappingId);
        public Task<ProtocolDataCategory?> GetCategory(int categoryId);
    }
}
