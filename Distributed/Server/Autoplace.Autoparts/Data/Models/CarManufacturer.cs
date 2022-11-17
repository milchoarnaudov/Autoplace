using Autoplace.Common.Data;
using System.ComponentModel.DataAnnotations;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Common;

namespace Autoplace.Autoparts.Data.Models
{
    public class CarManufacturer : BaseDeletableModel<int>
    {
        public CarManufacturer()
        {
            Models = new HashSet<CarModel>();
        }

        [MaxLength(Constants.CarManufacturerMaxLength)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<CarModel> Models { get; set; }
    }
}
