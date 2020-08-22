namespace Camino.Framework.Models
{
    public class BaseModel
    {
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanRead { get; set; }

        public void SetPageAuthorizationForModel(BaseModel model)
        {
            CanCreate = model.CanCreate;
            CanUpdate = model.CanUpdate;
            CanDelete = model.CanDelete;
            CanRead = model.CanRead;
        }
    }
}
