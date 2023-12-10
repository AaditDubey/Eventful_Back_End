using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Cms
{
    public partial class ApiSettings
    {
        public GetByIdSetting GetById { get; set; }
        public QuerySetting Query { get; set; }
        public CountSetting Count { get; set; }
        public CreateSetting Create { get; set; }
        public UpdateSetting Update { get; set; }
        public DeleteSetting Delete { get; set; }
    }

    public partial class GetByIdSetting : BaseApiSettings
    {
    }
    public partial class QuerySetting : BaseApiSettings
    {
    }
    public partial class CountSetting : BaseApiSettings
    {
    }
    public partial class CreateSetting : BaseApiSettings
    {
    }
    public partial class UpdateSetting : BaseApiSettings
    {
    }
    public partial class DeleteSetting : BaseApiSettings
    {
    }

    public partial class BaseApiSettings : SubBaseEntity
    {
        public bool Publish { get; set; }
        public bool Authencation { get; set; }
        public List<string> RoleIds { get; set; }
    }
}
