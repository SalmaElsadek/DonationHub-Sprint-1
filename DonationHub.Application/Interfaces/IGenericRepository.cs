using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Common;

namespace DonationHub.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct=default);

        //ابقي اعمليها بعدين في ال CampaignRepo
        //Task<IEnumerable<T>> GetAllIncludingDeletedAsync();

        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<T> AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        Task Remove(Guid id);
    }
}
