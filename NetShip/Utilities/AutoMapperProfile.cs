using AutoMapper;
using NetShip.DTOs;
using NetShip.DTOs.Auth;
using NetShip.DTOs.Branch;
using NetShip.DTOs.Brand;
using NetShip.DTOs.CatalogDTOs;
using NetShip.DTOs.Category;
using NetShip.DTOs.Item;
using NetShip.DTOs.Items;
using NetShip.DTOs.Modifier;
using NetShip.DTOs.ModifiersGroup;
using NetShip.DTOs.Platform;
using NetShip.DTOs.Product;
using NetShip.Entities;

namespace NetShip.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Branch, DTOs.DigitalMenu.BranchCatalogResponse>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.InstagramLink, opt => opt.MapFrom(src => src.Brand.Instagram))
                .ForMember(dest => dest.FacebookLink, opt => opt.MapFrom(src => src.Brand.Facebook))
                .ForMember(dest => dest.WebsiteLink, opt => opt.MapFrom(src => src.Brand.Website))
                .ForMember(dest => dest.BrandLogo, opt => opt.MapFrom(src => src.Brand.Logo))
                .ForMember(dest => dest.BrandSlogan, opt => opt.MapFrom(src => src.Brand.Slogan))
                .ForMember(dest => dest.MenuBackground, opt => opt.MapFrom(src => src.Brand.CatalogsBackground))
                .ForMember(dest => dest.Catalogs, opt => opt.Ignore());

            CreateMap<Catalog, DTOs.DigitalMenu.Catalog>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

            CreateMap<Category, DTOs.DigitalMenu.Category>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));


            CreateMap<CrCategoryReqDTO, Category>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpCategoryReqDTO, Category>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Category, CategoryDTO>();

            CreateMap<CrItemReqDTO, Item>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpItemReqDTO, Item>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Item, ItemDTO>(); 

            CreateMap<CrProductReqDTO, Product>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpProductReqDTO, Product>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Product, ProductDTO>();

            CreateMap<CrPlatformReqDTO, Platform>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpPlatformReqDTO, Platform>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Platform, PlatformDTO>();

            CreateMap<CrModifiersGroupReqDTO, ModifiersGroup>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpModifiersGroupReqDTO, ModifiersGroup>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<ModifiersGroup, ModifiersGroupDTO>();

            CreateMap<CrModifierReqDTO, Modifier>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpModifierReqDTO, Modifier>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Modifier, ModifierDTO>();

            CreateMap<SetPlatformItem, PricePerItemPerPlatform>();

            CreateMap<SetPlatformModifier, PricePerModifierPerPlatform>();

            CreateMap<CreateBrandDTO, Brand>();
            CreateMap<Brand, BrandDTO>();

            // Mapeo para InitCatalogDTO a Catalog
            CreateMap<InitCatalogDTO, Catalog>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.BranchId, act => act.MapFrom(src => src.BranchId))
                // Asegúrate de configurar todas las propiedades necesarias aquí.
                ;

            // Mapeo para InitCategoryDTO a Category
            CreateMap<InitCategoryDTO, Category>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.CatalogId, act => act.MapFrom(src => src.CatalogId))
                // Configura cualquier otra propiedad necesaria.
                ;

            // Mapeo para InitProductDTO a Product
            CreateMap<InitProductDTO, Product>()
                .ForMember(dest => dest.Alias, act => act.MapFrom(src => src.Alias))
                .ForMember(dest => dest.CategoryId, act => act.MapFrom(src => src.CategoryId))
                // Incluye cualquier otra configuración requerida para las propiedades.
                ;

            // Mapeo para InitModifiersGroupDTO a ModifiersGroup
            CreateMap<InitModifiersGroupDTO, ModifiersGroup>()
                .ForMember(dest => dest.Alias, act => act.MapFrom(src => src.Alias))
                .ForMember(dest => dest.ProductId, act => act.MapFrom(src => src.ProductId))
                // Añade aquí cualquier otra configuración de mapeo necesaria.
                ;

            // Mapeo para InitModifierDTO a Modifier
            CreateMap<InitModifierDTO, Modifier>()
                .ForMember(dest => dest.Alias, act => act.MapFrom(src => src.Alias))
                .ForMember(dest => dest.Price, act => act.MapFrom(src => src.Price))
                .ForMember(dest => dest.ModifiersGroupId, act => act.MapFrom(src => src.ModifiersGroupId))
                ;

            CreateMap<UpBrandReqDTO, Brand>()
                .ForMember(dest => dest.Logo, opt => opt.Ignore())
                .ForMember(dest => dest.CatalogsBackground, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Brand, UpBrandResDTO>();

            CreateMap<CreateBranchDTO, Branch>();
            CreateMap<Branch, BranchDTO>();

            CreateMap<Catalog, CatalogDetailsDTO>();
            CreateMap<CreateCatalogDTO, Catalog>();
            CreateMap<UpdateCatalogDTO, Catalog>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
