using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Text.Json;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Enums.Cms;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;
using Time.Commerce.Proxy.Cms;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Identity
{
    public class StoreService : IStoreService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStoreRepository _storeRepository;
        private readonly IWorkContext _workContext;
        private readonly IStorageProxy _storageProxy;
        private readonly IBrandService _storeService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IWidgetService _widgetService;
        private readonly ISpeakerService _speakerService;
        private readonly IBlogPostService _blogPostService;
        #endregion

        #region Ctor
        public StoreService(IMapper mapper, IStoreRepository storeRepository, IWorkContext workContext, IStorageProxy storageProxy, IBrandService storeService, ICategoryService categoryService, IProductService productService, IWidgetService widgetService, ISpeakerService speakerService, IBlogPostService blogPostService)
        {
            _mapper = mapper;
            _storeRepository = storeRepository;
            _workContext = workContext;
            _storageProxy = storageProxy;
            _storeService = storeService;
            _categoryService = categoryService;
            _productService = productService;
            _widgetService = widgetService;
            _speakerService = speakerService;
            _blogPostService = blogPostService; 
        }
        #endregion
        public async Task<StoreView> CreateAsync(CreateStoreModel model, CancellationToken cancellationToken = default)
        {
            var store = _mapper.Map<Store>(model);
            store.StoreId = await GetSeName(store);
            store.CreatedBy = _workContext.GetCurrentUserId();
            store.CreatedOn = DateTime.UtcNow;
            var storeCreated = await _storeRepository.InsertAsync(store);
            return _mapper.Map<StoreView>(storeCreated);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => await _storeRepository.DeleteAsync(id);

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
            => await _storeRepository.DeleteManyAsync(ids);

        public async Task<StoreView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default)
        {
            var store = await GetByIdAsync(id);
            store.Logo = _mapper.Map<Image>(model);
            return _mapper.Map<StoreView>(await _storeRepository.UpdateAsync(store));
        }

        public async Task<StoreView> DeleteImage(string id, CancellationToken cancellationToken = default)
        {
            var store = await GetByIdAsync(id);
            store.Logo = null;
            return _mapper.Map<StoreView>(await _storeRepository.UpdateAsync(store));
        }

        public async Task<bool> CheckStoreExistsAsync(string store, CancellationToken cancellationToken = default)
            => await _storeRepository.AnyAsync(x => x.StoreId == store);

        public async Task<PageableView<StoreView>> FindAsync(StoreQueryModel model, CancellationToken cancellationToken = default)
        {
            Expression<Func<Store, bool>> filter = null;
            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Name.ToLower().Contains(searchText) || x.Url.ToLower().Contains(searchText)
               || x.StoreId.ToLower().Contains(searchText);

            var query = new DataFilterPaging<Store> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            var stores = await _storeRepository.CountAndQueryAsync(query);
            return new PageableView<StoreView>
                (
                    model.PageIndex,
                    model.PageSize,
                    stores.Total,
                    _mapper.Map<IEnumerable<StoreView>>(stores.Data).ToList()
                );
        }

        public async Task<StoreView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<StoreView>(await GetByIdAsync(id));

        public async Task<StoreView> GetByStoreIdAsync(string storeId, CancellationToken cancellationToken = default)
          => _mapper.Map<StoreView>(await _storeRepository.FindOneAsync(x => x.StoreId == storeId));

        public async Task<StoreDetailsView> GetStoreDetailsAsync(string? storeId, CancellationToken cancellationToken = default)
        {
            StoreDetailsView result = new StoreDetailsView();
            if (string.IsNullOrEmpty(storeId))
            {
                var defaultStore = await _storeRepository.FindOneAsync(x => x.DefaultStore);
                result = _mapper.Map<StoreDetailsView>(defaultStore);
            }
            else
            {
                var store = await GetByIdAsync(storeId);
                result = _mapper.Map<StoreDetailsView>(store);
            }
            var widgets = await _widgetService.FindAsync(new WidgetQueryModel { PageSize = 20, StoreId = result?.Id });
            if (widgets.Items.Any())
            {
                result.Widgets ??= new List<WidgetView>();
                result.Widgets = _mapper.Map<List<WidgetView>>(widgets.Items);
                var menus = result.Widgets.FirstOrDefault(x => x.Type == nameof(WidgetTypes.HeaderMenus));

                //check menus
                var nodeList = new List<MenuView>();
                var list = new List<IMenuNode>();
                var widgetMenus = menus.WidgetMenus.Select(x =>
                {
                    x.Id = x.MenuId;
                    return x;
                }).OrderBy(i => i.DisplayOrder);
                list.AddRange(widgetMenus);
                foreach (var node in list)
                {
                    if (string.IsNullOrEmpty(node.ParentId))
                    {
                        var newNode = new MenuView
                        {
                            Id = node.Id,
                            Title = node.Title,
                            Link = node.Link,
                            Children = new List<MenuView>()
                        };
                        FillChildNodes(newNode, list);
                        nodeList.Add(newNode);
                    }
                }
                result.HeaderMenus = nodeList;
            }
            return result;
        }

        public async Task<DataFilterPagingResult<Store>> GetListAsync(DataFilterPaging<Store> model, CancellationToken cancellationToken = default)
        {
            return await _storeRepository.CountAndQueryAsync(model);
        }

        public async Task<StoreView> UpdateAsync(UpdateStoreModel model, CancellationToken cancellationToken = default)
        {
            var store = await GetByIdAsync(model.Id);
            store.Name = model.Name;
            store.StoreId = await GetSeName(store);
            if (store.StoreId != model.Name)
                store.StoreId = await GetSeName(store);
            store.Url = model.Url;
            store.DefaultCurrency = model.DefaultCurrency;
            store.CompanyName = model.CompanyName;
            store.CompanyAddress = model.CompanyAddress;
            store.CompanyPhoneNumber = model.CompanyPhoneNumber;
            store.CompanyVat = model.CompanyVat;
            store.Description = model.Description;
            store.MetaTitle = model.MetaTitle;
            store.MetaDescription = model.MetaDescription;
            store.MetaKeywords = model.MetaKeywords;
            store.DefaultStore = model.DefaultStore;
            store.BankAccount = _mapper.Map<BankAccount>(model.BankAccount);

            store.UpdatedOn = DateTime.UtcNow;
            store.UpdatedBy = _workContext.GetCurrentUserId();
            var userUpdated = await _storeRepository.UpdateAsync(store);
            return _mapper.Map<StoreView>(userUpdated);
        }

        public async Task<bool> InstallStoreAsync(RegisterStoreModel model, CancellationToken cancellationToken = default)
        {
            return true;
        }
        public async Task<bool> InstallThemeAsync(string key, CancellationToken cancellationToken = default)
        {
            if (key != "P123$567")
                throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);
            var speakerIds = await InstallSpeakersSampleData(cancellationToken);
            await InstallBlogsSampleData(cancellationToken);
            await InstallProductsSampleData(speakerIds, cancellationToken);
            return true;
        }

        #region Private Methods
        private async Task<Store> GetByIdAsync(string id)
            => await _storeRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        protected void FillChildNodes(MenuView parentNode, List<IMenuNode> nodes)
        {
            var children = nodes.Where(x => x.ParentId == parentNode.Id);
            foreach (var child in children)
            {
                var newNode = new MenuView
                {
                    Id = child.Id,
                    Title = child.Title,
                    Link = child.Link,
                    Children = new List<MenuView>()
                };

                FillChildNodes(newNode, nodes);

                parentNode.Children.Add(newNode);
            }
        }

        private async Task<string> GetSeName(Store store)
        {
            if (!string.IsNullOrEmpty(store.StoreId) && !await _storeRepository.AnyAsync(x => x.StoreId == store.StoreId))
                return store.StoreId;

            store.StoreId = SlugHelper.GenerateSlug(store.Name);

            int i = 2;
            var tempSeName = store.StoreId;
            while (true)
            {
                if (await _storeRepository.AnyAsync(x => x.StoreId == tempSeName))
                {
                    tempSeName = string.Format("{0}_{1}", store.StoreId, i);
                    i++;
                }
                else
                {
                    break;
                }

            }
            return tempSeName;
        }

        private async Task<List<string>> InstallSpeakersSampleData(CancellationToken cancellationToken = default)
        {
            List<string> guids = new List<string>();
            var speakers = InstallTheme.Speakers;
            int flag = 1;
            foreach (var speaker in speakers)
            {
                const string streamFile = "streamFile";
                var img = StorageHelper.GetFilePath($"themesInstall/images/speakers/{flag}.jpg");
                using var stream = new MemoryStream(File.ReadAllBytes(img).ToArray());
                var imgFormFile = new FormFile(stream, 0, stream.Length, streamFile, Path.GetFileName(img));
                var imgUploaded = await _storageProxy.UploadFileAsync(imgFormFile, $"content");
                speaker.Image = new ImageModel
                {
                    Id = imgUploaded.Id,
                    Path = imgUploaded.Path,
                    AlternateText = speaker.Name,
                    Title = speaker.Name,
                };
                var s = await _speakerService.CreateAsync(speaker, cancellationToken);
                guids.Add(s.Id);
                flag++;
            }
            return guids;
        }

        private async Task<bool> InstallBlogsSampleData(CancellationToken cancellationToken = default)
        {
            var path = Path.Combine(Path.GetDirectoryName(typeof(StoreService).Assembly.Location), @"Data/createSampleStore.json");
            string text = File.ReadAllText(path);
            var installSampleData = JsonSerializer.Deserialize<InstallSampleDataModel>(text);
            var stores = installSampleData.Brands;
            var blogs = InstallTheme.Blogs;
            int flag = 1;
            foreach (var blog in blogs)
            {
                const string streamFile = "streamFile";
                var img = StorageHelper.GetFilePath($"themesInstall/images/blogs/blog_{flag}.jpg");
                using var stream = new MemoryStream(File.ReadAllBytes(img).ToArray());
                var imgFormFile = new FormFile(stream, 0, stream.Length, streamFile, Path.GetFileName(img));
                var imgUploaded = await _storageProxy.UploadFileAsync(imgFormFile, $"content");
                blog.Image = new ImageModel
                {
                    Id = imgUploaded.Id,
                    Path = imgUploaded.Path,
                    AlternateText = blog.Title,
                    Title = blog.Title,
                };
                await _blogPostService.CreateAsync(blog);
                flag++;
            }
            return true;
        }

        private async Task<bool> InstallProductsSampleData(List<string> speakerIds, CancellationToken cancellationToken = default)
        {
            var path = Path.Combine(Path.GetDirectoryName(typeof(StoreService).Assembly.Location), @"Data/createSampleStore.json");
            string text = File.ReadAllText(path);
            var installSampleData = JsonSerializer.Deserialize<InstallSampleDataModel>(text);
            var stores = installSampleData.Brands;
            var products = InstallTheme.Products;
            int flag = 1;
            foreach (var product in products)
            {
                const string streamFile = "streamFile";
                var img = StorageHelper.GetFilePath($"themesInstall/images/products/i.jpg");
                using var stream = new MemoryStream(File.ReadAllBytes(img).ToArray());
                var imgFormFile = new FormFile(stream, 0, stream.Length, streamFile, Path.GetFileName(img));
                var imgUploaded = await _storageProxy.UploadFileAsync(imgFormFile, $"content");
                product.Images = new List<ImageModel>
                {
                    new ImageModel
                    {
                        Id = imgUploaded.Id,
                        Path = imgUploaded.Path,
                        AlternateText = product.Name,
                        Title = product.Name,
                    }
                } ;
                product.SpeakerId = speakerIds[flag];
                await _productService.CreateAsync(product);
                flag++;
            }
            return true;
        }
        #endregion
    }
}

