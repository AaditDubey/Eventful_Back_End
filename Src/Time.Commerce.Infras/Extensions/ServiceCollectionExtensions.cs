using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using Time.Commerce.Application.Extensions;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Options;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Application.Services.Common;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Application.Services.Reports;
using Time.Commerce.Application.Services.Sales;
using Time.Commerce.Core.Constants;
using Time.Commerce.Core.Extensions;
using Time.Commerce.Core.Options;
using Time.Commerce.Domains.Repositories.Catalog;
using Time.Commerce.Domains.Repositories.Cms;
using Time.Commerce.Domains.Repositories.Identity;
using Time.Commerce.Domains.Repositories.Sales;
using Time.Commerce.Infras.Repositories.Catalog;
using Time.Commerce.Infras.Repositories.Cms;
using Time.Commerce.Infras.Repositories.Identity;
using Time.Commerce.Infras.Repositories.Sales;
using Time.Commerce.Infras.Services.Catalog;
using Time.Commerce.Infras.Services.Cms;
using Time.Commerce.Infras.Services.Common;
using Time.Commerce.Infras.Services.Identity;
using Time.Commerce.Infras.Services.Reports;
using Time.Commerce.Infras.Services.Sales;
using Time.Commerce.Proxy.Cms;
using TimeCommerce.Core.MongoDB.Context;
using TimeCommerce.Core.MongoDB.Settings;
using AspNetCoreExtensions.Utils.JsonConverters;

namespace Time.Commerce.Infras.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMultiLanguages();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            //add dependency injection
            services.AddDependencyInjection(configuration);

            //add 3rd-party from application layer
            services.AddApplicationLayer();

            var distributedCacheOptions = configuration.GetSection("DistributedCacheOptions").Get<DistributedCacheOptions>();
            services.AddRedisCache(distributedCacheOptions.ConnectionString);

            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opts.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
                    opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    opts.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            var audience = configuration.GetSection("AuthOptions").Get<AuthOptions>();
            var tokenValidationParameters = JwtExtensions.GenTokenValidationParameters(audience);
            services.AddAuthentication(o =>
            {
                //o.DefaultAuthenticateScheme = CoreSystemConst.JWT_BEARER_SCHEMA;
                //o.DefaultChallengeScheme = CoreSystemConst.JWT_BEARER_SCHEMA;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(config =>
            {
                config.Cookie.Name = "Cookie.Basics";
                config.LoginPath = "/Account/Login";
                config.SlidingExpiration = true;
                config.AccessDeniedPath = "/Forbidden/";
            })
            .AddJwtBearer
            (
                //settings.Audience.AuthenticateScheme, //=> For own schema
                x =>
                {
                    //get token from cookies. (For secure spa)
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.ContainsKey(CoreSystemConst.ACCESS_TOKEN_COOKIE_KEY))
                            {
                                context.Token = context.Request.Cookies[CoreSystemConst.ACCESS_TOKEN_COOKIE_KEY];
                            }
                            return Task.CompletedTask;
                        },
                    };

                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = tokenValidationParameters;
                }
            );


            services.AddAuthorization(options =>
            {
                options.AddPolicy(CoreSystemConst.SYSTEM_ADMIN_POLICY, policy =>
                {
                    policy.RequireRole(CoreSystemConst.SYSTEM_ADMIN_ROLE, CoreSystemConst.ADMIN_ROLE);
                });

                options.AddPolicy(CoreSystemConst.ADMIN_POLICY, policy =>
                {
                    policy.RequireRole(CoreSystemConst.SYSTEM_ADMIN_ROLE, CoreSystemConst.ADMIN_ROLE, CoreSystemConst.EVENT_HOST_ROLE);
                });
            });
        }

        private static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDataSettings = configuration.GetSection("MongoSettings").Get<MongoDataSettings>();
            services.AddSingleton<MongoDataSettings>(mongoDataSettings);

            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IWorkContext, WorkContext>();
            services.AddScoped<IDistributedCacheService, DistributedCacheService>();

            services.AddIdentityDI();
            services.AddCatalogDI();
            services.AddSalesDI();
            services.AddCmsDI();
            services.AddReportDI();

            //This methods to fake proxy.
            services.AddProxy();
        }

        private static void AddIdentityDI(this IServiceCollection services)
        {
            //Di repository
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
            services.AddScoped<ISpeakerRepository, SpeakerRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();

            //Di services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IContactMessageService, ContactMessageService>();
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IStoreService, StoreService>();
        }

        private static void AddCatalogDI(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
            services.AddScoped<IDiscountCouponRepository, DiscountCouponRepository>();


            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductAttributeService, ProductAttributeService>();
            services.AddScoped<IDiscountCouponService, DiscountCouponService>();
            services.AddScoped<IPricingService, PricingService>();
        }

        private static void AddSalesDI(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
        }

        private static void AddCmsDI(this IServiceCollection services)
        {
            services.AddScoped<IContentRepository, ContentRepository>();
            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IWidgetRepository, WidgetRepository>();


            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IWidgetService, WidgetService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ILocaleService, LocaleService>();
            services.AddScoped<IChatGptService, ChatGptService>();
        }

        private static void AddReportDI(this IServiceCollection services)
        {
            services.AddScoped<IReportService, ReportService>();
        }

        private static void AddProxy(this IServiceCollection services)
        {
            services.AddScoped<IStorageProxy, StorageProxy>();
        }

        private static void AddMultiLanguages(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {
                    new CultureInfo("en-US"),
                    new CultureInfo("vi")
                };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
        }
    }
}
