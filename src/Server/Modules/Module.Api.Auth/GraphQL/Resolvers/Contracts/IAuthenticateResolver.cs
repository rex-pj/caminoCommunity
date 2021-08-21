using Camino.Framework.Models;
using Module.Api.Auth.Models;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IAuthenticateResolver
    {
        Task<UserTokenModel> LoginAsync(LoginModel criterias);
        Task<CommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias);
        Task<CommonResult> ResetPasswordAsync(ResetPasswordModel criterias);
    }
}
