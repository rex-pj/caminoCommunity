using Camino.Framework.Models;

namespace Camino.Framework.Infrastructure.Contracts
{
    public interface IPageAuthorizeManager
    {
        void SetPageAuthorizationForModel(BaseViewModel target, BaseViewModel source);
    }
}
