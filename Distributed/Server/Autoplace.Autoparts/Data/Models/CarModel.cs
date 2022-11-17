using System.ComponentModel.DataAnnotations;
using Autoplace.Common.Data;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Common;

namespace Autoplace.Autoparts.Data.Models
{
    public class CarModel : BaseDeletableModel<int>
    {
        [MaxLength(Constants.CarModelMaxLength)]
        [Required]
        public string Name { get; set; }

        public int ManufacturerId { get; set; }

        public virtual CarManufacturer Manufacturer { get; set; }
    }
}
