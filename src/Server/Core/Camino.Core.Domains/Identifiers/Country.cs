using Camino.Core.Domains.Users;
using System.ComponentModel.DataAnnotations;

namespace Camino.Core.Domains.Identifiers
{
    public class Country
    {
        public Country()
        {
            this.Users = new HashSet<User>();
        }

        public short Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
