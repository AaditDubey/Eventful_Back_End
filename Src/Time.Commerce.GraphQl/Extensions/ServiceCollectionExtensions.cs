using GraphQL.Instrumentation;
using GraphQL.MicrosoftDI;
using GraphQL.Reflection;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Time.Commerce.GraphQl.Schemas;

namespace Time.Commerce.GraphQl.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddSingleton<ISchema, TimeGraphQlSchema>(services => new TimeGraphQlSchema(new SelfActivatingServiceProvider(services)));
            services.AddSingleton<IFieldMiddleware, InstrumentFieldsMiddleware>();
            //services.AddGraphQL(options =>
            //{
            //    options.EnableMetrics = true;
            //}).AddSystemTextJson();
            services
                .AddGraphQL(
                    (options, provider) =>
                    {
                        // Load GraphQL Server configurations
                        //var graphQLOptions = Configuration
                        //    .GetSection("GraphQL")
                        //    .Get<GraphQLOptions>();
                        //options.ComplexityConfiguration = graphQLOptions.ComplexityConfiguration;
                        //options.EnableMetrics = graphQLOptions.EnableMetrics;
                        options.EnableMetrics = true;
                        // Log errors
                        var logger = provider.GetRequiredService<ILogger<Time.Commerce.GraphQl.Middleware.FieldsValidateMiddleware>>();
                        options.UnhandledExceptionDelegate = ctx =>
                            logger.LogError("{Error} occurred", ctx.OriginalException.Message);
                    })
                // Adds all graph types in the current assembly with a singleton lifetime.
                .AddGraphTypes()
                // Add GraphQL data loader to reduce the number of calls to our repository. https://graphql-dotnet.github.io/docs/guides/dataloader/
                .AddDataLoader()
                .AddSystemTextJson();
        }
    }
}
