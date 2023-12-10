using FluentValidation;

namespace Time.Commerce.Contracts.Models.Identity
{
    public class UpdateUserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string PhonePrefix { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public bool Active { get; set; }
        public bool IsSystemAccount { get; set; }
        public string VendorId { get; set; }
        public List<string> Roles { get; set; }
    }
    public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
    {
        public UpdateUserModelValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
        }
    }
}
