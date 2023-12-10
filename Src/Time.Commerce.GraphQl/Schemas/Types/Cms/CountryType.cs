using GraphQL.Types;
using Time.Commerce.Contracts.Views.Cms;

namespace Time.Commerce.GraphQl.Schemas.Types.Cms
{
    public class CountryType : ObjectGraphType<CountryView>
    {
        public CountryType()
        {
            Name = "CountryType";
            Description = "Country Type";
            Field(d => d.Id, nullable: false).Description("Id");
            Field(d => d.Name, nullable: true).Description("Name");
            Field(d => d.Code, nullable: true).Description("Code");
            Field(d => d.PhonePrefix, nullable: true).Description("PhonePrefix");
        }
    }
}
