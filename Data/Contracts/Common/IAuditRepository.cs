using Entities;

namespace Data.Repositories
{
    public interface IAuditRepository<TEntity> : IRepository<TEntity> where TEntity : AuditEntity
    { }
}