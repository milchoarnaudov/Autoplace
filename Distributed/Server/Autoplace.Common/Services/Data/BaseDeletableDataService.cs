using Autoplace.Common.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Autoplace.Common.Services.Data
{
    public abstract class BaseDeletableDataService<TEntity> : BaseDataService<TEntity>
        where TEntity : class, IDeletable
    {
        public BaseDeletableDataService(DbContext dbContext) : base(dbContext)
        {
        }

        public override IQueryable<TEntity> GetAllRecords() => Data.Set<TEntity>().Where(entity => entity.IsDeleted == false);

        public override void RemoveRecord(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
        }
    }
}
