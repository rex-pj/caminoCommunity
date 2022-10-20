using Camino.Infrastructure.Identity.Core;
using System.Threading.Tasks;

namespace Module.Api.Auth.ModelServices
{
    public interface IUserModelService
    {
        Task SendActiveEmailAsync(ApplicationUser user, string confirmationToken);
    }
}