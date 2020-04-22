using Coco.Framework.Commons.Enums;

namespace Coco.Framework.Models
{
    public class UserTokenResult
    {
        public string AuthenticationToken { get; set; }
        public bool IsSucceed { get; set; }
        public UserInfoModel UserInfo { get; internal set; }
        public AccessModeEnum AccessMode { get; set; }

        public UserTokenResult()
        {
            IsSucceed = false;
        }

        public UserTokenResult(bool isSucceed)
        {
            IsSucceed = isSucceed;
        }
    }
}
