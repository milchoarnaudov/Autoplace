using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Common;
using Autoplace.Common.Data.Models;

namespace Autoplace.Autoparts.Data.Models
{

    public class AutopartCategory : BaseDeletableModel<int>
    {
        public AutopartCategory()
        {
            Autoparts = new HashSet<Autopart>();
        }

        [MaxLength(Constants.AutopartCategoryMaxLength)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Autopart> Autoparts { get; set; }
    }
}
