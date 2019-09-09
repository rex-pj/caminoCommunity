using System;
using System.Collections.Generic;
using System.Text;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IKeyRingProvider
    {
        IKeyRing GetCurrentKeyRing();
    }
}
