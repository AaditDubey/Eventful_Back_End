using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Application.Services.Sales;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.GraphQl.Constants;
using Time.Commerce.GraphQl.Schemas.Types.Catalog;
using Time.Commerce.GraphQl.Schemas.Types.Cms;
using Time.Commerce.GraphQl.Schemas.Types.Sale;

namespace Time.Commerce.GraphQl.Schemas.Query
{
    public class TimeGraphQlQuery : ObjectGraphType
    {
        public TimeGraphQlQuery()
        {
            Name = "Queries";

            #region Catalog
            FieldAsync<ProductListType>("products",
                arguments: new QueryArguments(
                    new List<QueryArgument>
                    {
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.SKIP
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.TAKE
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.STORE
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.SEARCH_TEXT
                        },
                    }
                ),
                resolve: async context =>
                {
                    var service = context.RequestServices.GetRequiredService<IProductService>();
                    var skip = context.GetArgument<int>(TimeGraphQlConstants.SKIP);
                    var take = context.GetArgument<int>(TimeGraphQlConstants.TAKE);
                    var searchText = context.GetArgument<string>(TimeGraphQlConstants.SEARCH_TEXT);
                    var res = await service.FindAsync(new ProductQueryModel
                    {
                        PageIndex = skip,
                        PageSize = take,
                        StoreId = context.GetExtension($"{context.Path}.{TimeGraphQlConstants.STORE}").ToString(),
                        SearchText = searchText
                    });
                    return res;
                }
            );

            FieldAsync<ProductOverViewType>("product",
                arguments: new QueryArguments(
                    new List<QueryArgument>
                    {
                        new QueryArgument<StringGraphType>
                        {
                            Name = "slug"
                        },
                        new QueryArgument<StringGraphType>
                        {
                            Name = "id"
                        }
                    }
                ),
                resolve: async context =>
                {
                    var service = context.RequestServices.GetRequiredService<IProductService>();
                    var seName = context.GetArgument<string>("slug");

                    if (!string.IsNullOrEmpty(seName))
                        return await service.GetProductOverViewBySlugAsync(seName);

                    var id = context.GetArgument<string>("id");
                    return await service.GetProductOverViewByIdAsync(id);
                });

            FieldAsync<CategoryType>("category"
             , arguments: new QueryArguments(
                new List<QueryArgument>
                    {
                        new QueryArgument<StringGraphType>
                        {
                            Name = "slug"
                        },
                        new QueryArgument<StringGraphType>
                        {
                            Name = "id"
                        }
                    }
                 ),
             resolve: async context =>
             {
                 var service = context.RequestServices.GetRequiredService<ICategoryService>();
                 var seName = context.GetArgument<string>("slug");

                 if (!string.IsNullOrEmpty(seName))
                     return await service.GetBySlugAsync(seName);

                 var id = context.GetArgument<string>("id");
                 return await service.GetByIdAsync(id);
             }
            );

            FieldAsync<CategoryListType>("categories",
               arguments: new QueryArguments(
                  new List<QueryArgument>
                  {
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.SKIP
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.TAKE
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.STORE
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.SEARCH_TEXT
                        },
                  }
              ),
              resolve: async context =>
              {
                  var service = context.RequestServices.GetRequiredService<ICategoryService>();
                  var skip = context.GetArgument<int>(TimeGraphQlConstants.SKIP);
                  var take = context.GetArgument<int>(TimeGraphQlConstants.TAKE);
                  var searchText = context.GetArgument<string>(TimeGraphQlConstants.SEARCH_TEXT);
                  var res = await service.FindAsync(new CategoryQueryModel
                  {
                      PageIndex = skip,
                      PageSize = take,
                      StoreId = context.GetExtension($"{context.Path}.{TimeGraphQlConstants.STORE}").ToString(),
                      SearchText = searchText
                  });
                  return res;
              }
            );

            FieldAsync<ListGraphType<CategoryRecursiveType>>("categoriesRecursive",
                arguments: new QueryArguments(
                      new List<QueryArgument>
                      {
                                new QueryArgument<IdGraphType>
                                {
                                    Name = TimeGraphQlConstants.SKIP
                                },
                                new QueryArgument<IdGraphType>
                                {
                                    Name = TimeGraphQlConstants.TAKE
                                },
                                new QueryArgument<IdGraphType>
                                {
                                    Name = TimeGraphQlConstants.STORE
                                },
                                new QueryArgument<IdGraphType>
                                {
                                    Name = TimeGraphQlConstants.SEARCH_TEXT
                                },
                      }
                    ),
                resolve: async context =>
                {
                    var service = context.RequestServices.GetRequiredService<ICategoryService>();
                    var skip = context.GetArgument<int>(TimeGraphQlConstants.SKIP);
                    var take = context.GetArgument<int>(TimeGraphQlConstants.TAKE);
                    var searchText = context.GetArgument<string>(TimeGraphQlConstants.SEARCH_TEXT);
                    var res = await service.GetAllWithRecursive(new CategoryQueryModel
                    {
                        PageIndex = skip,
                        PageSize = take,
                        StoreId = context.GetExtension($"{context.Path}.{TimeGraphQlConstants.STORE}").ToString(),
                        SearchText = searchText
                    });
                    return res;
                }
            );

