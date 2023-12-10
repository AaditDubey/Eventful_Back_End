using System.ComponentModel.DataAnnotations;
using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Cms
{
    public class Field : SubBaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        public bool Required { get; set; }
        public bool Unique { get; set; }
    }
}
