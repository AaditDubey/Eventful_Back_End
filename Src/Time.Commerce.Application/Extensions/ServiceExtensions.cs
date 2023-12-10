using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Time.Commerce.Application.Extensions
{
    public static class ServiceExtensions
    {
        #region Methods
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            //Add AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Add FluentValidation
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Time.Commerce.Contracts.Models.Identity.LoginModel>());
        }

        public static void AddRedisCache(this IServiceCollection services, string connectionString)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
            });
        }
        #endregion
    }
}
