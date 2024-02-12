using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IThorFieldService
    {
        public Task<IList<ThorField>> GetFields();
        public bool SaveField(ThorField field);
        public Task<IList<ThorFieldType>> GetFieldTypes();
    }
}
