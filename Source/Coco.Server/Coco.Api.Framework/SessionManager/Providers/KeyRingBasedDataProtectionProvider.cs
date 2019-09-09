using Castle.Core.Logging;
using Coco.Api.Framework.SessionManager.Contracts;
using System;

namespace Coco.Api.Framework.SessionManager.Providers
{
    internal unsafe sealed class KeyRingBasedDataProtectionProvider : IDataProtectionProvider
    {
        private readonly IKeyRingProvider _keyRingProvider;
        private readonly ILogger _logger;

        public KeyRingBasedDataProtectionProvider(IKeyRingProvider keyRingProvider, ILoggerFactory loggerFactory)
        {
            _keyRingProvider = keyRingProvider;
            _logger = loggerFactory.CreateLogger<KeyRingBasedDataProtector>(); // note: for protector (not provider!) type
        }

        public IDataProtector CreateProtector(string purpose)
        {
            if (purpose == null)
            {
                throw new ArgumentNullException(nameof(purpose));
            }

            return new KeyRingBasedDataProtector(
                logger: _logger,
                keyRingProvider: _keyRingProvider,
                originalPurposes: null,
                newPurpose: purpose);
        }
    }
}
