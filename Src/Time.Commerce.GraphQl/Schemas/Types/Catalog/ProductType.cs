using GraphQL.Types;
using Time.Commerce.Contracts.Views.Catalog;

namespace Time.Commerce.GraphQl.Schemas.Types.Catalog
{
    public class ProductType : ObjectGraphType<ProductView>
    {
        public ProductType()
        {
            Name = "Product";
            Description = "Product Type";
            Field(d => d.Id, nullable: false).Description("Product Id");
            Field(d => d.Name, nullable: true).Description("Product Name");
            Field(d => d.SeName, nullable: true).Description("Product SeName");
            Field(d => d.Price, nullable: true).Description("Product Price");
            Field(d => d.OldPrice, nullable: true).Description("Product OldPrice");
            Field(d => d.ShortDescription, nullable: true).Description("Product ShortDescription");
            //Field(d => d.Description, nullable: true).Description("Product Description");
            Field(d => d.MetaTitle, nullable: true).Description("Product MetaTitle");
            Field(d => d.MetaKeywords, nullable: true).Description("Product MetaKeywords");
            Field(d => d.MetaDescription, nullable: true).Description("Product MetaDescription");
            Field<ListGraphType<ImageType>>("images");
            Field<ListGraphType<CategoryType>>("categories");
            Field<BrandType>("brand");
        }
    }
}
