using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Domains.Entities.Cms;

namespace Time.Commerce.Application.Services.Cms;

public interface IWidgetService
{
    Task<WidgetView> CreateAsync(Widget model, CancellationToken cancellationToken = default);
    Task<WidgetView> UpdateAsync(Widget model, CancellationToken cancellationToken = default);
    Task<WidgetView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<PageableView<WidgetView>> FindAsync(WidgetQueryModel model, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
}
