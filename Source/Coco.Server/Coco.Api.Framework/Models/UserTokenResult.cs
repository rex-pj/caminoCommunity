using System;

namespace Coco.Api.Framework.Models
{
    public class UserTokenResult
    {
        public UserTokenResult()
        {
            IsSuccess = false;
        }

        public UserTokenResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public string AuthenticationToken { get; set; }
        public bool IsSuccess { get; set; }
        public UserInfoModel UserInfo { get; internal set; }
    }
}
