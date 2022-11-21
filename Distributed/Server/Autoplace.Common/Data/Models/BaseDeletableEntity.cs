namespace Autoplace.Common.Data.Models
{
    public abstract class BaseDeletableEntity<TKey> : BaseEntity<TKey>, IDeletable
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
