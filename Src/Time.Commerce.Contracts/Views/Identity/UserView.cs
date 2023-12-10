
namespace Time.Commerce.Contracts.Views.Identity
{
    public class UserView
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string PhonePrefix { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string VendorId { get; set; }
        public IList<UserStoreMappingView> UserStoreMapping { get; set; }
        public IList<SummaryRoleView> Roles { get; set; }
    }
}
