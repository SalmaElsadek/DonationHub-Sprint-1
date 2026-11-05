using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Application.DTOs;

namespace DonationHub.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO>GetByIdAsync(Guid id);
        Task<CategoryDTO> CreateAsync(CategoryDTO model);
        Task UpdateAsync(Guid id, CategoryDTO model);
        Task DeleteAsync(Guid id);
    }
}