            FieldAsync<BrandType>("brand"
            , arguments: new QueryArguments(
               new List<QueryArgument>
                   {
                        new QueryArgument<StringGraphType>
                        {
                            Name = "slug"
                        },
                        new QueryArgument<StringGraphType>
                        {
                            Name = "id"
                        }
                   }
                ),
            resolve: async context =>
            {
                var service = context.RequestServices.GetRequiredService<IBrandService>();
                var seName = context.GetArgument<string>("slug");

                if (!string.IsNullOrEmpty(seName))
                    return await service.GetBySlugAsync(seName);

                var id = context.GetArgument<string>("id");
                return await service.GetByIdAsync(id);
            }
            );

            FieldAsync<BrandListType>("brands",
               arguments: new QueryArguments(
                  new List<QueryArgument>
                  {
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.SKIP
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.TAKE
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.STORE
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.SEARCH_TEXT
                        },
                  }
              ),
              resolve: async context =>
              {
                  var service = context.RequestServices.GetRequiredService<IBrandService>();
                  var skip = context.GetArgument<int>(TimeGraphQlConstants.SKIP);
                  var take = context.GetArgument<int>(TimeGraphQlConstants.TAKE);
                  var searchText = context.GetArgument<string>(TimeGraphQlConstants.SEARCH_TEXT);
                  var res = await service.FindAsync(new BrandQueryModel
                  {
                      PageIndex = skip,
                      PageSize = take,
                      StoreId = context.GetExtension($"{context.Path}.{TimeGraphQlConstants.STORE}").ToString(),
                      SearchText = searchText
                  });
                  return res;
              }
            );

            FieldAsync<ContentType>("content"
            , arguments: new QueryArguments(
               new List<QueryArgument>
                   {
                        new QueryArgument<StringGraphType>
                        {
                            Name = "slug"
                        },
                        new QueryArgument<StringGraphType>
                        {
                            Name = "id"
                        }
                   }
                ),
            resolve: async context =>
            {
                var service = context.RequestServices.GetRequiredService<IContentService>();
                var seName = context.GetArgument<string>("slug");

                if (!string.IsNullOrEmpty(seName))
                    return await service.GetBySlugAsync(seName);

                var id = context.GetArgument<string>("id");
                return await service.GetByIdAsync(id);
            }
            );

            FieldAsync<ContentListType>("contents",
               arguments: new QueryArguments(
                  new List<QueryArgument>
                  {
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.SKIP
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.TAKE
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.STORE
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = TimeGraphQlConstants.SEARCH_TEXT
                        },
                        new QueryArgument<IdGraphType>
                        {
                            Name = "type"
                        },
                  }
              ),
              resolve: async context =>
              {
                  var service = context.RequestServices.GetRequiredService<IContentService>();
                  var skip = context.GetArgument<int>(TimeGraphQlConstants.SKIP);
                  var take = context.GetArgument<int>(TimeGraphQlConstants.TAKE);
                  var searchText = context.GetArgument<string>(TimeGraphQlConstants.SEARCH_TEXT);
                  var res = await service.FindAsync(new Contracts.Models.Cms.ContentQueryModel
                  {
                      PageIndex = skip,
                      PageSize = take,
                      StoreId = context.GetExtension($"{context.Path}.{TimeGraphQlConstants.STORE}").ToString(),
                      SearchText = searchText,
                      Type = context.GetArgument<string>("type")
                  });
                  return res;
              }
            );

            FieldAsync<ListGraphType<CountryType>>("countries",
                resolve: async context =>
                {
                    var service = context.RequestServices.GetRequiredService<ILocaleService>();
                    var res = await service.GetCountriesAsync();
                    return res;
                }
             );

            FieldAsync<ListGraphType<StateProvinceType>>("stateProvinces",
            arguments: new QueryArguments(
               new List<QueryArgument>
               {
                    new QueryArgument<IdGraphType>
                    {
                        Name = "countryCode"
                    },
               }
            ),
            resolve: async context =>
            {
                var service = context.RequestServices.GetRequiredService<ILocaleService>();
                var res = await service.GetStateProvincesAsync(context.GetArgument<string>("countryCode"));
                return res;
            }
            );
            #endregion

            #region Sales
            FieldAsync<ShoppingCartType>("cart",
            arguments: new QueryArguments(
                new List<QueryArgument>
                {
                        new QueryArgument<NonNullGraphType<StringGraphType>>
                        {
                            Name = "cart_id"
                        }
                }
            ),
            resolve: async context =>
            {
                var service = context.RequestServices.GetRequiredService<IShoppingCartService>();
                var cartId = context.GetArgument<string>("cart_id");
                var shoppingCart= await service.GetShoppingCartViewWithProductInforAsync(cartId);
                return shoppingCart;
            });
            #endregion
        }
    }
}