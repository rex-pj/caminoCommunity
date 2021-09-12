namespace Camino.IdentityManager.Contracts.Core
{
    public static class IdentitySettings
    {
        public const string APP_SESSION_SCHEMA = "Camino.Application.SessionInfo";
        public const string APP_COOKIE_SCHEMA = "Camino.Application.Cookie";
        public const string RESET_PASSWORD_PURPOSE = "ResetPasswordByEmail";
        public const string AUTHENTICATION_TOKEN_PURPOSE = "AuthenticatorToken";
        public const string AUTHENTICATION_REFRESH_TOKEN_PURPOSE = "AuthenticatorRefreshToken";
    }
}
