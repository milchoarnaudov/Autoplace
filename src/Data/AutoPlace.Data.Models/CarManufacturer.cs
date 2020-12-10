namespace AutoPlace.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Data.Common.Models;

    public class CarManufacturer : BaseDeletableModel<int>, IItemEntity
    {
        public CarManufacturer()
        {
            this.Models = new HashSet<CarModel>();
        }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<CarModel> Models { get; set; }
    }
}
