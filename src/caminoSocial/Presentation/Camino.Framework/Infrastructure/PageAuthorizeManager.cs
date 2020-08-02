using Camino.Framework.Infrastructure.Contracts;
using Camino.Framework.Models;

namespace Camino.Framework.Infrastructure
{
    public class PageAuthorizeManager : IPageAuthorizeManager
    {
        public void SetPageAuthorizationForModel(BaseViewModel target, BaseViewModel source)
        {
            target.CanCreate = source.CanCreate;
            target.CanUpdate = source.CanUpdate;
            target.CanDelete = source.CanDelete;
            target.CanRead = source.CanRead;
        }
    }
}
