using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace Coco.Api.Framework.Commons.Options
{
    public class TextHasherOptions : PasswordHasherOptions
    {
        private static readonly RandomNumberGenerator _defaultRng = RandomNumberGenerator.Create(); // secure PRNG

        private PasswordHasherCompatibilityMode _compatibilityMode;
        public new PasswordHasherCompatibilityMode CompatibilityMode
        {
            get
            {
                _compatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
                return _compatibilityMode;
            }
            set
            {
                _compatibilityMode = value;
            }
        }

        private int _iterationCount;
        public new int IterationCount
        {
            get
            {
                _iterationCount = 20397;
                return _iterationCount;
            }
            set
            {
                _iterationCount = value;
            }
        }

        // for unit testing
        internal RandomNumberGenerator Rng { get; set; } = _defaultRng;
    }
}
