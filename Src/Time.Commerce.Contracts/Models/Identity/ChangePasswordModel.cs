namespace Time.Commerce.Contracts.Models.Identity
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string UserId { get; set; }
    }
}
