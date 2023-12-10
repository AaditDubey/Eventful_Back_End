using FluentValidation;

namespace Time.Commerce.Contracts.Models.Common
{
    public class GenericAttributeModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class GenericAttributeModelValidator : AbstractValidator<GenericAttributeModel>
    {
        public GenericAttributeModelValidator()
        {
            RuleFor(x => x.Key).NotNull().NotEmpty();
            RuleFor(x => x.Value).NotNull().NotEmpty();
        }
    }
}
