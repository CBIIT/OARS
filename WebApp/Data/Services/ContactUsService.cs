﻿using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;
using OARS.Data.Static;

namespace OARS.Data.Services
{
    public class ContactUsService : BaseService, IContactUsService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        protected readonly ILogger<ContactUsService> logger;
        public ContactUsService(ILogger<ContactUsService> logger, IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            this.logger = logger;
        }

        public List<ContactUsCategory> GetContactUsCategories()
        {
            List<ContactUsCategory> lstCUCategory = context.ContactUsCategory.ToList();
            return lstCUCategory;
        }

        public List<ContactUsCategorySelectItemList> GetContactUsCategoriesSelectItems()
        {
            List<ContactUsCategorySelectItemList> lstSelectItems = new List<ContactUsCategorySelectItemList>();
            List<ContactUsCategorySelectItem> lstSecondLevel = new List<ContactUsCategorySelectItem>();
            List<ContactUsCategory> lstCUCategory = GetContactUsCategories();
            List<ContactUsCategory> lstTopLevelCategory = lstCUCategory.Where(l => l.ParentCategoryId == null).OrderBy(o => o.Order).ToList();
            ContactUsCategorySelectItemList newItemList;
            int categoryID;
            bool isGroupNode;

            List<ContactUsCategory> categorySecondLevel;
            foreach (ContactUsCategory cuc in lstTopLevelCategory)
            {
                categoryID = cuc.ContactUsCategoryID;
                categorySecondLevel = lstCUCategory.Where(l => l.ParentCategoryId == categoryID).OrderBy(o => o.Order).ToList();
                isGroupNode = categorySecondLevel.Count > 0;
                lstSecondLevel = new List<ContactUsCategorySelectItem>();
                if (isGroupNode)
                {
                    foreach (ContactUsCategory secondCUC in categorySecondLevel)
                    {
                        lstSecondLevel.Add(new ContactUsCategorySelectItem(secondCUC.ContactUsCategoryID, secondCUC.Name));
                    }
                    newItemList = new ContactUsCategorySelectItemList(categoryID, cuc.Name, isGroupNode, lstSecondLevel);
                    lstSelectItems.Add(newItemList);
                }
                else
                {
                    lstSelectItems.Add(new ContactUsCategorySelectItemList(categoryID, cuc.Name, false, lstSecondLevel));
                }

            }
            return lstSelectItems;
        }

        public bool SaveContactUs(int userId, ContactUs contactUS)
        {
            try
            {
                contactUS.CreateDate = DateTime.UtcNow;
                context.ContactUs.Add(contactUS);
                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
