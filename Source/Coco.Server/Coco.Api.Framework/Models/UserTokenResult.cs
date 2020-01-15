namespace Coco.Api.Framework.Models
{
    public class UserTokenResult
    {
        public UserTokenResult()
        {
            IsSucceed = false;
        }

        public UserTokenResult(bool isSucceed)
        {
            IsSucceed = isSucceed;
        }

        public string AuthenticationToken { get; set; }
        public bool IsSucceed { get; set; }
        public UserInfoModel UserInfo { get; internal set; }
    }
}
