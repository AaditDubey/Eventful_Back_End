using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Cms
{
    public class Message : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets full name
        /// </summary>
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets enquiry
        /// </summary>
        public string Enquiry { get; set; }

        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public string VendorId { get; set; }
        public string Type { get; set; }
        #endregion
    }
}
