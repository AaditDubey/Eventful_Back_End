using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Catalog;
using TimeNet.Abstractions;
using TimeNet.Areas.Admin.Extensions;
using TimeNet.Areas.Admin.Models.Datatable;

namespace TimeNet.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : BaseCookieEventHostAuthController
{
    #region Fields
    private readonly IProductService _productService;
    private readonly IWorkContext _workContext;
    private readonly IChatGptService _chatGptService;
    private readonly IStorageService _storageService;
    private readonly ICategoryService _categoryService;
    #endregion

    #region Ctor
    public ProductController(IProductService productService, IWorkContext workContext, IChatGptService chatGptService, IStorageService storageService, ICategoryService categoryService)
    {
        _productService = productService;
        _workContext = workContext;
        _chatGptService = chatGptService;
        _storageService = storageService;
        _categoryService = categoryService;
    }
    #endregion

    #region Mvc Actions
    public IActionResult Index(string searchText)
    {
        ViewBag.SearchText = searchText;
        return View();
    }

    public IActionResult Add()
    {
        return View();
    }

    public IActionResult Update(string id)
    {
        ViewBag.Id = id;
        return View();
    }

    public IActionResult AddByAi()
    {
        return View();
    }
    #endregion

    #region Apis
    [HttpPost]
    public async Task<IActionResult> Get(DataTableRequestModel<ProductQueryModel> model, CancellationToken cancellationToken)
    {
        var pagingInput = model.ParseToPagingInput();
        ProductQueryModel productQueryModel = new ProductQueryModel
        {
            StoreId = _workContext.GetCurrentStoreId(),
            Published = model.FilterModel.Published,
            SearchText = model.FilterModel.SearchText,
            PageIndex = pagingInput.Page,
            PageSize = pagingInput.PageSize,
            OrderBy = pagingInput.OrderBy,
            Ascending = !pagingInput.Descending,
        };
        var products = await _productService.FindAsync(productQueryModel, cancellationToken);
        DataTableResponseModel dataTableResponseModel = new DataTableResponseModel();
        dataTableResponseModel.GetFromPagingResult<ProductView>(products);

        return Ok(dataTableResponseModel);
    }

