using GraphQL.Types;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.GraphQl.Schemas.Types.Catalog
{
    public class ImageType : ObjectGraphType<ImageView>
    {
        public ImageType()
        {
            Name = "Image";
            Description = "Image Type";
            Field(d => d.Id, nullable: false).Description("Id");
            Field(d => d.Path, nullable: false).Description("Url");
            Field(d => d.AlternateText, nullable: false).Description("AlternateText");
            Field(d => d.Title, nullable: false).Description("Title");
            Field(d => d.MimeType, nullable: false).Description("MimeType");
        }
    }
}