public static class InstallTheme
{
    public static string SpeakerShortInformation = $"""
        Anna assumed the role of Chief Product Officer in July 2017 and leads the product team, which designs, builds and optimizes the Netflix experience. Previously, Anna was International Development Officer for Netflix, responsible for the global partnerships with consumer electronics.
        Prior to joining Netflix in 2008, Anna was senior vice president of consumer electronics products for Macrovision Solutions Corp. (later renamed to Rovi Corporation) and previously held positions at digital entertainment software provider, Mediabolic Inc., Red Hat Network, the provider of Linux and Open Source technology, and online vendor Wine.
        """;
    public static List<GenericAttributeModel> GenericAttributes = new List<GenericAttributeModel>
    {
        new GenericAttributeModel
        {
            Key = "Speciality",
            Value = "Product Designer"
        },
        new GenericAttributeModel
        {
            Key = "Company",
            Value = "Netflix"
        },
        new GenericAttributeModel
        {
            Key = "Experience",
            Value = "10 Years"
        },
        new GenericAttributeModel
        {
            Key = "Email",
            Value = "khoanguyen@mail.com"
        },
        new GenericAttributeModel
        {
            Key = "Phone",
            Value = "700.123.456.123"
        },
        new GenericAttributeModel
        {
            Key = "Fax",
            Value = "700.123.456.123"
        }
    };

