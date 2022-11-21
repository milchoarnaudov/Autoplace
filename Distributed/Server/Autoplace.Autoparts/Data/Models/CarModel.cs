using System.ComponentModel.DataAnnotations;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Common;
using Autoplace.Common.Data.Models;

namespace Autoplace.Autoparts.Data.Models
{
    public class CarModel : BaseDeletableEntity<int>
    {
        [MaxLength(Constants.CarModelMaxLength)]
        [Required]
        public string Name { get; set; }

        public int ManufacturerId { get; set; }

        public virtual CarManufacturer Manufacturer { get; set; }
    }
}
