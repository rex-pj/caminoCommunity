using System;

namespace Coco.Api.Framework.Models
{
    public class LoginResult
    {
        public string AuthenticationToken { get; set; }
        public DateTime? Expiration { get; set; }
        public bool IsSuccess { get; set; }
        public UserInfo UserInfo { get; internal set; }
    }
}
