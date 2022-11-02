namespace AutoPlace.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Data.Common.Models;

    public class CarType : BaseDeletableModel<int>, IItemEntity
    {
        public CarType()
        {
            this.Cars = new HashSet<Car>();
        }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
