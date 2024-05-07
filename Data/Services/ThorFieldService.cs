using System.Collections.Generic;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ThorFieldService : BaseService, IThorFieldService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ThorFieldService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ThorFieldType>> GetFieldTypes()
        {
            return await context.THORFieldType.OrderBy(c => c.FieldTypeName).ToListAsync();
        }

        public async Task<IList<ThorField>> GetFields()
        {
            return await context.THORField.Include(f => f.Category).OrderBy(c => c.SortOrder).ToListAsync();
        }

        public async Task<IList<ThorField>> GetFields(string categoryId)
        {
            return await context.THORField.Where(f => f.ThorDataCategoryId == categoryId).OrderBy(c => c.SortOrder).ToListAsync();
        }

        public async Task<bool> SaveField(ThorField field)
        {
            if(field.ThorDataCategoryId.IsNullOrEmpty())
            {
                return false;
            }
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                ThorField currentField = context.THORField.Where(p => p.ThorFieldId == field.ThorFieldId).FirstOrDefault();

                if (currentField == null || field.CreateDate == null)
                {
                    field.CreateDate = currentDateTime;
                    field.UpdateDate = currentDateTime;
                    context.Add(field);
                }
                else
                {
                    //Can we change the ThorIDfield?
                    currentField.ThorFieldId = field.ThorFieldId;
                    currentField.ThorDataCategoryId = field.ThorDataCategoryId;
                    currentField.FieldLabel = field.FieldLabel;
                    currentField.FieldType = field.FieldType;
                    currentField.Derivable = field.Derivable;
                    currentField.ThorDictionaryId = field.ThorDictionaryId;
                    currentField.IsMultiForm = field.IsMultiForm;
                    currentField.SortOrder = field.SortOrder;
                    currentField.IsActive = field.IsActive;
                    currentField.UpdateDate = currentDateTime;
                    context.Update(currentField);
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
    }
}
