using AutoMapper;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Application.Mappers.Catalog
{
    public class CatalogMapper : Profile
    {
        public CatalogMapper()
        {
            CreateMap<ImageModel, Image>();
            CreateMap<Image, ImageModel>();
            CreateMap<Image, ImageView>();

            CreateMap<Category, CategoryView>();
            CreateMap<CreateCategoryModel, Category>();

            CreateMap<Brand, BrandView>();
            CreateMap<CreateBrandModel, Brand>();

            CreateMap<CreateProductModel, Product>();
            CreateMap<Product, ProductView>();
            CreateMap<Product, ProductDetailsView>().ForMember(dest => dest.Attributes,
            opt =>
                opt.MapFrom(src =>
                    src.ProductAttributeMapping));
            CreateMap<Product, ProductSimpleView>().ForMember(dest => dest.Image,
            opt =>
                opt.MapFrom(src =>
                    src.Images != null && src.Images.Any() ? src.Images.ToList()[0] : null));

            CreateMap<ProductOverView, ProductOverViewView>();
            CreateMap<ProductCategoryMapping, ProductCategoryMappingView>();
            CreateMap<ProductAddAttributeModel, ProductAttributeMapping>();
            CreateMap<ProductAttributeMapping, ProductAttributeMappingView>();
            
            CreateMap<CreateProductAttributeModel, ProductAttribute>();
            CreateMap<ProductAttributeOptionValueModel, ProductAttributeOptionValue>();
            CreateMap<ProductAttribute, ProductAttributeView>();
            CreateMap<ProductAttributeOptionValue, ProductAttributeOptionValueView>();

            CreateMap<ProductVariant, ProductVariantView>();
            CreateMap<ProductAddVariantModel, ProductVariant>();

            CreateMap<GenericAttributeModel, GenericAttribute>();
            CreateMap<GenericAttribute, GenericAttributeModel>();

            CreateMap<CreateDiscountCouponModel, DiscountCoupon>();
            CreateMap<DiscountCoupon, DiscountCouponView>();
        }
    }
}
