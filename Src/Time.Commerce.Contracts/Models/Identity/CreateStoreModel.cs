using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Identity
{
    public class CreateStoreModel
    {
        public string StoreId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool DefaultStore { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether SSL is enabled
        /// </summary>
        public bool SslEnabled { get; set; }

        /// <summary>
        /// Gets or sets the store secure URL (HTTPS)
        /// </summary>
        public string SecureUrl { get; set; }
        public IList<string> Domains { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the default language for this store; "" is set when we use the default language display order
        /// </summary>
        public string DefaultLanguageId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the default waregouse for this store
        /// </summary>
        public string DefaultWarehouseId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the default country for this store
        /// </summary>
        public string DefaultCountryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the default currency for this store
        /// </summary>
        public string DefaultCurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company registration number
        /// </summary>
        public string CompanyRegNo { get; set; }

        /// <summary>
        /// Gets or sets the company address
        /// </summary>
        public string CompanyAddress { get; set; }

        /// <summary>
        /// Gets or sets the store phone number
        /// </summary>
        public string CompanyPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the company VAT (used in Europe Union countries)
        /// </summary>
        public string CompanyVat { get; set; }

        /// <summary>
        /// Gets or sets the company email
        /// </summary>
        public string CompanyEmail { get; set; }

        /// <summary>
        /// Gets or sets the company opening hours
        /// </summary>
        public string CompanyHours { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the default admin theme for this store
        /// </summary>
        public string DefaultAdminTheme { get; set; }
       
        public string ThemeId { get; set; }
        public string DefaultCurrency { get; set; }
        public string CountryCode { get; set; }
        public string ApiKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public BankAccountModel BankAccount { get; set; }
        public ImageModel Logo { get; set; }
    }
}
