using System.ComponentModel.DataAnnotations;
using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Identity
{
    public partial class User : BaseAuditEntity
    {
        private ICollection<Role> _roles;
        private ICollection<UserStoreMapping> _userStoreMapping;

     
        public string FullName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Language { get; set; }
        public string PhonePrefix { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Avatar { get; set; }
        public string VendorId { get; set; }
        /// <summary>
        /// Gets or sets the password
        /// </summary>
        //public string Password { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the user is active
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the user account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }
        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }
        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDate { get; set; }
        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDate { get; set; }
        /// <summary>
        /// Last date to change password
        /// </summary>
        public DateTime? PasswordChangeDate { get; set; }
        #region Navigation properties
        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual ICollection<Role> Roles
        {
            get { return _roles ?? (_roles = new List<Role>()); }
            protected set { _roles = value; }
        }

        public virtual ICollection<UserStoreMapping> UserStoreMapping
        {
            get { return _userStoreMapping ??= new List<UserStoreMapping>(); }
            protected set { _userStoreMapping = value; }
        }
        #endregion
    }
}
