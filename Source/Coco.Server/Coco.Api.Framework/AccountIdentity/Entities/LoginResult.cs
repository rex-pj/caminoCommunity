namespace Coco.Api.Framework.AccountIdentity.Entities
{
    public class LoginResult
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorCode { get; set; }

        public LoginResult(bool isSuccess = false)
        {
            IsSuccess = isSuccess;
            Token = string.Empty;
        }
    }
}
