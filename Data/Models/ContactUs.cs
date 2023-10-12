using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("WRCONTACTUSCATEGORY", Schema = "WRUSER")]
    public class ContactUsCategory
    {
        [Key]
        public int WRContactUsCategoryID { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? EmailTo { get; set; }
        public int? Order { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    [Table("WRCONTACTUS", Schema = "WRUSER")]
    public class ContactUs
    {
        [Key]
        public int WRContactUsID { get; set; }
        public string? Subject { get; set; }
        public string Description { get; set; }
        public int? CategoryID { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UserID { get; set; }
        public string AttachmentName { get; set; }

    }

    public class ContactUsCategorySelectItemList
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }

        public bool IsGroupLevel { get; set; }
        public List<ContactUsCategorySelectItem> SecondLevel { get; set; }

        public ContactUsCategorySelectItemList(int categoryID, string name, bool isGroupLevel,  List<ContactUsCategorySelectItem> lstSecondLevel)
        {
            CategoryID = categoryID;
            Name = name;
            IsGroupLevel = isGroupLevel;
            SecondLevel = lstSecondLevel;
        }
    }

    public class ContactUsCategorySelectItem
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }

        public ContactUsCategorySelectItem(int categoryID, string name)
        {
            CategoryID = categoryID;
            Name = name;
        }
    }
}
