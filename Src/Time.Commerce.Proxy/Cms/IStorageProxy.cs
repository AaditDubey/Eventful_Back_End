﻿using Microsoft.AspNetCore.Http;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Cms;

namespace Time.Commerce.Proxy.Cms
{
    public interface IStorageProxy
    {
        Task<FileView> UploadFileAsync(IFormFile file, string directory = null, CancellationToken cancellationToken = default);
        Task<IList<FileView>> UploadFilesAsync(List<UploadFileModel> files, string directory = null, CancellationToken cancellationToken = default);
    }
}
