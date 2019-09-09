﻿using Castle.Core.Logging;
using Coco.Api.Framework.SessionManager.Contracts;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coco.Api.Framework.SessionManager.Providers
{
    public sealed class EphemeralDataProtectionProvider : IDataProtectionProvider
    {
        private readonly KeyRingBasedDataProtectionProvider _dataProtectionProvider;

        /// <summary>
        /// Creates an ephemeral <see cref="IDataProtectionProvider"/>.
        /// </summary>
        public EphemeralDataProtectionProvider()
            : this(NullLoggerFactory.Instance)
        { }

        /// <summary>
        /// Creates an ephemeral <see cref="IDataProtectionProvider"/> with logging.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory" />.</param>
        public EphemeralDataProtectionProvider(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            IKeyRingProvider keyringProvider;
            if (OSVersionUtil.IsWindows())
            {
                // Fastest implementation: AES-256-GCM [CNG]
                keyringProvider = new EphemeralKeyRing<CngGcmAuthenticatedEncryptorConfiguration>(loggerFactory);
            }
            else
            {
                // Slowest implementation: AES-256-CBC + HMACSHA256 [Managed]
                keyringProvider = new EphemeralKeyRing<ManagedAuthenticatedEncryptorConfiguration>(loggerFactory);
            }

            var logger = loggerFactory.CreateLogger<EphemeralDataProtectionProvider>();
            logger.UsingEphemeralDataProtectionProvider();

            _dataProtectionProvider = new KeyRingBasedDataProtectionProvider(keyringProvider, loggerFactory);
        }

        public IDataProtector CreateProtector(string purpose)
        {
            if (purpose == null)
            {
                throw new ArgumentNullException(nameof(purpose));
            }

            // just forward to the underlying provider
            return _dataProtectionProvider.CreateProtector(purpose);
        }

        private sealed class EphemeralKeyRing<T> : IKeyRing, IKeyRingProvider
            where T : AlgorithmConfiguration, new()
        {
            public EphemeralKeyRing(ILoggerFactory loggerFactory)
            {
                DefaultAuthenticatedEncryptor = GetDefaultEncryptor(loggerFactory);
            }

            // Currently hardcoded to a 512-bit KDK.
            private const int NUM_BYTES_IN_KDK = 512 / 8;

            public IAuthenticatedEncryptor DefaultAuthenticatedEncryptor { get; }

            public Guid DefaultKeyId { get; } = default(Guid);

            public IAuthenticatedEncryptor GetAuthenticatedEncryptorByKeyId(Guid keyId, out bool isRevoked)
            {
                isRevoked = false;
                return (keyId == default(Guid)) ? DefaultAuthenticatedEncryptor : null;
            }

            public IKeyRing GetCurrentKeyRing()
            {
                return this;
            }

            private static IAuthenticatedEncryptor GetDefaultEncryptor(ILoggerFactory loggerFactory)
            {
                var configuration = new T();
                if (configuration is CngGcmAuthenticatedEncryptorConfiguration)
                {
                    var descriptor = (CngGcmAuthenticatedEncryptorDescriptor)new T().CreateNewDescriptor();
                    return new CngGcmAuthenticatedEncryptorFactory(loggerFactory)
                        .CreateAuthenticatedEncryptorInstance(
                            descriptor.MasterKey,
                            configuration as CngGcmAuthenticatedEncryptorConfiguration);
                }
                else if (configuration is ManagedAuthenticatedEncryptorConfiguration)
                {
                    var descriptor = (ManagedAuthenticatedEncryptorDescriptor)new T().CreateNewDescriptor();
                    return new ManagedAuthenticatedEncryptorFactory(loggerFactory)
                        .CreateAuthenticatedEncryptorInstance(
                            descriptor.MasterKey,
                            configuration as ManagedAuthenticatedEncryptorConfiguration);
                }

                return null;
            }
        }
    }
}
