namespace Camino.Core.Domains.Users
{
    public class Gender
    {
        public Gender()
        {
            UserInfos = new HashSet<UserInfo>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
