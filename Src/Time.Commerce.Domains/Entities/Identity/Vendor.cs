using System.ComponentModel.DataAnnotations;
using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Domains.Entities.Identity;

/// <summary>
/// Represents a vendor
/// </summary>
public partial class Vendor : BaseAuditEntity
{
    private ICollection<string> _vendorNotes;
    private ICollection<string> _appliedDiscounts;

    public Vendor()
    {
        Address = new Address();
    }
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the sename
    /// </summary>
    public string SeName { get; set; }

    /// <summary>
    /// Gets or sets the image
    /// </summary>
    public Image? Image { get; set; }
    /// <summary>
    /// Gets or sets the email
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the store identifier
    /// </summary>
    public string StoreId { get; set; }

    /// <summary>
    /// Gets or sets the admin comment
    /// </summary>
    public string AdminComment { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is active
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity has been deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }


    /// <summary>
    /// Gets or sets the meta keywords
    /// </summary>
    public string MetaKeywords { get; set; }

    /// <summary>
    /// Gets or sets the meta description
    /// </summary>
    public string MetaDescription { get; set; }

    /// <summary>
    /// Gets or sets the meta title
    /// </summary>
    public string MetaTitle { get; set; }

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether customers can select the page size
    /// </summary>
    public bool AllowCustomersToSelectPageSize { get; set; }

    /// <summary>
    /// Gets or sets the available customer selectable page size options
    /// </summary>
    public string PageSizeOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the vendor allows customer reviews
    /// </summary>
    public bool AllowCustomerReviews { get; set; }

    /// <summary>
    /// Gets or sets the rating sum (approved reviews)
    /// </summary>
    public int ApprovedRatingSum { get; set; }

    /// <summary>
    /// Gets or sets the rating sum (not approved reviews)
    /// </summary>
    public int NotApprovedRatingSum { get; set; }

    /// <summary>
    /// Gets or sets the total rating votes (approved reviews)
    /// </summary>
    public int ApprovedTotalReviews { get; set; }

    /// <summary>
    /// Gets or sets the total rating votes (not approved reviews)
    /// </summary>
    public int NotApprovedTotalReviews { get; set; }

    /// <summary>
    /// Gets or sets the commission rate
    /// </summary>
    public double? Commission { get; set; }

    /// <summary>
    /// Gets or sets the vendor address
    /// </summary>
    public virtual Address Address { get; set; }

    /// <summary>
    /// Gets or sets the coordinates
    /// </summary>
    public GeoCoordinates Coordinates { get; set; }

    /// <summary>
    /// Gets or sets vendor notes
    /// </summary>
    public virtual ICollection<string> VendorNotes
    {
        get { return _vendorNotes ??= new List<string>(); }
        protected set { _vendorNotes = value; }
    }
    /// <summary>
    /// Gets or sets the collection of applied discounts
    /// </summary>
    public virtual ICollection<string> AppliedDiscounts
    {
        get { return _appliedDiscounts ??= new List<string>(); }
        protected set { _appliedDiscounts = value; }
    }

}
