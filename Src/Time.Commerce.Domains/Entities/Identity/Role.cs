using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Identity
{
    public partial class Role : BaseAuditEntity
    {
        /// <summary>
        /// Gets or sets the role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the role name
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// <summary>
        /// Gets or sets a value indicating whether the user role is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user role is system
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// Gets or sets the user role system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the users must change passwords after a specified time
        /// </summary>
        public bool EnablePasswordLifetime { get; set; }
    }
}
