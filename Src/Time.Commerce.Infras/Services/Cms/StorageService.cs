using Microsoft.AspNetCore.Http;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Domains.Entities.Identity;

namespace Time.Commerce.Infras.Services.Cms
{
    public class StorageService : IStorageService
    {
        private string currentDirectory;
        private readonly IWorkContext _workContext;

        public StorageService(IWorkContext workContext)
        {
            _workContext = workContext;
            currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(currentDirectory))
                Directory.CreateDirectory(currentDirectory);
        }

        public async Task<FileView> UploadFileAsync(IFormFile file, string directory = null, CancellationToken cancellationToken = default)
        {
            directory = FormatDirectory(directory);
            if (file.Length == 0)
                return null;
            var rootPath = GetRootPath(directory);
            var fileName = file.FileName.Replace(" ", "_");
            string imagePath = Path.Combine(rootPath, fileName);
            if (File.Exists(imagePath))
                File.Delete(imagePath);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileInfor = new FileInfo(imagePath);
            return new FileView
            {
                Id = imagePath,
                Type = fileInfor.Extension,
                Name = fileInfor.Name.Trim(),
                Path = string.IsNullOrEmpty(directory) ? $"{GetRootWebPath()}/{fileName}" : $"{directory}/{fileName}",
                Size = ByteToString(fileInfor.Length),
                CreationTime = fileInfor.CreationTimeUtc
            };
        }

        public async Task<IList<FileView>> UploadFilesAsync(List<IFormFile> files, string directory = null, CancellationToken cancellationToken = default)
        {

            directory = FormatDirectory(directory);
            if (files.Count == 0)
                return null;

            var filesInfor = new List<FileView>();
            var rootPath = GetRootPath(directory);
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = file.FileName.Replace(" ", "_");
                    string imagePath = Path.Combine(rootPath, fileName);
                    if (File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var fileInfor = new FileInfo(imagePath);
                    filesInfor.Add(new FileView
                    {
                        Id = imagePath,
                        Type = fileInfor.Extension,
                        Name = fileInfor.Name,
                        Path = string.IsNullOrEmpty(directory) ? $"{GetRootWebPath()}/{fileName}" : $"{directory}/{fileName}",
                        Size = ByteToString(fileInfor.Length),
                        CreationTime = fileInfor.CreationTimeUtc
                    });
                }
            }
            return filesInfor;
        }

        public async Task<IList<FileView>> UploadFilesAsync(List<UploadFileModel> files, string directory = null, CancellationToken cancellationToken = default)
        {
            directory = FormatDirectory(directory);
            if (files.Count == 0)
                return null;

            var filesInfor = new List<FileView>();
            var rootPath = GetRootPath(directory);
            foreach (var file in files)
            {
                if (file.Data.Length > 0)
                {
                    var fileName = file.FileName.Replace(" ", "_");
                    string imagePath = Path.Combine(rootPath, fileName);
                    if (File.Exists(imagePath))
                        File.Delete(imagePath);

                    using var stream = File.Create(imagePath);
                    await stream.WriteAsync(file.Data, 0, file.Data.Length);

                    var fileInfor = new FileInfo(imagePath);
                    filesInfor.Add(new FileView
                    {
                        Id = imagePath,
                        Type = fileInfor.Extension,
                        Name = fileInfor.Name.Trim(),
                        Path = string.IsNullOrEmpty(directory) ? $"{GetRootWebPath()}/{fileName}" : $"{directory}/{fileName}",
                        Size = ByteToString(fileInfor.Length),
                        CreationTime = fileInfor.CreationTimeUtc
                    });
                }
            }
            return filesInfor;
        }

        public void DeleteFiles(List<string> paths)
        {
            foreach (var path in paths)
            {
                if (!File.Exists(path))
                {
                    continue;
                }
                File.Delete(path);
            }
        }
        #region Private Methods
        protected string FormatDirectory(string directory)
        {
            if (!string.IsNullOrEmpty(directory) && !directory.StartsWith("/s3"))
                return $"s3/{directory}";
            var storeId = _workContext.GetCurrentStoreId();
            return $"s3/{storeId}/content";
        }

        protected string GetRootPath(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                directory = GetRootWebPath();
            //var pathToSave = Path.Combine(currentDirectory, directory);
            var pathToSave = Path.Combine(currentDirectory, directory);
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);
            return pathToSave;
        }
        protected string GetRootWebPath()
        {
            var storeId = _workContext.GetCurrentStoreId();
            return $"{currentDirectory}/s3/{storeId}/content";
        }
        protected string ByteToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return $"0 {suf[0]}";

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString()} {suf[place]}";
        }
        #endregion

    }
}
