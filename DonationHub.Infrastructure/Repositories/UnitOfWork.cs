using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Application.Interfaces;
using DonationHub.Infrastructure.Data;

namespace DonationHub.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CompeleteAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
