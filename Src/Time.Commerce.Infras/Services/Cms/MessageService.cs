using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Contracts.Models.Catalog;
using Time.Commerce.Contracts.Models.Cms;
using Time.Commerce.Contracts.Views.Cms;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Base;
using Time.Commerce.Domains.Entities.Cms;
using Time.Commerce.Domains.Repositories.Cms;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Cms
{
    public class MessageService : IMessageService
    {
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMapper mapper, IWorkContext workContext, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _workContext = workContext;
            _messageRepository = messageRepository;
        }
        public async Task<MessageView> CreateAsync(CreateMessageModel model, CancellationToken cancellationToken = default)
        {
            var message = _mapper.Map<Message>(model);
            //message.StoreId = model.StoreId;
            message.CreatedOn = DateTime.UtcNow;
            var messageCreated = await _messageRepository.InsertAsync(message);
            return _mapper.Map<MessageView>(messageCreated);
        }

        public async Task<MessageView> UpdateAsync(UpdateMessageModel model, CancellationToken cancellationToken = default)
        {
            var message = await GetByIdAsync(model.Id);
            message.Type = model.Type;
            message.CustomerId = model.CustomerId;
            message.FirstName = model.FirstName;
            message.LastName = model.LastName;
            message.FullName = model.FullName;
            message.Subject = model.Subject;
            message.Enquiry = model.Enquiry;
            message.Email = model.Email;
            message.VendorId = model.VendorId;

            var messageUpdated = await _messageRepository.UpdateAsync(message);
            return _mapper.Map<MessageView>(messageUpdated);
        }


        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _messageRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            return await _messageRepository.DeleteManyAsync(ids);
        }

        public async Task<PageableView<MessageView>> FindAsync(MessageQueryModel model, CancellationToken cancellationToken = default)
        {
            var messages = await GetAllAsync(model);
            return new PageableView<MessageView>
                (
                    model.PageIndex,
                    model.PageSize,
                    messages.Total,
                    _mapper.Map<IEnumerable<MessageView>>(messages.Data).ToList()
                );
        }

        public async Task<MessageView> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<MessageView>(await GetByIdAsync(id));
        #region Private Methods
        private async Task<DataFilterPagingResult<Message>> GetAllAsync(MessageQueryModel model)
        {
            Expression<Func<Message, bool>> filter = null;
            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.FirstName.ToLower().Contains(searchText) || x.LastName.ToLower().Contains(searchText) ||
                x.Email.ToLower().Contains(searchText) ||
                x.FullName.ToLower().Contains(searchText);

            if (!string.IsNullOrWhiteSpace(model?.StoreId))
                filter = filter.And(x => x.StoreId == model.StoreId);

            //if(!string.IsNullOrWhiteSpace(model.Type))
            //    filter = filter.And(x => x.Type.ToLower() == model.Type.ToLower());

            var query = new DataFilterPaging<Message> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if (!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            return await _messageRepository.CountAndQueryAsync(query);
        }
        private async Task<Message> GetByIdAsync(string id)
            => await _messageRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);
        #endregion
    }
}
