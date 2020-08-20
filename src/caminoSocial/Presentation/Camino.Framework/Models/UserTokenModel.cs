using Camino.Core.Enums;

namespace Camino.Framework.Models
{
    public class UserTokenModel
    {
        public string AuthenticationToken { get; set; }
        public bool IsSucceed { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public AccessMode AccessMode { get; set; }

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
