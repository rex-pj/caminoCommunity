using Coco.Common.Enums;

namespace Coco.Framework.Models
{
    public class UserTokenResult
    {
        public string AuthenticationToken { get; set; }
        public bool IsSucceed { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public AccessMode AccessMode { get; set; }

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
