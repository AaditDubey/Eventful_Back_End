using Microsoft.AspNetCore.Http;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Cms;

namespace Time.Commerce.Proxy.Cms
{
    public class StorageProxy : IStorageProxy
    {
        private readonly IStorageService _storageService;
        public StorageProxy(IStorageService storageService)
            => _storageService = storageService;
        public async Task<FileView> UploadFileAsync(IFormFile file, string directory = null, CancellationToken cancellationToken = default)
            => await _storageService.UploadFileAsync(file, directory, cancellationToken);

        public async Task<IList<FileView>> UploadFilesAsync(List<UploadFileModel> files, string directory = null, CancellationToken cancellationToken = default)
            =>  await _storageService.UploadFilesAsync(files, directory, cancellationToken);
    }
}
