using FluentValidation;

namespace Time.Commerce.Contracts.Models.Identity
{
    public class RegisterModel
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string PhonePrefix { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string StoreId { get; set; }
    }
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
