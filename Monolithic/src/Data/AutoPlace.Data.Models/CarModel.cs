namespace AutoPlace.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Data.Common.Models;

    public class CarModel : BaseDeletableModel<int>, IItemEntity
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public int ManufacturerId { get; set; }

        public virtual CarManufacturer Manufacturer { get; set; }
    }
}
