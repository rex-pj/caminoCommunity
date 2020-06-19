using System;

namespace Coco.Entities.Domain
{
    public abstract class BaseEntity : BaseEntity<long>
    {
    }

    public abstract class BaseEntity<TKey> where TKey : IComparable
    {
        public virtual TKey Id { get; set; }
    }
}
