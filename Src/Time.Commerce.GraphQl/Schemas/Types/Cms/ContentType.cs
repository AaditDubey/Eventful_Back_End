using GraphQL.Types;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.GraphQl.Schemas.Types.Catalog;

namespace Time.Commerce.GraphQl.Schemas.Types.Cms
{
    public class ContentType : ObjectGraphType<ContentView>
    {
        public ContentType()
        {
            Name = "ContentType";
            Description = "Content Type";
            Field(d => d.Id, nullable: false).Description("Id");
            Field(d => d.Title, nullable: true).Description("Name");
            Field(d => d.ShortContent, nullable: true).Description("ShortContent");
            Field(d => d.FullContent, nullable: true).Description("FullContent");
            Field(d => d.Type, nullable: true).Description("Type");
            Field(d => d.SeName, nullable: true).Description("SeName");
            Field(d => d.Publisher, nullable: true).Description("Publisher");
            Field(d => d.MetaKeywords, nullable: true).Description("MetaKeywords");
            Field(d => d.MetaTitle, nullable: true).Description("MetaTitle");
            Field(d => d.MetaDescription, nullable: true).Description("MetaDescription");
            Field(d => d.CreatedOn, nullable: true).Description("CreatedOn");
            Field(d => d.CreatedBy, nullable: true).Description("CreatedBy");
            Field(d => d.UpdatedOn, nullable: true).Description("UpdatedOn");
            Field(d => d.UpdatedBy, nullable: true).Description("UpdatedBy");
            Field<ImageType>("image");
        }
    }
}
