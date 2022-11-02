namespace AutoPlace.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Data.Common.Models;

    public class AutopartCategory : BaseDeletableModel<int>, IItemEntity
    {
        public AutopartCategory()
        {
            this.Autoparts = new HashSet<Autopart>();
        }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Autopart> Autoparts { get; set; }
    }
}
