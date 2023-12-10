using GraphQL.Types;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.GraphQl.Schemas.Types.Cms
{
    public class ContentListType : ObjectGraphType<PageableView<ContentView>>
    {
        public ContentListType()
        {
            Name = "ContentListType";
            Description = "Content list type";
            Field(d => d.TotalItems, nullable: false).Description("Total items");
            Field(d => d.PageIndex, nullable: false).Description("Page index");
            Field(d => d.PageSize, nullable: false).Description("Page size");
            Field<ListGraphType<ContentType>>("Items");
        }
    }
}
