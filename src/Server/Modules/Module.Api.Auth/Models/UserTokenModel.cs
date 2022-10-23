using Camino.Shared.Enums;
using System;

namespace Module.Api.Auth.Models
{
    public class UserTokenModel
    {
        public string AuthenticationToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsSucceed { get; set; }
        public AccessModes AccessMode { get; set; }

        public UserTokenModel()
        {
            IsSucceed = false;
        }

        public UserTokenModel(bool isSucceed)
        {
            IsSucceed = isSucceed;
        }
    }
}
