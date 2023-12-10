using System.ComponentModel.DataAnnotations;
using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Cms
{
    public partial class Model : BaseAuditEntity
    {
        /// <summary>
        /// Gets or sets the model name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the model description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the model fields
        /// </summary>
        public List<Field> FieldSettings { get; set; } = new List<Field>();
        /// <summary>
        /// Gets or sets the model api settings
        /// </summary>
        public ApiSettings ApiSettings { get; set; } = new ApiSettings();
    }
}
