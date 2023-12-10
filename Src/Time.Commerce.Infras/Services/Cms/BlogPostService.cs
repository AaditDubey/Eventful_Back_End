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
    public class BlogPostService : IBlogPostService
    {
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostService(IMapper mapper, IWorkContext workContext, IBlogPostRepository blogPostRepository)
        {
            _mapper = mapper;
            _workContext = workContext;
            _blogPostRepository = blogPostRepository;
        }
        public async Task<BlogPostView> CreateAsync(CreateBlogPostModel model, CancellationToken cancellationToken = default)
        {
            var blogPost = _mapper.Map<BlogPost>(model);
            blogPost.SeName = await GetSeName(blogPost);
            blogPost.Stores.Add(model.StoreId);
            blogPost.CreatedBy = _workContext.GetCurrentUserId();
            blogPost.CreatedOn = DateTime.UtcNow;
            var blogPostCreated = await _blogPostRepository.InsertAsync(blogPost);
            return _mapper.Map<BlogPostView>(blogPostCreated);
        }

        public async Task<BlogPostView> UpdateAsync(UpdateBlogPostModel model, CancellationToken cancellationToken = default)
        {
            var blogPost = await GetByIdAsync(model.Id);

            if (blogPost.Title != model.Title)
                blogPost.SeName = await GetSeName(blogPost);

            blogPost.Title = model.Title;
            blogPost.ShortContent = model.ShortContent;
            blogPost.FullContent = model.FullContent;
            blogPost.Type = model.Type;
            blogPost.Tags = model.Tags;
            blogPost.Publisher = model.Publisher;
            blogPost.Published = model.Published;
            blogPost.MetaKeywords = model.MetaKeywords;
            blogPost.MetaDescription = model.MetaDescription;
            blogPost.MetaTitle = model.MetaTitle;
            blogPost.UpdatedOn = DateTime.UtcNow;
            blogPost.UpdatedBy = _workContext.GetCurrentUserId();
            var blogPostUpdated = await _blogPostRepository.UpdateAsync(blogPost);
            return _mapper.Map<BlogPostView>(blogPostUpdated);
        }


        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _blogPostRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            return await _blogPostRepository.DeleteManyAsync(ids);
        }

        public async Task<PageableView<BlogPostView>> FindAsync(BlogPostQueryModel model, CancellationToken cancellationToken = default)
        {
            var blogPosts = await GetAllAsync(model);
            return new PageableView<BlogPostView>
                (
                    model.PageIndex,
                    model.PageSize,
                    blogPosts.Total,
                    _mapper.Map<IEnumerable<BlogPostView>>(blogPosts.Data).ToList()
                );
        }

        public async Task<BlogPostDetailsView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<BlogPostDetailsView>(await GetByIdAsync(id));

        public async Task<BlogPostDetailsView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
            => _mapper.Map<BlogPostDetailsView>(await _blogPostRepository.FindOneAsync(x => x.SeName == slug));

        public async Task<BlogPostView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default)
        {
            var blogPost = await GetByIdAsync(id);
            blogPost.Image = _mapper.Map<Image>(model);
            return _mapper.Map<BlogPostView>(await _blogPostRepository.UpdateAsync(blogPost));
        }

        public async Task<BlogPostView> DeleteImage(string id, CancellationToken cancellationToken = default)
        {
            var blogPost = await GetByIdAsync(id);
            blogPost.Image = null;
            return _mapper.Map<BlogPostView>(await _blogPostRepository.UpdateAsync(blogPost));
        }

        #region Private Methods
        private async Task<DataFilterPagingResult<BlogPost>> GetAllAsync(BlogPostQueryModel model)
        {
            Expression<Func<BlogPost, bool>> filter = null;
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

            var query = new DataFilterPaging<BlogPost> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            return await _blogPostRepository.CountAndQueryAsync(query);
        }
        private async Task<BlogPost> GetByIdAsync(string id)
            => await _blogPostRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

        private async Task<string> GetSeName(BlogPost blogPost)
        {
            if (!string.IsNullOrEmpty(blogPost.SeName) && !await _blogPostRepository.AnyAsync(x => x.SeName == blogPost.SeName))
                return blogPost.SeName;

            blogPost.SeName = SlugHelper.GenerateSlug(blogPost.Title);

            int i = 2;
            var tempSeName = blogPost.SeName;
            while (true)
            {
                if (await _blogPostRepository.AnyAsync(x => x.SeName == tempSeName))
                {
                    tempSeName = string.Format("{0}-{1}", blogPost.SeName, i);
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
