using Autoplace.Common.Messaging;
using MassTransit.RabbitMqTransport.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoplace.Common.Services
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

        public async Task SaveChangesAsync(params Message[] messages)
        {
            foreach (var message in messages)
            {
                this.Data.Add(message);
            }

            await this.Data.SaveChangesAsync();
        }
    }
}
