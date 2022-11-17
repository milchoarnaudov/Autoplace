using System.ComponentModel.DataAnnotations;
using Autoplace.Common.Data;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Common;

namespace Autoplace.Autoparts.Data.Models
{

    public class AutopartCondition : BaseDeletableModel<int>
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
