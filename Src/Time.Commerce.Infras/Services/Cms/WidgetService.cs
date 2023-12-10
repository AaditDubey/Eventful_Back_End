using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Catalog;
using Time.Commerce.Domains.Entities.Cms;
using Time.Commerce.Domains.Repositories.Cms;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Cms;

public class WidgetService : IWidgetService
{
    private readonly IMapper _mapper;
    private readonly IWorkContext _workContext;
    private readonly IWidgetRepository _widgetRepository;

    public WidgetService(IMapper mapper, IWorkContext workContext, IWidgetRepository widgetRepository)
    {
        _mapper = mapper;
        _workContext = workContext;
        _widgetRepository = widgetRepository;
    }

    public async Task<WidgetView> CreateAsync(Widget model, CancellationToken cancellationToken = default)
    {
        switch (model.Type)
        {
            case WidgetTypes.HeaderMenus:
                model.WidgetCarousels = null;
                model.WidgetFooters = null;
                model.Html = string.Empty;
                break;
            case WidgetTypes.Grid:
            case WidgetTypes.Carousel:
                model.WidgetFooters = null;
                model.WidgetMenus = null;
                model.Html = string.Empty;
                break;
            case WidgetTypes.Footer:
                model.WidgetCarousels = null;
                model.WidgetMenus = null;
                model.Html = string.Empty;
                break;
            default:
                model.WidgetCarousels = null;
                model.WidgetFooters = null;
                model.WidgetMenus = null;
                break;
        }

        model.CreatedBy = _workContext.GetCurrentUserId();
        model.CreatedOn = DateTime.UtcNow;
        return _mapper.Map<WidgetView>(await _widgetRepository.InsertAsync(model));
    }

    public async Task<WidgetView> UpdateAsync(Widget model, CancellationToken cancellationToken = default)
    {
        model.UpdatedOn = DateTime.UtcNow;
        model.UpdatedBy = _workContext.GetCurrentUserId();
        return _mapper.Map<WidgetView>(await _widgetRepository.UpdateAsync(model));
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _widgetRepository.DeleteAsync(id);
    }

    public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
    {
        return await _widgetRepository.DeleteManyAsync(ids);
    }

    public async Task<PageableView<WidgetView>> FindAsync(WidgetQueryModel model, CancellationToken cancellationToken = default)
    {
        var widgets = await GetAllAsync(model);
        return new PageableView<WidgetView>
            (
                model.PageIndex,
                model.PageSize,
                widgets.Total,
                _mapper.Map<IEnumerable<WidgetView>>(widgets.Data).ToList()
            );
    }

    public async Task<WidgetView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var widget = await GetByIdAsync(id);
        var result = _mapper.Map<WidgetView>(widget);
        switch (widget.Type)
        {
            case WidgetTypes.HeaderMenus:
                result.WidgetMenus = result.WidgetMenus?.OrderBy(x => x.DisplayOrder).ToList();
                break;
        }
        return result;
    }

    private async Task<DataFilterPagingResult<Widget>> GetAllAsync(WidgetQueryModel model)
    {
        Expression<Func<Widget, bool>> filter = null;
        var searchText = model?.SearchText?.ToLower() ?? string.Empty;
        filter = x => x.Name.ToLower().Contains(searchText);

        if (!string.IsNullOrWhiteSpace(model?.StoreId))
            filter = filter.And(x => x.StoreId == model.StoreId);

        if (model.Type != null)
            filter = filter.And(x => x.Type == (WidgetTypes)model.Type);

        var query = new DataFilterPaging<Widget> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

        if (!string.IsNullOrEmpty(model.OrderBy))
            query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

        return await _widgetRepository.CountAndQueryAsync(query);
    }

    private async Task<Widget> GetByIdAsync(string id)
        => await _widgetRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);
}
