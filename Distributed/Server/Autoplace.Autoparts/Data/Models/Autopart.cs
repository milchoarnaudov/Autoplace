using Autoplace.Common.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Common;

namespace Autoplace.Autoparts.Data.Models
{

    public class Autopart : BaseDeletableModel<int>
    {
        public Autopart()
        {
            Images = new HashSet<Image>();
        }

        [MaxLength(Constants.AutopartNameMaxLength)]
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [MaxLength(Constants.AutopartDescriptionMaxLength)]
        [Required]
        public string Description { get; set; }

        public int ViewsCount { get; set; }

        public bool IsApproved { get; set; }

        public int CategoryId { get; set; }

        public virtual AutopartCategory Category { get; set; }

        public int ConditionId { get; set; }

        public virtual AutopartCondition Condition { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
