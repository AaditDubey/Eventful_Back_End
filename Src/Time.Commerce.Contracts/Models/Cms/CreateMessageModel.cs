namespace Time.Commerce.Contracts.Models.Cms
{
    public class CreateMessageModel
    {
        public string CustomerId { get; set; }
        public string StoreId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subject { get; set; }
        public string Enquiry { get; set; }
        public string VendorId { get; set; }
        public string Type { get; set; }
    }
}
