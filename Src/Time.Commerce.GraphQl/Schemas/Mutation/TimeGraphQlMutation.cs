using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Application.Services.Sales;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Models.Sales;
using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.GraphQl.Constants;
using Time.Commerce.GraphQl.Schemas.Types.Identity;
using Time.Commerce.GraphQl.Schemas.Types.Sale;
using ShoppingCartType = Time.Commerce.GraphQl.Schemas.Types.Sale.ShoppingCartType;

namespace Time.Commerce.GraphQl.Schemas.Mutation
{
    public class TimeGraphQlMutation : ObjectGraphType
    {
        public TimeGraphQlMutation()
        {
            Name = "Mutations";

            #region Sales
            Field<StringGraphType>(
                "createEmptyCart",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType>
                    {
                        Name = TimeGraphQlConstants.STORE
                    },
                    new QueryArgument<StringGraphType> { Name = "customer_id" }
                ),
                resolve: context =>
                {
                    var service = context.RequestServices.GetRequiredService<IShoppingCartService>();
                    var storeId = context.GetExtension($"{context.Path}.{TimeGraphQlConstants.STORE}").ToString();
                    return service.CreateEmptyCartAsync(storeId, context.GetArgument<string>("customer_id"));
                }
            );

            Field<ShoppingCartType>(
                "addProductsToCart",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "cart_id" },
                    new QueryArgument<StringGraphType> { Name = "customer_id" },
                    new QueryArgument<NonNullGraphType<ListGraphType<AddProductToCartInputType>>> { Name = "cart_items" }
                ),
                resolve: context =>
                {
                    var service = context.RequestServices.GetRequiredService<IShoppingCartService>();
                    var cartId = context.GetArgument<string>("cart_id");
                    var items = context.GetArgument<IEnumerable<ShoppingCartItemModel>>("cart_items");

                    var shoppingCartView = service.AddProductsToCartAsync(cartId, items.ToList()).GetAwaiter().GetResult();
                    return shoppingCartView;
                }
            );

            Field<ShoppingCartType>(
                "updateCartItems",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "cart_id" },
                    new QueryArgument<NonNullGraphType<ListGraphType<UpdateCartItemInputType>>> { Name = "cart_items" }
                ),
                resolve: context =>
                {
                    var service = context.RequestServices.GetRequiredService<IShoppingCartService>();
                    var cartId = context.GetArgument<string>("cart_id");
                    var items = context.GetArgument<IEnumerable<ShoppingCartItemModel>>("cart_items");

                    var shoppingCartView = service.UpdateCartItemsAsync(cartId, items.ToList()).GetAwaiter().GetResult();
                    return shoppingCartView;
                }
            );


            Field<ShoppingCartType>(
                "removeItemFromCart",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "cart_id" },
                    new QueryArgument<StringGraphType> { Name = "cart_item_id" }
                ),
                resolve: context =>
                {
                    var service = context.RequestServices.GetRequiredService<IShoppingCartService>();
                    var cartId = context.GetArgument<string>("cart_id");
                    var cartItemId = context.GetArgument<string>("cart_item_id");
                    var shoppingCartView = service.RemoveCartItemsAsync(cartId, cartItemId).GetAwaiter().GetResult();
                    return shoppingCartView;
                }
            );
            #endregion

            #region Identity
            Field<CustomerType>(
               "createCustomer",
               arguments: new QueryArguments(
                    new QueryArgument<IdGraphType>
                    {
                        Name = TimeGraphQlConstants.STORE
                    },
                   new QueryArgument<NonNullGraphType<CustomerRegisterInputType>> { Name = "customer" }
               ),
               resolve: context =>
               {
                   var service = context.RequestServices.GetRequiredService<IAccountService>();
                   var storeId = context.GetExtension($"{context.Path}.{TimeGraphQlConstants.STORE}").ToString();
                   var model = context.GetArgument<RegisterModel>("customer");
                   model.StoreId = storeId;
                   return service.Register(model).GetAwaiter().GetResult();
               }
            );

            Field<LoginResultType>(
               "connectToken",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<StringGraphType>>
                   {
                       Name = "email"
                   },
                    new QueryArgument<NonNullGraphType<StringGraphType>>
                    {
                        Name = "password"
                    }
               ),
               resolve: context =>
               {
                   var service = context.RequestServices.GetRequiredService<IAuthService>();
                   var email = context.GetArgument<string>("email");
                   var password = context.GetArgument<string>("password");
                   return service.SignIn(new LoginModel { UserName = email, Password = password }).GetAwaiter().GetResult();
               }
            );
            #endregion

        }
    }
}