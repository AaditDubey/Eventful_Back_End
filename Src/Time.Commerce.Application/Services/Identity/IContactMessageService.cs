using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Domains.Entities.Identity;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Application.Services.Identity
{
    public interface IContactMessageService
    {
        Task<ContactMessageView> CreateContactMessageAsync(CreateContactMessageModel model, CancellationToken cancellationToken = default);
        Task<ContactMessageView> GetContactMessageByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<DataFilterPagingResult<ContactMessage>> GetListAsync(DataFilterPaging<ContactMessage> model, CancellationToken cancellationToken = default);
        Task<PageableView<ContactMessageView>> FindAsync(ContactMessageQueryModel model, CancellationToken cancellationToken = default);
        Task<ContactMessageView> UpdateContactMessageAsync(UpdateContactMessageModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteContactMessageAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
    }
}
