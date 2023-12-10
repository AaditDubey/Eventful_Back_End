using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Time.Commerce.Application.Services.Cms;
using TimeNet.Abstractions;

namespace TimeNet.Areas.Admin.Controllers;
[Area("Admin")]
public class StorageController : BaseCookieAuthController
{
    #region Fields
    private readonly IStorageService _storageService;
    #endregion

    #region Ctor
    public StorageController(IStorageService storageService)
        => _storageService = storageService;
    #endregion

    #region Apis
    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken = default)
        => Ok(await _storageService.UploadFileAsync(file, string.Empty, cancellationToken));

    [HttpPost]
    public ActionResult DeleteFiles([FromBody] List<string> paths)
    {
        _storageService.DeleteFiles(paths);
        return Ok("Delete files success");
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFiles(List<IFormFile> files, CancellationToken cancellationToken = default)
        => Ok(await _storageService.UploadFilesAsync(files, string.Empty, cancellationToken));

    //[HttpPost]
    //[DisableRequestSizeLimit]
    //public async Task<IActionResult> UploadFilesWithAI(List<IFormFile> files, CancellationToken cancellationToken = default)
    //{
    //    List<string> images = new List<string>();
    //    foreach (var file in files)
    //    {
    //        using (var multipartFormContent = new MultipartFormDataContent())
    //        {
    //            var stream = file.OpenReadStream();
    //            var memoryStream = new MemoryStream();
    //            stream.CopyTo(memoryStream);
    //            byte[] byteArray = memoryStream.ToArray();
    //            var byteContent = new ByteArrayContent(byteArray);
    //            byteContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
    //            multipartFormContent.Add(byteContent, name: "file", fileName: file.FileName);

    //            HttpClient httpClient = new HttpClient();
    //            var response = await httpClient.PostAsync("https://time-ai-api.bisfu.com/uploadfileAndDowload/?action=null&removeBg=true", multipartFormContent);
    //            response.EnsureSuccessStatusCode();
    //            var res = await response.Content.ReadAsStringAsync();
    //            images.Add(res);
    //        }
    //    }
    //    return Ok(images);
    //}
    #endregion
}