namespace AutoPlace.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Data.Common.Models;

    public class Autopart : BaseDeletableModel<int>
    {
        public Autopart()
        {
            this.Images = new HashSet<Image>();
        }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [MaxLength(500)]
        [Required]
        public string Description { get; set; }

        public int CountViews { get; set; }

        public int CategoryId { get; set; }

        public virtual AutopartCategory Category { get; set; }

        public int ConditionId { get; set; }

        public virtual AutopartCondition Condition { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
