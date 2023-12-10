using GraphQL.Types;
using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Sales;
using Time.Commerce.GraphQl.Schemas.Types.Catalog;

namespace Time.Commerce.GraphQl.Schemas.Types.Sale
{
    public class ShoppingCartType : ObjectGraphType<ShoppingCartView>
    {
        public ShoppingCartType()
        {
            Name = "ShoppingCartType";
            Description = "ShoppingCartType";
            Field(d => d.Id, nullable: true).Description("Id");
            Field(d => d.CustomerId, nullable: true).Description("CustomerId");
            Field(d => d.StoreId, nullable: true).Description("StoreId");
            Field(d => d.CreatedOn, nullable: true).Description("CreatedOn");
            Field(d => d.UpdatedOn, nullable: true).Description("UpdatedOn");
            Field<ListGraphType<ShoppingCartItemType>>("Items");
        }
    }

    public class ShoppingCartItemType : ObjectGraphType<ShoppingCartItemView>
    {
        public ShoppingCartItemType()
        {
            Name = "ShoppingCartItemType";
            Description = "ShoppingCartItemType";
            Field(d => d.Id, nullable: false).Description("Id");
            Field(d => d.ProductId, nullable: false).Description("ProductId");
            Field(d => d.Quantity, nullable: false).Description("Quantity");
            Field<ListGraphType<ProductAttributesViewType>>("Attributes");
            Field<ProductType>("Product");
        }
    }
   

    public class AddProductToCartInputType : InputObjectGraphType<ShoppingCartItemModel>
    {
        public AddProductToCartInputType()
        {
            Name = "AddProductToCartInputType";
            Field(d => d.ProductId, nullable: true).Description("CustomerId");
            Field(d => d.Quantity, nullable: true).Description("StoreId");
            Field<ListGraphType<ProductAttributesType>>("Attributes");
        }
    }

    public class UpdateCartItemInputType : InputObjectGraphType<ShoppingCartItemModel>
    {
        public UpdateCartItemInputType()
        {
            Name = "UpdateCartItemInputType";
            Field(d => d.Id, nullable: true).Description("CustomerId");
            Field(d => d.Quantity, nullable: true).Description("StoreId");
        }
    }

    public class ProductAttributesType : InputObjectGraphType<CustomAttributeModel>
    {
        public ProductAttributesType()
        {
            Name = "ProductAttributesType";
            Field(d => d.Key, nullable: true).Description("Key");
            Field(d => d.Value, nullable: true).Description("Value");
        }
    }

    public class ProductAttributesViewType : ObjectGraphType<CustomAttributeView>
    {
        public ProductAttributesViewType()
        {
            Name = "ProductAttributesViewType";
            Field(d => d.Key, nullable: true).Description("Key");
            Field(d => d.Value, nullable: true).Description("Value");
        }
    }
}
