using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Application.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        Task<int> CompeleteAsync(CancellationToken ct = default);
    }
}
