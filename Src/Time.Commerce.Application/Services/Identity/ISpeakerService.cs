using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;

namespace Time.Commerce.Application.Services.Identity;

public interface ISpeakerService
{
    Task<SpeakerView> CreateAsync(CreateSpeakerModel model, CancellationToken cancellationToken = default);
    Task<SpeakerView> UpdateAsync(UpdateSpeakerModel model, CancellationToken cancellationToken = default);
    Task<SpeakerView> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<SpeakerView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<PageableView<SpeakerView>> FindAsync(SpeakerQueryModel model, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default);
    Task<SpeakerView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default);
    Task<SpeakerView> DeleteImage(string id, CancellationToken cancellationToken = default);
}
