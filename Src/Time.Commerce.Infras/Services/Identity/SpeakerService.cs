using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Common;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Identity;

public class SpeakerService : ISpeakerService
{
    private readonly IMapper _mapper;
    private readonly IWorkContext _workContext;
    private readonly ISpeakerRepository _speakerRepository;
    private readonly IDistributedCacheService _distributedCacheService;
    public SpeakerService(IMapper mapper, IWorkContext workContext, ISpeakerRepository speakerRepository, IDistributedCacheService distributedCacheService)
    {
        _mapper = mapper;
        _workContext = workContext;
        _speakerRepository = speakerRepository;
        _distributedCacheService = distributedCacheService;
    }
    public async Task<SpeakerView> CreateAsync(CreateSpeakerModel model, CancellationToken cancellationToken = default)
    {
        var speaker = _mapper.Map<Speaker>(model);
        speaker.SeName = await GetSeName(speaker);
        speaker.CreatedBy = _workContext.GetCurrentUserId();
        speaker.CreatedOn = DateTime.UtcNow;
        var speakerCreated = await _speakerRepository.InsertAsync(speaker);
        return _mapper.Map<SpeakerView>(speakerCreated);
    }

    public async Task<SpeakerView> UpdateAsync(UpdateSpeakerModel model, CancellationToken cancellationToken = default)
    {
        var speaker = await GetByIdAsync(model.Id);

        if (speaker.Name != model.Name)
            speaker.SeName = await GetSeName(speaker);

        speaker.Name = model.Name;
        speaker.Description = model.Description;
        speaker.Information = model.Information;
        speaker.ShortInformation = model.ShortInformation;
        speaker.ShowOnHomePage = model.ShowOnHomePage;
        speaker.DisplayOrder = model.DisplayOrder;
        speaker.MetaKeywords = model.MetaKeywords;
        speaker.MetaDescription = model.MetaDescription;
        speaker.MetaTitle = model.MetaTitle;
        speaker.UpdatedOn = DateTime.UtcNow;
        speaker.UpdatedBy = _workContext.GetCurrentUserId();

        speaker.GenericAttributes.Clear();
        foreach(var a in model.GenericAttributes)
            speaker.GenericAttributes.Add(new Domains.Entities.Base.GenericAttribute { Key =  a.Key, Value = a.Value });    

        var speakerUpdated = await _speakerRepository.UpdateAsync(speaker);
    
        return _mapper.Map<SpeakerView>(speakerUpdated);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _speakerRepository.DeleteAsync(id);
    }

    public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
    {
        return await _speakerRepository.DeleteManyAsync(ids);
    }

    public async Task<PageableView<SpeakerView>> FindAsync(SpeakerQueryModel model, CancellationToken cancellationToken = default)
    {
        var speakers = await FindAsync(model);
        return new PageableView<SpeakerView>
            (
                model.PageIndex,
                model.PageSize,
                speakers.Total,
                _mapper.Map<IEnumerable<SpeakerView>>(speakers.Data).ToList()
            );
    }

    public async Task<SpeakerView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        => _mapper.Map<SpeakerView>(await GetByIdAsync(id));

    public async Task<SpeakerView> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => _mapper.Map<SpeakerView>(await _speakerRepository.FindOneAsync(x => x.SeName == slug));

    public async Task<SpeakerView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default)
    {
        var speaker = await GetByIdAsync(id);
        speaker.Image = _mapper.Map<Image>(model);
        return _mapper.Map<SpeakerView>(await _speakerRepository.UpdateAsync(speaker));
    }

    public async Task<SpeakerView> DeleteImage(string id, CancellationToken cancellationToken = default)
    {
        var speaker = await GetByIdAsync(id);
        speaker.Image = null;
        return _mapper.Map<SpeakerView>(await _speakerRepository.UpdateAsync(speaker));
    }

    #region Private Methods
    private async Task<DataFilterPagingResult<Speaker>> FindAsync(SpeakerQueryModel model)
    {
        Expression<Func<Speaker, bool>> filter = null;
        var searchText = model?.SearchText?.ToLower() ?? string.Empty;
        filter = x => x.Name.ToLower().Contains(searchText) ||
            x.MetaDescription.ToLower().Contains(searchText) ||
            x.MetaKeywords.ToLower().Contains(searchText) ||
            x.MetaTitle.ToLower().Contains(searchText);


        if (model.Ids is not null && model.Ids.Any())
            filter = filter.And(x => model.Ids.Contains(x.Id));

        var query = new DataFilterPaging<Speaker> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

        if (!string.IsNullOrEmpty(model.OrderBy))
            query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };
        else
            query.Sort = new List<SortOption> { new SortOption { Field = nameof(Product.CreatedOn), Ascending = false } };
        return await _speakerRepository.CountAndQueryAsync(query);
    }
    private async Task<Speaker> GetByIdAsync(string id)
        => await _speakerRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

    private async Task<string> GetSeName(Speaker speaker)
    {
        if (!string.IsNullOrEmpty(speaker.SeName) && !await _speakerRepository.AnyAsync(x => x.SeName == speaker.SeName))
            return speaker.SeName;

        speaker.SeName = SlugHelper.GenerateSlug(speaker.Name);

        int i = 2;
        var tempSeName = speaker.SeName;
        while (true)
        {
            if (await _speakerRepository.AnyAsync(x => x.SeName == tempSeName))
            {
                tempSeName = string.Format("{0}-{1}", speaker.SeName, i);
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
