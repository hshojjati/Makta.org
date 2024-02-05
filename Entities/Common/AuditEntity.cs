using System;

namespace Entities
{
    public abstract class AuditEntity<TKey> : BaseEntity<TKey>
    {
        public DateTime InsertDateTime { get; set; }
        public string InsertUserId { get; set; }

        public virtual ApplicationUser InsertUser { get; set; }
    }

    public abstract class AuditEntity : AuditEntity<int>
    { }
}