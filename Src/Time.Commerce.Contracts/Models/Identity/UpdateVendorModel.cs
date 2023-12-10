using FluentValidation;

namespace Time.Commerce.Contracts.Models.Identity
{
    public class UpdateVendorModel : CreateVendorModel
    {
        public string Id { get; set; }
    }
    public class UpdateVendorModelValidator : AbstractValidator<UpdateVendorModel>
    {
        public UpdateVendorModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
