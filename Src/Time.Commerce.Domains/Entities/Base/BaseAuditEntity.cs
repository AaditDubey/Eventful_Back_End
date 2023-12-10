namespace Time.Commerce.Domains.Entities.Base
{
    public abstract partial class BaseAuditEntity : BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
