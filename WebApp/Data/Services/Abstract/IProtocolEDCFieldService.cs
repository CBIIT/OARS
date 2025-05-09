﻿using System.Data;
using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolEDCFieldService
    {
        public Task<List<ProtocolEDCField>> GetFieldsByFormIds(List<int> formId);
        public Task<List<ProtocolEDCField>> GetFieldsByProtocolMappingId(int protocolMappingId);
        public Task<bool> SaveField(ProtocolEDCField field);
        public Task<bool> DeleteField(ProtocolEDCField field);
        public Task<bool> BulkSaveFields(DataTable fields);
        public Task<bool> DeleteAllFieldsForFormIds(int protocolMappingId);
    }
}
