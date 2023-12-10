using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Identity;

public class BankAccount : SubBaseEntity
{
    public string BankCode { get; set; }
    public string BankName { get; set; }
    public string SwiftCode { get; set; }
    public string BankAccountNumber { get; set; }
}