    [HttpGet]
    public async Task<IActionResult> FindById(string id, CancellationToken cancellationToken)
     => Ok(await _productService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> FindProducts([FromBody] ProductQueryModel model, CancellationToken cancellationToken)
    {
        var products = await _productService.FindAsync(model, cancellationToken);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateProductModel model, CancellationToken cancellationToken)
    {
        model.StoreId = _workContext.GetCurrentStoreId();
        var product = await _productService.CreateAsync(model, cancellationToken);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductByAi([FromBody] CreateProductByAIModel model, CancellationToken cancellationToken)
    {
        var language = Request.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;
        var storeId = _workContext.GetCurrentStoreId();
        model.StoreId = storeId;
        if (model.CreateShortDescription)
        {
            var desc = "";
            if (language == "vi")
                desc = await _chatGptService.ChatAsync($"Hãy viết mô tả sản phẩm {model.Name}", cancellationToken);
            else
                desc = await _chatGptService.ChatAsync($"Write product description for {model.Name}", cancellationToken);
            model.ShortDescription = desc;
            model.Description = desc;
        }
        else if (model.OptimiseDescription)
        {
            var str = "";
            if (language == "vi")
                str = await _chatGptService.ChatAsync($"Sử dụng {model.Description}, viết mô tả sản phẩm hay nhất, khoảng 2000 ký tự", cancellationToken);
            else
                str = await _chatGptService.ChatAsync($"Use {model.Description}, create a better description for the product , keep it  around 2000 characters", cancellationToken);
            var categories = str.Split('/');
            model.Description = str;
        }

        if (model.CreateMetaKeywords)
        {
            var desc = "";
            if (language == "vi")
                desc = await _chatGptService.ChatAsync($"Hãy viết meta keywords cho sản phẩm {model.Name}", cancellationToken);
            else
                desc = await _chatGptService.ChatAsync($"Write meta keywords for {model.Name}", cancellationToken);
            model.MetaKeywords = desc;
        }

        if (model.CreateMetaTitle)
        {
            var desc = "";
            if (language == "vi")
                desc = await _chatGptService.ChatAsync($"Hãy viết meta title cho sản phẩm {model.Name}", cancellationToken);
            else
                desc = await _chatGptService.ChatAsync($"Write meta title for {model.Name}", cancellationToken);
            model.MetaTitle = desc;
        }

        if (model.CreateMetaDescription)
        {
            var desc = "";
            if (language == "vi")
                desc = await _chatGptService.ChatAsync($"Hãy viết meta description cho sản phẩm {model.Name}", cancellationToken);
            else
                desc = await _chatGptService.ChatAsync($"Write meta description for {model.Name}", cancellationToken);
            model.MetaDescription = desc;
        }


        if (model.SuggestCategory)
        {
            var str = "";
            if (language == "vi")
                str = await _chatGptService.ChatAsync($"Hãy gợi ý danh mục sản phẩm cho {model.Name}", cancellationToken);
            else
                str = await _chatGptService.ChatAsync($"Suggest category for product {model.Name}", cancellationToken);
            var categories = str.Split('/');
            if (categories.Length > 0)
            {
                var cats = await _categoryService.FindAsync(new CategoryQueryModel
                {
                    StoreId = storeId,
                    PageSize = 1000
                });
                var parentId = "";
                var categoryId = "";

                var temp = 0;
                foreach (var category in categories)
                {
                    if (!category.Contains("or"))
                    {
                        var name = category.Trim().Replace(".", "");
                        var catInDb = cats.Items.FirstOrDefault(x => x.Name == name);
                        if (catInDb is null)
                        {
                            var c = await _categoryService.CreateAsync(new CreateCategoryModel
                            {
                                StoreId = storeId,
                                Name = name,
                                Published = true,
                                ParentId = parentId
                            });
                            parentId = c.Id;
                        }
                        else
                        {
                            catInDb.ParentId = parentId;
                            parentId = catInDb.Id;
                        }
                        if (temp == categories.Count() - 1)
                            categoryId = parentId;
                        temp++;
                    }
                }

                if (!string.IsNullOrEmpty(categoryId))
                {
                    model.Categories = new List<string> { categoryId };
                }

            }
        }
        var product = await _productService.CreateAsync(model, cancellationToken);

        if (model.Images.Count > 0)
        {
            var url = "https://time-ai-api.bisfu.com/media";
            List<UploadFileModel> fileModels = new List<UploadFileModel>();
            foreach (var image in model.Images)
            {
                var img = image.ToString().Replace("\"", "");
                HttpClient httpClient = new HttpClient();
                var stream = await httpClient.GetStreamAsync($"{url}/{img}");
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                byte[] byteArray = memoryStream.ToArray();
                fileModels.Add(new UploadFileModel
                {
                    FileName = img,
                    Data = byteArray
                });
            }
            var imgsUploaded = await _storageService.UploadFilesAsync(fileModels);
            var imamesToUpload = new List<ImageModel>();
            foreach (var item in imgsUploaded)
            {
                imamesToUpload.Add(new ImageModel
                {
                    Id = item.Id,
                    Path = item.Path,
                });
            }
            var pros = await _productService.AddImages(product.Id, imamesToUpload);
            return Ok(pros);
        }
        return Ok(product);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProductModel model, CancellationToken cancellationToken)
    {
        var product = await _productService.UpdateAsync(model, cancellationToken);
        return Ok(product);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        => Ok(await _productService.DeleteAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> DeleteMany([FromBody] List<string> ids, CancellationToken cancellationToken)
      => Ok(await _productService.DeleteManyAsync(ids, cancellationToken));


    [HttpPost]
    public async Task<IActionResult> AddImages(string id, [FromBody] List<ImageModel> model, CancellationToken cancellationToken)
    {
        return Ok(await _productService.AddImages(id, model, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteImage(string id, string imageId, CancellationToken cancellationToken)
        => Ok(await _productService.DeleteImage(id, imageId, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> AddAttribute(string id, [FromBody] ProductAddAttributeModel model, CancellationToken cancellationToken)
        => Ok(await _productService.AddAttribute(id, model, cancellationToken));

    [HttpPut]
    public async Task<IActionResult> UpdateAttribute(string id, [FromBody] ProductUpdateAttributeModel model, CancellationToken cancellationToken)
     => Ok(await _productService.UpdateAttribute(id, model, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> DeleteAttribute(string id, string attributeId, CancellationToken cancellationToken)
     => Ok(await _productService.DeleteAttribute(id, attributeId, cancellationToken));

    [HttpGet]
    public IActionResult GetAttributeControlType(CancellationToken cancellationToken)
        => Ok(_productService.GetAttributeControlType(cancellationToken));

    [HttpPost]
    public async Task<IActionResult> AddVariant(string id, [FromBody] ProductAddVariantModel model, CancellationToken cancellationToken)
        => Ok(await _productService.AddVariant(id, model, cancellationToken));

    [HttpPut]
    public async Task<IActionResult> UpdateVariant(string id, [FromBody] ProductUpdateVariantModel model, CancellationToken cancellationToken)
     => Ok(await _productService.UpdateVariant(id, model, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> DeleteVariant(string id, string variantId, CancellationToken cancellationToken)
     => Ok(await _productService.DeleteVariant(id, variantId, cancellationToken));
    
    [HttpPut]
    public async Task<IActionResult> ApplyVariants(string id, [FromBody] IList<ProductAddVariantModel> model, CancellationToken cancellationToken)
        => Ok(await _productService.ApplyVariants(id, model, cancellationToken));
    #endregion
}

public class CreateProductByAIModel : CreateProductModel
{
    public bool CreateShortDescription { get; set; }
    public bool CreateMetaKeywords { get; set; }
    public bool CreateMetaTitle { get; set; }
    public bool CreateMetaDescription { get; set; }
    public bool RemoveBackground { get; set; }
    public bool Resize { get; set; }
    public bool SuggestCategory { get; set; }
    public bool OptimiseDescription { get; set; }
    //public List<string> Images { get; set; }
}
