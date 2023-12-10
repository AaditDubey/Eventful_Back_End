using AspNetCoreExtensions.Exceptions;
using Time.Commerce.Application.Constants;
using Time.Commerce.Application.Services.Identity;
using Time.Commerce.Contracts.Models.Identity;
using Time.Commerce.Contracts.Views.Identity;
using Time.Commerce.Core.Constants;
using Time.Commerce.Core.Helpers;
using Time.Commerce.Domains.Entities.Identity;
using Time.Commerce.Domains.Repositories.Identity;

namespace Time.Commerce.Infras.Services.Identity
{
    public class AccountService : IAccountService
    {
        #region Fields
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor
        public AccountService(IUserRepository userRepository, IUserService userService, IStoreService storeService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _storeService = storeService;
        }
        #endregion
        public async Task<UserView> Register(RegisterModel model, CancellationToken cancellationToken = default)
        {
            var existUser = await CheckEmailExist(model.Email, cancellationToken);
            if (existUser)
                throw new BadRequestException(nameof(CommonErrors.ACCOUNT_EXISTED), CommonErrors.ACCOUNT_EXISTED);

            SecurityHelpers.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                PhonePrefix = model.PhonePrefix,
                PhoneNumber = model.PhoneNumber,
                Avatar = model.Avatar,
                IsSystemAccount = false,
                Active = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            var role = new Role { Name = CoreSystemConst.CUSTOMER_ROLE };
            user.Roles.Add(role);
            await _userRepository.InsertAsync(user);
            var store = await _storeService.GetByStoreIdAsync(model.StoreId);
            return await _userService.AddStoreAsync(user, store?.StoreId, cancellationToken);
        }

        public async Task<UserView> RegisterStore(RegisterStoreModel model, CancellationToken cancellationToken = default)
        {
            await _storeService.CreateAsync(new CreateStoreModel { StoreId = model.StoreId, Name = model.StoreId });
            var user = await Register(model, cancellationToken);
            await _storeService.InstallStoreAsync(model, cancellationToken);
            return user;
        }

        public async Task<bool> CheckEmailExist(string email, CancellationToken cancellationToken = default)
            => await _userRepository.AnyAsync(x => x.Email == email);

        public async Task<bool> ChangePassword(ChangePasswordModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindOneAsync(x => x.Id == model.UserId);
            if (user == null)
                throw new BadRequestException(nameof(CommonErrors.ACCOUNT_NOT_FOUND), CommonErrors.ACCOUNT_NOT_FOUND);

            if(!SecurityHelpers.VerifyPassword(model.OldPassword, user.PasswordHash, user.PasswordSalt))
                throw new BadRequestException(nameof(CommonErrors.INVALID_PASSWORD), CommonErrors.INVALID_PASSWORD);

            SecurityHelpers.CreatePasswordHash(model.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _userRepository.UpdateAsync(user);

            return true;
        }
    }
}
