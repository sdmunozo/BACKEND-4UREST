using AutoMapper;
using NetShip.DTOs.Auth;
using NetShip.DTOs.Branch;
using NetShip.DTOs.Brand;
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

            CreateMap<CreateBrandDTO, Brand>();
            CreateMap<Brand, BrandDTO>();

            CreateMap<CreateBranchDTO, Branch>();
            CreateMap<Branch, BranchDTO>();
        }
    }
}
