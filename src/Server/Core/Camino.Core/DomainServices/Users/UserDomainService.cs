using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Users.DomainServices;
using Camino.Shared.Enums;
using Camino.Shared.Utils;

namespace Camino.Core.DomainServices.Products.Users
{
    public class UserDomainService: IUserDomainService, IScopedDependency
    {
        private readonly IUserRepository _userRepository;

        public UserDomainService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ConfirmAsync(long id, long updatedById)
        {
            var existing = await _userRepository.FindByIdAsync(id);
            existing.StatusId = UserStatuses.Actived.GetCode();
            existing.IsEmailConfirmed = true;
            existing.UpdatedById = updatedById;

            return await _userRepository.UpdateAsync(existing);
        }
    }
}
