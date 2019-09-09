using System;
using System.Collections.Generic;
using System.Text;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IKeyRing
    {
        /// <summary>
        /// The authenticated encryptor that shall be used for new encryption operations.
        /// </summary>
        /// <remarks>
        /// Activation of the encryptor instance is deferred until first access.
        /// </remarks>
        IAuthenticatedEncryptor DefaultAuthenticatedEncryptor { get; }

        /// <summary>
        /// The id of the key associated with <see cref="DefaultAuthenticatedEncryptor"/>.
        /// </summary>
        Guid DefaultKeyId { get; }

        /// <summary>
        /// Returns an encryptor instance for the given key, or 'null' if the key with the
        /// specified id cannot be found in the keyring.
        /// </summary>
        /// <remarks>
        /// Activation of the encryptor instance is deferred until first access.
        /// </remarks>
        IAuthenticatedEncryptor GetAuthenticatedEncryptorByKeyId(Guid keyId, out bool isRevoked);
    }
}
