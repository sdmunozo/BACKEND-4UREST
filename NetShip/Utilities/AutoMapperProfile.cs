using AutoMapper;
using NetShip.DTOs;
using NetShip.DTOs.Auth;
using NetShip.DTOs.Branch;
using NetShip.DTOs.Brand;
using NetShip.DTOs.CatalogDTOs;
using NetShip.DTOs.Category;
using NetShip.DTOs.DigitalMenu;
using NetShip.DTOs.Item;
using NetShip.DTOs.Items;
using NetShip.DTOs.LandingUserEvent;
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

            CreateMap<Entities.Catalog, DTOs.DigitalMenu.Catalog>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

            CreateMap<Entities.Category, DTOs.DigitalMenu.Category>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            // Mapeo de la entidad a DTO
            CreateMap<LandingUserEvent, LandingUserEventDTO>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));

            // Mapeo del DTO a la entidad
            CreateMap<LandingUserEventDTO, LandingUserEvent>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignora nulos en todos los campos

            // Configuración de mapeo para EventDetails con tratamiento completo de nulos
            CreateMap<EventDetails, EventDetailsDTO>()
                .ForMember(dest => dest.PresentationViewSecondsElapsed, opt => opt.Condition(src => src.PresentationViewSecondsElapsed.HasValue))
                .ForMember(dest => dest.MenuHighlightsViewSecondsElapsed, opt => opt.Condition(src => src.MenuHighlightsViewSecondsElapsed.HasValue))
                .ForMember(dest => dest.MenuScreensViewSecondsElapsed, opt => opt.Condition(src => src.MenuScreensViewSecondsElapsed.HasValue))
                .ForMember(dest => dest.ForWhoViewSecondsElapsed, opt => opt.Condition(src => src.ForWhoViewSecondsElapsed.HasValue))
                .ForMember(dest => dest.WhyUsViewSecondsElapsed, opt => opt.Condition(src => src.WhyUsViewSecondsElapsed.HasValue))
                .ForMember(dest => dest.SuscriptionsViewSecondsElapsed, opt => opt.Condition(src => src.SuscriptionsViewSecondsElapsed.HasValue))
                .ForMember(dest => dest.TestimonialsViewSecondsElapsed, opt => opt.Condition(src => src.TestimonialsViewSecondsElapsed.HasValue))
                .ForMember(dest => dest.FaqViewSecondsElapsed, opt => opt.Condition(src => src.FaqViewSecondsElapsed.HasValue))
                .ForMember(dest => dest.TrustElementsViewSecondsElapsed, opt => opt.Condition(src => src.TrustElementsViewSecondsElapsed.HasValue))
                .ForMember(dest => dest.PlaybackTime, opt => opt.Condition(src => src.PlaybackTime.HasValue))
                .ForMember(dest => dest.Duration, opt => opt.Condition(src => src.Duration.HasValue))
                .ForMember(dest => dest.LinkDestination, opt => opt.Condition(src => src.LinkDestination != null))
                .ForMember(dest => dest.LinkLabel, opt => opt.Condition(src => src.LinkLabel != null))
                .ForMember(dest => dest.ImageId, opt => opt.Condition(src => src.ImageId != null))
                .ForMember(dest => dest.FAQId, opt => opt.Condition(src => src.FAQId != null))
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != null))
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Asegura que todos los mapeos inversos también ignoren nulos

            /*
            // Mapeo de LandingUserEvent a LandingUserEventDTO
            CreateMap<LandingUserEvent, LandingUserEventDTO>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));

            // Mapeo de EventDetails a EventDetailsDTO
            CreateMap<EventDetails, EventDetailsDTO>();

            // Asegúrate de que el mapeo inverso también esté configurado si es necesario
            CreateMap<LandingUserEventDTO, LandingUserEvent>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));

            CreateMap<EventDetailsDTO, EventDetails>();*/

            CreateMap<CrCategoryReqDTO, Entities.Category>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpCategoryReqDTO, Entities.Category>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Entities.Category, CategoryDTO>();

            CreateMap<CrItemReqDTO, Entities.Item>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpItemReqDTO, Entities.Item>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Entities.Item, ItemDTO>(); 

            CreateMap<CrProductReqDTO, Entities.Product>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpProductReqDTO, Entities.Product>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Entities.Product, ProductDTO>();

            CreateMap<CrPlatformReqDTO, Platform>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpPlatformReqDTO, Platform>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Platform, PlatformDTO>();

            CreateMap<CrModifiersGroupReqDTO, Entities.ModifiersGroup>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpModifiersGroupReqDTO, Entities.ModifiersGroup>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Entities.ModifiersGroup, ModifiersGroupDTO>();

            CreateMap<CrModifierReqDTO, Entities.Modifier>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<UpModifierReqDTO, Entities.Modifier>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());

            CreateMap<Entities.Modifier, ModifierDTO>();

            CreateMap<SetPlatformItem, PricePerItemPerPlatform>();

            CreateMap<SetPlatformModifier, PricePerModifierPerPlatform>();

            CreateMap<CreateBrandDTO, Brand>();
            CreateMap<Brand, BrandDTO>();

            // Mapeo para InitCatalogDTO a Catalog
            CreateMap<InitCatalogDTO, Entities.Catalog>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.BranchId, act => act.MapFrom(src => src.BranchId))
                ;

            // Mapeo para InitCategoryDTO a Category
            CreateMap<InitCategoryDTO, Entities.Category>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.CatalogId, act => act.MapFrom(src => src.CatalogId))
                ;

            // Mapeo para InitProductDTO a Product
            CreateMap<InitProductDTO, Entities.Product>()
                .ForMember(dest => dest.Alias, act => act.MapFrom(src => src.Alias))
                .ForMember(dest => dest.CategoryId, act => act.MapFrom(src => src.CategoryId))
                ;

            // Mapeo para InitModifiersGroupDTO a ModifiersGroup
            CreateMap<InitModifiersGroupDTO, Entities.ModifiersGroup>()
                .ForMember(dest => dest.Alias, act => act.MapFrom(src => src.Alias))
                .ForMember(dest => dest.ProductId, act => act.MapFrom(src => src.ProductId))
                ;

            // Mapeo para InitModifierDTO a Modifier
            CreateMap<InitModifierDTO, Entities.Modifier>()
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

            CreateMap<Entities.Catalog, CatalogDetailsDTO>();
            CreateMap<CreateCatalogDTO, Entities.Catalog>();
            CreateMap<UpdateCatalogDTO, Entities.Catalog>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<FeedbackDTO, Feedback>();
            CreateMap<DeviceTrackingDTO, DeviceTracking>();

        }
    }
}
