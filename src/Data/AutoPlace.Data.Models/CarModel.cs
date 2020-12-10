namespace AutoPlace.Data.Models
{
    using AutoPlace.Data.Common.Models;

    public class CarModel : BaseDeletableModel<int>, IItemEntity
    {
        public string Name { get; set; }

        public int ManufacturerId { get; set; }

        public virtual CarManufacturer Manufacturer { get; set; }
    }
}
