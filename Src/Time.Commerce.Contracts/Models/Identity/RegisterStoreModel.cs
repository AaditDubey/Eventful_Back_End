using FluentValidation;

namespace Time.Commerce.Contracts.Models.Identity
{
    public class RegisterStoreModel : RegisterModel
    {
        public string Category { get; set; }
        public string ThemeId { get; set; }
        public bool InstallDataSample { get; set; }
        public string BaseCurrency { get; set; }
        public string CountryCode { get; set; }
    }
    public class RegisterStoreModelValidator : AbstractValidator<RegisterStoreModel>
    {
        public RegisterStoreModelValidator()
        {
            RuleFor(x => x.StoreId).NotNull().NotEmpty();
        }
    }
}
