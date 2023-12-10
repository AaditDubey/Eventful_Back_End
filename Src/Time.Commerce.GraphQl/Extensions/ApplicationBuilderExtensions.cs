using GraphQL.Types;
using Microsoft.AspNetCore.Builder;

namespace Time.Commerce.GraphQl.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureGraphQl(this IApplicationBuilder app)
        {
            app.UseGraphQLAltair("/ui/graphql");
            app.UseGraphQL<ISchema>();
        }
    }
}
