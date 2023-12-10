using Time.Commerce.Contracts.Enums.Identity;

namespace Time.Commerce.Contracts.Models.Identity;

public class CreateContactMessageModel
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public ContactMessageType Type { get; set; }
}