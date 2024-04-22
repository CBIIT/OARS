using Oracle.ManagedDataAccess.Types;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolDataCategoryService
    {
        public Task<List<ProtocolDataCategory>> GetCategoriesByMappingId(int mappingId);
        public Task<bool> SaveCategory(ProtocolDataCategory category, int mappingId);
        public Task<ProtocolDataCategory> GetCategory(int categoryId);
    }
}
