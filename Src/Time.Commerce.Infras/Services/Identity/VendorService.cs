using AspNetCoreExtensions.Exceptions;
using AutoMapper;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Numerics;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Helpers;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Common;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Catalog;
using Time.Commerce.Contracts.Views.Common;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Core.Utilities;
using Time.Commerce.Domains.Entities.Common;
using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;
using TimeCommerce.Core.MongoDB.Models;

namespace Time.Commerce.Infras.Services.Identity
{
    public class VendorService : IVendorService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public VendorService(IMapper mapper, IVendorRepository vendorRepository, IRoleRepository roleRepository, IStoreRepository storeRepository, IWorkContext workContext)
        {
            _mapper = mapper;
            _vendorRepository = vendorRepository;
            _roleRepository = roleRepository;
            _storeRepository = storeRepository;
            _workContext = workContext;
        }
        #endregion

        #region Methods
        public async Task<VendorView> CreateVendorAsync(CreateVendorModel model, CancellationToken cancellationToken = default)
        {
            var existVendor = await _vendorRepository.FindOneAsync(x => x.Email == model.Email);
            if (existVendor != null)
                throw new BadRequestException(nameof(CommonErrors.ACCOUNT_EXISTED), CommonErrors.ACCOUNT_EXISTED);

            var vendor = _mapper.Map<Vendor>(model);
            vendor.CreatedOn = DateTime.UtcNow;
            vendor.CreatedBy = _workContext.GetCurrentUserId();

            await _vendorRepository.InsertAsync(vendor);
            return _mapper.Map<VendorView>(vendor);
        }

        public async Task<VendorView> GetVendorByIdAsync(string id, CancellationToken cancellationToken = default)
            => _mapper.Map<VendorView>(await GetVendorByIdAsync(id));

        public async Task<DataFilterPagingResult<Vendor>> GetListAsync(DataFilterPaging<Vendor> model, CancellationToken cancellationToken = default)
        {
            return await _vendorRepository.CountAndQueryAsync(model);
        }

        public async Task<PageableView<VendorView>> FindAsync(VendorQueryModel model, CancellationToken cancellationToken = default)
        {
            Expression<Func<Vendor, bool>> filter = null;

            var searchText = model?.SearchText?.ToLower() ?? string.Empty;
            filter = x => x.Email.ToLower().Contains(searchText) || x.Name.ToLower().Contains(searchText);

            if(model.Active.HasValue)
                filter = filter.And(x => x.Active == model.Active);

            if (!string.IsNullOrEmpty(model.StoreId))
                filter = filter.And(p => p.StoreId != null && p.StoreId == model.StoreId);

            var query = new DataFilterPaging<Vendor> { PageNumber = model.PageIndex, PageSize = model.PageSize, Filter = filter };

            if(!string.IsNullOrEmpty(model.OrderBy))
                query.Sort = new List<SortOption> { new SortOption { Field = model.OrderBy, Ascending = model.Ascending } };

            var vendors = await _vendorRepository.CountAndQueryAsync(query);
            return new PageableView<VendorView>
                (
                    model.PageIndex,
                    model.PageSize,
                    vendors.Total,
                    _mapper.Map<IEnumerable<VendorView>>(vendors.Data).ToList()
                );
        }

        public async Task<VendorView> UpdateVendorAsync(UpdateVendorModel model, CancellationToken cancellationToken = default)
        {
            var existVendor = await _vendorRepository.FindOneAsync(x => x.Email == model.Email && x.Id != model.Id);
            if (existVendor != null)
                throw new BadRequestException(nameof(CommonErrors.ACCOUNT_EXISTED), CommonErrors.ACCOUNT_EXISTED);

            var vendor = await GetVendorByIdAsync(model.Id);

            vendor.Name = model.Name;
            vendor.AdminComment = model.AdminComment;
            vendor.SeName = model.SeName;
            vendor.Email = model.Email;
            vendor.Description = model.Description;
            vendor.MetaDescription = model.MetaDescription;
            vendor.MetaKeywords = model.MetaKeywords;
            vendor.MetaTitle = model.MetaTitle;
            vendor.AllowCustomerReviews = model.AllowCustomerReviews;
            vendor.ApprovedRatingSum = model.ApprovedRatingSum;
            vendor.NotApprovedRatingSum = model.NotApprovedRatingSum;
            vendor.Commission = model.Commission;
            vendor.Address = _mapper.Map<Address>(model.Address);
            //vendor.Address = _mapper.Map<GeoCoordinates>(model.Coordinates);



            vendor.Active = model.Active;
            vendor.UpdatedOn = DateTime.UtcNow;
            vendor.UpdatedBy = _workContext.GetCurrentUserId();

            var vendorUpdated = await _vendorRepository.UpdateAsync(vendor);
            return _mapper.Map<VendorView>(vendorUpdated);
        }

        public async Task<bool> DeleteVendorAsync(string id, CancellationToken cancellationToken = default)
            => await _vendorRepository.DeleteAsync(id);

        public async Task<bool> DeleteManyAsync(List<string> ids, CancellationToken cancellationToken = default)
        {
            return await _vendorRepository.DeleteManyAsync(ids);
        }

        public async Task<VendorView> AddImage(string id, ImageModel model, CancellationToken cancellationToken = default)
        {
            var vendor = await GetVendorByIdAsync(id);
            vendor.Image = _mapper.Map<Image>(model);
            return _mapper.Map<VendorView>(await _vendorRepository.UpdateAsync(vendor));
        }

        public async Task<VendorView> DeleteImage(string id, CancellationToken cancellationToken = default)
        {
            var vendor = await GetVendorByIdAsync(id);
            vendor.Image = null;
            return _mapper.Map<VendorView>(await _vendorRepository.UpdateAsync(vendor));
        }
        #endregion

        #region Private Methods
        private async Task<Vendor> GetVendorByIdAsync(string id)
            =>  await _vendorRepository.GetByIdAsync(id) ?? throw new BadRequestException(nameof(CommonErrors.USER_NOT_FOUND), CommonErrors.USER_NOT_FOUND);
        #endregion
    }
}