    public static List<CreateSpeakerModel> Speakers = new List<CreateSpeakerModel>()
    {
        new CreateSpeakerModel
        {
            Name = "Yasir Schwartz",
            Description = "Co-Founder Facing",
            ShortInformation = SpeakerShortInformation,
            GenericAttributes = GenericAttributes
        },
        new CreateSpeakerModel
        {
            Name = "Elisha Garza",
            Description = "Co-Founder Alibaba",
            ShortInformation = SpeakerShortInformation,
            GenericAttributes = GenericAttributes
        },
        new CreateSpeakerModel
        {
            Name = "Sameer Hale",
            Description = "Finance Manager",
            ShortInformation = SpeakerShortInformation,
            GenericAttributes = GenericAttributes
        },
        new CreateSpeakerModel
        {
            Name = "Yasir Schwartz",
            Description = "Designer",
            ShortInformation = SpeakerShortInformation,
            GenericAttributes = GenericAttributes
        },
        new CreateSpeakerModel
        {
            Name = "Pedro Huard",
            Description = "Web Designer",
            ShortInformation = SpeakerShortInformation,
            GenericAttributes = GenericAttributes
        },
        new CreateSpeakerModel
        {
            Name = "Yasir Schwartz",
            Description = "Co-Founder Facing",
            ShortInformation = SpeakerShortInformation,
            GenericAttributes = GenericAttributes
        },
        new CreateSpeakerModel
        {
            Name = "Edgar Torrey",
            Description = "Team Leader",
            ShortInformation = SpeakerShortInformation,
            GenericAttributes = GenericAttributes
        },
        new CreateSpeakerModel
        {
            Name = "Thad Eddings",
            Description = "Project Manager",
            ShortInformation = SpeakerShortInformation,
            GenericAttributes = GenericAttributes
        },
        new CreateSpeakerModel
        {
            Name = "Yasir Schwartz",
            Description = "Co-Founder Facing",
            ShortInformation = SpeakerShortInformation,
            GenericAttributes = GenericAttributes
        },
    };

