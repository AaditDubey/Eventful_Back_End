using GraphQL.Types;
using Time.Commerce.Contracts.Views.Identity;

namespace Time.Commerce.GraphQl.Schemas.Types.Identity
{
    public class LoginResultType : ObjectGraphType<LoginView>
    {
        public LoginResultType()
        {
            Name = "LoginResultType";
            Description = "LoginResultType";
            Field(d => d.AccessToken, nullable: false).Description("AccessToken");
            Field(d => d.RefreshToken, nullable: true).Description("RefreshToken");
            Field(d => d.Exp, nullable: false).Description("Expired");
            Field<CustomerType>("User");
        }
    }
}
