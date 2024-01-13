using AutoMapper;
using NetShip.DTOs.Category;
using NetShip.Entities;

namespace NetShip.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
        }
    }
}
