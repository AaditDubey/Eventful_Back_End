using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Catalog;
using Time.Commerce.Application.Services.Common;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Repositories.Catalog;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Catalog
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDistributedCacheService _distributedCacheService;
        private const string _cacheNoStoreKey = "Category_GetAllWithRecursive";
        public CategoryService(IMapper mapper, IWorkContext workContext, ICategoryRepository categoryRepository, IDistributedCacheService distributedCacheService)
        {
            _mapper = mapper;
            _workContext = workContext;
            _categoryRepository = categoryRepository;
            _distributedCacheService = distributedCacheService;
        }
        public async Task<CategoryView> CreateAsync(CreateCategoryModel model, CancellationToken cancellationToken = default)
        {
            var category = _mapper.Map<Category>(model);
            category.SeName = await GetSeName(category);
            category.Stores.Add(model.StoreId);
            category.CreatedBy = _workContext.GetCurrentUserId();
            category.CreatedOn = DateTime.UtcNow;
            var categoryCreated = await _categoryRepository.InsertAsync(category);
            await DeleteAllCacheAsync(model.StoreId);
            return _mapper.Map<CategoryView>(categoryCreated);
        }

        public async Task<CategoryView> UpdateAsync(UpdateCategoryModel model, CancellationToken cancellationToken = default)
        {
            var category = await GetByIdAsync(model.Id);

            if (category.Name != model.Name)
                category.SeName = await GetSeName(category);

            category.ParentId = model.ParentId;
            category.Name = model.Name;
            category.Description = model.Description;
            category.Icon = model.Icon;
            category.PriceRanges = model.PriceRanges;
            category.ShowOnHomePage = model.ShowOnHomePage;
            category.FeaturedProductsOnHomePage = model.FeaturedProductsOnHomePage;
            category.ShowOnSearchBox = model.ShowOnSearchBox;
            category.SearchBoxDisplayOrder = model.SearchBoxDisplayOrder;
            category.IncludeInTopMenu = model.IncludeInTopMenu;
            category.SubjectToAcl = model.SubjectToAcl;
            category.LimitedToStores = model.LimitedToStores;
            category.Published = model.Published;
            category.DisplayOrder = model.DisplayOrder;
            category.MetaKeywords = model.MetaKeywords;
            category.MetaDescription = model.MetaDescription;
            category.MetaTitle = model.MetaTitle;
            category.UpdatedOn = DateTime.UtcNow;
            category.UpdatedBy = _workContext.GetCurrentUserId();
            var categoryUpdated = await _categoryRepository.UpdateAsync(category);
            foreach (var store in category.Stores)
                await DeleteAllCacheAsync(store);
            return _mapper.Map<CategoryView>(categoryUpdated);
        }


        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var cat = await GetByIdAsync(id);
            foreach(var store in cat.Stores)
                await DeleteAllCacheAsync(store);
            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            var cat = await GetByIdAsync(ids[0]);
            foreach (var store in cat.Stores)
                await DeleteAllCacheAsync(store);
            return await _categoryRepository.DeleteManyAsync(ids);
        }

        public async Task<PageableView<CategoryView>> FindAsync(CategoryQueryModel model, CancellationToken cancellationToken = default)
        {
            var categories = await GetAllAsync(model);
            return new PageableView<CategoryView>
                (
                    model.PageIndex,
                    model.PageSize,
                    categories.Total,
                    _mapper.Map<IEnumerable<CategoryView>>(categories.Data).ToList()
                );
        }

        public async Task<IEnumerable<CategoryView>> GetAllWithParentAsync(CategoryQueryModel model, CancellationToken cancellationToken = default)
        {
            return await _distributedCacheService.GetOrCreateAsync<IEnumerable<CategoryView>>(GetCacheKey(CategoryCacheEnum.GetAllWithParent, model.StoreId), async () =>
            {
                var allCategories = await GetAllAsync(model);
                var categories = _mapper.Map<IEnumerable<CategoryView>>(allCategories.Data);
                var categoriesList = new List<CategoryView>();
                foreach (var category in categories)
                {
                    category.LevelName = category.Name;
                    if (categories.Where(x => x.Id == category.ParentId).Count() > 0)
                    {
                        var parentCategory = categories.Where(x => x.Id == category.ParentId).ToList()[0];
                        while (parentCategory != null)
                        {
                            category.LevelName = $"{parentCategory.Name} >> {category.LevelName}";
                            if (categories.Where(x => x.Id == parentCategory.ParentId).Count() > 0)
                            {
                                parentCategory = categories.Where(x => x.Id == parentCategory.ParentId).ToList()[0];
                            }
                            else
                            {
                                parentCategory = null;
                            }
                        }
                    }
                    categoriesList.Add(category);
                }
                return categoriesList;
            });
        }

        public async Task<IEnumerable<CategoryRecursiveView>> GetAllWithRecursive(CategoryQueryModel model, CancellationToken cancellationToken = default)
        {
            var key = model.StoreId;
            if (string.IsNullOrEmpty(key))
                key = _cacheNoStoreKey;
            return await _distributedCacheService.GetOrCreateAsync<IEnumerable<CategoryRecursiveView>>(GetCacheKey(CategoryCacheEnum.GetAllWithRecursive, key), async () =>
            {
                var allCategories = await GetAllAsync(model);
                var categories = _mapper.Map<IEnumerable<CategoryView>>(allCategories.Data);
                categories = categories.Select(c =>
                {
                    if(!string.IsNullOrEmpty(c.Image?.Path))
                        c.ImageUrl = c.Image?.Path;
                    return c;
                });
                var categoriesList = new List<CategoryView>();
                var nodeList = new List<CategoryRecursiveView>();
                var list = new List<ICategoryNode>();
                list.AddRange(categories);
                foreach (var node in list)
                {
                    if (string.IsNullOrEmpty(node.ParentId))
                    {
                        var newNode = new CategoryRecursiveView
                        {
                            Id = node.Id,
                            Name = node.Name,
                            SeName = node.SeName,
                            ImageUrl = node.ImageUrl,
                            Children = new List<CategoryRecursiveView>()
                        };
                        FillChildNodes(newNode, list);
                        nodeList.Add(newNode);
                    }
                }
                return nodeList;
            });
        }

        public async Task<CategoryView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<CategoryView>(await GetByIdAsync(id));
        public async Task<CategoryView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => _mapper.Map<CategoryView>(await _categoryRepository.FindOneAsync(x => x.SeName == slug));

        public async Task<CategoryView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default)
        {
            var category = await GetByIdAsync(id);
            category.Image = _mapper.Map<Image>(model);
            return _mapper.Map<CategoryView>(await _categoryRepository.UpdateAsync(category));
        }

        public async Task<CategoryView> DeleteImage(string id, CancellationToken cancellationToken = default)
        {
            var category = await GetByIdAsync(id);
            category.Image = null;
            return _mapper.Map<CategoryView>(await _categoryRepository.UpdateAsync(category));
        }
        public async Task<bool> BulkUpdateAsync(IEnumerable<Category> categories, IEnumerable<string> updateFields,
          CancellationToken cancellationToken = default)
        {
            return await _categoryRepository.BulkUpdateAsync(categories, updateFields);
        }
        public async Task<bool> BulkInsertAsync(IEnumerable<Category> categories,
          CancellationToken cancellationToken = default)
        {
            await _categoryRepository.InsertListAsync(categories);
            return true;
        }
        #region Private Methods
        private async Task<DataFilterPagingResult<Category>> GetAllAsync(CategoryQueryModel model)
        {
            Expression<Func<Category, bool>> filter = null;
            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Name.ToLower().Contains(searchText) ||
                x.MetaDescription.ToLower().Contains(searchText) ||
                x.MetaKeywords.ToLower().Contains(searchText) ||
                x.MetaTitle.ToLower().Contains(searchText);

            if (!string.IsNullOrWhiteSpace(model?.StoreId))
                filter = filter.And(x => x.Stores.Contains(model.StoreId));

            if (model.Published.HasValue)
                filter = filter.And(x => x.Published == model.Published);


            if (model.ShowOnHomePage.HasValue)
                filter = filter.And(x => x.ShowOnHomePage == model.ShowOnHomePage);

            if (model.Ids is not null && model.Ids.Any())
                filter = filter.And(x => model.Ids.Contains(x.Id));

            var query = new DataFilterPaging<Category> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };
            else
                query.Sort = new List<SortOption> { new SortOption { Field = nameof(Category.CreatedOn), Ascending = false } };

            return await _categoryRepository.CountAndQueryAsync(query);
        }
        private async Task<Category> GetByIdAsync(string id)
            => await _categoryRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        private async Task<string> GetSeName(Category category)
        {
            if (!string.IsNullOrEmpty(category.SeName) && !await _categoryRepository.AnyAsync(x => x.SeName == category.SeName))
                return category.SeName;

            category.SeName = SlugHelper.GenerateSlug(category.Name);

            int i = 2;
            var tempSeName = category.SeName;
            while (true)
            {
                if (await _categoryRepository.AnyAsync(x => x.SeName == tempSeName))
                {
                    tempSeName = string.Format("{0}-{1}", category.SeName, i);
                    i++;
                }
                else
                {
                    break;
                }

            }
            return tempSeName;
        }

        private async Task DeleteAllCacheAsync(string storeId)
        {
            await _distributedCacheService.RemoveAsync(GetCacheKey(CategoryCacheEnum.GetAllWithParent, storeId));
            await _distributedCacheService.RemoveAsync(GetCacheKey(CategoryCacheEnum.GetAllWithRecursive, storeId));
            await _distributedCacheService.RemoveAsync(GetCacheKey(CategoryCacheEnum.GetAllWithRecursive, _cacheNoStoreKey));
        }

        protected void FillChildNodes(CategoryRecursiveView parentNode, List<ICategoryNode> nodes)
        {
            var children = nodes.Where(x => x.ParentId == parentNode.Id);
            foreach (var child in children)
            {
                var newNode = new CategoryRecursiveView
                {
                    Id = child.Id,
                    Name = child.Name,
                    SeName = child.SeName,
                    Children = new List<CategoryRecursiveView>()
                };

                FillChildNodes(newNode, nodes);

                parentNode.Children.Add(newNode);
            }
        }

        private string GetCacheKey(CategoryCacheEnum type, string storeId)
            => type == CategoryCacheEnum.GetAllWithParent ? $"timecommerce.category.getallwithparent.{storeId}" : $"timecommerce.category.getallwithrecursive.{storeId}";
        #endregion

        private enum CategoryCacheEnum
        {
            GetAllWithParent,
            GetAllWithRecursive
        }
    }
}
