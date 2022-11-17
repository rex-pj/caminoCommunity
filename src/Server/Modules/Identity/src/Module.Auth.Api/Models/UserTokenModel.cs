using Camino.Shared.Enums;
using System;

namespace Module.Auth.Api.Models
{
    public class UserTokenModel
    {
        public string AuthenticationToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsSucceed { get; set; }
        public AccessModes AccessMode { get; set; }
    }
}
