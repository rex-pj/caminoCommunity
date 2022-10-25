namespace Module.Auth.Api.ModelServices
{
    public interface IAuthenticationModelService
    {
        void AddRefreshTokenToCookie(string refreshToken);
    }
}