using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using DocumentFormat.OpenXml.VariantTypes;
using MongoDB.Driver;
using System.Linq.Expressions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Identity
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public UserService(IMapper mapper, IUserRepository userRepository, IRoleRepository roleRepository, IStoreRepository storeRepository, IWorkContext workContext)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _storeRepository = storeRepository;
            _workContext = workContext;
        }
        #endregion

        #region Methods
        public async Task<UserView> CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<User>(model);
            var existUser = await _userRepository.FindOneAsync(x => x.UserName == model.Email);
            if (existUser != null)
                throw new BadRequestException(nameof(CommonErrors.ACCOUNT_EXISTED), CommonErrors.ACCOUNT_EXISTED);
            SecurityHelpers.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.UserName = model.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.CreatedOn = DateTime.UtcNow;
            user.CreatedBy = _workContext.GetCurrentUserId();

            if (model.Roles is not null)
                foreach(var roleId in model.Roles)
                {
                    var role = await _roleRepository.GetByIdAsync(roleId);
                    if (role is not null)
                        user.Roles.Add(role);
                }

            user.UserStoreMapping.Add(new UserStoreMapping
            {
                StoreId = _workContext.GetCurrentStoreId(),
                StoreUrl = _workContext.GetCurrentStoreId()
            });

            await _userRepository.InsertAsync(user);
            return _mapper.Map<UserView>(user);
        }

        public async Task<UserView> GetUserByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<UserView>(await GetUserByIdAsync(id));

        public async Task<DataFilterPagingResult<User>> GetListAsync(DataFilterPaging<User> model, CancellationToken cancellationToken = default)
        {
            return await _userRepository.CountAndQueryAsync(model);
        }

        public async Task<PageableView<UserView>> FindAsync(UserQueryModel model, CancellationToken cancellationToken = default)
        {
            Expression<Func<User, bool>> filter = null;

            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Email.ToLower().Contains(searchText) || x.FullName.ToLower().Contains(searchText)
               || x.PhoneNumber.ToLower().Contains(searchText)
               || x.FullName.ToLower().Contains(searchText);

            if(model.Active.HasValue)
                filter = filter.And(x => x.Active == model.Active);

            if (!string.IsNullOrEmpty(model.StoreId))
                filter = filter.And(p => p.UserStoreMapping.Any(x => x.StoreUrl == model.StoreId));

            var query = new DataFilterPaging<User> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if(!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            var users = await _userRepository.CountAndQueryAsync(query);
            return new PageableView<UserView>
                (
                    model.PageIndex,
                    model.PageSize,
                    users.Total,
                    _mapper.Map<IEnumerable<UserView>>(users.Data).ToList()
                );
        }

        public async Task<UserView> UpdateUserAsync(UpdateUserModel model, CancellationToken cancellationToken = default)
        {
            var user = await GetUserByIdAsync(model.Id);
            user.FullName = model.FullName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Language = model.Language;
            user.PhonePrefix = model.PhonePrefix;
            user.PhoneNumber = model.PhoneNumber;
            user.Avatar = model.Avatar;
            user.Active = model.Active;
            user.VendorId = model.VendorId;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = _workContext.GetCurrentUserId();

            user.Roles.Clear();
            if (model.Roles is not null)
                foreach (var roleId in model.Roles)
                {
                    var role = await _roleRepository.GetByIdAsync(roleId);
                    if (role is not null)
                        user.Roles.Add(role);
                }

            var userUpdated = await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserView>(userUpdated);
        }

        public async Task<bool> DeleteUserAsync(string id, CancellationToken cancellationToken = default)
            => await _userRepository.DeleteAsync(id);

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            return await _userRepository.DeleteManyAsync(ids);
        }

        public async Task<UserView> AddStoreAsync(User user, string storeId, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

            if (string.IsNullOrEmpty(storeId))
                return _mapper.Map<UserView>(user);

            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
                throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

            user.UserStoreMapping.Add(new UserStoreMapping
            {
                StoreId = storeId, 
                UserId = user.Id,
                StoreUrl = store.Url
            });
            var userUpdated = await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserView>(userUpdated);
        }
        #endregion

        #region Private Methods
        private async Task<User> GetUserByIdAsync(string id)
            =>  await _userRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.USER_NOT_FOUND), CommonErrors.USER_NOT_FOUND);
        #endregion
    }
}
