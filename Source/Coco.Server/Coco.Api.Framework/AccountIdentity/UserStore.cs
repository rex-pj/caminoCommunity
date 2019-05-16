using Coco.Api.Framework.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;

namespace Coco.Api.Framework.AccountIdentity
{
    public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    //:
    //IUserStore<ApplicationUser>,
    //IUserPasswordStore<ApplicationUser>,
    //IUserEmailStore<ApplicationUser>
    {
        //private readonly IAccountBusiness _accountBusiness;

        //public ApplicationUserStore(IAccountBusiness accountBusiness)
        //{
        //    _accountBusiness = accountBusiness;
        //}

        //#region IUserStore<LoggedUser> Members
        //public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        if (cancellationToken != null)
        //        {
        //            cancellationToken.ThrowIfCancellationRequested();
        //        }

        //        if (user == null)
        //        {
        //            throw new ArgumentNullException(nameof(user));
        //        }

        //        var userModel = GetUserEntity(user);

        //        _accountBusiness.Add(userModel);

        //        return Task.FromResult(IdentityResult.Success);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        //    }
        //}

        //public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        if (cancellationToken != null)
        //        {
        //            cancellationToken.ThrowIfCancellationRequested();
        //        }

        //        if (user == null)
        //        {
        //            throw new ArgumentNullException(nameof(user));
        //        }

        //        _accountBusiness.Delete(user.Id);

        //        return Task.FromResult(IdentityResult.Success);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        //    }
        //}

        ///// Todo: Re-Comments
        //public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (string.IsNullOrWhiteSpace(userId))
        //    {
        //        throw new ArgumentNullException(nameof(userId));
        //    }

        //    if (!long.TryParse(userId, out long id))
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(userId), $"{nameof(userId)} is not a valid GUID");
        //    }

        //    var userEntity = await _accountBusiness.Find(id);

        //    return await Task.FromResult(GetLoggedUser(userEntity));
        //}

        //public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    var userEntity = await _accountBusiness.FindUserByUsername(normalizedUserName.ToLower(), true);

        //    return await Task.FromResult(GetLoggedUser(userEntity));
        //}

        //public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.NormalizedUserName);
        //}

        //public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.Id.ToString());
        //}

        //public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.UserName);
        //}

        //public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.NormalizedUserName = normalizedName;

        //    return Task.CompletedTask;
        //}

        //public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.UserName = userName;

        //    return Task.CompletedTask;
        //}

        //public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        if (cancellationToken != null)
        //        {
        //            cancellationToken.ThrowIfCancellationRequested();
        //        }

        //        if (user == null)
        //        {
        //            throw new ArgumentNullException(nameof(user));
        //        }

        //        var userModel = GetUserEntity(user);

        //        _accountBusiness.Update(userModel);

        //        return Task.FromResult(IdentityResult.Success);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        //    }
        //}

        //#endregion

        //#region IUserPasswordStore<LoggedUser> Members
        //public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.PasswordHash = passwordHash;

        //    return Task.CompletedTask;
        //}

        //public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.PasswordHash);
        //}

        //public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        //}
        //#endregion

        //#region IUserEmailStore<ApplicationUser> Members
        //public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.Email = email;

        //    return Task.CompletedTask;
        //}

        //public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.Email);
        //}

        //public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.EmailConfirmed);
        //}

        //public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.EmailConfirmed = confirmed;

        //    return Task.CompletedTask;
        //}

        //public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        //{
        //    if (string.IsNullOrWhiteSpace(normalizedEmail))
        //    {
        //        throw new ArgumentNullException(nameof(normalizedEmail));
        //    }

        //    var userEntity = await _accountBusiness.FindUserByEmail(normalizedEmail, true);

        //    return await Task.FromResult(GetApplicationUser(userEntity));
        //}

        //public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.NormalizedEmail);
        //}

        //public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.NormalizedEmail = normalizedEmail;

        //    return Task.CompletedTask;
        //}
        //#endregion

        //#region Private Methods
        //private UserModel GetUserEntity(ApplicationUser LoggedUser)
        //{
        //    if (LoggedUser == null)
        //    {
        //        return null;
        //    }

        //    var result = PopulateUserEntity(LoggedUser);

        //    return result;
        //}

        //private ApplicationUser GetApplicationUser(UserModel entity)
        //{
        //    if (entity == null)
        //    {
        //        return null;
        //    }

        //    var result = PopulateLoggedUser(entity);

        //    return result;
        //}

        //private UserModel PopulateUserEntity(ApplicationUser LoggedUser)
        //{
        //    UserModel userModel = new UserModel()
        //    {
        //        Email = LoggedUser.Email,
        //        Id = LoggedUser.Id,
        //        Address = LoggedUser.Address,
        //        BirthDate = LoggedUser.BirthDate,
        //        CountryId = LoggedUser.CountryId,
        //        CreatedById = LoggedUser.CreatedById,
        //        Description = LoggedUser.Description,
        //        DisplayName = LoggedUser.DisplayName,
        //        Firstname = LoggedUser.Firstname,
        //        Lastname = LoggedUser.Lastname,
        //        GenderId = LoggedUser.GenderId,
        //        IsActived = LoggedUser.IsActived,
        //        Password = LoggedUser.PasswordHash,
        //        PasswordSalt = LoggedUser.PasswordSalt,
        //        PhoneNumber = LoggedUser.PhoneNumber,
        //        StatusId = LoggedUser.StatusId,
        //        UpdatedById = LoggedUser.UpdatedById
        //    };

        //    return userModel;
        //}

        //private ApplicationUser GetLoggedUser(UserModel entity)
        //{
        //    if (entity == null)
        //    {
        //        return null;
        //    }

        //    var result = PopulateLoggedUser(entity);

        //    return result;
        //}

        //private ApplicationUser PopulateLoggedUser(UserModel userModel)
        //{
        //    ApplicationUser applicationUser = new ApplicationUser()
        //    {
        //        Email = userModel.Email,
        //        Id = userModel.Id,
        //        UserName = userModel.Email,
        //        Lastname = userModel.Lastname,
        //        Firstname = userModel.Firstname,
        //        Password = userModel.Password,
        //        PasswordHash = userModel.Password,
        //        PasswordSalt = userModel.PasswordSalt,
        //        PhoneNumber = userModel.PhoneNumber,
        //        NormalizedUserName = userModel.Lastname + " " + userModel.Firstname,
        //        Address = userModel.Address,
        //        BirthDate = userModel.BirthDate,
        //        CountryId = userModel.CountryId,
        //        CreatedById = userModel.CreatedById,
        //        Description = userModel.Description,
        //        DisplayName = userModel.DisplayName,
        //        GenderId = userModel.GenderId,
        //        IsActived = userModel.IsActived,
        //        StatusId = userModel.StatusId,
        //        UpdatedById = userModel.UpdatedById,
        //    };

        //    return applicationUser;
        //}
        //#endregion

        //public void Dispose()
        //{

        //}
    }
}
