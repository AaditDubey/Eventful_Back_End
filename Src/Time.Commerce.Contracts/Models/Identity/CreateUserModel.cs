namespace Time.Commerce.Contracts.Models.Identity
{
    public class CreateUserModel : RegisterModel
    {
        public bool Active { get; set; }
        public string VendorId { get; set; }
        public bool IsSystemAccount { get; set; }
        public List<string> Roles { get; set; }
    }
}