    public static string ShortContent = $"""
        There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour.
        randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet.
        """;
    public static List<CreateBlogPostModel> Blogs = new List<CreateBlogPostModel>()
    {
        new CreateBlogPostModel
        {
            Title = "International Conference On Art Business",
            FullContent = "",
            ShortContent = ShortContent,
            Tags = new List<string>() { "MongoDB", "React js", "Next js", "Node js", "Asp net core", "Event", "Web", "Platform" },
            Published = true
        },
         new CreateBlogPostModel
        {
            Title = "International Conference On Art Business",
            FullContent = "",
            ShortContent = ShortContent,
            Tags = new List<string>() { "MongoDB", "React js", "Next js", "Node js", "Asp net core", "Event", "Web", "Platform" },
            Published = true
        },
          new CreateBlogPostModel
        {
            Title = "International Conference On Art Business",
            FullContent = "",
            ShortContent = ShortContent,
            Tags = new List<string>() { "MongoDB", "React js", "Next js", "Node js", "Asp net core", "Event", "Web", "Platform" },
            Published = true
        },
           new CreateBlogPostModel
        {
            Title = "International Conference On Art Business",
            FullContent = "",
            ShortContent = ShortContent,
            Tags = new List<string>() { "MongoDB", "React js", "Next js", "Node js", "Asp net core", "Event", "Web", "Platform" },
            Published = true
        }
    };
    public static string ShortDescription = $"""
        We’re inviting the top creatives in the tech industry from all over the world to come learn, grow, scrape their knees, try new things, to be vulnerable, and to have epic adventures
        """;
    public static string Description = $"""
        <div><p style="border: none; margin-right: 0px; margin-bottom: var(--margin-bottom-30); margin-left: 0px; outline: none; padding: 0px; color: var(--color-five); font-size: var(--font-16); line-height: 1.7em; position: relative; font-family: Manrope, sans-serif;">Dolor sit amet consectetur elit sed do eiusmod tempor incd idunt labore et dolore magna aliqua enim ad minim veniam quis nostrud exercitation ullamco laboris nisi ut aliquip exea commodo consequat.duis aute irure dolor in repre hen derit in voluptate velit esse cillum dolore eu fugiat nulla pariatur cepteur sint occaecat cupidatat eaque ipsa quae illo proident sunt in culpa qui officia deserunt mollit anim id est laborum perspiciatis unde omnis iste natus error sit voluptatem accusantium dolore laudant rem aperiam eaque ipsa quae ab illo inventore veritatis quasi architecto.</p><p style="border: none; margin-right: 0px; margin-bottom: var(--margin-bottom-30); margin-left: 0px; outline: none; padding: 0px; color: var(--color-five); font-size: var(--font-16); line-height: 1.7em; position: relative; font-family: Manrope, sans-serif;">Dolor sit amet consectetur elit sed do eiusmod tempor incd idunt labore et dolore magna aliqua enim ad minim veniam quis nostrud exercitation ullamco laboris nisi ut aliquip exea commodo consequat.duis aute irure dolor in reprehen derit in voluptate velit esse cillum dolore eu fugiat nulla pariatur cepteur sint occaecat cupidatat.</p><div class="middle-column" style="border: none; margin-top: var(--margin-top-30); margin-right: 0px; margin-bottom: var(--margin-bottom-30); margin-left: 0px; outline: none; padding: 0px; position: relative; color: rgb(0, 0, 0); font-family: Manrope, sans-serif;"><div class="row clearfix" style="border: none; margin-top: calc(var(--bs-gutter-y)*-1); margin-right: calc(var(--bs-gutter-x)*-.5); margin-bottom: 0px; margin-left: calc(var(--bs-gutter-x)*-.5); outline: none; padding: 0px; --bs-gutter-x: 1.5rem; --bs-gutter-y: 0;"><div class="column col-lg-6 col-md-12 col-sm-12" style="border: none; margin-top: var(--bs-gutter-y); margin-right: 0px; margin-bottom: 0px; margin-left: 0px; outline: none; padding-top: 0px; padding-right: calc(var(--bs-gutter-x)*.5); padding-bottom: 0px; padding-left: calc(var(--bs-gutter-x)*.5); flex-basis: auto; max-width: 100%; width: 497px;"><h3 class="event-detail_subtitle" style="border: none; margin-right: 0px; margin-bottom: var(--margin-bottom-25); margin-left: 0px; outline: none; padding: 0px; font-weight: 700; line-height: 1.3em; font-size: var(--font-30); background: none; color: var(--main-color-two); font-family: var(--font-family-Manrope); position: relative;">Evolution of user Interface</h3><ul class="event-detail_list" style="border: none; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; outline: none; padding: 0px; list-style: none; position: relative;"><li style="border: none; margin-top: 0px; margin-right: 0px; margin-bottom: var(--margin-bottom-15); margin-left: 0px; outline: none; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: var(--padding-left-30); list-style: none; color: var(--color-five); font-size: var(--font-16); position: relative;">Multiple Announcements during the event.</li><li style="border: none; margin-top: 0px; margin-right: 0px; margin-bottom: var(--margin-bottom-15); margin-left: 0px; outline: none; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: var(--padding-left-30); list-style: none; color: var(--color-five); font-size: var(--font-16); position: relative;">Logo &amp; company details on the WordCamp Kolkata.</li><li style="border: none; margin-top: 0px; margin-right: 0px; margin-bottom: var(--margin-bottom-15); margin-left: 0px; outline: none; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: var(--padding-left-30); list-style: none; color: var(--color-five); font-size: var(--font-16); position: relative;">Dedicated blog post thanking each of our Gold.</li><li style="border: none; margin-top: 0px; margin-right: 0px; margin-bottom: var(--margin-bottom-15); margin-left: 0px; outline: none; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: var(--padding-left-30); list-style: none; color: var(--color-five); font-size: var(--font-16); position: relative;">Acknowledgment and thanks in opening and closing.</li></ul></div><div class="column col-lg-6 col-md-6 col-sm-12" style="border: none; margin-top: var(--bs-gutter-y); margin-right: 0px; margin-bottom: 0px; margin-left: 0px; outline: none; padding-top: 0px; padding-right: calc(var(--bs-gutter-x)*.5); padding-bottom: 0px; padding-left: calc(var(--bs-gutter-x)*.5); flex-basis: auto; max-width: 100%; width: 497px;"><div class="event-detail_image-two" style="border: none; margin: 0px; outline: none; padding: 0px; position: relative;"><img src="https://conat-react.netlify.app/event-details/assets/images/resource/event-1.jpg" alt="" style="border-width: initial; border-color: initial; border-image: initial; margin: 0px; outline: none; padding: 0px; display: block; height: auto; position: relative; width: 473px;"></div></div></div></div><p style="border: none; margin-right: 0px; margin-bottom: var(--margin-bottom-30); margin-left: 0px; outline: none; padding: 0px; color: var(--color-five); font-size: var(--font-16); line-height: 1.7em; position: relative; font-family: Manrope, sans-serif;">Dolor sit amet consectetur elit sed do eiusmod tempor incd idunt labore et dolore magna aliqua enim ad minim veniam quis nostrud exercitation ullamco laboris nisi ut aliquip exea commodo consequat.duis aute irure dolor in repre hen derit in voluptate velit esse cillum dolore eu fugiat nulla pariatur cepteur sint occaecat cupidatat eaque ipsa quae illo proident sunt in culpa qui officia deserunt mollit anim id est laborum perspiciatis unde omnis iste natus error sit voluptatem accusantium dolore laudant rem aperiam eaque ipsa quae ab illo inventore veritatis quasi architecto.</p></div>
        """;
    public static List<CreateProductModel> Products = new List<CreateProductModel>()
    {
        new CreateProductModel
        {
            Name = "Registration For Opening Workshop",
            Description = Description,
            ShortDescription = ShortDescription,
            Published = true,
            Location = "Hall 1, Building, Golden Street , Southafrica"
        },
         new CreateProductModel
        {
            Name = "Registration For Opening Workshop",
            Description = Description,
            ShortDescription = ShortDescription,
            Published = true,
            Location = "Hall 1, Building, Golden Street , Southafrica"
        },
          new CreateProductModel
        {
            Name = "Registration For Opening Workshop",
            Description = Description,
            ShortDescription = ShortDescription,
            Published = true,
            Location = "Hall 1, Building, Golden Street , Southafrica"
        },
           new CreateProductModel
        {
            Name = "Registration For Opening Workshop",
            Description = Description,
            ShortDescription = ShortDescription,
            Published = true,
            Location = "Hall 1, Building, Golden Street , Southafrica"
        }
    };
}
