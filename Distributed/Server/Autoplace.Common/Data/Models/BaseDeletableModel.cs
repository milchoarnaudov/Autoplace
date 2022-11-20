namespace Autoplace.Common.Data.Models
{
    public abstract class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletable
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
