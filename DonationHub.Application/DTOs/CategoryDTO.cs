using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Application.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string NameEn { get; set; } = default!;
        public string NameAr { get; set; } = default!;
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
    }
}
