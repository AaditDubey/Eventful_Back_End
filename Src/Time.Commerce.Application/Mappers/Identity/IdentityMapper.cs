using AutoMapper;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Domains.Entities.Identity;

namespace Time.Commerce.Application.Mappers.Identity;

public class IdentityMapper : Profile
{
    public IdentityMapper()
    {
        CreateMap<User, UserView>();
        CreateMap<CreateUserModel, User>().ForMember(x => x.Roles, opt => opt.Ignore());
        CreateMap<UserStoreMapping, UserStoreMappingView>();

        CreateMap<Role, RoleView>();
        CreateMap<Role, SummaryRoleView>();
        CreateMap<CreateRoleModel, Role>();

        CreateMap<ContactMessage, ContactMessageView>();
        CreateMap<CreateContactMessageModel, ContactMessage>();

        CreateMap<CreateStoreModel, Store>();
        CreateMap<Store, StoreView>();
        CreateMap<Store, StoreDetailsView>();

        CreateMap<Vendor, VendorView>();
        CreateMap<CreateVendorModel, Vendor>();

        CreateMap<BankAccount, BankAccountView>();
        CreateMap<BankAccountModel, BankAccount>();

        CreateMap<Speaker, SpeakerView>();
        CreateMap<CreateSpeakerModel, Speaker>();
    }
}
