using GraphQL.Types;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.GraphQl.Schemas.Types.Catalog
{
    public class BrandListType : ObjectGraphType<PageableView<BrandView>>
    {
        public BrandListType()
        {
            Name = "BrandListType";
            Description = "Brand list type";
            Field(d => d.TotalItems, nullable: false).Description("Total items");
            Field(d => d.PageIndex, nullable: false).Description("Page index");
            Field(d => d.PageSize, nullable: false).Description("Page size");
            Field<ListGraphType<BrandType>>("Items");
        }
    }
}
