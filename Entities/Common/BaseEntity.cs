using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public abstract class BaseEntity<TKey> : IBaseEntity
    {
        [Key]
        public TKey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    { }
}