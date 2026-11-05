using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Common;

namespace DonationHub.Domain.Entities
{
    public class Category:BaseEntity
    {
        // EF Core constructor
        //بعمله عشان يقدر يعمل Materialize for queries when it comes from Db
        private Category()
        {
        }

        public string NameAr { get;private set; } = default!;
        public string NameEn { get;private set; } = default!;
        public string DescriptionAr { get;private set; } = default!;
        public string DescriptionEn { get;private set; } = default!;
        public DateTimeOffset UpdatedAt { get; private set; }

        // private ICollection<Campaign> Campaigns { get; set; } count will alwayes return null
        private IReadOnlyCollection<Campaign> Campaigns = [];

        //دي كدا ال factory Method 
        public static Category Create(string nameAr, string nameEn, string descriptionAr, string descriptionEn, DateTimeOffset time)
        {
            ValidateName(nameAr, nameEn);
            return new Category
            {
                NameAr = nameAr,
                NameEn = nameEn,
                DescriptionAr = descriptionAr,
                DescriptionEn = descriptionEn,
                UpdatedAt = DateTimeOffset.UtcNow,
            };
        }
        public void UpdateCreate(string nameAr, string nameEn, string descriptionAr, string descriptionEn, DateTimeOffset time)
        {
            ValidateName(nameAr, nameEn);

            NameAr = nameAr;
            NameEn = nameEn;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            UpdatedAt = DateTimeOffset.UtcNow;
            
        }
        //Rich Domain Model
            /*
            لو متأكدتش هنا اي كود في ال application layer 
            ممكن يعمل new category()
            ممكن يدخل قيم غليط ومش هعرف انها غلط غير لما اجي اسجلها في الداتا بيز ووقتها ممكن ترمي اكسبشن
             */
        private static void ValidateName(string nameAr,string nameEn)
        {
            if(string.IsNullOrWhiteSpace(nameAr)) 
                throw new ArgumentNullException("Arabic Name is required",nameof(nameAr));

            if (string.IsNullOrWhiteSpace(nameEn))
                throw new ArgumentNullException("English Name is required", nameof(nameEn));

            if (nameEn.Length > 100 || nameAr.Length > 100)
                throw new ArgumentException("The names can't be more than 100 characters", nameof(nameEn));
        }
        public bool CanBeDeleted()=>Campaigns.Count==0;
         
    }
}
