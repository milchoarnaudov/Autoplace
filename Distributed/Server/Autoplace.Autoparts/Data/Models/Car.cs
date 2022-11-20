using Autoplace.Common.Data.Models;

namespace Autoplace.Autoparts.Data.Models
{
    public class Car : BaseDeletableModel<int>
    {
        public int ModelId { get; set; }

        public virtual CarModel Model { get; set; }

        public int CarTypeId { get; set; }

        public virtual CarType CarType { get; set; }
    }
}
