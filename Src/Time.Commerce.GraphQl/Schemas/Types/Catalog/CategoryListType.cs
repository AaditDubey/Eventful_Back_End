using GraphQL.Types;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.GraphQl.Schemas.Types.Catalog
{
    public class CategoryListType : ObjectGraphType<PageableView<CategoryView>>
    {
        public CategoryListType()
        {
            Name = "CategoriesListType";
            Description = "Categories list type";
            Field(d => d.TotalItems, nullable: false).Description("Total items");
            Field(d => d.PageIndex, nullable: false).Description("Page index");
            Field(d => d.PageSize, nullable: false).Description("Page size");
            Field<ListGraphType<CategoryType>>("Items");
        }
    }
}
