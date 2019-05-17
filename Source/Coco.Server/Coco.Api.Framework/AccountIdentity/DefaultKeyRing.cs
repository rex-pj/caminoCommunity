using Coco.Api.Framework.AccountIdentity.Contracts;
using System.Collections.Generic;

namespace Coco.Api.Framework.AccountIdentity
{
    public class DefaultKeyRing : ILookupProtectorKeyRing
    {
        public static string Current = "Default";
        public string this[string keyId] => keyId;
        public string CurrentKeyId => Current;

        public IEnumerable<string> GetAllKeyIds()
        {
            return new string[] { "Default", "NewPad" };
        }
    }
}
