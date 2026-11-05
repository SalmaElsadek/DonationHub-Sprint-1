using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DonationHub.Application.DTOs;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;

namespace DonationHub.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _generic;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Campaign> _campaignRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IGenericRepository<Category> generic,IMapper mapper,IGenericRepository<Campaign> campaignRepo,IUnitOfWork unitOfWork)
        {
             _generic = generic;
            _mapper = mapper;
            _campaignRepo = campaignRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CategoryDTO> CreateAsync(CategoryDTO model)
        {
           var result=_mapper.Map<Category>(model);
            await _generic.AddAsync(result);
            await _unitOfWork.CompeleteAsync();
            return _mapper.Map<CategoryDTO>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            var result= await _generic.GetByIdAsync(id);
            if (result == null) throw new KeyNotFoundException("Category.NotFound");
            
            var campaigns= await _campaignRepo.GetAllAsync();
            var hasCampaign=campaigns.Any(c=>c.CategoryId==id);

            if(hasCampaign) throw new InvalidOperationException("Category.HasCampaigns"); //409 Conflict

            _generic.Remove(id);
            await _unitOfWork.CompeleteAsync();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var result = await _generic.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(result);
        }

        public async Task<CategoryDTO> GetByIdAsync(Guid id)
        {
            var result= await _generic.GetByIdAsync(id);
            if (result == null) throw new KeyNotFoundException("Category.NotFound");
            return _mapper.Map<CategoryDTO>(result);
        }

        public async Task UpdateAsync(Guid id, CategoryDTO model)
        {
            var result = await _generic.GetByIdAsync(id);
            if (result == null) throw new KeyNotFoundException("Category.NotFound");
            _mapper.Map<CategoryDTO>(result);
            _generic.Update(result);
            await _unitOfWork.CompeleteAsync();
        }
    }
}
