using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DonationHub.Application.DTOs;
using DonationHub.Domain.Entities;

namespace DonationHub.Application.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper() {
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
