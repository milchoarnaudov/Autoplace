using System.ComponentModel.DataAnnotations;
using Autoplace.Autoparts.Common;
using Autoplace.Common.Data.Models;

namespace Autoplace.Autoparts.Data.Models
{
    public class CarType : BaseDeletableModel<int>
    {
        public CarType()
        {
            Cars = new HashSet<Car>();
        }

        [MaxLength(Constants.CarTypeMaxLength)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
