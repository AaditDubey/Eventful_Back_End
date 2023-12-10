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
    public class RoleService : IRoleService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public RoleService(IMapper mapper, IRoleRepository roleRepository, IStoreRepository storeRepository, IWorkContext workContext)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _storeRepository = storeRepository;
            _workContext = workContext;
        }
        #endregion

        #region Methods
        public async Task<RoleView> CreateRoleAsync(CreateRoleModel model, CancellationToken cancellationToken = default)
        {
            var role = _mapper.Map<Role>(model);
            var existRole = await _roleRepository.FindOneAsync(x => x.Name == model.Name);
            if (existRole != null)
                throw new BadRequestException(nameof(CommonErrors.ROLE_EXISTED), CommonErrors.ROLE_EXISTED);
            await _roleRepository.InsertAsync(role);
            return _mapper.Map<RoleView>(role);
        }

        public async Task<RoleView> GetRoleByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<RoleView>(await GetRoleByIdAsync(id));

        public async Task<DataFilterPagingResult<Role>> GetListAsync(DataFilterPaging<Role> model, CancellationToken cancellationToken = default)
        {
            return await _roleRepository.CountAndQueryAsync(model);
        }

        public async Task<PageableView<RoleView>> FindAsync(RoleQueryModel model, CancellationToken cancellationToken = default)
        {
            Expression<Func<Role, bool>> filter = null;

            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Name.ToLower().Contains(searchText);

            if(model.Active.HasValue)
                filter = filter.And(x => x.Active == model.Active);

            var query = new DataFilterPaging<Role> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if(!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            var roles = await _roleRepository.CountAndQueryAsync(query);
            return new PageableView<RoleView>
                (
                    model.PageIndex,
                    model.PageSize,
                    roles.Total,
                    _mapper.Map<IEnumerable<RoleView>>(roles.Data).ToList()
                );
        }

        public async Task<RoleView> UpdateRoleAsync(UpdateRoleModel model, CancellationToken cancellationToken = default)
        {
            var role = await GetRoleByIdAsync(model.Id);
            role.Name = model.Name;
            role.Description = model.Description;
            role.IsSystemRole = model.IsSystemRole;
            role.SystemName = model.SystemName;
            role.EnablePasswordLifetime = model.EnablePasswordLifetime;
            role.Active = model.Active;
            role.UpdatedOn = DateTime.UtcNow;
            role.UpdatedBy = _workContext.GetCurrentUserId();
            var roleUpdated = await _roleRepository.UpdateAsync(role);
            return _mapper.Map<RoleView>(roleUpdated);
        }

        public async Task<bool> DeleteRoleAsync(string id, CancellationToken cancellationToken = default)
            => await _roleRepository.DeleteAsync(id);

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            return await _roleRepository.DeleteManyAsync(ids);
        }

        public async Task<RoleView> AddStoreAsync(string roleId, string storeId, CancellationToken cancellationToken = default)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
                throw new BadRequestException(nameof(CommonErrors.RECORD_NOT_FOUND), CommonErrors.RECORD_NOT_FOUND);

            var role = await GetRoleByIdAsync(roleId);
            var roleUpdated = await _roleRepository.UpdateAsync(role);
            return _mapper.Map<RoleView>(roleUpdated);
        }
        #endregion

        #region Private Methods
        private async Task<Role> GetRoleByIdAsync(string id)
            =>  await _roleRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.ROLE_NOT_FOUND), CommonErrors.ROLE_NOT_FOUND);
        #endregion
    }
}
