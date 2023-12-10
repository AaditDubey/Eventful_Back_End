namespace Time.Commerce.Contracts.Views.Common
{
    public class BaseAuditView
    {
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
