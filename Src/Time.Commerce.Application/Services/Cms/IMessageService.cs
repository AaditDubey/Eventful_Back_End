using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Contracts.Views.Common;

namespace Time.Commerce.Application.Services.Cms
{
    public interface IMessageService
    {
        Task<MessageView> CreateAsync(CreateMessageModel model, CancellationToken cancellationToken = default);
        Task<MessageView> UpdateAsync(UpdateMessageModel model, CancellationToken cancellationToken = default);
        Task<MessageView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PageableView<MessageView>> FindAsync(MessageQueryModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
    }
}
