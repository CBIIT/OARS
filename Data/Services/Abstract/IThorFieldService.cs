using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IThorFieldService
    {
        public Task<IList<ThorField>> GetFields();
        public Task<IList<ThorField>> GetFields(string categoryId);
        public Task<IList<ThorField>> GetFieldsForMapping(int mappingId);
        public Task<bool> SaveField(ThorField field);
        public Task<IList<ThorFieldType>> GetFieldTypes();
    }
}
