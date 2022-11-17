using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Autoparts.Models.OutputModels
{
    public class BaseAutopartOutputModel : IMapFrom<Autopart>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
