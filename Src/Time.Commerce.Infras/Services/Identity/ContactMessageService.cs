using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using MongoDB.Driver;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Identity
{
    public class ContactMessageService : IContactMessageService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public ContactMessageService(IMapper mapper, IContactMessageRepository contactMessageRepository, IRoleRepository roleRepository, IStoreRepository storeRepository, IWorkContext workContext)
        {
            _mapper = mapper;
            _contactMessageRepository = contactMessageRepository;
            _roleRepository = roleRepository;
            _storeRepository = storeRepository;
            _workContext = workContext;
        }
        #endregion

        #region Methods
        public async Task<ContactMessageView> CreateContactMessageAsync(CreateContactMessageModel model, CancellationToken cancellationToken = default)
        {
            var contactMessage = _mapper.Map<ContactMessage>(model);
            await _contactMessageRepository.InsertAsync(contactMessage);
            return _mapper.Map<ContactMessageView>(contactMessage);
        }

        public async Task<ContactMessageView> GetContactMessageByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<ContactMessageView>(await GetContactMessageByIdAsync(id));

        public async Task<DataFilterPagingResult<ContactMessage>> GetListAsync(DataFilterPaging<ContactMessage> model, CancellationToken cancellationToken = default)
        {
            return await _contactMessageRepository.CountAndQueryAsync(model);
        }

        public async Task<PageableView<ContactMessageView>> FindAsync(ContactMessageQueryModel model, CancellationToken cancellationToken = default)
        {
            Expression<Func<ContactMessage, bool>> filter = null;

            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Email.ToLower().Contains(searchText) || x.FullName.ToLower().Contains(searchText)
               || x.PhoneNumber.ToLower().Contains(searchText)
               || x.FullName.ToLower().Contains(searchText);

            if(model.Type is not null)
            {
                filter = filter.And( c => c.Type == (Domains.Entities.Identity.ContactMessageType)model.Type );
            }

            var query = new DataFilterPaging<ContactMessage> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if(!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            var contactMessages = await _contactMessageRepository.CountAndQueryAsync(query);
            return new PageableView<ContactMessageView>
                (
                    model.PageIndex,
                    model.PageSize,
                    contactMessages.Total,
                    _mapper.Map<IEnumerable<ContactMessageView>>(contactMessages.Data).ToList()
                );
        }

        public async Task<ContactMessageView> UpdateContactMessageAsync(UpdateContactMessageModel model, CancellationToken cancellationToken = default)
        {
            var contactMessage = await GetContactMessageByIdAsync(model.Id);
            contactMessage.FullName = model.FullName;
            contactMessage.PhoneNumber = model.PhoneNumber;
            contactMessage.Email = model.PhoneNumber;
            contactMessage.Subject = model.Subject;
            contactMessage.Content = model.Content;
            contactMessage.UpdatedOn = DateTime.UtcNow;
            contactMessage.UpdatedBy = _workContext.GetCurrentUserId();
            var contactMessageUpdated = await _contactMessageRepository.UpdateAsync(contactMessage);
            return _mapper.Map<ContactMessageView>(contactMessageUpdated);
        }

        public async Task<bool> DeleteContactMessageAsync(string id, CancellationToken cancellationToken = default)
            => await _contactMessageRepository.DeleteAsync(id);

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            return await _contactMessageRepository.DeleteManyAsync(ids);
        }
        #endregion

        #region Private Methods
        private async Task<ContactMessage> GetContactMessageByIdAsync(string id)
            =>  await _contactMessageRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.USER_NOT_FOUND), CommonErrors.USER_NOT_FOUND);
        #endregion
    }
}
