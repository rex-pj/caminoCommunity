using Camino.Infrastructure.Identity.Core;
using System.Threading.Tasks;

namespace Module.Auth.Api.ModelServices
{
    public interface IUserModelService
    {
        Task SendActiveEmailAsync(ApplicationUser user, string confirmationToken);
    }
}