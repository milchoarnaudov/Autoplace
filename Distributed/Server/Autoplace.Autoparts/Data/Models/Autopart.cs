using System.ComponentModel.DataAnnotations;
using Autoplace.Autoparts.Common;
using Autoplace.Common.Data.Models;
using Autoplace.Common.Enums;

namespace Autoplace.Autoparts.Data.Models
{

    public class Autopart : BaseDeletableEntity<string>
    {
        public Autopart()
        {
            Id = Guid.NewGuid().ToString();
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

        public AutopartStatus Status { get; set; }

        public int CategoryId { get; set; }

        public virtual AutopartCategory Category { get; set; }

        public int ConditionId { get; set; }

        public virtual AutopartCondition Condition { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        public string Username { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
