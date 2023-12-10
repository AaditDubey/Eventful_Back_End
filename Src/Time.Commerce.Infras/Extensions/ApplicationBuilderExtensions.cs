using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Time.Commerce.Core.Constants;
using Time.Commerce.Core.Extensions;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Cms;
using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Entities.Sales;
using Time.Commerce.Domains.Repositories.Catalog;
using Time.Commerce.Domains.Repositories.Cms;
using Time.Commerce.Domains.Repositories.Identity;
using Time.Commerce.Domains.Repositories.Sales;

namespace Time.Commerce.Infras.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task ConfigureRequestPipelineAsync(this WebApplication app, IConfiguration configuration)
        {
            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.AddMulilangguages();

            app.MapFallback(context => {
                context.Response.Redirect("/notfound");
                return Task.CompletedTask;
            });

            await app.AppInstall();
            await app.ConfigureBaseRequestPipeline(configuration);
        }

        #region Private Methods
        private static async Task AppInstall(this IApplicationBuilder application)
        {
            await application.IdentityMigrationAsync();
            await application.CatalogMigrationAsync();
            await application.SalesMigrationAsync();
            await application.CmsMigrationAsync();
        }

        private static async Task IdentityMigrationAsync(this IApplicationBuilder application)
        {
            using var serviceScope = application.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var userRepository = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();
            var roleRepository = serviceScope.ServiceProvider.GetRequiredService<IRoleRepository>();
            var storeRepository = serviceScope.ServiceProvider.GetRequiredService<IStoreRepository>();

            await roleRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Role>((Builders<Role>.IndexKeys.Ascending(x => x.Name)), new CreateIndexOptions() { Name = "role_name_idx", Unique = true }));
            await userRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<User>((Builders<User>.IndexKeys.Ascending(x => x.UserName)), new CreateIndexOptions() { Name = "user_username_idx", Unique = true }));
            await userRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<User>((Builders<User>.IndexKeys.Ascending(x => x.Email)), new CreateIndexOptions() { Name = "user_email_idx", Unique = true }));
            await storeRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Store>((Builders<Store>.IndexKeys.Ascending(x => x.StoreId)), new CreateIndexOptions() { Name = "store_store_id_idx", Unique = true }));

            string[] roles = new string[] {
                CoreSystemConst.SYSTEM_ADMIN_ROLE,
                CoreSystemConst.ADMIN_ROLE,
                CoreSystemConst.EVENT_HOST_ROLE
            };
            var adminRole = new Role();
            foreach (var role in roles)
            {
                if (await roleRepository.FindOneAsync(r => r.Name == role) == null)
                {
                    var r = await roleRepository.InsertAsync(new Role { Name = role });
                    if (r.Name == CoreSystemConst.SYSTEM_ADMIN_ROLE)
                        adminRole = r;
                }
            }

            var adminUser = userRepository.Table.FirstOrDefault(X => X.UserName == "admin@admin.com");
            if (adminUser == null)
            {
                SecurityHelpers.CreatePasswordHash("123456", out byte[] passwordHash, out byte[] passwordSalt);
                adminUser = new User
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    FullName = "Tai Nguyen",
                    IsSystemAccount = true,
                    Active = true,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                adminUser.Roles.Add(adminRole);
                await userRepository.InsertAsync(adminUser);
            }
        }

        private static async Task CatalogMigrationAsync(this IApplicationBuilder application)
        {
            using var serviceScope = application.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var categoryRepository = serviceScope.ServiceProvider.GetRequiredService<ICategoryRepository>();
            await categoryRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Category>((Builders<Category>.IndexKeys.Ascending(x => x.Name)), new CreateIndexOptions() { Name = "category_name_idx" }));
            await categoryRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Category>((Builders<Category>.IndexKeys.Ascending(x => x.SeName)), new CreateIndexOptions() { Name = "category_SeName_idx" }));
            await categoryRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Category>((Builders<Category>.IndexKeys.Ascending(x => x.Stores)), new CreateIndexOptions() { Name = "category_stores_idx" }));
            await categoryRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Category>((Builders<Category>.IndexKeys.Ascending(x => x.ParentId)), new CreateIndexOptions() { Name = "category_ParentId_idx" }));

            var brandRepository = serviceScope.ServiceProvider.GetRequiredService<IBrandRepository>();
            await brandRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Brand>((Builders<Brand>.IndexKeys.Ascending(x => x.Name)), new CreateIndexOptions() { Name = "brand_name_idx" }));
            await brandRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Brand>((Builders<Brand>.IndexKeys.Ascending(x => x.SeName)), new CreateIndexOptions() { Name = "brand_SeName_idx" }));
            await brandRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Brand>((Builders<Brand>.IndexKeys.Ascending(x => x.Stores)), new CreateIndexOptions() { Name = "brand_stores_idx" }));

            var productRepository = serviceScope.ServiceProvider.GetRequiredService<IProductRepository>();
            await productRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Product>((Builders<Product>.IndexKeys.Ascending(x => x.Name)), new CreateIndexOptions() { Name = "product_name_idx" }));
            await productRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Product>((Builders<Product>.IndexKeys.Ascending(x => x.SeName)), new CreateIndexOptions() { Name = "product_SeName_idx" }));
            await productRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Product>((Builders<Product>.IndexKeys.Ascending(x => x.Stores)), new CreateIndexOptions() { Name = "product_stores_idx" }));
        }

        private static async Task SalesMigrationAsync(this IApplicationBuilder application)
        {
            using var serviceScope = application.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var orderRepository = serviceScope.ServiceProvider.GetRequiredService<IOrderRepository>();

            await orderRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Order>((Builders<Order>.IndexKeys.Ascending(x => x.OrderNumber)), new CreateIndexOptions() { Name = "order_number_idx" }));
            await orderRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Order>((Builders<Order>.IndexKeys.Ascending(x => x.Note)), new CreateIndexOptions() { Name = "order_Note_idx" }));
            await orderRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Order>((Builders<Order>.IndexKeys.Ascending(x => x.CustomerEmail)), new CreateIndexOptions() { Name = "order_CustomerEmail_idx" }));
            await orderRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Order>((Builders<Order>.IndexKeys.Ascending(x => x.FirstName)), new CreateIndexOptions() { Name = "order_FirstName_idx" }));
            await orderRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Order>((Builders<Order>.IndexKeys.Ascending(x => x.LastName)), new CreateIndexOptions() { Name = "order_LastName_idx" }));
        }

        private static async Task CmsMigrationAsync(this IApplicationBuilder application)
        {
            using var serviceScope = application.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var contentRepository = serviceScope.ServiceProvider.GetRequiredService<IContentRepository>();

            await contentRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Content>((Builders<Content>.IndexKeys.Ascending(x => x.SeName)), new CreateIndexOptions() { Name = "cms_sename_idx" }));
            await contentRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Content>((Builders<Content>.IndexKeys.Ascending(x => x.Stores)), new CreateIndexOptions() { Name = "cms_stores_idx" }));
            await contentRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Content>((Builders<Content>.IndexKeys.Ascending(x => x.Type)), new CreateIndexOptions() { Name = "cms_type_idx" }));
            await contentRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Content>((Builders<Content>.IndexKeys.Ascending(x => x.ShortContent)), new CreateIndexOptions() { Name = "cms_ShortContent_idx" }));
        }

        private static void AddMulilangguages(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        }
        #endregion
    }
}
