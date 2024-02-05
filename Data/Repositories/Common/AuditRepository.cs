using Common;
using Common.Utilities;
using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class AuditRepository<TEntity> : Repository<TEntity>, IAuditRepository<TEntity> where TEntity : AuditEntity
    {
        private readonly string CurrentUserId;
        private readonly CommonSetting _commonSettings;

        public AuditRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor,
            CommonSetting commonSetting) : base(dbContext)
        {
            CurrentUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _commonSettings = commonSetting;
        }

        #region Add
        public override void Add(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Audit(entity);
            Entities.Add(entity);

            if (saveNow)
            {
                DbContext.SaveChanges();
            }
        }

        public override async Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Audit(entity);
            await Entities.AddAsync(entity, cancellationToken);

            if (saveNow)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public override void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Audit(entities);
            Entities.AddRange(entities);

            if (saveNow)
            {
                DbContext.SaveChanges();
            }
        }

        public override async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Audit(entities);
            await Entities.AddRangeAsync(entities, cancellationToken);

            if (saveNow)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }
        #endregion

        #region Methods
        public void Audit(TEntity entity)
        {
            entity.InsertUserId = CurrentUserId;
            entity.InsertDateTime = DateTime.UtcNow;
        }

        public void Audit(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Audit(entity);
            }
        }
        #endregion
    }
}
