using AutoMapper;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Sales;
using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Entities.Sales;

namespace Time.Commerce.Application.Mappers.Sales
{
    public class SalesMapper : Profile
    {
        public SalesMapper()
        {
            CreateMap<Order, OrderView>();
            CreateMap<OrderItem, OrderItemView>();
            CreateMap<Address, AddressView>();
            CreateMap<OrderTax, OrderTaxView>();
            CreateMap<CustomAttribute, CustomAttributeView>();
            CreateMap<GenericAttribute, CustomAttributeView>();
            CreateMap<CustomAttributeView, CustomAttribute>();

            CreateMap<CreateOrderModel, Order>();
            CreateMap<OrderItemModel, OrderItem>();
            CreateMap<CustomAttributeModel, CustomAttribute>();
            CreateMap<AddressModel, Address>();

            CreateMap<ShoppingCart, ShoppingCartView>();
            CreateMap<ShoppingCartItem, ShoppingCartItemView>();
            CreateMap<CreateShoppingCartModel, ShoppingCart>();
            CreateMap<ShoppingCartItemModel, ShoppingCartItem>();
        }
    }
}
