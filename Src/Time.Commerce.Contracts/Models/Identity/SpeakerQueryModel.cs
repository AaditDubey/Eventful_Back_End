using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Identity
{
    public class SpeakerQueryModel : BaseQueryModel
    {
        public bool? Active { get; set; }
        public List<string> Ids { get; set; }
    }
}
