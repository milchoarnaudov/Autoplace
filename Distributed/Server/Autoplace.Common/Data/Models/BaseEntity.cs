using System.ComponentModel.DataAnnotations;

namespace Autoplace.Common.Data.Models
{
    public abstract class BaseEntity<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
