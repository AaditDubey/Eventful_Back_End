using GraphQL.Instrumentation;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Time.Commerce.GraphQl.Middleware;
using Time.Commerce.GraphQl.Schemas.Mutation;
using Time.Commerce.GraphQl.Schemas.Query;

namespace Time.Commerce.GraphQl.Schemas
{
    public class TimeGraphQlSchema : Schema
    {
        public TimeGraphQlSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<TimeGraphQlQuery>();
            FieldMiddleware.Use(new FieldsValidateMiddleware(serviceProvider.GetRequiredService<IHttpContextAccessor>()));
            Mutation = serviceProvider.GetRequiredService<TimeGraphQlMutation>();
        }
    }
}
