using AutoMapper;
using NetShip.DTOs.Category;
using NetShip.DTOs.Product;
using NetShip.Entities;

namespace NetShip.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(x => x.Icon, options => options.Ignore());
            CreateMap<Category, CategoryDTO>();

            CreateMap<CreateProductDTO, Product>()
                .ForMember(x => x.Icon, options => options.Ignore());
            CreateMap<Product, ProductDTO>();
        }
    }
}
