using Autoplace.Common.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Autoplace.Common.Services.Data
{
    public abstract class BaseDataService<TEntity>
        where TEntity : class
    {
        public BaseDataService(DbContext dbContext)
        {
            Data = dbContext;
        }

        public DbContext Data { get; }

        public virtual IQueryable<TEntity> GetAllRecords() => Data.Set<TEntity>();

        public virtual void RemoveRecord(TEntity entity) => Data.Set<TEntity>().Remove(entity);
    }
}
