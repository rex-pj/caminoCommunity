namespace Camino.Service.Projections.Identity
{
    public class UserTokenProjection
    {
        public virtual string LoginProvider { get; set; }
        public virtual string Name { get; set; }
        public virtual long UserId { get; set; }
        public virtual string Value { get; set; }
    }
}
