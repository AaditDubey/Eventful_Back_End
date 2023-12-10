using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Cms;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Repositories.Cms;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Cms
{
    public class ContentService : IContentService
    {
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IContentRepository _contentRepository;

        public ContentService(IMapper mapper, IWorkContext workContext, IContentRepository contentRepository)
        {
            _mapper = mapper;
            _workContext = workContext;
            _contentRepository = contentRepository;
        }
        public async Task<ContentView> CreateAsync(CreateContentModel model, CancellationToken cancellationToken = default)
        {
            var content = _mapper.Map<Content>(model);
            content.SeName = await GetSeName(content);
            content.Stores.Add(model.StoreId);
            content.CreatedBy = _workContext.GetCurrentUserId();
            content.CreatedOn = DateTime.UtcNow;
            var contentCreated = await _contentRepository.InsertAsync(content);
            return _mapper.Map<ContentView>(contentCreated);
        }

        public async Task<ContentView> UpdateAsync(UpdateContentModel model, CancellationToken cancellationToken = default)
        {
            var content = await GetByIdAsync(model.Id);

            if (content.Title != model.Title)
                content.SeName = await GetSeName(content);

            content.Title = model.Title;
            content.ShortContent = model.ShortContent;
            content.FullContent = model.FullContent;
            content.Type = model.Type;
            content.Tags = model.Tags;
            content.Publisher = model.Publisher;
            content.Published = model.Published;
            content.MetaKeywords = model.MetaKeywords;
            content.MetaDescription = model.MetaDescription;
            content.MetaTitle = model.MetaTitle;
            content.UpdatedOn = DateTime.UtcNow;
            content.UpdatedBy = _workContext.GetCurrentUserId();
            var contentUpdated = await _contentRepository.UpdateAsync(content);
            return _mapper.Map<ContentView>(contentUpdated);
        }


        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _contentRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            return await _contentRepository.DeleteManyAsync(ids);
        }

        public async Task<PageableView<ContentView>> FindAsync(ContentQueryModel model, CancellationToken cancellationToken = default)
        {
            var contents = await GetAllAsync(model);
            return new PageableView<ContentView>
                (
                    model.PageIndex,
                    model.PageSize,
                    contents.Total,
                    _mapper.Map<IEnumerable<ContentView>>(contents.Data).ToList()
                );
        }

        public async Task<ContentView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<ContentView>(await GetByIdAsync(id));

        public async Task<ContentView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
            => _mapper.Map<ContentView>(await _contentRepository.FindOneAsync(x => x.SeName == slug));

        public async Task<ContentView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default)
        {
            var content = await GetByIdAsync(id);
            content.Image = _mapper.Map<Image>(model);
            return _mapper.Map<ContentView>(await _contentRepository.UpdateAsync(content));
        }

        public async Task<ContentView> DeleteImage(string id, CancellationToken cancellationToken = default)
        {
            var content = await GetByIdAsync(id);
            content.Image = null;
            return _mapper.Map<ContentView>(await _contentRepository.UpdateAsync(content));
        }

        #region Private Methods
        private async Task<DataFilterPagingResult<Content>> GetAllAsync(ContentQueryModel model)
        {
            Expression<Func<Content, bool>> filter = null;
            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Title.ToLower().Contains(searchText) || x.ShortContent.ToLower().Contains(searchText) ||
                x.MetaDescription.ToLower().Contains(searchText) ||
                x.MetaKeywords.ToLower().Contains(searchText) ||
                x.MetaTitle.ToLower().Contains(searchText);

            if (!string.IsNullOrWhiteSpace(model?.StoreId))
                filter = filter.And(x => x.Stores.Contains(model.StoreId));

            if (model.Published.HasValue)
                filter = filter.And(x => x.Published == model.Published);

            if(!string.IsNullOrWhiteSpace(model.Type))
                filter = filter.And(x => x.Type.ToLower() == model.Type.ToLower());

            if (!string.IsNullOrWhiteSpace(model.Tag))
                filter = filter.And(x => x.Tags.Any(x => x.ToLower().Contains(model.Tag.ToLower()) ));

            var query = new DataFilterPaging<Content> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            return await _contentRepository.CountAndQueryAsync(query);
        }
        private async Task<Content> GetByIdAsync(string id)
            => await _contentRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        private async Task<string> GetSeName(Content content)
        {
            if (!string.IsNullOrEmpty(content.SeName) && !await _contentRepository.AnyAsync(x => x.SeName == content.SeName))
                return content.SeName;

            content.SeName = SlugHelper.GenerateSlug(content.Title);

            int i = 2;
            var tempSeName = content.SeName;
            while (true)
            {
                if (await _contentRepository.AnyAsync(x => x.SeName == tempSeName))
                {
                    tempSeName = string.Format("{0}-{1}", content.SeName, i);
                    i++;
                }
                else
                {
                    break;
                }

            }
            return tempSeName;
        }
        #endregion
    }
}
