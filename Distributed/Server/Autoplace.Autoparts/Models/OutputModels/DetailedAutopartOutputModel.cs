using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Mappings;

namespace Autoplace.Autoparts.Models.OutputModels
{
    public class DetailedAutopartOutputModel : AutopartOutputModel, IMapFrom<Autopart>
    {
        public string Description { get; set; }

        public int ViewsCount { get; set; }

        public CategoryOutputModel Category { get; set; }

        public ConditionOutputModel Condition { get; set; }

        public CarOutputModel Car { get; set; }
    }
}
