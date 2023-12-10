using AutoMapper;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Sales;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Infras.Services.Catalog;

public class PricingService : IPricingService
{
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    private readonly IDiscountCouponService _discountCouponService;
    public PricingService(IMapper mapper, IProductService productService, IDiscountCouponService discountCouponService)
    {
        _mapper = mapper;
        _productService = productService;
        _discountCouponService = discountCouponService;
    }

    public ShoppingCartView CalculateCartTotalAsync(ShoppingCartView cart, CancellationToken cancellationToken = default)
    {
        double subTotal = 0;
        double total = 0;
        double discountAmount = 0;
        foreach(var item in cart.Items) {
            var amount = (double)item.Price * item.Quantity;
            subTotal += amount;
            item.Amount = amount;
        }
        total = subTotal + (double)discountAmount;
        cart.SubTotal = subTotal;
        cart.DiscountAmount = discountAmount;
        cart.Total = total;
        return cart;
    }

    public async Task<ShoppingCartView> CalculateTotalAsync(UpdateShoppingCartModel model, CancellationToken cancellationToken = default)
    {
        var cartItems = model.Items;
        double subTotal = 0;
        double total = 0;
        double discountAmount = 0;

        if (!cartItems.Any()) return new ShoppingCartView
        {
            Id = model.Id,
            SubTotal = subTotal,
            DiscountAmount = discountAmount,
            Total = total,
        };

        var productIds = cartItems.Select(x => x.ProductId).ToList();
        var products = await _productService.FindProductsAsync(new ProductQueryModel
        {
            Ids = productIds,
            PageSize = productIds.Count
        });

        foreach (var product in model.Items)
        {

        }

        var coupons = await _discountCouponService.FindAsync(new DiscountCouponQueryModel
        {
            CouponCodes = model.CouponCodes,
            PageSize = model.CouponCodes.Count,
        });
        throw new NotImplementedException();
    }

    public ShoppingCartItemView GetProductWithUnitPrice(Product product, IList<CustomAttribute> attributesOptions)
    {
        var item = new ShoppingCartItemView
        {
            Product = _mapper.Map<ProductSimpleView>(product),
            Price = product.Price
        };
        if (attributesOptions is not null && attributesOptions.Any() && product.Variants is not null && product.Variants.Any())
        {
            foreach (var variant in product.Variants)
            {
                var attributes = variant.Attributes;
                if (attributes is not null && attributes.Any())
                {
                    if (attributesOptions.All(i => attributes.Any(a => a.Key == i.Key && a.Value == i.Value)))
                    {
                        if (variant.Price > 0)
                        {
                            item.Price = variant.Price;
                            item.Product.Price = variant.Price;
                        }
                        if (!string.IsNullOrEmpty(variant.ImageId))
                        {
                            var image = product.Images.FirstOrDefault(i => i.Id == variant.ImageId);
                            item.Product.Image = _mapper.Map<ImageView>(image);
                        }

                        break;
                    }
                }
            }
        }
        return item;
    }
}
