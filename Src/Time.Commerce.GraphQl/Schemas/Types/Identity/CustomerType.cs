using GraphQL.Types;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Identity;

namespace Time.Commerce.GraphQl.Schemas.Types.Identity
{
    public class CustomerType : ObjectGraphType<UserView>
    {
        public CustomerType()
        {
            Name = "CustomerType";
            Description = "CustomerType";
            Field(d => d.Id, nullable: false).Description("Id");
            Field(d => d.FullName, nullable: true).Description("FullName");
            Field(d => d.PhoneNumber, nullable: false).Description("PhoneNumber");
            Field(d => d.Email, nullable: false).Description("Email");
        }
    }

    public class CustomerRegisterInputType : InputObjectGraphType<RegisterModel>
    {
        public CustomerRegisterInputType()
        {
            Name = "CustomerRegisterInputType";
            Field(d => d.Email, nullable: false).Description("Email");
            Field(d => d.Password, nullable: false).Description("Password");
            Field(d => d.StoreId, nullable: true).Description("StoreId");
            Field(d => d.FullName, nullable: false).Description("FullName");
            Field(d => d.PhoneNumber, nullable: true).Description("PhoneNumber");
        }
    }
}
