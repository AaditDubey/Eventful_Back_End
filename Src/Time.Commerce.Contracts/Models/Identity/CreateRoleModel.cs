namespace Time.Commerce.Contracts.Models.Identity;

public class CreateRoleModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
    public bool IsSystemRole { get; set; }
    public string SystemName { get; set; }
    public bool EnablePasswordLifetime { get; set; }
}
