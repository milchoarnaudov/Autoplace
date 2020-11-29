namespace AutoPlace.Data.Models
{
    using System.Collections.Generic;

    using AutoPlace.Data.Common.Models;

    public class CarManufacturer : BaseDeletableModel<int>
    {
        public CarManufacturer()
        {
            this.Models = new HashSet<CarModel>();
        }

        public string Name { get; set; }

        public virtual ICollection<CarModel> Models { get; set; }
    }
}
