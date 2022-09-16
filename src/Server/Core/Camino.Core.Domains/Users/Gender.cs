namespace Camino.Core.Domains.Users
{
    public class Gender
    {
        public Gender()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
