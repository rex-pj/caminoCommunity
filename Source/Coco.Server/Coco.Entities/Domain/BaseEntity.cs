namespace Coco.Entities.Domain
{
    public class BaseEntity : BaseEntity<int>
    {
    }

    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
