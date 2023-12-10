using GraphQL.Types;
using Time.Commerce.Contracts.Views.Cms;

namespace Time.Commerce.GraphQl.Schemas.Types.Cms
{
    public class StateProvinceType : ObjectGraphType<StateProvinceView>
    {
        public StateProvinceType()
        {
            Name = "StateProvinceType";
            Description = "State Province Type";
            Field(d => d.Id, nullable: false).Description("Id");
            Field(d => d.CountryCode, nullable: true).Description("CountryCode");
            Field(d => d.City, nullable: true).Description("City");
            Field(d => d.StateProvince, nullable: true).Description("StateProvince");
        }
    }
}
