using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Common;
using DonationHub.Domain.Enum;
using DonationHub.Domain.ValueObjects;
namespace DonationHub.Domain.Entities
{
    public class Campaign : BaseEntity, ISoftDeletable
    {
        public Campaign()
        {
        }

        public string TitleAr { get; private set; } = default!;
        public string TitleEn { get; private set; } = default!;
        public string DescriptionAr { get; private set; } = default!;
        public string DescriptionEn { get; private set; } = default!;
        public Money GoalAmount { get; private set; }
        public decimal RaisedAmount { get; private set; }
        public DateTimeOffset EndDate { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }
        public string? BannerFileName { get;private set; } = default!;
        public CampaignStatus Status { get;private set; }
        public Guid CreatedByUserId { get;private set; }
        public DateTimeOffset UpdatedAt { get; private set; }
        //Ask1
        public byte[] RowVersion { get; private set; } = [];

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        private IReadOnlyCollection<Donation> Donations = [];

        public static Campaign Create(string titleAr,string titleEn,string descriptionAr
            ,string descriptionEn, Money goalAmount,DateTimeOffset endDate,
        Guid categoryId,Guid createdByUserId,DateTimeOffset time,
        string? bannerFileName = null)
        {
            if (endDate <= time)
                throw new ArgumentException("End time should not be Expired", nameof(endDate));
            
            if (string.IsNullOrWhiteSpace(titleEn) || string.IsNullOrWhiteSpace(titleAr))
                throw new ArgumentException("Titles are required",nameof(titleEn));

            if (createdByUserId == Guid.Empty)
                throw new ArgumentException("User Id Can't be null", nameof(createdByUserId));

            return new Campaign
            {
                TitleAr = titleAr,
                TitleEn = titleEn,
                DescriptionAr = descriptionAr,
                DescriptionEn = descriptionEn,
                GoalAmount = goalAmount,
                RaisedAmount = 0,
                EndDate = endDate,
                CategoryId = categoryId,
                CreatedByUserId = createdByUserId,
                Status = CampaignStatus.Active,
                BannerFileName = bannerFileName,
            };

        }
        public void UpdateEndDate(DateTimeOffset time,DateTimeOffset endDate)
        {
            if (Status == CampaignStatus.Completed || Status == CampaignStatus.Expired)
                throw new InvalidOperationException("Can't change the EndDate of completed or expired campaign");

            if (endDate <= time) throw new ArgumentException("the new date should be in the future");

            EndDate = endDate;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        //GoalAmount cannot be lowered below the current RaisedAmount.
        public void UpdateGoalAmount(Money goalAmount)
        {
            if (goalAmount.Amount <= 0)
                throw new ArgumentException("The amount should be greater than 0", nameof(goalAmount));
            if (goalAmount.Amount < RaisedAmount)
                throw new ArgumentException("GoalAmount cannot be lowered below the current RaisedAmount.");

            GoalAmount = goalAmount;
            UpdatedAt= DateTimeOffset.UtcNow;

        }  
        public bool CanAcceptDonation(DateTimeOffset time)
        {
            if (Status == CampaignStatus.Active && EndDate > time && !IsDeleted) return true;
            else return false;
        }

        public void AddDonation(decimal money,DateTimeOffset now)
        {
            if (!CanAcceptDonation(now))
                throw new InvalidOperationException("Can't accept donations now");
            
            if(money< 0) throw new ArgumentException("The Donation money should be positive",nameof(money));

            RaisedAmount += money;
            UpdatedAt = DateTimeOffset.UtcNow;

            if (RaisedAmount >= GoalAmount.Amount) MakeItCompleted();
        }

        public void MakeItCompleted()
        {
            if (Status == CampaignStatus.Completed || Status == CampaignStatus.Expired) return;

            Status = CampaignStatus.Completed;
            UpdatedAt=DateTimeOffset.UtcNow;
        }
        public void MakeItExpired(DateTimeOffset time)
        {
            if (Status == CampaignStatus.Completed || Status == CampaignStatus.Expired) return;
            if (EndDate <= time)
            {
                Status= CampaignStatus.Expired;
                UpdatedAt= DateTimeOffset.UtcNow;
            }
        }
        //A campaign that has donations cannot be hard-deleted
        public bool CanBeDeleted() => Donations.Count == 0;
        
    }
}
