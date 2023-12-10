using AspNetCoreExtensions.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace Time.Commerce.Core.Extensions
{
    public static class BaseApplicationBuilderExtensions
    {
        public static async Task ConfigureBaseRequestPipeline(this WebApplication app, IConfiguration configuration)
        {
            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseRouting();

            //// global cors policy FOR COOKIES HTTP ONLY
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            //// global cors policy
            //app.UseCors(x => x
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .SetIsOriginAllowed(origin => true)); // allow any origin;

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();

            app.UseGlobalExceptionHandler(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            }, enableDiagnostics: true);

            await app.RunAsync();
        }
    }
}
