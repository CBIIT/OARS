using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IContactUsService
    {
        public List<ContactUsCategory> GetContactUsCategories();
        public List<ContactUsCategorySelectItemList> GetContactUsCategoriesSelectItems();

        public bool SaveContactUs(int userId, ContactUs contactUSEmail);
    }
}
