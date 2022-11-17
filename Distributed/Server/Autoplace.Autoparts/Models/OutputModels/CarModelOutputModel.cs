using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Autoparts.Models.OutputModels
{
    public class CarModelOutputModel : IMapFrom<CarModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CarManufacturerOutputModel Manufacturer { get; set; }
    }
}
