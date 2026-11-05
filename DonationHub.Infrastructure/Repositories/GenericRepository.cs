using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default) =>await _context.Set<T>().ToListAsync();

        //public async Task<IEnumerable<Campaign>> GetAllIncludingDeletedAsync() => await _context.Set<Campaign>().IgnoreQueryFilters().ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) => await _context.Set<T>().FindAsync(id);
       
        public async Task Remove(Guid id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            if (result != null) _context.Set<T>().Remove(result);

            //await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
             _context.Set<T>().Update(entity);
            //await _context.SaveChangesAsync();
        }
    }
}
