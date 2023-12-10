using AutoMapper;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;

namespace Time.Commerce.Application.Mappers.Common;

public class CommonMapper : Profile
{
    public CommonMapper()
    {
        CreateMap<CustomAttribute, CustomAttributeView>();
        CreateMap<GenericAttribute, CustomAttributeView>();
        CreateMap<CustomAttributeView, CustomAttribute>();
    }
}
