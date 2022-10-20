namespace Module.Api.Auth.ModelServices
{
    public interface IAuthenticationModelService
    {
        void AddRefreshTokenToCookie(string refreshToken);
    }
}