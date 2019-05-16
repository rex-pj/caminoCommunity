using System;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IAccountManager<TUser> : IDisposable where TUser : class
    {
    }
}
