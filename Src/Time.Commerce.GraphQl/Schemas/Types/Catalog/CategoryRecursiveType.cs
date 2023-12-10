using GraphQL.Types;
using Time.Commerce.Contracts.Views.Catalog;

namespace Time.Commerce.GraphQl.Schemas.Types.Catalog
{
    public class CategoryRecursiveType : ObjectGraphType<CategoryRecursiveView>
    {
        public CategoryRecursiveType()
        {
            Name = "CategoryRecursiveType";
            Description = "Category Recursive Type";
            Field(d => d.Id, nullable: false).Description("Id");
            Field(d => d.Name, nullable: true).Description("Name");
            Field(d => d.SeName, nullable: true).Description("SeName");
            Field<ListGraphType<CategoryRecursiveType>>("children");
            ////Field(x => x.Children, type: typeof(ListGraphType<CategoryType>)).Description("CategoryType s.");
            //Field<ListGraphType<CategoryType>>(
            //    "children",
            //    resolve: context =>
            //    {
            //        return new List<CategoryModel>() { new CategoryModel { Name = "adsfd", Id = "adsfdsaere" } };
            //    }
            //);
        }
    }
}
