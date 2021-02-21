using Camino.Shared.Results.Identifiers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Services.Users;
using Camino.Shared.Requests.UpdateItems;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.Requests.Identifiers;
using Camino.Core.Contracts.Repositories.Users;

namespace Camino.Services.Users
{
    public partial class UserService : IUserService
    {
        #region Fields/Properties
        private readonly IUserRepository _userRepository;
        #endregion

        #region Ctor
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region CRUD
        public async Task<long> CreateAsync(UserModifyRequest request)
        {
            return await _userRepository.CreateAsync(request);
        }

        public async Task DeleteAsync(long id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> ActiveAsync(long id)
        {
            return await _userRepository.ActiveAsync(id);
        }

        public async Task<UpdateItemRequest> UpdateInfoItemAsync(UpdateItemRequest request)
        {
            return await _userRepository.UpdateInfoItemAsync(request);
        }

        public async Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(UserIdentifierUpdateRequest request)
        {
            return await _userRepository.UpdateIdentifierAsync(request);
        }

        public async Task<bool> UpdateAsync(UserModifyRequest request)
        {
            return await _userRepository.UpdateAsync(request);
        }
        #endregion

        #region GET
        public async Task<UserResult> FindByEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }

        public async Task<UserResult> FindByUsernameAsync(string username)
        {
            return await _userRepository.FindByUsernameAsync(username);
        }

        public async Task<UserResult> FindByIdAsync(long id)
        {
            return await _userRepository.FindByIdAsync(id);
        }

        public async Task<UserFullResult> FindFullByIdAsync(long id)
        {
            return await _userRepository.FindFullByIdAsync(id);
        }

        public List<UserFullResult> Search(string query = "", List<long> currentUserIds = null, int page = 1, int pageSize = 10)
        {
            return _userRepository.Search(query, currentUserIds, page, pageSize);
        }

        public async Task<BasePageList<UserFullResult>> GetAsync(UserFilter filter)
        {
            return await _userRepository.GetAsync(filter);
        }
        #endregion
    }
}
