using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Autoparts.Models.OutputModels
{
    public class CarManufacturerOutputModel : IMapFrom<CarManufacturer>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
