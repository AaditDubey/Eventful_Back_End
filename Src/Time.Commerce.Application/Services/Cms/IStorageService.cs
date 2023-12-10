using Microsoft.AspNetCore.Http;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Cms;

namespace Time.Commerce.Application.Services.Cms
{
    public interface IStorageService
    {
        Task<FileView> UploadFileAsync(IFormFile file, string directory = null, CancellationToken cancellationToken = default);
        Task<IList<FileView>> UploadFilesAsync(List<IFormFile> files, string directory = null, CancellationToken cancellationToken = default);
        Task<IList<FileView>> UploadFilesAsync(List<UploadFileModel> files, string directory = null, CancellationToken cancellationToken = default);
        void DeleteFiles(List<string> paths);
    }
}
