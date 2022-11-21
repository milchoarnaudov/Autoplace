using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Mappings;
using System.Runtime.Serialization;

namespace Autoplace.Autoparts.Models.OutputModels
{
    public class CarOutputModel : IMapFrom<Car>
    {
        public CarModelOutputModel Model { get; set; }

        public CarTypeOutputModel CarType { get; set; }
    }
}
