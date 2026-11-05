using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Application.Options
{
    public class DonationOptions
    {
        public const string SectionName = "Donation";
        [Range(1, 1000000)]
        public decimal MinAmount { get; set; } = 1.00m;
        [Range(1, 10000000)]
        public decimal MaxAmount { get; set; } = 10000.00m;
        [Range(1, 60*24)]
        public int PendingTimeoutMinutes { get; set; } = 30;
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string DefaultCurrency { get; set; } = "USD";
    }
}
