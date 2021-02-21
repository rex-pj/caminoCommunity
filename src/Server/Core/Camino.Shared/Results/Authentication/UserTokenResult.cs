namespace Camino.Shared.Results.Authentication
{
    public class UserTokenResult
    {
        public virtual string LoginProvider { get; set; }
        public virtual string Name { get; set; }
        public virtual long UserId { get; set; }
        public virtual string Value { get; set; }
    }
}
