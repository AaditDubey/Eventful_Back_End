using GraphQL.Types;
using Time.Commerce.Contracts.Views.Catalog;

namespace Time.Commerce.GraphQl.Schemas.Types.Catalog
{
    public class BrandType : ObjectGraphType<BrandView>
    {
        public BrandType()
        {
            Name = "BrandType";
            Description = "Brand Type";
            Field(d => d.Id, nullable: false).Description("Id");
            Field(d => d.Name, nullable: true).Description("Name");
            Field(d => d.SeName, nullable: true).Description("SeName");
            Field(d => d.MetaTitle, nullable: true).Description("MetaTitle");
            Field(d => d.MetaKeywords, nullable: true).Description("MetaKeywords");
            Field(d => d.MetaDescription, nullable: true).Description("MetaDescription");
            Field<ImageType>("image");
        }
    }
}
