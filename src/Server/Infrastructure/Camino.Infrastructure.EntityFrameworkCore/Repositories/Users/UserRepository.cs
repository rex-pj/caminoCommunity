using Camino.Shared.Enums;
using Camino.Core.Contracts.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Camino.Shared.Utils;
using Camino.Core.Domains.Users;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Users
{
    public partial class UserRepository : IUserRepository, IScopedDependency
    {
        #region Fields/Properties
        private readonly IEntityRepository<User> _userRepository;
        private readonly IDbContext _dbContext;
        private readonly int _userDeletedStatus;
        private readonly int _userInactivedStatus;
        #endregion

        #region Ctor
        public UserRepository(IEntityRepository<User> userRepository, IDbContext dbContext)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _userDeletedStatus = UserStatuses.Deleted.GetCode();
            _userInactivedStatus = UserStatuses.Inactived.GetCode();
        }
        #endregion

        #region CRUD
        public async Task<long> CreateAsync(User user)
        {
            await _userRepository.InsertAsync(user);
            await _dbContext.SaveChangesAsync();

            return user.Id;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _userRepository.DeleteAsync(x => x.Id == id);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateStatusAsync(long id, long updatedById, UserStatuses status)
        {
            var existing = await _userRepository.FindAsync(x => x.Id == id);
            existing.StatusId = status.GetCode();
            existing.UpdatedById = updatedById;
            existing.UpdatedDate = DateTime.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            user.UpdatedDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
        #endregion

        #region GET
        public async Task<User> FindByEmailAsync(string email)
        {
            email = email.ToLower();

            var user = await _userRepository
                .Get(x => x.Email.Equals(email))
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            username = username.ToLower();

            var user = await _userRepository
                .Get(x => x.UserName.Equals(username))
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> FindByIdAsync(long id)
        {
            var existUser = await _userRepository
                .Get(x => x.Id.Equals(id))
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            return existUser;
        }

        public async Task<IList<User>> GetByIdsAsync(IEnumerable<long> ids)
        {
            var existUsers = await _userRepository
                .Get(x => ids.Contains(x.Id))
                .Select(x => new User()
                {
                    DisplayName = x.DisplayName,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    UserName = x.UserName,
                    Id = x.Id,
                    IsEmailConfirmed = x.IsEmailConfirmed
                })
                .ToListAsync();

            return existUsers;
        }

        public async Task<User> FindFullByIdAsync(long id, bool canGetDeleted, bool canGetInactived)
        {
            var existUser = await _userRepository
                .Get(x => x.Id.Equals(id) && (x.StatusId == _userDeletedStatus && canGetDeleted)
                        || (x.StatusId == _userInactivedStatus && canGetInactived)
                        || (x.StatusId != _userDeletedStatus && x.StatusId != _userInactivedStatus))
            .OrderByDescending(x => x.CreatedDate)
            .FirstOrDefaultAsync();

            return existUser;
        }
        #endregion
    }
}
