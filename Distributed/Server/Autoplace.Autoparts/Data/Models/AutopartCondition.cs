using System.ComponentModel.DataAnnotations;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Common;
using Autoplace.Common.Data.Models;

namespace Autoplace.Autoparts.Data.Models
{

    public class AutopartCondition : BaseDeletableEntity<int>
    {
        public AutopartCondition()
        {
            Autoparts = new HashSet<Autopart>();
        }

        [MaxLength(Constants.AutopartConditionMaxLength)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Autopart> Autoparts { get; set; }
    }
}
