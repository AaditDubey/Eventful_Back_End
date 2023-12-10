using System.ComponentModel.DataAnnotations;
using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Identity;

public partial class ContactMessage : BaseAuditEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Subject { get; set; }
    public string Content { get; set; }
    public ContactMessageType Type { get; set; }
}

public enum ContactMessageType
{
    Message = 1,
    Subscribe = 2
}
