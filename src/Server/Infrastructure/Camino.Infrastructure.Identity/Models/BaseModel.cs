namespace Camino.Infrastructure.Identity.Models
{
    public class BaseIdentityModel
    {
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanRead { get; set; }

        public void SetPageAuthorizationForModel(BaseIdentityModel model)
        {
            CanCreate = model.CanCreate;
            CanUpdate = model.CanUpdate;
            CanDelete = model.CanDelete;
            CanRead = model.CanRead;
        }
    }
}
