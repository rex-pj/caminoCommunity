using Coco.Api.Framework.AccountIdentity.Contracts;
using System;

namespace Coco.Api.Framework.AccountIdentity
{
    public class SillyEncryptor : ILookupProtector
    {
        private readonly ILookupProtectorKeyRing _keyRing;

        public SillyEncryptor(ILookupProtectorKeyRing keyRing) => _keyRing = keyRing;

        public string Unprotect(string keyId, string data)
        {
            var pad = _keyRing[keyId];
            if (!data.StartsWith(pad))
            {
                throw new InvalidOperationException("Didn't find pad.");
            }
            return data.Substring(pad.Length);
        }

        public string Protect(string keyId, string data)
            => _keyRing[keyId] + data;
    }
}
